using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 设备拆装部件信息
    /// </summary>
    [XmlType("EquipmentPart")]
    public class EquipmentPart
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [XmlAttribute("equipmentName")]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("sprite")]
        public string Sprite { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 部件状态
        /// </summary>
        [XmlIgnore]
        public PartState State { get; set; }
    }

    /// <summary>
    /// 部件状态
    /// </summary>
    public enum PartState
    {
        None,
        /// <summary>
        /// 组合
        /// </summary>
        Assembly,
        /// <summary>
        /// 拆卸
        /// </summary>
        Disassembly,
    }
}
