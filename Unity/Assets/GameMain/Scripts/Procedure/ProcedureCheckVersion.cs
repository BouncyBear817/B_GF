// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 14:22:49
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Text;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool mCheckVersionComplete = false;
        private bool mNeedUpdateVersion = false;
        // private VersionInfo mVersionInfo;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Log.Fatal("The device is not connected to the network. Please Check it.");
                return;
            }

            mCheckVersionComplete = false;
            mNeedUpdateVersion = false;

            MainEntry.Resource.UpdatePrefixUri = SettingsUtils.GetResourceServerPath();

            MainEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            MainEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            WebRequestVersionList();
        }

        private void WebRequestVersionList()
        {
            var versionListPath = SettingsUtils.GetVersionListPath();
            Log.Info(versionListPath);
            //向服务器请求版本信息
            MainEntry.WebRequest.AddWebRequest(versionListPath);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!mCheckVersionComplete)
            {
                return;
            }

            if (mNeedUpdateVersion)
            {
            }
            else
            {
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            MainEntry.Event.Unsubscribe(UnityGameFramework.Runtime.WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            MainEntry.Event.Unsubscribe(UnityGameFramework.Runtime.WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        private void OnWebRequestSuccess(object sender, BaseEventArgs e)
        {
            var webRequest = e as WebRequestSuccessEventArgs;
            if (webRequest != null)
            {
                Log.Info(Encoding.UTF8.GetString(webRequest.GetWebResponseBytes()));
            }
        }

        private void OnWebRequestFailure(object sender, BaseEventArgs e)
        {
            var eventArgs = e as WebRequestFailureEventArgs;
            if (eventArgs != null)
            {
                Log.Error(eventArgs.ErrorMessage);
            }
        }
    }
}