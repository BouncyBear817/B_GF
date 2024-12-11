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
        public static void OpenUIAutoBindGlobalSetting()
        {
            SettingsService.OpenProjectSettings("Settings/UIAutoBindGlobalSettings");
        }

        [MenuItem("Tools/Settings/Game Global Settings", priority = 1)]
        public static void OpenGameGlobalSetting()
        {
            SettingsService.OpenProjectSettings("Settings/GameGlobalSettings");
        }

        [MenuItem("Tools/Settings/Game Config Settings", priority = 2)]
        public static void OpenGameConfigSetting()
        {
            SettingsService.OpenProjectSettings("Settings/GameConfigSettings");
        }

        [MenuItem("Tools/Settings/Game Path Settings", priority = 3)]
        public static void OpenGamePathSetting()
        {
            SettingsService.OpenProjectSettings("Settings/GamePathSettings");
        }
        
        [ToolsMenuMethod("Game Settings/UI AutoBind Global Settings", null, 3, 1)]
        public static void OpenUIAutoBindGlobalSetting1()
        {
            Selection.activeObject = SettingsUtils.GetSettings("UIAutoBindGlobalSettings");
        }

        [ToolsMenuMethod("Game Settings/Game Global Settings", null, 3, 2)]
        public static void OpenGameGlobalSetting1()
        {
            Selection.activeObject = SettingsUtils.GetSettings("GameGlobalSettings");
        }

        [ToolsMenuMethod("Game Settings/Game Config Settings", null, 3, 3)]
        public static void OpenGameConfigSetting1()
        {
            Selection.activeObject = SettingsUtils.GetSettings("GameConfigSettings");
        }

        [ToolsMenuMethod("Game Settings/Game Path Settings", null, 3, 4)]
        public static void OpenGamePathSetting1()
        {
            Selection.activeObject = SettingsUtils.GetSettings("GamePathSettings");
        }
    }
}