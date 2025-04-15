using UnityEngine;
using UnityEditor;
using System.IO;
using XFramework.Module;
using System.Collections.Generic;
using XFramework.Core;
using System.Text;

public class EquipmentExtensionsScript : ScriptableObject
{
    [MenuItem("Tools/EquipmentExtensions/Build Json")]
    static void DoIt()
    {
        string path = Application.streamingAssetsPath + "/EquipmentExtensions";
        if (!Directory.Exists(path))
            return;

        string[] folders = Directory.GetDirectories(path);
        foreach (string folder in folders)
        {
            ExtensionInfo extensionInfo = new ExtensionInfo();
            extensionInfo.Items = new List<ExtensionItemInfo>();
            
            DirectoryInfo TheFolder = new DirectoryInfo(folder);
            foreach (var file in TheFolder.GetFiles())
            {
                ExtensionItemInfo item = new ExtensionItemInfo();
                item.Name = file.Name;
                switch (file.Extension)
                {
                    case ".pdf":
                        item.ExpansionType = ExpansionType.PDF;
                        item.Sprite = "PDF";
                        extensionInfo.Items.Add(item);
                        break;
                    case ".swf":
                        item.ExpansionType = ExpansionType.SWF;
                        item.Sprite = "SWF";
                        extensionInfo.Items.Add(item);
                        break;
                    case ".mp4":
                    case ".avi":
                        item.ExpansionType = ExpansionType.VIDEO;
                        item.Sprite = "VIDEO";
                        extensionInfo.Items.Add(item);
                        break;
                    default:
                        break;
                }
                
            }
            string tempPath = folder.Replace("\\", "/");
            string xmlPath = tempPath + "/" + tempPath.Substring(tempPath.LastIndexOf("/") + 1) + ".json";
            using (FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string json = extensionInfo.ToJson();
                    sw.Write(json);
                }
            }
        }

        AssetDatabase.Refresh();
    }
}