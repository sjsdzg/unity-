using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;

namespace XFramework.Simulation
{
    /// <summary>
    /// 通用工段（处理通用部分）
    /// </summary>
    public class CustomStage : BaseStage
    {
        private AssessmentGrade m_AssessmentGrade = new AssessmentGrade();
        /// <summary>
        /// 考评内容
        /// </summary>
        public AssessmentGrade AssessmentGrade
        {
            get { return m_AssessmentGrade; }
            set { m_AssessmentGrade = value; }
        }

        /// <summary>
        /// 知识点字典
        /// </summary>
        public Dictionary<string, KnowledgePoint> KnowledgePointDict { get; set; }

        private ProcedureMonitor m_OperateMonitor = new ProcedureMonitor();
        /// <summary>
        /// 操作监视器
        /// </summary>
        public ProcedureMonitor OperateMonitor
        {
            get { return m_OperateMonitor; }
            set { m_OperateMonitor = value; }
        }

        /// <summary>
        /// 问答题目列表
        /// </summary>
        public List<CheckQuestion> CheckQuestionList { get; set; }

        /// <summary>
        /// 生产线操作仿真模块
        /// </summary>
        public ProductionSimulationModule standardModule;

        /// <summary>
        /// 故障模块
        /// </summary>
        public FaultSimulationModule faultModule;

        /// <summary>
        /// 关卡信息模块
        /// </summary>
        public LevelInfoModule levelModule;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            //注册事件
            EventDispatcher.RegisterEvent<Document>(Events.Item.Document.Click, ProcessDocumentData);
            EventDispatcher.RegisterEvent<DocumentResult>(Events.Item.Document.Result, HandleDocumentResult);
            //初始化字典
            KnowledgePointDict = new Dictionary<string, KnowledgePoint>();

            CheckQuestionList = new List<CheckQuestion>();

            switch (StageStyle)
            {
                case StageStyle.Standard:
                    //初始化数据
                    InitStandard();
                    break;
                case StageStyle.Fault:
                    InitFault();
                    break;
                default:
                    break;
            }
        }

        protected virtual void LoadWorkshops(EnumWorkshopType[] workshopTypes)
        {
            CoroutineManager.Instance.StartCoroutine(LoadingWorkshops(workshopTypes));
        }

