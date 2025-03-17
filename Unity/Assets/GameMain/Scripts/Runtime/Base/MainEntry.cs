/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:
 * Modify Record:
 *************************************************************/

using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 主入口
    /// </summary>
    public sealed partial class MainEntry : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this);

            InitBuiltinComponents();
            InitCustomComponents();

            InitComponentsSet();
        }

        private void InitComponentsSet()
        {
            Debugger.ActiveWindow = SettingsUtils.GameBuildSettings.DebugMode;

            var resourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (resourceComponent != null)
            {
                var resourceType = resourceComponent.GetType();
                var resourceMode = resourceType.GetField("m_ResourceMode", BindingFlags.Instance | BindingFlags.NonPublic);
                if (resourceMode != null)
                {
                    resourceMode.SetValue(resourceComponent, SettingsUtils.GameBuildSettings.ResourceMode);
                }
            }

            BearUIForm.SetMainFont(SettingsUtils.GameGlobalSettings.MainFont);

            BuiltinUIForm.InitBuiltinForm();
        }
    }
}