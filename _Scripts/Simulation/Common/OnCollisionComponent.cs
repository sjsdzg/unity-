using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Simulation.Component
{
    public class OnCollisionComponent : MonoBehaviour
    {
        /// <summary>
        /// 碰撞次数
        /// </summary>
        private int colliCount = 0;

        public class OnCollisionEvent : UnityEvent<bool> { }

        private OnCollisionEvent m_OnCollision = new OnCollisionEvent();
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        public OnCollisionEvent OnCollision
        {
            get { return m_OnCollision; }
            set { m_OnCollision = value; }
        }

        void OnTriggerEnter(Collider collider)
        {
            GameObject obj = collider.gameObject;

            if (obj.tag == "NPC")
            {
                colliCount++;

                if (colliCount % 2 == 1)
                {
                    OnCollision.Invoke(true);
                }
                else
                {
                    OnCollision.Invoke(false);
                }
            }
        }
    }
}

