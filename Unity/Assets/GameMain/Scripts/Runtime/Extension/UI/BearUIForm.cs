/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/04/18 15:21:28
 * Description:
 * Modify Record:
 *************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public abstract class BearUIForm : UIFormLogic, IMessenger
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font sMainFont = null;
        private Canvas mCachedCanvas = null;
        private CanvasGroup mCanvasGroup = null;

        public int OriginDepth { get; private set; }

        public int Depth => mCachedCanvas.sortingOrder;

        public void Close(bool ignoreFade = false)
        {
            StopAllCoroutines();
            if (ignoreFade)
            {
                MainEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseFade(FadeTime));
            }
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main Font is invalid.");
                return;
            }

            sMainFont = mainFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            mCachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            mCachedCanvas.overrideSorting = true;
            OriginDepth = mCachedCanvas.sortingOrder;

            mCanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            var trans = GetComponent<RectTransform>();
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.one;
            trans.anchoredPosition = Vector2.zero;
            trans.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            var texts = GetComponentsInChildren<Text>(true);
            foreach (var t in texts)
            {
                t.font = sMainFont;
            }
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            StopAllCoroutines();
            StartCoroutine(OpenFade(FadeTime));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            StopAllCoroutines();
            StartCoroutine(OpenFade(FadeTime));
        }

        protected override void OnCover()
        {
            base.OnCover();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);

            var oldDepth = Depth;
            var deltaDepth = BearUIGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginDepth;

            foreach (var canvas in GetComponentsInChildren<Canvas>(true))
            {
                canvas.sortingOrder += deltaDepth;
            }
        }

        private IEnumerator OpenFade(float duration)
        {
            mCanvasGroup.alpha = 0;
            yield return mCanvasGroup.FadeToAlpha(1f, duration);
        }

        private IEnumerator CloseFade(float duration)
        {
            yield return mCanvasGroup.FadeToAlpha(0f, duration);
            MainEntry.UI.CloseUIForm(this);
        }

        public void Init(object userData)
        {
            OnInit(userData);
        }

        public void Recycle()
        {
            OnRecycle();
        }

        public void Open(object userData)
        {
            OnOpen(userData);
        }

        public void Close(bool isShutdown, object userData)
        {
            OnClose(isShutdown, userData);
        }

        public void Broadcast(uint msgId, params object[] args)
        {
            MainEntry.Messenger.Broadcast(msgId, args);
        }

        public void AddListener(uint msgId, MessageEvent messageEvent)
        {
            MainEntry.Messenger.AddListener(msgId, messageEvent);
        }

        public void RemoveListener(uint msgId, MessageEvent messageEvent)
        {
            MainEntry.Messenger.RemoveListener(msgId, messageEvent);
        }

        public void RemoveAll(uint msgId)
        {
            MainEntry.Messenger.RemoveAll(msgId);
        }
    }
}