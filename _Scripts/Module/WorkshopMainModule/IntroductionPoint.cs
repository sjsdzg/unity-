using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 介绍内容知识点
    /// </summary>
    [XmlType("IntroductionPoint")]
    public class IntroductionPoint {

        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string  Id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [XmlAttribute("name")]
        public string  Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [XmlAttribute("level")]
        public string  Level { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 介绍类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// 路径 ？
        /// </summary>
        [XmlAttribute("mp3Url")]
        public string mp3Url { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 知识点内容
        /// </summary>
        [XmlElement("IntroduceContents")]
        public List<IntroduceContents> IntroduceList = new List<IntroduceContents>();

        /// <summary>
        /// 房间设备列表
        /// </summary>
        [XmlElement("Machine")]
        public List<Machine> MachinesList = new List<Machine>();

    }

    /// <summary>
    /// 介绍点
    /// </summary>
    [XmlType("IntroduceContents")]
    public class IntroduceContents
    {
        /// <summary>
        /// 名字
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// 层级
        /// </summary>
        [XmlAttribute("level")]
        public string Level { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 介绍类型
        /// </summary>
        [XmlAttribute("type")]
        public IntroContentType Type { get; set; }

        /// <summary>
        /// 知识点
        /// </summary>
        /// <summary>
        /// 设备列表
        /// </summary>
        [XmlElement("IntroduceContent")]
        public List<IntroduceContent> ContentItemList = new List<IntroduceContent>();

    }

    /// <summary>
    /// 具体的介绍内容的知识点
    /// </summary>
    public class IntroduceContent
    {
        /// <summary>
        /// 名字
        /// </summary>
        [XmlAttribute("name")]
        public string Name;

        /// <summary>
        /// 层级
        /// </summary>
        [XmlAttribute("level")]
        public string Level { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        [XmlText]
        public string Text { get; set; }

        /// <summary>
        /// MP3路径
        /// </summary>
        [XmlAttribute("mp3Url")]
        public string  Mp3Url { get; set; }

        /// <summary>
        /// 内容介绍类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

    }
    /// <summary>
    /// 介绍信息类型
    /// </summary>
    public enum IntrodutionType
    {
        /// <summary>
        /// 
        /// </summary>
        General,
        Machine,
        GMP,
        Verification
    }
}