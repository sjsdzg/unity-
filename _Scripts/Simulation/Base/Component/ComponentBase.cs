using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    /// <summary>
    /// 基本仿真元件类型
    /// </summary>
    public abstract class ComponentBase : MonoBehaviour, IComponent
    {
        /// <summary>
        /// 元件唯一ID
        /// </summary>
        public string UUID { get; set; }

        private bool visible = true;
        /// <summary>
        /// 设置是否可见。
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value)
                    return;

                visible = value;
                gameObject.SetActive(visible);//设置物体是否显示
            }
        }
    }
}
