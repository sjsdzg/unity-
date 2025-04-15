using UnityEngine;
using System.Collections;
using UnityEditor;
using XFramework.UIWidgets;
using UnityEditor.UI;

[CustomEditor(typeof(UIButton), true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor
{
    //使用SerializedProperty 必须在UIButton类的字段前加[SerializeField]
    private SerializedProperty m_IconGraphicProperty;
    private SerializedProperty m_IconColorsProperty;
    private SerializedProperty m_TextGraphicProperty;
    private SerializedProperty m_TextColorsProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_IconGraphicProperty = serializedObject.FindProperty("m_IconGraphic");
        m_IconColorsProperty = serializedObject.FindProperty("m_IconColors");
        m_TextGraphicProperty = serializedObject.FindProperty("m_TextGraphic");
        m_TextColorsProperty = serializedObject.FindProperty("m_TextColors");
    }

    /// <summary>
    /// 如果用这种序列化方式，需要在 OnInspectorGUI 开头和结尾各加一句 serializedObject.Update();  serializedObject.ApplyModifiedProperties();
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(); //空行

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_IconGraphicProperty);
        if (m_IconGraphicProperty.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(m_IconColorsProperty);
        }
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_TextGraphicProperty);
        if (m_TextGraphicProperty.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(m_TextColorsProperty);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
