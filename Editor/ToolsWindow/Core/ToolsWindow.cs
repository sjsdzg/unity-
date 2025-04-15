using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WH.Editor;

namespace XFramework.Editor
{
    /// <summary>
    /// 查找替换窗口
    /// </summary>
    public class ToolsWindow : EditorWindow
    {
        protected class Styles
        {
            public readonly GUIStyle listItem = new GUIStyle("PR Label");
            public readonly GUIStyle listItemBackground = new GUIStyle("CN EntryBackOdd");
            public readonly GUIStyle listItemBackground2 = new GUIStyle("CN EntryBackEven");
            public readonly GUIStyle listBackgroundStyle = new GUIStyle("CN Box");
            public Styles()
            {
                Texture2D background = this.listItem.hover.background;
                // 开启即失去焦点时，也显示蓝色
                //this.listItem.onNormal.background = background;
                this.listItem.onActive.background = background;
                this.listItem.onFocused.background = background;
            }
        }

        protected static List<string> m_Contents;
        protected static ListViewState m_ListView;
        protected static bool m_Focus;
        protected static Styles s_Styles;

        /// <summary>
        /// 选择 Index
        /// </summary>
        private static int selectedIndex;

        /// <summary>
        /// 搜索文件夹数组
        /// </summary>
        public static string[] searchInFolders = new string[] { "Assets/Resources", "Assets/_Prefabs", "Assets/_Scenes" };

        /// <summary>
        /// horizontalSplitView
        /// </summary>
        private static EditorGUISplitView horizontalSplitView = new EditorGUISplitView(EditorGUISplitView.Direction.Horizontal);

        static ToolsWindow()
        {
            m_ListView = new ListViewState();
            m_ListView.row = 0;
            m_Contents = new List<string>();
            m_Contents.Add("替换字符");
            m_Contents.Add("修改 Text");
            m_Contents.Add("修改 TextMeshPro");
            m_Contents.Add("清理 Missing Script");
            m_Contents.Add("添加 WebGLInput");
            m_Contents.Add("收集 ShaderVariant");
            m_Contents.Add("WebGL 设置");
        }

        [MenuItem("Tools/ToolsWindow")]
        private static void Init()
        {
            ToolsWindow window = EditorWindow.GetWindow<ToolsWindow>("工具窗口");
            window.Show();
        }

        private void OnGUI()
        {
            horizontalSplitView.BeginSplitView();
            DrawLeftView();
            horizontalSplitView.Split();
            DrawRightView();
            horizontalSplitView.EndSplitView();
            Repaint();
        }

        private void DrawLeftView()
        {
            if (s_Styles == null)
            {
                s_Styles = new Styles();
            }
            m_ListView.totalRows = m_Contents.Count;

            UnityEngine.Event current = UnityEngine.Event.current;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            GUIContent textContent = new GUIContent();
            foreach (ListViewElement el in ListViewGUI.ListView(m_ListView, s_Styles.listBackgroundStyle))
            {
                if (current.type == EventType.MouseDown && current.button == 0 && el.position.Contains(current.mousePosition) && current.clickCount == 1)
                {
                    //Debug.Log(el.row);
                    selectedIndex = el.row;
                }
                if (current.type == EventType.Repaint)
                {
                    textContent.text = GetRowText(el);

                    // 交替显示不同背景色
                    GUIStyle style = (el.row % 2 != 0) ? s_Styles.listItemBackground2 : s_Styles.listItemBackground;
                    style.Draw(el.position, false, false, m_ListView.row == el.row, false);
                    s_Styles.listItem.Draw(el.position, textContent, false, false, m_ListView.row == el.row, m_Focus);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawRightView()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            switch (selectedIndex)
            {
                case 0: // 替换字符
                    ReplaceCharsScript.Draw();
                    break;
                case 1: // 修改Text
                    ModifyTextScript.Draw();
                    break;
                case 2: // 修改TMP
                    ModifyTMPScript.Draw();
                    break;
                case 3: // 清理 Missing Scripts
                    CleanupMissingScript.Draw();
                    break;
                case 4: // 添加WebGLInput
                    AddWebGLInputScript.Draw();
                    break;
                case 5: // 收集 ShaderVariant
                    ShaderVariantScript.Draw();
                    break;
                case 6: // WebGL打包设置
                    WebGLSettingScript.Draw();
                    break;
                default:
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        protected string GetRowText(ListViewElement el)
        {
            return m_Contents[el.row];
        }

        private void OnFocus()
        {
            m_Focus = true;
        }

        private void OnLostFocus()
        {
            m_Focus = false;
        }

    }
}