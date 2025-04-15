using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 实验项
    /// </summary>
    public class EmpiricalItem
    {
        /// <summary>
        /// 变量
        /// </summary>
        public string Variable { get; set; }

        /// <summary>
        /// 产品收益率
        /// </summary>
        public string ProductRate { get; set; }

        /// <summary>
        /// 产品质量
        /// </summary>
        public string ProductAmount { get; set; }

        public EmpiricalItem(string variable)
        {
            Variable = variable;
            ProductRate = "";
            ProductAmount = "";
        }
    }
}
