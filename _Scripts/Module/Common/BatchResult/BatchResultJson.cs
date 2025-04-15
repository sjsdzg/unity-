using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 批量操作结果
    /// </summary>
    public class BatchResultJson : DataObject<BatchResultJson>
    {
        /// <summary>
        /// 正常
        /// </summary>
        public const int NORMAL = 0;

        /// <summary>
        /// 警告
        /// </summary>
        public const int WARNING = 1;

        /// <summary>
        /// 错误
        /// </summary>
        public const int ERROR = 2;

        /// <summary>
        /// 异常
        /// </summary>
        public const int EXCEPTION = 3;

        /// <summary>
        /// 批量操作项列表
        /// </summary>
        public List<ResultJson> Items { get; set; }
    }
}
