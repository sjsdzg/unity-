using UnityEngine;
using System.Collections;
using XFramework;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    public class FocusComponent : MonoBehaviour
    {
        /// <summary>
        /// 相机Transform
        /// </summary>
        private Transform m_Camera;

        /// <summary>
        /// 目标Transform
        /// </summary>
        private Transform m_Target;

        /// <summary>
        /// Orbit
        /// </summary>
        private MouseOrbit mouseOrbit;


        void Awake()
        {
            m_Camera = transform.Find("Camera");
            m_Target = transform.Find("Target");
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();
        }
        /// <summary>
        /// 进入最佳视角
        /// </summary>
        public void Focus(UnityAction callback=null)
        {
            mouseOrbit.Focus(m_Camera, m_Target, callback);
        }

        /// <summary>
        /// 跟随 视角
        /// </summary>
        public void FollowTarget(float distance = 5)
        {
            mouseOrbit.Focus(m_Target.position, distance);
        }
        public Transform GetTarget()
        {
            return m_Target;
        }
    }
}

