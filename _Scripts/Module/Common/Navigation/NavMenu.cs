using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 导航栏菜单
    /// </summary>
    [XmlType("NavMenu")]
    public class NavMenu
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
        /// 导航栏菜单项列表
        /// </summary>
        [XmlElement("NavMenuItem")]
        public List<NavMenuItem> Items = new List<NavMenuItem>();
    }
}
