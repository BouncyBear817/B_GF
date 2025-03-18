using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class UIInitRootForm
	{
		private RectTransform mTransLaunchView;
		private RawImage mRILaunchView;
		private Button mBtnPlay;
		private TMP_Dropdown mDrLevel;

		private void GetBindComponents(GameObject go)
		{
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mTransLaunchView = uiAutoBindTool.GetBindComponent<RectTransform>(0);
			mRILaunchView = uiAutoBindTool.GetBindComponent<RawImage>(1);
			mBtnPlay = uiAutoBindTool.GetBindComponent<Button>(2);
			mDrLevel = uiAutoBindTool.GetBindComponent<TMP_Dropdown>(3);
		}
	}
}
