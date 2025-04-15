using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class RepeatButton : Selectable
    {
        /// <summary>
        /// 回调触发间隔时间
        /// </summary>
        public float interval = 0.1f;

        /// <summary>
        /// 延迟时间
        /// </summary>
        public float delay = 0.5f;

        /// <summary>
        /// 记录延迟时间
        /// </summary>
        private float m_Delay = 0f;

        /// <summary>
        /// 上一次调用时间
        /// </summary>
        private float lastInvokeTime = 0f;

        private UnityEvent m_OnClick = new UnityEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UnityEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        /// <summary>
        /// 是否按下
        /// </summary>
        private bool isPress = false;

        void Update()
        {
            if (isPress)
            {
                m_Delay += Time.deltaTime;
                if (m_Delay < delay)
                    return;

                if (Time.time - lastInvokeTime > interval)
                {
                    OnClick.Invoke();
                    lastInvokeTime = Time.time;
                }
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPress = true;
            m_Delay = 0;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isPress = false;
            m_Delay = 0;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            isPress = false;
            m_Delay = 0;
        }
    }
}
