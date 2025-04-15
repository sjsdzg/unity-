using Simulation.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Component;

namespace XFramework.Simulation.Component
{
    public class QAControlDoorComponent: MonoBehaviour
    {
        /// <summary>
        /// 碰撞器
        /// </summary>
        [SerializeField]
        private OnCollisionComponent[] m_CollisionComponents;
        //[SerializeField]
        //private Extension_OnCollisionComponent[] m_Extension_OnCollisionComponent;
        private DoorComponent m_Door;
        private void Awake()
        {
            m_Door = transform.GetComponent<DoorComponent>();
            OnEvent();
        }
        public void OnEvent()
        {
            if (m_CollisionComponents != null)
            {
                for (int i = 0; i < m_CollisionComponents.Length; i++)
                {
                    OnCollisionComponent colli = m_CollisionComponents[i];
                    colli.OnCollision.AddListener(isOn =>
                    {
                        m_Door.Opening(isOn);
                    });
                }
            }
            //if (m_Extension_OnCollisionComponent != null)
            //{
            //    for (int i = 0; i < m_Extension_OnCollisionComponent.Length; i++)
            //    {
            //        Extension_OnCollisionComponent colli = m_Extension_OnCollisionComponent[i];
            //        colli.OnCollision.AddListener(isOn =>
            //        {
            //            m_Door.Opening(isOn);
            //        });
            //    }
            //}
        }
        
    }
}
