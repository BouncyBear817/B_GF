using System.Net.WebSockets;
using GameFramework;
using GameFramework.Event;

namespace GameMain
{
    public class WebSocketMessageEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketMessageEventArgs).GetHashCode();

        public override int Id => EventId;

        public byte[] Data
        {
            get; 
            private set;
        }

        public WebSocketMessageType MessageType
        {
            get;
            private set;
        }

        public static WebSocketMessageEventArgs Create(byte[] data, WebSocketMessageType messageType)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketMessageEventArgs>();
            eventArgs.Data = data;
            eventArgs.MessageType = messageType;
            return eventArgs;
        }

        public override void Clear()
        {
        }
    }
}