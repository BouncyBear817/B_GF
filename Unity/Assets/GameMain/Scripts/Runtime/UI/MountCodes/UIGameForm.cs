using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace GameMain.UI
{
	/// <summary>
	/// Please modify the description.
	/// </summary>
	public partial class UIGameForm : BearUIForm
	{
		private SudokuBoard mSudokuBoard;
		private SudokuGrid mSudokuGrid;
		private SudokuSubGrid mSudokuSubGrid;
		private SudokuCell mSudokuCell;
		
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			mBtnHome.onClick.AddListener(BtnHomeEvent);
			mBtnSetting.onClick.AddListener(BtnSettingEvent);
			#endregion
			
			AddListener(UIMsgId.GamePlay, OnGamePlay);
			
			mSudokuGrid = mTransGameArea.GetOrAddComponent<SudokuGrid>();
			mSudokuBoard = mTransGameArea.GetOrAddComponent<SudokuBoard>();
			mSudokuBoard.SetGrid(mSudokuGrid);
			mSudokuBoard.SetDifficultLevel(40);
			mSudokuSubGrid = mTransGameArea.Find("Grid").GetOrAddComponent<SudokuSubGrid>();
			mSudokuSubGrid.gameObject.SetActive(false);
			mSudokuCell = mTransGameArea.Find("Grid/Cell").GetOrAddComponent<SudokuCell>();
			mSudokuCell.gameObject.SetActive(false);

			MainEntry.Coroutine.DoCoroutine(GenerateGrid());
		}
		
		private void BtnHomeEvent()
		{
		}

		private void BtnSettingEvent()
		{
		}
		
		private void OnGamePlay(params object[] args)
		{
			var level = args[0] as string;
			var number = (int)args[1];
			
		}

		private IEnumerator GenerateGrid()
		{
			yield return mSudokuGrid.GenerateGrid(mSudokuSubGrid, mSudokuCell);
			
			mSudokuBoard.Init();
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
