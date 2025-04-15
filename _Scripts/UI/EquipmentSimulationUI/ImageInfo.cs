using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 图标
    /// </summary>
    [Serializable]
    public class ImageInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SerializeField]
        public string name;

        /// <summary>
        /// 精灵
        /// </summary>
        [SerializeField]
        public Sprite sprite;
    }
}
