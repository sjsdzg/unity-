using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 文件夹节点
    /// </summary>
    [XmlType("Folder")]
    public class Folder : DataObject<Folder>
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
        /// 文件列表
        /// </summary>
        [XmlElement("File")]
        public List<File> FileList = new List<File>();

        /// <summary>
        /// 文件夹列表
        /// </summary>
        [XmlElement("Folder")]
        public List<Folder> FolderList = new List<Folder>();
    }
}
