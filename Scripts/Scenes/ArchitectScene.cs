using UnityEngine;
using System.Collections;
using XFramework.Architectural;
using XFramework.Core;
using System;
using XFramework.Common;

namespace XFramework
{
    public class ArchitectScene : BaseScene
    {

        protected override void OnLoad()
        {
            base.OnLoad();
            Architect.Instance.Enable = true;
            PrefSettings.ResetAllToDefault();

            Document document = Document.LoadFromResources("Architect/Groups/base.arch");
            Architect.Instance.CurrentDocument = document;

            GraphicManager.Instance.Show(ViewMode.Drawing);
            ScreenFader.Instance.FadeOut(1).Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            EventDispatcher.UnregisterAllEvent();
            Camera.main.gameObject.SetActive(false);
            Architect.Instance.Release();
            GraphicManager.Instance.Release();
        }
    }
}
