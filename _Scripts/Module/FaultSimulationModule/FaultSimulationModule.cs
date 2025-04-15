using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 故障仿真Module
    /// </summary>
    public class FaultSimulationModule : BaseModule
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootDir { get; private set; }

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// 当前工段
        /// </summary>
        public StageType CurrentStageType { get; private set; }

        /// <summary>
        /// 故障ID
        /// </summary>
        public string CurrentFaultID { get; set; }

        /// <summary>
        /// 故障
        /// </summary>
        private Fault fault = null;

        /// <summary>
        /// 故障信息集合
        /// </summary>
        private FaultInfoCollection faultInfoCollection = null;

        /// <summary>
        /// 清洁物品集合
        /// </summary>
        private CleanCollection cleanCollection = null;

        /// <summary>
        /// 物品集合
        /// </summary>
        private GoodsCollection goodsCollection = null;

        /// <summary>
        /// 文件结合
        /// </summary>
        private DocumentCollection documentCollection = null;

        /// <summary>
        /// 加载
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            RootDir = "Simulation/";
            Project = Project.Parser.ParseXmlFromResources(RootDir + "Project.xml");
            //Items
            cleanCollection = CleanCollection.Parser.ParseXmlFromResources(RootDir + "Items/CleanConfig.xml");
            goodsCollection = GoodsCollection.Parser.ParseXmlFromResources(RootDir + "Items/GoodsConfig.xml");
            documentCollection = DocumentCollection.Parser.ParseXmlFromResources(RootDir + "Items/DocumentConfig.xml");
        }

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
        public void Init(StageType stageType, string faultID)
        {
            CurrentStageType = stageType;
            CurrentFaultID = faultID;
            //Fault
            Fault temp = Project.GetFault(stageType, faultID);
            fault = Fault.Parser.ParseXmlFromResources(RootDir + temp.URL);
            fault.Id = temp.Id;
            fault.Name = temp.Name;
            fault.Workshops = temp.Workshops;
            //FaultInfoCollection
            Stage stage = Project.GetStage(stageType);
            string faultInfoUrl = RootDir + stage.FaultInfoUrl;
            faultInfoCollection = FaultInfoCollection.Parser.ParseXmlFromResources(faultInfoUrl);
        }

        #region 获取数据
        /// <summary>
        /// 获取操作流程
        /// </summary>
        /// <returns></returns>
        public Fault GetFault()
        {
            return fault;
        }

        /// <summary>
        /// 获取故障信息集合
        /// </summary>
        /// <returns></returns>
        public FaultInfoCollection GetFaultInfoCollection()
        {
            return faultInfoCollection;
        }

        /// <summary>
        /// 根据故障ID获取故障信息
        /// </summary>
        /// <param name="faultID"></param>
        /// <returns></returns>
        public FaultInfo GetFaultInfo(string faultID)
        {
            FaultInfo faultInfo = faultInfoCollection.FaultInfos.Find(x => x.ID == faultID);
            return faultInfo;
        }

        /// <summary>
        /// 获取物品Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Goods GetGoods(string name)
        {
            Goods item = null;
            try
            {
                item = goodsCollection.Goods.Find(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return item;
        }

        /// <summary>
        /// 获取文件Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Document GetDocument(string name)
        {
            Document item = null;
            try
            {
                item = documentCollection.Documents.Find(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return item;
        }

        /// <summary>
        /// 获取清洁Item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Clean GetClean(string name)
        {
            Clean item = null;
            try
            {
                item = cleanCollection.Cleans.Find(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return item;
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <returns></returns>
        public List<EntityBase> GetEntities()
        {
            return fault.Entities;
        }

        /// <summary>
        /// 获取考核点列表
        /// </summary>
        /// <returns></returns>
        public List<AssessmentPoint> GetAssessmentPoints()
        {
            return fault.AssessmentPoints;
        }

        /// <summary>
        /// 获取知识点列表
        /// </summary>
        /// <returns></returns>
        public List<KnowledgePoint> GetKnowledgePoints()
        {
            return fault.KnowledgePoints;
        }
        #endregion
    }
}
