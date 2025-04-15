using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 旋转组件（一直旋转或停）
    /// </summary>
    public class LoopRotateCompent : ComponentBase
    {
        /// <summary>
        /// 轴向
        /// </summary>
        public RotationAxis m_Axis = RotationAxis.Y;
          
        /// <summary>
        /// 转动速度
        /// </summary>
        public float torque = 500f;

        /// <summary>
        /// 是否旋转
        /// </summary>
        private bool isOn = false;

        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                if (isOn = value)
                    return;
                isOn = value;
            }
        }

        //public RotateCompent(GameObject _gameObject, bool _isOn = true, RotationAxis _Axis = RotationAxis.Y,float _torque = 500)
        //{
        //    gameObject = _gameObject;
        //    isOn = _isOn;
        //    m_Axis = _Axis;
        //    torque = _torque;
        //}
        //public override void Execute()
        //{
        //    if (gameObject != null)
        //    {
        //        SwitchComponent m_Switch = gameObject.GetOrAddComponent<SwitchComponent>();
        //        m_Switch.State = isOn;
        //    }

        //}
        void Update()
        {
            if (IsOn)
            {
                switch (m_Axis)
                {
                    case RotationAxis.X:                        
                        transform.Rotate(Vector3.right, Time.deltaTime* torque);                        
                        break;
                    case RotationAxis.Y:
                        transform.Rotate(Vector3.up, Time.deltaTime* torque);
                        break;
                    case RotationAxis.Z:
                        transform.Rotate(Vector3.forward, Time.deltaTime* torque);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
