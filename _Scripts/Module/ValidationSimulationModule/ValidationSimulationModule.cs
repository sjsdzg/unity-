using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// 验证仿真Module
    /// </summary>
    public class ValidationSimulationModule : BaseModule
    {
        /// <summary>
        /// 工段流程路径
        /// </summary>
        public string ValidationPath { get; private set; }

        /// <summary>
        /// 标准操作流程名称
        /// </summary>
        public string ProcedureName { get; private set; }

        /// <summary>
        /// 文件项字典
        /// </summary>
        private Dictionary<string, Document> Documents = new Dictionary<string, Document>();

        /// <summary>
        /// 物品项字典
        /// </summary>
        private Dictionary<string, Goods> Goodss = new Dictionary<string, Goods>();

        /// <summary>
        /// 清洁项字典
        /// </summary>
        private Dictionary<string, Clean> Cleans = new Dictionary<string, Clean>();

        /// <summary>
        /// 操作流程
        /// </summary>
        private Procedure m_Procedure = new Procedure();

        /// <summary>
        /// 实体列表
        /// </summary>
        private List<EntityBase> m_Entities = new List<EntityBase>();

        /// <summary>
        /// 考核点列表
        /// </summary>
        private List<AssessmentPoint> m_AssessmentPoints = new List<AssessmentPoint>();

        /// <summary>
        /// 知识点列表
        /// </summary>
        private List<KnowledgePoint> m_KnowledgePoints = new List<KnowledgePoint>();

        /// <summary>
        /// 加载
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="args"></param>
        //protected override void SetParams(params object[] args)
        //{
        //    base.SetParams(args);
        //    ValidationPath = args[0].ToString();
        //    ProcedureName = args[1].ToString();
        //    Initialize(ValidationPath, ProcedureName);
        //}

        /// <summary>
        /// 释放
        /// </summary>
        protected override void OnRelease()
        {
            base.OnRelease();
        }
        
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="stagePath"></param>
        /// <param name="procName"></param>
        private void Initialize(string stagePath, string procName)
        {
            LoadProcedureFromXml(stagePath, procName);
            LoadDocumentFromXml();
            LoadGoodsFromXml();
            LoadCleanFromXml();
            LoadEntitiesFromXml();
            LoadAssessmentPointsFromXml();
            LoadKnowledgePointsFromXml();
        }

        #region 加载数据
        /// <summary>
        /// 从XML中加载流程
        /// </summary>
        /// <param name="path">流程xml文件路径</param>
        /// <param name="name">流程名称（检查，操作，清场）</param>
        /// <returns></returns>
        public void LoadProcedureFromXml(string path, string name)
        {
            //如果文件存在话开始解析。
            if (!System.IO.File.Exists(path))
                return;

            try
            {
                m_Procedure.Name = name;
                //加载流程xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode node = xmlDoc.SelectSingleNode("Procedures/Procedure[@name='" + name + "']/Sequences");
                //获取序列和动作
                foreach (XmlElement seqElement in node.ChildNodes)
                {
                    Sequence seq = new Sequence();
                    seq.ID = XmlConvert.ToInt32(seqElement.GetAttribute("id"));
                    seq.Desc = seqElement.GetAttribute("desc");
                    if (!string.IsNullOrEmpty(seqElement.GetAttribute("monitor")))
                    {

                        seq.Monitor = XmlConvert.ToBoolean(seqElement.GetAttribute("monitor"));
                    }

                    foreach (XmlElement actElement in seqElement.ChildNodes)
                    {
                        _Action act = new _Action();
                        act.ID = XmlConvert.ToInt32(actElement.GetAttribute("id"));
                        act.ShortDesc = actElement.GetAttribute("shortDesc");
                        act.Desc = actElement.GetAttribute("desc");
                        if (!string.IsNullOrEmpty(actElement.GetAttribute("monitor")))
                        {
                            act.Monitor = XmlConvert.ToBoolean(actElement.GetAttribute("monitor"));
                        }

                        seq.Actions.Add(act);
                    }

                    m_Procedure.Sequences.Add(seq);
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "ProductionSimulationModule", "GetProcedure");
            }
        }

        /// <summary>
        /// 从XML中加载文件项列表
        /// </summary>
        /// <returns></returns>
        private void LoadDocumentFromXml()
        {
            //List<Document> items = new List<Document>();
            try
            {
                string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/Files";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

                if (node == null)
                    return;

                foreach (XmlElement element in node.ChildNodes)
                {
                    Document item = new Document();
                    item.ID = element.GetAttribute("id");
                    item.Name = element.GetAttribute("name");
                    item.Sprite = element.GetAttribute("icon");
                    item.Description = element.GetAttribute("desc");
                    item.URL = element.GetAttribute("url");
                    //item.Active = XmlConvert.ToBoolean(element.GetAttribute("active"));
                    item.DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), element.GetAttribute("type"));

                    Documents.Add(item.Name, item);
                    //items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "ProductionSimulationModule", "LoadFilesFromXml");
            }

            //return items;
        }

        /// <summary>
        /// 从XML中加载物品项列表
        /// </summary>
        /// <returns></returns>
        private void LoadGoodsFromXml()
        {
            //List<Goods> items = new List<Goods>();

            string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/Goods";
            XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

            if (node == null)
                return;

            foreach (XmlElement element in node.ChildNodes)
            {
                Goods item = new Goods();
                item.ID = element.GetAttribute("id");
                item.Name = element.GetAttribute("name");
                item.Sprite = element.GetAttribute("icon");
                item.Description = element.GetAttribute("desc");
                //item.URL = element.GetAttribute("url");
                //item.Active = XmlConvert.ToBoolean(element.GetAttribute("active"));
                item.GoodsType = (GoodsType)Enum.Parse(typeof(GoodsType), element.GetAttribute("type"));

                Goodss.Add(item.Name, item);
                //items.Add(item);
            }

            //return items;
        }

        /// <summary>
        /// 从XML中加载清洁项列表
        /// </summary>
        /// <returns></returns>
        private void LoadCleanFromXml()
        {
            //List<Clean> items = new List<Clean>();

            string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/Cleans";
            XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

            if (node == null)
                return;

            foreach (XmlElement element in node.ChildNodes)
            {
                Clean item = new Clean();
                item.ID = element.GetAttribute("id");
                item.Name = element.GetAttribute("name");
                item.Sprite = element.GetAttribute("icon");
                item.Description = element.GetAttribute("desc");
                //item.URL = element.GetAttribute("url");
                //item.Active = XmlConvert.ToBoolean(element.GetAttribute("active"));
                item.CleanType = (CleanType)Enum.Parse(typeof(CleanType), element.GetAttribute("type"));

                Cleans.Add(item.Name, item);
                //items.Add(item);
            }

            //return items;
        }

        /// <summary>
        /// 从XML中加载实体列表
        /// </summary>
        /// <returns></returns>
        public void LoadEntitiesFromXml()
        {
            string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/Entities";
            XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

            if (node == null)
                return;

            foreach (XmlElement element in node.ChildNodes)
            {
                EntityBase entity;

                if (element.Name.Equals("EntityMyself"))
                {
                    entity = new EntityMyself();
                }
                else
                {
                    entity = new EntityNPC();
                }

                entity.Id = element.GetAttribute("id");
                entity.Name = element.GetAttribute("name");
                entity.Vocation = (Vocation)Enum.Parse(typeof(Vocation), element.GetAttribute("vocation"));

                string sex = element.GetAttribute("gender");
                if (sex.Equals("男"))
                    entity.Gender = Gender.Male;
                else
                    entity.Gender = Gender.Female;

                entity.Cleanliness = (Cleanliness)Enum.Parse(typeof(Cleanliness), element.GetAttribute("cleanliness"));
                entity.Greeting = element.GetAttribute("greeting");
                entity.Position = element.GetAttribute("position");
                entity.Rotation = element.GetAttribute("rotation");
                entity.Scale = element.GetAttribute("scale");
                m_Entities.Add(entity);
            }
        }

        /// <summary>
        /// 从XML中加载考核点列表
        /// </summary>
        public void LoadAssessmentPointsFromXml()
        {
            string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/AssessmentPoints";
            XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

            if (node == null)
                return;

            foreach (XmlElement element in node.ChildNodes)
            {
                AssessmentPoint item = new AssessmentPoint();
                item.Id = XmlConvert.ToInt32(element.GetAttribute("id"));
                item.Desc = element.GetAttribute("desc");
                item.Value = XmlConvert.ToInt32(element.GetAttribute("value"));
                m_AssessmentPoints.Add(item);
            }
        }

        public void LoadKnowledgePointsFromXml()
        {
            string xpath = "Procedures/Procedure[@name='" + ProcedureName + "']/KnowledgePoints";
            XmlNode node = XMLHelper.GetXmlNodeByXpath(ValidationPath, xpath);

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
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取操作流程
        /// </summary>
        /// <returns></returns>
        public Procedure GetProcedure()
        {
            return m_Procedure;
        }

        /// <summary>
        /// 获取物品Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Goods GetGoods(string name)
        {
            Goods item = null;

            if (Goodss.TryGetValue(name, out item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Document GetDocument(string name)
        {
            Document item = null;

            if (Documents.TryGetValue(name, out item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取清洁Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Clean GetClean(string name)
        {
            Clean item = null;

            if (Cleans.TryGetValue(name, out item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取启用的文件项列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Document> GetActiveDocuments()
        {
            List<Document> items = new List<Document>();

            //foreach (Document item in Documents.Values)
            //{
            //    if (item.Active)
            //    {
            //        items.Add(item);
            //    }
            //}

            return items;
        }

        /// <summary>
        /// 获取启用的清洁项列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Clean> GetActiveCleans()
        {
            List<Clean> items = new List<Clean>();

            //foreach (Clean item in Cleans.Values)
            //{
            //    if (item.Active)
            //    {
            //        items.Add(item);
            //    }
            //}

            return items;
        }

        /// <summary>
        /// 获取启用的物品项列表
        /// </summary>
        /// <returns></returns>
        public List<Goods> GetActiveGoodss()
        {
            List<Goods> items = new List<Goods>();

            //foreach (Goods item in Goodss.Values)
            //{
            //    if (item.Active)
            //    {
            //        items.Add(item);
            //    }
            //}

            return items;
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns></returns>
        public List<EntityBase> GetEntities()
        {
            return m_Entities;
        }

        /// <summary>
        /// 获取考核点列表
        /// </summary>
        /// <returns></returns>
        public List<AssessmentPoint> GetAssessmentPoints()
        {
            return m_AssessmentPoints;
        }

        /// <summary>
        /// 获取知识点列表
        /// </summary>
        /// <returns></returns>
        public List<KnowledgePoint> GetKnowledgePoints()
        {
            return m_KnowledgePoints;
        }
        #endregion

        /// <summary>
        /// 将字符串转换成Vector3
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
