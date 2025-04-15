using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 地图切换碰撞器
    /// </summary>
    public class GroundConllision : MonoBehaviour
    {
        /// <summary>
        /// 地面索引
        /// </summary>
        public int groundIndex;

        public class OnGroundCollisionEvent : UnityEvent<int> { }

        private OnGroundCollisionEvent m_OnGroundCollision = new OnGroundCollisionEvent();
        /// <summary>
        /// 地面触发事件
        /// </summary>
        public OnGroundCollisionEvent OnGroundCollision
        {
            get { return m_OnGroundCollision; }
            set { m_OnGroundCollision = value; }
        }

        /// <summary>
        /// 当和地面进行碰撞
        /// </summary>
        /// <param name="collisionInfo"></param>
        void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.tag == "Player")
            {
                OnGroundCollision.Invoke(groundIndex);
            }
        }
    }
}
