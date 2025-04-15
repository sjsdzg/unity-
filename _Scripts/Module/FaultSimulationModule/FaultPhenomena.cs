using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 故障现象
    /// </summary>
    [XmlType("FaultPhenomena")]
    public class FaultPhenomena
    {
        /// <summary>
        /// 故障现象编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 故障现象
        /// </summary>
        [XmlAttribute("phenomena")]
        public string Phenomena { get; set; }
    }
}
