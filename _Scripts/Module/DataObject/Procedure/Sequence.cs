using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 序列
    /// </summary>
    public class Sequence
    {
        /// <summary>
        /// 序列id
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// 是否监视
        /// </summary>
        [XmlAttribute("monitor")]
        public bool Monitor { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 动作列表
        /// </summary>
        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<_Action> Actions { get; set; }
    }
}
