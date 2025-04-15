using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    [XmlType("ProcessElement")]
    public class ProcessElement
    {
        /// <summary>
        /// 工艺元件Id
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 工艺元件名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 工艺元件描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }
    }
}
