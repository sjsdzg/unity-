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
    /// 引导集合
    /// </summary>
    [XmlType("GuideCollection")]
    public class GuideCollection : DataObject<GuideCollection>
    {
        /// <summary>
        /// 引导列表
        /// </summary>
        [XmlArray("GuideNodes")]
        [XmlArrayItem("GuideNode")]
        public List<GuideNode> GuideNodes { get; set; }
    }
}
