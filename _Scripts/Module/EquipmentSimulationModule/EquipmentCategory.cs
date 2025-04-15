using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 设备库节点
    /// </summary>
    [XmlType("EquipmentCategory")]
    public class EquipmentCategory : DataObject<EquipmentCategory>
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

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
        /// 设备库列表
        /// </summary>
        [XmlElement("EquipmentCategory")]
        public List<EquipmentCategory> Categorys = new List<EquipmentCategory>();

        /// <summary>
        /// 设备列表
        /// </summary>
        [XmlElement("Equipment")]
        public List<Equipment> Equipments = new List<Equipment>();

        /// <summary>
        /// 获取设备库
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EquipmentCategory Get(string name)
        {
            EquipmentCategory category = Categorys.Find(x => x.Name.Equals(name));
            return category;
        }
    }
}
