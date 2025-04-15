using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.UI;
using XFramework.Module;
using System.Collections.Generic;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// ProductionMainWebGLUI
    /// </summary>
    public class ProductionMainLiteUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.ProductionMainLiteUI;
        }

        /// <summary>
        /// SectionHomePanel
        /// </summary>
        private SectionHomePanel m_SectionHomePanel;

        /// <summary>
        /// SectionDetailedPanel
        /// </summary>
        private SectionDetailedPanel m_SectionDetailedPanel;


        /// <summary>
        /// 工艺选择面板
        /// </summary>
        private ProcSelectPanel m_ProcSelectPanel;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 氢化
        /// </summary>
        private Button buttonStage_1;

        /// <summary>
        /// 溶解脱色
        /// </summary>
        private Button buttonStage_2;

        /// <summary>
        /// 干燥
        /// </summary>
        private Button buttonStage_3;

        private Transform m_HorizontalBar;

        /// <summary>
        /// SectionContent列表
        /// </summary>
        private List<Stage> m_Stages = new List<Stage>();

        /// <summary>
        /// SectionMainModule
        /// </summary>
        private ProductionMainModule module;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<ProductionMainModule>();
            module = ModuleManager.Instance.GetModule<ProductionMainModule>();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ProductionMainModule>();
        }

        private void InitGUI()
        {
            buttonBack = transform.Find("Background/Header/ButtonBack").GetComponent<Button>();
            m_SectionHomePanel = transform.Find("Background/MiddleBar/HomePanel").GetComponent<SectionHomePanel>();
            m_SectionDetailedPanel = transform.Find("Background/MiddleBar/DetailedPanel").GetComponent<SectionDetailedPanel>();
            m_ProcSelectPanel = transform.Find("Background/ProcSelectPanel").GetComponent<ProcSelectPanel>();

            m_HorizontalBar = transform.Find("Background/HorizontalBar");
            buttonStage_1 = transform.Find("Background/HorizontalBar/HorizontalScrollSnap/Content/Page_1/ButtonStage_1").GetComponent<Button>();
            buttonStage_2 = transform.Find("Background/HorizontalBar/HorizontalScrollSnap/Content/Page_2/ButtonStage_2").GetComponent<Button>();
            buttonStage_3 = transform.Find("Background/HorizontalBar/HorizontalScrollSnap/Content/Page_2/ButtonStage_3").GetComponent<Button>();
        }

        private void InitEvent()
        {
            buttonBack.onClick.AddListener(buttonBack_onClick);
            m_SectionHomePanel.OnClicked.AddListener(HomePanel_OnClicked);
            m_SectionDetailedPanel.ItemOnClicked.AddListener(DetailedPanel_ItemOnClicked);
            m_SectionDetailedPanel.OnBack.AddListener(DetailedPanel_OnBack);
            m_ProcSelectPanel.OnSelected.AddListener(m_ProcSelectPanel_OnSelected);

            buttonStage_1.onClick.AddListener(buttonStage_1_onClick);
            buttonStage_2.onClick.AddListener(buttonStage_2_onClick);
            buttonStage_3.onClick.AddListener(buttonStage_3_onClick);
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_SectionDetailedPanel.Hide();
            m_ProcSelectPanel.Hide();

            if (App.Instance.VersionTag == VersionTag.Default || App.Instance.VersionTag == VersionTag.SNTCM)
            {
                m_SectionDetailedPanel.SetButtonBackActive(false);
                m_HorizontalBar.gameObject.SetActive(false);
                ShowSectionDetailedPanel();
            }
            else if (App.Instance.VersionTag == VersionTag.CZDX)
            {
                m_SectionHomePanel.Show();
                m_HorizontalBar.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        /// <summary>
        /// Home Panel 点击触发
        /// </summary>
        /// <param name="arg0"></param>
        private void HomePanel_OnClicked(string arg0)
        {
            m_SectionHomePanel.Hide();
            if (m_Stages == null)
                m_Stages = new List<Stage>();
            else
                m_Stages.Clear();

            if (arg0.Equals("一    层"))
            {
                //m_Stages.Add(module.Project.GetStage(StageType.LiquorRoom));
                //m_Stages.Add(module.Project.GetStage(StageType.Semi_finishedProductsConfiguration));
                //m_Stages.Add(module.Project.GetStage(StageType.CleanBottle));
                //m_Stages.Add(module.Project.GetStage(StageType.FillingLyophilizer));
                //m_Stages.Add(module.Project.GetStage(StageType.CleaningSterilizationStage));
                //m_Stages.Add(module.Project.GetStage(StageType.InterCapper));
            }
            else if (arg0.Equals("二    层"))
            {
                //m_Stages.Add(module.Project.GetStage(StageType.NewFermentation));
                ////m_Stages.Add(module.Project.GetStage(StageType.CentrifugeClear));
                //m_Stages.Add(module.Project.GetStage(StageType.WZYK_CentrifugalStage));
                //m_Stages.Add(module.Project.GetStage(StageType.UltrafilterSterilizeStage));
                ////m_Stages.Add(module.Project.GetStage(StageType.WZYK_ChromatPurifyStage));
                //m_Stages.Add(module.Project.GetStage(StageType.UltrafilterConcentrateStage));
            }

            m_SectionDetailedPanel.Show(m_Stages);
        }

        public void ShowSectionDetailedPanel()
        {
            m_SectionHomePanel.Hide();
            if (m_Stages == null)
                m_Stages = new List<Stage>();
            else
                m_Stages.Clear();


            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                case VersionTag.SNTCM:
                    m_Stages.Add(module.Project.GetStage(StageType.CentrifugeStage));

                    break;
                default:
                    break;
            }

            m_SectionDetailedPanel.Show(m_Stages);
        }


        /// <summary>
        /// DetailedPanel点击返回按钮时，触发
        /// </summary>
        private void DetailedPanel_OnBack()
        {
            m_SectionDetailedPanel.Hide();
            m_SectionHomePanel.Show();
        }

        /// <summary>
        /// 详细面板Item点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void DetailedPanel_ItemOnClicked(SectionItemComponent arg0)
        {
            m_ProcSelectPanel.Show(arg0.data);
        }

        /// <summary>
        /// 氢化
        /// </summary>
        private void buttonStage_1_onClick()
        {
            //Stage stage = module.Project.GetStage(StageType.HydridingStage);
            //m_ProcSelectPanel.Show(stage);
        }

        /// <summary>
        /// 溶解脱色
        /// </summary>
        private void buttonStage_2_onClick()
        {
            //Stage stage = module.Project.GetStage(StageType.DecolorationStage);
            //m_ProcSelectPanel.Show(stage);
        }

        /// <summary>
        /// 干燥
        /// </summary>
        private void buttonStage_3_onClick()
        {
            //Stage stage = module.Project.GetStage(StageType.DryingStage);
            //m_ProcSelectPanel.Show(stage);
        }

        /// <summary>
        /// 进入到操作场景
        /// </summary>
        private void m_ProcSelectPanel_OnSelected()
        {
            switch (m_ProcSelectPanel.CurrentStageStyle)
            {
                case StageStyle.Standard:
                    ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(m_ProcSelectPanel.CurrentStage.Type, m_ProcSelectPanel.CurrentProductionMode, m_ProcSelectPanel.CurrentProcedureType);
                    SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionSimulationScene, sceneInfo);
                    break;
                case StageStyle.Fault:
                    sceneInfo = new ProductionSimulationSceneInfo(m_ProcSelectPanel.CurrentStage.Type, m_ProcSelectPanel.CurrentProductionMode, m_ProcSelectPanel.CurrentFaultID);
                    SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionSimulationScene, sceneInfo);
                    break;
                default:
                    break;
            }
        }
    }
}

