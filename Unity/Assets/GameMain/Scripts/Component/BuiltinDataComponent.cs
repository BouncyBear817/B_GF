// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/22 15:10:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using GameFramework;
using GameMain.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace GameMain
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField] private DialogForm mDialogForm;

        [SerializeField] private ProgressForm mProgressForm;


        public DialogForm DialogForm => mDialogForm;

        public ProgressForm ProgressForm => mProgressForm;

        public void InitBuiltinForm()
        {
            mProgressForm = Object.Instantiate<ProgressForm>(mProgressForm, MainEntry.UI.GetUIGroupRoot(Constant.EUIGroupName.Default));
            mProgressForm.Init(null);
            mProgressForm.gameObject.SetActive(false);

            mDialogForm = Object.Instantiate<DialogForm>(mDialogForm, MainEntry.UI.GetUIGroupRoot(Constant.EUIGroupName.Default));
            mDialogForm.Init(null);
            mDialogForm.gameObject.SetActive(false);
        }

        public void ShowDialog(DialogParams dialogParams)
        {
            mDialogForm.Open(dialogParams);
        }

        public void HideDialog()
        {
            mDialogForm.Close(false, null);
        }

        public void ShowProgress(string message, float progress)
        {
            if (!mProgressForm.gameObject.activeSelf)
            {
                mProgressForm.Open(null);
            }

            mProgressForm.SetProgress(message, progress);
        }

        public void HideProgress()
        {
            mProgressForm.Close(false, null);
        }
    }
}