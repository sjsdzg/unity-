using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    /// <summary>
    /// 按钮开启关闭，自发光颜色设置
    /// </summary>
    public class SetPowerButtonAction : ActionBase
    {
        /// <summary>
        /// 物体 (注意传入的物体可能被销毁)
        /// </summary>
        public Transform transform;

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool isOpen;

        /// <summary>
        /// 颜色
        /// </summary>
        public Color color;

        public SetPowerButtonAction(Transform _transform,bool _isOpen,Color _color)
        {
            transform = _transform;
            isOpen = _isOpen;
            color = _color;
        }

        public override void Execute()
        {
            if (transform != null)
            {
                if (isOpen)
                {
                    transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
                }
                else
                {
                    transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                }
                Completed();
            }
        }
    }
}
