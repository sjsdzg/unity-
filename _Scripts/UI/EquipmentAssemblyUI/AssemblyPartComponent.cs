using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using XFramework.Module;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Simulation.Component;
using XFramework.Component;

namespace XFramework.UI
{
    /// <summary>
    /// 设备部件Item
    /// </summary>
    public class AssemblyPartComponent : ToolTipComponent, IPointerClickHandler
    {
        public class OnClickedEvent : UnityEvent<AssemblyPartComponent> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        public override void OnStart()
        {
            base.OnStart();
            // 提示
            CatchToolTip = this.name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked.Invoke(this);
            }
        }
    }
}
