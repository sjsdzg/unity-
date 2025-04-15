using UnityEngine;
using UnityEditor;
using System.IO;

public class MoveScript : ScriptableObject
{
    [MenuItem("Tools/Move/Icons of Equipment")]
    static void DoIt()
    {
        string fromPath = "Assets/Resources/DeviceSimulation";
        string toPath = "Assets/Textures/Equipments";

        string[] SubFolders = Directory.GetDirectories(fromPath);
        
        foreach (string item in SubFolders)
        {
            string path = item.Replace("\\", "/");
            string folder = path.Substring(path.LastIndexOf('/') + 1);
            
            if (!Directory.Exists(toPath + "/" + folder))
            {
                AssetDatabase.CreateFolder(toPath, folder);
            }
            string newFolder = toPath + "/" + folder;

            if (Directory.Exists(path + "/Icons"))
            {
                string[] icons = Directory.GetFiles(path + "/Icons", "*.png");
                foreach (string icon in icons)
                {
                    string iconPath = icon.Replace("\\", "/");
                    string newPath = newFolder + "/" + iconPath.Substring(iconPath.LastIndexOf('/') + 1);
                    File.Copy(iconPath, newPath);
                }
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Cleanup Missing Scripts")]
    static void CleanupMissingScripts()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            var gameObject = Selection.gameObjects[i];

            // We must use the GetComponents array to actually detect missing components
            var components = gameObject.GetComponentsInChildren<Component>();

            foreach (var trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                CleanupMissingScripts(trans.gameObject, gameObject);
            }
        }
    }

    static void CleanupMissingScripts(GameObject go, GameObject parent)
    {
        // We must use the GetComponents array to actually detect missing components
        var components = go.GetComponents<Component>();

        // Create a serialized object so that we can edit the component list
        var serializedObject = new SerializedObject(go);
        // Find the component list property
        var prop = serializedObject.FindProperty("m_Component");

        // Track how many components we've removed
        int r = 0;

        // Iterate over all components
        for (int j = 0; j < components.Length; j++)
        {
            // Check if the ref is null
            if (components[j] == null)
            {
                // If so, remove from the serialized component array
                prop.DeleteArrayElementAtIndex(j - r);

                Debug.Log("成功移除丢失脚本，gameObject : " + go.name + " --- parent：" + parent.name);
                // Increment removed count
                r++;
            }
        }

        // Apply our changes to the game object
        serializedObject.ApplyModifiedProperties();
    }
}