using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework
{
    public class MessageType
    {
        #region 场景漫游模块
        /// <summary>
        /// 进出房间消息
        /// </summary>
        public const string WorkshopInterOrExterMsg = "WorkshopInterOrExterMsg";

        /// <summary>
        /// GMP点击消息
        /// </summary>
        public const string WorkshopGmpMsg = "WorkshopGmpMsg";

        /// <summary>
        /// 总体介绍点击消息
        /// </summary>
        public const string WorkshopOverallMsg = "WorkshopOverallMsg";


        /// <summary>
        /// 设备相关消息
        /// </summary>
        public const string WorkshopDeviceMsg = "WorkshopDeviceMsg";
        #endregion

        /// <summary>
        /// 打开文件
        /// </summary>
        public const string OpenFileMsg = "OpenFileMsg";
    }
}
