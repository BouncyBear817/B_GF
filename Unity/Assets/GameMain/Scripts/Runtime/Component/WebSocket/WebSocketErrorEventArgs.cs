using GameFramework;
using GameFramework.Event;

namespace GameMain
{
    public class WebSocketErrorEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketErrorEventArgs).GetHashCode();
        
        public override int Id => EventId;

        public string ErrorMessage
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static WebSocketErrorEventArgs Create(string errorMessage, object userData = null)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketErrorEventArgs>();
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
            return eventArgs;
        }
        
        public override void Clear()
        {
            
        }
    }
}