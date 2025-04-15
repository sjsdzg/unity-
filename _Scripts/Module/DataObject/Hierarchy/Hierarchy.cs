using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 层级(无子集)
    /// </summary>
    [XmlType("hierarchy")]
    public class Hierarchy : DataObject<Hierarchy>
    {
        /// <summary>
        /// 层级的名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("sprite")]
        public string Sprite { get; set; }

        /// <summary>
        /// 层级的名称
        /// </summary>
        [XmlAttribute("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 层级的Level
        /// </summary>
        [XmlAttribute("level")]
        public string Level { get; set; }

        /// <summary>
        /// 层级的所有层级路径
        /// </summary>
        [XmlAttribute("allHierarchyPath")]
        public string AllHierarchyPath { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("Hierarchy")]
        public List<Hierarchy> HierarchyList { get; set; }
    }
}
