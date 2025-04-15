using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class SetPositionAction : ActionBase
    { 
        /// <summary>
        /// 物体
        /// </summary>
        public Transform transform;

        /// <summary>
        /// color
        /// </summary>
        public Vector3 position;

        public SetPositionAction(Transform _transform, Vector3 _position)
        {
            transform = _transform;
            position = _position;
        }

        public override void Execute()
        {
            transform.position = position;
            Completed();
        }
    }

}
