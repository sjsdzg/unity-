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
    public class WorkshopWalkModule : BaseModule
    {
        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlPath { get; private set; }

        /// <summary>
        /// 知识点列表  knowledgePoint为公用类
        /// </summary>
        private List<KnowledgePoint> m_KnowledgePoints;

        protected override void OnLoad()
        {
            base.OnLoad();
            m_KnowledgePoints = new List<KnowledgePoint>();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            m_KnowledgePoints = null;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData(string xmlPath)
        {
            XmlPath = xmlPath;
            LoadKnowledgePointFromXml();
        }

        /// <summary>
        /// 从XML中加载知识点数据
        /// </summary>
        private void LoadKnowledgePointFromXml()
        {
            try
            {
                string xpath = "WorkshopWalk/KnowledgePoints";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(XmlPath, xpath);

                if (node == null)
                    return;

                foreach (XmlElement element in node.ChildNodes)
                {
                    if (element.Name.Equals("KnowledgePoint"))
                    {
                        KnowledgePoint point = new KnowledgePoint();
                        point.Id = element.GetAttribute("id");
                        point.Name = element.GetAttribute("name");
                        point.Type = (KnowledgePointType)Enum.Parse(typeof(KnowledgePointType), element.GetAttribute("type"));
                        point.URL = element.GetAttribute("url");
                        point.Sprite = element.GetAttribute("icon");
                        point.Description = element.GetAttribute("desc");
                        m_KnowledgePoints.Add(point);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "FactoryWalkModule", "LoadKnowledgePointFromXml");
            }
        }

        /// <summary>
        /// 获取知识点列表
        /// </summary>
        /// <returns></returns>
        public List<KnowledgePoint> GetKnowledgePoints()
        {
            return m_KnowledgePoints;
        }
    }
}
