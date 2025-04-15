using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class EngineeringDesignScene : BaseScene
    {
        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.EngineeringDesignUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUI(EnumUIType.EngineeringDesignUI);
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            //EngineeringDesignFluidManager.Instance.Release();
            UIManager.Instance.CloseUI(EnumUIType.EngineeringDesignUI);
        }
    }
}

