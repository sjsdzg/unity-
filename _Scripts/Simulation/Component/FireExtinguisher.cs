using UnityEngine;
using System.Collections;

namespace XFramework.Component
{
    /// <summary>
    /// 灭火器
    /// </summary>
    public class FireExtinguisher : MonoBehaviour
    {
        /// <summary>
        /// 粒子效果
        /// </summary>
        private ParticleSystem m_ParticleSystem;

        private bool isOn;
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOn
        {
            get { return isOn; }
            set
            {
                isOn = value;
                if (isOn)
                {
                    m_ParticleSystem.Play();
                }
                else
                {
                    m_ParticleSystem.Stop();
                }
            }
        }

        private void Awake()
        {
            m_ParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
            m_ParticleSystem.Stop();
        }
    }
}

