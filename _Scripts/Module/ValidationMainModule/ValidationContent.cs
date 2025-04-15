using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ValidationContent
    {
        /// <summary>
        /// 生产操作流程编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 生产操作流程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ValidationItem列表
        /// </summary>
        public List<ValidationItem> Items = new List<ValidationItem>();
    }
}
