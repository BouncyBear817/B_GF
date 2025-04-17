using UnityEngine;

namespace GameMain.Editor
{
    [CreateAssetMenu(fileName = "GameBuildSettings", menuName = "Tools/Game Build Settings", order = 5)]
    public class GameBuildSettings : ScriptableObject
    {
        [SerializeField] private bool mForceUpdateApp = false;
        [SerializeField] private string mAppUpdateUri = "";
        [SerializeField] private string mAppUpdateDesc = "";
        [SerializeField] private string mGameBuildPath = "";
        [SerializeField] private string mApplicableGameVersion = "";

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

        public string GameBuildPath
        {
            get => mGameBuildPath;
            set => mGameBuildPath = value;
        }

        public string ApplicableGameVersion
        {
            get => mApplicableGameVersion;
            set => mApplicableGameVersion = value;
        }
    }
}