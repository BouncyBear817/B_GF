/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/4/19 9:57:29
 * Description:
 * Modify Record:
 *************************************************************/

using System.Collections;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
                if (mUICamera == null && MainEntry.UI != null)
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

        private static IUIManager mUIManager;

        public static IUIManager UIManager
        {
            get
            {
                if (mUIManager == null)
                {
                    mUIManager = GameFrameworkEntry.GetModule<IUIManager>();
                }

                return mUIManager;
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

        public static void CloseUIForm(this UIComponent uiComponent, BUIForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
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

        public static Transform GetUIGroupRoot(this UIComponent uiComponent, Constant.EUIGroup uiGroup)
        {
            var group = uiComponent.GetUIGroup(uiGroup.ToString());
            if (group != null)
            {
                var helper = group.Helper as BUIGroupHelper;
                if (helper != null)
                {
                    return helper.transform;
                }
            }

            return null;
        }

        public static IUIFormHelper GetUIFormHelper(this UIComponent uiComponent)
        {
            return uiComponent.GetComponentInChildren<IUIFormHelper>();
        }

        public static void SetAnchoredPositionX(this RectTransform rectTransform, float anchoredPositionX)
        {
            var value = rectTransform.anchoredPosition;
            value.x = anchoredPositionX;
            rectTransform.anchoredPosition = value;
        }

        public static void SetAnchoredPositionY(this RectTransform rectTransform, float anchoredPositionY)
        {
            var value = rectTransform.anchoredPosition;
            value.y = anchoredPositionY;
            rectTransform.anchoredPosition = value;
        }

        public static void SetAnchoredPosition3DZ(this RectTransform rectTransform, float anchoredPositionZ)
        {
            var value = rectTransform.anchoredPosition3D;
            value.z = anchoredPositionZ;
            rectTransform.anchoredPosition3D = value;
        }

        public static void SetColorAlpha(this Graphic graphic, float alpha)
        {
            var value = graphic.color;
            value.a = alpha;
            graphic.color = value;
        }

        public static Vector2 GetFlexibleSize(this LayoutElement layoutElement)
        {
            return new Vector2(layoutElement.flexibleWidth, layoutElement.flexibleHeight);
        }

        public static void SetFlexibleSize(this LayoutElement layoutElement, Vector2 flexibleSize)
        {
            layoutElement.flexibleWidth = flexibleSize.x;
            layoutElement.flexibleHeight = flexibleSize.y;
        }

        public static Vector2 GetMinSize(this LayoutElement layoutElement)
        {
            return new Vector2(layoutElement.minWidth, layoutElement.minHeight);
        }
        
        public static void SetMinSize(this LayoutElement layoutElement, Vector2 size)
        {
            layoutElement.minWidth = size.x;
            layoutElement.minHeight = size.y;
        }
        
        public static Vector2 GetPreferredSize(this LayoutElement layoutElement)
        {
            return new Vector2(layoutElement.preferredWidth, layoutElement.preferredHeight);
        }
        
        public static void SetPreferredSize(this LayoutElement layoutElement, Vector2 size)
        {
            layoutElement.preferredWidth = size.x;
            layoutElement.preferredHeight = size.y;
        }

        public static void SetActive(this RectTransform transform, bool active)
        {
            transform.gameObject.SetActive(active);
        }
        
        public static void SetActive(this Button button, bool active)
        {
            button.gameObject.SetActive(active);
        }

        public static void SetActive(this Image image, bool active)
        {
            image.gameObject.SetActive(active);
        }
        
        public static void SetActive(this RawImage image, bool active)
        {
            image.gameObject.SetActive(active);
        }
        
        public static void SetActive(this Toggle toggle, bool active)
        {
            toggle.gameObject.SetActive(active);
        }

        public static void SetActive(this TextMeshProUGUI textMeshProUGUI, bool active)
        {
            textMeshProUGUI.gameObject.SetActive(active);
        }
        
        public static Sprite ToSprite(this Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public static void RefreshContentSize(this RectTransform rect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
        
        public static IEnumerator CaptureUIPanel(RectTransform targetPanel, byte[] imageBytes)
        {
            // 等待当前帧渲染完全结束
            yield return new WaitForEndOfFrame();

            // 2. 获取UI区域的世界坐标角点并转换为屏幕坐标
            var worldCorners = new Vector3[4];
            targetPanel.GetWorldCorners(worldCorners);
            var bottomLeft = RectTransformUtility.WorldToScreenPoint(UICamera, worldCorners[0]);
            var topRight = RectTransformUtility.WorldToScreenPoint(UICamera, worldCorners[2]);

            // 3. 计算截取区域的宽高
            var width = (int)(topRight.x - bottomLeft.x);
            var height = (int)(topRight.y - bottomLeft.y);
            var rect = new Rect(bottomLeft.x, bottomLeft.y, width, height);

            // 4. 创建Texture2D（使用支持Alpha的格式）
            var screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

            // 5. 读取像素
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            // 6. 保存（示例）
            var file = @"C:\Users\RZ\Documents\CameraScreenShot.png";
            var bytes = screenShot.EncodeToPNG();
            imageBytes = bytes;
            System.IO.File.WriteAllBytes(file, bytes);
            Debug.Log("截图保存成功！");

            // 7. 清理
            Object.Destroy(screenShot);
        }
        
         /// <summary>
        /// 用于检测是否点击到UI元素
        /// </summary>
        /// <param name="uiComponent">UI组件</param>
        /// <returns>是否点击到UI元素</returns>
#if UNITY_IOS || UNITY_ANDROID
        public static bool IsPointerOverUIObject(this UIComponent uiComponent, Vector3 mousePosition)
        {

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(mousePosition.x, mousePosition.y)
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
#else
        public static bool IsPointerOverUIObject(this UIComponent uiComponent)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
#endif

        private static IDataTable<DTUITable> GetUITable()
        {
            if (!MainEntry.DataTable.HasDataTable<DTUITable>())
            {
                Log.Error("UI Table is empty.");
                return null;
            }

            return MainEntry.DataTable.GetDataTable<DTUITable>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiComponent"></param>
        /// <param name="uiViews"></param>
        /// <returns></returns>
        private static string GetUIFormAssetName(this UIComponent uiComponent, UIViews uiViews)
        {
            var uiTable = GetUITable();
            if (uiTable != null)
            {
                var uiId = (int)uiViews;
                if (uiTable.HasDataRow(uiId))
                {
                    return AssetUtil.GetUIFormAsset(uiTable.GetDataRow(uiId).AssetName);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 打开UI界面
        /// </summary>
        /// <param name="uiComponent">UI界面组件</param>
        /// <param name="uiViews">UI界面Id</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>界面的序列编号</returns>
        public static int OpenUIForm(this UIComponent uiComponent, UIViews uiViews, object userData = null)
        {
            var uiTable = GetUITable();
            if (uiTable != null)
            {
                var uiId = (int)uiViews;
                if (!uiTable.HasDataRow(uiId))
                {
                    Log.Error($"UI Table 不存在id : {uiId}.");
                    return -1;
                }

                var uiRow = uiTable.GetDataRow(uiId);
                var uiAssetName = uiComponent.GetUIFormAssetName(uiViews);

                if (uiRow.PauseCovered)
                {
                    
                }
                if (uiComponent.IsLoadingUIForm(uiAssetName))
                {
                    return -1;
                }

                if (uiComponent.HasUIForm(uiAssetName))
                {
                    uiComponent.GetUIForm(uiViews).OnOpen(userData);
                    return -1;
                }

                return uiComponent.OpenUIForm(uiAssetName, uiRow.GroupName, uiRow.Priority, uiRow.PauseCovered, userData);
            }

            return -1;
        }

        public static bool HasUIForm(this UIComponent uiComponent, UIViews uiViews)
        {
            var uiAssetName = uiComponent.GetUIFormAssetName(uiViews);
            return uiComponent.HasUIForm(uiAssetName);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, UIViews uiViews)
        {
            var uiAssetName = uiComponent.GetUIFormAssetName(uiViews);
            if (uiComponent.HasUIForm(uiAssetName))
            {
                return uiComponent.GetUIForm(uiAssetName);
            }

            return null;
        }
        
        public static bool IsLoadingUIForm(this UIComponent uiComponent, UIViews uiViews)
        {
            var uiAssetName = uiComponent.GetUIFormAssetName(uiViews);
            return uiComponent.IsLoadingUIForm(uiAssetName);
        }
        
        public static void CloseUIForm(this UIComponent uiComponent, UIViews uiViews)
        {
            var uiAssetName = uiComponent.GetUIFormAssetName(uiViews);
            if (uiComponent.HasUIForm(uiAssetName))
            {
                var uiForm = uiComponent.GetUIForm(uiAssetName);
                uiComponent.CloseUIForm(uiForm);
            }
        }
    }
}