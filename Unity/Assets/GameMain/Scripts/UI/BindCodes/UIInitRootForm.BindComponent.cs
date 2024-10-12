using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class UIInitRootForm
	{
		private RectTransform mTransLaunchView;
		private RawImage mRILaunchView;

		private void GetBindComponents(GameObject go)
		{
			mUIFormInfo = new Constant.UI.UIFormInfo(Constant.UI.EUIFormType.MainForm, "Base", "UIInitRootForm", Constant.UI.EUIGroupName.BackgroundUI, false, true);
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mTransLaunchView = uiAutoBindTool.GetBindComponent<RectTransform>(0);
			mRILaunchView = uiAutoBindTool.GetBindComponent<RawImage>(1);
		}
	}
}
