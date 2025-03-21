using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class UIGameForm
	{
		private Button mBtnHome;
		private Button mBtnSetting;
		private RectTransform mTransGameArea;
		private TextMeshProUGUI mTMTimer;
		private RectTransform mTransInputGrid;

		private void GetBindComponents(GameObject go)
		{
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mBtnHome = uiAutoBindTool.GetBindComponent<Button>(0);
			mBtnSetting = uiAutoBindTool.GetBindComponent<Button>(1);
			mTransGameArea = uiAutoBindTool.GetBindComponent<RectTransform>(2);
			mTMTimer = uiAutoBindTool.GetBindComponent<TextMeshProUGUI>(3);
			mTransInputGrid = uiAutoBindTool.GetBindComponent<RectTransform>(4);
		}
	}
}
