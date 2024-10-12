// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 14:21:43
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedurePreload : ProcedureBase
    {
        private Dictionary<string, bool> mLoadedFlag = new Dictionary<string, bool>();

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            MainEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            MainEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            MainEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            MainEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

            mLoadedFlag.Clear();

            PreloadResources();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            foreach (var (_, isLoaded) in mLoadedFlag)
            {
                if (!isLoaded)
                {
                    return;
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            MainEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            MainEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFailure);
            MainEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            MainEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        }

        private void PreloadResources()
        {
        }

        private void OnLoadConfigSuccess(object sender, BaseEventArgs e)
        {
        }

        private void OnLoadConfigFailure(object sender, BaseEventArgs e)
        {
        }

        private void OnLoadDataTableSuccess(object sender, BaseEventArgs e)
        {
        }

        private void OnLoadDataTableFailure(object sender, BaseEventArgs e)
        {
        }
    }
}