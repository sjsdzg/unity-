using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace XFramework.Editor
{
    public class TMPProperties
    {
        public TMP_FontAsset FontAsset;
    }

    public class ModifyTMPScript : ScriptableObject
    {
        private static ApplyMode m_ApplyMode;
        private static TMPProperties m_TMPProperties = new TMPProperties();

        public static void Draw()
        {
            m_ApplyMode = (ApplyMode)EditorGUILayout.EnumPopup("应用模式：", m_ApplyMode);
            m_TMPProperties.FontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("修改TMP：", m_TMPProperties.FontAsset, typeof(TMP_FontAsset), true);
            EditorGUILayout.Space();
            if (GUILayout.Button("修改TMP"))
                ModifyTMPAsset();
        }

        /// <summary>
        /// 修改TMP
        /// </summary>
        private static void ModifyTMPAsset()
        {
            switch (m_ApplyMode)
            {
                case ApplyMode.Selection:
                    ModifyTMPAssetForSelection(m_TMPProperties);
                    break;
                case ApplyMode.All:
                    if (EditorUtility.DisplayDialog("修改TMP", "确定对项目中【所有】的 TMP_Text 修改字体 ？", "确定", "取消"))
                    {
                        ModifyTMPAssetForAll(m_TMPProperties);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ModifyTMPAssetForSelection(TMPProperties properties)
        {
            if (Selection.objects == null || Selection.objects.Length == 0) return;

            int number = 0;

            EditorUtility.DisplayProgressBar("批量修改TMP", "正在设置修改TMP中...", number);
            TMP_Text[] labels = Selection.GetFiltered<TMP_Text>(SelectionMode.Deep);

            foreach (TMP_Text label in labels)
            {
                if (properties.FontAsset != null)
                    label.font = properties.FontAsset;

                EditorUtility.SetDirty(label);

                number++;
                EditorUtility.DisplayProgressBar("批量修改TMP", "Gameobject : " + label.name, number / (float)labels.Length);
            }

            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        public static void ModifyTMPAssetForAll(TMPProperties properties)
        {
            string[] searchInFolders = ToolsWindow.searchInFolders;

            // prefabs
            string[] guids = AssetDatabase.FindAssets("t:prefab", searchInFolders);
            int number = 0;

            EditorUtility.DisplayProgressBar("批量修改TMP", "正在设置修改TMP中...", number);
            List<TMP_Text> labels = new List<TMP_Text>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                labels.Clear();
                labels.AddRange(prefab.GetComponentsInChildren<TMP_Text>(true));

                bool changed = labels.Count > 0;

                foreach (TMP_Text label in labels)
                {
                    if (properties.FontAsset != null)
                        label.font = properties.FontAsset;
                }

                if (changed)
                {
                    PrefabUtility.SavePrefabAsset(prefab);
                }
                
                number++;
                EditorUtility.DisplayProgressBar("批量修改TMP", "Prefab : " + prefab.name, number / (float)guids.Length);
            }

            // scenes
            string[] scene_guids = AssetDatabase.FindAssets("t:scene", searchInFolders);
            for (int i = 0; i < scene_guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(scene_guids[i]);
                Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                var roots = scene.GetRootGameObjects();
                labels.Clear();

                foreach (var item in roots)
                {
                    labels.AddRange(item.GetComponentsInChildren<TMP_Text>(true));
                }

                number = 0;
                foreach (TMP_Text label in labels)
                {
                    if (properties.FontAsset != null)
                        label.font = properties.FontAsset;

                    number++;
                    EditorUtility.DisplayProgressBar("修改场景【" + scene.name + "】中的TMP", "Gameobject : " + label.name, number / (float)labels.Count);
                }

                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
