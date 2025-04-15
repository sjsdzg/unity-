using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Module
{
    /// <summary>
    /// 验证仿真Module
    /// </summary>
    public class ProductionSimulationModule : BaseModule
    {
        /// <summary>
        /// root dir
        /// </summary>
        public string rootDir { get; private set; }

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// 当前工段
        /// </summary>
        public StageType CurrentStageType { get; private set; }

        /// <summary>
        /// 当前流程类型
        /// </summary>
        public ProcedureType CurrentProcedureType { get; private set; }

        /// <summary>
        /// 操作流程
        /// </summary>
        private Procedure procedure = null;

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
            rootDir = "Simulation/";
            Project = Project.Parser.ParseXmlFromResources(rootDir + "Project.xml");
            //if (App.Instance.VersionTag == VersionTag.SNTCM)
            //{
            //    Project = Project.Parser.ParseXmlFromResources(rootDir + "SNTCM_Project.xml");
            //}
            //else
            //{

            //}

            //Items
            cleanCollection = CleanCollection.Parser.ParseXmlFromResources(rootDir + "Items/CleanConfig.xml");
            goodsCollection = GoodsCollection.Parser.ParseXmlFromResources(rootDir + "Items/GoodsConfig.xml");
            documentCollection = DocumentCollection.Parser.ParseXmlFromResources(rootDir + "Items/DocumentConfig.xml");
            

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
        public void Init(StageType stageType, ProcedureType procedureType)
        {
            CurrentStageType = stageType;
            CurrentProcedureType = procedureType;
            //Procedure
            Procedure temp = Project.GetProcedure(stageType, procedureType);
            procedure = Procedure.Parser.ParseXmlFromResources(rootDir + temp.URL);
            Debug.Log(procedure);
            procedure.Name = temp.Name;
            procedure.Type = temp.Type;
            procedure.Workshops = temp.Workshops;
            ReplaceProcedureDrugName(procedure);

        }
        /// <summary>
        /// 替换操作流程全部药名
        /// </summary>
        /// <param name="procedure"></param>
        private void ReplaceProcedureDrugName(Procedure procedure)
        {
            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                    break;
                case VersionTag.FZDX:
                    break;
                case VersionTag.TJCU:
                    break;
                case VersionTag.SNTCM:
                case VersionTag.WHGCDX:
                    #region 改名
                    for (int i = 0; i < procedure.Sequences.Count; i++)
                    {
                        if (procedure.Sequences[i].Desc.IndexOf("缬沙坦") != -1)
                        {
                            procedure.Sequences[i].Desc.Replace("缬沙坦", GlobalManager.DrugName);  //seq   dsec                          
                        }
                        for (int j = 0; j < procedure.Sequences[i].Actions.Count; j++)
                        {
                            if (procedure.Sequences[i].Actions[j].ShortDesc.IndexOf("缬沙坦") != -1)
                            {
                                procedure.Sequences[i].Actions[j].ShortDesc.Replace("缬沙坦", GlobalManager.DrugName);//act short
                            }
                            else if (procedure.Sequences[i].Actions[j].Desc.IndexOf("缬沙坦") != -1)
                            {
                                procedure.Sequences[i].Actions[j].Desc.Replace("缬沙坦", GlobalManager.DrugName); //act desc
                            }
                        }
                    }
                    for (int i = 0; i < procedure.KnowledgePoints.Count; i++)
                    {
                        if (procedure.KnowledgePoints[i].Description.IndexOf("缬沙坦") != -1)
                        {
                            procedure.KnowledgePoints[i].Description.Replace("缬沙坦", GlobalManager.DrugName); //KnowledgePoints Name
                        }
                        if (procedure.KnowledgePoints[i].Name.IndexOf("缬沙坦") != -1)
                        {
                            procedure.KnowledgePoints[i].Name.Replace("缬沙坦", GlobalManager.DrugName);//KnowledgePoints desc
                        }
                    }

                    for (int i = 0; i < procedure.AssessmentPoints.Count; i++)
                    {
                        if (procedure.AssessmentPoints[i].Desc.IndexOf("缬沙坦") != -1)
                        {
                            procedure.AssessmentPoints[i].Desc.Replace("缬沙坦", GlobalManager.DrugName); //AssessmentPoints Desc
                        }
                    }


                    #endregion
                    break;
                default:
                    break;
            }
        }

        #region 获取数据
        /// <summary>
        /// 获取操作流程
        /// </summary>
        /// <returns></returns>
        public Procedure GetProcedure()
        {
            return procedure;
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
            return procedure.Entities;
        }

        /// <summary>
        /// 获取考核点列表
        /// </summary>
        /// <returns></returns>
        public List<AssessmentPoint> GetAssessmentPoints()
        {
            return procedure.AssessmentPoints;
        }

        /// <summary>
        /// 获取知识点列表
        /// </summary>
        /// <returns></returns>
        public List<KnowledgePoint> GetKnowledgePoints()
        {
            return procedure.KnowledgePoints;
        }

        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <returns></returns>
        public List<CheckQuestion> GetCheckQuestions()
        {
            return procedure.CheckQuestions;
        }
        #endregion
    }
}
