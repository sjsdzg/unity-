using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Simulation;
using PathologicalGames;
using UIWidgets;
using System.Text;
using XFramework.Proto;
using Newtonsoft.Json.Linq;

namespace XFramework.UI
{
    /// <summary>
    /// 验证模拟UI
    /// </summary>
    public class ProductionSimulationUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.ProductionSimulationUI;
        }

#if ILAB_X
        private DateTime startDate;
#endif

        /// <summary>
        /// 工具按钮
        /// </summary>
        private Button buttonTool;

        /// <summary>
        /// 日志按钮
        /// </summary>
        private Button buttonLog;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 帮助按钮
        /// </summary>
        private Button buttonHelp;

        /// <summary>
        /// 状态按钮
        /// </summary>
        private Button buttonStatus;

        /// <summary>
        /// 最佳视角按钮
        /// </summary>
        private Button buttonView;

        /// <summary>
        /// 选择器栏
        /// </summary>
        private SelectionBar m_SelectionBar;

        /// <summary>
        /// 提示栏
        /// </summary>
        private PromptBar m_PromptBar;

        /// <summary>
        /// 日志栏组件
        /// </summary>
        private LogBar m_LogBar;

        /// <summary>
        /// 生产工具栏
        /// </summary>
        private ProductionItemBar m_ProductionItemBar;

        /// <summary>
        /// 通知
        /// </summary>
        private NotifyContainer m_NotifyContainer;

        /// <summary>
        /// 标题栏
        /// </summary>
        private ProductionTitleBar productionTitleBar;

        /// <summary>
        /// 流程栏目
        /// </summary>
        private ProcedureBar m_ProcedureBar;

        /// <summary>
        /// 当前流程栏目
        /// </summary>
        private CurrentProcedureBar m_CurrentProcedureBar;

        /// <summary>
        /// 文件栏
        /// </summary>
        private DocumentViewBar m_DocumentPanel;

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button sumbitButton;

        /// <summary>
        /// 状态列表
        /// </summary>
        private StatusBar m_StatusBar;

        /// <summary>
        /// 对话框栏
        /// </summary>
        private DialogueBar m_DialogueBar;

        /// <summary>
        /// 流程
        /// </summary>
        private Procedure m_Procedure;

        /// <summary>
        /// 时间栏
        /// </summary>
        private TimeBar m_TimeBar;

        /// <summary>
        /// 操作帮助面板
        /// </summary>
        private ProductionHelpPanel m_ProductionHelpPanel;

        /// <summary>
        /// 场景信息
        /// </summary>
        private ProductionSimulationScene m_Scene;

        /// <summary>
        /// 知识点面板
        /// </summary>
        private KnowledgePointPanel m_KnowledgePointPanel;

        /// <summary>
        /// 自动隐藏通知模块
        /// </summary>
        private Notify notify;

        /// <summary>
        /// 文件项列表
        /// </summary>
        private ObservableCollection<Document> DocumentList = new ObservableCollection<Document>();
        /// <summary>
        /// 物品项列表
        /// </summary>
        private ObservableCollection<Goods> GoodsList = new ObservableCollection<Goods>();
        /// <summary>
        /// 清洁项列表
        /// </summary>
        private ObservableCollection<Clean> CleanList = new ObservableCollection<Clean>();

        /// <summary>
        /// 验证操作模块
        /// </summary>
        private ProductionSimulationModule module;
        /// <summary>
        /// 跳转大步骤下拉框
        /// </summary>
        private Dropdown m_ProduceDropdown;
        private SoundBar soundBar;

        private Button buttonSound;

        private StepPromptBar stepPromptBar;

        /// <summary>
        /// 用户操作日志模块
        /// </summary>
        private UserOperationLogModule userOperationLogModule;

        private LogData m_OperationLogData = new LogData();

        protected override void OnAwake()
        {
            base.OnAwake();
            module = ModuleManager.Instance.GetModule<ProductionSimulationModule>();

            ModuleManager.Instance.Register<UserOperationLogModule>();
            userOperationLogModule = ModuleManager.Instance.GetModule<UserOperationLogModule>();

            EventDispatcher.RegisterEvent<string, string>(Events.Selector.Select, selector_OnSelect);
            EventDispatcher.RegisterEvent(Events.Selector.Deselect, selector_OnDeselect);
            EventDispatcher.RegisterEvent<string>(Events.Item.Goods.Add, OnAddGoods);
            EventDispatcher.RegisterEvent<string>(Events.Item.Goods.Remove, OnRemoveGoods);
            EventDispatcher.RegisterEvent(Events.Item.Goods.Clear, OnClearGoods);
            EventDispatcher.RegisterEvent<string, LogType>(Events.LogInfo.Show, OnShowLogInfo);
            EventDispatcher.RegisterEvent<KnowledgePoint>(Events.KnowledgePoint.Notify, OnKnowledgePointNotify);
            EventDispatcher.RegisterEvent<int, int>(Events.Procedure.Current, OnProcedureCurrent);
            EventDispatcher.RegisterEvent<DocumentSetting>(Events.Item.Document.Open, OnOpenDocument);
            EventDispatcher.RegisterEvent(Events.Item.Document.Close, OnCloseDocument);
            EventDispatcher.RegisterEvent<GameObject, string>(Events.Entity.Speak, this.EntityEvent_Speak);
            EventDispatcher.RegisterEvent<string>(Events.Status.Init, InitStatusBar);
            EventDispatcher.RegisterEvent<string, object>(Events.Status.Update, UpdateStatusItem);
            EventDispatcher.RegisterEvent<string>(Events.Prompt.Show, promptBar_OnShow);
            EventDispatcher.RegisterEvent(Events.Prompt.Hide, promptBar_OnHide);
            EventDispatcher.RegisterEvent<int>(Events.Procedure.StepPrompt, ShowStepPrompt);
            EventDispatcher.RegisterEvent<int>("选择操作流程", m_ProduceDropdown_onValueChanged);
            InitGUI();
            InitEvent();
        }



        protected override void OnRelease()
        {
            base.OnRelease();
            //向服务器更新用户日志
            UpdateUserOperationLog();

            ModuleManager.Instance.Unregister<UserOperationLogModule>();
            EventDispatcher.UnregisterEvent<int>("选择操作流程", m_ProduceDropdown_onValueChanged);
            LookPointManager.Instance.Release();
            m_Scene.Release();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            //productionTitleBar = transform.Find("Background/TitleBar").GetComponent<ProductionTitleBar>();
            m_SelectionBar = transform.Find("Background/SelectionBar").GetComponent<SelectionBar>();
            m_PromptBar = transform.Find("Background/PromptBar").GetComponent<PromptBar>();
            m_ProductionItemBar = transform.Find("Background/ProductionItemBar").GetComponent<ProductionItemBar>();
            m_LogBar = transform.Find("Background/LogBar").GetComponent<LogBar>();
            buttonBack = transform.Find("Background/TitleBar/ButtonBack").GetComponent<Button>();
            buttonLog = transform.Find("Background/MenuBar/ButtonLog").GetComponent<Button>();
            buttonTool = transform.Find("Background/MenuBar/ButtonTool").GetComponent<Button>();
            buttonLog = transform.Find("Background/MenuBar/ButtonLog").GetComponent<Button>();
            buttonStatus = transform.Find("Background/MenuBar/ButtonStatus").GetComponent<Button>();
            buttonView = transform.Find("Background/MenuBar/ButtonView").GetComponent<Button>();
            buttonSound = transform.Find("Background/MenuBar/ButtonSound").GetComponent<Button>();
            m_NotifyContainer = transform.Find("Background/NotifyContainer").GetComponent<NotifyContainer>();
            m_KnowledgePointPanel = transform.Find("Background/KnowledgePointPanel").GetComponent<KnowledgePointPanel>();
            m_ProcedureBar = transform.Find("Background/ProcedureBar").GetComponent<ProcedureBar>();
            m_CurrentProcedureBar = transform.Find("Background/CurrentProcedureBar").GetComponent<CurrentProcedureBar>();
            m_DocumentPanel = transform.Find("Background/DocumentViewBar").GetComponent<DocumentViewBar>();
            buttonHelp = transform.Find("Background/MenuBar/ButtonHelp").GetComponent<Button>();
            sumbitButton = transform.Find("Background/TimeBar/SumbitButton").GetComponent<Button>();
            m_StatusBar = transform.Find("Background/StatusBar").GetComponent<StatusBar>();
            m_TimeBar = transform.Find("Background/TimeBar").GetComponent<TimeBar>();
            notify = transform.Find("Background/Notify/NotifyTemplate").GetComponent<Notify>();
            m_ProductionHelpPanel = transform.Find("Background/ProductionHelpPanel").GetComponent<ProductionHelpPanel>();
            m_ProduceDropdown = transform.Find("Background/ProdecuDropdown").GetComponent<Dropdown>();

            soundBar = transform.Find("Background/SoundBar").GetComponent<SoundBar>();
            stepPromptBar = transform.Find("Background/CurrentStepPrompt").GetComponent<StepPromptBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            //DocumentList.OnAdd += ItemList_OnAdd;
            //DocumentList.OnRemove += ItemList_OnRemove;
            //GoodsList.OnAdd += ItemList_OnAdd;
            //GoodsList.OnRemove += ItemList_OnRemove;
            //CleanList.OnAdd += ItemList_OnAdd;
            //CleanList.OnRemove += ItemList_OnRemove;
            DocumentList.CollectionChanged += ItemList_CollectionChanged;
            GoodsList.CollectionChanged += ItemList_CollectionChanged;
            CleanList.CollectionChanged += ItemList_CollectionChanged;
            buttonLog.onClick.AddListener(buttonLog_onClick);
            buttonTool.onClick.AddListener(buttonTool_onClick);
            buttonHelp.onClick.AddListener(buttonHelp_onClick);
            buttonSound.onClick.AddListener(buttonSound_onClick);
            m_ProductionItemBar.ItemOnClicked.AddListener(m_ProductionItemBar_ItemOnClicked);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            buttonStatus.onClick.AddListener(buttonStatus_onClick);
            buttonView.onClick.AddListener(buttonView_onClick);
            sumbitButton.onClick.AddListener(sumbitButton_onClick);
            m_NotifyContainer.OnClicked.AddListener(m_NotifyContainer_OnClicked);
            m_TimeBar.OnPrompt.AddListener(m_TimeBar_OnPrompt);
            m_TimeBar.OnEnd.AddListener(m_TimeBar_OnEnd);

            soundBar.OnSliderMusicValueChanged.AddListener(SliderMusicValueChanged);
            soundBar.OnSliderEffectValueChanged.AddListener(SliderEffectValueChanged);
        }

        protected override void SetUI(params object[] uiParams)
        {
            base.SetUI(uiParams);
            m_Scene = uiParams[0] as ProductionSimulationScene;
            ProductionMode productionMode = m_Scene.Examining ? m_Scene.SceneExamInfo.SelectMode : m_Scene.SceneInfo.SelectMode;
            //productionTitleBar.SetMode(productionMode);

            m_Procedure = module.GetProcedure();
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("跳转大步骤"));
            for (int i = 0; i < m_Procedure.Sequences.Count; i++)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData(m_Procedure.Sequences[i].Desc);
                optionDatas.Add(optionData);
            }
            m_ProduceDropdown.options = optionDatas;
            m_ProduceDropdown.onValueChanged.AddListener(m_ProduceDropdown_onValueChanged);

            switch (productionMode)
            {
                case ProductionMode.None:
                    sumbitButton.gameObject.SetActive(false);
                    break;
                case ProductionMode.Study:
                    sumbitButton.gameObject.SetActive(false);
                    break;
                case ProductionMode.Examine:
                    sumbitButton.gameObject.SetActive(true);
                    m_ProduceDropdown.gameObject.SetActive(false);
                    soundBar.Hide();
                    buttonSound.gameObject.SetActive(false);
                    break;
                case ProductionMode.Banditos:
                    sumbitButton.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            if (m_Scene.Examining) //考试中
            {
                m_ProduceDropdown.gameObject.SetActive(false);
                buttonBack.gameObject.SetActive(false);
                m_TimeBar.StartCutDowning(m_Scene.ExamTransmitInfo.LeftTime);//倒计时
            }
            else
            {
                buttonBack.gameObject.SetActive(true);
                m_TimeBar.StartTiming(0);//正常计时
            }
           
        }
        private int m_CurrentStepValve = 0;
        private bool isJump = false;
        private void m_ProduceDropdown_onValueChanged(int valve)
        {
            if (valve > 0)
            {
              
                m_CurrentStepValve = valve;
                currentSeqID = valve;
                isJump = true;
                EventDispatcher.ExecuteEvent(Events.Procedure.Initialize, valve);
                //UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                //SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
            }
        }

        private void m_ProductionItemBar_ItemOnClicked(ProductionItemElement component)
        {
            ItemType type = component.Item.Type;
            switch (type)
            {
                case ItemType.Document:
                    //m_DocumentPanel.gameObject.SetActive(true);
                    //m_DocumentPanel.SetDocumentData(item, DocumentResultAction);
                    Document item = component.Item as Document;
                    EventDispatcher.ExecuteEvent<Document>(Events.Item.Document.Click, item);
                    //m_Stage.ProcessDocumentData(item);
                    break;
                case ItemType.Goods:
                    //Clean ToolItem = component.data as Clean;
                    //CleanCursor.Instance.SetCleanCursor(component.image.sprite, ToolItem);
                    break;
                //case ItemType.Clean:
                //    Clean Clean = component.data as Clean;
                //    CleanCursor.Instance.SetCleanCursor(component.image.sprite, Clean);
                //    break;
                default:
                    break;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_ProductionItemBar.Hide();
            m_LogBar.Hide();
            m_StatusBar.Hide();
            m_ProductionHelpPanel.Hide();
            m_DocumentPanel.gameObject.SetActive(false);
            m_Procedure = module.GetProcedure();
            m_KnowledgePointPanel.Hide();
            //m_ProcedureBar.SetProcedure(m_Procedure);
            m_ProcedureBar.SetValue(m_Procedure.Name, StageDefine.GetStageName(module.CurrentStageType), m_Procedure.Sequences);
            //productionTitleBar.SetValue(StageDefine.GetStageName(module.CurrentStageType), m_Procedure.Name);

            //向服务器，初始化用户操作日志
            CreateUserOperationLog();

            if (PlayerPrefs.HasKey("_bgmAudioVolume"))
                soundBar.sliderMusic.value = PlayerPrefs.GetFloat("_bgmAudioVolume");
            if (PlayerPrefs.HasKey("_guideAudioSource"))
                soundBar.sliderEffect.value = PlayerPrefs.GetFloat("_guideAudioSource");
        }

        #region Item List Change 事件
        //private void ItemList_OnAdd(Item item)
        //{
        //    m_ProductionItemBar.AddItem(item);
        //}

        //private void ItemList_OnRemove(Item item)
        //{
        //    m_ProductionItemBar.RemoveItem(item);
        //}
        private void ItemList_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        Item item = (Item)newItem;
                        m_ProductionItemBar.AddItem(item);
                    }
                    break;
                case CollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        Item item = (Item)oldItem;
                        m_ProductionItemBar.RemoveItem(item);
                    }
                    break;
                case CollectionChangedAction.Replace:
                    break;
                case CollectionChangedAction.Move:
                    break;
                case CollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 创建用户操作日志
        /// </summary>
        public void CreateUserOperationLog()
        {
            if (!NetworkManager.Instance.IsConnected)
                return;

            UserOperationLog userOperationLog = new UserOperationLog();
            userOperationLog.SoftwareId = App.Instance.SoftwareId;
            userOperationLog.UserId = GlobalManager.user.Id;
            userOperationLog.SoftwareModule = SoftwareModule.Production;
            //软件模块细节
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Scene.StageStyle.ToString() + "-");//工段风格
            sb.Append(m_Scene.StageType.ToString() + "-");//工段类型
            sb.Append(m_Scene.ProductionMode.ToString() + "-");//工段模式
            sb.Append(m_Scene.ProcedureType.ToString() + "-");//操作类型
            sb.Append(m_Scene.FaultID);//故障ID
            userOperationLog.SoftwareModuleDetail = sb.ToString();
            //操作数据
            userOperationLog.Data = m_OperationLogData.ToJson();
            //操作日志描述
            userOperationLog.Description = App.Instance.BuildBrief(m_Scene.StageType, m_Scene.ProcedureType);
            userOperationLogModule.InsertUserOperationLog(userOperationLog, ReceiveInsertUserOperationLog);

