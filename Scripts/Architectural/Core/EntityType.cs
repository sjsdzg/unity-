using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    /// <summary>
    /// 元件类型
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 墙角
        /// </summary>
        Corner,
        /// <summary>
        /// 墙
        /// </summary>
        Wall,
        /// <summary>
        /// 房间
        /// </summary>
        Room,
        /// <summary>
        /// 门
        /// </summary>
        Door,
        /// <summary>
        /// 窗
        /// </summary>
        Window,
        /// <summary>
        /// 垭口
        /// </summary>
        Pass,
        /// <summary>
        /// 组
        /// </summary>
        Group,
        /// <summary>
        /// 区域
        /// </summary>
        Area,
        /// <summary>
        /// 楼层
        /// </summary>
        Floor,
        /// <summary>
        /// 设备
        /// </summary>
        Equipment,

        #region 注释

        /// <summary>
        /// 文本标注
        /// </summary>
        AText,

        #endregion
    }
}
