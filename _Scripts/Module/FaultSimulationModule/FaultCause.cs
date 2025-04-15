using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 故障原因
    /// </summary>
    [XmlType("FaultCause")]
    public class FaultCause
    {
        /// <summary>
        /// 故障原因编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 故障原因
        /// </summary>
        [XmlAttribute("Cause")]
        public string Cause { get; set; }
    }
}
