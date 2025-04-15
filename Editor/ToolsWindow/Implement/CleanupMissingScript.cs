using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace XFramework.Editor
{
    public class CleanupMissingScript : ScriptableObject
    {
        private static ApplyMode applyMode;

        public static void Draw()
        {
            applyMode = (ApplyMode)EditorGUILayout.EnumPopup("应用模式：", applyMode);
            if (GUILayout.Button("清理 Missing Script"))
                CleanupMissingScripts();
        }

        /// <summary>
        /// 清理 Missing Script
        /// </summary>
        private static void CleanupMissingScripts()
        {
            switch (applyMode)
            {
                case ApplyMode.Selection:
                    CleanupMissingScriptsForSelection();
                    break;
                case ApplyMode.All:
                    if (EditorUtility.DisplayDialog("清理 Missing Script", "确定对清理项目中【所有】的 Missing Script ？", "确定", "取消"))
                    {
                        CleanupMissingScriptsForAll();
                    }
                    break;
                default:
                    break;
            }

        }

        public static void CleanupMissingScriptsForSelection()
        {
            if (Selection.objects == null || Selection.objects.Length == 0) return;

            EditorUtility.DisplayProgressBar("清理 Missing Script", "清理 Missing Script中...", 0);
            int count = Selection.gameObjects.Length;

            for (int i = 0; i < count; i++)
            {
                var gameObject = Selection.gameObjects[i];

                // We must use the GetComponents array to actually detect missing components
                Transform[] list = gameObject.GetComponentsInChildren<Transform>(true);
                List<SerializedObject> serializedObjects = new List<SerializedObject>();

                foreach (var trans in list)
                {
                    var serializedObject = CleanupMissingScripts(trans.gameObject);
                    serializedObjects.Add(serializedObject);
                }

                foreach (SerializedObject so in serializedObjects)
                {
                    // Apply之后cmps里的所有组件都会被销毁，导致后面的清理无法执行，
                    // 所以将SO对象缓存，最后一起清理。
                    // Apply our changes to the game object
                    so.ApplyModifiedProperties();
                }

                EditorUtility.DisplayProgressBar("清理 Missing Script", gameObject.name, (i + 1) / (float)count);
            }

            EditorUtility.ClearProgressBar();
        }

        public static void CleanupMissingScriptsForAll()
        {
            string[] searchInFolders = ToolsWindow.searchInFolders;
            // prefabs
            string[] guids = AssetDatabase.FindAssets("t:prefab", searchInFolders);
            int number = 0;

            EditorUtility.DisplayProgressBar("清理 Missing Script", "清理 Missing Script中...", 0);
            List<SerializedObject> serializedObjects = new List<SerializedObject>();
            List<Transform> transforms = new List<Transform>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                transforms.Clear();
                transforms.AddRange(prefab.GetComponentsInChildren<Transform>(true));

                serializedObjects.Clear();
                foreach (var trans in transforms)
                {
                    var serializedObject = CleanupMissingScripts(trans.gameObject);
                    serializedObjects.Add(serializedObject);
                }

                foreach (SerializedObject so in serializedObjects)
                {
                    // Apply之后cmps里的所有组件都会被销毁，导致后面的清理无法执行，
                    // 所以将SO对象缓存，最后一起清理。
                    // Apply our changes to the game object
                    so.ApplyModifiedProperties();
                }

                number++;
                EditorUtility.DisplayProgressBar("清理 Missing Script", "Prefab : " + prefab.name, number / (float)guids.Length);
            }

            // scenes

            string[] scene_guids = AssetDatabase.FindAssets("t:scene", searchInFolders);
            for (int i = 0; i < scene_guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(scene_guids[i]);
                Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                var roots = scene.GetRootGameObjects();
                transforms.Clear();
                foreach (var item in roots)
                {
                    transforms.AddRange(item.GetComponentsInChildren<Transform>(true));
                }

                number = 0;
                serializedObjects.Clear();
                //List<SerializedObject> serializedObjects = new List<SerializedObject>();
                foreach (var trans in transforms)
                {
                    var serializedObject = CleanupMissingScripts(trans.gameObject);
                    serializedObjects.Add(serializedObject);

                    number++;
                    EditorUtility.DisplayProgressBar("清理场景【" + scene.name + "】中的 Missing Script", "Gameobject : " + trans.name, number / (float)transforms.Count);
                }

                foreach (SerializedObject so in serializedObjects)
                {
                    // Apply之后cmps里的所有组件都会被销毁，导致后面的清理无法执行，
                    // 所以将SO对象缓存，最后一起清理。
                    // Apply our changes to the game object
                    so.ApplyModifiedProperties();
                }

                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
            }

            EditorUtility.ClearProgressBar();
        }

        public static SerializedObject CleanupMissingScripts(GameObject go)
        {
            //CleanupMissingScripts(trans.gameObject, prefab);
            // We must use the GetComponents array to actually detect missing components
            var components = go.GetComponents<UnityEngine.Component>();

            // Create a serialized object so that we can edit the component list
            var serializedObject = new SerializedObject(go);
            // Find the component list property
            var prop = serializedObject.FindProperty("m_Component");

            // Track how many components we've removed
            int r = 0;

            // Iterate over all components
            for (int j = 0; j < components.Length; j++)
            {
                // Check if the ref is null
                if (components[j] == null)
                {
                    // If so, remove from the serialized component array
                    prop.DeleteArrayElementAtIndex(j - r);

                    Debug.Log("成功移除丢失脚本，gameObject : " + go.name);
                    // Increment removed count
                    r++;
                }
            }

            return serializedObject;
        }
    }
}
