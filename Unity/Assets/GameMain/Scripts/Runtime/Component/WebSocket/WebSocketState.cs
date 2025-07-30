using System;
using System.Net.WebSockets;

namespace GameMain
{
    public enum WebSocketState : ushort
    {
        Connecting = 1,
        Open = 2,
        Closing = 3,
        Closed = 4
    }
}