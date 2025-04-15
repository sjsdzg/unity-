using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 清洁项
    /// </summary>
    [XmlType("Clean")]
    public class Clean : Item
    {
        /// <summary>
        /// 清洁项类型
        /// </summary>
        [XmlAttribute("cleanType")]
        public CleanType CleanType { get; set; }
    }

    [XmlType("CleanCollection")]
    public class CleanCollection : DataObject<CleanCollection>
    {
        /// <summary>
        /// 物品列表
        /// </summary>
        [XmlElement]
        public List<Clean> Cleans { get; set; }
    }
}
