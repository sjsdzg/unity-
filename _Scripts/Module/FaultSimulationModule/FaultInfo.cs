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
    /// 故障信息
    /// </summary>
    [XmlType("FaultInfo")]
    public class FaultInfo
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
        /// 故障现象列表
        /// </summary>
        [XmlArray("FaultPhenomenas")]
        [XmlArrayItem("FaultPhenomena")]
        public List<FaultPhenomena> FaultPhenomenas { get; set; }

        /// <summary>
        /// 故障原因列表
        /// </summary>
        [XmlArray("FaultCauses")]
        [XmlArrayItem("FaultCause")]
        public List<FaultCause> FaultCauses { get; set; }

        /// <summary>
        /// 获取故障现象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FaultPhenomena GetFaultPhenomena(string id)
        {
            FaultPhenomena faultPhenomena = FaultPhenomenas.Find(x => x.ID == id);
            return faultPhenomena;
        }
    }


    [XmlType("FaultInfoCollection")]
    public class FaultInfoCollection : DataObject<FaultInfoCollection>
    {
        /// <summary>
        /// 故障信息列表
        /// </summary>
        [XmlElement]
        public List<FaultInfo> FaultInfos { get; set; }
    }
}
