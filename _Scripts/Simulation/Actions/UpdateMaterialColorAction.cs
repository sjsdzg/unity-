using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateMaterialColorAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// color
        /// </summary>
        public Color color;

        public UpdateMaterialColorAction(GameObject _gameObject, Color _color)
        {
            gameObject = _gameObject;
            color = _color;
        }

        public override void Execute()
        {
            gameObject.GetComponent<Renderer>().material.color = color;
            //Completed
            Completed();
        }
    }
}
