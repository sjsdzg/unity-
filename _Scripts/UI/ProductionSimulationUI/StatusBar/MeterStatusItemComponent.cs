using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class MeterStatusItemComponent : StatusItemComponent<float>
    {
        private Text m_Text;

        private Text m_TextValue;

        private MeterStatusItem m_StatusItem;
        /// <summary>
        /// 
        /// </summary>
        public MeterStatusItem StatusItem
        {
            get { return m_StatusItem; }
            set
            {
                m_StatusItem = value;
                m_Text.text = m_StatusItem.ChineseName;
                m_TextValue.text = m_StatusItem.Value.ToString() + StatusItem.Unit;
            }
        }


        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_TextValue = transform.Find("Value").GetComponent<Text>();
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            m_TextValue.text = Value.ToString(StatusItem.Format) + StatusItem.Unit;
        }
    }
}

