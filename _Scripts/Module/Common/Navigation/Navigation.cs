using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 导航
    /// </summary>
    [XmlType("Navigation")]
    public class Navigation : DataObject<Navigation>
    {
        /// <summary>
        /// 导航栏菜单列表
        /// </summary>
        [XmlElement("NavMenu")]
        public List<NavMenu> Items = new List<NavMenu>();
    }
}
