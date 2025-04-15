using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// PLC模块
    /// </summary>
    public class PLC_Module : BaseModule
    {
        /// <summary>
        /// PLC工段配置文件路径
        /// </summary>
        public string PLC_ConfigPath { get; private set; }

        /// <summary>
        /// PLC配置文件
        /// </summary>
        public PLC_Config m_PLC_Config { get; private set; }

        /// <summary>
        /// 加载
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 根据配置文件路径，初始化数据
        /// </summary>
        /// <param name="configPath"></param>
        public void InitData(string configPath)
        {
            PLC_ConfigPath = configPath;
            LoadPLC_ConfigFromXml();
        }

        /// <summary>
        /// 从XML中获取PLC配置文件
        /// </summary>
        /// <returns></returns>
        public void LoadPLC_ConfigFromXml()
        {
            //如果文件存在话开始解析。
            if (!System.IO.File.Exists(PLC_ConfigPath))
                return;

            try
            {
                string xpath = "PLC_Config";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(PLC_ConfigPath, xpath);

                if (node == null)
                    return;

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
                m_PLC_Config = config;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "PLC_Module", "GetPLC_ConfigFromXml");
            }
        }

        /// <summary>
        /// 获取PLC_Element列表
        /// </summary>
        /// <param name="stageId">PLC工段Id</param>
        /// <param name="flowId">PLC流程Id</param>
        /// <returns></returns>
        public List<PLC_Element> GetPLC_Elements(int stageId, int flowId)
        {
            PLC_Stage stage = m_PLC_Config == null ? null : m_PLC_Config.m_Stages.Find(x => x.Id == stageId);
            PLC_Flow flow = stage == null ? null : stage.m_Flows.Find(x => x.Id == flowId);
            List<PLC_Element> m_Elements = flow == null ? null : flow.m_Elements;
            return m_Elements;
        }
    }
}
