using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using System.Linq;
using System.Security.Cryptography;
using XFramework.Core;
using System.Text;

namespace XFramework.Core
{
    public class BuildScript
    {
        public static string overloadedDevelopmentServerURL = "";

        static public string CreateAssetBundleDirectory()
        {
            // Choose the output path according to the build target.
            string outputPath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            return outputPath;
        }

        public static void BuildAssetBundles()
        {
            BuildAssetBundles(null);
        }

        public static void BuildAssetBundles(AssetBundleBuild[] builds)
        {
            // Choose the output path according to the build target.
            string outputPath = CreateAssetBundleDirectory();

            var options = BuildAssetBundleOptions.None;

            bool shouldCheckODR = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
#if UNITY_TVOS
            shouldCheckODR |= EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS;
#endif
            if (shouldCheckODR)
            {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                if (PlayerSettings.iOS.useOnDemandResources)
                    options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
#if ENABLE_IOS_APP_SLICING
                options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
            }

            if (builds == null || builds.Length == 0)
            {
                //@TODO: use append hash... (Make sure pipeline works correctly with it.)
                BuildPipeline.BuildAssetBundles(outputPath, options, EditorUserBuildSettings.activeBuildTarget);
            }
            else
            {
                BuildPipeline.BuildAssetBundles(outputPath, builds, options, EditorUserBuildSettings.activeBuildTarget);
            }
        }

        /// <summary>
        /// 构建版本
        /// </summary>
        public static void BuildVersion()
        {
            string outputPath = CreateAssetBundleDirectory();
            string output = outputPath.Replace("\\", "/") + "/";
            string[] filePaths = Directory.GetFiles(outputPath, "*.*", SearchOption.AllDirectories).Where(s => !s.EndsWith(".manifest") && !s.EndsWith(".txt")).ToArray();
            Version version = new Version();
            version.AssetBundleInfos = new List<AssetBundleInfo>();

            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                AssetBundleInfo info = new AssetBundleInfo();
                info.Id = 1 + i.ToString("D4");
                info.Name = filePath.Replace("\\", "/").Replace(output, "");

                Debug.LogFormat("Build Version Name : {0}", info.Name);

                using (FileStream fs = new FileStream(filePath,FileMode.Open, FileAccess.Read))
                {
                    MD5 md5 = MD5.Create();
                    byte[] md5Bytes = md5.ComputeHash(fs);
                    info.Hash = BitConverter.ToString(md5Bytes).Replace("-", "").ToLower();
                    info.Length = fs.Length;
                }

                version.AssetBundleInfos.Add(info);
            }
            //版本号
            version.Number = DateTime.Now.ToString("10.yy.MM.dd.HH.mm.ss");
            //保存
            string json = version.ToJson();
            using (FileStream fs = new FileStream(outputPath + "/" + Version.url, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(json);
                }
            }
        }

        public static void WriteServerURL()
        {
            string downloadURL;
            if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
            {
                downloadURL = overloadedDevelopmentServerURL;
            }
            else
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                downloadURL = "http://" + localIP + ":7888/";
            }

            string assetBundleManagerResourcesDirectory = "Assets/AssetBundleManager/Resources";
            string assetBundleUrlPath = Path.Combine(assetBundleManagerResourcesDirectory, "AssetBundleServerURL.bytes");
            Directory.CreateDirectory(assetBundleManagerResourcesDirectory);
            File.WriteAllText(assetBundleUrlPath, downloadURL);
            AssetDatabase.Refresh();
        }

        public static void BuildPlayer()
        {
            var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            string[] levels = GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }

            string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

            // Build and copy AssetBundles.
            BuildScript.BuildAssetBundles();
            WriteServerURL();

#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = levels;
            buildPlayerOptions.locationPathName = outputPath + targetName;
            buildPlayerOptions.assetBundleManifestPath = GetAssetBundleManifestFilePath();
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildPlayerOptions.options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(buildPlayerOptions);
#endif
        }

        public static void BuildStandalonePlayer()
        {
            var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            string[] levels = GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }

            string targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;

            // Build and copy AssetBundles.
            BuildScript.BuildAssetBundles();
            BuildScript.CopyAssetBundlesTo(Path.Combine(Application.streamingAssetsPath, Utility.AssetBundlesOutputPath));
            AssetDatabase.Refresh();

#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
            BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = levels;
            buildPlayerOptions.locationPathName = outputPath + targetName;
            buildPlayerOptions.assetBundleManifestPath = GetAssetBundleManifestFilePath();
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;
            buildPlayerOptions.options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            BuildPipeline.BuildPlayer(buildPlayerOptions);
