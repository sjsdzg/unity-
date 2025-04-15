using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using XFramework.Simulation.Component;
using XFramework.UI;

namespace XFramework.Component
{
    /// <summary>
    /// 检查压力仪表组件
    /// </summary>
    public class CheckPressureMeterComponent : ContextMenuComponent
    {
        private UnityEvent m_OnChecked = new UnityEvent();
        /// <summary>
        /// 阀门检查事件
        /// </summary>
        public UnityEvent OnChecked
        {
            get { return m_OnChecked; }
            set { m_OnChecked = value; }
        }
        public override void OnAwake()
        {
            base.OnAwake();
        }

        /// <summary>
        /// 获取对象自己的名字
        /// </summary>
        public bool isGetObjectName = false;

        public override void InitContextMenu()
        {
            base.InitContextMenu();
            Parameters.Add(new ContextMenuParameter(1, "检查", "", true, Checked_Callback));
            Parameters.Add(new ContextMenuParameter(2, "关闭", "", true, x => ContextMenuEx.Instance.Hide()));
        }

        /// <summary>
        /// 检查回调
        /// </summary>
        /// <param name="parameter"></param>
        private void Checked_Callback(ContextMenuParameter parameter)
        {
            OnChecked.Invoke();
        }

        void Start()
        {
            if(isGetObjectName)
            {
                CatchToolTip = "仪表:" + transform.name;
            }
            else
            {
                CatchToolTip = transform.parent.name;
            }
        }
    }
}
