using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// 加载完成之后选项
    /// </summary>
    public enum LoadOptions
    {
        /// <summary>
        /// 不使用
        /// </summary>
        None,
        /// <summary>
        /// 预加载
        /// </summary>
        Preload,
        /// <summary>
        /// 懒加载 - 按需加载
        /// </summary>
        Lazyload,
    }
}
