using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    [Serializable]
    public class IndicatorOffScreenArgs : HUDArgs
    {
        /// <summary>
        /// 图标
        /// </summary>
        public Sprite m_Sprite = null;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color m_Color = new Color(0, 0, 0, 1);
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
