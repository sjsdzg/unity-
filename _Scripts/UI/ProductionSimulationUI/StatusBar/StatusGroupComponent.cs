using UnityEngine;
using System.Collections;
using System;
using XFramework.Module;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class StatusGroupComponent : MonoBehaviour
    {
        /// <summary>
        /// ValveStatusItemComponent
        /// </summary>
        public GameObject m_DefaultValve;

        /// <summary>
        /// MeterStatusItemComponent
        /// </summary>
        public GameObject m_DefaultMeter;

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        private StatusGroup m_StatusGroup;
        /// <summary>
        /// StatusGroup
        /// </summary>
        public StatusGroup StatusGroup
        {
            get { return m_StatusGroup; }
            set
            {
                m_StatusGroup = value;
                m_Title.text = m_StatusGroup.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public StatusBar m_StatusBar { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        private void Awake()
        {
            if (m_DefaultValve == null)
            {
                throw new Exception("DefaultValve is null");
            }

            if (m_DefaultMeter == null)
            {
                throw new Exception("DefaultMeter is null");
            }
            m_DefaultValve.gameObject.SetActive(false);
            m_DefaultMeter.gameObject.SetActive(false);
            m_Title = transform.Find("Title/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 增加阀门状态项
        /// </summary>
        /// <param name="item"></param>
        public void AddValveStatusItem(ValveStatusItem item)
        {
            GameObject obj = Instantiate(m_DefaultValve);
            obj.SetActive(true);
            ValveStatusItemComponent valveComponent = obj.GetComponent<ValveStatusItemComponent>();
            if (valveComponent != null && Content != null)
            {
                Transform t = valveComponent.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                valveComponent.StatusItem = item;
                m_StatusBar.ValveStatusItems.Add(item.Name, valveComponent);
            }
        }

        /// <summary>
        /// 增加仪表状态项
        /// </summary>
        /// <param name="item"></param>
        public void AddMeterStatusItem(MeterStatusItem item)
        {
            GameObject obj = Instantiate(m_DefaultMeter);
            obj.SetActive(true);
            MeterStatusItemComponent meterComponent = obj.GetComponent<MeterStatusItemComponent>();
            if (meterComponent != null && Content != null)
            {
                Transform t = meterComponent.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                meterComponent.StatusItem = item;
                m_StatusBar.MeterStatusItems.Add(item.Name, meterComponent);
            }
        }
    }
}