using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public sealed partial class WebSocketManager : IWebSocketManager
    {
        public class WebSocket : IWebSocket
        {
            private ClientWebSocket mWebSocket;
            private CancellationTokenSource mCancellationTokenSource;
            private bool mCloseProcessing;
            private ConcurrentQueue<SendBuffer> mSendQueue = new ConcurrentQueue<SendBuffer>();
            private ConcurrentQueue<GameEventArgs> mEventQueue = new ConcurrentQueue<GameEventArgs>();

            private bool mIsOpen => mWebSocket != null && mWebSocket.State == System.Net.WebSockets.WebSocketState.Open;

            public string Address { get; private set; }
            public string[] SubProtocols { get; private set; }

            public WebSocketState ReadyState
            {
                get
                {
                    if (mWebSocket == null)
                    {
                        return WebSocketState.Closed;
                    }

                    switch (mWebSocket.State)
                    {
                        case System.Net.WebSockets.WebSocketState.None:
                        case System.Net.WebSockets.WebSocketState.Aborted:
                        case System.Net.WebSockets.WebSocketState.Closed:
                            return WebSocketState.Closed;
                        case System.Net.WebSockets.WebSocketState.CloseReceived:
                        case System.Net.WebSockets.WebSocketState.CloseSent:
                            return WebSocketState.Closing;
                        case System.Net.WebSockets.WebSocketState.Connecting:
                            return WebSocketState.Connecting;
                        case System.Net.WebSockets.WebSocketState.Open:
                            return WebSocketState.Open;
                    }

                    return WebSocketState.Closed;
                }
            }

            public event EventHandler<WebSocketOpenEventArgs> OnOpen;
            public event EventHandler<WebSocketMessageEventArgs> OnMessage;
            public event EventHandler<WebSocketCloseEventArgs> OnClose;
            public event EventHandler<WebSocketErrorEventArgs> OnError;

            public WebSocket(string address, string[] subProtocols = null)
            {
                Address = address;
                SubProtocols = subProtocols;
            }

            public async void ConnectAsync()
            {
                if (mWebSocket != null)
                {
                    mEventQueue.Enqueue(WebSocketErrorEventArgs.Create("The web socket is already connected."));
                    return;
                }

                mWebSocket = new ClientWebSocket();
                mCancellationTokenSource = new CancellationTokenSource();

                if (SubProtocols != null)
                {
                    foreach (var subProtocol in SubProtocols)
                    {
                        if (!string.IsNullOrEmpty(subProtocol))
                        {
                            mWebSocket.Options.AddSubProtocol(subProtocol);
                        }
                    }
                }

                await UniTask.RunOnThreadPool(ConnectTask);
            }

            public void CloseAsync()
            {
                if (!mIsOpen)
                {
                    return;
                }

                mCloseProcessing = true;
            }

            public void SendAsync(string message)
            {
                if (!mIsOpen)
                {
                    return;
                }

                var data = Encoding.UTF8.GetBytes(message);
                var buffer = new SendBuffer(data, WebSocketMessageType.Text);
                mSendQueue.Enqueue(buffer);
            }

            public void SendAsync(byte[] message)
            {
                if (!mIsOpen)
                {
                    return;
                }

                var buffer = new SendBuffer(message, WebSocketMessageType.Binary);
                mSendQueue.Enqueue(buffer);
            }

            public void Update()
            {
                while (mEventQueue.Count > 0 && mEventQueue.TryDequeue(out var eventArgs))
                {
                    if (eventArgs is WebSocketOpenEventArgs openEventArgs)
                    {
                        OnOpen?.Invoke(this, openEventArgs);
                    }
                    else if (eventArgs is WebSocketMessageEventArgs messageEventArgs)
                    {
                        OnMessage?.Invoke(this, messageEventArgs);
                    }
                    else if (eventArgs is WebSocketErrorEventArgs errorEventArgs)
                    {
                        OnError?.Invoke(this, errorEventArgs);
                    }
                    else if (eventArgs is WebSocketCloseEventArgs closeEventArgs)
                    {
                        OnClose?.Invoke(this, closeEventArgs);
                        WebSocketDispose();
                    }

                    ReferencePool.Release(eventArgs);
                }
            }

            private async UniTask ConnectTask()
            {
                try
                {
                    var uri = new Uri(Address);
                    await mWebSocket.ConnectAsync(uri, mCancellationTokenSource.Token);
                }
                catch (Exception e)
                {
                    mEventQueue.Enqueue(WebSocketErrorEventArgs.Create(e.Message));
                    return;
                }

                mEventQueue.Enqueue(WebSocketOpenEventArgs.Create());

                SendTask();
                ReceiveTask();
            }

            private async void SendTask()
            {
                try
                {
                    while (!mCloseProcessing && mWebSocket != null && mCancellationTokenSource != null && !mCancellationTokenSource.IsCancellationRequested)
                    {
                        while (!mCloseProcessing && mSendQueue.Count > 0 && mSendQueue.TryDequeue(out var buffer))
                        {
                            await mWebSocket.SendAsync(new ArraySegment<byte>(buffer.Data), buffer.MessageType, true, mCancellationTokenSource.Token);
                        }

                        await UniTask.WaitForSeconds(3);
                    }

                    if (mCloseProcessing && mWebSocket != null && mCancellationTokenSource != null && !mCancellationTokenSource.IsCancellationRequested)
                    {
                        ClearSendQueue();

                        await mWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, mCancellationTokenSource.Token);
                    }
                }
                catch (Exception e)
                {
                    mEventQueue.Enqueue(WebSocketErrorEventArgs.Create(e.Message));
                }
                finally
                {
                    mCloseProcessing = false;
                }
            }

            private async void ReceiveTask()
            {
                var closeReason = "";
                WebSocketCloseStatus? closeStatus = WebSocketCloseStatus.Empty;
                var isClosed = false;
                var segment = new ArraySegment<byte>(new byte[8192]);
                var memoryStream = new MemoryStream();

                try
                {
                    while (!isClosed && mWebSocket != null && mCancellationTokenSource != null && !mCancellationTokenSource.IsCancellationRequested)
                    {
                        var result = await mWebSocket.ReceiveAsync(segment, mCancellationTokenSource.Token);
                        if (segment.Array == null)
                        {
                            continue;
                        }

                        memoryStream.Write(segment.Array, 0, result.Count);
                        if (!result.EndOfMessage)
                        {
                            continue;
                        }

                        var data = memoryStream.ToArray();
                        memoryStream.SetLength(0);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            isClosed = true;
                            closeStatus = result.CloseStatus;
                            closeReason = result.CloseStatusDescription;
                        }
                        else
                        {
                            mEventQueue.Enqueue(WebSocketMessageEventArgs.Create(data, result.MessageType));
                        }
                    }
                }
                catch (Exception e)
                {
                    mEventQueue.Enqueue(WebSocketErrorEventArgs.Create(e.Message));
                    closeStatus = WebSocketCloseStatus.NormalClosure;
                    closeReason = e.Message;
                }
                finally
                {
                    memoryStream.Close();
                }

                mEventQueue.Enqueue(WebSocketCloseEventArgs.Create(closeReason, closeStatus));
            }

            private void WebSocketDispose()
            {
                ClearSendQueue();
                ClearEventQueue();
                mWebSocket.Dispose();
                mWebSocket = null;
                mCancellationTokenSource.Dispose();
                mCancellationTokenSource = null;
            }

            private void ClearSendQueue()
            {
                while (mSendQueue.Count > 0 && mSendQueue.TryDequeue(out var buffer)) ;
            }

            private void ClearEventQueue()
            {
                while (mEventQueue.Count > 0 && mEventQueue.TryDequeue(out var eventArgs))
                {
                    ReferencePool.Release(eventArgs);
                }
            }
        }
    }
}