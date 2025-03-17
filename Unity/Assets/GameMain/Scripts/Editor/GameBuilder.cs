using System;
using System.IO;
using System.Reflection;
using GameFramework;
using GameFramework.Resource;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;

namespace GameMain.Editor
{
    public class GameBuilder : EditorWindow
    {
        private ResourceBuilderController mController;
        private bool mOrderBuildResources;
        private int mCompressionHelperTypeNameIndex;
        private int mBuildEventHandlerTypeNameIndex;

        private Vector2 mScrollPosition;

        public static void Open()
        {
            var window = GetWindow<GameBuilder>("Game Builder", true);
            window.minSize = new Vector2(800f, 800f);
        }

        private void OnEnable()
        {
            mController = new ResourceBuilderController();
            mController.OnLoadingResource += OnLoadingResource;
            mController.OnLoadingAsset += OnLoadingAsset;
            mController.OnLoadCompleted += OnLoadCompleted;
            mController.OnAnalyzingAsset += OnAnalyzingAsset;
            mController.OnAnalyzeCompleted += OnAnalyzeCompleted;
            mController.ProcessingAssetBundle += OnProcessingAssetBundle;
            mController.ProcessingBinary += OnProcessingBinary;
            mController.ProcessResourceComplete += OnProcessResourceComplete;
            mController.BuildResourceError += OnBuildResourceError;

            mOrderBuildResources = false;

            if (mController.Load())
            {
                Debug.Log("Load configuration success.");

                mCompressionHelperTypeNameIndex = 0;
                string[] compressionHelperTypeNames = mController.GetCompressionHelperTypeNames();
                for (int i = 0; i < compressionHelperTypeNames.Length; i++)
                {
                    if (mController.CompressionHelperTypeName == compressionHelperTypeNames[i])
                    {
                        mCompressionHelperTypeNameIndex = i;
                        break;
                    }
                }

                mController.RefreshCompressionHelper();

                mBuildEventHandlerTypeNameIndex = 0;
                string[] buildEventHandlerTypeNames = mController.GetBuildEventHandlerTypeNames();
                for (int i = 0; i < buildEventHandlerTypeNames.Length; i++)
                {
                    if (mController.BuildEventHandlerTypeName == buildEventHandlerTypeNames[i])
                    {
                        mBuildEventHandlerTypeNameIndex = i;
                        break;
                    }
                }

                mController.RefreshBuildEventHandler();
            }
            else
            {
                Debug.LogWarning("Load configuration failure.");
            }

            if (string.IsNullOrWhiteSpace(mController.OutputDirectory) || !Directory.Exists(mController.OutputDirectory))
            {
                mController.OutputDirectory = Constant.AssetBundleOutputPath;
            }

            if (SettingsUtils.GameBuildSettings.ResourceMode != ResourceMode.Unspecified)
            {
                SetResourceMode(SettingsUtils.GameBuildSettings.ResourceMode);
            }
        }

