using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 阀门状态项
    /// </summary>
    [XmlType("ValveStatusItem")]
    public class ValveStatusItem : StatusItem
    {
        /// <summary>
        /// 值
        /// </summary>
        [XmlAttribute("value")]
        public bool Value { get; set; }
    }
}
