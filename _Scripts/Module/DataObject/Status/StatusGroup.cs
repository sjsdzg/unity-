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
    [XmlType("StatusGroup")]
    public class StatusGroup
    {
        /// <summary>
        /// Name
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 状态项列表
        /// </summary>
        [XmlArray("StatusItems")]
        [XmlArrayItem(typeof(ValveStatusItem))]
        [XmlArrayItem(typeof(MeterStatusItem))]
        public List<StatusItem> StatusItems { get; set; }
    }

    [XmlType("StatusGroupCollection")]
    public class StatusGroupCollection : DataObject<StatusGroupCollection>
    {
        /// <summary>
        /// 状态项列表
        /// </summary>
        [XmlArray("StatusGroups")]
        [XmlArrayItem("StatusGroup")]
        public List<StatusGroup> StatusGroups { get; set; }
    }
}
