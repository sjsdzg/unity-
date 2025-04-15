using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ValidationItem
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
        /// 验证类型
        /// </summary>
        public ValidationType Type { get; set; }

        /// <summary>
        /// 是否有学习模式
        /// </summary>
        public bool Study { get; set; }

        /// <summary>
        /// 是否有考核模式
        /// </summary>
        public bool Examine { get; set; }
    }
}
