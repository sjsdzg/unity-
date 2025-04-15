using System;
using UnityEngine;
using UnityEngine.Events;

namespace Simulation.Component
{
    public class ScrubEffectComponent:MonoBehaviour
    {
        /// <summary>
        /// 过渡时间
        /// </summary>
        public float duration = 1f;

        private UnityEvent m_OnExit = new UnityEvent();
        /// <summary>
        /// 退出动画
        /// </summary>
        public UnityEvent OnExit
        {
            get { return m_OnExit; }
            set { m_OnExit = value; }
        }

        /// <summary>
        /// Plane的Renderer
        /// </summary>
        private Renderer planeRenderer;

        /// <summary>
        /// 星星粒子
        /// </summary>
        public ParticleSystem m_Star;

        /// <summary>
        /// Plane的offset的速度
        /// </summary>
        public Vector2 offsetVelocity = Vector2.zero;

        /// <summary>
        /// Plane的offset
        /// </summary>
        public Vector2 offset = Vector2.zero;

        /// <summary>
        /// 初始Plane的offset
        /// </summary>
        private Vector2 rawOffset = Vector2.zero;

        /// <summary>
        /// 目标offset
        /// </summary>
        public Vector2 targetOffset = Vector2.zero;

        public bool isOn = false;
        /// <summary>
        /// Plane是否打开
        /// </summary>
        public bool IsOn
        {
            get { return isOn; }
            set
            {
                isOn = value;

                if (m_Star == null)
                    return;

                if (isOn == false)
                {
                    offset = rawOffset;
                    planeRenderer.material.SetTextureOffset("_MainTex", offset);
                }
            }
        }

        void Awake()
        {
            planeRenderer = transform.GetComponent<Renderer>();
            if (planeRenderer != null)
            {
                planeRenderer.material.SetTextureOffset("_MainTex", offset);
            }

            m_Star = transform.GetComponentInChildren<ParticleSystem>();
            if (m_Star != null)
            {
                m_Star.Stop();
            }

            rawOffset = offset;
        }

        void Update()
        {
            if (isOn)
            {
                if (planeRenderer != null)
                {
                    offset += offsetVelocity * Time.deltaTime;
                    planeRenderer.material.SetTextureOffset("_MainTex", offset);

                    if (offset.x <= targetOffset.x && offset.y <= targetOffset.y)
                    {
                        IsOn = false;
                        m_Star.Play();
                        OnExit.Invoke();
                    }
                }
            }
        }
    }
}
