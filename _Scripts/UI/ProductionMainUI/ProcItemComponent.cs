using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace XFramework.UI
{
    public class ProcItemComponent : MonoBehaviour
    {
        public class OnValueChangedEvent : UnityEvent<string, bool> { }

        private OnValueChangedEvent m_OnValueChanged = new OnValueChangedEvent();
        /// <summary>
        /// OnValueChanged
        /// </summary>
        public OnValueChangedEvent OnValueChanged
        {
            get { return m_OnValueChanged; }
            set { m_OnValueChanged = value; }
        }

        /// <summary>
        /// Toggle
        /// </summary>
        private Toggle m_Toggle;

        /// <summary>
        /// Label
        /// </summary>
        private Text m_Label;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        private void Awake()
        {
            m_Toggle = transform.GetComponent<Toggle>();
            m_Label = transform.Find("Label").GetComponent<Text>();
            m_Toggle.onValueChanged.AddListener(m_Toggle_onValueChanged);
        }

        public void SetValue(string name, bool flag = false)
        {
            Name = name;
            m_Label.text = name;
            m_Toggle.isOn = flag;
        }

        private void m_Toggle_onValueChanged(bool arg0)
        {
            OnValueChanged.Invoke(Name, arg0);
        }
    }

}