#endif
        }

        public static string GetBuildTargetName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "/test.apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "/test.exe";
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSX:
                    return "/test.app";
                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }

        public static void CopyAssetBundlesTo(string outputPath)
        {
            // Clear streaming assets folder.
            FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
            Directory.CreateDirectory(outputPath);

            string outputFolder = Utility.GetPlatformName();

            // Setup the source folder for assetbundles.
            var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, Utility.AssetBundlesOutputPath), outputFolder);
            if (!System.IO.Directory.Exists(source))
                Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

            // Setup the destination folder for assetbundles.
            var destination = System.IO.Path.Combine(outputPath, outputFolder);
            if (System.IO.Directory.Exists(destination))
                FileUtil.DeleteFileOrDirectory(destination);

            FileUtil.CopyFileOrDirectory(source, destination);
        }

        static string[] GetLevelsFromBuildSettings()
        {
            List<string> levels = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add(EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray();
        }

        static string GetAssetBundleManifestFilePath()
        {
            var relativeAssetBundlesOutputPathForPlatform = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            return Path.Combine(relativeAssetBundlesOutputPathForPlatform,  Utility.GetPlatformName()) + ".manifest";
        }

        const string buildRuleSettingsPath = "Assets/Resources/BuildNameSettings.asset";
        public static void BatchAssetBundlesName()
        {
            if (!File.Exists(buildRuleSettingsPath))
            {
                //File.Create(buildRulesPath).Close();
                //using (FileStream stream = new FileStream(buildRulesPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                //{
                //    using (StreamWriter writer = new StreamWriter(stream))
                //    {
                //        string str = "{\r\n  \"BuildRules\": [\r\n    {\r\n      \"seatchPath\" : \"Assets/Resources\",\r\n      \"searchPattern\" : \"*.png\",\r\n      \"searchOption\" : \"AllDirectories\",\r\n      \"buildRuleOption\" : \"FileName\",\r\n      \"assetBundleName\" : \"\",\r\n      \"assetBundleVariant\" : \"\"\r\n    }\r\n  ]\r\n}";
                //        writer.Write(str);
                //    }
                //}
                BuildNameSettings settings = ScriptableObject.CreateInstance<BuildNameSettings>();
                settings.BuildNames.Add(new BuildName());//添加一个实例
                AssetDatabase.CreateAsset(settings, buildRuleSettingsPath);
                EditorUtility.DisplayDialog("批量设置AssetBundle名称", "配置文件BuildRuleSettings.asset已生成，请在BuildNameSettings.asset文件设置构建规则，然后批量设置AssetBundle的名称。", "确认");
                return;
            }

            float progress = 0;
            float count = 0;

            EditorUtility.DisplayProgressBar("批量设置AssetBundle名称", "正在设置AssetName名称中...", progress);
            BuildNameSettings buildNameSettings = AssetDatabase.LoadAssetAtPath<BuildNameSettings>(buildRuleSettingsPath);
            foreach (BuildName item in buildNameSettings.BuildNames)
            {
                item.seatchDirectory = item.seatchDirectory.Replace("\\", "/").TrimEnd('/');
                if (!Directory.Exists(item.seatchDirectory))
                {
                    Debug.LogErrorFormat("不存在此文件夹：{0}", item.seatchDirectory);
                    continue;
                }

                List<string> assetPaths = GetAssetPaths(item.seatchDirectory, item.searchPattern, item.searchOption);
                count += assetPaths.Count;
            }

            //设置Asset的AssetBundle名称
            foreach (BuildName item in buildNameSettings.BuildNames)
            {
                item.seatchDirectory = item.seatchDirectory.Replace("\\", "/").TrimEnd('/');
                if (!Directory.Exists(item.seatchDirectory))
                {
                    Debug.LogErrorFormat("不存在此文件夹：{0}", item.seatchDirectory);
                    continue;
                }

                List<string> assetPaths = GetAssetPaths(item.seatchDirectory, item.searchPattern, item.searchOption);
                foreach (var assetPath in assetPaths)//遍历资源路径
                {
                    progress++;
                    EditorUtility.DisplayProgressBar("批量设置AssetBundle名称", "正在设置AssetName名称中...", progress / count);
                    var importer = AssetImporter.GetAtPath(assetPath);
                    if (importer == null)
                    {
                        continue;
                    }

                    string assetBundleName = string.Empty;
                    
                    switch (item.buildRuleOption)
                    {
                        case BuildNameOption.AssetsWithFileName:
                            assetBundleName = assetPath.Substring(0, assetPath.LastIndexOf('.'));
                            break;
                        case BuildNameOption.AssetsWithDirectroyName:
                            assetBundleName = assetPath.Substring(0, assetPath.LastIndexOf('/'));
                            break;
                        case BuildNameOption.AssetBundleName:
                            assetBundleName = item.assetBundleName;
                            break;
                        case BuildNameOption.FileName:
                            assetBundleName = Path.GetFileNameWithoutExtension(assetPath);
                            break;
                        case BuildNameOption.DirectroyName:
                            assetBundleName = item.seatchDirectory.Substring(item.seatchDirectory.LastIndexOf('/'));
                            break;
                        case BuildNameOption.DirectroyWithFileName:
                            string subName = assetPath.Substring(item.seatchDirectory.Length).Remove(assetPath.Substring(item.seatchDirectory.Length).LastIndexOf('.'));
                            assetBundleName = item.seatchDirectory.Substring(item.seatchDirectory.LastIndexOf('/') + 1) + subName;
                            break;
                        default:
                            break;
                    }

                    importer.assetBundleName = assetBundleName;
                    importer.assetBundleVariant = item.assetBundleVariant;
                }
            }

            AssetDatabase.Refresh();
            AssetDatabase.RemoveUnusedAssetBundleNames();
            EditorUtility.ClearProgressBar();
        }

        static List<string> GetAssetPaths(string path, string searchPattern, SearchOption searchOption)
        {
            var files = searchPattern.Split('|').SelectMany(filter => Directory.GetFiles(path, filter, searchOption));
            List<string> items = new List<string>();
            foreach (var item in files)
            {
                var assetPath = item.Replace('\\', '/');
                if (!Directory.Exists(assetPath))
                {
                    items.Add(assetPath);
                }
            }
            return items;
        }

    }
}
