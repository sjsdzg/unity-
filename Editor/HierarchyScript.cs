﻿using UnityEngine;
using UnityEditor;

public class HierarchyScript : ScriptableObject
{
    private static TextEditor textEditor = new TextEditor();
    [MenuItem("GameObject/CopyPath _F1", priority = 12)]
    private static void CopyPathMenuOption()
    {
        if (Selection.activeGameObject)
        {
            string s = GetTransPath(Selection.activeGameObject.transform);
            textEditor.text = s;
            textEditor.OnFocus();
            textEditor.Copy();
            Debug.Log("[CopyPath]:" + s);
        }
    }

    /// <summary>
    /// 获得GameObject在Hierarchy中的完整路径
    /// </summary>
    public static string GetTransPath(Transform trans)
    {
        if (!trans.parent)
        {
            return trans.name;

        }
        return GetTransPath(trans.parent) + "/" + trans.name;
    }
}