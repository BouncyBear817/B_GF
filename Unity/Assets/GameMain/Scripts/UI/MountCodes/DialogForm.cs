using System;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.UI
{
    /// <summary>
    /// Please modify the description.
    /// </summary>
    public partial class DialogForm : BearUIForm
    {
        private GameFrameworkAction<object> mOnConfirmClick = null;
        private GameFrameworkAction<object> mOnCancelClick = null;
        private GameFrameworkAction<object> mOnOtherClick = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);

            #region Auto Generate,Do not modify!

            mConfirm.onClick.AddListener(ConfirmEvent);
            mCancel.onClick.AddListener(CancelEvent);
            mOther.onClick.AddListener(OtherEvent);

            #endregion
        }

        private void ConfirmEvent()
        {
            mOnConfirmClick?.Invoke(this);
            OnClose(false, null);
        }

        private void CancelEvent()
        {
            mOnCancelClick?.Invoke(this);
            OnClose(false, null);
        }

        private void OtherEvent()
        {
            mOnOtherClick?.Invoke(this);
            OnClose(false, null);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var dialogParams = userData as DialogParams;
            if (dialogParams == null)
            {
                Log.Error("dialogParams is invalid.");
                return;
            }

            InitData();

            RefreshDialog(dialogParams);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            InitData();
        }

        private void InitData()
        {
            mConfirm.gameObject.SetActive(false);
            mCancel.gameObject.SetActive(false);
            mOther.gameObject.SetActive(false);

            mTitle.text = string.Empty;
            mMessage.text = string.Empty;

            mConfirmText.text = string.Empty;
            mCancelText.text = string.Empty;
            mOtherText.text = string.Empty;

            mOnConfirmClick = null;
            mOnCancelClick = null;
            mOnOtherClick = null;
        }

        private void RefreshDialog(DialogParams dialogParams)
        {
            mConfirm.gameObject.SetActive(dialogParams.Mode >= 1);
            mCancel.gameObject.SetActive(dialogParams.Mode >= 2);
            mOther.gameObject.SetActive(dialogParams.Mode >= 3);

            if (dialogParams.IsPauseGame)
            {
                MainEntry.Base.PauseGame();
            }

            mTitle.text = dialogParams.Title;
            mMessage.text = dialogParams.Message;

            mConfirmText.text = dialogParams.ConfirmButtonText;
            mCancelText.text = dialogParams.CancelButtonText;
            mOtherText.text = dialogParams.OtherButtonText;

            mOnConfirmClick = dialogParams.OnConfirmClick;
            mOnCancelClick = dialogParams.OnCancelClick;
            mOnOtherClick = dialogParams.OnOtherClick;
        }


/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
    }
}