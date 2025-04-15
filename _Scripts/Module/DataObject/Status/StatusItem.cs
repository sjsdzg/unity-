using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Module
{
    [XmlType("StatusItem")]
    public class StatusItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        [XmlAttribute("chineseName")]
        public string ChineseName { get; set; }

        /// <summary>
        /// 所属设备
        /// </summary>
        [XmlAttribute("em")]
        public string EM { get; set; }

        /// <summary>
        /// TopMost
        /// </summary>
        [XmlAttribute("topMost")]
        public bool TopMost { get; set; }
    }


}
