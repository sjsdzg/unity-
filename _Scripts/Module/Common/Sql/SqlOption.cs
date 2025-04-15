using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// Sql选项
    /// </summary>
    public enum SqlOption : int
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 模糊like
        /// </summary>
        Like,
        /// <summary>
        /// 排序
        /// </summary>
        OrderBy,
        /// <summary>
        /// 限制
        /// </summary>
        Limit,
        /// <summary>
        /// 在...之内
        /// </summary>
        In,
    }
}
