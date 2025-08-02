using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameFramework.Event;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Example_WebSocket : MonoBehaviour
{
    private string mLoginAddress = "http://shixun.ruzhoukj.com/auth/login/in";
    private string mUserName = "zhangsan";
    private string mPassword = "123456";
    
    private bool mIsLogin = false;
    private string mNickName = string.Empty;
    
    private string mAddress = "ws://websocket.ruzhoukj.com/websocket/";
    private string[] mSubProtocol;
    
    private string mMessage;
    private bool mIsText;
    
    private List<string> mInfo = new List<string>(10);
    private Vector2 mScrollPosition = Vector2.zero;
    private int mHeight = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        
        MainEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
        MainEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        
        MainEntry.Event.Subscribe(WebSocketOpenEventArgs.EventId, OnWebSocketOpen);
        MainEntry.Event.Subscribe(WebSocketMessageEventArgs.EventId, OnWebSocketMessage);
        MainEntry.Event.Subscribe(WebSocketErrorEventArgs.EventId, OnWebSocketError);
        MainEntry.Event.Subscribe(WebSocketCloseEventArgs.EventId, OnWebSocketClose);
    }

    private void OnWebRequestSuccess(object sender, GameEventArgs e)
    {
        if (e is WebRequestSuccessEventArgs eventArgs)
        {
            var response = Encoding.UTF8.GetString(eventArgs.GetWebResponseBytes());
            Log.Info($"Web Request Success: {response}");
            var loginResponse = Utility.Json.ToObject<LoginResponse>(response);
            if (loginResponse != null)
            {
                if (loginResponse.code == 200)
                {
                    mNickName = loginResponse.data.nickname;
                    mAddress += mNickName;
                    mIsLogin = true;
                }
            }
        }
    }

    private void OnWebRequestFailure(object sender, GameEventArgs e)
    {
        if (e is WebRequestFailureEventArgs eventArgs)
        {
            Log.Error(eventArgs.ErrorMessage);
        }
    }

    private void OnWebSocketOpen(object sender, GameEventArgs e)
    {
        if (e is WebSocketOpenEventArgs)
        {
            var data = "The websocket is open.";
            mInfo.Add("Open : " + data);
            mScrollPosition = new Vector2(0, mHeight);
        }
    }

    private void OnWebSocketMessage(object sender, GameEventArgs e)
    {
        if (e is WebSocketMessageEventArgs eventArgs)
        {
            var data = Encoding.UTF8.GetString(eventArgs.RawData);
            mInfo.Add("Message : " + data);
            mScrollPosition = new Vector2(0, mHeight);
        }
    }

    private void OnWebSocketError(object sender, GameEventArgs e)
    {
        if (e is WebSocketErrorEventArgs eventArgs)
        {
            var data = eventArgs.ErrorMessage + " ," + eventArgs.Exception.Message;
            mInfo.Add("Error" + data);
            mScrollPosition = new Vector2(0, mHeight);
        }
    }
    
    private void OnWebSocketClose(object sender, GameEventArgs e)
    {
        if (e is WebSocketCloseEventArgs eventArgs)
        {
            var data = eventArgs.Code + " ," + eventArgs.Reason;
            mInfo.Add("Close : " + data);
            mScrollPosition = new Vector2(0, mHeight);
        }
    }

    private void OnGUI()
    {
        if (!mIsLogin)
        {
            GUI.Label(new Rect(20, 20, 100, 30), "Login Address : ");
            mLoginAddress = GUI.TextField(new Rect(140, 20, 300, 30), mLoginAddress);
            
            GUI.Label(new Rect(20, 70, 100, 30), "User Name : ");
            mUserName = GUI.TextField(new Rect(140, 70, 150, 30), mUserName);
            
            GUI.Label(new Rect(20, 120, 100, 30), "Password : ");
            mPassword = GUI.TextField(new Rect(140, 120, 150, 30), mPassword);

            if (GUI.Button(new Rect(95, 170, 100, 30), "Login in"))
            {
                var loginData = new LoginData(mUserName, mPassword);
                var json = JsonUtility.ToJson(loginData);
                var postData = Encoding.UTF8.GetBytes(json);
                MainEntry.WebRequest.AddWebRequest(mLoginAddress, postData);
            }
        }
        else
        {
            GUI.Label(new Rect(20, 20, 200, 30), "NickName : " + mNickName);
            
            mAddress = GUI.TextField(new Rect(20, 70, 400, 30), mAddress);
        
            if (GUI.Button(new Rect(440, 70, 100, 30), "Connect"))
            {
                MainEntry.WebSocket.Connect(mAddress);
            }

            if (GUI.Button(new Rect(560, 70, 100, 30), "Close"))
            {
                MainEntry.WebSocket.Close();
            }

            mMessage = GUI.TextArea(new Rect(20, 120, 300, 200), mMessage);

            mIsText = GUI.Toggle(new Rect(350, 120, 100, 30), mIsText, "Text");

            if (GUI.Button(new Rect(500, 120, 100, 30), "Send"))
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
            mHeight = mInfo.Count < 9 ? 200 : 200 + (mInfo.Count - 9) * 20;
            mScrollPosition = GUI.BeginScrollView(new Rect(20, 320, 800, 200), mScrollPosition, new Rect(0,0, 750, mHeight));
            {
                for (int i = 0; i < mInfo.Count; i++)
                {
                    GUILayout.Label(mInfo[i]);
                }
            }
            GUI.EndScrollView();
        }
    }

    [Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class LoginResponse
    {
        public int code;
        public string msg;
        public long time;
        public ResponseData data;
    }
    
    [Serializable]
    public class ResponseData
    {
        public string token;
        public string username;
        public string nickname;
        public bool firstLogin;
    }

    
}
