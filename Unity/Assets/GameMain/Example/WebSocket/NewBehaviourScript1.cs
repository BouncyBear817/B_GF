using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class NewBehaviourScript1 : MonoBehaviour
{
  private WebSocket _ws;
    
  void Start()
  {
    string url = "ws://localhost:8765";
    Debug.Log($"Connecting to {url}");
        
    _ws = new WebSocket(url);
        
    // 设置更详细的日志
    _ws.Log.Level = LogLevel.Trace;
        
    // 事件处理
    _ws.OnOpen += (sender, e) => Debug.Log("Connection opened");
    _ws.OnMessage += (sender, e) => 
    {
      if (e.IsBinary) Debug.Log($"Binary data received: {e.RawData.Length} bytes");
      else Debug.Log($"Message received: {e.Data}");
    };
    _ws.OnError += (sender, e) => Debug.LogError($"Error: {e.Message}");
    _ws.OnClose += (sender, e) => Debug.LogWarning($"Closed: {e.Code} {e.Reason}");
        
    // 连接
    _ws.Connect();
  }
    
  void Update()
  {
    // 按空格发送测试消息
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (_ws.IsAlive)
      {
        Debug.Log("Sending 'Hello Server'");
        _ws.Send("Hello Server");
                
        // 发送二进制测试
        byte[] binaryData = new byte[] {72, 101, 108, 108, 111}; // "Hello" in ASCII
        Debug.Log("Sending binary data");
        _ws.Send(binaryData);
      }
    }
  }
    
  void OnDestroy()
  {
    if (_ws != null && _ws.IsAlive)
    {
      _ws.Close(CloseStatusCode.Normal);
    }
  }
}
