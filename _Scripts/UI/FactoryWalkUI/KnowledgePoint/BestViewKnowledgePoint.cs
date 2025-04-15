using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 具有最佳视角知识点
    /// </summary>
    public class BestViewKnowledgePoint : BaseKnowledgePoint
    {
        public override KnowledgePointType GetKnowledgePointType()
        {
            return KnowledgePointType.None;
        }

        /// <summary>
        /// 主相机
        /// </summary>
        private MouseOrbit mouseOrbit;

        /// <summary>
        /// 相机最佳视角点
        /// </summary>
        private Transform cameraPoint;

        /// <summary>
        /// 
        /// </summary>
        private Transform targetPoint;

        void Awake()
        {
            cameraPoint = transform.Find("Camera");
            targetPoint = transform.Find("Target");
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();

            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        public override void Display()
        {
            Observing();
        }

        public override void Close()
        {
            
        }

        public void Observing()
        {
            mouseOrbit.Focus(cameraPoint.position, targetPoint.position);
        }
    }
}
