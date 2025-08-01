using System;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;
using WebSocketSharp;

namespace GameMain
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/WebSocket")]
    public class WebSocketComponent : GameFrameworkComponent
    {
        private IWebSocketManager mWebSocketManager;

        [SerializeField] private string mAddress;

        [SerializeField] private string[] mSubProtocol;

        protected override void Awake()
        {
            base.Awake();

            mWebSocketManager = new WebSocketManager();

            mWebSocketManager.OnOpen += WebSocketOnOpen;
            mWebSocketManager.OnMessage += WebSocketOnMessage;
            mWebSocketManager.OnError += WebSocketOnError;
            mWebSocketManager.OnClose += WebSocketOnClose;
        }

        public void Connect(string address, string[] subProtocols = null)
        {
            mAddress = address;
            subProtocols = subProtocols ?? Array.Empty<string>();
            mWebSocketManager.Connect(mAddress, subProtocols);
        }

        public void Send(string message)
        {
            mWebSocketManager.Send(message);
        }

        public void Send(byte[] bytes)
        {
            mWebSocketManager.Send(bytes);
        }

        public void Close()
        {
            mWebSocketManager.Close();
        }

        public void Dispose()
        {
            mWebSocketManager.Dispose();
        }

        private void WebSocketOnOpen(object sender, EventArgs e)
        {
            Log.Info($"WebSocket is opened : {mAddress}");
        }

        private void WebSocketOnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsPing)
            {
                Log.Info($"WebSocket message ping : {e.Data}");
            }
            else if (e.IsText)
            {
                Log.Info($"WebSocket message text : {e.Data}");
            }
            else if (e.IsBinary)
            {
                Log.Info($"WebSocket message binary : {Encoding.UTF8.GetString(e.RawData)}, {e.Data}");
            }
        }
        
        private void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            Log.Error($"WebSocket error : {e.Message}, exception: {e.Exception}");
        }

        private void WebSocketOnClose(object sender, CloseEventArgs e)
        {
            Log.Info($"WebSocket closed : {e.Code} , reason: {e.Reason}");
            mWebSocketManager.Dispose();
        }
    }
}