using System;
using GameFramework;
using WebSocketSharp;

namespace GameMain
{
    /// <summary>
    /// websocket管理器接口
    /// </summary>
    public interface IWebSocketManager
    {
        /// <summary>
        /// websocket地址
        /// </summary>
        string Address { get; }
        
        string[] SubProtocols { get; }
        
        event EventHandler OnOpen;
        
        event EventHandler<MessageEventArgs> OnMessage;
        
        event EventHandler<ErrorEventArgs> OnError;
        
        event EventHandler<CloseEventArgs> OnClose;

        void Connect(string address, string[] subProtocols = null);
        
        void Send(string message);
        
        void Send(byte[] bytes);
        
        void Close();
        
        void Dispose();
    }
}