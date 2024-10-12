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
            /// 界面信息
            /// </summary>
            public class UIFormInfo
            {
                /// <summary>
                /// 界面类型
                /// </summary>
                public EUIFormType UIFormType { get; }

                /// <summary>
                /// 模块名称
                /// </summary>
                public string ModuleName { get; }

                /// <summary>
                /// 资源名称
                /// </summary>
                public string AssetName { get; }

                /// <summary>
                /// 所属资源组名称
                /// </summary>
                public EUIGroupName UIGroupName { get; }

                /// <summary>
                /// 是否允许多个实例
                /// </summary>
                public bool AllowMultiInstance { get; }

                /// <summary>
                /// 是否暂停被其覆盖的界面
                /// </summary>
                public bool PauseCoveredUIForm { get; }

                public UIFormInfo(EUIFormType uiFormType, string moduleName, string assetName, EUIGroupName uiGroupName, bool allowMultiInstance, bool pauseCoveredUIForm)
                {
                    UIFormType = uiFormType;
                    ModuleName = moduleName;
                    AssetName = assetName;
                    UIGroupName = uiGroupName;
                    AllowMultiInstance = allowMultiInstance;
                    PauseCoveredUIForm = pauseCoveredUIForm;
                }
            }
        }
    }
}