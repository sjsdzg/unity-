using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Component;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Simulation.Component;
using XFramework.Simulation.Component;

namespace XFramework.UI
{
    public class EquipmentPartComponent : ToolTipComponent, IPointerClickHandler
    {
        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked.Invoke(this.name);
            }
        }
    }
}
