using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XFramework.Core
{
#if UNITY_WEBGL
    public static class WebGLUtils
    {
        [DllImport("__Internal")]
        public static extern void GetAppSettings();

        [DllImport("__Internal")]
        public static extern void OpenUrl(string url);

        [DllImport("__Internal")]
        public static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

        [DllImport("__Internal")]
        private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);
    }
#endif
}
