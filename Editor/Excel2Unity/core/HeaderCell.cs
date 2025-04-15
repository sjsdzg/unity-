using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    /// <summary>
    /// 表头单元
    /// </summary>
    public class HeaderCell
    {
        /// <summary>
        /// 表头文本
        /// </summary>
        public string HeaderText { set; get; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { set; get; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { set; get; }
    }
}
