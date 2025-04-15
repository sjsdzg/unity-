using UnityEngine;
using UnityEditor;
using System.IO;

namespace XFramework.Common
{
    public class BuildSceneLoader
    {
        public const string path = "Assets/_Scenes";

        [MenuItem("Tools/SceneLoader/Build Scene Loader Settings")]
        static void DoIt()
        {
            SceneLoaderSettings settings = ScriptableObject.CreateInstance<SceneLoaderSettings>();

            //EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] scenes = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);
            foreach (var scene in scenes)
            {
                SceneLoaderInfo info = new SceneLoaderInfo();
                info.SceneName = Path.GetFileNameWithoutExtension(scene);
                AssetImporter importer = AssetImporter.GetAtPath(scene);
                info.AssetBundleName = importer.assetBundleName;
                settings.SceneLoaderInfos.Add(info);
            }
            AssetDatabase.CreateAsset(settings, "Assets/Resources/SceneLoaderSettings.asset");
            EditorUtility.DisplayDialog("场景加载程序", "场景加载程序的设置构建完成！", "确认");
        }
    }
}
