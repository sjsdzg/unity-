using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 液位值
    /// </summary>
    public class FluidLevelComponent : ComponentBase
    {
        /// <summary>
        /// 液位插件
        /// </summary>
        private LiquidVolume m_Liquid;

        /// <summary>
        /// 液位值
        /// </summary>
        [SerializeField]
        private float m_Value;

        public float Value
        {
            get
            {
                return m_Value;
            }
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
        public float maxValue=1;

        private void Awake()
        {
            m_Liquid = transform.GetComponentInChildren<LiquidVolume>();
            m_Liquid.level = m_Value;
        }

        private void OnValueChanged()
        {
            m_Liquid = transform.GetComponentInChildren<LiquidVolume>();
            m_Liquid.level = m_Value;
        }
    }
}
