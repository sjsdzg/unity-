using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Module;
using System.Xml;
using XFramework.Common;
using System.Text;
using UnityEditor;

public class DeviceToEquipment : MonoBehaviour
{
    /// <summary>
    /// 设备信息列表
    /// </summary>
    private static List<DeviceInfo> m_DeviceInfos;

    [MenuItem("Tools/Convert/DeviceToEquipment")]
    static void Convert()
    {
        m_DeviceInfos = new List<DeviceInfo>();
        string path = Application.streamingAssetsPath + "/DeviceSimulation/DeviceInfo.xml";
        string xpath = "DeviceSimulation";
        XmlNode node = XMLHelper.GetXmlNodeByXpath(path, xpath);

        if (node == null)
            return;

        foreach (XmlElement element in node.ChildNodes)
        {
            DeviceInfo deviceInfo = new DeviceInfo();
            deviceInfo.Id = element.GetAttribute("Id");
            deviceInfo.Name = element.GetAttribute("Name");
            deviceInfo.Desc = element.GetAttribute("Desc");

            foreach (XmlElement partElement in element.ChildNodes)
            {
                PartInfo partInfo = new PartInfo();
                partInfo.Id = partElement.GetAttribute("Id");
                partInfo.Name = partElement.GetAttribute("Name");
                partInfo.DeviceName = deviceInfo.Name;
                partInfo.Icon = partElement.GetAttribute("Icon");
                partInfo.Desc = partElement.GetAttribute("Desc");

                deviceInfo.partInfos.Add(partInfo);
            }

            m_DeviceInfos.Add(deviceInfo);
        }

        
        foreach (var device in m_DeviceInfos)
        {
            Equipment equeipment = new Equipment();
            equeipment.EquipmentParts = new List<EquipmentPart>();
            equeipment.AssemblySteps = new List<AssemblyStep>();
            equeipment.Id = device.Id;
            equeipment.Name = device.Name;
            equeipment.Description = device.Desc;

            AssemblyStep assemblyStep = new AssemblyStep();
            assemblyStep.Number = 1;
            assemblyStep.EquipmentParts = new List<EquipmentPart>();
            foreach (var part in device.partInfos)
            {
                EquipmentPart equipmentPart = new EquipmentPart();
                equipmentPart.Id = part.Id;
                equipmentPart.Name = part.Name;
                equipmentPart.Sprite = part.Icon;
                equipmentPart.Description = part.Desc;
                equeipment.EquipmentParts.Add(equipmentPart);
                assemblyStep.EquipmentParts.Add(equipmentPart);
            }

            equeipment.AssemblySteps.Add(assemblyStep);
            //序列化流程
            string dir = "Assets/Resources/DeviceSimulation/" + equeipment.Name;
            AssetDatabase.CreateFolder(dir, "Assembly");
            //string savePath = Application.streamingAssetsPath + "/DeviceSimulation/" + equeipment.Name + "/Assembly/" + equeipment.Name + ".xml";
            XMLHelper.SerializeToFile(equeipment, dir + "/Assembly/" + equeipment.Name + ".xml", Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }


}