#if ILAB_X
            startDate = DateTime.Now;
#endif
        }

        private string userOperationLogId;

        /// <summary>
        /// 接受插入用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertUserOperationLog(NetworkPackageInfo packageInfo)
        {
            InsertUserOperationLogResp resp = InsertUserOperationLogResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
            {
                userOperationLogId = resp.Id;
                Debug.Log(resp.Detail);
            }
            else
            {
                Debug.LogWarning(resp.Detail);
            }
        }

        /// <summary>
        /// 更新操作日志
        /// </summary>
        public void UpdateUserOperationLog()
        {
            if (!NetworkManager.Instance.IsConnected)
                return;

            UserOperationLog userOperationLog = new UserOperationLog();
            userOperationLog.Id = userOperationLogId;
            userOperationLog.SoftwareId = App.Instance.SoftwareId;
            userOperationLog.UserId = GlobalManager.user.Id;
            userOperationLog.SoftwareModule = SoftwareModule.Production;
            //软件模块细节
            StringBuilder sb = new StringBuilder();
            sb.Append(m_Scene.StageStyle.ToString() + "-");//工段风格
            sb.Append(m_Scene.StageType.ToString() + "-");//工段类型
            sb.Append(m_Scene.ProductionMode.ToString() + "-");//工段模式
            sb.Append(m_Scene.ProcedureType.ToString() + "-");//操作类型
            sb.Append(m_Scene.FaultID);//故障ID
            userOperationLog.SoftwareModuleDetail = sb.ToString();
            //操作数据
            userOperationLog.Data = m_OperationLogData.ToJson();
            //操作日志描述
            userOperationLog.Description = App.Instance.BuildBrief(m_Scene.StageType, m_Scene.ProcedureType);
            userOperationLogModule.UpdateUserOperationLog(userOperationLog, ReceiveUpdateUserOperationLog);

#if ILAB_X
            DateTime endDate = DateTime.Now;
            JObject jObj = new JObject();
            jObj.Add(new JProperty("username", GlobalManager.openUser.OpenId));

            string projectTitle = string.Empty;
            if (App.Instance.VersionTag == VersionTag.CZDX)
            {
                projectTitle = "多西他赛原料药生产仿真实习";
            }

            jObj.Add(new JProperty("projectTitle", projectTitle));
            jObj.Add(new JProperty("childProjectTitle", StageDefine.GetStageName(m_Scene.StageType)));
            switch (m_Scene.ProductionMode)
            {
                case ProductionMode.Study:
                    //jObj.Add(new JProperty("childProjectTitle", "学习模式"));
                    jObj.Add(new JProperty("status", 1));
                    jObj.Add(new JProperty("score", 100));
                    break;
                case ProductionMode.Examine:
                    //jObj.Add(new JProperty("childProjectTitle", "考核模式"));
                    AssessmentGrade grade = StageManager.Instance.GetStage(m_Scene.StageType).GetAssessmentGrade();
                    jObj.Add(new JProperty("status", grade.IsAllFinished ? 1 : 0));
                    jObj.Add(new JProperty("score", grade.Score));
                    break;
                default:
                    break;
            }

            jObj.Add(new JProperty("startDate", DateTimeUtil.ToEpochMilli(startDate)));
            jObj.Add(new JProperty("endDate", DateTimeUtil.ToEpochMilli(endDate)));

            int timeUsed = Mathf.CeilToInt((int)(endDate - startDate).TotalMinutes);
            //最小是1分钟
            if (timeUsed == 0) timeUsed = 1;
            jObj.Add(new JProperty("timeUsed", timeUsed));

            //jObj.Add(new JProperty("issuerId", XJWTUtil.issueId));
            jObj.Add(new JProperty("attachmentId", ""));
            string text = jObj.ToString();
            Debug.Log("text: " + text);

            // 实验结果数据回传
            WebRequestOperation async = IlabUtil.LogUpload(text);
            async.OnCompleted(x =>
            {
                if (!string.IsNullOrEmpty(x.Error))
                {
                    Debug.LogError(x.Error);
                    return;
                }

                // 获取请求结果
                string json = async.GetText();
                Debug.Log("Received: " + json);
                JObject result = JObject.Parse(json);
                if (result["code"].ToString().Equals("0"))
                {
                    Debug.Log("实验结果数据回传成功");
                }
                else
                {
                    Debug.LogError("实验结果数据回传失败");
                }
            });
#endif
        }

        /// <summary>
        /// 接受更新用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUpdateUserOperationLog(NetworkPackageInfo packageInfo)
        {
            UpdateUserOperationLogResp resp = UpdateUserOperationLogResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
                Debug.Log(resp.Detail);
            else
                Debug.LogWarning(resp.Detail);
        }

        /// <summary>
        /// 提交按钮，点击时触发
        /// </summary>
        private void sumbitButton_onClick()
        {
            if (m_Scene.Examining)
            {
                BackExamSystem();
            }
            else
            {
                Document document = module.GetDocument(DocumentType.AssessmentReport.ToString());
                EventDispatcher.ExecuteEvent(Events.Item.Document.Click, document);
            }
        }
        public void ShowStepPrompt(int seqID)
        {
            string seqDesc = m_Procedure.GetSequence(seqID).Desc;
            string content = seqID + ". " + seqDesc;
            stepPromptBar.Show(content);
        }
        private void buttonSound_onClick()
        {
            soundBar.Show();
        }

        private void SliderMusicValueChanged(float volume)
        {
            ProductionAudioController.Instance.bgmAudioSource.volume = volume;
            PlayerPrefs.SetFloat("_bgmAudioVolume", volume);
        }

        private void SliderEffectValueChanged(float volume)
        {
            ProductionAudioController.Instance.guideAudioSource.volume = volume;
            ProductionAudioController.Instance.effectAudioSource.volume = volume;
            PlayerPrefs.SetFloat("_guideAudioSource", volume);
            PlayerPrefs.SetFloat("_effectAudioVolume", volume);
        }
        /// <summary>
        /// selector selected.
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="usable"></param>
        private void selector_OnSelect(string name, string message)
        {
            m_SelectionBar.OnSelected(name, message);
        }

        /// <summary>
        /// selector deselected.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void selector_OnDeselect()
        {
            m_SelectionBar.OnDeselected();
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        private void promptBar_OnShow(string message)
        {
            m_PromptBar.Show(message);
        }

        /// <summary>
        /// 隐藏提示信息
        /// </summary>
        private void promptBar_OnHide()
        {
            m_PromptBar.Hide();
        }

        /// <summary>
        /// 知识点通知
        /// </summary>
        /// <param name="obj"></param>
        private void OnKnowledgePointNotify(KnowledgePoint item)
        {
            //m_KnowledgePointNotify.Template().Show(obj);
            m_NotifyContainer.AddItem(item);
        }

        /// <summary>
        /// 日志按钮点击时，触发
        /// </summary>
        private void buttonLog_onClick()
        {
            m_LogBar.Show();
        }

        /// <summary>
        /// 工具按钮点击时，触发
        /// </summary>
        private void buttonTool_onClick()
        {
            m_ProductionItemBar.Show();
        }

        /// <summary>
        /// 状态按钮点击时，触发
        /// </summary>
        private void buttonStatus_onClick()
        {
            m_StatusBar.Show();
        }

        private void buttonView_onClick()
        {
            LookPointManager.Instance.isEnterAngle = true;
            LookPointManager.Instance.EnterCurrent();
           
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="itemName"></param>
        private void OnAddGoods(string itemName)
        {
            Goods item = module.GetGoods(itemName);
            GoodsList.Add(item);
        }

        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="itemName"></param>
        private void OnRemoveGoods(string itemName)
        {
            Goods item = module.GetGoods(itemName);
            GoodsList.Remove(item);
        }
        /// <summary>
        /// 清空物品
        /// </summary>
        /// <param name="itemName"></param>
        private void OnClearGoods()
        {
            // GoodsList.Clear();
            int count = GoodsList.Count;
            for (int i = 0; i < count; i++)
            {
                GoodsList.Remove(GoodsList[0]);
            }
        }

        /// <summary>
        /// 显示日志信息
        /// </summary>
        /// <param name="obj"></param>
        private void OnShowLogInfo(string content, LogType type)
        {
            string condition = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), content);
            LogItemData data = new LogItemData(condition, "", type);
            m_OperationLogData.Add(data);
            m_LogBar.AddLogItem(data);
        }
        int currentSeqID = 0;
        bool isFirstTime = true;
        bool isStopJump = false;
        /// <summary>
        /// 当前流程内容
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnProcedureCurrent(int seqId, int actionId)
        {
            m_ProcedureBar.SetCurrentItem(seqId, actionId);
            string title = string.Format("当前流程({0}.{1})", seqId, actionId);
            string content = m_Procedure.GetAction(seqId, actionId).Desc;
            m_CurrentProcedureBar.SetValue(title, content);

            if (isStopJump) return;


            if (isJump)
            {
                isJump = false;
                isStopJump = true;
                CoroutineManager.Instance.Invoke(0.6f, () =>
                {
                    ShowStepPrompt(m_CurrentStepValve);
                    isStopJump = false;
                });
                return;
            }

            if (isFirstTime)
            {
                isFirstTime = false;
            }
          
            else if (currentSeqID != seqId)
            {
                ShowStepPrompt(seqId);
            }
            currentSeqID = seqId;
        }

        /// <summary>
        /// 接受打开文件消息
        /// </summary>
        /// <param name="message"></param>
        private void OnOpenDocument(DocumentSetting setting)
        {
            if (setting.Document != null)
            {
                setting.Document = module.GetDocument(setting.Document.Name.ToString());
            }
            else
            {
                setting.Document = module.GetDocument(setting.Type.ToString());
            }
            setting.Action = OnDocumentResult;
            m_DocumentPanel.SetDocumentData(setting);
        }

        /// <summary>
        /// 接受关闭文档消息
        /// </summary>
        private void OnCloseDocument()
        {
            m_DocumentPanel.Close();
        }

        private void OnDocumentResult(DocumentResult result)
        {
            EventDispatcher.ExecuteEvent(Events.Item.Document.Result, result);
        }

        /// <summary>
        /// 初始状态栏
        /// </summary>
        /// <param name="path"></param>
        private void InitStatusBar(string path)
        {
            m_StatusBar.Show();
            m_StatusBar.Init(path);
        }

        private void UpdateStatusItem(string name, object state)
        {
            m_StatusBar.UpdateStatusItem(name, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>

        private void EntityEvent_Speak(GameObject sender, string text)
        {
            HUDTextInfo info = new HUDTextInfo(sender.transform, text);
            info.Color = Color.white;
            info.VerticalPositionOffset = 0.15f;
            info.Side = bl_Guidance.Right;
            info.Size = 20;
            info.Speed = 1;
            info.FadeSpeed = 250;
            info.ExtraDelayTime = 1;
            info.AnimationType = bl_HUDText.TextAnimationType.SmallToNormal;
            info.FixedFontSize = true;
            SpawnPool pool = PathologicalGames.PoolManager.Pools["HUDText"];
            info.TextPrefab = pool.prefabs["BubbleText"].gameObject;
            bl_UHTUtils.GetHUDText.NewText(info);
        }

        /// <summary>
        /// 帮助按钮点击时，触发
        /// </summary>
        private void buttonHelp_onClick()
        {
            //UIManager.Instance.OpenUI(EnumUIType.ValidationHelpUI);
            m_ProductionHelpPanel.Show();
        }

        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            if (App.Instance.VersionTag == VersionTag.CZDX)
            {
                Application.Quit();
            }
            else
            {
                UIManager.Instance.CloseUI(EnumUIType.ProductionSimulationUI);
                SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
            }           
        }

        /// <summary>
        /// 消息通知，Item点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_NotifyContainer_OnClicked(KnowledgePoint arg0)
        {
            m_KnowledgePointPanel.Show(arg0);
        }

        /// <summary>
        /// 返回考试系统
        /// </summary>
        private void BackExamSystem()
        {
            BaseStage stage = StageManager.Instance.GetStage(module.CurrentStageType);
            AssessmentGrade assessmentGrade = stage.GetAssessmentGrade();
            List<CheckQuestion> m_CheckQuestion = stage.GetCheckQuestionList();
            List<OperationPointJson> PointJsons = new List<OperationPointJson>();
            int tempID = 0;
            foreach (var item in assessmentGrade)
            {
                AssessmentPoint point = item.Value;
                OperationPointJson pointJson = new OperationPointJson();
                pointJson.Id = point.Id;
                pointJson.Desc = point.Desc;
                pointJson.Value = point.Value;
                pointJson.Score = point.Score;
                PointJsons.Add(pointJson);
                tempID++;
            }
            if (m_CheckQuestion!=null)
            {
                foreach (var item in m_CheckQuestion)
                {
                    CheckQuestion question = item;
                    OperationPointJson pointJson = new OperationPointJson();
                    pointJson.Id = int.Parse(question.Name)+tempID;
                    pointJson.Desc = question.Content;
                    pointJson.Value = question.Value;
                    pointJson.Score = question.Score;
                    PointJsons.Add(pointJson);
                }
            }          
            m_Scene.SceneExamInfo.ExamTransmitInfo.LeftTime = m_TimeBar.TotalSeconds;
            m_Scene.SceneExamInfo.ExamTransmitInfo.OperationPointJsons = PointJsons;
            ExamSystemSceneInfo sceneInfo = new ExamSystemSceneInfo();
            sceneInfo.ExamTransmitInfo = m_Scene.SceneExamInfo.ExamTransmitInfo;
            //返回考试场景
            SceneLoader.Instance.LoadSceneAsync(SceneType.ExamSystemScene, sceneInfo);
        }

        /// <summary>
        /// 倒计时到达提示时间时，触发事件
        /// </summary>
        private void m_TimeBar_OnPrompt()
        {
            notify.Template().Show("注意还有5分钟！\n时间结束后，如果您没有交卷，试卷将自动提交。", 15f);
        }

        /// <summary>
        /// 倒计时结束时，触发事件
        /// </summary>
        private void m_TimeBar_OnEnd()
        {
            BackExamSystem();
        }

    }

}

