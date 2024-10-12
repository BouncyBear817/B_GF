// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/09 14:10:01
//  * Description:
//  * Modify Record:
//  *************************************************************/


using UnityEditor;

namespace GameMain.Editor
{
    public partial class GameConfigGenerator
    {
        [MenuItem("Bear Tools/Game Config/Refresh All Languages")]
        public static void RefreshAllLanguages()
        {
            AssetDatabase.Refresh();
        }
    }
}