using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class DialogForm
	{
		private Text mTitle;
		private Text mMessage;
		private Button mConfirm;
		private Text mConfirmText;
		private Button mCancel;
		private Text mCancelText;
		private Button mOther;
		private Text mOtherText;

		private void GetBindComponents(GameObject go)
		{
			mUIFormInfo = new Constant.UI.UIFormInfo("DialogForm", Constant.EUIGroupName.PopupUI, false, false);
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mTitle = uiAutoBindTool.GetBindComponent<Text>(0);
			mMessage = uiAutoBindTool.GetBindComponent<Text>(1);
			mConfirm = uiAutoBindTool.GetBindComponent<Button>(2);
			mConfirmText = uiAutoBindTool.GetBindComponent<Text>(3);
			mCancel = uiAutoBindTool.GetBindComponent<Button>(4);
			mCancelText = uiAutoBindTool.GetBindComponent<Text>(5);
			mOther = uiAutoBindTool.GetBindComponent<Button>(6);
			mOtherText = uiAutoBindTool.GetBindComponent<Text>(7);
		}
	}
}
