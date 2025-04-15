using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// 拆装步骤
    /// </summary>
    [XmlType("AssemblyStep")]
    public class AssemblyStep
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute("number")]
        public int Number { get; set; }

        /// <summary>
        /// 当前步骤
        /// </summary>
        [XmlArray("EquipmentParts")]
        [XmlArrayItem("EquipmentPart")]
        public List<EquipmentPart> EquipmentParts;

        /// <summary>
        /// 该步骤是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            bool isComplete = true;
            foreach (var item in EquipmentParts)
            {
                if (item.State == PartState.None)
                {
                    isComplete = false;
                    break;
                }
            }
            return isComplete;
        }
    }
}

