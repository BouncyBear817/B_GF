using System;

namespace GameMain
{
    public interface IWebSocketManager
    {
        void Add(string address, string[] subProtocols);
        
        void Remove(string address);
    }
}