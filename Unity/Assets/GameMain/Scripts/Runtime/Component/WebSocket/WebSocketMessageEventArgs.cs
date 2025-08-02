using GameFramework;
using GameFramework.Event;
using WebSocketSharp;

namespace GameMain
{
    public class WebSocketMessageEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebSocketMessageEventArgs).GetHashCode();

        public override int Id => EventId;

        public string Data
        {
            get;
            private set;
        }

        public uint Opcode
        {
            get;
            private set;
        }

        public byte[] RawData
        {
            get; 
            private set;
        }

        public static WebSocketMessageEventArgs Create(string data, uint opcode, byte[] rawData)
        {
            var eventArgs = ReferencePool.Acquire<WebSocketMessageEventArgs>();
            eventArgs.Data = data;
            eventArgs.Opcode = opcode;
            eventArgs.RawData = rawData;
            return eventArgs;
        }

        public override void Clear()
        {
            Data = string.Empty;
            Opcode = 0;
            RawData = null;
        }
    }
}