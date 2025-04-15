using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 属性项
    /// </summary>
    [XmlType("PropertyItem")]
    public class PropertyItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [XmlAttribute("type")]
        public PropertyItemType Type { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// 属性项类型
    /// </summary>
    public enum PropertyItemType
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 长文本
        /// </summary>
        LongText,
        /// <summary>
        /// 输入框
        /// </summary>
        InputField,
        /// <summary>
        /// 下拉框
        /// </summary>
        Dropdown,
        /// <summary>
        /// 链接
        /// </summary>
        Link,
    }
}
