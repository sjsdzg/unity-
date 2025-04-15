using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 设备仿真模块
    /// </summary>
    public class EquipmentSimulationModule : BaseModule
    {
        /// <summary>
        /// 设备信息集合
        /// </summary>
        public EquipmentCategory EquipmentCategory { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            string path = "Equipments/EquipmentCategory.xml";
            EquipmentCategory = EquipmentCategory.Parser.ParseXmlFromResources(path);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            EquipmentCategory = null;
        }
    }
}
