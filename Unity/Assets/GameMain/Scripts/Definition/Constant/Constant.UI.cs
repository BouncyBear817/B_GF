// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/09/19 10:09:35
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;

namespace GameMain
{
    public static partial class Constant
    {
        public static partial class UI
        {
            public static Dictionary<EUIGroupName, int> UIGroupMap = new Dictionary<EUIGroupName, int>()
            {
                { EUIGroupName.AlwaysBottomUI, 1000 },
                { EUIGroupName.BackgroundUI, 2000 },
                { EUIGroupName.CommonUI, 3000 },
                { EUIGroupName.AnimationOnUI, 4000 },
                { EUIGroupName.PopupUI, 5000 },
                { EUIGroupName.GuideUI, 6000 }
            };
        }
    }
}