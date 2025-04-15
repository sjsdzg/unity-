using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Simulation
{
    public class UpdateSwitchAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 状态
        /// </summary>
        public bool state;

        public UpdateSwitchAction(GameObject _gameObject, bool _state = true)
        {
            gameObject = _gameObject;
            state = _state;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public override void Execute()
        {
            if (gameObject != null)
            {
                SwitchComponent m_Switch = gameObject.GetOrAddComponent<SwitchComponent>();
                m_Switch.State = state;
                Completed();
            }
        }
    }
}
