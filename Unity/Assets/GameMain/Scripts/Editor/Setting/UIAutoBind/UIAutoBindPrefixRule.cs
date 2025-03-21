/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/23 11:13:3
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using UnityEngine;

namespace GameMain.Editor
{
    /// <summary>
    /// 自动绑定索引前缀规则
    /// </summary>
    [Serializable]
    public class UIAutoBindPrefixRule
    {
        public string Prefix;
        public string FullName;

        public UIAutoBindPrefixRule(string prefix, string fullName)
        {
            Prefix = prefix;
            FullName = fullName;
        }
    }
    
    [UnityEditor.CustomPropertyDrawer(typeof(UIAutoBindPrefixRule))]
    public class UIAutoBindPrefixRuleDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginProperty(position, label, property);
            //FocusType.Passive 使用Tab键切换时不会被选中，FocusType.Keyboard 使用Tab键切换时会被选中，很显然这里我们不需要label能被选中进行编辑 
            position = UnityEditor.EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            //不让indentLevel层级影响到同一行的绘制，因为PropertyDrawer在很多地方都有可能被用到，可能出现嵌套使用
            var indent = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = 0;
            var prefixRect = new Rect(position.x, position.y, 80, position.height);
            var fullNameRect = new Rect(position.x + 85, position.y, 150, position.height);
            UnityEditor.EditorGUI.PropertyField(prefixRect, property.FindPropertyRelative("Prefix"), GUIContent.none);
            UnityEditor.EditorGUI.PropertyField(fullNameRect, property.FindPropertyRelative("FullName"), GUIContent.none);
            UnityEditor.EditorGUI.indentLevel = indent;
            UnityEditor.EditorGUI.EndProperty();
        }
    }
}