using GameFramework;
using GameFramework.Event;

namespace GameMain
{
    public class WebSocketOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketOpenEventArgs).GetHashCode();

        public override int Id => EventId;
        
        

        public override void Clear()
        {
        }

        public static WebSocketOpenEventArgs Create()
        {
            var eventArgs = ReferencePool.Acquire<WebSocketOpenEventArgs>();
            return eventArgs;
        }
    }
}