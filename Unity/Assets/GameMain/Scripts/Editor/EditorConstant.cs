using System.IO;
using UnityEngine.Device;

namespace GameMain.Editor
{
    public class EditorConstant
    {
		public const string SharedAssetBundleName = "SharedAssets";

        public const string UITableExcel = "UITable.xlsx";

        public static string DataTableExcelFullPath = PathUtil.GetGameConfigFullPath(SettingsExtension.GamePathSettings.DataTableExcelPath);
        
        public static string ConfigExcelFullPath = PathUtil.GetGameConfigFullPath(SettingsExtension.GamePathSettings.ConfigExcelPath);
        
        public static string LocalizationExcelFullPath = PathUtil.GetGameConfigFullPath(SettingsExtension.GamePathSettings.LocalizationExcelPath);
        
        public static string UITableExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, UITableExcel);
        
        public static string EntityGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, SettingsExtension.GamePathSettings.EntityGroupDataTableExcelPath);
        
        public static string SoundGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, SettingsExtension.GamePathSettings.SoundGroupDataTableExcelPath);
        
        public static string UIGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, SettingsExtension.GamePathSettings.UIGroupDataTableExcelPath);
        
        public static string AssetBundleOutputPath => PathUtil.GetCombinePath(Directory.GetParent(Application.dataPath)?.FullName, "GameAssetBundle");
        
    }
}