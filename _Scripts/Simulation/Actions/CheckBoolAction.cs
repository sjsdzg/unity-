using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Actions
{
    public class CheckBoolAction : ActionBase
    {
        /// <summary>
        /// 变量
        /// </summary>
        private bool variable;

        /// <summary>
        /// 是否匹配
        /// </summary>
        public bool isMatch;

        /// <summary>
        /// 失败提示文本
        /// </summary>
        public string defeatText;
        public CheckBoolAction(bool _variable,bool _isMatch, string _defeatText = "")
        {
            variable = _variable;
            isMatch = _isMatch;
            defeatText = _defeatText;
        }
        public override void Execute()
        {
            if (variable == isMatch)
            {
                Completed();
            }
            else
            {
                if (!string.IsNullOrEmpty(defeatText))
                {
                    EventDispatcher.ExecuteEvent<Transform, string, Color>(Events.HUDText.Show, Utils.NewGameObject().transform, defeatText, Color.red);
                }
                Error(new System.Exception("条件不满足！"));
            }
        }
    }
}
