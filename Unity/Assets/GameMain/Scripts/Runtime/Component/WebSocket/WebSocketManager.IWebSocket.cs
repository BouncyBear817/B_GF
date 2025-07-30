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
        public interface IWebSocket
        {
            string Address { get; }
        
            string[] SubProtocols { get; }
        
            WebSocketState ReadyState { get; }
        
            event EventHandler<WebSocketOpenEventArgs> OnOpen;
        
            event EventHandler<WebSocketMessageEventArgs> OnMessage;
        
            event EventHandler<WebSocketCloseEventArgs> OnClose;
        
            event EventHandler<WebSocketErrorEventArgs> OnError;
        
            void ConnectAsync();
        
            void CloseAsync();
        
            void SendAsync(string message);
        
            void SendAsync(byte[] message);
        }
    }
}