using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 查找替换窗口
/// </summary>
public class FindReplacementWindow : EditorWindow
{
    public enum FontStyle
    {
        None,
        //
        // 摘要:
        //     No special style is applied.
        Normal,
        //
        // 摘要:
        //     Bold style applied to your texts.
        Bold,
        //
        // 摘要:
        //     Italic style applied to your texts.
        Italic,
        //
        // 摘要:
        //     Bold and Italic styles applied to your texts.
        BoldAndItalic
    }


    public enum ResizeMode
    {
        None,
        Auto,
    }

    static FindReplacementWindow()
    {

    }

    private static int selected;//选择
    private static GUIContent[] contents;

    #region 替换字符
    private static string oldValue = "";
    private static string newValue = "";
    #endregion

    #region 替换字体
    private static Font toFont;
    private static FontStyle toFontStyle;
    private static ResizeMode resizeMode;
    private static bool alignByGeometry = false;
    #endregion

    [MenuItem("Tools/Bath Replace【批量替换】")]
    private static void Init()
    {
        contents = new GUIContent[2] { new GUIContent("替换字符"), new GUIContent("替换字体") };
        FindReplacementWindow window = EditorWindow.GetWindow<FindReplacementWindow>(true, "查找替换窗口");
        window.Show();
    }

    private void OnGUI()
    {
        selected = GUILayout.Toolbar(selected, contents);
        EditorGUILayout.Space();
        GUILayout.Label("please select one or more objects first!!!");
        EditorGUILayout.Space();
        switch (selected)
        {
            case 0:
                oldValue = EditorGUILayout.TextField("搜索词：", oldValue);
                newValue = EditorGUILayout.TextField("替换词：", newValue);
                break;
            case 1:
                toFont = (Font)EditorGUILayout.ObjectField("替换字体：", toFont, typeof(Font), true);
                toFontStyle = (FontStyle)EditorGUILayout.EnumPopup("替换字体类型：", toFontStyle);
                resizeMode = (ResizeMode)EditorGUILayout.EnumPopup("自动调整模式：", resizeMode);
                alignByGeometry = EditorGUILayout.Toggle("AlignByGeometry：", alignByGeometry);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("替换"))
            Replace();
    }

    /// <summary>
    /// 替换
    /// </summary>
    public static void Replace()
    {
        if (Selection.objects == null || Selection.objects.Length == 0) return;

        if (selected == 0)
        {
            Text[] labels = Selection.GetFiltered<Text>(SelectionMode.Deep);
            foreach (Text label in labels)
            {
                label.text = label.text.Replace(oldValue, newValue);
                Debug.Log(label.name + ":" + label.text);
                EditorUtility.SetDirty(label);
            }
        }
        else if (selected == 1)
        {
            float radio = (float)toFont.lineHeight / toFont.fontSize;
            Text[] labels = Selection.GetFiltered<Text>(SelectionMode.Deep);
            foreach (Text label in labels)
            {
                label.font = toFont;
                //label.alignByGeometry = alignByGeometry;

                switch (toFontStyle)
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

                RectTransform labelRect = label.GetComponent<RectTransform>();
                RectTransform parentRect = null;
                float size = (float)Math.Ceiling(radio * label.fontSize);
                if (size == radio * label.fontSize && size % 2 == 1)
                {
                    size = size + 1;
                }
                switch (resizeMode)
                {
                    case ResizeMode.None:
                        break;
                    case ResizeMode.Auto:
                        if (labelRect.anchorMin == Vector2.zero && labelRect.anchorMax == Vector2.one)
                        {
                            parentRect = labelRect.parent.GetComponent<RectTransform>();
                            if (parentRect != null && parentRect.rect.size.y < size)
                            {
                                parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
                            }

                            if (parentRect != null && parentRect.rect.size.y >= size)
                            {
                                float _size = (parentRect.rect.size.y - size);
                                float _witch = labelRect.offsetMax.y * -1 + labelRect.offsetMin.y;
                                float _radioUpper = labelRect.offsetMax.y / _witch;
                                float _radioLower = labelRect.offsetMin.y / _witch;

                                if (_witch > _size)
                                {
                                    labelRect.offsetMax = new Vector2(labelRect.offsetMax.x, _radioUpper * _size);
                                    labelRect.offsetMin = new Vector2(labelRect.offsetMin.x, _radioLower * _size);
                                }
                            }
                        }
                        else
                        {
                            if (labelRect != null && labelRect.rect.size.y < size)
                            {
                                labelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
                            }
                        }
                        break;
                    default:
                        break;
                }

                Debug.Log(label.name + ":" + label.text + " - size:" + labelRect.rect.size.y);
                EditorUtility.SetDirty(label);
            }
        }
    }
}