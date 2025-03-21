﻿// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 16:35:3
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
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

        private const string GameBuildSettingsPath = "Settings/GameBuildSettings";
        private static GameBuildSettings sGameBuildSettings;

        public static GameBuildSettings GameBuildSettings
        {
            get
            {
                if (sGameBuildSettings == null)
                {
                    sGameBuildSettings = GetSettingsByResources<GameBuildSettings>(GameBuildSettingsPath);
                }

                return sGameBuildSettings;
            }
        }

        public static string GetVersionListPath(string platform)
        {
            return PathUtil.GetCombinePath(GameBuildSettings.UpdatePrefixUri, platform, GameBuildSettings.ResourceVersionFileName);
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

        public static ScriptableObject GetSettings(string assetTypeName)
        {
            var paths = UnityEditor.AssetDatabase.FindAssets($"t:{assetTypeName}");
            if (paths.Length == 0)
            {
                Debug.LogError($"{assetTypeName} is not existed.");
                return null;
            }

            if (paths.Length > 1)
            {
                Debug.LogError($"{assetTypeName} is more than 1, please delete others and leave one.");
                return null;
            }

            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
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