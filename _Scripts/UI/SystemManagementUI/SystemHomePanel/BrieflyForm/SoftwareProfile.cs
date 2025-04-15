using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.UI
{
    /// <summary>
    /// 软件简介信息
    /// </summary>
    public class SoftwareProfile
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 许可证点数
        /// </summary>
        public int LicenseNumber { get; set; }
    }
}
