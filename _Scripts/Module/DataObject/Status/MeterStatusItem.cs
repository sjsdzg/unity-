using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 仪表状态项
    /// </summary>
    [XmlType("MeterStatusItem")]
    public class MeterStatusItem : StatusItem
    {
        /// <summary>
        /// 值
        /// </summary>
        [XmlAttribute("value")]
        public float Value { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        [XmlAttribute("format")]
        public string Format { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [XmlAttribute("unit")]
        public string Unit { get; set; }
    }
}
