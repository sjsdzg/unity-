using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 生产操作场景（精简版）
    /// </summary>
    public class ProductionMainLiteScene : BaseScene
    {
        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.ProductionMainLiteUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUI(EnumUIType.ProductionMainLiteUI);
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(EnumUIType.ProductionMainLiteUI);
        }
    }
}

