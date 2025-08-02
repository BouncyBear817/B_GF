using System;
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

        public Exception Exception
        {
            get;
            private set;
        }

        public static WebSocketErrorEventArgs Create(string errorMessage, Exception exception)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketErrorEventArgs>();
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.Exception = exception;
            return eventArgs;
        }
        
        public override void Clear()
        {
            ErrorMessage = null;
            Exception = null;
        }
    }
}