using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class UIGameForm
	{
		private Button mBtnHome;
		private Button mBtnSetting;
		private Button mBtnNew;
		private Button mBtnRestart;
		private Button mBtnSubmit;
		private Button mBtnTipNext;
		private Button mBtnTipBack;
		private Button mBtnCheck;
		private RectTransform mTransGameArea;
		private TextMeshProUGUI mTMTimer;
		private RectTransform mTransInputGrid;

		private void GetBindComponents(GameObject go)
		{
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mBtnHome = uiAutoBindTool.GetBindComponent<Button>(0);
			mBtnSetting = uiAutoBindTool.GetBindComponent<Button>(1);
			mBtnNew = uiAutoBindTool.GetBindComponent<Button>(2);
			mBtnRestart = uiAutoBindTool.GetBindComponent<Button>(3);
			mBtnSubmit = uiAutoBindTool.GetBindComponent<Button>(4);
			mBtnTipNext = uiAutoBindTool.GetBindComponent<Button>(5);
			mBtnTipBack = uiAutoBindTool.GetBindComponent<Button>(6);
			mBtnCheck = uiAutoBindTool.GetBindComponent<Button>(7);
			mTransGameArea = uiAutoBindTool.GetBindComponent<RectTransform>(8);
			mTMTimer = uiAutoBindTool.GetBindComponent<TextMeshProUGUI>(9);
			mTransInputGrid = uiAutoBindTool.GetBindComponent<RectTransform>(10);
		}
	}
}
