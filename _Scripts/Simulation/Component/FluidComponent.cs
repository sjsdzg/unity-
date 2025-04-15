using System;
using UnityEngine;
using XFramework.Simulation;

namespace XFramework.Component
{
    public class FluidComponent : ComponentBase
    {
        /// <summary>
        /// UV控制流动的效果
        /// </summary>
        public Vector2 UV { get; set; }
        public bool IsActive=false;

        private int m_Velocity = 0;
        /// <summary>
        /// 流动速度
        /// </summary>
        public int Velocity
        {
            get { return m_Velocity; }
            set
            {
                m_Velocity = value;

                if (m_Velocity == 0)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 自身renderer
        /// </summary>
        private Renderer m_Renderer;

        void Start()
        {
            m_Renderer = transform.GetComponent<Renderer>();
            if (!IsActive)
            {
                gameObject.SetActive(false);
            }          
        }

        void Update()
        {
            if (m_Velocity != 0)
            {
                
                float offsetU = Time.time * UV.x * m_Velocity;
                float offsetV = Time.time * UV.y * m_Velocity;
                if (m_Renderer != null)
                {
                    m_Renderer.material.SetTextureOffset("_BumpMap", new Vector2(offsetU, offsetV));
                    m_Renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetU, offsetV));                    
                }
            }
        }
    }
}
