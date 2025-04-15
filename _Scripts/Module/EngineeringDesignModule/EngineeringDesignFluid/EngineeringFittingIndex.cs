using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 工程设计 -- 流体节点
    /// </summary>
    [XmlType("EngineeringFittingIndex")]
    public class EngineeringFittingIndex
    {
        /// <summary>
        /// 流体所属房间
        /// </summary>
        [XmlAttribute("parentName")]
        public string ParentName { get; set; }

        /// <summary>
        /// 流体所属类型
        /// </summary>
        [XmlAttribute("fluidType")]
        public string FluidType { get; set; }

        /// <summary>
        /// 流体所属类型的知识点
        /// </summary>
        [XmlAttribute("knowledgePointIndex")]
        public string KnowledgepointIndex { get; set; }
    }
}
