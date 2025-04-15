using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XFramework.Module;
using System;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class FaultCauseSelectComponent : MonoBehaviour
    {
        public class ToggleEvent : UnityEvent<FaultCauseSelectComponent, bool> { }

        private ToggleEvent m_OnValueChanged = new ToggleEvent();
        /// <summary>
        /// 值改变事件
        /// </summary>
        public ToggleEvent OnValueChanged
        {
            get { return m_OnValueChanged; }
            set { m_OnValueChanged = value; }
        }

        /// <summary>
        /// Toggle
        /// </summary>
        private Toggle m_Toggle;

        /// <summary>
        /// 故障信息
        /// </summary>
        public FaultCause Data { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        private Text label;

        private void Awake()
        {
            label = transform.Find("Label").GetComponent<Text>();
            m_Toggle = transform.GetComponent<Toggle>();
            m_Toggle.onValueChanged.AddListener(m_Toggle_onValueChanged);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="data"></param>
        public void SetValue(FaultCause data)
        {
            Data = data;
            label.text = Data.Cause;
        }

        private void m_Toggle_onValueChanged(bool arg0)
        {
            OnValueChanged.Invoke(this, arg0);
        }

        /// <summary>
        /// 提示出来
        /// </summary>
        /// <param name="flag"></param>
        public void Hint(bool flag = false)
        {
            if (flag)
            {
                label.text = Data.Cause + " <color=green>[正确]</color>";
            }
            else
            {
                label.text = Data.Cause;
            }
        }
    }
}

