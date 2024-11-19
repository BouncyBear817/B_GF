// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/5 11:28:24
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameMain.Editor
{
    public class GameGlobalSettingsProvider : SettingsProvider
    {
        private const string GameGlobalSettingsPath = "Assets/GameMain/Resources/Settings/GameGlobalSettings.asset";
        private const string HeaderName = "Settings/GameGlobalSettings";

        private SerializedObject mCustomSettings;

        public GameGlobalSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public static SerializedObject GetSerializedSettings() => new SerializedObject(SettingsUtils.GameGlobalSettings);

        public static bool IsSettingsAvailable() => File.Exists(GameGlobalSettingsPath);

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
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mScriptAuthor"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mMainFont"));
                
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mForceUpdateApp"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mAppUpdateUri"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mAppUpdateDesc"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mAppBuildPath"));
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("App Version");
                    PlayerSettings.bundleVersion = EditorGUILayout.TextField(PlayerSettings.bundleVersion);
                }
                EditorGUILayout.EndHorizontal();
                
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mApplicableGameVersion"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mResourceVersionFileName"));
                
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mServerType"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mInternalNet"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mExternalNet"));
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mFormalNet"));
                
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
                var provider = new GameGlobalSettingsProvider(HeaderName, SettingsScope.Project)
                {
                    keywords = GetSearchKeywordsFromGUIContentProperties<GameGlobalSettings>()
                };
                return provider;
            }

            return null;
        }
    }
}