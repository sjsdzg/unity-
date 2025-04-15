using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class ValveStatusItemComponent : StatusItemComponent<bool>
    {
        /// <summary>
        /// Toggle
        /// </summary>
        private Toggle m_Toggle;

        /// <summary>
        /// Text(开或关)
        /// </summary>
        private Text m_TextState;

        /// <summary>
        /// 
        /// </summary>
        private Text m_Text;

        private ValveStatusItem m_StatusItem;
        /// <summary>
        /// StatusItem
        /// </summary>
        public ValveStatusItem StatusItem
        {
            get { return m_StatusItem; }
            set
            {
                m_StatusItem = value;
                m_Text.text = m_StatusItem.ChineseName;
                Value = m_StatusItem.Value;
            }
        }


        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_Toggle = transform.Find("Toggle").GetComponentInChildren<Toggle>();
            m_TextState = transform.Find("Toggle/Label").GetComponent<Text>();
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            m_Toggle.isOn = Value;
            if (Value)
            {
                m_TextState.text = "开";
            }
            else
            {
                m_TextState.text = "关";
            }
        }
    }
}

