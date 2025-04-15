using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
[CanEditMultipleObjects]
[CustomEditor(typeof(HierarchyMenu))]
public class HierarchyMenu : Editor
{
    [MenuItem("GameObject/Tools/Create Guide", false, 0)]
    static void CreateGuide()
    {
        if (Selection.objects.Length > 0)
        {
            //获取选中的对象
            GameObject selection = Selection.objects[0] as GameObject;
            GameObject root = selection.transform.GetRoot().gameObject;
            if (true)
            {
                Debug.Log(root.name);
            }
            //EditorUtility.DisplayDialog("selection", selection.name, "ok");
        }
    }
}