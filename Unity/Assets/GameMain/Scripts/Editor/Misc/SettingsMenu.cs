// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/24 16:6:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEditor;

namespace GameMain.Editor
{
    public static class SettingsMenu
    {
        [MenuItem("Tools/Settings/UI Auto Bind Global Settings", priority = 20)]
        public static void OpenUIAutoBindGlobalSetting() => SettingsService.OpenProjectSettings("Settings/UIAutoBindGlobalSettings");
        
        [MenuItem("Tools/Settings/Game Global Settings", priority = 1)]
        public static void OpenGameGlobalSetting() => SettingsService.OpenProjectSettings("Settings/GameGlobalSettings");
        
        [MenuItem("Tools/Settings/Game Config Settings", priority = 2)]
        public static void OpenGameConfigSetting() => SettingsService.OpenProjectSettings("Settings/GameConfigSettings");
        
        [MenuItem("Tools/Settings/Game Path Settings", priority = 3)]
        public static void OpenGamePathSetting() => SettingsService.OpenProjectSettings("Settings/GamePathSettings");
    }
}