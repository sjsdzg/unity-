using UnityEngine;
using System.Collections;
using Crosstales.FB;

public class FileBrowerSample01 : MonoBehaviour
{

    private string _path;

    void OnGUI()
    {
        //var guiScale = new Vector3(Screen.width / 800.0f, Screen.height / 600.0f, 1.0f);
        //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        // Open File Samples

        if (GUILayout.Button("Open File"))
        {
            WriteResult(FileBrowser.OpenSingleFile());
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open File Async"))
        {
            FileBrowser.OpenSingleFileAsync(paths => { WriteResult(paths); } ,"Open File", "", "*");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open File Multiple"))
        {
            WriteResult(FileBrowser.OpenFiles("Open File", "", ""));
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open File Extension"))
        {
            WriteResult(FileBrowser.OpenSingleFile("Open File", "", "txt"));
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open File Directory"))
        {
            WriteResult(FileBrowser.OpenFiles("Open File", Application.dataPath, ""));
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open File Filter"))
        {
            var extensions = new[] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
                new ExtensionFilter("Sound Files", "mp3", "wav" ),
                new ExtensionFilter("All Files", "*" ),
            };
            WriteResult(FileBrowser.OpenFiles("Open File", "", extensions));
        }

        GUILayout.Space(15);

        // Open Folder Samples

        if (GUILayout.Button("Open Folder"))
        {
            var paths = FileBrowser.OpenFolders("Select Folder", "");
            WriteResult(paths);
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open Folder Async"))
        {
            FileBrowser.OpenFoldersAsync(paths => { WriteResult(paths); }, "Select Folder", "", true);
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Open Folder Directory"))
        {
            var paths = FileBrowser.OpenFolders("Select Folder", Application.dataPath);
            WriteResult(paths);
        }

        GUILayout.Space(15);

        // Save File Samples

        if (GUILayout.Button("Save File"))
        {
            _path = FileBrowser.SaveFile("MySaveFile", "txt"); ;
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Save File Async"))
        {
            FileBrowser.SaveFileAsync(path => { WriteResult(path); } , "Save File", "", "", "*");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Save File Default Name"))
        {
            _path = FileBrowser.SaveFile("Save File", "", "MySaveFile", "");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Save File Default Name Ext"))
        {
            _path = FileBrowser.SaveFile("Save File", "", "MySaveFile", "dat");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Save File Directory"))
        {
            _path = FileBrowser.SaveFile("Save File", Application.dataPath, "", "");
        }
        GUILayout.Space(5);
        if (GUILayout.Button("Save File Filter"))
        {
            // Multiple save extension filters with more than one extension support.
            var extensionList = new[] {
                new ExtensionFilter("Binary", "bin"),
                new ExtensionFilter("Text", "txt"),
            };
            _path = FileBrowser.SaveFile("Save File", "", "MySaveFile", extensionList);
        }

        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.Label(_path);
        GUILayout.EndHorizontal();
    }

    public void WriteResult(string[] paths)
    {
        if (paths.Length == 0)
        {
            return;
        }

        _path = "";
        foreach (var p in paths)
        {
            _path += p + "\n";
        }
    }

    public void WriteResult(string path)
    {
        _path = path;
    }
}
