using System.Net.WebSockets;
using GameFramework;
using GameFramework.Event;

namespace GameMain
{
    public class WebSocketCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketCloseEventArgs).GetHashCode();
        
        public override int Id => EventId;
        
        public ushort Code
        {
            get;
            private set;
        }

        public string Reason
        {
            get; 
            private set;
        }

        public static WebSocketCloseEventArgs Create(ushort code, string reason)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketCloseEventArgs>();
            eventArgs.Code = code;
            eventArgs.Reason = reason;
            return eventArgs;
        }
        
        public override void Clear()
        {
            Code = 0;
            Reason = null;
        }
    }
}