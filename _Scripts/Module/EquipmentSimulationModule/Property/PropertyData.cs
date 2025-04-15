using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// 属性数据
    /// </summary>
    [XmlType("PropertyData")]
    public class PropertyData
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 属性区域列表
        /// </summary>
        [XmlArray("PropertyRegions")]
        [XmlArrayItem("PropertyRegion")]
        public List<PropertyRegion> PropertyRegions { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 从StreamingAssets中加载
        /// </summary>
        /// <returns></returns>
        public static PropertyData LoadFromStreamingAssets(string path, Encoding encoding)
        {
            PropertyData propertyData = null;
            try
            {
                propertyData = XMLHelper.DeserializeFromFile<PropertyData>(path, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return propertyData;
        }

        /// <summary>
        /// 从Resource中加载
        /// </summary>
        /// <returns></returns>
        public static PropertyData LoadFromResources(string path, Encoding encoding)
        {
            PropertyData propertyData = null;
            try
            {
                TextAsset urlFile = Resources.Load(path) as TextAsset;
                string url = (urlFile != null) ? urlFile.text.Trim() : null;
                if (string.IsNullOrEmpty(url))
                    return null;
                propertyData = XMLHelper.Deserialize<PropertyData>(url, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return propertyData;
        }
    }
}
