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
    /// PLC信息
    /// </summary>
    [XmlType("PLCInfo")]
    public class PLCInfo : DataObject<PLCInfo>
    {
        /// <summary>
        /// 控制设备名称
        /// </summary>
        [XmlElement("equipmentName")]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 药品名称
        /// </summary>
        [XmlElement("drugName")]
        public string DrugName { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [XmlIgnore]
        public string Batch { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        [XmlElement("workshopName")]
        public string WorkshopName { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [XmlElement("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// QA负责人
        /// </summary>
        [XmlElement("QALeader")]
        public string QALeader { get; set; }

        /// <summary>
        /// QC负责人
        /// </summary>
        [XmlElement("QCLeader")]
        public string QCLeader { get; set; }

        /// <summary>
        /// 洁净等级
        /// </summary>
        [XmlElement("cleanness")]
        public string Cleanness { get; set; }
    }
}
