using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [CreateAssetMenu(fileName = "GameBuildSettings", menuName = "Tools/Game Build Settings", order = 5)]
    public class GameBuildSettings : ScriptableObject
    {
        public bool ForceUpdateApp = false;
        public string AppUpdateUri = "";
        public string AppUpdateDesc = "";
        [FormerlySerializedAs("AppBuildPath")] public string GameBuildPath = "";
        public string ApplicableGameVersion = "";
        public string ResourceVersionFileName = "ResourceVersion.txt";

        public ServerType ServerType = ServerType.None;
        public string InternalNet = "";
        public string ExternalNet = "";
        public string FormalNet = "";

        public bool DebugMode = false;
        public ResourceMode ResourceMode = ResourceMode.Unspecified;
        
        public string UpdatePrefixUri
        {
            get
            {
                switch (ServerType)
                {
                    case ServerType.InternalNet:
                        return InternalNet;
                    case ServerType.ExternalNet:
                        return ExternalNet;
                    case ServerType.FormalNet:
                        return FormalNet;
                    default:
                        return "";
                }
            }
        }

    }
}