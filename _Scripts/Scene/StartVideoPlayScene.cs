using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.UI
{
    public class StartVideoPlayScene : BaseScene
    {

        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.StartVideoPlayUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUI(EnumUIType.StartVideoPlayUI, 0);
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(EnumUIType.StartVideoPlayUI);
        }
    }
}

