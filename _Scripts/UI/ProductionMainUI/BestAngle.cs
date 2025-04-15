using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 最佳视角
    /// </summary>
    public class BestAngle : MonoBehaviour
    {
        /// <summary>
        /// 相机Transform
        /// </summary>
        private Transform cameraTrans;

        /// <summary>
        /// 目标Transform
        /// </summary>
        private Transform targetTrans;

        /// <summary>
        /// Orbit
        /// </summary>
        private MouseOrbit mouseOrbit;

        void Awake()
        {
            cameraTrans = transform.Find("Camera");
            targetTrans = transform.Find("Target");
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();
        }

        /// <summary>
        /// 进入最佳视角
        /// </summary>
        public void Enter()
        {
            mouseOrbit.Focus(cameraTrans.position, targetTrans.position);
        }

        public void FollowTarget(float dis = 2)
        {
            mouseOrbit.Focus(targetTrans.position, dis);

        }
    }
}
