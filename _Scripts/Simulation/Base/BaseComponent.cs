using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.Simulation.Base
{
    /// <summary>
    /// 基本仿真元件类型
    /// </summary>
    public abstract class BaseComponent : MonoBehaviour, IComponent
    {
        /// <summary>
        /// 元件名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 获取元件类型的抽象方法
        /// </summary>
        /// <returns></returns>
        public abstract int GetComponentType();

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
