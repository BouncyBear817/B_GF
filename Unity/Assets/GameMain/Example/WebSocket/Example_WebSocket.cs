using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameMain;
using UnityEngine;

public class Example_WebSocket : MonoBehaviour
{
    private string mAddress = "ws://127.0.0.1:8765";
    private string[] mSubProtocol;
    
    private string mMessage;

    private bool mIsText;
    
    private string mInfo;

    private void OnGUI()
    {
        mAddress = GUI.TextField(new Rect(20, 20, 300, 30), mAddress);
        
        if (GUI.Button(new Rect(350, 20, 100, 30), "Connect"))
        {
            MainEntry.WebSocket.Connect(mAddress);
        }

        if (GUI.Button(new Rect(500, 20, 100, 30), "Close"))
        {
            MainEntry.WebSocket.Close();
        }

        mMessage = GUI.TextArea(new Rect(20, 70, 300, 200), mMessage);

        mIsText = GUI.Toggle(new Rect(350, 70, 100, 30), mIsText, "Text");

        if (GUI.Button(new Rect(500, 70, 100, 30), "Send"))
        {
            if (mIsText)
                MainEntry.WebSocket.Send(mMessage);
            else
                MainEntry.WebSocket.Send(Encoding.UTF8.GetBytes(mMessage));
        }
        
        // if (GUI.Button(new Rect(500, 120, 100, 30), "Send Ping"))
        // {
        //     MainEntry.WebSocket.Send();
        // }
        
        GUI.Label(new Rect(420, 70, 800, 800), mInfo);
    }
}
