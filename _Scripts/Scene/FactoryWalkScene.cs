using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.UI
{
    public class FactoryWalkScene : BaseScene
    {

        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.FactoryWalkUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUI(EnumUIType.FactoryWalkUI);
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(EnumUIType.FactoryWalkUI);
        }
    }
}

