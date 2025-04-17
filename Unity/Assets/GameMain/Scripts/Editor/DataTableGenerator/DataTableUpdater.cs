using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public static class DataTableUpdater
    {
        private const string Filter = "*.xlsx";
        private static IList<string> sDataTableChangedList;
        private static IList<string> sConfigChangedList;
        private static IList<string> sLocalizationChangedList;

        private static bool sIsInitialized = false;
        private static GameConfigSettings sGameConfigSettings;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (sIsInitialized)
            {
                return;
            }

            sDataTableChangedList = new List<string>();
            sConfigChangedList = new List<string>();
            sLocalizationChangedList = new List<string>();

            EditorApplication.update += OnUpdate;

            sGameConfigSettings = SettingsExtension.GameConfigSettings;
            
            var dtWatcher = new FileSystemWatcher(EditorConstant.DataTableExcelFullPath, Filter);
            dtWatcher.IncludeSubdirectories = true;
            dtWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            dtWatcher.EnableRaisingEvents = true;
            var dtFileChanged = new FileSystemEventHandler(OnDataTableFileChanged);
            var dtFileRename = new RenamedEventHandler(OnDataTableFileChanged);
            dtWatcher.Changed += dtFileChanged;
            dtWatcher.Deleted += dtFileChanged;
            dtWatcher.Renamed += dtFileRename;
            
            var cWatcher = new FileSystemWatcher(EditorConstant.ConfigExcelFullPath, Filter);
            cWatcher.IncludeSubdirectories = true;
            cWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            cWatcher.EnableRaisingEvents = true;
            var cFileChanged = new FileSystemEventHandler(OnConfigFileChanged);
            var cFileRename = new RenamedEventHandler(OnConfigFileChanged);
            cWatcher.Changed += cFileChanged;
            cWatcher.Deleted += cFileChanged;
            cWatcher.Renamed += cFileRename;
            
            var lWatcher = new FileSystemWatcher(EditorConstant.LocalizationExcelFullPath, Filter);
            lWatcher.IncludeSubdirectories = true;
            lWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            lWatcher.EnableRaisingEvents = true;
            var lFileChanged = new FileSystemEventHandler(OnLocalizationFileChanged);
            var lFileRename = new RenamedEventHandler(OnLocalizationFileChanged);
            lWatcher.Changed += lFileChanged;
            lWatcher.Deleted += lFileChanged;
            lWatcher.Renamed += lFileRename;

            sIsInitialized = true;
        }

        private static void OnUpdate()
        {
            if (!sIsInitialized)
            {
                return;
            }

            if (sDataTableChangedList.Count > 0)
            {
                var changedFiles = GetMainExcelFiles(GameConfigType.DataTable, sGameConfigSettings.DataTables, sDataTableChangedList);
                GameConfigGenerator.RefreshDataTables(changedFiles);
                if (changedFiles.Contains(EditorConstant.UITableExcelFullPath))
                {
                    GameConfigGenerator.GenerateUIViewScript();
                }

                if (changedFiles.Contains(EditorConstant.EntityGroupExcelFullPath) || changedFiles.Contains(EditorConstant.SoundGroupExcelFullPath) || changedFiles.Contains(EditorConstant.UIGroupExcelFullPath))
                {
                    GameConfigGenerator.GenerateGroupEnumScript();
                }

                foreach (var file in changedFiles)
                {
                    Debug.Log($"Auto Refresh DataTable : {file}");
                }

                sDataTableChangedList.Clear();
            }

            if (sConfigChangedList.Count > 0)
            {
                var changedFiles = GetMainExcelFiles(GameConfigType.Config, sGameConfigSettings.Configs, sConfigChangedList);
                GameConfigGenerator.RefreshConfigs(changedFiles);

                foreach (var file in changedFiles)
                {
                    Debug.Log($"Auto Refresh Config : {file}");
                }

                sConfigChangedList.Clear();
            }

            if (sLocalizationChangedList.Count > 0)
            {
                var changedFiles = GetMainExcelFiles(GameConfigType.Localization, sGameConfigSettings.Localizations, sLocalizationChangedList);
                GameConfigGenerator.RefreshLocalizations(changedFiles);

                foreach (var file in changedFiles)
                {
                    Debug.Log($"Auto Refresh Localization : {file}");
                }

                sLocalizationChangedList.Clear();
            }
        }

        private static IList<string> GetMainExcelFiles(GameConfigType gameConfigType, IList<string> relativeFiles, IList<string> changedFileList)
        {
            var result = new List<string>();
            foreach (var changedFile in changedFileList)
            {
                var relativePathNoExtension = GameConfigGenerator.GetGameConfigExcelRelativeFileName(gameConfigType, changedFile);
                foreach (var relativeFile in relativeFiles)
                {
                    if (string.Compare(relativePathNoExtension, relativeFile, StringComparison.Ordinal) == 0)
                    {
                        var excelFullPath = GameConfigGenerator.GetGameConfigExcelRelativeFullPath(gameConfigType, relativeFile);

                        if (!result.Contains(excelFullPath))
                        {
                            result.Add(excelFullPath);
                        }
                    }
                }
            }

            return result;
        }

        private static void OnDataTableFileChanged(object sender, FileSystemEventArgs e)
        {
            var name = Path.GetFileNameWithoutExtension(e.Name);
            if (!name.StartsWith("~$"))
            {
                sDataTableChangedList.Add(e.FullPath);
            }
        }

        private static void OnConfigFileChanged(object sender, FileSystemEventArgs e)
        {
            var name = Path.GetFileNameWithoutExtension(e.Name);
            if (!name.StartsWith("~$"))
            {
                sConfigChangedList.Add(e.FullPath);
            }
        }

        private static void OnLocalizationFileChanged(object sender, FileSystemEventArgs e)
        {
            var name = Path.GetFileNameWithoutExtension(e.Name);
            if (!name.StartsWith("~$"))
            {
                sLocalizationChangedList.Add(e.FullPath);
            }
        }
    }
}