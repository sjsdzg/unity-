using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.Simulation;
using PathologicalGames;

namespace XFramework.UI
{
    /// <summary>
    /// 故障模拟UI
    /// </summary>
    public class FaultSimulationUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.FaultSimulationUI;
        }

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
        /// 知识点面板
        /// </summary>
        private KnowledgePointPanel m_KnowledgePointPanel;

        /// <summary>
        /// 标题栏
        /// </summary>
        private FaultTitleBar faultTitleBar;

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
        /// 时间栏
        /// </summary>
        private TimeBar m_TimeBar;

        /// <summary>
        /// 故障信息视图栏
        /// </summary>
        private FaultInfoViewBar m_FaultInfoViewBar;

        /// <summary>
        /// 故障名称选择栏
        /// </summary>
        private FaultNameSelectBar m_FaultNameSelectBar;

        /// <summary>
        /// 故障原因选择栏
        /// </summary>
        private FaultCauseSelectBar m_FaultCauseSelectBar;

        /// <summary>
        /// 操作帮助面板
        /// </summary>
        private ProductionHelpPanel m_ProductionHelpPanel;

        /// <summary>
        /// 故障类型按钮
        /// </summary>
        private Button buttonName;

        /// <summary>
        /// 流程
        /// </summary>
        private Fault m_Fault;

        /// <summary>
        /// 场景信息
        /// </summary>
        private ProductionSimulationScene m_Scene;

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
        private FaultSimulationModule module;

        protected override void OnAwake()
        {
            base.OnAwake();
            module = ModuleManager.Instance.GetModule<FaultSimulationModule>();
            EventDispatcher.RegisterEvent<string, string>(Events.Selector.Select, selector_OnSelect);
            EventDispatcher.RegisterEvent(Events.Selector.Deselect, selector_OnDeselect);
            EventDispatcher.RegisterEvent<string>(Events.Item.Goods.Add, OnAddGoods);
            EventDispatcher.RegisterEvent<string>(Events.Item.Goods.Remove, OnRemoveGoods);
            EventDispatcher.RegisterEvent<string, LogType>(Events.LogInfo.Show, OnShowLogInfo);
            EventDispatcher.RegisterEvent<KnowledgePoint>(Events.KnowledgePoint.Notify, OnKnowledgePointNotify);
            EventDispatcher.RegisterEvent<int, int>(Events.Procedure.Current, OnProcedureCurrent);
            EventDispatcher.RegisterEvent<DocumentSetting>(Events.Item.Document.Open, OnOpenDocument);
            EventDispatcher.RegisterEvent<GameObject, string>(Events.Entity.Speak, this.EntityEvent_Speak);
            EventDispatcher.RegisterEvent<string>(Events.Status.Init, InitStatusBar);
            EventDispatcher.RegisterEvent<string, object>(Events.Status.Update, UpdateStatusItem);
            EventDispatcher.RegisterEvent<string>(Events.Prompt.Show, promptBar_OnShow);
            EventDispatcher.RegisterEvent(Events.Prompt.Hide, promptBar_OnHide);
            EventDispatcher.RegisterEvent<string>(Events.Fault.AddPhenomena, Fault_AddPhenomena);
            EventDispatcher.RegisterEvent<string>(Events.Guide.Show, Guide_Show);
            EventDispatcher.RegisterEvent<DialogueInfo>(Events.Dialogue.Show, DialogueBar_Show);
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            m_Scene.Release();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            faultTitleBar = transform.Find("Background/TitleBar").GetComponent<FaultTitleBar>();
            m_SelectionBar = transform.Find("Background/SelectionBar").GetComponent<SelectionBar>();
            m_PromptBar = transform.Find("Background/PromptBar").GetComponent<PromptBar>();
            m_ProductionItemBar = transform.Find("Background/ProductionItemBar").GetComponent<ProductionItemBar>();
            m_LogBar = transform.Find("Background/LogBar").GetComponent<LogBar>();
            buttonBack = transform.Find("Background/MenuBar/ButtonBack").GetComponent<Button>();
            buttonLog = transform.Find("Background/MenuBar/ButtonLog").GetComponent<Button>();
            buttonTool = transform.Find("Background/MenuBar/ButtonTool").GetComponent<Button>();
            buttonLog = transform.Find("Background/MenuBar/ButtonLog").GetComponent<Button>();
            buttonStatus = transform.Find("Background/MenuBar/ButtonStatus").GetComponent<Button>();
            m_NotifyContainer = transform.Find("Background/NotifyContainer").GetComponent<NotifyContainer>();
            m_KnowledgePointPanel = transform.Find("Background/KnowledgePointPanel").GetComponent<KnowledgePointPanel>();
            m_ProcedureBar = transform.Find("Background/ProcedureBar").GetComponent<ProcedureBar>();
            m_CurrentProcedureBar = transform.Find("Background/CurrentProcedureBar").GetComponent<CurrentProcedureBar>();
            m_DocumentPanel = transform.Find("Background/DocumentViewBar").GetComponent<DocumentViewBar>();
            buttonHelp = transform.Find("Background/MenuBar/ButtonHelp").GetComponent<Button>();
            sumbitButton = transform.Find("Background/TimeBar/SumbitButton").GetComponent<Button>();
            m_StatusBar = transform.Find("Background/StatusBar").GetComponent<StatusBar>();
            m_FaultInfoViewBar = transform.Find("Background/FaultInfoViewBar").GetComponent<FaultInfoViewBar>();
            m_FaultNameSelectBar = transform.Find("Background/FaultInfoViewBar/FaultNameSelectBar").GetComponent<FaultNameSelectBar>();
            m_FaultCauseSelectBar = transform.Find("Background/FaultInfoViewBar/FaultCauseSelectBar").GetComponent<FaultCauseSelectBar>();
            buttonName = transform.Find("Background/TitleBar/Name").GetComponent<Button>();
            m_DialogueBar = transform.Find("Background/DialogueBar").GetComponent<DialogueBar>();
            m_TimeBar = transform.Find("Background/TimeBar").GetComponent<TimeBar>();
            m_ProductionHelpPanel = transform.Find("Background/ProductionHelpPanel").GetComponent<ProductionHelpPanel>();
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
            m_ProductionItemBar.ItemOnClicked.AddListener(m_ProductionItemBar_ItemOnClicked);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            buttonStatus.onClick.AddListener(buttonStatus_onClick);
            sumbitButton.onClick.AddListener(sumbitButton_onClick);
            m_FaultInfoViewBar.OnNameClicked.AddListener(m_FaultInfoViewBar_OnNameClicked);
            m_FaultInfoViewBar.OnCauseClicked.AddListener(m_FaultInfoViewBar_OnCauseClicked);
            buttonName.onClick.AddListener(buttonType_onClick);
            m_NotifyContainer.OnClicked.AddListener(m_NotifyContainer_OnClicked);
        }


        protected override void SetUI(params object[] uiParams)
        {
            base.SetUI(uiParams);
            //m_Scene = uiParams[0] as ProductionSimulationScene;
            //faultTitleBar.SetMode(m_Scene.SceneInfo.SelectMode);
            //if (m_Scene.SceneInfo.SelectMode == ProductionMode.Examine)
            //{
            //    sumbitButton.gameObject.SetActive(true);
            //}
            //else
            //{
            //    sumbitButton.gameObject.SetActive(false);
            //}

            m_Scene = uiParams[0] as ProductionSimulationScene;
            ProductionMode productionMode = m_Scene.Examining ? m_Scene.SceneExamInfo.SelectMode : m_Scene.SceneInfo.SelectMode;
            faultTitleBar.SetMode(productionMode);
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
                    break;
                default:
                    break;
            }

            if (m_Scene.Examining) //考试中
            {
                buttonBack.gameObject.SetActive(false);
                m_TimeBar.StartCutDowning(m_Scene.ExamTransmitInfo.LeftTime);//倒计时
            }
            else
            {
                buttonBack.gameObject.SetActive(true);
                m_TimeBar.StartTiming(0);//正常计时
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
                    Goods goods = component.Item as Goods;
                    EventDispatcher.ExecuteEvent<Goods>(Events.Item.Goods.Click, goods);
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
            m_KnowledgePointPanel.Hide();
            m_Fault = module.GetFault();
            m_ProcedureBar.SetValue(m_Fault.Name, m_Fault.Sequences);
            faultTitleBar.SetValue("故障模拟", m_Fault.Name);
            m_FaultNameSelectBar.Init(module.GetFaultInfoCollection().FaultInfos);
            m_FaultCauseSelectBar.Init(module.GetFaultInfoCollection().FaultInfos);
            m_FaultInfoViewBar.DefaultFaultInfo = module.GetFaultInfo(m_Fault.Id);
            m_FaultNameSelectBar.Hide();
            m_FaultCauseSelectBar.Hide();
            m_FaultInfoViewBar.Hide();
        }

        /// <summary>
        /// 物品项列表添加时，触发
        /// </summary>
        /// <param name="item"></param>
        //private void ItemList_OnAdd(Item item)
        //{
        //    m_ProductionItemBar.AddItem(item);
        //}

        /// <summary>
        /// 物品项列表移除时，触发
        /// </summary>
        /// <param name="item"></param>
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

        /// <summary>
        /// 提交按钮，点击时触发
        /// </summary>
        private void sumbitButton_onClick()
        {
            Document document = module.GetDocument(DocumentType.AssessmentReport.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Document.Click, document);
        }

        /// <summary>
        /// 故障类型按钮点击时，触发
        /// </summary>
        private void buttonType_onClick()
        {
            m_FaultInfoViewBar.Show();
            UGUIGuide.Instance.Hide();
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
        /// 显示日志信息
        /// </summary>
        /// <param name="obj"></param>
        private void OnShowLogInfo(string content, LogType type)
        {
            string condition = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), content);
            LogItemData data = new LogItemData(condition, "", type);
            m_LogBar.AddLogItem(data);
        }

        /// <summary>
        /// 当前流程内容
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnProcedureCurrent(int seqId, int actionId)
        {
            m_ProcedureBar.SetCurrentItem(seqId, actionId);
            string title = string.Format("当前流程({0}-{1})", seqId, actionId);
            string content = m_Fault.GetAction(seqId, actionId).Desc;
            m_CurrentProcedureBar.SetValue(title, content);
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
        /// <param name="sender"></param>
        /// <param name="text"></param>

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
            UIManager.Instance.CloseUI(EnumUIType.FaultSimulationUI);
            SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
        }

        /// <summary>
        /// 故障信息视图栏，故障名称面板点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_FaultInfoViewBar_OnNameClicked(FaultNamePanel panel)
        {
            m_FaultNameSelectBar.Show(panel);
        }

        /// <summary>
        /// 故障信息视图栏，故障原因面板点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_FaultInfoViewBar_OnCauseClicked(FaultCausePanel panel)
        {
            m_FaultCauseSelectBar.Show(panel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void Fault_AddPhenomena(string name)
        {
            m_FaultInfoViewBar.AddFaultPhenomena(name);
        }

        /// <summary>
        /// 引导显示
        /// </summary>
        /// <param name="obj"></param>
        private void Guide_Show(string name)
        {
            UGUIGuide.Instance.Show(buttonName.GetComponent<RectTransform>());
        }

        /// <summary>
        /// 对话框显示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        private void DialogueBar_Show(DialogueInfo dialogueInfo)
        {
            m_DialogueBar.Show(dialogueInfo);
        }

        /// <summary>
        /// 消息通知，Item点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_NotifyContainer_OnClicked(KnowledgePoint arg0)
        {
            m_KnowledgePointPanel.Show(arg0);
        }
    }
}

