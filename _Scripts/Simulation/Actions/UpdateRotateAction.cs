using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class UpdateRotateAction : ActionBase
    {
        /// <summary>
        /// 轴向
        /// </summary>
        public RotationAxis m_Axis = RotationAxis.Y;

        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 是否旋转
        /// </summary>
        public bool isOn = false;

        /// <summary>
        /// 转动速度
        /// </summary>
        public float torque = 500f;

        public UpdateRotateAction(GameObject _gameObject,bool _isOn,RotationAxis _axis, float _speed)
        {
            gameObject = _gameObject;
            isOn = _isOn;
            m_Axis = _axis;
            torque = _speed;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                RotateCompent m_RotateCom = gameObject.GetOrAddComponent<RotateCompent>();
                m_RotateCom.gameObject = gameObject;
                m_RotateCom.IsOn = isOn;
                m_RotateCom.m_Axis = m_Axis;
                m_RotateCom.torque = torque;
                Completed();        
            }
        }
    }
}
