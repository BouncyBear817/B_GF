// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/09 10:10:27
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameMain.Editor
{
    public class GameConfigSettingsProvider : SettingsProvider
    {
        private const string GameConfigSettingsPath = "Assets/GameMain/Resources/Settings/GameConfigSettings.asset";
        private const string HeaderName = "Settings/GameConfigSettings";

        private SerializedObject mCustomSettings;

        public GameConfigSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public static SerializedObject GetSerializedSettings() => new SerializedObject(SettingsUtils.GameConfigSettings);

        public static bool IsSettingsAvailable() => File.Exists(GameConfigSettingsPath);

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);

            mCustomSettings = GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            mCustomSettings.Update();

            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mDataTables"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mConfigs"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mLanguages"));
                EditorGUILayout.Space(20);
                if (!changeCheckScope.changed)
                {
                    return;
                }
            }

            mCustomSettings.ApplyModifiedPropertiesWithoutUndo();
            mCustomSettings.ApplyModifiedProperties();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new GameConfigSettingsProvider(HeaderName, SettingsScope.Project)
                {
                    keywords = GetSearchKeywordsFromGUIContentProperties<GameConfigSettings>()
                };
                return provider;
            }

            return null;
        }
    }
}