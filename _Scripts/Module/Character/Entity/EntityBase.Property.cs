using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 实体属性类
    /// </summary>
    public partial class EntityBase
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
        /// 岗位
        /// </summary>
        [XmlAttribute("vocation")]
        public Vocation Vocation { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [XmlAttribute("gender")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 清洁等级
        /// </summary>
        [XmlAttribute("cleanliness")]
        public Cleanliness Cleanliness { get; set; }

        /// <summary>
        /// 招呼语
        /// </summary>
        [XmlAttribute("greeting")]
        public string Greeting { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [XmlAttribute("position")]
        public string Position { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        [XmlAttribute("rotation")]
        public string Rotation { get; set; }

        /// <summary>
        /// 缩放
        /// </summary>
        [XmlAttribute("scale")]
        public string Scale { get; set; }


        private Transform cacheTransform;
        /// <summary>
        /// 缓存Transform
        /// </summary>
        [XmlIgnore]
        public Transform CacheTransform
        {
            get { return cacheTransform; }
            set { cacheTransform = value; }
        }

    }
}
