using System.Collections;
using GameFramework;
using UnityEngine;

namespace GameMain.UI
{
	/// <summary>
	/// Please modify the description.
	/// </summary>
	public partial class SplashForm: BearUIForm
	{
		private GameFrameworkAction mCompleteAction;
		private Animation mAnimation;
		
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			#endregion
			
			mAnimation = mISplash.GetComponent<Animation>();
		}

		public void SetBackground(Sprite sprite)
		{
			mISplash.sprite = sprite;
		}

		public IEnumerator StartSplash(GameFrameworkAction completeAction)
		{
			mCompleteAction = completeAction;
			
			mISplash.color = new Color(mISplash.color.r, mISplash.color.g, mISplash.color.b, .5f);

			mAnimation.Play();
			
			yield return new WaitForSeconds(mAnimation.clip.length);
			
			completeAction?.Invoke();
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
