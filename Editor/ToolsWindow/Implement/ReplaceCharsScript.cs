using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace XFramework.Editor
{
    public class ReplaceCharsScript : ScriptableObject
    {
        private static string oldValue = "";
        private static string newValue = "";

        public static void Draw()
        {
            oldValue = EditorGUILayout.TextField("搜索词：", oldValue);
            newValue = EditorGUILayout.TextField("替换词：", newValue);
            EditorGUILayout.Space();
            if (GUILayout.Button("替换"))
                ReplaceChars();
        }

        /// <summary>
        /// 替换字符
        /// </summary>
        private static void ReplaceChars()
        {
            if (Selection.objects == null || Selection.objects.Length == 0) return;

            if (string.IsNullOrEmpty(oldValue) || string.IsNullOrEmpty(newValue)) return;

            int number = 0;
            EditorUtility.DisplayProgressBar("批量替换字符", "正在替换字符中...", number);

            Text[] labels = Selection.GetFiltered<Text>(SelectionMode.Deep);
            foreach (Text label in labels)
            {
                label.text = label.text.Replace(oldValue, newValue);
                Debug.Log(label.name + ":" + label.text);
                EditorUtility.SetDirty(label);

                number++;
                EditorUtility.DisplayProgressBar("批量替换字符", "Gameobject : " + label.name, number / (float)labels.Length);
            }

            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
    }
}
