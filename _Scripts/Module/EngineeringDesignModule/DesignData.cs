using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    [XmlType("DesignData")]
    public class DesignData : DataObject<DesignData>
    {
        /// <summary>
        /// 文件夹列表
        /// </summary>
        [XmlElement("Folder")]
        public List<Folder> FolderList = new List<Folder>();
    }
}
