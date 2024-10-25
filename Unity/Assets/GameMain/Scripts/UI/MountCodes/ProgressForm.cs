using UnityEngine;

namespace GameMain.UI
{
	/// <summary>
	/// Please modify the description.
	/// </summary>
	public partial class ProgressForm: BearUIForm
	{
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			mProgress.onValueChanged.AddListener(ProgressEvent);
			#endregion
		}

		private void SlProgressEvent(float value)
		{
		}

		public void SetProgress(string message, float value)
		{
			mMessage.text = message;
			mProgress.value = value;
		}

		private void ProgressEvent(float value)
		{
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
