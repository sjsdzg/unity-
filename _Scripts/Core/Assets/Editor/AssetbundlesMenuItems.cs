using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace XFramework.Core
{
    public class AssetBundlesMenuItems
    {
        const string kSimulationMode = "Tools/AssetBundles/Simulation Mode";

        [MenuItem(kSimulationMode)]
        public static void ToggleSimulationMode()
        {
            Assets.SimulateAssetBundleInEditor = !Assets.SimulateAssetBundleInEditor;
        }

        [MenuItem(kSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(kSimulationMode, Assets.SimulateAssetBundleInEditor);
            return true;
        }

        [MenuItem("Tools/AssetBundles/Batch AssetBundles Name")]
        static public void BatchAssetBundlesName()
        {
            BuildScript.BatchAssetBundlesName();
            AssetDatabase.Refresh();
            Debug.Log("Batch AssetBundles Name Finished!!!");
        }

        [MenuItem("Tools/AssetBundles/Build AssetBundles")]
        static public void BuildAssetBundles()
        {
            BuildScript.BuildAssetBundles();
            Debug.LogError("AB包 打包完成！！！！！！！！！");
        }

        [MenuItem("Tools/AssetBundles/Copy AssetBundles to StreamingAssets")]
        public static void CopyAssetBundlesToStreamingAssets()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            BuildScript.CopyAssetBundlesTo(Path.Combine(Application.streamingAssetsPath, Utility.AssetBundlesOutputPath));
            AssetDatabase.Refresh();
            Debug.Log("Copy AssetBundles to StreamingAssets Finished!!!");
        }

        /// <summary>
        /// 移除所有的AssetBundles名称
        /// </summary>
        [MenuItem("Tools/AssetBundles/Clear All AssetBundles Name")]
        public static void ClearAllAssetBundlesName()
        {
            //移除所有的资源包名称
            foreach (var name in AssetDatabase.GetAllAssetBundleNames())
            {
                AssetDatabase.RemoveAssetBundleName(name, true);
            }
            AssetDatabase.Refresh();
            Debug.Log("Clear All AssetBundles Name Finished!!!");
        }

        [MenuItem ("Tools/AssetBundles/Build Player (for use with engine code stripping)")]
        static public void BuildPlayer ()
        {
            BuildScript.BuildPlayer();
        }

        [MenuItem("Tools/AssetBundles/Build AssetBundles from Selection")]
        private static void BuildBundlesFromSelection()
        {
            // Get all selected *assets*
            var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();
            
            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            HashSet<string> processedBundles = new HashSet<string>();

            // Get asset bundle names from selection
            foreach (var o in assets)
            {
                var assetPath = AssetDatabase.GetAssetPath(o);
                var importer = AssetImporter.GetAtPath(assetPath);

                if (importer == null)
                {
                    continue;
                }

                // Get asset bundle name & variant
                var assetBundleName = importer.assetBundleName;
                var assetBundleVariant = importer.assetBundleVariant;
                var assetBundleFullName = string.IsNullOrEmpty(assetBundleVariant) ? assetBundleName : assetBundleName + "." + assetBundleVariant;
                
                // Only process assetBundleFullName once. No need to add it again.
                if (processedBundles.Contains(assetBundleFullName))
                {
                    continue;
                }

                processedBundles.Add(assetBundleFullName);
                
                AssetBundleBuild build = new AssetBundleBuild();

                build.assetBundleName = assetBundleName;
                build.assetBundleVariant = assetBundleVariant;
                build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleFullName);
                
                assetBundleBuilds.Add(build);
            }

            BuildScript.BuildAssetBundles(assetBundleBuilds.ToArray());
        }


        [MenuItem("Tools/AssetBundles/Batch AssetBundle Name From Selection")]
        private static void SetAssetBundleNameFromSelection()
        {
            // Get all selected *asset*
            var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();
            // Get asset bundle names from selection
            for (int i = 0; i < assets.Length; i++)
            {
                var o = assets[i];
                var assetPath = AssetDatabase.GetAssetPath(o);
                var importer = AssetImporter.GetAtPath(assetPath);
                
                if (!Directory.Exists(assetPath))
                {
                    Debug.Log(importer.assetPath);
                }
                string dir = assetPath.Substring(0, assetPath.LastIndexOf('/'));
                string assetBundleName = dir.Substring(dir.LastIndexOf('/') + 1) + "/" + o.name;
                Debug.Log(assetBundleName);
                //importer.assetBundleName = assetBundleName;
                //importer.assetBundleVariant = "";
                bool isCancel = EditorUtility.DisplayCancelableProgressBar("设置AssetBundle名称", "正在设置AssetBundle名称中...", 1f * (i + 1) / assets.Length);
                if (isCancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
            }
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 构建版本
        /// </summary>
        [MenuItem("Tools/AssetBundles/Build Version")]
        public static void BuildVersion()
        {
            BuildScript.BuildVersion();
            Debug.Log("资源版本构建完成。");
        }
    }
}