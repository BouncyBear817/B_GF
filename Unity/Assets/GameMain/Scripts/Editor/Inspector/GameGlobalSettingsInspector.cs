// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/18 15:10:44
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEditor;

namespace GameMain.Editor
{
    [CustomEditor(typeof(GameGlobalSettings))]
    public class GameGlobalSettingsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                Helper.DrawPropertyField(serializedObject.FindProperty("mScriptAuthor"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mScriptVersion"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mAppStage"));
                
                Helper.DrawPropertyField(serializedObject.FindProperty("mMainFont"));
                
                Helper.DrawPropertyField(serializedObject.FindProperty("mForceUpdateApp"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mAppUpdateUri"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mAppUpdateDesc"));
                
                Helper.DrawPropertyField(serializedObject.FindProperty("mApplicableGameVersion"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mInternalResourceVersion"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mResourceVersionFileName"));
                
                Helper.DrawPropertyField(serializedObject.FindProperty("mServerType"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mInternalNet"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mExternalNet"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mFormalNet"));
                
                Helper.DrawPropertyField(serializedObject.FindProperty("mCurrentUseServerChannel"));
                Helper.DrawPropertyField(serializedObject.FindProperty("mServerChannelInfos"));
                EditorGUILayout.Space(20);
                if (!changeCheckScope.changed)
                {
                    return;
                }
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            serializedObject.ApplyModifiedProperties();
        }
    }
}