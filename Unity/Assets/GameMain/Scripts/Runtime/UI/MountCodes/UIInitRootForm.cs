using System.Collections.Generic;
using TMPro; 

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
			MainEntry.UI.OpenUIForm(UIViews.UIGameForm, mSelectedLevelTable);
			Close();
		}

		private void DrLevelEvent(int index)
		{
			var difficultLevel = mDrLevel.options[index];
			mSelectedLevelTable = mLevelTables.Find((levelTable => levelTable.DiffcultLevel == difficultLevel.text));
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
