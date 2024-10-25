// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 11:28:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [CreateAssetMenu(fileName = "GameGlobalSettings", menuName = "Bear Tools/Game Global Settings", order = 1)]
    public class GameGlobalSettings : ScriptableObject
    {
        [Header("Framework")] 
        [SerializeField] private string mScriptAuthor = "Default";
        [SerializeField] private string mScriptVersion = "0.0.1";
        [SerializeField] private AppStage mAppStage = AppStage.None;

        [Header("Font")] 
        [SerializeField] private Font mMainFont;

        [Header("App Version")] 
        [SerializeField] private bool mForceUpdateApp = false;
        [SerializeField] private string mAppUpdateUri = "";
        [SerializeField] private string mAppUpdateDesc = "";

        [Header("Resource Version")]
        [SerializeField] private string mApplicableGameVersion = "";
        [SerializeField] private int mInternalResourceVersion = 0;
        [SerializeField] private string mResourceVersionFileName = "ResourceVersion.txt";

        [Header("Resource Update Prefix Uri")] 
        [SerializeField] private ServerType mServerType = ServerType.None;
        [SerializeField] private string mInternalNet = "";
        [SerializeField] private string mExternalNet = "";
        [SerializeField] private string mFormalNet = "";

        [Header("Server")] [SerializeField] private string mCurrentUseServerChannel;
        [SerializeField] private List<ServerChannelInfo> mServerChannelInfos;


        public string ScriptAuthor => mScriptAuthor;

        public string ScriptVersion => mScriptVersion;

        public AppStage AppStage => mAppStage;

        public Font MainFont => mMainFont;

        public bool ForceUpdateApp => mForceUpdateApp;

        public string AppUpdateUri => mAppUpdateUri;

        public string AppUpdateDesc => mAppUpdateDesc;

        public string UpdatePrefixUri
        {
            get
            {
                switch (ServerType)
                {
                    case ServerType.InternalNet:
                        return mInternalNet;
                    case ServerType.ExternalNet:
                        return mExternalNet;
                    case ServerType.FormalNet:
                        return mFormalNet;
                    default:
                        return "";
                }
            }
        }

        public string ApplicableGameVersion => mApplicableGameVersion;

        public int InternalResourceVersion => mInternalResourceVersion;

        public string ResourceVersionFileName => mResourceVersionFileName;

        public ServerType ServerType => mServerType;

        public string InternalNet => mInternalNet;

        public string ExternalNet => mExternalNet;

        public string FormalNet => mFormalNet;

        public string CurrentUseServerChannel => mCurrentUseServerChannel;

        public List<ServerChannelInfo> ServerChannelInfos => mServerChannelInfos;
    }

    [Serializable]
    public class ServerChannelInfo
    {
        public string ChannelName;
        public string CurrentUseServerName;
        public List<ServerIPAndPort> ServerIPAndPorts;
    }

    [Serializable]
    public class ServerIPAndPort
    {
        public string ServerName;
        public string IP;
        public string Port;
    }
}