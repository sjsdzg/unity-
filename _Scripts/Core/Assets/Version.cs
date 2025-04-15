using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Core
{
    /// <summary>
    /// 版本
    /// </summary>
    [XmlType("Version")]
    public class Version : DataObject<Version>
    {
        /// <summary>
        /// 版本文件
        /// </summary>
        public const string url = "version.txt";

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlAttribute("number")]
        public string Number { get; set; }

        /// <summary>
        /// 资源列表
        /// </summary>
        [XmlArray("AssetBundleInfos")]
        [XmlArrayItem("AssetBundleInfo")]
        public List<AssetBundleInfo> AssetBundleInfos { get; set; }
    }
}
