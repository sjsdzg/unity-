using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 在线用户项数据
    /// </summary>
    public class OnlineUserItemData
    {
        /// <summary>
        /// 用户简介
        /// </summary>
        public UserProfile UserProfile { get; set; }

        /// <summary>
        /// 用户图标
        /// </summary>
        public Sprite UserIcon { get; set; }
    }
}
