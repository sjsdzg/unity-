using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 网络文件项数据
    /// </summary>
    public class NetworkFileItemData
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        public NetworkFile NetworkFile { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public Sprite FileIcon { get; set; }

        /// <summary>
        /// 是否编辑
        /// </summary>
        public bool IsEdit { get; set; }
    }
}
