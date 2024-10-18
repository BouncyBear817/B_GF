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
    public class GamePathSettingsProvider : SettingsProvider
    {
        private const string GamePathSettingsPath = "Assets/GameMain/Resources/Settings/GamePathSettings.asset";
        private const string HeaderName = "Settings/GamePathSettings";

        private SerializedObject mCustomSettings;

        public GamePathSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public static SerializedObject GetSerializedSettings() => new SerializedObject(SettingsUtils.GamePathSettings);

        public static bool IsSettingsAvailable() => File.Exists(GamePathSettingsPath);

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
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mConfigPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mDataTablePath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mLocalizationPath"));

                Helper.DrawPropertyField(mCustomSettings.FindProperty("mConfigExcelPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mDataTableExcelPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mLocalizationExcelPath"));

                Helper.DrawPropertyField(mCustomSettings.FindProperty("mEntityGroupDataTableExcelPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mSoundGroupDataTableExcelPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mUIGroupDataTableExcelPath"));

                Helper.DrawPropertyField(mCustomSettings.FindProperty("mDataTableGroupCodePath"));

                Helper.DrawPropertyField(mCustomSettings.FindProperty("mEntityPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mFontPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mMusicPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mScenePath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mSoundPath"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mUIPath"));
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
                var provider = new GamePathSettingsProvider(HeaderName, SettingsScope.Project)
                {
                    keywords = GetSearchKeywordsFromGUIContentProperties<GamePathSettings>()
                };
                return provider;
            }

            return null;
        }
    }
}