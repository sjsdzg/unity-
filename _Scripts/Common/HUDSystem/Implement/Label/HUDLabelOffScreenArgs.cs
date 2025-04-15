using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Common
{
    [Serializable]
    /// <summary>
    /// 标签在屏幕外参数
    /// </summary>
	public class HUDLabelOffScreenArgs  :HUDArgs{

        /// <summary>
        /// 图标
        /// </summary>
        public Sprite m_Sprite = null;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color m_Color = new Color(1, 1, 1, 1);
        /// <summary>
        /// 默认尺寸
        /// </summary>
        public float size = 30;
        /// <summary>
        /// 闪光
        /// </summary>
        public bool flashing = true;

    }
}