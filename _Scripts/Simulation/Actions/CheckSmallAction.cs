using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class CheckSmallAction : ActionBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 检测状态
        /// </summary>
        public bool status;
        /// <summary>
        /// 失败提示文本
        /// </summary>
        public string defeatText;
        public CheckSmallAction(string _name,bool _status,string _defeatText = "")
        {
            name = _name;
            status = _status;
            defeatText = _defeatText;
        }

        public override void Execute()
        {
            bool isMatch = SmallActionManager.Instance.CheckSmallAction(name, status);
            if (isMatch)
            {
                Completed();
            }
            else
            {
                if (!string.IsNullOrEmpty(defeatText))
                {
                    EventDispatcher.ExecuteEvent(Events.HUDText.Show, defeatText, Color.red);
                }
                Error(new System.Exception("执行当前小步骤时,结果不匹配"));
            }
        }
    }
}
