using UnityEngine;
using UnityEditor;

public class PrefabLightmapDataEditor : Editor
{
    [MenuItem("Tools/保存烘焙信息", false, 0)]
    static void SaveLightmapInfoByGameObject()
    {
        GameObject go = Selection.activeGameObject;

        if (null == go) return;

        PrefabLightmapData data = go.GetComponent<PrefabLightmapData>();
        if (data == null)
        {
            data = go.AddComponent<PrefabLightmapData>();
        }
        //save lightmapdata info by mesh.render
        data.SaveLightmap();

        EditorUtility.SetDirty(go);
        //applay prefab
        var targetPrefab = PrefabUtility.GetPrefabParent(go) as GameObject;
        if (targetPrefab != null)
        {
            PrefabUtility.ReplacePrefab(go, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }
}