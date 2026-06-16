using System.IO;
using UnityEngine.Device;

namespace GameMain.Editor
{
    public class EditorConstant
    {
		public const string SharedAssetBundleName = "SharedAssets";
        
        public const string ConfigPath = "Assets/GameMain/Configs";
        public const string DataTablePath = "Assets/GameMain/DataTables";
        public const string LocalizationPath = "Assets/GameMain/Localizations";
        
        public const string ConfigExcelPath = "GameData/Configs";
        public const string DataTableExcelPath = "GameData/DataTables";
        public const string LocalizationExcelPath = "GameData/Localizations";

        public const string EntityGroupDataTableExcelPath = "Core/EntityGroupTable.xlsx";
        public const string SoundGroupDataTableExcelPath = "Core/SoundGroupTable.xlsx";
        public const string UIGroupDataTableExcelPath = "Core/UIGroupTable.xlsx";
        
        public const string UITableExcelPath = "UITable.xlsx";
        
        public static string DataTableFullPath => PathUtil.GetGameConfigFullPath(DataTablePath);
        
        public static string ConfigFullPath => PathUtil.GetGameConfigFullPath(ConfigPath);
        
        public static string LocalizationFullPath => PathUtil.GetGameConfigFullPath(LocalizationPath);
        
        public static string DataTableExcelFullPath => PathUtil.GetGameConfigFullPath(DataTableExcelPath);
        
        public static string ConfigExcelFullPath => PathUtil.GetGameConfigFullPath(ConfigExcelPath);
        
        public static string LocalizationExcelFullPath => PathUtil.GetGameConfigFullPath(LocalizationExcelPath);
        
        public static string EntityGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, EntityGroupDataTableExcelPath);
        
        public static string SoundGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, SoundGroupDataTableExcelPath);
        
        public static string UIGroupExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, UIGroupDataTableExcelPath);
        
        public static string UITableExcelFullPath => PathUtil.GetCombinePath(DataTableExcelFullPath, UITableExcelPath);
        
        public static string AssetBundleOutputPath => PathUtil.GetCombinePath(Directory.GetParent(Application.dataPath)?.FullName, "GameAssetBundle");
        
    }
}