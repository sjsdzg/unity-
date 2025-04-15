using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FixMatShader {

    [MenuItem("Tools/Fix Standard Shader")]
    static void FixStandardShader()
    {
        Shader shader = Shader.Find("Standard");
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        List<Material> materials = new List<Material>();
        for (int i = 0; i < assetPaths.Length; i++)
        {
            string ext = Path.GetExtension(assetPaths[i]);
            if (ext != ".mat")
            {
                continue;
            }
            materials.Add(AssetDatabase.LoadAssetAtPath(assetPaths[i], typeof(Material)) as Material);
        }

        if (materials.Count != 0)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i] == null)
                {
                    continue;
                }

                if (materials[i].shader.name == "Hidden/InternalErrorShader")
                {
                    materials[i].shader = shader;
                }

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("Fix Standard Shader", "正在 Fix Standard Shader 中...", 1f * (i + 1) / materials.Count);
                if (isCancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
