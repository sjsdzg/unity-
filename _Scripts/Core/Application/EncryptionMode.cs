using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework
{
    public enum EncryptionMode
    {
        /// <summary>
        /// 不加密
        /// </summary>
        None,
        /// <summary>
        /// 软加密
        /// </summary>
        Soft,
        /// <summary>
        /// 加密狗
        /// </summary>
        Dongle,
    }
}
