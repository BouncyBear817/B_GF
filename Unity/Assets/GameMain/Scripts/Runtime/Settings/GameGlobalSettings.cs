// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 11:28:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using GameFramework.Resource;
using TMPro;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu(fileName = "GameGlobalSettings", menuName = "Tools/Game Global Settings", order = 1)]
    public class GameGlobalSettings : ScriptableObject
    {
        [SerializeField] private string mScriptAuthor = "Default";
        [SerializeField] private TMP_FontAsset mMainFont;

        [SerializeField] private bool mDebugMode = false;
        [SerializeField] private ResourceMode mResourceMode = ResourceMode.Unspecified;

        [SerializeField] private ServerType mServerType = ServerType.None;
        [SerializeField] private string mInternalNet = "";
        [SerializeField] private string mExternalNet = "";
        [SerializeField] private string mFormalNet = "";
        
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

        public string ScriptAuthor
        {
            get => mScriptAuthor;
            set => mScriptAuthor = value;
        }

        public TMP_FontAsset MainFont
        {
            get => mMainFont;
            set => mMainFont = value;
        }

        public bool DebugMode
        {
            get => mDebugMode;
            set => mDebugMode = value;
        }

        public ResourceMode ResourceMode
        {
            get => mResourceMode;
            set => mResourceMode = value;
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