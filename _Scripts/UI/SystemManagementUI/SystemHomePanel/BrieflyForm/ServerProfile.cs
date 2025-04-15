using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    /// <summary>
    /// 服务器简介信息
    /// </summary>
    public class ServerProfile
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 运行环境
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OperationSystem { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string Database { get; set; }
    }
}
