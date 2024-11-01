// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/31 15:10:26
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    [CustomPropertyDrawer(typeof(SequenceAnimation))]
    public class SequenceAnimationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel++;

            var addType = property.FindPropertyRelative("AddType");
            var animationType = property.FindPropertyRelative("AnimationType");

            var useFromTarget = property.FindPropertyRelative("UseFromTarget");
            var fromTarget = property.FindPropertyRelative("FromTarget");
            var fromValue = property.FindPropertyRelative("FromValue");
            //
            // var useToTarget = property.FindPropertyRelative("UseToTarget");
            // var toTarget = property.FindPropertyRelative("ToTarget");
            // var toValue = property.FindPropertyRelative("ToValue");
            //
            // var speedBased = property.FindPropertyRelative("SpeedBased");
            // var duration = property.FindPropertyRelative("DurationOrSpeed");
            // var snapping = property.FindPropertyRelative("Snapping");
            //
            // var loops = property.FindPropertyRelative("Loops");
            // var loopType = property.FindPropertyRelative("LoopType");
            // var updateType = property.FindPropertyRelative("UpdateType");
            // var delay = property.FindPropertyRelative("Delay");
            // var curveOrEase = property.FindPropertyRelative("CurveOrEase");
            // var easeCurve = property.FindPropertyRelative("EaseCurve");
            // var ease = property.FindPropertyRelative("Ease");
            // var onPlay = property.FindPropertyRelative("OnPlay");
            // var onUpdate = property.FindPropertyRelative("OnUpdate");
            // var onComplete = property.FindPropertyRelative("OnComplete");

            var lastRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(lastRect, addType);

            // EditorGUI.BeginChangeCheck();
            {
                lastRect.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(lastRect, animationType);
                lastRect.y += EditorGUIUtility.singleLineHeight;
            }

            useFromTarget.boolValue = EditorGUI.Toggle(lastRect, "From", useFromTarget.boolValue);
            lastRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginDisabledGroup(!useFromTarget.boolValue);
            {
                fromValue.vector4Value = EditorGUI.Vector3Field(lastRect, "From", fromValue.vector4Value);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var onPlay = property.FindPropertyRelative("OnPlay");
            var onUpdate = property.FindPropertyRelative("OnUpdate");
            var onComplete = property.FindPropertyRelative("OnComplete");
            return EditorGUIUtility.singleLineHeight * 11 +
                   (property.isExpanded ? (EditorGUI.GetPropertyHeight(onPlay) + EditorGUI.GetPropertyHeight(onUpdate) + EditorGUI.GetPropertyHeight(onComplete)) : 0);
        }

        private static Component GetFixedComponent(Component component, DOTweenType tweenType)
        {
            if (component == null)
            {
                return null;
            }

            switch (tweenType)
            {
                case DOTweenType.DOMove:
                case DOTweenType.DOMoveX:
                case DOTweenType.DOMoveY:
                case DOTweenType.DOMoveZ:
                case DOTweenType.DOLocalMove:
                case DOTweenType.DOLocalMoveX:
                case DOTweenType.DOLocalMoveY:
                case DOTweenType.DOLocalMoveZ:
                case DOTweenType.DOScale:
                case DOTweenType.DOScaleX:
                case DOTweenType.DOScaleY:
                case DOTweenType.DOScaleZ:
                case DOTweenType.DORotate:
                case DOTweenType.DOLocalRotate:
                    return component.gameObject.GetComponent<Transform>();
                case DOTweenType.DOAnchorPos:
                case DOTweenType.DOAnchorPosX:
                case DOTweenType.DOAnchorPosY:
                case DOTweenType.DOAnchorPos3D:
                case DOTweenType.DOAnchorPos3DZ:
                case DOTweenType.DOSizeDelta:
                    return component.gameObject.GetComponent<RectTransform>();
                case DOTweenType.DOColor:
                case DOTweenType.DOColorAlphaFade:
                    return component.gameObject.GetComponent<UnityEngine.UI.Graphic>();
                case DOTweenType.DOCanvasGroupAlphaFade:
                    return component.gameObject.GetComponent<CanvasGroup>();
                case DOTweenType.DOFillAmount:
                    return component.gameObject.GetComponent<UnityEngine.UI.Image>();
                case DOTweenType.DOFlexibleSize:
                case DOTweenType.DOMinSize:
                case DOTweenType.DOPreferredSize:
                    return component.gameObject.GetComponent<UnityEngine.UI.LayoutElement>();
                case DOTweenType.DOSliderValue:
                    return component.gameObject.GetComponent<UnityEngine.UI.Slider>();
            }

            return null;
        }
    }
}