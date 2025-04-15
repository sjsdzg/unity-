using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 导航栏菜单项
    /// </summary>
    [XmlType("NavMenuItem")]
    public class NavMenuItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        [XmlAttribute("active")]
        public bool Active { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 超链接
        /// </summary>
        [XmlAttribute("href")]
        public string Href { get; set; }
    }
}
