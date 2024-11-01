// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/31 10:10:31
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace GameMain
{
    
    [Serializable]
    public class SequenceAnimation
    {
        public AddType AddType = AddType.Append;
        public DOTweenType AnimationType = DOTweenType.DOMove;

        // From Target
        public bool UseFromTarget = true;
        public Component FromTarget;
        public Vector4 FromValue = Vector4.zero;

        // To Target
        public bool UseToTarget;
        public Component ToTarget;
        public Vector4 ToValue = Vector4.zero;

        // Sequence Animation params
        public bool SpeedBased;
        public float DurationOrSpeed = 1; // value > 0
        public bool Snapping;

        // DOTween params
        public int Loops = 1;
        public LoopType LoopType;
        public UpdateType UpdateType = UpdateType.Normal;
        public float Delay;
        public bool CurveOrEase;
        public AnimationCurve EaseCurve;
        public Ease Ease = Ease.OutQuad;
        public UnityEvent OnPlay;
        public UnityEvent OnUpdate;
        public UnityEvent OnComplete;
        
        public Tween CreateTween(bool reverse = false)
        {
            Tween result = null;

            switch (AnimationType)
            {
                case DOTweenType.DOMove:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        Vector3 start = UseFromTarget ? fromTrans.position : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.position : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.position = start;
                        var duration = GetDuration(Vector3.Distance(start, end));

                        result = fromTrans.DOMove(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOMoveX:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.position.x : FromValue.x;
                        var end = toTrans != null && UseToTarget ? toTrans.position.x : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetPositionX(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOMoveX(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOMoveY:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.position.y : FromValue.y;
                        var end = toTrans != null && UseToTarget ? toTrans.position.y : ToValue.y;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetPositionY(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOMoveY(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOMoveZ:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.position.z : FromValue.z;
                        var end = toTrans != null && UseToTarget ? toTrans.position.z : ToValue.z;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetPositionZ(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOMoveZ(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOLocalMove:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        Vector3 start = UseFromTarget ? fromTrans.localPosition : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.localPosition : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.localPosition = start;
                        var duration = GetDuration(Vector3.Distance(start, end));

                        result = fromTrans.DOLocalMove(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOLocalMoveX:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localPosition.x : FromValue.x;
                        var end = toTrans != null && UseToTarget ? toTrans.localPosition.x : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalPositionX(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOLocalMoveX(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOLocalMoveY:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localPosition.y : FromValue.y;
                        var end = toTrans != null && UseToTarget ? toTrans.localPosition.y : ToValue.y;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalPositionY(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOLocalMoveY(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOLocalMoveZ:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localPosition.z : FromValue.z;
                        var end = toTrans != null && UseToTarget ? toTrans.localPosition.z : ToValue.z;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalPositionZ(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOLocalMoveZ(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOScale:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        Vector3 start = UseFromTarget ? fromTrans.localScale : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.localScale : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.localScale = start;
                        var duration = GetDuration(Vector3.Distance(start, end));

                        result = fromTrans.DOScale(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOScaleX:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localScale.x : FromValue.x;
                        var end = toTrans != null && UseToTarget ? toTrans.localScale.x : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalScaleX(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOScaleX(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOScaleY:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localScale.y : FromValue.y;
                        var end = toTrans != null && UseToTarget ? toTrans.localScale.y : ToValue.y;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalScaleY(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOScaleY(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOScaleZ:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        var start = UseFromTarget ? fromTrans.localScale.z : FromValue.z;
                        var end = toTrans != null && UseToTarget ? toTrans.localScale.z : ToValue.z;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetLocalScaleZ(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOScaleZ(end, duration);
                    }
                }
                    break;
                case DOTweenType.DORotate:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        Vector3 start = UseFromTarget ? fromTrans.eulerAngles : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.eulerAngles : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.eulerAngles = start;
                        var duration = GetDuration(GetEulerAngle(end, start));

                        result = fromTrans.DORotate(end, duration, RotateMode.FastBeyond360);
                    }
                }
                    break;
                case DOTweenType.DOLocalRotate:
                {
                    var fromTrans = FromTarget as Transform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as Transform;
                        Vector3 start = UseFromTarget ? fromTrans.localEulerAngles : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.localEulerAngles : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.localEulerAngles = start;
                        var duration = GetDuration(GetEulerAngle(end, start));

                        result = fromTrans.DOLocalRotate(end, duration, RotateMode.FastBeyond360);
                    }
                }
                    break;
                case DOTweenType.DOAnchorPos:
                {
                    var fromTrans = FromTarget as RectTransform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as RectTransform;
                        Vector2 start = UseFromTarget ? fromTrans.anchoredPosition : FromValue;
                        Vector2 end = toTrans != null && UseToTarget ? toTrans.anchoredPosition : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.anchoredPosition = start;
                        var duration = GetDuration(Vector2.Distance(start, end));

                        result = fromTrans.DOAnchorPos(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOAnchorPosX:
                {
                    var fromTrans = FromTarget as RectTransform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as RectTransform;
                        var start = UseFromTarget ? fromTrans.anchoredPosition.x : FromValue.x;
                        var end = toTrans != null && UseToTarget ? toTrans.anchoredPosition.x : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetAnchoredPositionX(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOAnchorPosX(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOAnchorPosY:
                {
                    var fromTrans = FromTarget as RectTransform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as RectTransform;
                        var start = UseFromTarget ? fromTrans.anchoredPosition.y : FromValue.y;
                        var end = toTrans != null && UseToTarget ? toTrans.anchoredPosition.y : ToValue.y;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetAnchoredPositionY(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOAnchorPosY(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOAnchorPos3D:
                {
                    var fromTrans = FromTarget as RectTransform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as RectTransform;
                        Vector3 start = UseFromTarget ? fromTrans.anchoredPosition3D : FromValue;
                        Vector3 end = toTrans != null && UseToTarget ? toTrans.anchoredPosition3D : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.anchoredPosition3D = start;
                        var duration = GetDuration(Vector3.Distance(start, end));

                        result = fromTrans.DOAnchorPos3D(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOAnchorPos3DZ:
                {
                    var fromTrans = FromTarget as RectTransform;
                    if (fromTrans != null)
                    {
                        var toTrans = ToTarget as RectTransform;
                        var start = UseFromTarget ? fromTrans.anchoredPosition3D.z : FromValue.z;
                        var end = toTrans != null && UseToTarget ? toTrans.anchoredPosition3D.z : ToValue.z;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        fromTrans.SetAnchoredPosition3DZ(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = fromTrans.DOAnchorPos3DZ(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOSizeDelta:
                {
                    var from = FromTarget as RectTransform;
                    if (from != null)
                    {
                        var to = ToTarget as RectTransform;
                        Vector2 start = UseFromTarget ? from.sizeDelta : FromValue;
                        Vector2 end = to != null && UseToTarget ? to.sizeDelta : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.sizeDelta = start;
                        var duration = GetDuration(Vector2.Distance(end, start));

                        result = from.DOSizeDelta(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOColor:
                {
                    var from = FromTarget as UnityEngine.UI.Graphic;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.Graphic;
                        var start = UseFromTarget ? from.color : (Color)FromValue;
                        var end = to != null && UseToTarget ? to.color : (Color)ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.color = start;
                        var duration = GetDuration(Vector4.Distance(end, start));

                        result = from.DOColor(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOColorAlphaFade:
                {
                    var from = FromTarget as UnityEngine.UI.Graphic;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.Graphic;
                        var start = UseFromTarget ? from.color.a : FromValue.w;
                        var end = to != null && UseToTarget ? to.color.a : ToValue.w;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.SetColorAlpha(start);
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = from.DOFade(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOCanvasGroupAlphaFade:
                {
                    var from = FromTarget as CanvasGroup;
                    if (from != null)
                    {
                        var to = ToTarget as CanvasGroup;
                        var start = UseFromTarget ? from.alpha : FromValue.w;
                        var end = to != null && UseToTarget ? to.alpha : ToValue.w;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.alpha = start;
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = from.DOFade(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOFillAmount:
                {
                    var from = FromTarget as UnityEngine.UI.Image;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.Image;
                        var start = UseFromTarget ? from.fillAmount : FromValue.x;
                        var end = to != null && UseToTarget ? to.fillAmount : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.fillAmount = start;
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = from.DOFillAmount(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOFlexibleSize:
                {
                    var from = FromTarget as UnityEngine.UI.LayoutElement;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.LayoutElement;
                        Vector2 start = UseFromTarget ? from.GetFlexibleSize() : FromValue;
                        Vector2 end = to != null && UseToTarget ? to.GetFlexibleSize() : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.SetFlexibleSize(start);
                        var duration = GetDuration(Vector2.Distance(end, start));

                        result = from.DOFlexibleSize(end, duration);
                    }
                }
                    break;
                case DOTweenType.DOMinSize:
                {
                    var from = FromTarget as UnityEngine.UI.LayoutElement;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.LayoutElement;
                        Vector2 start = UseFromTarget ? from.GetMinSize() : FromValue;
                        Vector2 end = to != null && UseToTarget ? to.GetMinSize() : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.SetMinSize(start);
                        var duration = GetDuration(Vector2.Distance(end, start));

                        result = from.DOMinSize(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOPreferredSize:
                {
                    var from = FromTarget as UnityEngine.UI.LayoutElement;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.LayoutElement;
                        Vector2 start = UseFromTarget ? from.GetPreferredSize() : FromValue;
                        Vector2 end = to != null && UseToTarget ? to.GetPreferredSize() : ToValue;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.SetPreferredSize(start);
                        var duration = GetDuration(Vector2.Distance(end, start));

                        result = from.DOPreferredSize(end, duration, Snapping);
                    }
                }
                    break;
                case DOTweenType.DOSliderValue:
                {
                    var from = FromTarget as UnityEngine.UI.Slider;
                    if (from != null)
                    {
                        var to = ToTarget as UnityEngine.UI.Slider;
                        var start = UseFromTarget ? from.value : FromValue.x;
                        var end = to != null && UseToTarget ? to.value : ToValue.x;
                        if (reverse)
                        {
                            (end, start) = (start, end);
                        }

                        from.value = start;
                        var duration = GetDuration(Mathf.Abs(end - start));

                        result = from.DOValue(end, duration, Snapping);
                    }
                }
                    break;
            }

            if (result != null && FromTarget != null)
            {
                result.SetAutoKill(true).SetTarget(FromTarget.gameObject).SetLoops(Loops, LoopType).SetUpdate(UpdateType);
                if (Delay > 0)
                {
                    result.SetDelay(Delay);
                }

                if (CurveOrEase)
                {
                    result.SetEase(EaseCurve);
                }
                else
                {
                    result.SetEase(Ease);
                }

                if (OnPlay != null)
                {
                    result.OnPlay(OnPlay.Invoke);
                }

                if (OnUpdate != null)
                {
                    result.OnUpdate(OnUpdate.Invoke);
                }

                if (OnComplete != null)
                {
                    result.OnComplete(OnComplete.Invoke);
                }
            }

            return result;
        }

        private float GetDuration(float CalculatedValue)
        {
            if (SpeedBased)
            {
                return CalculatedValue / DurationOrSpeed;
            }

            return DurationOrSpeed;
        }

        private float GetEulerAngle(Vector3 to, Vector3 from)
        {
            var delta = from - to;
            delta.x = Mathf.DeltaAngle(to.x, from.x);
            delta.y = Mathf.DeltaAngle(to.y, from.y);
            delta.z = Mathf.DeltaAngle(to.z, from.z);

            var angle = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);
            return (angle + 360) % 360;
        }
    }
}