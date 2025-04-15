using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Crosstales.FB
{
    /// <summary>Native file browser various actions like open file, open folder and save file.</summary>
    public class FileBrowser
    {
        #region Properties

        /// <summary>Indicates if this wrapper can open multiple files.</summary>
        /// <returns>Wrapper can open multiple files.</returns>
        public static bool canOpenMultipleFiles
        {
            get { return platformWrapper.canOpenMultipleFiles; }
        }

        /// <summary>Indicates if this wrapper can open multiple folders.</summary>
        /// <returns>Wrapper can open multiple folders.</returns>
        public static bool canOpenMultipleFolders
        {
            get { return platformWrapper.canOpenMultipleFolders; }
        }

        /// <summary>Indicates if this wrapper is supporting the current platform.</summary>
        /// <returns>True if this wrapper supports current platform.</returns>
        public static bool isPlatformSupported
        {
            get { return platformWrapper.isPlatformSupported; }
        }

        #endregion

        #region Variables

        private static Wrapper.IFileBrowser platformWrapper;

        #endregion


        #region Constructor

        static FileBrowser()
        {
#if UNITY_STANDALONE_WIN
            platformWrapper = new Wrapper.FileBrowserWindows();
#elif UNITY_STANDALONE_OSX
            platformWrapper = new Wrapper.FileBrowserMac();
#elif UNITY_STANDALONE_LINUX
            platformWrapper = new Wrapper.FileBrowserLinux();
#elif UNITY_EDITOR
            platformWrapper = new Wrapper.FileBrowserEditor();
#else
            platformWrapper = new Wrapper.FileBrowserGeneric();
#endif
        }

        #endregion

        #region Public methods

        /// <summary>Open native file browser for a single file.</summary>
        /// <param name="extension">Allowed extension, e.g. "png" (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string OpenSingleFile(string extension = "*")
        {
            return OpenSingleFile(FileBrowserUtil.TEXT_OPEN_FILE, string.Empty, getFilter(extension));
        }

        /// <summary>Open native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">Allowed extensions, e.g. "png" (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string OpenSingleFile(string title, string directory, params string[] extensions)
        {
            return OpenSingleFile(title, directory, getFilter(extensions));
        }

        /// <summary>Open native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string OpenSingleFile(string title, string directory, params ExtensionFilter[] extensions)
        {
            return platformWrapper.OpenSingleFile(title, directory, extensions);
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="extension">Allowed extension, e.g. "png" (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string[] OpenFiles(string extension = "*")
        {
            return OpenFiles(FileBrowserUtil.TEXT_OPEN_FILES, string.Empty, getFilter(extension));
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">Allowed extensions, e.g. "png" (optional)</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static string[] OpenFiles(string title, string directory, params string[] extensions)
        {
            return OpenFiles(title, directory, getFilter(extensions));
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static string[] OpenFiles(string title, string directory, params ExtensionFilter[] extensions)
        {
            return platformWrapper.OpenFiles(title, directory, extensions, true);
        }

        /// <summary>Open native folder browser for a single folder.</summary>
        /// <returns>Returns a string of the chosen folder. Empty string when cancelled</returns>
        public static string OpenSingleFolder()
        {
            return OpenSingleFolder(FileBrowserUtil.TEXT_OPEN_FOLDER);
        }

        /// <summary>
        /// Open native folder browser for a single folder.
        /// NOTE: Title is not supported under Windows and UWP (WSA)!
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <returns>Returns a string of the chosen folder. Empty string when cancelled</returns>
        public static string OpenSingleFolder(string title, string directory = "")
        {
            return platformWrapper.OpenSingleFolder(title, directory);
        }

        /// <summary>
        /// Open native folder browser for multiple folders.
        /// NOTE: Title and multiple folder selection are not supported under Windows and UWP (WSA)!
        /// </summary>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static string[] OpenFolders()
        {
            return OpenFolders(FileBrowserUtil.TEXT_OPEN_FOLDERS);
        }

        /// <summary>
        /// Open native folder browser for multiple folders.
        /// NOTE: Title and multiple folder selection are not supported under Windows and UWP (WSA)!
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static string[] OpenFolders(string title, string directory = "")
        {
            return platformWrapper.OpenFolders(title, directory, true);
        }

        /// <summary>Open native save file browser.</summary>
        /// <param name="defaultName">Default file name (optional)</param>
        /// <param name="extension">File extensions, e.g. "png" (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static string SaveFile(string defaultName = "", string extension = "*")
        {
            return SaveFile(FileBrowserUtil.TEXT_SAVE_FILE, string.Empty, defaultName, getFilter(extension));
        }

        /// <summary>Open native save file browser.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">File extensions, e.g. "png" (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static string SaveFile(string title, string directory, string defaultName, params string[] extensions)
        {
            return SaveFile(title, directory, defaultName, getFilter(extensions));
        }

        /// <summary>Open native save file browser</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static string SaveFile(string title, string directory, string defaultName, params ExtensionFilter[] extensions)
        {
            return platformWrapper.SaveFile(title, directory, string.IsNullOrEmpty(defaultName) ? FileBrowserUtil.TEXT_SAVE_FILE_NAME : defaultName, extensions);
        }

        /// <summary>Asynchronously opens native file browser for a single file.</summary>
        /// <param name="extension">Allowed extension, e.g. "png" (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static void OpenSingleFileAsync(System.Action<string[]> action, string extension = "*")
        {
            OpenSingleFileAsync(action, FileBrowserUtil.TEXT_OPEN_FILE, string.Empty, getFilter(extension));
        }

        /// <summary>Asynchronously opens native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">Allowed extensions, e.g. "png" (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static void OpenSingleFileAsync(System.Action<string[]> action, string title, string directory, params string[] extensions)
        {
            OpenSingleFileAsync(action, title, directory, getFilter(extensions));
        }

        /// <summary>Asynchronously opens native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static void OpenSingleFileAsync(System.Action<string[]> action, string title, string directory, params ExtensionFilter[] extensions)
        {
            platformWrapper.OpenFilesAsync(title, directory, extensions, false, action);
        }

        /// <summary>Asynchronously opens native file browser for multiple files.</summary>
        /// <param name="multiselect">Allow multiple file selection (default: true, optional)</param>
        /// <param name="extensions">Allowed extensions, e.g. "png" (optional)</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static void OpenFilesAsync(System.Action<string[]> action, bool multiselect = true, params string[] extensions)
        {
            OpenFilesAsync(action, multiselect ? FileBrowserUtil.TEXT_OPEN_FILES : FileBrowserUtil.TEXT_OPEN_FILE, string.Empty, multiselect, getFilter(extensions));
        }

        /// <summary>Asynchronously opens native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect">Allow multiple file selection (default: true, optional)</param>
        /// <param name="extensions">Allowed extensions, e.g. "png" (optional)</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static void OpenFilesAsync(System.Action<string[]> action, string title, string directory, bool multiselect = true, params string[] extensions)
        {
            OpenFilesAsync(action, title, directory, multiselect, getFilter(extensions));
        }

        /// <summary>Asynchronously opens native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect">Allow multiple file selection (default: true, optional)</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static void OpenFilesAsync(System.Action<string[]> action, string title, string directory, bool multiselect = true, params ExtensionFilter[] extensions)
        {
            platformWrapper.OpenFilesAsync(title, directory, extensions, multiselect, action);
        }

        /// <summary>Asynchronously opens native folder browser for a single folder.</summary>
        /// <returns>Returns a string of the chosen folder. Empty string when cancelled</returns>
        public static void OpenSingleFolderAsync(System.Action<string[]> action)
        {
            OpenSingleFolderAsync(action, FileBrowserUtil.TEXT_OPEN_FOLDER);
        }

        /// <summary>
        /// Asynchronously opens native folder browser for a single folder.
        /// NOTE: Title is not supported under Windows and UWP (WSA)!
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <returns>Returns a string of the chosen folder. Empty string when cancelled</returns>
        public static void OpenSingleFolderAsync(System.Action<string[]> action, string title, string directory = "")
        {
            platformWrapper.OpenFoldersAsync(title, directory, false, action);
        }

        /// <summary>Asynchronously opens native folder browser for multiple folders.</summary>
        /// <param name="multiselect">Allow multiple folder selection (default: true, optional)</param>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static void OpenFoldersAsync(System.Action<string[]> action, bool multiselect = true)
        {
            OpenFoldersAsync(action, FileBrowserUtil.TEXT_OPEN_FOLDERS, string.Empty, multiselect);
        }

        /// <summary>Asynchronously opens native folder browser for multiple folders.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <param name="multiselect">Allow multiple folder selection (default: true, optional)</param>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static void OpenFoldersAsync(System.Action<string[]> action, string title, string directory = "", bool multiselect = true)
        {
            platformWrapper.OpenFoldersAsync(title, directory, multiselect, action);
        }

        /// <summary>Asynchronously opens native save file browser.</summary>
        /// <param name="defaultName">Default file name (optional)</param>
        /// <param name="extension">File extension, e.g. "png" (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static void SaveFileAsync(System.Action<string> action, string defaultName = "", string extension = "*")
        {
            SaveFileAsync(action, FileBrowserUtil.TEXT_SAVE_FILE, string.Empty, defaultName, getFilter(extension));
        }

        /// <summary>Asynchronously opens native save file browser.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">File extensions, e.g. "png" (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static void SaveFileAsync(System.Action<string> action, string title, string directory, string defaultName, params string[] extensions)
        {
            SaveFileAsync(action, title, directory, defaultName, getFilter(extensions));
        }

        /// <summary>Asynchronously opens native save file browser (async)</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters (optional)</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static void SaveFileAsync(System.Action<string> action, string title, string directory, string defaultName, params ExtensionFilter[] extensions)
        {
            platformWrapper.SaveFileAsync(title, directory, string.IsNullOrEmpty(defaultName) ? FileBrowserUtil.TEXT_SAVE_FILE_NAME : defaultName, extensions, action);
        }

        #endregion


        #region Private methods

        private static ExtensionFilter[] getFilter(params string[] extensions)
        {
            if (extensions != null && extensions.Length > 0)
            {
                //Debug.Log("Extension: " + extensions[0]);

                if (extensions.Length == 1 && "*".Equals(extensions[0]))
                {
                    //Debug.Log("Wildcard!");
                    return null;
                }

                ExtensionFilter[] filter = new ExtensionFilter[extensions.Length];

                for (int ii = 0; ii < extensions.Length; ii++)
                {
                    string extension = string.IsNullOrEmpty(extensions[ii]) ? "*" : extensions[ii];

                    if (extension.Equals("*"))
                    {
                        filter[ii] = new ExtensionFilter(FileBrowserUtil.TEXT_ALL_FILES, FileBrowserUtil.isMacOSEditor ? string.Empty : extension);
                    }
                    else
                    {
                        filter[ii] = new ExtensionFilter(extension, extension);
                    }
                }

                return filter;
            }

            //Debug.Log("Wildcard!");
            return null;
        }

        #endregion

    }

    /// <summary>Filter for extensions.</summary>
    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions)
        {
            Name = filterName;
            Extensions = filterExtensions;
        }

        public override string ToString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            result.Append(GetType().Name);

            result.Append("Name='");
            result.Append(Name);

            result.Append("Extensions='");
            result.Append(Extensions.Dump());

            return result.ToString();
        }
    }
}
// © 2017-2020 crosstales LLC (https://www.crosstales.com)