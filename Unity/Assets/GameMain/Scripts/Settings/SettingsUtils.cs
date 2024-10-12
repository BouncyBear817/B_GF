// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 16:35:3
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class SettingsUtils
    {
        private const string GameGlobalSettingsPath = "Settings/GameGlobalSettings";
        private static GameGlobalSettings mGameGlobalSettings;

        public static GameGlobalSettings GameGlobalSettings
        {
            get
            {
                if (mGameGlobalSettings == null)
                {
                    mGameGlobalSettings = GetSettingsByResources<GameGlobalSettings>(GameGlobalSettingsPath);
                }

                return mGameGlobalSettings;
            }
        }

        private const string GameConfigSettingsPath = "Settings/GameConfigSettings";
        private static GameConfigSettings mGameConfigSettings;

        public static GameConfigSettings GameConfigSettings
        {
            get
            {
                if (mGameConfigSettings == null)
                {
                    mGameConfigSettings = GetSettingsByResources<GameConfigSettings>(GameConfigSettingsPath);
                }

                return mGameConfigSettings;
            }
        }
        
        private const string GamePathSettingsPath = "Settings/GamePathSettings";
        private static GamePathSettings mGamePathSettings;

        public static GamePathSettings GamePathSettings
        {
            get
            {
                if (mGamePathSettings == null)
                {
                    mGamePathSettings = GetSettingsByResources<GamePathSettings>(GamePathSettingsPath);
                }

                return mGamePathSettings;
            }
        }

        public static string GetResourceServerPath()
        {
            switch (GameGlobalSettings.ServerType)
            {
                case ServerType.InternalNet:
                    return GameGlobalSettings.InternalNet;
                case ServerType.ExternalNet:
                    return GameGlobalSettings.ExternalNet;
                case ServerType.FormalNet:
                    return GameGlobalSettings.FormalNet;
                case ServerType.None:
                default:
                    Log.Fatal("GameGlobalSettings.ServerType is invalid.");
                    return null;
            }
        }

        public static string GetVersionListPath()
        {
            return GetResourceServerPath() + GameGlobalSettings.ConfigPath + GameGlobalSettings.VersionListFileName;
        }

#if UNITY_EDITOR
        public static T GetSettings<T>() where T : ScriptableObject, new()
        {
            var assetType = typeof(T).Name;
            var paths = UnityEditor.AssetDatabase.FindAssets($"t:{assetType}");
            if (paths.Length == 0)
            {
                Debug.LogError($"{assetType} is not existed.");
                return null;
            }

            if (paths.Length > 1)
            {
                Debug.LogError($"{assetType} is more than 1, please delete others and leave one.");
                return null;
            }

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }
#endif

        public static T GetSettingsByResources<T>(string assetsPath) where T : ScriptableObject, new()
        {
            var assetType = typeof(T).Name;

#if UNITY_EDITOR
            var paths = UnityEditor.AssetDatabase.FindAssets($"t:{assetType}");
            if (paths.Length == 0)
            {
                Debug.LogError($"{assetType} is not existed.");
                return null;
            }

            if (paths.Length > 1)
            {
                Debug.LogError($"{assetType} is more than 1, please delete others and leave one.");
                return null;
            }
#endif

            var settings = Resources.Load<T>(assetsPath);
            if (settings == null)
            {
                Debug.LogError($"Not found {assetType} asset, please create one.");
                return null;
            }

            return settings;
        }
    }
}