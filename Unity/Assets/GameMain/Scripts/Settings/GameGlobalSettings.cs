// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 11:28:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu(fileName = "GameGlobalSettings", menuName = "Tools/Game Global Settings", order = 1)]
    public class GameGlobalSettings : ScriptableObject
    {
        [SerializeField] private string mScriptAuthor = "Default";
        [SerializeField] private Font mMainFont;

        [SerializeField] private bool mForceUpdateApp = false;
        [SerializeField] private string mAppUpdateUri = "";
        [SerializeField] private string mAppUpdateDesc = "";
        [SerializeField] private string mAppBuildPath = "";

        [SerializeField] private string mApplicableGameVersion = "";
        [SerializeField] private string mResourceVersionFileName = "ResourceVersion.txt";

        [SerializeField] private ServerType mServerType = ServerType.None;
        [SerializeField] private string mInternalNet = "";
        [SerializeField] private string mExternalNet = "";
        [SerializeField] private string mFormalNet = "";

        public string ScriptAuthor
        {
            get => mScriptAuthor;
            set => mScriptAuthor = value;
        }

        public Font MainFont
        {
            get => mMainFont;
            set => mMainFont = value;
        }

        public bool ForceUpdateApp
        {
            get => mForceUpdateApp;
            set => mForceUpdateApp = value;
        }

        public string AppUpdateUri
        {
            get => mAppUpdateUri;
            set => mAppUpdateUri = value;
        }

        public string AppUpdateDesc
        {
            get => mAppUpdateDesc;
            set => mAppUpdateDesc = value;
        }

        public string AppBuildPath
        {
            get => mAppBuildPath;
            set => mAppBuildPath = value;
        }

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

        public string ApplicableGameVersion
        {
            get => mApplicableGameVersion;
            set => mApplicableGameVersion = value;
        }

        public string ResourceVersionFileName
        {
            get => mResourceVersionFileName;
            set => mResourceVersionFileName = value;
        }

        public ServerType ServerType
        {
            get => mServerType;
            set => mServerType = value;
        }

        public string InternalNet
        {
            get => mInternalNet;
            set => mInternalNet = value;
        }

        public string ExternalNet
        {
            get => mExternalNet;
            set => mExternalNet = value;
        }

        public string FormalNet
        {
            get => mFormalNet;
            set => mFormalNet = value;
        }
    }
}