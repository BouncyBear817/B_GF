using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

		private SudokuCell mCurrentCell;
		
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			mBtnHome.onClick.AddListener(BtnHomeEvent);
			mBtnSetting.onClick.AddListener(BtnSettingEvent);
			#endregion
			
			AddListener(UIMsgId.GamePlay, OnGamePlay);
			AddListener(UIMsgId.OpenInputPanel, OnOpenInputPanel);
			
			transform.Find("Mask").GetComponent<Button>().onClick.AddListener(OnMaskClick);
			
			mSudokuGrid = mTransGameArea.GetOrAddComponent<SudokuGrid>();
			mSudokuBoard = mTransGameArea.GetOrAddComponent<SudokuBoard>();
			mSudokuBoard.SetGrid(mSudokuGrid);
			mSudokuBoard.SetDifficultLevel(40);
			mSudokuSubGrid = mTransGameArea.Find("Grid").GetOrAddComponent<SudokuSubGrid>();
			mSudokuSubGrid.gameObject.SetActive(false);
			mSudokuCell = mTransGameArea.Find("Grid/Cell").GetOrAddComponent<SudokuCell>();
			mSudokuCell.gameObject.SetActive(false);
			
			mTransInputGrid.gameObject.SetActive(false);
			foreach (var inputButton in mTransInputGrid.GetComponentsInChildren<Button>())
			{
				var number = inputButton.transform.Find("Number").GetComponent<TextMeshProUGUI>();
				inputButton.onClick.AddListener((() => OnInputButton(int.Parse(number.text))));
			}

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

		private void OnOpenInputPanel(object[] args)
		{
			var uiPosition = mTransInputGrid.parent.GetComponent<RectTransform>().ScreenPointToUIPoint(Input.mousePosition);
			var sizeData = mTransInputGrid.GetComponent<RectTransform>().sizeDelta;
			var pos = uiPosition.x > 0 ? new Vector2(uiPosition.x - sizeData.x / 2, uiPosition.y - sizeData.y / 2) : new Vector2(uiPosition.x + sizeData.x / 2, uiPosition.y - sizeData.y / 2);
			mTransInputGrid.transform.localPosition = pos;

			var cell = args[0] as SudokuCell;
			if (cell != null)
			{
				mCurrentCell = cell;
			}

			mTransInputGrid.gameObject.SetActive(true);
		}

		private void OnInputButton(int number)
		{
			mCurrentCell.SetValue(number);
			
			mTransInputGrid.gameObject.SetActive(false);
		}

		private void OnMaskClick()
		{
			mTransInputGrid.gameObject.SetActive(false);
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
