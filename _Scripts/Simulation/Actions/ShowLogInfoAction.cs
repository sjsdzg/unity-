using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Simulation
{
    public class ShowLogInfoAction : ActionBase
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string text;

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType type;

        public ShowLogInfoAction(string _text,LogType _type = LogType.Log)
        {
            text = _text;
            type = _type;
        }

        public override void Execute()
        {
            EventDispatcher.ExecuteEvent(Events.LogInfo.Show, text,type);
            //TODO
            Completed();
        }
    }
}
