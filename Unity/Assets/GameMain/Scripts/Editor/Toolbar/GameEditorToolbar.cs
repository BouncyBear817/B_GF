using System.Collections.Generic;
using System.IO;
using GameFramework;
using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameMain.Editor
{
    public static class GameEditorToolbar
    {
        private static List<string> sSettingsList = new List<string>()
        {
            "UI Auto Bind Global Settings",
            "Game Global Settings",
            "Game Config Settings",
            "Game Path Settings"
        };

        private static GUIContent sScenesContent;
        private static GUIContent sBuildAppContent;

        private static GUIContent sGameSettingsContent;
        private static GUIContent sOpenCsProjectContent;

        [InitializeOnLoadMethod]
        static void Init()
        {
            sScenesContent = EditorGUIUtility.TrTextContentWithIcon("Switch Scene", "切换场景", "SceneAsset Icon");
            sBuildAppContent = EditorGUIUtility.TrTextContentWithIcon("Build", "打包", "BuildSettings.Standalone");

            sGameSettingsContent = EditorGUIUtility.TrTextContentWithIcon("Game Settings", "游戏配置", "Settings");
            sOpenCsProjectContent = EditorGUIUtility.TrTextContentWithIcon("Open C# Project", "打开C#工程", "dll Script Icon");

            EditorSceneManager.sceneOpened += (scene, mode) =>
            {
                sScenesContent.text = scene.name;
            };

            ToolbarExtension.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            ToolbarExtension.RightToolbarGUI.Add(OnRightToolbarGUI);
        }

        static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            if (EditorGUILayout.DropdownButton(sScenesContent, FocusType.Passive, EditorStyles.toolbarPopup, GUILayout.Width(125)))
            {
                DrawScenesDropdownMenu();
            }
        }

        private static void DrawScenesDropdownMenu()
        {
            var popMenu = new GenericMenu()
            {
                allowDuplicateNames = true
            };

            var sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { SettingsUtils.GamePathSettings.ScenePath });
            for (var i = 0; i < sceneGuids.Length; i++)
            {
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);
                
                var fileDir = Path.GetDirectoryName(scenePath);
                var isInRootDir = Utility.Path.GetRegularPath(SettingsUtils.GamePathSettings.ScenePath).TrimEnd('/') == Utility.Path.GetRegularPath(fileDir).TrimEnd('/');
                if (!isInRootDir)
                {
                    var sceneDir = Path.GetRelativePath(SettingsUtils.GamePathSettings.ScenePath, fileDir);
                    sceneName = $"{sceneDir}/{sceneName}";
                }
                
                popMenu.AddItem(new GUIContent(sceneName), false, path =>
                {
                    EditorSceneManager.OpenScene((string)path, OpenSceneMode.Single);
                }, scenePath);
            }
            
            popMenu.ShowAsContext();
        }

        static void OnRightToolbarGUI()
        {
            if (GUILayout.Button(sBuildAppContent, EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                GameBuilder.Open();
            }
            
            GUILayout.Space(10);
            if (EditorGUILayout.DropdownButton(sGameSettingsContent, FocusType.Passive, EditorStyles.toolbarPopup, GUILayout.Width(125)))
            {
                DrawSettingsDropdownMenu();
            }

            GUILayout.Space(10);
            if (GUILayout.Button(sOpenCsProjectContent, EditorStyles.toolbarButton, GUILayout.Width(125)))
            {
                // Ensure that the mono islands are up-to-date
                AssetDatabase.Refresh();
                CodeEditor.Editor.CurrentCodeEditor.SyncAll();

                CodeEditor.Editor.CurrentCodeEditor.OpenProject();
            }
        }

        private static void DrawSettingsDropdownMenu()
        {
            var popMenu = new GenericMenu()
            {
                allowDuplicateNames = true
            };

            for (var i = 0; i < sSettingsList.Count; i++)
            {
                popMenu.AddItem(new GUIContent(sSettingsList[i]), false, data =>
                {
                    var settingsTypeName = sSettingsList[(int)data].Replace(" ", "");
                    Selection.activeObject = SettingsUtils.GetSettings(settingsTypeName);
                }, i);
            }

            popMenu.ShowAsContext();
        }
    }
}