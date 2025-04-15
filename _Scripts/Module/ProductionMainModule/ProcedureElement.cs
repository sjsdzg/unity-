using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Simulation;

namespace XFramework.Module
{
    /// <summary>
    /// 生产操作流程元素
    /// </summary>
    public class ProcedureElement
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
        /// 生产操作流程类型
        /// </summary>
        public ProcedureType Type { get; set; }

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
