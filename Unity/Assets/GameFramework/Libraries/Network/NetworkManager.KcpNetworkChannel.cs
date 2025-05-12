//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using kcp2k;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        /// <summary>
        /// TCP 网络频道。
        /// </summary>
        private sealed class KcpNetworkChannel : NetworkChannelBase
        {
            private const int CHANNEL_HEADER_SIZE = 1;
            private const int COOKIE_HEADER_SIZE = 4;
            private const int METADATA_SIZE = CHANNEL_HEADER_SIZE + COOKIE_HEADER_SIZE;

            private Kcp m_Kcp;
            private KcpConfig m_Config;

            private uint m_Cookie;
            private byte[] m_RawSendBytes;
            private byte[] m_RawReceiveBytes;

            private int m_ReliableMax;
            private byte[] m_KcpSendBytes;
            private byte[] m_KcpReceiveBytes;

            private Stopwatch m_Watch = new Stopwatch();

            private int ReliableMaxMessageSize_Unconstrained(int mtu, uint rcv_wnd) =>
                (mtu - Kcp.OVERHEAD - METADATA_SIZE) * ((int)rcv_wnd - 1) - 1;

            private int ReliableMaxMessageSize(int mtu, uint rcv_wnd) =>
                ReliableMaxMessageSize_Unconstrained(mtu, Math.Min(rcv_wnd, Kcp.FRG_MAX));

            /// <summary>
            /// 初始化网络频道的新实例。
            /// </summary>
            /// <param name="name">网络频道名称。</param>
            /// <param name="networkChannelHelper">网络频道辅助器。</param>
            public KcpNetworkChannel(string name, INetworkChannelHelper networkChannelHelper)
                : base(name, networkChannelHelper)
            {
                m_Config = new KcpConfig();

                m_RawSendBytes = new byte[m_Config.Mtu];
                m_RawReceiveBytes = new byte[m_Config.Mtu];

                m_ReliableMax = ReliableMaxMessageSize(m_Config.Mtu, m_Config.ReceiveWindowSize);
                m_KcpSendBytes = new byte[m_ReliableMax + CHANNEL_HEADER_SIZE];
                m_KcpReceiveBytes = new byte[m_ReliableMax + CHANNEL_HEADER_SIZE];
            }

            /// <summary>
            /// 获取网络服务类型。
            /// </summary>
            public override ServiceType ServiceType
            {
                get { return ServiceType.Udp; }
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="userData">用户自定义数据。</param>
            public override void Connect(IPAddress ipAddress, int port, object userData)
            {
                base.Connect(ipAddress, port, userData);
                m_Socket = new Socket(ipAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                if (m_Socket == null)
                {
                    string errorMessage = "Initialize network channel failure.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SocketError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                m_Socket.Blocking = false;

                m_NetworkChannelHelper.PrepareForConnecting();
                ConnectSync(ipAddress, port, userData);
            }

            public override void Update(float elapseSeconds, float realElapseSeconds)
            {
                base.Update(elapseSeconds, realElapseSeconds);

                if (m_Socket == null || m_Kcp == null)
                {
                    return;
                }

                m_Kcp.Update((uint)m_Watch.ElapsedMilliseconds);
            }

            public override void Shutdown()
            {
                base.Shutdown();
            }

            protected override bool ProcessSend()
            {
                if (base.ProcessSend())
                {
                    SendSync();
                    return true;
                }

                return false;
            }

            protected override void ProcessReceive()
            {
                base.ProcessReceive();

                if (m_Active)
                {
                    while (OnRawReceive(out var segment))
                    {
                        RawInput(segment);
                    }

                    ReceiveSync();
                }
            }

            private void Disconnect()
            {
                m_Active = false;
                Close();
            }

            private void Reset()
            {
                m_Cookie = 0;
                m_Watch.Restart();

                m_Kcp = new Kcp(0, OnRawSend);

                m_Kcp.SetNoDelay(m_Config.NoDelay ? 1u : 0u, m_Config.Interval, m_Config.FastResend, !m_Config.CongestionWindow);
                m_Kcp.SetWindowSize(m_Config.SendWindowSize, m_Config.ReceiveWindowSize);
                m_Kcp.SetMtu((uint)m_Config.Mtu - METADATA_SIZE);

                m_SentPacketCount = 0;
                m_ReceivedPacketCount = 0;

                lock (m_SendPacketPool)
                {
                    m_SendPacketPool.Clear();
                }

                m_ReceivePacketPool.Clear();

                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(true);
                }
            }

            private void ConnectSync(IPAddress ipAddress, int port, object userData)
            {
                try
                {
                    m_Socket.Connect(new IPEndPoint(ipAddress, port));
                }
                catch (Exception exception)
                {
                    if (NetworkChannelError != null)
                    {
                        SocketException socketException = exception as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.ConnectError, socketException != null ? socketException.SocketErrorCode : SocketError.Success, exception.ToString());
                        return;
                    }

                    throw;
                }

                Reset();

                if (NetworkChannelConnected != null)
                {
                    NetworkChannelConnected(this, m_Socket.Connected);
                }

                m_Active = true;
            }

            private void OnRawSend(byte[] data, int length)
            {
                try
                {
                    if (!m_Socket.Poll(0, SelectMode.SelectWrite))
                    {
                        return;
                    }

                    m_RawSendBytes[0] = (byte)KcpChannel.Reliable;
                    Utils.Encode32U(m_RawSendBytes, CHANNEL_HEADER_SIZE, m_Cookie);
                    Buffer.BlockCopy(data, 0, m_RawSendBytes, METADATA_SIZE, length);

                    var segment = new ArraySegment<byte>(m_RawSendBytes, 0, length + METADATA_SIZE);
                    if (segment.Array != null)
                    {
                        m_Socket.Send(segment.Array, segment.Offset, segment.Count, SocketFlags.None);
                    }
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        SocketException socketException = exception as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.SendError, socketException != null ? socketException.SocketErrorCode : SocketError.Success, exception.ToString());
                        return;
                    }

                    throw;
                }
            }

            private void SendSync()
            {
                try
                {
                    if (m_SendState.Stream.Length + 1 > m_KcpSendBytes.Length)
                    {
                        throw new Exception($"Failed to send reliable message of size {m_SendState.Stream.Length}, because it's larger than ReliableMaxMessageSize '{m_ReliableMax}'.");
                    }

                    m_KcpSendBytes[0] = (byte)KcpChannel.Reliable;
                    Buffer.BlockCopy(m_SendState.Stream.GetBuffer(), (int)m_SendState.Stream.Position, m_KcpSendBytes, 1, (int)m_SendState.Stream.Length);
                    var sent = m_Kcp.Send(m_KcpSendBytes, 0, 1 + (int)m_SendState.Stream.Length);
                    if (sent < 0)
                    {
                        throw new Exception($"Send failed with error '{sent}' for content with length '{m_SendState.Stream.Length}'");
                    }
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        SocketException socketException = exception as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.SendError, socketException != null ? socketException.SocketErrorCode : SocketError.Success, exception.ToString());
                        return;
                    }

                    throw;
                }

                m_SentPacketCount++;
                m_SendState.Reset();
            }

            private bool OnRawReceive(out ArraySegment<byte> segment)
            {
                segment = default;
                try
                {
                    if (!m_Socket.Poll(0, SelectMode.SelectRead))
                    {
                        return false;
                    }

                    var bytesReceived = m_Socket.Receive(m_RawReceiveBytes, 0, m_RawReceiveBytes.Length, SocketFlags.None);

                    segment = new ArraySegment<byte>(m_RawReceiveBytes, 0, bytesReceived);
                    return true;
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        SocketException socketException = exception as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.ReceiveError, socketException != null ? socketException.SocketErrorCode : SocketError.Success, exception.ToString());
                        return false;
                    }

                    throw;
                }
            }

            private void RawInput(ArraySegment<byte> segment)
            {
                if (segment.Count <= 5 || segment.Array == null)
                {
                    return;
                }

                var channel = segment.Array[segment.Offset + 0];
                Utils.Decode32U(segment.Array, segment.Offset + 1, out var messageCookie);

                if (m_Cookie == 0)
                {
                    m_Cookie = messageCookie;
                }
                else if (m_Cookie != messageCookie)
                {
                    throw new GameFrameworkException($"Dropping message with mismatching cookie '{messageCookie} expected '{m_Cookie}'");
                }

                var message = new ArraySegment<byte>(segment.Array, segment.Offset + METADATA_SIZE, segment.Count - METADATA_SIZE);

                switch (channel)
                {
                    case (byte)KcpChannel.Reliable:
                    {
                        var input = m_Kcp.Input(message.Array, message.Offset, message.Count);
                        if (input != 0)
                        {
                            throw new GameFrameworkException($"Input failed with error '{input}' for buffer with length '{message.Count - 1}'.");
                        }
                    }
                        break;
                    case (byte)KcpChannel.Unreliable:
                        break;
                }
            }

            private bool ReceiveSync(out KcpChannel header, out ArraySegment<byte> segment)
            {
                segment = default;
                header = KcpChannel.Reliable;

                var msgSize = m_Kcp.PeekSize();
                if (msgSize <= 0)
                {
                    return false;
                }

                if (msgSize > m_KcpReceiveBytes.Length)
                {
                    Disconnect();
                    throw new Exception($"Possible allocation attack for msg size '{msgSize}' > buffer '{m_KcpReceiveBytes.Length}'.Disconnect the connection.");
                }

                var received = m_Kcp.Receive(m_KcpReceiveBytes, msgSize);
                if (received < 0)
                {
                    Disconnect();
                    throw new GameFrameworkException($"Receive failed with error '{received}'. closing connection.");
                }

                var headerByte = m_KcpReceiveBytes[0];
                if (!Enum.IsDefined(typeof(KcpChannel), headerByte))
                {
                    Disconnect();
                    throw new GameFrameworkException($"Receive failed to parse header: {headerByte} is not defined in {typeof(KcpChannel)}.");
                }

                header = (KcpChannel)headerByte;
                segment = new ArraySegment<byte>(m_KcpReceiveBytes, 1, msgSize - 1);
                return true;
            }

            private void ReceiveSync()
            {
                while (ReceiveSync(out var header, out var segment))
                {
                    switch (header)
                    {
                        case KcpChannel.Reliable:
                        {
                            if (segment.Count > 0 && segment.Array != null)
                            {
                                Buffer.BlockCopy(segment.Array, segment.Offset, m_ReceiveState.Stream.GetBuffer(), 0, segment.Count);
                                m_ReceiveState.Stream.Position = 0L;

                                if (m_ReceiveState.PacketHeader != null)
                                {
                                    ProcessPacket();
                                    m_ReceivedPacketCount++;
                                }
                                else
                                {
                                    if (ProcessPacketHeader())
                                    {
                                        Buffer.BlockCopy(segment.Array, segment.Offset + 8, m_ReceiveState.Stream.GetBuffer(), 0, segment.Count - 8);
                                        ProcessPacket();
                                    }
                                }
                            }
                        }
                            break;
                    }
                }
            }
        }
    }
}