// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/09/19 10:09:35
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace GameMain
{
    public static partial class Constant
    {
        public static partial class UI
        {
            /// <summary>
            /// 界面类型
            /// </summary>
            public enum EUIFormType
            {
                /// <summary>
                /// 独立主界面
                /// </summary>
                MainForm = 0,

                /// <summary>
                /// 独立主界面下子界面
                /// </summary>
                SubForm = 1,

                /// <summary>
                /// 公共子界面
                /// </summary>
                CommonSubForm = 2
            }
        }
    }
}