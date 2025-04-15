using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 数字显示仪表组件
    /// </summary>
    public class DigitalMeterComponent : ComponentBase
    {
        /// <summary>
        /// 字体插件
        /// </summary>
        private TextMeshPro m_TextMeshPro;

        [SerializeField]
        private float m_Value;
        /// <summary>
        /// 示数
        /// </summary>
        public float Value
        {
            get { return m_Value; }
            set
            {
                if (value > maxValue)
                {
                    m_Value = maxValue;
                }
                else if (value < minValue)
                {
                    m_Value = minValue;
                }
                else
                {
                    m_Value = value;
                }
                OnValueChanged();
                //发送
                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, UUID, Value);
            }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        [SerializeField]
        public float minValue;

        /// <summary>
        /// 最大值
        /// </summary>
        [SerializeField]
        public float maxValue;

        /// <summary>
        /// 后缀单位（°C MPa %vol-氧气浓度）
        /// </summary>
        [Tooltip("后缀单位（°C MPa %vol-氧气浓度）")]
        [SerializeField]
        private string unit = string.Empty;

        /// <summary>
        /// 单位
        /// </summary>
        [Tooltip("格式化标准：f1保留一位小数")]
        [SerializeField]
        private string format = "f1";

        private void Awake()
        {
            m_TextMeshPro = transform.GetComponentInChildren<TextMeshPro>();

            string text = m_Value.ToString(format) + unit;

            m_TextMeshPro.text = text.Replace("\\n", "\n");
        }

        private void OnValueChanged()
        {
            string text = null;
            if (string.IsNullOrEmpty(format))
            {
                int value = Mathf.CeilToInt(m_Value);
                text = value.ToString() + unit;
            }
            else
            {
                text = m_Value.ToString(format) + unit;
            }          
            m_TextMeshPro.text = text.Replace("\\n", "\n");
        }
    }
}
