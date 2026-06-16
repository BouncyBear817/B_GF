using System;
using GameFramework;
using WebSocketSharp;

namespace GameMain
{
    public sealed partial class WebSocketManager : IWebSocketManager
    {
        private WebSocket mWebSocket;

        public string Address { get; private set; }

        public string[] SubProtocols { get; private set; }
        
        public WebSocket WebSocket => mWebSocket;
        
        public event EventHandler OnOpen;

        public event EventHandler<MessageEventArgs> OnMessage;

        public event EventHandler<ErrorEventArgs> OnError;

        public event EventHandler<CloseEventArgs> OnClose;

        public void Connect(string address, string[] subProtocols = null)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new GameFrameworkException("Address is invalid.");
            }

            if (mWebSocket != null)
            {
                throw new GameFrameworkException("WebSocket is existed.");
            }
            
            Address = address;
            SubProtocols = subProtocols;

            mWebSocket = new WebSocket(Address, SubProtocols);
            {
                mWebSocket.NoDelay = true;
                // mWebSocket.EmitOnPing = true;

                if (OnOpen != null) mWebSocket.OnOpen += OnOpen;

                if (OnMessage != null) mWebSocket.OnMessage += OnMessage;

                if (OnError != null) mWebSocket.OnError += OnError;

                if (OnClose != null) mWebSocket.OnClose += OnClose;

                mWebSocket.Connect();
            }
        }

        public void Send(string message)
        {
            if (mWebSocket == null)
            {
                throw new GameFrameworkException("WebSocket is invalid.");
            }

            mWebSocket.Send(message);
        }

        public void Send(byte[] bytes)
        {
            if (mWebSocket == null)
            {
                throw new GameFrameworkException("WebSocket is invalid.");
            }

            mWebSocket.Send(bytes);
        }

        public void Close()
        {
            if (mWebSocket == null)
            {
                throw new GameFrameworkException("WebSocket is invalid.");
            }

            mWebSocket.Close();
        }

        public void Dispose()
        {
            Address = null;
            SubProtocols = null;

            if (mWebSocket != null)
            {
                mWebSocket.OnOpen -= OnOpen;
                mWebSocket.OnMessage -= OnMessage;
                mWebSocket.OnError -= OnError;
                mWebSocket.OnClose -= OnClose;
                mWebSocket = null;
            }
        }
    }
}