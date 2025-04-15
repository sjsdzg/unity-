using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 子工艺
    /// </summary>
    [XmlType("SubprocessInfo")]
    public class SubprocessInfo
    {
        /// <summary>
        /// 子工艺Id
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 子工艺序号
        /// </summary>
        [XmlAttribute("number")]
        public int Number { get; set; }

        /// <summary>
        /// 子工艺名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 子工艺描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 工艺元件列表
        /// </summary>
        [XmlElement("ProcessElement")]
        public List<ProcessElement> ProcessElements { get; set; }
    }
}
