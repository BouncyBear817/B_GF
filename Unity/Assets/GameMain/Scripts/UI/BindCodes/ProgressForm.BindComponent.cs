using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class ProgressForm
	{
		private Text mMessage;
		private Slider mProgress;

		private void GetBindComponents(GameObject go)
		{
			mUIFormInfo = new Constant.UI.UIFormInfo("ProgressForm", Constant.EUIGroupName.AlwaysBottomUI, false, false);
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mMessage = uiAutoBindTool.GetBindComponent<Text>(0);
			mProgress = uiAutoBindTool.GetBindComponent<Slider>(1);
		}
	}
}
