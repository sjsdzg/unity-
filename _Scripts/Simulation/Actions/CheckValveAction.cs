using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Actions
{
    /// <summary>
    /// 检查阀门状态行为
    /// </summary>
    public class CheckValveAction : ActionBase
    {
        /// <summary>
        /// 阀门
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 阀门状态
        /// </summary>
        private ValveState m_State = ValveState.NON;

        /// <summary>
        /// 失败提示文本
        /// </summary>
        public string defeatText;

        public CheckValveAction(GameObject _gameObject,ValveState _state = ValveState.NON, string _defeatText = "")
        {
            gameObject = _gameObject;
            m_State = _state;
            defeatText = _defeatText;
        }

        public override void Execute()
        {
            ValveComponent m_Valve = gameObject.GetOrAddComponent<ValveComponent>();
            if (m_Valve != null)
            {
                //判断标签类型
                if (m_Valve.State == m_State)
                {
                    //Completed
                    Completed();
                }
                else
                {
                    if (!string.IsNullOrEmpty(defeatText))
                    {
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, Utils.NewGameObject().transform, defeatText, Color.red);
                    }
                    string msg = string.Format("检查阀门[{0}]状态：{1},结果不匹配", m_Valve.UUID, m_State);
                    Error(new Exception(msg));
                }                
            }
        }
    }
}
