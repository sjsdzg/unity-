using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 流体
    /// </summary>
    [XmlType("Fluid")]
    public class Fluid
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 流速
        /// </summary>
        [XmlAttribute("velocity")]
        public int Velocity { get; set; }

        /// <summary>
        /// UV 注意格式 0,0
        /// </summary>
        [XmlAttribute("uv")]
        public string UV { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}
