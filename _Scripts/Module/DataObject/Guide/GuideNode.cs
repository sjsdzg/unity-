using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 引导数据
    /// </summary>
    [XmlType("GuideData")]
    public class GuideNode
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        [XmlAttribute("target")]
        public string Target { get; set; }

        /// <summary>
        /// 提示内容
        /// </summary>
        [XmlAttribute("content")]
        public string Content { get; set; }

        /// <summary>
        /// 引导类型
        /// </summary>
        [XmlAttribute("type")]
        public GuideType Type { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    public enum GuideType
    {
        None,
        /// <summary>
        /// 指示器
        /// </summary>
        Indicator,
    }
}
