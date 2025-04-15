using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class LogAction : ActionBase
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        private LogType logType;
        /// <summary>
        /// 
        /// </summary>
        private object message;

        public LogAction(object _message, LogType _logType = LogType.Log)
        {
            message = _message;
            logType = _logType;
        }

        public override void Execute()
        {
            Debug.unityLogger.Log(logType, message);
            Completed();
        }
    }
}
