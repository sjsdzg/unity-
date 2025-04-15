using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class UpdateMaterialTextureAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 贴图
        /// </summary>
        public Texture texture;

        public UpdateMaterialTextureAction(GameObject _gameObject, Texture _texture)
        {
            gameObject = _gameObject;
            texture = _texture;
        }

        public override void Execute()
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = texture;
            //Completed
            Completed();
        }
    }
}
