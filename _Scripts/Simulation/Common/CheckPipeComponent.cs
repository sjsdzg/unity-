using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using XFramework.Simulation.Component;
using XFramework.UI;
using XFramework.Common;

namespace XFramework.Component
{
    public class CheckPipeComponent : ContextMenuComponent
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

        private bool transparent = false;
        /// <summary>
        /// 获取或设置管道是否透明
        /// </summary>
        public bool Transparent
        {
            get { return transparent; }
            set
            {
                if (transparent == value)
                    return;

                transparent = value;
                OnTransparent(transparent);
            }
        }

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
            CatchToolTip = "管道:" + name;
        }

        /// <summary>
        /// 是否管道透明
        /// </summary>
        /// <param name="isTransparent"></param>
        public virtual void OnTransparent(bool isTransparent)
        {
            if (isTransparent)
            {
                TransparentHelper.SetObjectAlpha(gameObject, 0.3f);
            }
            else
            {
                TransparentHelper.RestoreBack(gameObject);
            }
        }

    }
}
