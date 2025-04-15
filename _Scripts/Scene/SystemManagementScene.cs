using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.UI
{
    public class SystemManagementScene : BaseScene
    {
        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.SystemManagementUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUICloseOthers(EnumUIType.SystemManagementUI);
            ScreenFader.Instance.FadeOut().Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(EnumUIType.SystemManagementUI);
        }
    }
}
