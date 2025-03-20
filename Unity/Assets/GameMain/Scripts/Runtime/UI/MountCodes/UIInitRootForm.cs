using System.Collections.Generic;
using GameFramework;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain.UI
{
	/// <summary>
	/// Please modify the description.
	/// </summary>
	public partial class UIInitRootForm: BearUIForm
	{
		private List<DTLevelTable> mLevelTables = new List<DTLevelTable>();
		private DTLevelTable mSelectedLevelTable;
		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GetBindComponents(gameObject);

			#region Auto Generate,Do not modify!
			mBtnPlay.onClick.AddListener(BtnPlayEvent);
			mDrLevel.onValueChanged.AddListener(DrLevelEvent);
			#endregion

			AddDropDownOptions();
			mSelectedLevelTable = mLevelTables[0];
		}

		private void BtnPlayEvent()
		{
			//TODO: need msdId
			var level = mSelectedLevelTable.DiffcultLevel;
			var number = Utility.Random.GetRandom(mSelectedLevelTable.StartIndex, mSelectedLevelTable.EndIndex);
			Broadcast(UIMsgId.GamePlay, level, number);
			Log.Info($"Start Sudoku : level '{level}', number '{number}'.");
			MainEntry.UI.OpenUIForm(UIViews.UIGameForm);
		}

		private void DrLevelEvent(int index)
		{
			var difficultLevel = mDrLevel.options[index];
			mSelectedLevelTable = mLevelTables.Find((levelTable => levelTable.DiffcultLevel == difficultLevel.text));
			
			Log.Info(mSelectedLevelTable.DiffcultLevel);
		}

		private void AddDropDownOptions()
		{
			MainEntry.DataTable.GetDataTable<DTLevelTable>().GetAllDataRows(mLevelTables);
			
			foreach (var levelTable in mLevelTables)
			{
				var option = new TMP_Dropdown.OptionData(levelTable.DiffcultLevel);
				if (!mDrLevel.options.Contains(option))
				{
					mDrLevel.options.Add(option);
				}
			}
		}

/*--------------------Auto generate footer.Do not add anything below the footer!------------*/
	}
}
