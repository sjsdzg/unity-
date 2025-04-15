using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// 工段元素
    /// </summary>
    public class StageElement
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 工段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工段类型
        /// </summary>
        public StageType Type { get; set; }

        /// <summary>
        /// 生产操作流程元素列表
        /// </summary>
        public List<ProcedureElement> ProcedureElements = new List<ProcedureElement>();
    }
}
