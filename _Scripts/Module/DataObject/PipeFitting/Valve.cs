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
    /// 阀门
    /// </summary>
    [XmlType("Valve")]
    public class Valve
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 所属设备
        /// </summary>
        [XmlAttribute("equipment")]
        public string Equipment { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        [XmlAttribute("chineseName")]
        public string ChineseName { get; set; }
        /// <summary>
        /// 2D名称
        /// </summary>
        [XmlAttribute("name2D")]
        public string Name2D { get; set; }
        /// <summary>
        /// 阀门类型
        /// </summary>
        [XmlAttribute("type")]
        public ValveType Type { get; set; }

        /// <summary>
        /// 所在管路
        /// </summary>
        [XmlAttribute("location")]
        public string Location { get; set; }

        /// <summary>
        /// 阀门状态
        /// </summary>
        [XmlAttribute("state")]
        public ValveState State { get; set; }

        /// <summary>
        /// 是否交换
        /// </summary>
        [XmlAttribute("interaction")]
        public bool Interaction { get; set; }

        /// <summary>
        /// 公称直径
        /// </summary>
        [XmlAttribute("DN")]
        public string NominalDiameter { get; set; }

        /// <summary>
        /// 通过介质
        /// </summary>
        [XmlAttribute("medium")]
        public string Medium { get; set; }
    }

    [XmlType("ValveCollection")]
    public class ValveCollection : DataObject<ValveCollection>
    {
        [XmlElement]
        public List<Valve> Valves { get; set; }
    }

    /// <summary>
    /// 阀门状态
    /// </summary>
    public enum ValveState
    {
        /// <summary>
        /// 空
        /// </summary>
        NON,
        /// <summary>
        /// 开
        /// </summary>
        ON,
        /// <summary>
        /// 关
        /// </summary>
        OFF,
    }

    /// <summary>
    /// 阀门类型
    /// </summary>
    public enum ValveType
    {
        None,
        /// <summary>
        /// 手动球阀
        /// </summary>
        VQ,
        /// <summary>
        /// 手动截止阀
        /// </summary>
        VJ,
        /// <summary>
        /// 安全阀
        /// </summary>
        VA,
        /// <summary>
        /// 止回阀
        /// </summary>
        VH,
        /// <summary>
        /// 气动截止阀
        /// </summary>
        SV,
        /// <summary>
        /// 气动球阀
        /// </summary>
        SQ,
        /// <summary>
        /// 手动闸阀
        /// </summary>
        VZ,
        /// <summary>
        /// 气动闸阀
        /// </summary>
        SZ,
        /// <summary>
        /// 疏水阀
        /// </summary>
        VS,
        /// <summary>
        /// 气动隔膜阀
        /// </summary>
        SG,
        /// <summary>
        /// 法兰电磁阀
        /// </summary>
        DC,

    }
}
