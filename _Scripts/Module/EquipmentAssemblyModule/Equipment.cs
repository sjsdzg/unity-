using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Text;
using XFramework.Common;
using System;
using System.Collections.Generic;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 设备信息
    /// </summary>
    [XmlType("Equipment")]
    public class Equipment : DataObject<Equipment>
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        /// <summary>
        /// 名称
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
        /// 引导列表
        /// </summary>
        [XmlArray("EquipmentParts")]
        [XmlArrayItem("EquipmentPart")]
        public List<EquipmentPart> EquipmentParts { get; set; }

        /// <summary>
        /// 拆装步骤
        /// </summary>
        [XmlArray("AssemblySteps")]
        [XmlArrayItem("AssemblyStep")]
        public List<AssemblyStep> AssemblySteps { get; set; }

        /// <summary>
        /// 获取拆装步骤
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public AssemblyStep GetAssemblyStep(int number)
        {
            AssemblyStep assemblyStep = null;
            assemblyStep = AssemblySteps.Find(x => x.Number == number);
            return assemblyStep;
        }

        /// <summary>
        /// 是否拆装部件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsAssemblyPart(string name)
        {
            bool flag = false;
            foreach (var assemblySteps in AssemblySteps)
            {
                foreach (var equipmentPart in assemblySteps.EquipmentParts)
                {
                    if (equipmentPart.Name == name)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 从StreamingAssets中加载
        /// </summary>
        /// <returns></returns>
        public static Equipment LoadFromStreamingAssets(string path, Encoding encoding)
        {
            Equipment equipment = null;
            try
            {
                equipment = XMLHelper.DeserializeFromFile<Equipment>(path, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return equipment;
        }

        /// <summary>
        /// 从Resource中加载
        /// </summary>
        /// <returns></returns>
        public static Equipment LoadFromResources(string path, Encoding encoding)
        {
            Equipment equipment = null;
            try
            {
                TextAsset urlFile = Resources.Load(path) as TextAsset;
                string url = (urlFile != null) ? urlFile.text.Trim() : null;
                if (string.IsNullOrEmpty(url))
                    return null;
                equipment = XMLHelper.Deserialize<Equipment>(url, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return equipment;
        }
    }
}

