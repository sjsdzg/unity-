using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 工艺信息
    /// </summary>
    [XmlType("ProcessInfo")]
    public class ProcessInfo
    {
        /// <summary>
        /// 工艺Id
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 工艺名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 视频目录
        /// </summary>
        [XmlAttribute("movieDir")]
        public string MovieDir { get; set; }

        /// <summary>
        /// prefab路径
        /// </summary>
        [XmlAttribute("prefabPath")]
        public string PrefabPath { get; set; }

        /// <summary>
        /// 工作原理文档文件夹
        /// </summary>
        [XmlAttribute("documentDir")]
        public string DocumentDir { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 子工艺列表
        /// </summary>
        [XmlElement("SubprocessInfo")]
        public List<SubprocessInfo> SubProcessInfos { get; set; }
    }

    [XmlType("ProcessInfoCollection")]
    public class ProcessInfoCollection : DataObject<ProcessInfoCollection>
    {
        [XmlElement("ProcessInfo")]
        public List<ProcessInfo> ProcessInfos { get; set; }
    }
}
