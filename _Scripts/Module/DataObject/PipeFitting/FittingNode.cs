using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 管件节点
    /// </summary>
    [XmlType("FittingNode")]
    public class FittingNode
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 管件列表
        /// </summary>
        [XmlArray("Pipes")]
        [XmlArrayItem("Pipe")]
        public List<Pipe> Pipes { get; set; }

        /// <summary>
        /// 流体列表
        /// </summary>
        [XmlArray("Fluids")]
        [XmlArrayItem("Fluid")]
        public List<Fluid> Fluids { get; set; }
        /// <summary>
        /// 阀门列表
        /// </summary>
        [XmlArray("Valves")]
        [XmlArrayItem("Valve")]
        public List<Valve> Valves { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}
