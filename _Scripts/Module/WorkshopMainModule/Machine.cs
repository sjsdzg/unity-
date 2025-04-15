using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    [XmlType("Machine")]
    public class Machine
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
        /// 文件夹路径
        /// </summary>
        [XmlAttribute("dir")]
        public string Dir { get; set; }

        /// <summary>
        /// url
        /// </summary>
        [XmlAttribute("url")]
        public string Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 设备个体的列表
        /// </summary>
        [XmlElement("MachineItem")]
        public List<MachineItem> MachineList = new List<MachineItem>();
      
}

    /// <summary>
    /// 设备项
    /// </summary>
    [XmlType("MachineItem")]
    public class MachineItem {

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
        /// 设备路径
        /// </summary>
        [XmlAttribute("dir")]
        public string Dir { get; set; }

        /// <summary>
        /// 设备路径
        /// </summary>
        [XmlAttribute("devUrl")]
        public string DevUrl { get; set; }

        /// <summary>
        /// MP3路径
        /// </summary>
        [XmlAttribute("mp3Url")]
        public string Mp3Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

    }
}
