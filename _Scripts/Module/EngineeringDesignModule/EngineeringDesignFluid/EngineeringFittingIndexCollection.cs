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
    /// 工程设计 -- 流体集合
    /// </summary>
    [XmlType("EngineeringFittingIndexCollection")]
    public class EngineeringFittingIndexCollection : DataObject<EngineeringFittingIndexCollection>
    {
        /// <summary>
        /// 流体父物体列表
        /// </summary>
        [XmlArray("EngineeringFittingIndexes")]
        [XmlArrayItem("EngineeringFittingIndex")]
        public List<EngineeringFittingIndexCollection> Collection { get; set; }
    }
}
