using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Common;
using XFramework.Module;
using XFramework.UI;

namespace XFramework.PLC
{
    /// <summary>
    /// PLC类型
    /// </summary>
    public enum PLC_Type
    {
        /// <summary>
        /// 空
        /// </summary>
        None,

        /// <summary>
        /// 泡罩机
        /// </summary>
        PLC_BlisterMachine,
        
        /// <summary>
        /// 湿法制粒机PLC
        /// </summary>
        PLC_WetGranulation,
        /// <summary>
        /// 装盒机
        /// </summary>
        PLC_BoxingMachine,
        /// <summary>
        /// 热风循环风箱
        /// </summary>
        PLC_HotWindMachine,

        /// <summary>
        /// 混合机
        /// </summary>
        PLC_MixingMachine,

        /// <summary>
        /// 胶囊填充
        /// </summary>
        PLC_CapsuleFilling,

        /// <summary>
        /// 压片机
        /// </summary>
        PLC_TabletMachine,

        /// <summary>
        /// 包衣机
        /// </summary>
        plc_CoatingMachine,

        /// <summary>
        /// 制水-预处理
        /// </summary>
        PLC_PureWater_Pre_treatment,

        /// <summary>
        /// 制水-纯化水分配
        /// </summary>
        PLC_PW_Distribution,

        /// <summary>
        /// 制水-功能说明
        /// </summary>
        PLC_PureWater_FunSpecification,

        /// <summary>
        /// 悬浮粒子计数器
        /// </summary>
        PLC_SuspendedParticleCounter,
    }

    /// <summary>
    /// PLC定义
    /// </summary>
    public static class PLC_Define
    {
        public const string PLC_PREFAB = "Prefabs/PLC/";

        /// <summary>
        /// 获取PLC预设的路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPLC_PrefabPath(PLC_Type type)
        {
            string path = string.Empty;
            switch (type)
            {
                case PLC_Type.PLC_WetGranulation:
                    path = PLC_PREFAB + "WetTypeGranulator/PLC_WetTypeGranulator";
                    break;
                case PLC_Type.PLC_HotWindMachine:
                    path = PLC_PREFAB + "HotWindCirculationOven/PLC_HotWindCirculationOven";
                    break;                    
                case PLC_Type.PLC_MixingMachine:
                    path = PLC_PREFAB + "PLC_MixingMachine";
                    break;
                case PLC_Type.PLC_CapsuleFilling:
                    path = PLC_PREFAB + "CapsuleFilling/PLC_CapsuleFilling";
                    break;
                case PLC_Type.PLC_TabletMachine:
                    path = PLC_PREFAB + "TabletMachine/PLC_TabletMachine";
                    break;
                case PLC_Type.plc_CoatingMachine:
                    path = PLC_PREFAB + "CoatingMachine/PLC_CoatingMachine";
                    break;
                case PLC_Type.PLC_PureWater_Pre_treatment:
                    path = PLC_PREFAB + "PLC_PureWater_Pre_treatment";
                    break;
                case PLC_Type.PLC_PureWater_FunSpecification:
                    path = PLC_PREFAB + "PLC_PureWter_FunctionSpecification";
                    break;
                case PLC_Type.PLC_PW_Distribution:
                    path = PLC_PREFAB + "PLC_PW_Distribution";
                    break;
                case PLC_Type.PLC_SuspendedParticleCounter:
                    path = PLC_PREFAB + "PLC_SuspendedParticleCounter";
                    break;
                case PLC_Type.PLC_BlisterMachine:
                    path = PLC_PREFAB + "PLC_BlisterMachine";
                    break;
                default:
                    Debug.Log("Not Find PLC_Type! type: " + type.ToString());
                    break;
            }
            return path;
        }

        /// <summary>
        /// 获取PLC挂载脚本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetPLC_Script(PLC_Type type)
        {
            Type scriptType = null;
            return scriptType;
        }


        /// <summary>
        /// 从XML中获取PLC配置文件
        /// </summary>
        /// <returns></returns>
        public static PLC_Config LoadPLC_ConfigFromXml(string PLC_ConfigPath)
        {
            //如果文件存在话开始解析。
            if (!System.IO.File.Exists(PLC_ConfigPath))
                return null;

            try
            {
                string xpath = "PLC_Config";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(PLC_ConfigPath, xpath);

                if (node == null)
                    return null;

                //PLC配置文件实例
                PLC_Config config = new PLC_Config();
                //遍历
                foreach (XmlElement stageElement in node.ChildNodes)
                {
                    PLC_Stage stage = new PLC_Stage();
                    stage.Id = XmlConvert.ToInt32(stageElement.GetAttribute("id"));
                    stage.Desc = stageElement.GetAttribute("desc");

                    foreach (XmlElement flowElement in stageElement.ChildNodes)
                    {
                        PLC_Flow flow = new PLC_Flow();
                        flow.Id = XmlConvert.ToInt32(flowElement.GetAttribute("id"));
                        flow.Desc = flowElement.GetAttribute("desc");

                        foreach (XmlElement xmlElement in flowElement.ChildNodes)
                        {
                            PLC_Element element = new PLC_Element();
                            element.Id = xmlElement.GetAttribute("id");
                            element.Name = xmlElement.GetAttribute("name");
                            element.Type = (PLC_ElementType)Enum.Parse(typeof(PLC_ElementType), xmlElement.GetAttribute("type"));
                            element.Value = XmlConvert.ToInt32(xmlElement.GetAttribute("value"));
                            element.Desc = xmlElement.GetAttribute("desc");
                            //添加PLC元件
                            flow.m_Elements.Add(element);
                        }
                        //添加PLC流程
                        stage.m_Flows.Add(flow);
                    }
                    //添加PLC工段
                    config.m_Stages.Add(stage);
                }
                //赋值

                return config;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "PLC_Module", "GetPLC_ConfigFromXml");
                return null;
            }
        }

        /// <summary>
        /// 获取PLC_Element列表
        /// </summary>
        /// <param name="stageId">PLC工段Id</param>
        /// <param name="flowId">PLC流程Id</param>
        /// <returns></returns>
        public static List<PLC_Element> GetPLC_Elements(PLC_Config m_PLC_Config, int stageId, int flowId)
        {
            PLC_Stage stage = m_PLC_Config == null ? null : m_PLC_Config.m_Stages.Find(x => x.Id == stageId);
            PLC_Flow flow = stage == null ? null : stage.m_Flows.Find(x => x.Id == flowId);
            List<PLC_Element> m_Elements = flow == null ? null : flow.m_Elements;
            return m_Elements;
        }

        /// <summary>
        /// 获取对应的PLC配置文件的路径
        /// </summary>
        /// <param name="type"></param>
        public static string GetPLC_ConfigPath(EnumUIType type)
        {
            string path = string.Empty;
            switch (type)
            {
                default:
                    Debug.Log("not found config path, type" + type.ToString());
                    break;
            }
            return path;
        }
    }
}
