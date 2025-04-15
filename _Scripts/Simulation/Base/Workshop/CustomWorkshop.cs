using UnityEngine;
using System.Collections;
using XFramework.Core;
using XFramework.Core;
using System;
using XFramework.Common;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;

namespace XFramework.Simulation
{
    /// <summary>
    /// 通用车间（处理通用部分）
    /// </summary>
    public class CustomWorkshop : BaseWorkshop
    {
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.None;
        }

        /// <summary>
        /// root dir
        /// </summary>
        public string RootDir { get; private set; }

        private int currentSeqId = 1;
        /// <summary>
        /// CurrentSeqId
        /// </summary>
        public int CurrentSeqId
        {
            get { return currentSeqId; }
            set { currentSeqId = value; }
        }

        private int currentActId = 1;
        /// <summary>
        /// CurrentActId
        /// </summary>
        public int CurrentActId
        {
            get { return currentActId; }
            set { currentActId = value; }
        }

        /// <summary>
        /// 通用工段
        /// </summary>
        [HideInInspector]
        public CustomStage customStage;


        protected override void OnAwake()
        {
            base.OnAwake();
            RootDir = "Simulation/";
            EventDispatcher.RegisterEvent<Transform, string, Color>(Events.HUDText.Show, ShowHUDTextHandler);
            EventDispatcher.RegisterEvent<int, int>(Events.Procedure.Completed, Completed);
            EventDispatcher.RegisterEvent<string>(Events.Interactable.Misoperation, MisoperationHandler);
            EventDispatcher.RegisterEvent<bool>("闯关模式提交成绩单结束", ReceiveBanditosEndEvent);
            EventDispatcher.RegisterEvent<int>(Events.Procedure.Initialize, ResetInitialize);
            EventDispatcher.RegisterEvent("ProductionSimulationFaderComplete", ProductionSimulationFaderComplete);
        }


        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            EventDispatcher.UnregisterEvent<Transform, string, Color>(Events.HUDText.Show, ShowHUDTextHandler);
            EventDispatcher.UnregisterEvent<int, int>(Events.Procedure.Completed, Completed);
            EventDispatcher.UnregisterEvent<string>(Events.Interactable.Misoperation, MisoperationHandler);
            EventDispatcher.UnregisterEvent<bool>("闯关模式提交成绩单结束", ReceiveBanditosEndEvent);
            EventDispatcher.UnregisterEvent<int>(Events.Procedure.Initialize, ResetInitialize);
            EventDispatcher.UnregisterEvent("ProductionSimulationFaderComplete", ProductionSimulationFaderComplete);
            ObservationPointManager.Instance.Release();
        }

        protected override void SetWorkshop(params object[] workshopParams)
        {
            base.SetWorkshop(workshopParams);
            customStage = workshopParams[0] as CustomStage;
            switch (customStage.StageStyle)
            {
                case StageStyle.Standard:
                    InitStandard();
                    break;
                case StageStyle.Fault:
                    InitFault();
                    break;
                default:
                    break;
            }
            FlashManager.Instance.isStudy = (customStage.ProductMode == ProductionMode.Study);
        }

        /// <summary>
        /// 初始化标准模式
        /// </summary>
        protected virtual void InitStandard()
        {

        }

        /// <summary>
        /// 初始化故障模式
        /// </summary>
        protected virtual void InitFault()
        {

        }
        protected virtual void ResetProduceInitialize(int index)
        {
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Clear);
        }
        public void ResetInitialize(int currentProduceIndex)
        {
            ResetInitStandard(currentProduceIndex);
        }
        public void ResetInitStandard(int currentProduceIndex)
        {         
            SmallActionManager.Instance.SmallActionDict.Clear();
            FlashManager.Instance.ClearAllFlash();
            //                  
            CoroutineManager.Instance.StopAllCoroutines();
            TransparentHelper.RestoreBackAll();
            //DOTween.KillAll();
            CoroutineManager.Instance.Invoke(0.5f, () =>
            {
                DOTween.KillAll();
            });
            CoroutineManager.Instance.Invoke(0.7f, () =>
            {
                ProductionGuideManager.Instance.ShowGuide(currentProduceIndex + "-1-1");
            });
            //当前步骤
            EventDispatcher.ExecuteEvent(Events.Procedure.Current, 1, 1);
            EventDispatcher.ExecuteEvent(Events.Procedure.InitActionState);         
            ///往后跳转步骤
            if (currentProduceIndex > CurrentSeqId)
            {
                int tempCurrentSeqId = CurrentSeqId;
                for (int i = CurrentSeqId; i <= currentProduceIndex; i++)
                {
                    //初始化当前步骤
                    ResetProduceInitialize(i);
                }
                for (int i = 1; i < currentProduceIndex; i++)
                {
                    for (int j = 1; j <= customStage.OperateMonitor[i].ActionPoints.Count; j++)
                    {                       
                        //前面步骤默认完成
                        Completed(i, j);
                    }
                }
                //for (int i = tempCurrentSeqId; i < currentProduceIndex; i++)
                //{
                //    for (int j = 1; j <= customStage.OperateMonitor[i].ActionPoints.Count; j++)
                //    {
                //        ///未结算考核
                //        UnFinish(i, j);
                //    }
                //}                                
            }
            ///往前跳转步骤
            else if (currentProduceIndex < CurrentSeqId)
            {
                int tempCurrentSeqId = CurrentSeqId;
                for (int i = CurrentSeqId; i >= currentProduceIndex; i--)
                {
                    //初始化当前步骤
                    ResetProduceInitialize(i);
                }
                for (int i = 1; i < currentProduceIndex; i++)
                {
                    for (int j = 1; j <= customStage.OperateMonitor[i].ActionPoints.Count; j++)
                    {
                        Completed(i, j);
                    }
                }
                //后面步骤默认未完成
                for (int i = CurrentSeqId; i >= currentProduceIndex; i--)
                {
                    for (int j = customStage.OperateMonitor[i].ActionPoints.Count; j>=1; j--)
                    {
                        customStage.OperateMonitor[i, j].Completed = false;
                       // Debug.Log("大步骤 = "+i+"小步骤 = "+j+"     " + customStage.OperateMonitor[i, j].Completed);
                    }
                }            
                //for (int i = 1; i <= tempCurrentSeqId; i++)
                //{
                //    for (int j = 1; j <= customStage.OperateMonitor[i].ActionPoints.Count; j++)
                //    {
                //        ///未结算考核
                //        UnFinish(i, j);
                //    }
                //}
                if (currentProduceIndex == 1)
                {
                    currentSeqId = 1;
                    currentActId = 1;
                }
            }
            else ///跳转当前步骤步骤
            {             
                //初始化当前步骤
                ResetProduceInitialize(CurrentSeqId);
                for (int i = customStage.OperateMonitor[currentProduceIndex].ActionPoints.Count; i >=1; i--)
                {
                    customStage.OperateMonitor[currentProduceIndex, i].Completed = false;
                    ///未结算考核
                  //  UnFinish(currentProduceIndex, i);
                }
            }          
            EventDispatcher.ExecuteEvent(Events.Procedure.Current, currentProduceIndex, 1);

            ProductionGuideManager.Instance.CurrentSeqId = currentProduceIndex;
            ProductionGuideManager.Instance.CurrentActionId = 1;

            PipeFittingManager.Instance.CurrentSeqId = currentProduceIndex;
            PipeFittingManager.Instance.CurrentActionId = 1;
            //Debug.LogFormat("当前大步骤 = {0}，当前小步骤 = {1}", currentSeqId,currentActId);
        }
        /// <summary>
        /// 显示HUDText
        /// </summary>
        /// <param name="content"></param>
        /// <param name="color"></param>
        private void ShowHUDTextHandler(Transform trans, string content, Color color)
        {
            ShowHUDText(trans, content, color);
        }

        /// <summary>
        /// 显示提示文本
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="text"></param>
        protected void ShowHUDText(Transform trans, string text, Color color, float Offset = 0f)
        {
            if (color == Color.red)
            {
                ShowLogInfo(text, LogType.Error);
                HUDTextInfo info = new HUDTextInfo(trans, text);
                info.Color = color;
                info.Size = 24;
                info.Speed = UnityEngine.Random.Range(10, 20);
                info.VerticalAceleration = 1;
                info.VerticalPositionOffset = Offset;
                info.VerticalFactorScale = UnityEngine.Random.Range(1.2f, 3);
                info.Side = bl_Guidance.Right;
                info.FadeSpeed = 250;
                info.ExtraDelayTime = 2;
                info.FixedFontSize = true;
                info.AnimationType = bl_HUDText.TextAnimationType.HorizontalSmall;
                //Send the information
                bl_UHTUtils.GetHUDText.NewText(info);
            }
            else
            {
                ShowLogInfo(text, LogType.Log);
            }

            //HUDTextInfo info = new HUDTextInfo(trans, text);
            //info.Color = color;
            //info.Size = 24;
            //info.Speed = UnityEngine.Random.Range(10, 20);
            //info.VerticalAceleration = 1;
            //info.VerticalPositionOffset = Offset;
            //info.VerticalFactorScale = UnityEngine.Random.Range(1.2f, 3);
            //info.Side = bl_Guidance.Right;
            //info.FadeSpeed = 250;
            //info.ExtraDelayTime = 2;
            //info.FixedFontSize = true;
            //info.AnimationType = bl_HUDText.TextAnimationType.HorizontalSmall;
            ////Send the information
            //bl_UHTUtils.GetHUDText.NewText(info);
        }

        /// <summary>
        /// 误操作，处理
        /// </summary>
        /// <param name="name"></param>
        protected virtual void MisoperationHandler(string name)
        {
            //if (customStage.ProductMode == ProductionMode.Examine)
            //{
            //    int index = GetCurrentAssessmentId();
            //    if (index > 0)
            //    {
            //        customStage.Deduct(index);
            //    }
            //}
            switch (customStage.ProductMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                case ProductionMode.Banditos:
                    int index = GetCurrentAssessmentId();
                    if (index > 0)
                    {
                        customStage.Deduct(index);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取当前考核Id
        /// </summary>
        /// <returns></returns>
        private int GetCurrentAssessmentId()
        {
            int id = 0;
            foreach (var item in customStage.AssessmentGrade)
            {
                AssessmentPoint point = item.Value;
                if (CurrentSeqId >= point.Rect.x && CurrentSeqId <= point.Rect.width)
                {
                    if (CurrentActId >= point.Rect.y && CurrentActId <= point.Rect.height)
                    {
                        id = point.Id;
                        break;
                    }
                }
            }
            //返回Id
            return id;
        }
        /// <summary>
        /// 获取屏幕中心点的位置  返回一个临时物体
        /// </summary>
        /// <returns></returns>
        protected GameObject GetCenterObj()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, 0));
            UnityEngine.GameObject objCenter = new UnityEngine.GameObject();
            objCenter.transform.position = ray.GetPoint(5);
            objCenter.AddComponent<MeshCollider>();
            Destroy(objCenter, 5);
            return objCenter;
        }
        /// <summary>
        /// 发送日志消息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type"></param>
        public void ShowLogInfo(string content, LogType type)
        {
            EventDispatcher.ExecuteEvent(Events.LogInfo.Show, content, type);
        }
        
        private void UnFinish(int seqId, int actId)
        {
            //未完成考核
            if (customStage.ProductMode == ProductionMode.Examine || customStage.ProductMode == ProductionMode.Banditos)
            {
                foreach (var item in customStage.AssessmentGrade)
                {
                    AssessmentPoint point = item.Value;
                    if (seqId == point.Rect.width)
                    {
                        if (actId == point.Rect.height)
                        {
                            customStage.UnFinish(point.Id);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 完成其中一步流程
        /// </summary>
        /// <param name="seqId"></param>
        /// <param name="actId"></param>
        public virtual void Completed(int seqId, int actId)
        {
            Debug.LogFormat("完成流程{0}-{1}", seqId, actId);
            //完成考核
            if (customStage.ProductMode == ProductionMode.Examine || customStage.ProductMode == ProductionMode.Banditos)
            {
                foreach (var item in customStage.AssessmentGrade)
                {
                    AssessmentPoint point = item.Value;
                    if (seqId == point.Rect.width)
                    {
                        if (actId == point.Rect.height)
                        {
                            customStage.Finish(point.Id);
                        }
                    }
                }
            }
            customStage.OperateMonitor[seqId, actId].Completed = true;
            if (actId < customStage.OperateMonitor[seqId].ActionPoints.Count)
            {
                CurrentSeqId = seqId;
                CurrentActId = actId + 1;
            }
            else
            {
                if (seqId < customStage.OperateMonitor.MonitorCount)
                {
                    CurrentSeqId = seqId + 1;
                    CurrentActId = 1;
                    ShowCheckQuestion(seqId);
                }
                else
                {
                    CurrentSeqId = seqId;
                    CurrentActId = actId;
                    //流程结束
                    ShowCheckQuestion(seqId, true);
                    //isCompletedAll = true;
                    //OnCompletedAll();
                }
            }
            //当前步骤
            EventDispatcher.ExecuteEvent(Events.Procedure.Current, CurrentSeqId, CurrentActId);
            //当前引导
            ProductionGuideManager.Instance.CurrentSeqId = CurrentSeqId;
            ProductionGuideManager.Instance.CurrentActionId = CurrentActId;
            //当前管件
            PipeFittingManager.Instance.CurrentSeqId = CurrentSeqId;
            PipeFittingManager.Instance.CurrentActionId = CurrentActId;
            //当前知识点 针对没有Number编号的知识点
            NotifyContainer.Instance.Clear();
            ShowCurrentKnowledgePoint();
            //ShowCheckQuestion();
        }

        public void ShowCurrentKnowledgePoint(int number = -1)
        {
            string name = string.Empty;
            if (number == - 1)
            {
                name = string.Format("{0}-{1}", CurrentSeqId, CurrentActId);
            }
            else
            {
                name = string.Format("{0}-{1}-{2}", CurrentSeqId, CurrentActId, number);
            }
            KnowledgePoint knowledgePoint = null;
            if (customStage.KnowledgePointDict.TryGetValue(name , out knowledgePoint))
            {
                EventDispatcher.ExecuteEvent(Events.KnowledgePoint.Notify, knowledgePoint);
            }
        }

        public void ShowCheckQuestion(string questionName,bool isCompletedAll = false)
        {
            CheckQuestion question = customStage.CheckQuestionList.Find(x => x.Name.Equals(questionName));
            if (question != null)
            {
                if (isCompletedAll)
                {
                    MessageBoxEx.Show("", "考核题目", MessageBoxExEnum.CheckQuestionDialog, x =>
                    {
                        OnCompletedAll();
                    }, question);
                }
                else
                {
                    MessageBoxEx.Show("", "考核题目", MessageBoxExEnum.CheckQuestionDialog, null, question);
                }
            }
            else
            {
                if (isCompletedAll)
                {
                    OnCompletedAll();
                }
            }
        }

        /// <summary>
        /// 显示考核题目
        /// </summary>
        public void ShowCheckQuestion(int index0, bool isCompletedAll = false)
        {
            string name = index0.ToString();
            CheckQuestion question = customStage.CheckQuestionList.Find(x => x.Name.Equals(name));
            if (question != null)
            {
                if (isCompletedAll)
                {
                    MessageBoxEx.Show("", "考核题目", MessageBoxExEnum.CheckQuestionDialog, x =>
                    {
                        OnCompletedAll();
                    }, question);
                }
                else
                {
                    MessageBoxEx.Show("", "考核题目", MessageBoxExEnum.CheckQuestionDialog, null, question);
                }
            }
            else
            {
                if (isCompletedAll)
                {
                    OnCompletedAll();
                }
            }
        }

        /// <summary>
        /// 整个工段完成
        /// </summary>
        protected virtual void OnCompletedAll()
        {
            if (customStage.ProductMode == ProductionMode.Study)
            {
                MessageBoxEx.Show("恭喜您，完成了本次学习！", "提示", MessageBoxExEnum.SingleDialog, x =>
                {
                    if (App.Instance.VersionTag==VersionTag.CZDX)
                    {
                        Application.Quit();
                    }
                    switch (customStage.StageStyle)
                    {
                        case StageStyle.Standard:
                            UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
                            //Application.Quit();
                            break;
                        case StageStyle.Fault:
                            UIManager.Instance.CloseUI(EnumUIType.FaultSimulationUI);
                            SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                            break;
                        default:
                            break;
                    }
                });
            }

            if (customStage.StageStyle == StageStyle.Standard && customStage.ProductMode == ProductionMode.Banditos)
            {
                int count = customStage.GetAllStageCount();
                Stage current = customStage.GetCurrentStage();
                int currentLevel = Convert.ToInt32(current.ID);
                if (currentLevel >= count)
                {
                    MessageBoxEx.Show("恭喜您完成了所有关卡！", "提示", MessageBoxExEnum.SingleDialog, result =>
                    {
                        UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                        SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                    });
                }
                else
                {
                    if (currentLevel >= GlobalManager.levelInfo.Level)
                    {
                        isFirst = true;
                        GlobalManager.levelInfo.Level = currentLevel + 1;
                        customStage.UpdateLevelInfo(GlobalManager.levelInfo, ReceiveSaveLevelInfoResp);
                    }
                    else
                    {
                        MessageBoxEx.Show("恭喜您完成了本次关卡！", "提示", MessageBoxExEnum.SingleDialog, result =>
                        {
                            isFirst = false;
                            Document document = customStage.standardModule.GetDocument(DocumentType.AssessmentReport.ToString());
                            EventDispatcher.ExecuteEvent(Events.Item.Document.Click, document);
                        });
                    }
                }
            }
        }

        private Stage current;

        private int level;
        private Stage nextStage;
        private bool isFirst = true;
        /// <summary>
        /// 接受保存用户信息的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveSaveLevelInfoResp(NetworkPackageInfo packageInfo)
        {
             current = customStage.GetCurrentStage();
             level = Convert.ToInt32(current.ID);
             nextStage = customStage.GetNextStage();
            Document document = customStage.standardModule.GetDocument(DocumentType.AssessmentReport.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Document.Click, document);

        }
        private void ProductionSimulationFaderComplete()
        {
            if (GlobalManager.DefaultMode == ProductionMode.Examine)
                return;
            ProductionAudioController.Instance.PlayBGM();
            EventDispatcher.ExecuteEvent(Events.Procedure.StepPrompt, 1);
            string nodeName = ProductionGuideManager.Instance.m_Collection.GuideNodes[0].Name;
            ProductionAudioController.Instance.Play(nodeName);
        }
        private void ReceiveBanditosEndEvent(bool _isFirst)
        {
            if (!isFirst)
            {
                UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                return;
            }
            MessageBoxEx.Show("下一关：" + nextStage.Name + "！", "恭喜你闯过第" + level + "关", MessageBoxExEnum.GameOverDialog, result =>
                {
                    bool flag = (bool)result.Content;
                    if (flag)
                    {
                        UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                        ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(nextStage.Type, ProductionMode.Banditos, ProcedureType.Operate);
                        SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionSimulationScene, sceneInfo);
                    }
                    else
                    {
                        UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                        SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
                    }
                }
                , "开始下一关", "退出");
        }
    }
}

