using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// 生产车间主场景（沙盘场景）
    /// </summary>
	public class WorkshopRoamingModule : BaseModule
    {

        /// <summary>
        /// XML文件路径
        /// </summary>
        //public string XmlPath { get; private set; }

        /// <summary>
        /// 介绍实体列表
        /// </summary>
        //private List<IntroductionPoint> m_IntroductionPoint;

        /// <summary>
        /// 介绍实体集合
        /// </summary>
        private IntroductionPointCollection m_PointCollection;

        protected override void OnLoad()
        {
            base.OnLoad();
            //m_IntroductionPoint = new List<IntroductionPoint>();
            m_PointCollection = IntroductionPointCollection.Parser.ParseXmlFromResources("WorkshopRoaming/IntroductionPoint.xml");
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            //m_IntroductionPoint = null;
        }

        //public void InitData(string xmlPath)
        //{
        //    XmlPath = xmlPath;
        //    LoadIntroductionPointXML();
        //}

        //private void LoadIntroductionPointXML()
        //{
        //    try
        //    {
        //        string xpath = "WorkshopMain/IntroductionPoints";

        //        XmlNode node = XMLHelper.GetXmlNodeByXpath(XmlPath,xpath);

        //        if (node == null)
        //            return;
        //        foreach (XmlElement element in node.ChildNodes)

        //        {
        //            if(element.Name.Equals("IntroductionPoint"))
        //            {
        //                IntroductionPoint point = new IntroductionPoint();
        //                point.Id = element.GetAttribute("id");
        //                point.Name = element.GetAttribute("name");
        //                point.Type = element.GetAttribute("type");
        //                point.Level = element.GetAttribute("level");
        //                point.Icon = element.GetAttribute("icon");
        //                point.Desc = element.GetAttribute("desc");
        //                point.mp3Url = element.GetAttribute("mp3Url");
        //                foreach (XmlElement ConsItme in element.ChildNodes)
        //                {
        //                    if (ConsItme.Name.Equals("Contents"))
        //                    {
        //                        IntroduceContents InItem = new IntroduceContents();
        //                        InItem.Name = ConsItme.GetAttribute("name");
        //                        //InItem.type = (IntroContentType)Enum.Parse(typeof(IntroContentType),ele.GetAttribute("type"));
        //                        InItem.Level = ConsItme.GetAttribute("level");
        //                        InItem.Icon = ConsItme.GetAttribute("icon");
        //                        InItem.Desc = ConsItme.GetAttribute("desc");
        //                        InItem.Type = (IntroContentType)Enum.Parse(typeof(IntroContentType), ConsItme.GetAttribute("type"));

        //                        //foreach (XmlElement _conItem in ConsItme.ChildNodes)
        //                        //{
        //                        //    InItem.ContentItemList.Add(GetChildContent(_conItem));
        //                        //}
        //                        foreach (XmlElement _conItem in ConsItme.ChildNodes)
        //                        {
        //                            switch (InItem.Type)
        //                            {
        //                                case IntroContentType.GMP:
        //                                    InItem.ContentItemList.Add(GetGMPContent(_conItem));
        //                                    break;

        //                                case IntroContentType.OVERALL:
        //                                    InItem.ContentItemList.Add(GetChildContent(_conItem));

        //                                    break;
        //                                case IntroContentType.OTHER:
        //                                    InItem.ContentItemList.Add(GetChildContent(_conItem));
        //                                    break;
        //                                default:
        //                                    Debug.LogError("Type为空："+point.Name+"  "+InItem.Name);
        //                                    break;
        //                            }
        //                        }
        //                        point.IntroduceList.Add(InItem);
        //                    }
        //                    ///获取某房间的设备列表
        //                    else if(ConsItme.Name.Equals("Machines"))
        //                    {
        //                        Machine machine = new Machine();
        //                        machine.Name = ConsItme.GetAttribute("name");
        //                        machine.Level = ConsItme.GetAttribute("level");
        //                        machine.Icon = ConsItme.GetAttribute("icon");
        //                        machine.Desc = ConsItme.GetAttribute("desc");
        //                        foreach (XmlElement item in ConsItme.ChildNodes)
        //                        {
        //                            MachineItem _machineItme = new MachineItem();
        //                            _machineItme.Id = item.GetAttribute("id");
        //                            _machineItme.Name = item.GetAttribute("name");
        //                            _machineItme.Level = item.GetAttribute("level");
        //                            //macPoint.Type = (KnowledgePointType)Enum.Parse(typeof(KnowledgePointType), element.GetAttribute("type"));
        //                            _machineItme.DevUrl = item.GetAttribute("devUrl");
        //                            _machineItme.Mp3Url = item.GetAttribute("mp3Url");
        //                            _machineItme.Icon = item.GetAttribute("icon");
        //                            _machineItme.Desc = item.GetAttribute("desc");
        //                            machine.MachineList.Add(_machineItme);
        //                        }
        //                        point.MachinesList.Add(machine);
        //                    }
        //                }

        //                m_IntroductionPoint.Add(point);
        //            }
        //        }
        //    }
        //    catch(Exception ex) {

        //        Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "WorkshopMainModule", "LoadIntroductionPointXML");
        //    }
        //}

        ///// <summary>
        ///// 获取GMP
        ///// </summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //private GmpPoint  GetGMPContent(XmlElement element)
        //{

        //        GmpPoint _content = new GmpPoint();
        //        _content.Name = element.GetAttribute("name");
        //        _content.Level = element.GetAttribute("level");
        //        _content.Icon = element.GetAttribute("icon");
        //        _content.Desc = element.GetAttribute("desc");
        //        _content.Position = ConvertToVector3(element.GetAttribute("position"));
        //        _content.Rotation = ConvertToVector3(element.GetAttribute("rotation"));
        //        _content.ImpObject = element.GetAttribute("impObject");
        //        _content.CameraMode =(ViewMode)Enum.Parse(typeof(XFramework.UI.ViewMode), element.GetAttribute("cameraMode"));
        //        _content.Text = element.InnerText;
        //        return _content;
        //}

        /// <summary>
        /// /获取 other
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private IntroduceContent GetChildContent(XmlElement element)
        {

            IntroduceContent _content = new IntroduceContent();
            _content.Name = element.GetAttribute("name");
            _content.Level = element.GetAttribute("level");
            _content.Icon = element.GetAttribute("icon");
            _content.Desc = element.GetAttribute("desc");
            _content.Mp3Url = element.GetAttribute("mp3Url");

            _content.Text = element.InnerText;
            return _content;
        }

        public List<IntroductionPoint> GetIntroducePoints()
        {
            return m_PointCollection.IntroductionPoints;
        }

        /// <summary>
        /// 获取某房间的整体介绍内容
        /// </summary>
        /// <returns></returns>
        public IntroduceContent GetOverallInfo(string workshopname)
        {
            IntroductionPoint _point = m_PointCollection.IntroductionPoints.FirstOrDefault(x => x.Name == workshopname);

            IntroduceContents _content = _point.IntroduceList.FirstOrDefault(y => y.Name == "整体介绍");

            return _content.ContentItemList[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Vector3 ConvertToVector3(string value)
        {
            Vector3 vector = Vector3.zero;
            string[] temps = value.Split(',');
            if (temps.Length == 3)
            {
                vector.x = Convert.ToSingle(temps[0]);
                vector.y = Convert.ToSingle(temps[1]);
                vector.z = Convert.ToSingle(temps[2]);
            }
            return vector;
        }
    }
}