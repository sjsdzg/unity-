using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Module;

namespace XFramework.Actions
{
    public class UpdateValveAction : ActionBase
    {
        /// <summary>
        /// 阀门
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 阀门状态（开/关）
        /// </summary>
        private ValveState state = ValveState.NON;

        /// <summary>
        /// 旋转角度度数
        /// </summary>
        private float angle;
        /// <summary>
        /// 旋转角度类型 0 =x 1=y z=2
        /// </summary>
        private int angleType;

        public UpdateValveAction(GameObject _gameObject, ValveState _state = ValveState.NON,float _angle = 90,int _angleType=3)
        {
            gameObject = _gameObject;
            state = _state;
            angle = _angle;
            angleType = _angleType;
        }
        public override void Execute()
        {
            ValveComponent m_Valve = gameObject.GetOrAddComponent<ValveComponent>();
            switch (angleType)
            {
                case 0:
                   
                    break;
                case 1:
                    m_Valve.angle = new Vector3(-angle,0,0);
                    break;
                case 2:
                    m_Valve.angle = new Vector3(0, -angle,0);
                    break;
                case 3:
                    m_Valve.angle = new Vector3(0, 0,-angle);
                    break;               
                default:
                    break;
            }                              
            m_Valve.State = state;            
            Completed();
        }
    }
}
