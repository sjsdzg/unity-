using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 属性区域
    /// </summary>
    [XmlType("PropertyRegion")]
    public class PropertyRegion
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 属性项列表
        /// </summary>
        [XmlArray("PropertyList")]
        [XmlArrayItem("PropertyItem")]
        public List<PropertyItem> PropertyList { get; set; }
    }
}
