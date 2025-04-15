using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    /// <summary>
    /// 对象状态 
    /// </summary>
    public enum EnumObjectState
    {
        /// <summary>
        /// The none.
        /// </summary>
        None,
        /// <summary>
        /// The initial.
        /// </summary>
        Initial,
        /// <summary>
        /// The loading.
        /// </summary>
        Loading,
        /// <summary>
        /// The ready.
        /// </summary>
        Ready,
        /// <summary>
        /// The disabled.
        /// </summary>
        Disabled,
        /// <summary>
        /// The closing.
        /// </summary>
        Closing
    }
}
