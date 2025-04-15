using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    [Serializable]
    public class FaultPointOnScreenArgs : HUDArgs
    {
        /// <summary>
        /// 图标
        /// </summary>
        public Sprite m_Sprite = null;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color m_Color = Color.white;
        /// <summary>
        /// 文本内容
        /// </summary>
        [TextArea]
        public string m_Text = null;
        /// <summary>
        /// 指示器缩放的最大尺寸
        /// </summary>
        public float m_MaxSize = 50f;
        /// <summary>
        /// 闪光
        /// </summary>
        public bool flashing = false;
        /// <summary>
        /// Scaling
        /// </summary>
        public Scaling scaling = Scaling.None;
    }
}
