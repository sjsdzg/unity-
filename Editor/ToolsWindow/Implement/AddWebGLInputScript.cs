using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace XFramework.Editor
{
    public class AddWebGLInputScript : ScriptableObject
    {
        private static ApplyMode applyMode;

        public static void Draw()
        {
            applyMode = (ApplyMode)EditorGUILayout.EnumPopup("应用模式：", applyMode);
            if (GUILayout.Button("添加WebGLInput"))
                AddWebGLInput();
        }

        /// <summary>
        /// 添加 WebGLSupport.WebGLInput
        /// </summary>
        private static void AddWebGLInput()
        {
            switch (applyMode)
            {
                case ApplyMode.Selection:
                    AddWebGLInputForSelection();
                    break;
                case ApplyMode.All:
                    if (EditorUtility.DisplayDialog("批量添加WebGLInput", "确定对项目中【所有】的 InputField 添加 WebGLInput ？", "确定", "取消"))
                    {
                        AddWebGLInputForAll();
                    }
                    break;
                default:
                    break;
            }
        }

        public static void AddWebGLInputForSelection()
        {
            if (Selection.objects == null || Selection.objects.Length == 0) return;

            int number = 0;
            int count = 0;

            EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "正在批量添加WebGLSupport.WebGLInput中...", number);

            InputField[] InputFields = Selection.GetFiltered<InputField>(SelectionMode.Deep);
            TMP_InputField[] TMP_InputFields = Selection.GetFiltered<TMP_InputField>(SelectionMode.Deep);

            count = InputFields.Length + TMP_InputFields.Length;

            foreach (var obj in Selection.objects)
            {
                if (PrefabUtility.IsPartOfPrefabAsset(obj))
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    var prefab = PrefabUtility.LoadPrefabContents(path);

                    InputFields = prefab.GetComponentsInChildren<InputField>(true);
                    TMP_InputFields = prefab.GetComponentsInChildren<TMP_InputField>(true);

                    foreach (var inputField in InputFields)
                    {
                        if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                        {
                            inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                        }

                        number++;
                        EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                    }

                    foreach (var inputField in TMP_InputFields)
                    {
                        if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                        {
                            inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                        }

                        number++;
                        EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                    }

                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                    PrefabUtility.UnloadPrefabContents(prefab);
                }
                else
                {
                    GameObject go = (GameObject)obj;
                    if (go == null) continue;

                    InputFields = go.GetComponentsInChildren<InputField>(true);
                    TMP_InputFields = go.GetComponentsInChildren<TMP_InputField>(true);

                    foreach (var inputField in InputFields)
                    {
                        if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                        {
                            inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                        }

                        EditorUtility.SetDirty(inputField.gameObject);

                        number++;
                        EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                    }

                    foreach (var inputField in TMP_InputFields)
                    {
                        if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                        {
                            inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                        }

                        EditorUtility.SetDirty(inputField.gameObject);

                        number++;
                        EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                    }
                }

            }

            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        public static void AddWebGLInputForAll()
        {
            string[] searchInFolders = ToolsWindow.searchInFolders;
            // prefabs
            string[] guids = AssetDatabase.FindAssets("t:prefab", searchInFolders);
            int number = 0;
            int count = 0;

            EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "正在批量添加WebGLSupport.WebGLInput中...", number);

            List<InputField> InputFields = new List<InputField>();
            List<TMP_InputField> TMP_InputFields = new List<TMP_InputField>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefab = PrefabUtility.LoadPrefabContents(path);

                InputFields.Clear();
                TMP_InputFields.Clear();

                InputFields.AddRange(prefab.GetComponentsInChildren<InputField>(true));
                TMP_InputFields.AddRange(prefab.GetComponentsInChildren<TMP_InputField>(true));

                bool changed = (InputFields.Count + TMP_InputFields.Count) > 0;

                foreach (var inputField in InputFields)
                {
                    if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                    {
                        inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                    }
                }

                foreach (var inputField in TMP_InputFields)
                {
                    if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                    {
                        inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                    }
                }

                number++;
                EditorUtility.DisplayProgressBar("批量添加WebGLSupport.WebGLInput", "Prefab : " + prefab.name, number / (float)guids.Length);

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                }
                
                PrefabUtility.UnloadPrefabContents(prefab);
            }

            // scenes
            string[] scene_guids = AssetDatabase.FindAssets("t:scene", searchInFolders);
            for (int i = 0; i < scene_guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(scene_guids[i]);
                Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                var roots = scene.GetRootGameObjects();
                InputFields.Clear();
                TMP_InputFields.Clear();

                foreach (var item in roots)
                {
                    InputFields.AddRange(item.GetComponentsInChildren<InputField>(true));
                    TMP_InputFields.AddRange(item.GetComponentsInChildren<TMP_InputField>(true));
                }

                number = 0;
                count = InputFields.Count + TMP_InputFields.Count;

                foreach (var inputField in InputFields)
                {
                    if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                    {
                        inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                    }

                    number++;
                    EditorUtility.DisplayProgressBar("场景【" + scene.name + "】：批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                }

                foreach (var inputField in TMP_InputFields)
                {
                    if (inputField.GetComponent<WebGLSupport.WebGLInput>() == null)
                    {
                        inputField.gameObject.AddComponent<WebGLSupport.WebGLInput>();
                    }

                    number++;
                    EditorUtility.DisplayProgressBar("场景【" + scene.name + "】：批量添加WebGLSupport.WebGLInput", "Gameobject : " + inputField.name, number / (float)count);
                }

                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
