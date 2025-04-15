using UnityEngine;
using System.Collections;
using XFramework.Core;
using XFramework.Common;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class LoginScene : BaseScene
    {
        private EnumUIType uiType = EnumUIType.LoginUI;

        protected override List<string> GetPreloadPaths()
        {
            List<string> paths = new List<string>();

#if ILAB_X
            if (App.Instance.VersionTag == VersionTag.CZDX)
            {
                uiType = EnumUIType.OpenLoginUI;
            }
#endif

            paths.Add(UIDefine.GetUIPrefabPath(uiType));
            return paths;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            UIManager.Instance.OpenUI(uiType);
            ScreenFader.Instance.FadeOut(1).Execute();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            UIManager.Instance.CloseUI(uiType);
        }
    }
}


