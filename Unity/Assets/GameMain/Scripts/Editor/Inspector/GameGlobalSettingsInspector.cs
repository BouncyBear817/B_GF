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
        private void OnEnable()
        {
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.TextField("Global Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                Helper.DrawPropertyField(serializedObject.FindProperty("mScriptAuthor"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mMainFont"));
                
                EditorGUILayout.Space(10);
                Helper.DrawPropertyField(serializedObject.FindProperty("mDebugMode"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mResourceMode"));
                
                EditorGUILayout.Space(10);
                Helper.DrawPropertyField(serializedObject.FindProperty("mServerType"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mInternalNet"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mExternalNet"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mFormalNet"));
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}