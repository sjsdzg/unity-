using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.UI
{
    public class TaskMonitorItemData
    {
        /// <summary>
        /// 异步操作
        /// </summary>
        public AsyncLoadOperation Async { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public Sprite FileIcon { get; set; }
    }
}
