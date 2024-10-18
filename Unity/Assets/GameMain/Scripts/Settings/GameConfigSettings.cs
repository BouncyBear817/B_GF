// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/09 10:10:52
//  * Description:
//  * Modify Record:
//  *************************************************************/

using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    [CreateAssetMenu(fileName = "GameConfigSettings", menuName = "Bear Tools/Game Config Settings[配置数据表、配置表]", order = 2)]
    public class GameConfigSettings : ScriptableObject
    {
        [SerializeField] private string[] mDataTables;
        [SerializeField] private string[] mConfigs;
        [SerializeField] private string[] mLocalizations;

        public string[] DataTables
        {
            get => mDataTables;
            set => mDataTables = value;
        }

        public string[] Configs
        {
            get => mConfigs;
            set => mConfigs = value;
        }

        public string[] Localizations
        {
            get => mLocalizations;
            set => mLocalizations = value;
        }
    }
}