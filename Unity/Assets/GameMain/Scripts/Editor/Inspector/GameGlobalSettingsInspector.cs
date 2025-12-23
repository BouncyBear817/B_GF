// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/18 15:10:44
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using GameFramework.Resource;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomEditor(typeof(GameGlobalSettings))]
    public class GameGlobalSettingsInspector : UnityEditor.Editor
    {
        private GameGlobalSettings mGameGlobalSettings;
        private SerializedProperty mServerType;

        private void OnEnable()
        {
            mGameGlobalSettings = target as GameGlobalSettings;

            mServerType = serializedObject.FindProperty("mServerType");

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.TextField("Global Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Script Author", GUILayout.Width(160f));
                    mGameGlobalSettings.ScriptAuthor = EditorGUILayout.TextField(mGameGlobalSettings.ScriptAuthor);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Main Font", GUILayout.Width(160f));
                    mGameGlobalSettings.MainFont = (TMP_FontAsset)EditorGUILayout.ObjectField("", mGameGlobalSettings.MainFont, typeof(TMP_FontAsset), false);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Debug Mode", GUILayout.Width(160f));
                    mGameGlobalSettings.DebugMode = EditorGUILayout.Toggle(mGameGlobalSettings.DebugMode, GUILayout.Width(160f));
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Resource Mode", GUILayout.Width(160f));
                    mGameGlobalSettings.ResourceMode = (ResourceMode)EditorGUILayout.EnumPopup(mGameGlobalSettings.ResourceMode, GUILayout.Width(160f));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Server Type", GUILayout.Width(160f));
                    mGameGlobalSettings.ServerType = (ServerType)EditorGUILayout.EnumPopup(mGameGlobalSettings.ServerType, GUILayout.Width(160f));
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Server Address", GUILayout.Width(160f));

                    switch (mGameGlobalSettings.ServerType)
                    {
                        case ServerType.InternalNet:
                            mGameGlobalSettings.InternalNet = EditorGUILayout.TextField(mGameGlobalSettings.InternalNet);
                            break;
                        case ServerType.ExternalNet:
                            mGameGlobalSettings.ExternalNet = EditorGUILayout.TextField(mGameGlobalSettings.ExternalNet);
                            break;
                        case ServerType.FormalNet:
                            mGameGlobalSettings.FormalNet = EditorGUILayout.TextField(mGameGlobalSettings.FormalNet);
                            break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}