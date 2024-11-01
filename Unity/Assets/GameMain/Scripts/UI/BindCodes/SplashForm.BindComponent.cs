using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameMain.UI
{
	public partial class SplashForm
	{
		private Image mISplash;

		private void GetBindComponents(GameObject go)
		{
			mUIFormInfo = new Constant.UI.UIFormInfo("SplashForm", Constant.EUIGroupName.Default, false, false);
			var uiAutoBindTool = go.GetComponent<UIAutoBindTool>();

			mISplash = uiAutoBindTool.GetBindComponent<Image>(0);
		}
	}
}
