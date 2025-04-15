using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class TypeGroup : MonoBehaviour
    {
        public class OnSelectedEvent : UnityEvent<StageStyle> { }

        private OnSelectedEvent m_OnSelected = new OnSelectedEvent();
        /// <summary>
        /// 选中事件
        /// </summary>
        public OnSelectedEvent OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        private bool m_StandardActive;
        /// <summary>
        /// 标准模式是否显示
        /// </summary>
        public bool StandardActive
        {
            get { return m_StandardActive; }
            set
            {
                m_StandardActive = value;
                toggleStandard.gameObject.SetActive(m_StandardActive);
            }
        }

        private bool m_FaultActive;
        /// <summary>
        /// 故障模式是否显示
        /// </summary>
        public bool FaultActive
        {
            get { return m_FaultActive; }
            set
            {
                m_FaultActive = value;
                toggleFault.gameObject.SetActive(m_FaultActive);
            }
        }

        /// <summary>
        /// 标准模式Toggle
        /// </summary>
        public Toggle toggleStandard;

        /// <summary>
        /// 故障模式Toggle
        /// </summary>
        public Toggle toggleFault;

        private void Awake()
        {
            toggleStandard = transform.Find("Content/ToggleStandard").GetComponent<Toggle>();
            toggleFault = transform.Find("Content/ToggleFault").GetComponent<Toggle>();
            //Event
            toggleStandard.onValueChanged.AddListener(toggleStandard_onValueChanged);
            toggleFault.onValueChanged.AddListener(toggleFault_onValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleStandard_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(StageStyle.Standard);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleFault_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(StageStyle.Fault);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public void SetStandardToggle(bool flag)
        {
            toggleStandard.isOn = flag;
        }
    }
}

