using System.Net.WebSockets;
using GameFramework;
using GameFramework.Event;

namespace GameMain
{
    public class WebSocketCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketCloseEventArgs).GetHashCode();
        
        public override int Id => EventId;

        public string Reason
        {
            get; 
            private set;
        }

        public WebSocketCloseStatus? CloseCode
        {
            get;
            private set;
        }

        public static WebSocketCloseEventArgs Create(string reason, WebSocketCloseStatus? closeCode)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketCloseEventArgs>();
            eventArgs.Reason = reason;
            eventArgs.CloseCode = closeCode;
            return eventArgs;
        }
        
        public override void Clear()
        {
            
        }
    }
}