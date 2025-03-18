using System.IO;
using UnityEngine.Device;

namespace GameMain.Editor
{
    public class Constant
    {
		public const string SharedAssetBundleName = "SharedAssets";

        public const string UITableExcel = "UITable.xlsx";
        public const string UIViewScriptFile = "Assets/GameMain/Scripts/Runtime/UI/UIViews.cs";
        
        public static string AssetBundleOutputPath => PathUtil.GetCombinePath(Directory.GetParent(Application.dataPath)?.FullName, "GameAssetBundle");
        
    }
}