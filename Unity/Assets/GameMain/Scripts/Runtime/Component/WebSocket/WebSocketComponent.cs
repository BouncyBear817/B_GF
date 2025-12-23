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
        private EventComponent mEventComponent;

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

        private void Start()
        {
            mEventComponent = GameEntry.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }
        }

        public WebSocket WebSocket => mWebSocketManager.WebSocket;

        public bool IsValid => mWebSocketManager.WebSocket != null;

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
            mEventComponent.Fire(this, WebSocketOpenEventArgs.Create());
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
            
            mEventComponent.Fire(this, WebSocketMessageEventArgs.Create(e.Data, (uint)e.Opcode, e.RawData));
        }
        
        private void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            Log.Error($"WebSocket error : {e.Message}, exception: {e.Exception}");
            mEventComponent.Fire(this, WebSocketErrorEventArgs.Create(e.Message, e.Exception));
        }

        private void WebSocketOnClose(object sender, CloseEventArgs e)
        {
            Log.Info($"WebSocket closed : {e.Code} , reason: {e.Reason}");
            mEventComponent.Fire(this, WebSocketCloseEventArgs.Create(e.Code, e.Reason));
            mWebSocketManager.Dispose();
        }
    }
}