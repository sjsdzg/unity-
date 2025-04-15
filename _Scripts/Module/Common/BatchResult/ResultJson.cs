using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 批量操作项
    /// </summary>
    public class ResultJson : DataObject<ResultJson>
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
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 类型 
        /// 0 Normal
        /// 1 Warning
        /// 2 Error
        /// 3 Exception
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 关键内容
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }
    }
}
