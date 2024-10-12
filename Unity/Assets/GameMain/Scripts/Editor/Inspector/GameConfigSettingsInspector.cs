// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/09 10:10:36
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(GameConfigSettings))]
    public partial class GameConfigSettingsInspector : UnityEditor.Editor
    {
        private const int ONE_LINE_SHOW_COUNT = 3;
        private const string EXCEL_EXTENSION = ".xlsx";

        private GameConfigSettings mGameConfigSettings;
        private GameConfigView[] mGameConfigViews;

        private void OnEnable()
        {
            mGameConfigSettings = (GameConfigSettings)target;
            mGameConfigViews = new[]
            {
                new GameConfigView(mGameConfigSettings, GameConfigType.Config),
                new GameConfigView(mGameConfigSettings, GameConfigType.DataTable),
                new GameConfigView(mGameConfigSettings, GameConfigType.Language)
            };

            ReloadView(mGameConfigSettings);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var itemWidth = GUILayout.Width(Mathf.Max(EditorGUIUtility.currentViewWidth / ONE_LINE_SHOW_COUNT - 20, 100));
            foreach (var gameConfigView in mGameConfigViews)
            {
                if (gameConfigView.DrawPanel(itemWidth))
                {
                    SaveConfig(mGameConfigSettings);
                }
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Reload", GUILayout.Height(30)))
                {
                    ReloadView(mGameConfigSettings);
                }

                if (GUILayout.Button("Save", GUILayout.Height(30)))
                {
                    SaveConfig(mGameConfigSettings);
                }
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void SaveConfig(GameConfigSettings gameConfigSettings)
        {
            foreach (var gameConfigView in mGameConfigViews)
            {
                var type = gameConfigSettings.GetType();
                if (gameConfigView.ConfigType == GameConfigType.Config)
                {
                    var field = type.GetField("mConfigs", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        field.SetValue(gameConfigSettings, gameConfigView.GetSelectedGameConfig());
                    }
                }
                else if (gameConfigView.ConfigType == GameConfigType.DataTable)
                {
                    var field = type.GetField("mDataTables", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        field.SetValue(gameConfigSettings, gameConfigView.GetSelectedGameConfig());
                    }
                }
                else if (gameConfigView.ConfigType == GameConfigType.Language)
                {
                    var field = type.GetField("mLanguages", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        field.SetValue(gameConfigSettings, gameConfigView.GetSelectedGameConfig());
                    }
                }
            }

            EditorUtility.SetDirty(gameConfigSettings);
        }

        private void ReloadView(GameConfigSettings gameConfigSettings)
        {
            foreach (var gameConfigView in mGameConfigViews)
            {
                gameConfigView.Reload();
            }
        }
    }
}