        /// <summary>
        /// 加载车间
        /// </summary>
        /// <param name="workshopTypes"></param>
        /// <returns></returns>
        IEnumerator LoadingWorkshops(EnumWorkshopType[] workshopTypes)
        {
            //判断实体是否加载完成
            foreach (EnumWorkshopType workshopType in workshopTypes)
            {
                WorkshopManager.Instance.LoadWorkshop(workshopType, this);
                while (!WorkshopManager.Instance.IsLoaded(workshopType))
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            switch (StageStyle)
            {
                case StageStyle.Standard:
                    //初始化实体
                    EntityManager.Instance.Initialize(standardModule.GetEntities());
                    break;
                case StageStyle.Fault:
                    //初始化实体
                    EntityManager.Instance.Initialize(faultModule.GetEntities());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化标准模式
        /// </summary>
        protected virtual void InitStandard()
        {
            standardModule = ModuleManager.Instance.GetModule<ProductionSimulationModule>();
            levelModule = ModuleManager.Instance.GetModule<LevelInfoModule>();
            //初始化监视器
            InitMonitor();
            InitAudioSource();
            //考核模式
            if (ProductMode == ProductionMode.Examine)
            {
                InitAssessment();
            }
            //初始化知识点
            InitKnowledgePoint();
            //初始化问答题目
            InitCheckQuestion();
            //当前步骤
            EventDispatcher.ExecuteEvent(Events.Procedure.Current, 1, 1);
            //引导
            //if (this.ProductMode == ProductionMode.Study)
            //{
            //    ProductionGuideManager.Instance.Enabled = true;
            //    ProductionGuideManager.Instance.CurrentSeqId = 1;
            //    ProductionGuideManager.Instance.CurrentActionId = 1;
            //}

            switch (this.ProductMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    ProductionGuideManager.Instance.Enabled = true;
                    ProductionGuideManager.Instance.CurrentSeqId = 1;
                    ProductionGuideManager.Instance.CurrentActionId = 1;
                    break;
                case ProductionMode.Examine:
                case ProductionMode.Banditos:
                    InitAssessment();
                    break;
                default:
                    break;
            }

            PipeFittingManager.Instance.CurrentSeqId = 1;
            PipeFittingManager.Instance.CurrentActionId = 1;
        }

        /// <summary>
        /// 初始化故障模式
        /// </summary>
        protected virtual void InitFault()
        {
            faultModule = ModuleManager.Instance.GetModule<FaultSimulationModule>();
            //初始化监视器
            InitMonitor();
            //考核模式
            if (ProductMode == ProductionMode.Examine)
            {
                InitAssessment();
            }
            //当前步骤
            EventDispatcher.ExecuteEvent(Events.Procedure.Current, 1, 1);

            if (this.ProductMode == ProductionMode.Study)
            {
                ProductionGuideManager.Instance.Enabled = true;
            }
        }
        public void InitAudioSource()
        {
            ProductionAudioController.Instance.stageName = GetCurrentStage().Name;
        }
        /// <summary>
        /// 获取当前故障信息
        /// </summary>
        /// <returns></returns>
        public FaultInfo GetFaultInfo()
        {
            return faultModule.GetFaultInfo(FaultID);
        }

        public Stage GetCurrentStage()
        {
            return standardModule.Project.GetStage(StageType);
        }

        public Stage GetNextStage()
        {
            Stage current = GetCurrentStage();
            int level = Convert.ToInt32(current.ID);
            string id = (level + 1).ToString("00");
            return standardModule.Project.GetStageById(id);
        }

        /// <summary>
        /// 获取所有工段数量
        /// </summary>
        /// <returns></returns>
        public int GetAllStageCount()
        {
            return standardModule.Project.Stages.Count;
        }

        /// <summary>
        /// 保存用户关卡信息
        /// </summary>
        /// <param name="levelInfo"></param>
        /// <param name="handler"></param>
        public void UpdateLevelInfo(LevelInfo levelInfo, PackageHandler<NetworkPackageInfo> handler)
        {
            levelModule.UpdateLevelInfo(levelInfo, handler);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            EventDispatcher.UnregisterEvent<Document>(Events.Item.Document.Click, ProcessDocumentData);
            EventDispatcher.UnregisterEvent<DocumentResult>(Events.Item.Document.Result, HandleDocumentResult);
            //卸载车间
            WorkshopManager.Instance.UnloadWorkshopAll();
            //释放管理器
            FaultPointManager.Instance.Release();
            PipeFittingManager.Instance.Release();
            ProductionGuideManager.Instance.Release();
            ValveManager.Instance.Release();
            SmallActionManager.Instance.Release();
        }

        /// <summary>
        /// 初始化监视器
        /// </summary>
        public void InitMonitor()
        {
            List<Sequence> Sequences = null;
            switch (StageStyle)
            {
                case StageStyle.Standard:
                    Sequences = standardModule.GetProcedure().Sequences;
                    break;
                case StageStyle.Fault:
                    Sequences = faultModule.GetFault().Sequences;
                    break;
                default:
                    break;
            }

            foreach (Sequence seq in Sequences)
            {
                if (!seq.Monitor)
                    continue;

                //队列监视点
                SequencePoint sequencePoint = new SequencePoint();
                sequencePoint.Id = seq.ID;

                foreach (_Action _action in seq.Actions)
                {
                    if (!_action.Monitor)
                        continue;

                    ActionPoint actionPoint = new ActionPoint();
                    actionPoint.Id = _action.ID;
                    sequencePoint.Add(_action.ID, actionPoint);
                }
                OperateMonitor.Add(sequencePoint.Id, sequencePoint);
            }
        }
        public override List<CheckQuestion> GetCheckQuestionList()
        {
            return CheckQuestionList;
        }
        /// <summary>
        /// 初始化考核内容
        /// </summary>
        public void InitAssessment()
        {
            List<AssessmentPoint> points = standardModule.GetAssessmentPoints();
            switch (StageStyle)
            {
                case StageStyle.Standard:
                    points = standardModule.GetAssessmentPoints();
                    break;
                case StageStyle.Fault:
                    points = faultModule.GetAssessmentPoints();
                    break;
                default:
                    break;
            }

            foreach (var item in points)
            {
                AssessmentGrade.Add(item.Id, item);
                item.Rect = ToRect(item.Region);
            }
        }

        private void InitKnowledgePoint()
        {
            List<KnowledgePoint> points = null;
            switch (StageStyle)
            {
                case StageStyle.Standard:
                    points = standardModule.GetKnowledgePoints();
                    break;
                case StageStyle.Fault:
                    points = faultModule.GetKnowledgePoints();
                    break;
                default:
                    break;
            }

            foreach (var item in points)
            {
                KnowledgePointDict.Add(item.Id, item);
            }
        }

        /// <summary>
        /// 初始化考核问答题
        /// </summary>
        private void InitCheckQuestion()
        {
            switch (StageStyle)
            {
                case StageStyle.Standard:
                    CheckQuestionList = standardModule.GetCheckQuestions();
                    break;
                case StageStyle.Fault:

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 将字符串转换成Vector2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Rect ToRect(string value)
        {
            Rect rect = new Rect();
            rect.x = Convert.ToInt32(value.Substring(1, 1));
            rect.y = Convert.ToInt32(value.Substring(3, 1));
            rect.width = Convert.ToInt32(value.Substring(5, 1));
            rect.height = Convert.ToInt32(value.Substring(7, 1));
            return rect;
        }

        #region 考核
        /// <summary>
        /// 扣分
        /// </summary>
        /// <param name="index"></param>
        public void Deduct(int index)
        {
            //if (ProductMode == ProductionMode.Examine)
            //{
            //    AssessmentGrade.Deduct(index);
            //}
            switch (ProductMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                case ProductionMode.Banditos:
                    AssessmentGrade.Deduct(index);
                    Debug.Log("操作错误开始扣分，扣分ID = " + index);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 结算考核项
        /// </summary>
        public void Finish(int index)
        {
            //if (ProductMode == ProductionMode.Examine)
            //{
            //    AssessmentGrade.Finish(index);
            //}
            switch (ProductMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                case ProductionMode.Banditos:
                    AssessmentGrade.Finish(index);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 结算考核项
        /// </summary>
        public void UnFinish(int index)
        {
            //if (ProductMode == ProductionMode.Examine)
            //{
            //    AssessmentGrade.Finish(index);
            //}
            switch (ProductMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                case ProductionMode.Banditos:
                    AssessmentGrade.UnFinish(index);
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 加工文件数据
        /// </summary>
        /// <param name="item"></param>
        public virtual void ProcessDocumentData(Document item)
        {
            if (ProductMode== ProductionMode.Banditos && item.DocumentType==DocumentType.AssessmentReport)
            {
                DocumentSetting setting = new DocumentSetting();
                //Message msg = new Message(MessageType.OpenFileMsg, this, setting);
                //EventDispatcher.ExecuteEvent();
                setting.Type = DocumentType.AssessmentReport;
                setting.EntityNPCList = null;
                setting.Data = new object[3] { "闯关模式成绩单", AssessmentGrade, CheckQuestionList };
                setting.ButtonText = "确认";
                EventDispatcher.ExecuteEvent(Events.Item.Document.Open, setting);
            }
        }

        /// <summary>
        /// 处理文件结果
        /// </summary>
        /// <param name="result"></param>
        public virtual void HandleDocumentResult(DocumentResult result)
        {
            BaseDocument document = result.Sender.GetComponent<BaseDocument>();
            switch (document.GetDocumentType())
            {
                case DocumentType.None:
                    break;
                case DocumentType.AssessmentReport:
                    switch (ProductMode)
                    {
                        case ProductionMode.Banditos:
                            EventDispatcher.ExecuteEvent<bool>("闯关模式提交成绩单结束",true);
                            break;
                        case ProductionMode.Examine:
                            if (App.Instance.VersionTag == VersionTag.CZDX)
                            {
                                Application.Quit();
                            }
                            switch (StageStyle)
                            {

                                case StageStyle.Standard:
                                    UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                                    SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                                    break;
                                case StageStyle.Fault:
                                    UIManager.Instance.CloseUI(EnumUIType.FaultSimulationUI);
                                    SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 主动弹出文件
        /// </summary>
        /// <param name="type"></param>
        public void PopupDocument(string name)
        {
            Document item = standardModule.GetDocument(name);
            ProcessDocumentData(item);
        }

        /// <summary>
        /// 关闭文件栏
        /// </summary>
        public void CloseDocumentBar()
        {
            EventDispatcher.ExecuteEvent(Events.Item.Document.Close);
        }

        /// <summary>
        /// 获取考核
        /// </summary>
        /// <returns></returns>
        public override AssessmentGrade GetAssessmentGrade()
        {
            return AssessmentGrade;
        }
    }
}
