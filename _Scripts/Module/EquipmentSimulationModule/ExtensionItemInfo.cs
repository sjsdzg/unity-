using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace XFramework.Module
{
    /// <summary>
    /// 扩展Item信息
    /// </summary>
    [XmlType("ExtensionItemInfo")]
    public class ExtensionItemInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 扩展类型
        /// </summary>
        [XmlAttribute("expansionType")]
        public ExpansionType ExpansionType { get; set; }

        /// <summary>
        /// Sprite
        /// </summary>
        [XmlAttribute("sprite")]
        public string Sprite { get; set; }
    }


    public enum ExpansionType
    {
        PDF,
        VIDEO,
        SWF,
    }
}
