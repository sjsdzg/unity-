using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework
{
    public class BootstrapScene : BaseScene
    {
        [DllImport("__Internal")]
        private static extern void GetAppSettings();


#if UNITY_WEBGL && !UNITY_EDITOR
        public void InitializeAppSettings(string json)
        {
            Debug.Log("json : " + json);
            AppSettings.Settings.ParseFromJson(json);
            base.OnAwake();
        }
#endif


        protected override void OnAwake()
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            base.OnAwake();
#else 
            GetAppSettings();
#endif
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Launch();
        }

        private void Launch()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            QualitySettings.vSyncCount = 0;
            QualitySettings.SetQualityLevel(2);
            Paroxe.PdfRenderer.WebGL.PDFJS_Library.Instance.OnLibraryInitialized("");
#else
            QualitySettings.vSyncCount = 1;
            QualitySettings.SetQualityLevel(5);
            Screen.SetResolution(1366, 768, false);
#endif
            App.Instance.StartUp();
        }

    }
}
