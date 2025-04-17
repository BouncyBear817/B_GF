using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public class EditorUtilityExtension
    {
        /// <summary>
        /// 选择相对工程路径文件夹
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="relativePath">默认打开的路径(相对路径)</param>
        /// <returns></returns>
        public static string OpenRelativeFolderPanel(string title, string relativePath)
        {
            var rootPath = Directory.GetParent(Application.dataPath)?.FullName;
            if (rootPath != null)
            {
                var curFullPath = !string.IsNullOrWhiteSpace(relativePath) ? Path.Combine(rootPath, relativePath) : rootPath;
                var selectPath = EditorUtility.OpenFolderPanel(title, curFullPath, null);

                return string.IsNullOrWhiteSpace(selectPath) ? selectPath : Path.GetRelativePath(rootPath, selectPath);
            }

            return string.Empty;
        }
    }
}