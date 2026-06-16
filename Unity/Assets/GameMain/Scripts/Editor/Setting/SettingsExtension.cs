using UnityEngine;

namespace GameMain.Editor
{
    public class SettingsExtension
    {
        private static GameConfigSettings sGameConfigSettings;

        public static GameConfigSettings GameConfigSettings
        {
            get
            {
                if (sGameConfigSettings == null)
                {
                    sGameConfigSettings = GetSettings<GameConfigSettings>();
                }

                return sGameConfigSettings;
            }
            
        }
        
        private static GameBuildSettings sGameBuildSettings;

        public static GameBuildSettings GameBuildSettings
        {
            get
            {
                if (sGameBuildSettings == null)
                {
                    sGameBuildSettings = GetSettings<GameBuildSettings>();
                }

                return sGameBuildSettings;
            }
        }
        
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
    }
}