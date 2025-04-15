using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 工具抽象类
    /// </summary>
    public class Item
    {
        /// <summary>
        /// 工具Id
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 工具名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 工具分类
        /// </summary>
        [XmlAttribute("type")]
        public ItemType Type { get; set; }

        /// <summary>
        /// 工具图标
        /// </summary>
        [XmlAttribute("sprite")]
        public string Sprite { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// 工具分类
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 清洁工具
        /// </summary>
        Clean,
        /// <summary>
        /// 文件
        /// </summary>
        Document,
        /// <summary>
        /// 物品
        /// </summary>
        Goods,
    }

}
