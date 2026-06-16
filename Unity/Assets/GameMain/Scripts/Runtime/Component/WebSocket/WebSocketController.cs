using System.Text;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class WebSocketController : MonoBehaviour
    {
        private void Start()
        {
            
            MainEntry.Event.Subscribe(WebSocketOpenEventArgs.EventId, OnWebSocketOpen);
            MainEntry.Event.Subscribe(WebSocketMessageEventArgs.EventId, OnWebSocketMessage);
            MainEntry.Event.Subscribe(WebSocketErrorEventArgs.EventId, OnWebSocketError);
            MainEntry.Event.Subscribe(WebSocketCloseEventArgs.EventId, OnWebSocketClose);
        }

        private void OnWebSocketOpen(object sender, GameEventArgs e)
        {
            if (e is WebSocketOpenEventArgs)
            {
                Log.Info("The websocket is open.");
            }
        }

        private void OnWebSocketMessage(object sender, GameEventArgs e)
        {
            if (e is WebSocketMessageEventArgs eventArgs)
            {
                var data = Encoding.UTF8.GetString(eventArgs.RawData);
                Log.Info("WebSocket Message : " + data);
                
            }
        }

        private void OnWebSocketError(object sender, GameEventArgs e)
        {
            if (e is WebSocketErrorEventArgs eventArgs)
            {
                var data = eventArgs.ErrorMessage + " ," + eventArgs.Exception.Message;
                Log.Error("WebSocket Error" + data);
            }
        }

        private void OnWebSocketClose(object sender, GameEventArgs e)
        {
            if (e is WebSocketCloseEventArgs eventArgs)
            {
                var data = eventArgs.Code + " ," + eventArgs.Reason;
                Log.Info("WebSocket Close : " + data);
            }
        }
    }
}