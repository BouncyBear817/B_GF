// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/18 15:10:44
//  * Description:
//  * Modify Record:
//  *************************************************************/

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
                    mGameGlobalSettings.MainFont = (Font)EditorGUILayout.ObjectField("", mGameGlobalSettings.MainFont, typeof(Font), false);
                }
                EditorGUILayout.EndHorizontal();
                
            }
            EditorGUILayout.EndVertical();
            
            GUILayout.Space(10);
            EditorGUILayout.TextField("App Version", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Force Update App", GUILayout.Width(160f));
                    mGameGlobalSettings.ForceUpdateApp = EditorGUILayout.ToggleLeft("", mGameGlobalSettings.ForceUpdateApp);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("App Update Uri", GUILayout.Width(160f));
                    mGameGlobalSettings.AppUpdateUri = EditorGUILayout.TextField(mGameGlobalSettings.AppUpdateUri);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("App Update Desc", GUILayout.Width(160f));
                    mGameGlobalSettings.AppUpdateDesc = EditorGUILayout.TextArea(mGameGlobalSettings.AppUpdateDesc);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("App Build Path", GUILayout.Width(160f));
                    mGameGlobalSettings.AppBuildPath = EditorGUILayout.TextField(mGameGlobalSettings.AppBuildPath);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("App Version", GUILayout.Width(160f));
                    PlayerSettings.bundleVersion = EditorGUILayout.TextField(PlayerSettings.bundleVersion);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
            EditorGUILayout.TextField("Resource Version", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                    mGameGlobalSettings.ApplicableGameVersion = EditorGUILayout.TextField(mGameGlobalSettings.ApplicableGameVersion);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Resource Version FileName", GUILayout.Width(160f));
                    mGameGlobalSettings.ResourceVersionFileName = EditorGUILayout.TextField(mGameGlobalSettings.ResourceVersionFileName);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
            EditorGUILayout.TextField("Server", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Server Type", GUILayout.Width(160f));
                    mServerType.enumValueIndex = (int)(ServerType)EditorGUILayout.EnumPopup("", mGameGlobalSettings.ServerType);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (mServerType.enumValueIndex == (int)ServerType.InternalNet)
                    {
                        EditorGUILayout.LabelField("Internal Net", GUILayout.Width(160f));
                        mGameGlobalSettings.InternalNet = EditorGUILayout.TextField(mGameGlobalSettings.InternalNet);
                    }
                    else if (mServerType.enumValueIndex == (int)ServerType.ExternalNet)
                    {
                        EditorGUILayout.LabelField("External Net", GUILayout.Width(160f));
                        mGameGlobalSettings.ExternalNet = EditorGUILayout.TextField(mGameGlobalSettings.ExternalNet);
                    }
                    else if (mServerType.enumValueIndex == (int)ServerType.FormalNet)
                    {
                        EditorGUILayout.LabelField("Formal Net", GUILayout.Width(160f));
                        mGameGlobalSettings.FormalNet = EditorGUILayout.TextField(mGameGlobalSettings.FormalNet);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}