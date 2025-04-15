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
    /// 管件
    /// </summary>
    [XmlType("PipeFitting")]
    public class PipeFitting : DataObject<PipeFitting>
    {
        /// <summary>
        /// 流体列表
        /// </summary>
        [XmlArray("FittingNodes")]
        [XmlArrayItem("FittingNode")]
        public List<FittingNode> FittingNodes { get; set; }
    }
}
