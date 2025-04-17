// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 16:35:3
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Threading.Tasks;
using UnityEngine;

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

        private const string GameConfigSettingsPath = "GameConfigSettings";
        private static GameConfigSettings mGameConfigSettings;

        public static async Task<GameConfigSettings> GetGameConfigSettings()
        {
            if (mGameConfigSettings == null)
            {
                mGameConfigSettings = await GetSettingsByAsset<GameConfigSettings>(GameConfigSettingsPath);
            }

            return mGameConfigSettings;
        }

        public static string GetVersionListPath(string platform)
        {
            return PathUtil.GetCombinePath(GameGlobalSettings.UpdatePrefixUri, platform, Constant.ResourceVersionFileName);
        }

        public static T GetSettingsByResources<T>(string assetsPath) where T : ScriptableObject, new()
        {
            var assetType = typeof(T).Name;
            var settings = Resources.Load<T>(assetsPath);
            if (settings == null)
            {
                Debug.LogError($"Not found {assetType} asset, please create one.");
                return null;
            }

            return settings;
        }

        public static async Task<T> GetSettingsByAsset<T>(string assetsPath) where T : ScriptableObject, new()
        {
            var asset = AssetUtil.GetSettingsAsset(assetsPath);
            return await MainEntry.Resource.LoadAssetsAsync<T>(asset);
        }
    }
}