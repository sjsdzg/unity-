using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// 注册类型
    /// </summary>
    public enum RegisterType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 请求/响应模式
        /// </summary>
        Request = 1,
        /// <summary>
        /// 订阅/发布模式
        /// </summary>
        Subscribe = 2,
    }
}
