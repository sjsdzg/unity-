using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Core
{
    /// <summary>
    /// AssetBundle信息
    /// </summary>
    [XmlType("AssetBundleInfo")]
    public class AssetBundleInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// AssetBundle 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// MD5
        /// </summary>
        [XmlAttribute("hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Length(文件大小)
        /// </summary>
        [XmlAttribute("length")]
        public long Length { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}
