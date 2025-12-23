using System;
using System.Collections.Generic;

namespace GameMain
{
    public class DataManager : SingletonMono<DataManager>
    {
        private List<IDataInfo> mDataInfoList = new List<IDataInfo>();

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            foreach (var dataInfo in mDataInfoList)
            {
                dataInfo.OnInit();
            }
        }

        private void OnDestroy()
        {
            foreach (var dataInfo in mDataInfoList)
            {
                dataInfo.OnDispose();
            }

            if (Instance != null)
            {
                OnClear();
            }
        }
    }
}