        private void Update()
        {
            if (mOrderBuildResources)
            {
                mOrderBuildResources = false;
                BuildResources();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField("Environment Information", EditorStyles.boldLabel);
                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Product Name", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.ProductName);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Company Name", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.CompanyName);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Game Identifier", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.GameIdentifier);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Game Framework Version", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.GameFrameworkVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Unity Version", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.UnityVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                            EditorGUILayout.LabelField(mController.ApplicableGameVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(5f);
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.LabelField("Platforms", EditorStyles.boldLabel);
                            EditorGUILayout.BeginHorizontal("box");
                            {
                                EditorGUILayout.BeginVertical();
                                {
                                    DrawPlatform(Platform.Windows, "Windows");
                                    DrawPlatform(Platform.Windows64, "Windows x64");
                                    DrawPlatform(Platform.MacOS, "macOS");
                                }
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.BeginVertical();
                                {
                                    DrawPlatform(Platform.Linux, "Linux");
                                    DrawPlatform(Platform.IOS, "iOS");
                                    DrawPlatform(Platform.Android, "Android");
                                }
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.BeginVertical();
                                {
                                    DrawPlatform(Platform.WindowsStore, "Windows Store");
                                    DrawPlatform(Platform.WebGL, "WebGL");
                                }
                                EditorGUILayout.EndVertical();
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField("Compression", EditorStyles.boldLabel);
                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("AssetBundle Compression", GUILayout.Width(160f));
                            mController.AssetBundleCompression = (AssetBundleCompressionType)EditorGUILayout.EnumPopup(mController.AssetBundleCompression);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Compression Helper", GUILayout.Width(160f));
                            string[] names = mController.GetCompressionHelperTypeNames();
                            int selectedIndex = EditorGUILayout.Popup(mCompressionHelperTypeNameIndex, names);
                            if (selectedIndex != mCompressionHelperTypeNameIndex)
                            {
                                mCompressionHelperTypeNameIndex = selectedIndex;
                                mController.CompressionHelperTypeName = selectedIndex <= 0 ? string.Empty : names[selectedIndex];
                                if (mController.RefreshCompressionHelper())
                                {
                                    Debug.Log("Set compression helper success.");
                                }
                                else
                                {
                                    Debug.LogWarning("Set compression helper failure.");
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Additional Compression", GUILayout.Width(160f));
                            mController.AdditionalCompressionSelected =
                                EditorGUILayout.ToggleLeft("Additional Compression for Output Full Resources with Compression Helper", mController.AdditionalCompressionSelected);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                    GUILayout.Space(5f);
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                        if (GUILayout.Button("Resource Editor", GUILayout.Width(120f)))
                        {
                            var editorClass = Utility.Assembly.GetType("UnityGameFramework.Editor.ResourceTools.ResourceEditor");
                            editorClass?.GetMethod("Open", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, null);
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Force Rebuild AssetBundle", GUILayout.Width(160f));
                            mController.ForceRebuildAssetBundleSelected = EditorGUILayout.Toggle(mController.ForceRebuildAssetBundleSelected);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build Event Handler", GUILayout.Width(160f));
                            string[] names = mController.GetBuildEventHandlerTypeNames();
                            int selectedIndex = EditorGUILayout.Popup(mBuildEventHandlerTypeNameIndex, names);
                            if (selectedIndex != mBuildEventHandlerTypeNameIndex)
                            {
                                mBuildEventHandlerTypeNameIndex = selectedIndex;
                                mController.BuildEventHandlerTypeName = selectedIndex <= 0 ? string.Empty : names[selectedIndex];
                                if (mController.RefreshBuildEventHandler())
                                {
                                    Debug.Log("Set build event handler success.");
                                }
                                else
                                {
                                    Debug.LogWarning("Set build event handler failure.");
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Internal Resource Version", GUILayout.Width(160f));
                            mController.InternalResourceVersion = EditorGUILayout.IntField(mController.InternalResourceVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Resource Version", GUILayout.Width(160f));
                            GUILayout.Label(Utility.Text.Format("{0} ({1})", mController.ApplicableGameVersion, mController.InternalResourceVersion));
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Resource Mode", GUILayout.Width(160f));
                            EditorGUI.BeginChangeCheck();
                            {
                                SettingsUtils.GameBuildSettings.ResourceMode = (ResourceMode)EditorGUILayout.EnumPopup(SettingsUtils.GameBuildSettings.ResourceMode, GUILayout.Width(160f));
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                SetResourceMode(SettingsUtils.GameBuildSettings.ResourceMode);
                                if (SettingsUtils.GameBuildSettings.ResourceMode == ResourceMode.Updatable)
                                {
                                    EditorGUILayout.LabelField("Resource Version File Name", GUILayout.Width(160f));
                                    SettingsUtils.GameBuildSettings.ResourceVersionFileName = EditorGUILayout.TextField(SettingsUtils.GameBuildSettings.ResourceVersionFileName);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        if (SettingsUtils.GameBuildSettings.ResourceMode == ResourceMode.Unspecified)
                        {
                            EditorGUILayout.HelpBox("ResourceMode is invalid.", MessageType.Error);
                        }

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Output Directory", GUILayout.Width(160f));
                            mController.OutputDirectory = EditorGUILayout.TextField(mController.OutputDirectory);
                            if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                            {
                                BrowseOutputDirectory();
                            }
                            if (GUILayout.Button("Reveal In Finder", GUILayout.Width(120f)))
                            {
                                EditorUtility.RevealInFinder(mController.OutputDirectory);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Working Path", GUILayout.Width(160f));
                            GUILayout.Label(mController.WorkingPath);
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        SetOutputResourcePathByResourceMode(SettingsUtils.GameBuildSettings.ResourceMode);
                        
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build Report Path", GUILayout.Width(160f));
                            GUILayout.Label(mController.BuildReportPath);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();

                    GetBuildMessage(out var buildMessage, out var buildMessageType);
                    EditorGUILayout.HelpBox(buildMessage, buildMessageType);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Build App Settings : ", EditorStyles.boldLabel, GUILayout.Width(160f));
#if UNITY_ANDROID
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build App Bundle(GP)", GUILayout.Width(160f));
                            EditorUserBuildSettings.buildAppBundle = EditorGUILayout.ToggleLeft("", EditorUserBuildSettings.buildAppBundle);
                        }
                        EditorGUILayout.EndHorizontal();
#endif
                        SettingsUtils.GameBuildSettings.DebugMode = EditorGUILayout.ToggleLeft("Debug Mode", SettingsUtils.GameBuildSettings.DebugMode);

                        if (GUILayout.Button("Player Settings", GUILayout.Width(120f)))
                        {
                            SettingsService.OpenProjectSettings("Project/Player");
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical("box");
                    {
                        if (mController.OutputFullSelected || mController.OutputPackedSelected)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                                SettingsUtils.GameBuildSettings.ApplicableGameVersion = EditorGUILayout.TextField(SettingsUtils.GameBuildSettings.ApplicableGameVersion);
                            }
                            EditorGUILayout.EndHorizontal();
                        
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("App Update Uri", GUILayout.Width(160f));
                                SettingsUtils.GameBuildSettings.AppUpdateUri = EditorGUILayout.TextField(SettingsUtils.GameBuildSettings.AppUpdateUri);
                                SettingsUtils.GameBuildSettings.ForceUpdateApp = EditorGUILayout.ToggleLeft("Force Update", SettingsUtils.GameBuildSettings.ForceUpdateApp, GUILayout.Width(100f));
                            }
                            EditorGUILayout.EndHorizontal();
                            
                            EditorGUILayout.LabelField("App Update Desc", GUILayout.Width(160f));
                            SettingsUtils.GameBuildSettings.AppUpdateDesc = EditorGUILayout.TextArea(SettingsUtils.GameBuildSettings.AppUpdateDesc, GUILayout.Height(50f));
                        }
                        
                        EditorGUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("App Version", GUILayout.Width(160f));
                            PlayerSettings.bundleVersion = EditorGUILayout.TextField(PlayerSettings.bundleVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build Game Path", GUILayout.Width(160f));
                            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
                            {
                                SettingsUtils.GameBuildSettings.GameBuildPath = EditorGUILayout.TextField(SettingsUtils.GameBuildSettings.GameBuildPath);
                                if (GUILayout.Button("Select", GUILayout.Width(120f)))
                                {
                                    var folder = Path.Combine(Application.dataPath, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
                                    Debug.Log(EditorUserBuildSettings.activeBuildTarget);
                                    if (!Directory.Exists(folder))
                                    {
                                        folder = Application.dataPath;
                                    }

                                    var path = EditorUtility.OpenFolderPanel("Select Build Game Path", folder, "");
                                    if (!string.IsNullOrEmpty(path))
                                    {
                                        SettingsUtils.GameBuildSettings.GameBuildPath = path;
                                    }

                                    GUIUtility.ExitGUI();
                                }

                                if (changeCheckScope.changed)
                                {
                                    EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, SettingsUtils.GameBuildSettings.GameBuildPath);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        
#if UNITY_ANDROID
                        EditorGUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Bundle Version Code", GUILayout.Width(160f));
                            PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField(PlayerSettings.Android.bundleVersionCode);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Use Custom Keystore", GUILayout.Width(160f));
                            PlayerSettings.Android.useCustomKeystore = EditorGUILayout.ToggleLeft("", PlayerSettings.Android.useCustomKeystore);
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUI.BeginDisabledGroup(!PlayerSettings.Android.useCustomKeystore);
                            {
                                EditorGUILayout.LabelField("", GUILayout.Width(160f));
                                PlayerSettings.Android.keystoreName = EditorGUILayout.TextField(PlayerSettings.Android.keystoreName);
                                if (GUILayout.Button("Select Keystore", GUILayout.Width(120f)))
                                {
                                    var folder = Path.Combine(Application.dataPath, PlayerSettings.Android.keystoreName);
                                    if (!Directory.Exists(folder))
                                    {
                                        folder = Application.dataPath;
                                    }

                                    var path = EditorUtility.OpenFilePanel("Select Keystore", folder, "keystore,jks,ks");
                                    if (!string.IsNullOrEmpty(path))
                                    {
                                        PlayerSettings.Android.keystoreName = path.Replace(Application.dataPath + "/", "");
                                    }

                                    GUIUtility.ExitGUI();
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        if (PlayerSettings.Android.useCustomKeystore)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Keystore Password", GUILayout.Width(160f));
                                PlayerSettings.keystorePass = EditorGUILayout.PasswordField(PlayerSettings.keystorePass);
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Key Alias Name", GUILayout.Width(160f));
                                PlayerSettings.Android.keyaliasName = EditorGUILayout.TextField(PlayerSettings.Android.keyaliasName);
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Key Alias Password", GUILayout.Width(160f));
                                PlayerSettings.keyaliasPass = EditorGUILayout.PasswordField(PlayerSettings.keyaliasPass);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
#elif UNITY_IOS
                        EditorGUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build Number", GUILayout.Width(160f));
                            PlayerSettings.iOS.buildNumber = EditorGUILayout.TextField(PlayerSettings.iOS.buildNumber);
                        }
                        EditorGUILayout.EndHorizontal();
#endif
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();


                GUILayout.Space(2f);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(mController.Platforms == Platform.Undefined || string.IsNullOrEmpty(mController.CompressionHelperTypeName) || !mController.IsValidOutputDirectory);
                    {
                        if (GUILayout.Button("Build Resources", GUILayout.Height(35f)))
                        {
                            mOrderBuildResources = true;
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Save", GUILayout.Height(35f)))
                    {
                        SaveConfiguration();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void SetResourceMode(ResourceMode resourceMode)
        {
            if (resourceMode == ResourceMode.Unspecified)
            {
                return;
            }

            mController.OutputPackageSelected = false;
            mController.OutputFullSelected = false;
            mController.OutputPackedSelected = false;

            switch (resourceMode)
            {
                case ResourceMode.Package:
                    mController.OutputPackageSelected = true;
                    break;

                case ResourceMode.Updatable:
                case ResourceMode.UpdatableWhilePlaying:
                    mController.OutputFullSelected = true;
                    break;
            }
        }

        private void SetOutputResourcePathByResourceMode(ResourceMode resourceMode)
        {
            switch (resourceMode)
            {
                case ResourceMode.Package:
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Package Path", GUILayout.Width(160f));
                        GUILayout.Label(mController.OutputPackagePath);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                    break;
                case ResourceMode.Updatable:
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Full Path", GUILayout.Width(160f));
                        GUILayout.Label(mController.OutputFullPath);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                    break;
                case ResourceMode.UpdatableWhilePlaying:
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Packed Path", GUILayout.Width(160f));
                        GUILayout.Label(mController.OutputPackedPath);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                    break;
            }
        }

        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", mController.OutputDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                mController.OutputDirectory = directory;
            }
        }

        private void GetBuildMessage(out string message, out MessageType messageType)
        {
            message = string.Empty;
            messageType = MessageType.Error;
            if (mController.Platforms == Platform.Undefined)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                message += "Platform is invalid.";
            }

            if (string.IsNullOrEmpty(mController.CompressionHelperTypeName))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                message += "Compression helper is invalid.";
            }

            if (!mController.IsValidOutputDirectory)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                message += "Output directory is invalid.";
            }

            if (!string.IsNullOrEmpty(message))
            {
                return;
            }

            messageType = MessageType.Info;
            if (Directory.Exists(mController.OutputPackagePath))
            {
                message += Utility.Text.Format("{0} will be overwritten.", mController.OutputPackagePath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(mController.OutputFullPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += Utility.Text.Format("{0} will be overwritten.", mController.OutputFullPath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(mController.OutputPackedPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += Utility.Text.Format("{0} will be overwritten.", mController.OutputPackedPath);
                messageType = MessageType.Warning;
            }

            if (messageType == MessageType.Warning)
            {
                return;
            }

            message = "Ready to build.";
        }

        private void BuildResources()
        {
            if (mController.BuildResources())
            {
                Debug.Log("Build resources success.");
                SaveConfiguration();
            }
            else
            {
                Debug.LogWarning("Build resources failure.");
            }
        }

        private void SaveConfiguration()
        {
            if (mController.Save())
            {
                Debug.Log("Save configuration success.");
            }
            else
            {
                Debug.LogWarning("Save configuration failure.");
            }
        }

        private void DrawPlatform(Platform platform, string platformName)
        {
            mController.SelectPlatform(platform, EditorGUILayout.ToggleLeft(platformName, mController.IsPlatformSelected(platform)));
        }

        private void OnLoadingResource(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Resources", Utility.Text.Format("Loading resources, {0}/{1} loaded.", index, count), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", Utility.Text.Format("Loading assets, {0}/{1} loaded.", index, count), (float)index / count);
        }

        private void OnLoadCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnAnalyzingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Analyzing Assets", Utility.Text.Format("Analyzing assets, {0}/{1} analyzed.", index, count), (float)index / count);
        }

        private void OnAnalyzeCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private bool OnProcessingAssetBundle(string assetBundleName, float progress)
        {
            if (EditorUtility.DisplayCancelableProgressBar("Processing AssetBundle", Utility.Text.Format("Processing '{0}'...", assetBundleName), progress))
            {
                EditorUtility.ClearProgressBar();
                return true;
            }
            else
            {
                Repaint();
                return false;
            }
        }

        private bool OnProcessingBinary(string binaryName, float progress)
        {
            if (EditorUtility.DisplayCancelableProgressBar("Processing Binary", Utility.Text.Format("Processing '{0}'...", binaryName), progress))
            {
                EditorUtility.ClearProgressBar();
                return true;
            }
            else
            {
                Repaint();
                return false;
            }
        }

        private void OnProcessResourceComplete(Platform platform)
        {
            EditorUtility.ClearProgressBar();
            Debug.Log(Utility.Text.Format("Build resources for '{0}' complete.", platform));
        }

        private void OnBuildResourceError(string errorMessage)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogWarning(Utility.Text.Format("Build resources error with error message '{0}'.", errorMessage));
        }
    }
}