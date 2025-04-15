using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crosstales.FB
{
    public class FileBrowserUtil
    {
        /// <summary>Path delimiter for Windows.</summary>
        public const string PATH_DELIMITER_WINDOWS = @"\";

        /// <summary>Path delimiter for Unix.</summary>
        public const string PATH_DELIMITER_UNIX = "/";

        #region Changable variables

        // Text fragments for the asset
        public static string TEXT_OPEN_FILE = "Open file";
        public static string TEXT_OPEN_FILES = "Open files";
        public static string TEXT_OPEN_FOLDER = "Open folder";
        public static string TEXT_OPEN_FOLDERS = "Open folders";
        public static string TEXT_SAVE_FILE = "Save file";
        public static string TEXT_ALL_FILES = "All files";
        public static string TEXT_SAVE_FILE_NAME = "MySaveFile";

        #endregion


        /// <summary>Checks if the current platform is Windows.</summary>
        /// <returns>True if the current platform is Windows.</returns>
        public static bool isWindowsPlatform
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return true;
#else
            return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is OSX.</summary>
        /// <returns>True if the current platform is OSX.</returns>
        public static bool isMacOSPlatform
        {
            get
            {
#if UNITY_STANDALONE_OSX
             return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is Linux.</summary>
        /// <returns>True if the current platform is Linux.</returns>
        public static bool isLinuxPlatform
        {
            get
            {
#if UNITY_STANDALONE_LINUX
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is standalone (Windows, macOS or Linux).</summary>
        /// <returns>True if the current platform is standalone (Windows, macOS or Linux).</returns>
        public static bool isStandalonePlatform => isWindowsPlatform || isMacOSPlatform || isLinuxPlatform;

        /// <summary>Checks if the current platform is Android.</summary>
        /// <returns>True if the current platform is Android.</returns>
        public static bool isAndroidPlatform
        {
            get
            {
#if UNITY_ANDROID
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is iOS.</summary>
        /// <returns>True if the current platform is iOS.</returns>
        public static bool isIOSPlatform
        {
            get
            {
#if UNITY_IOS
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is tvOS.</summary>
        /// <returns>True if the current platform is tvOS.</returns>
        public static bool isTvOSPlatform
        {
            get
            {
#if UNITY_TVOS
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is WSA.</summary>
        /// <returns>True if the current platform is WSA.</returns>
        public static bool isWSAPlatform
        {
            get
            {
#if UNITY_WSA
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is XboxOne.</summary>
        /// <returns>True if the current platform is XboxOne.</returns>
        public static bool isXboxOnePlatform
        {
            get
            {
#if UNITY_XBOXONE
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is PS4.</summary>
        /// <returns>True if the current platform is PS4.</returns>
        public static bool isPS4Platform
        {
            get
            {
#if UNITY_PS4
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is WebGL.</summary>
        /// <returns>True if the current platform is WebGL.</returns>
        public static bool isWebGLPlatform
        {
            get
            {
#if UNITY_WEBGL
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if the current platform is Web (WebPlayer or WebGL).</summary>
        /// <returns>True if the current platform is Web (WebPlayer or WebGL).</returns>
        public static bool isWebPlatform => isWebGLPlatform;

        /// <summary>Checks if the current platform is Windows-based (Windows standalone, WSA or XboxOne).</summary>
        /// <returns>True if the current platform is Windows-based (Windows standalone, WSA or XboxOne).</returns>
        public static bool isWindowsBasedPlatform => isWindowsPlatform || isWSAPlatform || isXboxOnePlatform;

        /// <summary>Checks if the current platform is WSA-based (WSA or XboxOne).</summary>
        /// <returns>True if the current platform is WSA-based (WSA or XboxOne).</returns>
        public static bool isWSABasedPlatform => isWSAPlatform || isXboxOnePlatform;

        /// <summary>Checks if the current platform is Apple-based (macOS standalone, iOS or tvOS).</summary>
        /// <returns>True if the current platform is Apple-based (macOS standalone, iOS or tvOS).</returns>
        public static bool isAppleBasedPlatform => isMacOSPlatform || isIOSPlatform || isTvOSPlatform;

        /// <summary>Checks if the current platform is iOS-based (iOS or tvOS).</summary>
        /// <returns>True if the current platform is iOS-based (iOS or tvOS).</returns>
        public static bool isIOSBasedPlatform => isIOSPlatform || isTvOSPlatform;

        /// <summary>Checks if we are inside the Editor.</summary>
        /// <returns>True if we are inside the Editor.</returns>
        public static bool isEditor => isWindowsEditor || isMacOSEditor || isLinuxEditor;

        /// <summary>Checks if we are inside the Windows Editor.</summary>
        /// <returns>True if we are inside the Windows Editor.</returns>
        public static bool isWindowsEditor
        {
            get
            {
#if UNITY_EDITOR_WIN
                return true;
#else
            return false;
#endif
            }
        }

        /// <summary>Checks if we are inside the macOS Editor.</summary>
        /// <returns>True if we are inside the macOS Editor.</returns>
        public static bool isMacOSEditor
        {
            get
            {
#if UNITY_EDITOR_OSX
            return true;
#else
                return false;
#endif
            }
        }

        /// <summary>Checks if we are inside the Linux Editor.</summary>
        /// <returns>True if we are inside the Linux Editor.</returns>
        public static bool isLinuxEditor
        {
            get
            {
#if UNITY_EDITOR_LINUX
            return true;
#else
                return false;
#endif
            }
        }

        protected static readonly System.Random rnd = new System.Random();

        /// <summary>Creates a string of characters with a given length.</summary>
        /// <param name="replaceChars">Characters to generate the string (if more than one character is used, the generated string will be a randomized result of all characters)</param>
        /// <param name="stringLength">Length of the generated string</param>
        /// <returns>Generated string</returns>
        public static string CreateString(string replaceChars, int stringLength)
        {
            if (replaceChars.Length > 1)
            {
                char[] chars = new char[stringLength];

                for (int ii = 0; ii < stringLength; ii++)
                {
                    chars[ii] = replaceChars[rnd.Next(0, replaceChars.Length)];
                }

                return new string(chars);
            }

            return replaceChars.Length == 1 ? new string(replaceChars[0], stringLength) : string.Empty;
        }

        /// <summary>Validates a given path and add missing slash.</summary>
        /// <param name="path">Path to validate</param>
        /// <param name="addEndDelimiter">Add delimiter at the end of the path (optional, default: true)</param>
        /// <returns>Valid path</returns>
        public static string ValidatePath(string path, bool addEndDelimiter = true)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string pathTemp = path.Trim();
                string result;

                if ((isWindowsBasedPlatform || isWindowsEditor) && !isMacOSEditor && !isLinuxEditor)
                {
                    result = pathTemp.Replace('/', '\\');

                    if (addEndDelimiter)
                    {
                        if (!result.EndsWith(PATH_DELIMITER_WINDOWS))
                        {
                            result += PATH_DELIMITER_WINDOWS;
                        }
                    }
                }
                else
                {
                    result = pathTemp.Replace('\\', '/');

                    if (addEndDelimiter)
                    {
                        if (!result.EndsWith(PATH_DELIMITER_UNIX))
                        {
                            result += PATH_DELIMITER_UNIX;
                        }
                    }
                }

                return string.Join(string.Empty, result.Split(System.IO.Path.GetInvalidPathChars()));
            }

            return path;
        }

        /// <summary>Validates a given file.</summary>
        /// <param name="path">File to validate</param>
        /// <returns>Valid file path</returns>
        public static string ValidateFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string result = ValidatePath(path);

                if (result.EndsWith(PATH_DELIMITER_WINDOWS) ||
                    result.EndsWith(PATH_DELIMITER_UNIX))
                {
                    result = result.Substring(0, result.Length - 1);
                }

                string fileName;
                if ((isWindowsBasedPlatform || isWindowsEditor) && !isMacOSEditor && !isLinuxEditor)
                {
                    fileName = result.Substring(result.LastIndexOf(PATH_DELIMITER_WINDOWS) + 1);
                }
                else
                {
                    fileName = result.Substring(result.LastIndexOf(PATH_DELIMITER_UNIX) + 1);
                }

                string newName =
                   string.Join(string.Empty,
                      fileName.Split(System.IO.Path
                         .GetInvalidFileNameChars())); //.Replace(BaseConstants.PATH_DELIMITER_WINDOWS, string.Empty).Replace(BaseConstants.PATH_DELIMITER_UNIX, string.Empty);

                return result.Substring(0, result.Length - fileName.Length) + newName;
            }

            return path;
        }
    }
}
