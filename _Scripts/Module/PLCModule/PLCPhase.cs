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
    /// PLC阶段
    /// </summary>
    [XmlType("PLCPhase")]
    public class PLCPhase
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

        [XmlArray("Flows")]
        [XmlArrayItem("Flow")]
        public List<PLCFlow> Flows { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [XmlIgnore]
        public float MinTick { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [XmlIgnore]
        public float MaxTick { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        [XmlIgnore]
        public RunningStatus Status { get; set; }

        public void Internal()
        {
            Flows[0].Internal();
            Flows[Flows.Count - 1].Internal();
            MinTick = Flows[0].MinTick;
            MaxTick = Flows[Flows.Count - 1].MaxTick;
        }
    }

    /// <summary>
    /// PLC阶段集合
    /// </summary>
    [XmlType("PLCPhaseCollection")]
    public class PLCPhaseCollection : DataObject<PLCPhaseCollection>
    {
        /// <summary>
        /// 引导列表
        /// </summary>
        [XmlArray("Phases")]
        [XmlArrayItem("Phase")]
        public List<PLCPhase> PLCPhases { get; set; }
    }
}
