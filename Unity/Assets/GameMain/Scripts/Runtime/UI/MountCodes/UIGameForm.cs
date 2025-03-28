using System.Collections;
using GameFramework;
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
		private DTLevelTable mLevelTable;

		private bool mIsGridGenerated = false;
		private float mLevelStartTime = 0f;
		
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			mBtnHome.onClick.AddListener(BtnHomeEvent);
			mBtnSetting.onClick.AddListener(BtnSettingEvent);
			mBtnNew.onClick.AddListener(BtnNewEvent);
			mBtnRestart.onClick.AddListener(BtnRestartEvent);
			mBtnSubmit.onClick.AddListener(BtnSubmitEvent);
			mBtnTipNext.onClick.AddListener(BtnTipNextEvent);
			mBtnTipBack.onClick.AddListener(BtnTipBackEvent);
			mBtnCheck.onClick.AddListener(BtnCheckEvent);
			#endregion
			
			AddListener(UIMsgId.OpenInputPanel, OnOpenInputPanel);
			
			transform.Find("Mask").GetComponent<Button>().onClick.AddListener(OnMaskClick);
			
			mSudokuGrid = mTransGameArea.GetOrAddComponent<SudokuGrid>();
			mSudokuBoard = mTransGameArea.GetOrAddComponent<SudokuBoard>();
			mSudokuBoard.SetGrid(mSudokuGrid);
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

			MainEntry.Coroutine.DoCoroutine(GenerateGrid(userData));
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			
			if (mIsGridGenerated)
			{
				SetDifficult(userData);
				mSudokuBoard.Init();
				mLevelStartTime = Time.realtimeSinceStartup;
			}
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			var t = Time.realtimeSinceStartup - mLevelStartTime;
			var seconds = (int)(t % 60);
			t /= 60;
			var minutes = (int)(t % 60);
			mTMTimer.text = $"{minutes:00}:{seconds:00}";
		}

		private void BtnHomeEvent()
		{
			mSudokuBoard.Clear();
			MainEntry.UI.OpenUIForm(UIViews.UIInitRootForm);
			Close();
			Clear();
		}

		private void BtnSettingEvent()
		{
		}

		private void BtnNewEvent()
		{
			if (mLevelTable != null)
			{
				var number = Utility.Random.GetRandom(mLevelTable.StartIndex, mLevelTable.EndIndex);
				mSudokuBoard.SetDifficultLevel(number);
			}
			
			mSudokuBoard.Reset();
			mLevelStartTime = Time.realtimeSinceStartup;
		}

		private void BtnSubmitEvent()
		{
			var completed = mSudokuBoard.CheckCompleted();
			var message = completed ? "Success" : "Failed";
			MainEntry.UI.OpenUIForm(UIViews.DialogForm, new DialogParams("Check Complete", message, "ok", o =>
			{
				BtnNewEvent();
			}));
		}

		private void BtnRestartEvent()
		{
			mSudokuBoard.Restart();
			mLevelStartTime = Time.realtimeSinceStartup;
		}

		private void BtnTipNextEvent()
		{
			mSudokuBoard.TipNext();
		}

		private void BtnTipBackEvent()
		{
			mSudokuBoard.TipBack();
		}

		private void BtnCheckEvent()
		{
			mSudokuBoard.Check();
		}

		private void SetDifficult(object userData)
		{
			var levelTable = userData as DTLevelTable;
			if (levelTable != null)
			{
				mLevelTable = levelTable;
				var number = Utility.Random.GetRandom(mLevelTable.StartIndex, mLevelTable.EndIndex);
				mSudokuBoard.SetDifficultLevel(number);
			}
		}

		private IEnumerator GenerateGrid(object userData)
		{
			yield return mSudokuGrid.GenerateGrid(mSudokuSubGrid, mSudokuCell);

			mIsGridGenerated = true;
			SetDifficult(userData);
			mSudokuBoard.Init();
			mLevelStartTime = Time.realtimeSinceStartup;
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
			mSudokuBoard.UpdatePuzzle(mCurrentCell.Coordinate.x, mCurrentCell.Coordinate.y, number);
			
			mTransInputGrid.gameObject.SetActive(false);
			mCurrentCell = null;
		}

		private void OnMaskClick()
		{
			mTransInputGrid.gameObject.SetActive(false);
		}

		private void Clear()
		{
			mLevelStartTime = 0;
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
