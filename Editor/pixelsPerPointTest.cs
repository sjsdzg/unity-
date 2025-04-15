using UnityEngine;
using UnityEditor;

public class pixelsPerPointTest : ScriptableObject
{
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        //EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        Debug.Log(EditorGUIUtility.pixelsPerPoint);
        //Material mat = (Material)EditorGUIUtility.LoadRequired("SceneView/HandleDottedLines.mat");
        //AssetDatabase.CreateAsset(mat, "Assets/HandleDottedLines__.mat");
        AssetDatabase.Refresh();
        Object[] UnityAssets = AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
        foreach (var asset in UnityAssets)
        {
            AssetDatabase.CreateAsset(asset, "Assets/Tests" + asset.name);
        }
        AssetDatabase.Refresh();
    }
}