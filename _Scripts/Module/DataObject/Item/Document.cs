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
    /// 文件项
    /// </summary>
    [XmlType("Document")]
    public class Document : Item
    {
        public Document()
        {
            base.Type = ItemType.Document;
        }

        /// <summary>
        /// 文件项类型
        /// </summary>
        [XmlAttribute("documentType")]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// 资源索引
        /// </summary>
        [XmlAttribute("url")]
        public string URL { get; set; }
    }

    [XmlType("DocumentCollection")]
    public class DocumentCollection : DataObject<DocumentCollection>
    {
        /// <summary>
        /// 物品列表
        /// </summary>
        [XmlElement]
        public List<Document> Documents { get; set; }
    }
}
