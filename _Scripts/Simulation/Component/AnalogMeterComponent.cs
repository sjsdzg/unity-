using UnityEngine;
using System.Collections;
using XFramework.Simulation;
using XFramework.Core;

namespace XFramework.Component
{
    /// <summary>
    /// 模拟仪表
    /// </summary>
    public class AnalogMeterComponent : ComponentBase
    {
        /// <summary>
        /// 表针
        /// </summary>
        [SerializeField]
        private Transform m_Pointer;

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
        private float minValue;

        /// <summary>
        /// 最大值
        /// </summary>
        [SerializeField]
        private float maxValue;

        /// <summary>
        /// 比率 (角度和值比例)
        /// </summary>
        [Tooltip("角度/示数的比例")]
        [SerializeField]
        private float ratio = 1;

        [Header("初始值")]
        /// <summary>
        /// 初始值
        /// </summary>
        [SerializeField]
        private float originalValue;

        /// <summary>
        /// 初始角度
        /// </summary>
        [SerializeField]
        private Vector3 originalEuler;

        [Header("插值")]
        [SerializeField]
        private LerpType m_LerpType = LerpType.Lerp;
        /// <summary>
        /// 插值类型
        /// </summary>
        public LerpType LerpType
        {
            get { return m_LerpType; }
            set { m_LerpType = value; }
        }

        /// <summary>
        /// 插值
        /// </summary>
        private float lerpValue;

        /// <summary>
        /// 默认插值速度
        /// </summary>
        [SerializeField]
        private float lerpSpeed = 360;

        /// <summary>
        /// 最小插值速度
        /// </summary>
        private float minLerpSpeed = 30;

        private void Start()
        {
            Value = originalValue;
        }

        protected virtual void OnValueChanged()
        {
            CheckLerp();
        }

        private void Update()
        {
            switch (m_LerpType)
            {
                case LerpType.None:
                    lerpValue = m_Value;
                    break;
                case LerpType.Lerp:
                    var lastValue = lerpValue;
                    lerpValue = Mathf.Lerp(lerpValue, m_Value, lerpSpeed / ratio * Time.deltaTime);
                    var tempLerpSpeed = Mathf.Abs((lerpValue - lastValue) / Time.deltaTime);
                    if (tempLerpSpeed < minLerpSpeed / ratio)
                        lerpValue = Mathf.MoveTowards(lastValue, m_Value, minLerpSpeed / ratio * Time.deltaTime);
                    break;
                case LerpType.Towards:
                    lerpValue = Mathf.MoveTowards(lerpValue, m_Value, this.lerpSpeed / ratio * Time.deltaTime);
                    break;
                default:
                    break;
            }
            SetLocalRotation(lerpValue);
            CheckLerp();
        }

        private void CheckLerp()
        {
            enabled = (m_Value - lerpValue != 0);
        }

        private void SetLocalRotation(float value)
        {
            float angle = (value - originalValue) * ratio;
            Vector3 euler = originalEuler + Vector3.forward * angle;
            m_Pointer.localRotation = Quaternion.Euler(euler);
        }

    }

    /// <summary>
    /// 仪表插值类型
    /// </summary>
    public enum LerpType
    {
        None,
        Lerp,
        Towards,
    }
}

