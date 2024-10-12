// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 16:9:17
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameMain.Editor
{
    public class UIAutoBindGlobalSettingProvider : SettingsProvider
    {
        private const string AutoBindGlobalSettingPath = "Assets/GameMain/Settings/UIAutoBindGlobalSettings.asset";
        private const string HeaderName = "Settings/UIAutoBindGlobalSettings";

        private SerializedObject mCustomSettings;
        private SerializedProperty mPrefixRuleList;
        private SerializedProperty mComponentCodePath;
        private SerializedProperty mMountCodePath;
        private SerializedProperty mMountScriptAssemblyList;

        public static SerializedObject GetSerializedSettings()
        {
            var setting = SettingsUtils.GetSettings<UIAutoBindGlobalSettings>();
            return new SerializedObject(setting);
        }

        public static bool IsSettingsAvailable()
        {
            return File.Exists(AutoBindGlobalSettingPath);
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            mCustomSettings = GetSerializedSettings();
            mComponentCodePath = mCustomSettings.FindProperty("mComponentCodePath");
            mMountCodePath = mCustomSettings.FindProperty("mMountCodePath");
            mMountScriptAssemblyList = mCustomSettings.FindProperty("mMountScriptAssemblyList");
            mPrefixRuleList = mCustomSettings.FindProperty("mPrefixRuleList");
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);
            mCustomSettings.Update();

            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                Helper.DrawPropertyField(mCustomSettings.FindProperty("mNamespace"));

                EditorGUILayout.BeginVertical();
                Helper.DrawPropertyField(mComponentCodePath);
                if (GUILayout.Button("Select", GUILayout.Width(50)))
                {
                    var folder = Path.Combine(Application.dataPath, mComponentCodePath.stringValue);
                    if (!Directory.Exists(folder))
                    {
                        folder = Application.dataPath;
                    }

                    var path = EditorUtility.OpenFolderPanel("Select Component Code Saved Path", folder, "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        mComponentCodePath.stringValue = path.Replace(Application.dataPath + "/", "");
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                Helper.DrawPropertyField(mMountCodePath);
                if (GUILayout.Button("Select", GUILayout.Width(50)))
                {
                    var folder = Path.Combine(Application.dataPath, mMountCodePath.stringValue);
                    if (!Directory.Exists(folder))
                    {
                        folder = Application.dataPath;
                    }

                    var path = EditorUtility.OpenFolderPanel("Select Mount Code Saved Path", folder, "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        mMountCodePath.stringValue = path.Replace(Application.dataPath + "/", "");
                    }
                }

                EditorGUILayout.EndVertical();

                Helper.DrawPropertyField(mMountScriptAssemblyList);
                Helper.DrawPropertyField(mPrefixRuleList);
                
                EditorGUILayout.Space(20);
                if (!changeCheckScope.changed)
                {
                    return;
                }

                mCustomSettings.ApplyModifiedPropertiesWithoutUndo();
                mCustomSettings.ApplyModifiedProperties();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public UIAutoBindGlobalSettingProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new UIAutoBindGlobalSettingProvider(HeaderName, SettingsScope.Project)
                {
                    keywords = GetSearchKeywordsFromGUIContentProperties<UIAutoBindGlobalSettings>()
                };
                return provider;
            }

            return null;
        }
    }
}