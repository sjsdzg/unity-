using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.UI
{
    public class ProductionMainScene : BaseScene
    {

        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            //paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.ProductionMainLiteUI));//无沙盘webgl
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.ProductionMainUI));//有沙盘
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            //UIManager.Instance.OpenUI(EnumUIType.ProductionMainLiteUI);//无沙盘webgl
            UIManager.Instance.OpenUI(EnumUIType.ProductionMainUI);//有沙盘
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            //UIManager.Instance.CloseUI(EnumUIType.ProductionMainLiteUI);//无沙盘webgl
            UIManager.Instance.CloseUI(EnumUIType.ProductionMainUI);//有沙盘
        }
    }
}

