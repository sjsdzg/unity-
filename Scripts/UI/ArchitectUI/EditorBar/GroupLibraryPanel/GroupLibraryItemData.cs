using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Architectural;
using XFramework.Module;

namespace XFramework.UI
{
    public class GroupLibraryItemData
    {
        /// <summary>
        /// 组信息
        /// </summary>
        public GroupInfo GroupInfo { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public Sprite Sprite { get; set; }

        public GroupLibraryItemData()
        {

        }

        public GroupLibraryItemData(GroupInfo groupInfo)
        {
            GroupInfo = groupInfo;
        }
    }
}
