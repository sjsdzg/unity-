using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateMaterialAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// material
        /// </summary>
        public Material material;

        public UpdateMaterialAction(GameObject _gameObject, Material _material)
        {
            gameObject = _gameObject;
            material = _material;
        }

        public override void Execute()
        {
            gameObject.GetComponent<Renderer>().material = material;
            Completed();
        }
    }
}
