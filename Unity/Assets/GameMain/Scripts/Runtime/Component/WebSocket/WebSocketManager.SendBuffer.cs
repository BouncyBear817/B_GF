using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public sealed partial class WebSocketManager : IWebSocketManager
    {
        public class SendBuffer
        {
            public byte[] Data;
            public WebSocketMessageType MessageType;

            public SendBuffer(byte[] data, WebSocketMessageType messageType)
            {
                Data = data;
                MessageType = messageType;
            }
        }
    }
}