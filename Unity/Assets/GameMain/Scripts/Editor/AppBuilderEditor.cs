using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor
{
    public class AppBuilderEditor : EditorWindow
    {
        private readonly string[] KeystoreExtensions =
        {
            ".keystore",
            ".jks",
            ".ks"
        };

        private Vector2 mScrollPosition;

        public static void Open()
        {
            var window = GetWindow<AppBuilderEditor>("App Build", true);
            window.minSize = new Vector2(600f, 800f);
        }

        private void OnEnable()
        {
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                mScrollPosition = EditorGUILayout.BeginScrollView(mScrollPosition);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Settings Tools : ", EditorStyles.boldLabel, GUILayout.Width(160f));
                        if (GUILayout.Button("Global", GUILayout.Width(120f)))
                        {
                            SettingsService.OpenProjectSettings("Settings/GameGlobalSettings");
                            GUIUtility.ExitGUI();
                        }

                        if (GUILayout.Button("Config", GUILayout.Width(120f)))
                        {
                            SettingsService.OpenProjectSettings("Settings/GameConfigSettings");
                            GUIUtility.ExitGUI();
                        }

                        if (GUILayout.Button("Path", GUILayout.Width(120f)))
                        {
                            SettingsService.OpenProjectSettings("Settings/GamePathSettings");
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(10);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Resource Tools : ", EditorStyles.boldLabel, GUILayout.Width(160f));
                        if (GUILayout.Button("Resource Builder", GUILayout.Width(120f)))
                        {
                            var builderClass = Utility.Assembly.GetType("UnityGameFramework.Editor.ResourceTools.ResourceBuilder");
                            builderClass?.GetMethod("Open", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, null);
                            GUIUtility.ExitGUI();
                        }

                        if (GUILayout.Button("Resource Editor", GUILayout.Width(120f)))
                        {
                            var editorClass = Utility.Assembly.GetType("UnityGameFramework.Editor.ResourceTools.ResourceEditor");
                            editorClass?.GetMethod("Open", BindingFlags.Static | BindingFlags.NonPublic)?.Invoke(null, null);
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(10);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Build App Settings : ", EditorStyles.boldLabel, GUILayout.Width(160f));

                        if (GUILayout.Button("Player Settings", GUILayout.Width(120f)))
                        {
                            SettingsService.OpenProjectSettings("Project/Player");
                            GUIUtility.ExitGUI();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("App Force Update", GUILayout.Width(160f));
                            SettingsUtils.GameGlobalSettings.ForceUpdateApp = EditorGUILayout.ToggleLeft("", SettingsUtils.GameGlobalSettings.ForceUpdateApp);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("App Version", GUILayout.Width(160f));
                            PlayerSettings.bundleVersion = EditorGUILayout.TextField(PlayerSettings.bundleVersion);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("App Update Uri", GUILayout.Width(160f));
                            SettingsUtils.GameGlobalSettings.AppUpdateUri = EditorGUILayout.TextField(SettingsUtils.GameGlobalSettings.AppUpdateUri);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("App Update Desc", GUILayout.Width(160f));
                            SettingsUtils.GameGlobalSettings.AppUpdateDesc = EditorGUILayout.TextArea(SettingsUtils.GameGlobalSettings.AppUpdateDesc);
                        }
                        EditorGUILayout.EndHorizontal();

#if UNITY_ANDROID
                        EditorGUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build App Bundle(GP)", GUILayout.Width(160f));
                            EditorUserBuildSettings.buildAppBundle = EditorGUILayout.ToggleLeft("", EditorUserBuildSettings.buildAppBundle);
                        }
                        EditorGUILayout.EndHorizontal();

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
                        EditorGUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Build App Path", GUILayout.Width(160f));
                            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
                            {
                                SettingsUtils.GameGlobalSettings.AppBuildPath = EditorGUILayout.TextField(SettingsUtils.GameGlobalSettings.AppBuildPath);
                                if (GUILayout.Button("Select", GUILayout.Width(120f)))
                                {
                                    var folder = Path.Combine(Application.dataPath, EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget));
                                    Debug.Log(EditorUserBuildSettings.activeBuildTarget);
                                    if (!Directory.Exists(folder))
                                    {
                                        folder = Application.dataPath;
                                    }

                                    var path = EditorUtility.OpenFolderPanel("Select Build App Path", folder, "");
                                    if (!string.IsNullOrEmpty(path))
                                    {
                                        SettingsUtils.GameGlobalSettings.AppBuildPath = path;
                                    }

                                    GUIUtility.ExitGUI();
                                }

                                if (changeCheckScope.changed)
                                {
                                    EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, SettingsUtils.GameGlobalSettings.AppBuildPath);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button(EditorGUIUtility.TrTextContentWithIcon("Build", "Build App", "BuildSettings.Standalone"), GUILayout.Height(35)))
                    {
#if UNITY_ANDROID
                        if (PlayerSettings.Android.useCustomKeystore && !CheckKeystoreAvailable(PlayerSettings.Android.keystoreName))
                        {
                            EditorUtility.DisplayDialog("Build Error!", $"Keystore file is not exist or has an incorrect format '{PlayerSettings.Android.keystoreName}'.", "GOT IT");
                            return;
                        }
#endif
                        
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private bool CheckKeystoreAvailable(string keystore)
        {
            if (string.IsNullOrEmpty(keystore))
            {
                return false;
            }

            return File.Exists(keystore) && KeystoreExtensions.Contains(Path.GetExtension(keystore).ToLower());
        }
    }
}