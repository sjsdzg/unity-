using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace XFramework.Editor
{
    public class TextProperties
    {
        public Font Font;
        public FontStyle FontStyle;
    }

    public class ModifyTextScript : ScriptableObject
    {
        private static ApplyMode applyMode;
        private static TextProperties m_TextProperties = new TextProperties();

        public static void Draw()
        {
            applyMode = (ApplyMode)EditorGUILayout.EnumPopup("应用模式：", applyMode);
            m_TextProperties.Font = (Font)EditorGUILayout.ObjectField("修改字体：", m_TextProperties.Font, typeof(Font), true);
            m_TextProperties.FontStyle = (FontStyle)EditorGUILayout.EnumPopup("修改字体类型：", m_TextProperties.FontStyle);
            EditorGUILayout.Space();
            if (GUILayout.Button("修改Text"))
                ModifyText();
        }

        /// <summary>
        /// 修改Text的属性
        /// </summary>
        private static void ModifyText()
        {
            switch (applyMode)
            {
                case ApplyMode.Selection:
                    ModifyTextForSelection(m_TextProperties);
                    break;
                case ApplyMode.All:
                    if (EditorUtility.DisplayDialog("修改Text", "确定对项目中【所有】的 Text 修改字体 ？", "确定", "取消"))
                    {
                        ModifyTextForAll(m_TextProperties);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void ModifyTextForSelection(TextProperties properties)
        {
            if (Selection.objects == null || Selection.objects.Length == 0) return;

            int number = 0;

            EditorUtility.DisplayProgressBar("批量修改Text", "正在设置修改Text中...", number);
            Text[] labels = Selection.GetFiltered<Text>(SelectionMode.Deep);

            foreach (Text label in labels)
            {
                if (properties.Font != null)
                    label.font = properties.Font;

                switch (properties.FontStyle)
                {
                    case FontStyle.None:
                        break;
                    case FontStyle.Normal:
                        label.fontStyle = UnityEngine.FontStyle.Normal;
                        break;
                    case FontStyle.Bold:
                        label.fontStyle = UnityEngine.FontStyle.Bold;
                        break;
                    case FontStyle.Italic:
                        label.fontStyle = UnityEngine.FontStyle.Italic;
                        break;
                    case FontStyle.BoldAndItalic:
                        label.fontStyle = UnityEngine.FontStyle.BoldAndItalic;
                        break;
                    default:
                        break;
                }

                EditorUtility.SetDirty(label);

                number++;
                EditorUtility.DisplayProgressBar("批量修改Text", "Gameobject : " + label.name, number / (float)labels.Length);
            }

            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        public static void ModifyTextForAll(TextProperties properties)
        {
            string[] searchInFolders = ToolsWindow.searchInFolders;
            // prefabs
            string[] guids = AssetDatabase.FindAssets("t:prefab", searchInFolders);
            int number = 0;

            EditorUtility.DisplayProgressBar("批量修改Text", "正在设置修改Text中...", number);
            List<Text> labels = new List<Text>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                labels.Clear();
                labels.AddRange(prefab.GetComponentsInChildren<Text>(true));

                bool changed = labels.Count > 0;

                foreach (Text label in labels)
                {
                    if (properties.Font != null)
                        label.font = properties.Font;

                    switch (properties.FontStyle)
                    {
                        case FontStyle.None:
                            break;
                        case FontStyle.Normal:
                            label.fontStyle = UnityEngine.FontStyle.Normal;
                            break;
                        case FontStyle.Bold:
                            label.fontStyle = UnityEngine.FontStyle.Bold;
                            break;
                        case FontStyle.Italic:
                            label.fontStyle = UnityEngine.FontStyle.Italic;
                            break;
                        case FontStyle.BoldAndItalic:
                            label.fontStyle = UnityEngine.FontStyle.BoldAndItalic;
                            break;
                        default:
                            break;
                    }
                }

                if (changed)
                {
                    PrefabUtility.SavePrefabAsset(prefab);
                }
                
                number++;
                EditorUtility.DisplayProgressBar("批量修改Text", "Prefab : " + prefab.name, number / (float)guids.Length);
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
                    labels.AddRange(item.GetComponentsInChildren<Text>(true));
                }

                number = 0;
                foreach (Text label in labels)
                {
                    if (properties.Font != null)
                        label.font = properties.Font;

                    switch (properties.FontStyle)
                    {
                        case FontStyle.None:
                            break;
                        case FontStyle.Normal:
                            label.fontStyle = UnityEngine.FontStyle.Normal;
                            break;
                        case FontStyle.Bold:
                            label.fontStyle = UnityEngine.FontStyle.Bold;
                            break;
                        case FontStyle.Italic:
                            label.fontStyle = UnityEngine.FontStyle.Italic;
                            break;
                        case FontStyle.BoldAndItalic:
                            label.fontStyle = UnityEngine.FontStyle.BoldAndItalic;
                            break;
                        default:
                            break;
                    }

                    number++;
                    EditorUtility.DisplayProgressBar("修改场景【" + scene.name + "】中的Text", "Gameobject : " + label.name, number / (float)labels.Count);
                }

                EditorSceneManager.SaveScene(scene);
                EditorSceneManager.CloseScene(scene, true);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
