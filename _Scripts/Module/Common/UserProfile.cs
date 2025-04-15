using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 用户简介
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别 [0：男 1：女]
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 软件Id
        /// </summary>
        public string SoftwareId { get; set; }
    }
}
