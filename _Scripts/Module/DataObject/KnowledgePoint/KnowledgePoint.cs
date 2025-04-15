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
    [XmlType("KnowledgePoint")]
    public class KnowledgePoint
    {
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 知识点类型
        /// </summary>
        [XmlAttribute("type")]
        public KnowledgePointType Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("sprite")]
        public string Sprite { get; set; }

        /// <summary>
        /// 文件夹路径
        /// </summary>
        [XmlAttribute("directory")]
        public string Directory { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// url
        /// </summary>
        [XmlAttribute("url")]
        public string URL { get; set; }
    }

    /// <summary>
    /// 引导集合
    /// </summary>
    [XmlType("KnowledgePointCollection")]
    public class KnowledgePointCollection : DataObject<KnowledgePointCollection>
    {
        /// <summary>
        /// 引导列表
        /// </summary>
        [XmlArray("KnowledgePoints")]
        [XmlArrayItem("KnowledgePoint")]
        public List<KnowledgePoint> KnowledgePoints { get; set; }
    }
}
