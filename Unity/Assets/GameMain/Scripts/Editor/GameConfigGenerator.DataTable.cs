// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/10/09 14:10:01
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public partial class GameConfigGenerator
    {
        private static List<string> mDataTableVarTypes = null;

        [MenuItem("Bear Tools/Game Config/Refresh All DataTables")]
        public static void RefreshAllDataTables()
        {
            var excelFiles = GetAllGameConfigExcelFullPathList(GameConfigType.DataTable);

            RefreshDataTables(excelFiles);
        }

        public static void RefreshDataTables(List<string> excelFiles)
        {
            var gameConfig = SettingsUtils.GameConfigSettings;

            for (int i = 0; i < excelFiles.Count; i++)
            {
                var excelPath = excelFiles[i];
                var outputPath = GetGameConfigExcelOutputPath(GameConfigType.DataTable, excelPath);
                EditorUtility.DisplayProgressBar($"Refreshing DataTable Progress : {i + 1} / {excelFiles.Count}",
                    $"{excelPath} -> {outputPath}",
                    (i + 1) / (float)excelFiles.Count);
                try
                {
                    if (ExcelToTxtFile(excelPath, outputPath))
                    {
                        Debug.Log($"Refreshed DataTable success : {excelPath} -> {outputPath}.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Refreshed DataTable failed. exception message: {e.Message}.");
                }
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();

            GenerateDataTableCode(gameConfig);
        }

        /// <summary>
        /// 创建 DataTable Excel
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <returns>是否成功创建 DataTable Excel</returns>
        public static bool CreateDataTableExcel(string excelPath)
        {
            if (File.Exists(excelPath))
            {
                Debug.LogWarning($"Create DataTable excel failed. Excel file already exist : {excelPath}.");
                return false;
            }

            try
            {
                var excelDirectory = Path.GetDirectoryName(excelPath);
                if (excelDirectory != null && !Directory.Exists(excelDirectory))
                {
                    Directory.CreateDirectory(excelDirectory);
                }

                using (var excel = new ExcelPackage(excelPath))
                {
                    var sheet = excel.Workbook.Worksheets.Add("Sheet1");
                    sheet.SetValue(1, 1, "#");
                    sheet.SetValue(1, 2, Path.GetFileNameWithoutExtension(excelPath));
                    sheet.SetValue(2, 1, "#");
                    sheet.SetValue(2, 2, "ID");
                    sheet.SetValue(3, 1, "#");
                    sheet.SetValue(3, 2, "int");
                    sheet.SetValue(4, 1, "#");
                    sheet.SetValue(4, 3, "备注");
                    sheet.SetValue(4, 4, "请添加字段, 字段名首字母大写");
                    if (mDataTableVarTypes == null)
                    {
                        mDataTableVarTypes = ScanVariableTypes();
                    }

                    if (mDataTableVarTypes != null)
                    {
                        var listValidation = sheet.DataValidations.AddListValidation("D3:Z3");
                        listValidation.AllowBlank = false;
                        listValidation.Formula.Values.Clear();

                        foreach (var type in mDataTableVarTypes)
                        {
                            listValidation.Formula.Values.Add(type);
                        }
                    }

                    var i18nValidation = sheet.DataValidations.AddListValidation("D1:Z1");
                    i18nValidation.AllowBlank = true;
                    i18nValidation.Formula.Values.Clear();
                    i18nValidation.Formula.Values.Add("i18n");
                    excel.Save();
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Create DataTable excel failed. Path : {excelPath}, Exception : {e}.");
                return false;
            }
        }

        /// <summary>
        /// 生成DataTable对应的脚本
        /// </summary>
        /// <param name="gameConfigSettings">游戏配置设置</param>
        public static void GenerateDataTableCode(GameConfigSettings gameConfigSettings)
        {
            var dataTablesCount = gameConfigSettings.DataTables.Length;
            var outputDir = GetGameConfigPrePath(GameConfigType.DataTable);
            var outputExtension = GetGameConfigExcelOutputFileExtension(GameConfigType.DataTable);

            for (int i = 0; i < dataTablesCount; i++)
            {
                var dataTableName = gameConfigSettings.DataTables[i];
                var dataTablePath = PathUtil.GetCombinePath(outputDir, dataTableName + outputExtension);
                EditorUtility.DisplayProgressBar($"Generate Code Progress : {i + 1} / {dataTablesCount}",
                    $"Generate DataTable Code : {dataTablePath}",
                    (i + 1) / (float)dataTablesCount);
                if (!File.Exists(dataTablePath))
                {
                    Debug.LogWarning($"Generate DataTable Code failed. File is not exist : {dataTablePath}.");
                    continue;
                }

                var dataTableProcessor = DataTableGenerator.Create(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError($"Check Raw Data Failed. DataTableName : {dataTableName}.");
                    continue;
                }

                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }
}