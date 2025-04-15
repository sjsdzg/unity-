using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Common;

namespace XFramework.Common
{
    public class TooltipAction : ActionBase
    {
        public string text = "";
        public bool isShow = true;
        public float wait = 0.4f;

        public TooltipAction(bool _isShow = true, string _text = "", float _wait = 0.4f)
        {
            text = _text;
            isShow = _isShow;
            wait = _wait;
        }

        public override void Execute()
        {
            ToolTip.Instance.Show(isShow, 0.4f, text);
            //Completed
            Completed();
        }
    }
}
