using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 行为
    /// </summary>
    [XmlType("Action")]
    public class _Action 
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute("")]
        public int ID { get; set; }

        /// <summary>
        /// 是否监视
        /// </summary>
        [XmlAttribute("monitor")]
        public bool Monitor { get; set; }

        /// <summary>
        /// 短描述
        /// </summary>
        [XmlAttribute("shortDesc")]
        public string ShortDesc { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }
    }
}
