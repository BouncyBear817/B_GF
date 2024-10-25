// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/25 10:10:23
//  * Description:
//  * Modify Record:
//  *************************************************************/

using GameFramework;

namespace GameMain
{
    public class BearVersionHelper : Version.IVersionHelper
    {
        public string GameVersion => SettingsUtils.GameGlobalSettings.ApplicableGameVersion;
        public int InternalGameVersion => SettingsUtils.GameGlobalSettings.InternalResourceVersion;
    }
}