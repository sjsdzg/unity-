using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Network
{
    public class Tag
    {
        /// <summary>
        /// 普通数据请求
        /// </summary>
        public const byte GENERAL = 0x01;
        /// <summary>
        /// 心跳
        /// </summary>
        public const byte HEARTBEAT = 0x02;
    }
}
