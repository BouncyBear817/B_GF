// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/18 14:10:52
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using UnityEditor;

namespace GameMain.Editor
{
    [CustomEditor(typeof(GamePathSettings))]
    public class GamePathSettingsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            Helper.DrawPropertyField(serializedObject.FindProperty("mConfigPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mDataTablePath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mLocalizationPath"));

            Helper.DrawPropertyField(serializedObject.FindProperty("mConfigExcelPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mDataTableExcelPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mLocalizationExcelPath"));

            Helper.DrawPropertyField(serializedObject.FindProperty("mEntityGroupDataTableExcelPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mSoundGroupDataTableExcelPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mUIGroupDataTableExcelPath"));

            Helper.DrawPropertyField(serializedObject.FindProperty("mDataTableGroupCodePath"));

            Helper.DrawPropertyField(serializedObject.FindProperty("mEntityPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mFontPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mMusicPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mScenePath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mSoundPath"));
            Helper.DrawPropertyField(serializedObject.FindProperty("mUIPath"));
            
            serializedObject.ApplyModifiedProperties();

            Repaint();
        }
    }
}