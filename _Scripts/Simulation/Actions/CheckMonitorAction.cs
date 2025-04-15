using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 检查流程Action
    /// </summary>
    public class CheckMonitorAction : ActionBase
    {
        /// <summary>
        /// 流程监视器
        /// </summary>
        public ProcedureMonitor monitor;

        /// <summary>
        /// 序列ID
        /// </summary>
        public int sequenceID;

        /// <summary>
        /// 行为ID -1 不检测ActionID
        /// </summary>
        public int actionID;

        /// <summary>
        /// 判断是否完成
        /// </summary>
        public bool isCompleted;

        /// <summary>
        /// 失败提示文本
        /// </summary>
        public string defeatText;

        public CheckMonitorAction(ProcedureMonitor _monitor, int _sequenceID, int _actionID = -1, bool _isCompleted = true, string _defeatText = "")
        {
            monitor = _monitor;
            sequenceID = _sequenceID;
            actionID = _actionID;
            isCompleted = _isCompleted;
            defeatText = _defeatText;
        }

        public override void Execute()
        {
            if (monitor != null)
            {
                if (actionID == -1)//不检查ActionID
                {
                    if (monitor[sequenceID].Completed == isCompleted)
                    {
                        Completed();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(defeatText))
                        {
                            EventDispatcher.ExecuteEvent(Events.HUDText.Show, Utils.NewGameObject().transform, defeatText, Color.red);
                        }
                        string msg = string.Format("检查流程{0}：{1}，不匹配！", sequenceID, isCompleted);
                        Error(new Exception(msg));
                    }
                }
                else
                {
                    if (monitor[sequenceID, actionID].Completed == isCompleted)
                    {
                        Completed();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(defeatText))
                        {
                            EventDispatcher.ExecuteEvent(Events.HUDText.Show, Utils.NewGameObject().transform, defeatText, Color.red);
                        }
                        string msg = string.Format("检查流程{0}-{1}：{2}，不匹配！", sequenceID, actionID, isCompleted);
                        Error(new Exception(msg));
                    }
                }
            }
            else
            {
                Error(new Exception("monitor is null"));
            }
        }
    }
}
