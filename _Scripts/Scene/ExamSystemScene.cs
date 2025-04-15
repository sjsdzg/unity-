using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.UI
{
    public class ExamSystemScene : BaseScene
    {
        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();
            paths.Add(UIDefine.GetUIPrefabPath(EnumUIType.ExamSystemUI));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUICloseOthers(EnumUIType.ExamSystemUI);
            ScreenFader.Instance.FadeOut().Execute();
        }
    }
}
