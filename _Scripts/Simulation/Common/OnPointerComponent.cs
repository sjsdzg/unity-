using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.Component
{
    /// <summary>
    /// 指针事件组件
    /// </summary>
    public class OnPointerComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private OnClickEvent m_OnMouseClick = new OnClickEvent();
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public OnClickEvent OnMouseClick
        {
            get { return m_OnMouseClick; }
            set { m_OnMouseClick = value; }
        }

        private UnityEvent m_OnMouseEnter = new UnityEvent();
        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        public UnityEvent OnMouseEnter
        {
            get { return m_OnMouseEnter; }
            set { m_OnMouseEnter = value; }
        }

        private UnityEvent m_OnMouseExit = new UnityEvent();
        /// <summary>
        /// 鼠标退出事件
        /// </summary>
        public UnityEvent OnMouseExit
        {
            get { return m_OnMouseExit; }
            set { m_OnMouseExit = value; }
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseClick.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit.Invoke();
        }
    }
}
