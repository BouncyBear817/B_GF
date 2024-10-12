/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/19 9:57:29
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// UI扩展
    /// </summary>
    public static class UIExtension
    {
        private static Transform mInstanceRoot;

        public static Transform InstanceRoot
        {
            get
            {
                if (mInstanceRoot == null)
                {
                    mInstanceRoot = MainEntry.UI.transform.Find("UI Form Instances");
                }

                return mInstanceRoot;
            }
        }

        private static Camera mUICamera;

        public static Camera UICamera
        {
            get
            {
                if (mUICamera == null && MainEntry.UI != null && MainEntry.UI.transform != null)
                {
                    var cameraTransform = MainEntry.UI.transform.Find("UICamera");
                    if (cameraTransform != null)
                    {
                        mUICamera = cameraTransform.GetComponent<Camera>();
                    }
                }

                return mUICamera;
            }
        }

        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            var time = 0f;
            var originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            var time = 0f;
            var originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static void CloseUIForm(this UIComponent uiComponent, BearUIForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static void CreateUIGroups(this UIComponent uiComponent)
        {
            foreach (var (euiGroupName, value) in Constant.UI.UIGroupMap)
            {
                uiComponent.AddUIGroup(euiGroupName.ToString(), value);
            }
        }

        /// <summary>
        /// 适配挖孔屏与刘海屏
        /// </summary>
        /// <param name="topBar"></param>
        public static void FitHoleScreen(RectTransform topBar)
        {
            if (topBar == null)
            {
                return;
            }

            var topSpace = Screen.height - Screen.safeArea.height;
            if (topSpace < 1f)
            {
                return;
            }

#if UNITY_IOS
            topSpace = 80;
#endif
            var pos = topBar.anchoredPosition;
            pos.y = -topSpace;
            topBar.anchoredPosition = pos;
        }
    }
}