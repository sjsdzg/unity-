using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    /// <summary>
    /// Sprite
    /// </summary>
    [Serializable]
    public class SpriteInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        [SerializeField]
        public string id;

        /// <summary>
        /// 精灵
        /// </summary>
        [SerializeField]
        public Sprite sprite;
    }
}
