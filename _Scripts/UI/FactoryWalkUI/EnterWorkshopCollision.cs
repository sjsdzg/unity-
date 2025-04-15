using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 进入车间碰撞器
    /// </summary>
    public class EnterWorkshopCollision : MonoBehaviour
    {
        private UnityEvent m_OnCollision = new UnityEvent();
        /// <summary>
        /// 触发更衣流程事件
        /// </summary>
        public UnityEvent OnCollision
        {
            get { return m_OnCollision; }
            set { m_OnCollision = value; }
        }

        /// <summary>
        /// 当进入碰撞器
        /// </summary>
        /// <param name="collisionInfo"></param>
        void OnCollisionEnter(Collision collisionInfo)
        {
            OnCollision.Invoke();
        }
    }
}
