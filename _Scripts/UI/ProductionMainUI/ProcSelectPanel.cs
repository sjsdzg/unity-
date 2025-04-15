using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using System;
using XFramework.Simulation;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 流程选择面板
    /// </summary>
    public class ProcSelectPanel : MonoBehaviour
    {
        private UnityEvent m_OnSelected = new UnityEvent();
        /// <summary>
        /// 选中场景，进入操作
        /// </summary>
        public UnityEvent OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 
        /// </summary>
        private Button buttonEnter;

        /// <summary>
        /// 流程类型
        /// </summary>
        private TypeGroup m_TypeGroup;

        /// <summary>
        /// 流程名称
        /// </summary>
        private NameGroup m_NameGroup;

        /// <summary>
        /// 操作模式
        /// </summary>
        private ModeGroup m_ModeGroup;

        /// <summary>
        /// 当前工段
        /// </summary>
        public Stage CurrentStage { get; set; }

        private StageStyle currentStageStyle = StageStyle.Standard;

        /// <summary>
        /// 当前工段风格
        /// </summary>
        public StageStyle CurrentStageStyle
        {
            get { return currentStageStyle; }
            set { currentStageStyle = value; }
        }

        private ProcedureType currentProcedureType = ProcedureType.Operate;

        /// <summary>
        /// 当前流程类型
        /// </summary>
        public ProcedureType CurrentProcedureType
        {
            get { return currentProcedureType; }
            set { currentProcedureType = value; }
        }

        private ProductionMode currentProductionMode = ProductionMode.None;
        /// <summary>
        /// 当前生产模式
        /// </summary>
        public ProductionMode CurrentProductionMode
        {
            get { return currentProductionMode; }
            set { currentProductionMode = value; }
        }

        private string currentFaultID;
        /// <summary>
        /// 故障ID
        /// </summary>
        public string CurrentFaultID
        {
            get { return currentFaultID; }
            set { currentFaultID = value; }
        }

        private void Awake()
        {
            m_Title = transform.Find("TitleBar/Text").GetComponent<Text>();
            buttonEnter = transform.Find("Bottom/ButtonEnter").GetComponent<Button>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            m_TypeGroup = transform.Find("Panel/TypeGroup").GetComponent<TypeGroup>();
            m_NameGroup = transform.Find("Panel/NameGroup").GetComponent<NameGroup>();
            m_ModeGroup = transform.Find("Panel/ModeGroup").GetComponent<ModeGroup>();
            //Event
            m_TypeGroup.OnSelected.AddListener(m_TypeGroup_OnSelected);
            m_NameGroup.OnSelected.AddListener(m_NameGroup_OnSelected);
            m_ModeGroup.OnSelected.AddListener(m_ModeGroup_OnSelected);
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonEnter.onClick.AddListener(buttonEnter_onClick);
        }


        /// <summary>
        /// 流程名称选择时，触发
        /// </summary>
        /// <param name="stageStyle"></param>
        private void m_TypeGroup_OnSelected(StageStyle stageStyle)
        {
            m_NameGroup.Clear();
            CurrentStageStyle = stageStyle;
            switch (stageStyle)
            {
                case StageStyle.Standard:
                    for (int i = 0; i < CurrentStage.Procedures.Count; i++)
                    {
                        if (CurrentStage.Procedures[i].Name.Equals("操作流程"))
                        {
                            m_NameGroup.AddItem(CurrentStage.Procedures[i].Name, true);
                        }
                        else
                        {
                            m_NameGroup.AddItem(CurrentStage.Procedures[i].Name);
                        }
                    }
                    //m_NameGroup.AddItem("检查流程");
                    //m_NameGroup.AddItem("操作流程", true);
                    //m_NameGroup.AddItem("清场流程");
                    break;
                case StageStyle.Fault:
                    for (int i = 0; i < CurrentStage.Faults.Count; i++)
                    {
                        if (i == 0)
                        {
                            m_NameGroup.AddItem(CurrentStage.Faults[i].Name, true);
                        }
                        else
                        {
                            m_NameGroup.AddItem(CurrentStage.Faults[i].Name, false);
                        }
                    }
                    break;
                default:
                    break;
            }         
        }

        /// <summary>
        /// 流程名称选择时，触发
        /// </summary>
        /// <param name="name"></param>
        private void m_NameGroup_OnSelected(string name)
        {
            if (CurrentStageStyle == StageStyle.Standard)
            {
                switch (name)
                {
                    case "检查流程":
                        CurrentProcedureType = ProcedureType.Check;
                        break;
                    case "操作流程":
                        CurrentProcedureType = ProcedureType.Operate;
                        break;
                    case "清场流程":
                        CurrentProcedureType = ProcedureType.Clear;
                        break;
                    default:
                        break;
                }
                Procedure procedure = CurrentStage.GetProcedure(CurrentProcedureType);
                m_ModeGroup.StudyActive = procedure.Study;
                m_ModeGroup.ExamActive = procedure.Examine;
            }
            else if (CurrentStageStyle == StageStyle.Fault)
            {
                Fault fault =  CurrentStage.GetFaultByName(name);
                CurrentFaultID = fault.Id;
                m_ModeGroup.StudyActive = fault.Study;
                m_ModeGroup.ExamActive = fault.Examine;
            }

            if (m_ModeGroup.StudyActive)
            {
                m_ModeGroup.SetToggleStatus(true);
            }
        }

        /// <summary>
        /// 模式选择时，触发
        /// </summary>
        /// <param name="productionMode"></param>
        private void m_ModeGroup_OnSelected(ProductionMode productionMode)
        {
            CurrentProductionMode = productionMode;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="stage"></param>
        public void Show(Stage stage)
        {
            CurrentStage = stage;
            gameObject.SetActive(true);
            m_TypeGroup.toggleStandard.isOn = true;
            m_TypeGroup_OnSelected(StageStyle.Standard);
            m_Title.text = stage.Name;
            if (CurrentStage.Faults.Count == 0)
            {
                m_TypeGroup.FaultActive = false;
            }
            else
            {
                m_TypeGroup.FaultActive = true;
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            Hide();
        }

        private void buttonEnter_onClick()
        {
            Hide();
            OnSelected.Invoke();
        }

    }
}

