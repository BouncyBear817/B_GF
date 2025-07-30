using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, WebSocket> mWebSockets = new Dictionary<string, WebSocket>();
        
        public void Add(string address, string[] subProtocols)
        {
            if (!mWebSockets.ContainsKey(address))
            {
                mWebSockets.Add(address, new WebSocket(address, subProtocols));
            }
        }

        public void Remove(string address)
        {
            if (mWebSockets.ContainsKey(address))
            {
                mWebSockets.Remove(address);
            }
        }
    }
}