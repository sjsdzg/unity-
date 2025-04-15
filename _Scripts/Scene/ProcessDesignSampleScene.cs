using UnityEngine;
using System.Collections;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.UI
{
    public class ProcessDesignSampleScene : BaseScene
    {
        protected override void OnLoad()
        {
            base.OnLoad();
            ScreenFader.Instance.FadeOut().Execute();
        }
    }
}

