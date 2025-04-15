using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Simulation;
using UnityEngine.Events;
using XFramework.Network;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 生产主界面
    /// </summary>
    public class ProductionMainUI : BaseUI
    {
        /// <summary>
        /// 沙盘栏
        /// </summary>
        private SandTableBar m_SandTableBar;

        /// <summary>
        /// 沙盘控制器
        /// </summary>
        private SandTableController m_SandTableControl;

        /// <summary>
        /// 工艺流程按钮
        /// </summary>
        private Button buttonProcess;

        /// <summary>
        /// 沙盘展示按钮
        /// </summary>
        private Button buttonDisplay;

        /// <summary>
        /// 工段面板
        /// </summary>
        //private StagePanel stagePanel;

        /// <summary>
        /// 流程面板
        /// </summary>
        //private ProcedurePanel procedurePanel;
        /// <summary>
        /// 工段元素列表
        /// </summary>
        //private List<StageElement> stageElements;

        private ProcSelectPanel m_ProcSelectPanel;

        /// <summary>
        /// Project
        /// </summary>
        private Project m_Project;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;        

        /// <summary>
        /// 生产操作主界面模块
        /// </summary>
        private ProductionMainModule module;

        /// <summary>
        /// 关卡信息模块
        /// </summary>
        private LevelInfoModule levelInfoModule;
        /// <summary>
        /// 添加的工段
        /// </summary>
        private Button button_CarbonStage;

        public override EnumUIType GetUIType()
        {
            return EnumUIType.ProductionMainUI;
        }

        /// <summary>
        /// 沙盘类型
        /// </summary>
        private SandTableType type = SandTableType.efavirenzSandTable;

        protected override void OnAwake()
        {
            base.OnAwake();

            ModuleManager.Instance.Register<ProductionMainModule>();
            ModuleManager.Instance.Register<LevelInfoModule>();
            module = ModuleManager.Instance.GetModule<ProductionMainModule>();
            levelInfoModule = ModuleManager.Instance.GetModule<LevelInfoModule>();

            InitGUI();
            InitEvent();

            if (App.Instance.RunningEdition == RunningEdition.Network && GlobalManager.DefaultMode == ProductionMode.Banditos)
            {
                InitLevelInfo();
            }
            //if (App.Instance.VersionTag!=VersionTag.SNTCM)
            //{
            //    button_CarbonStage.gameObject.SetActive(false);
            //}
            button_CarbonStage.gameObject.SetActive(false);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ProductionMainModule>();
            DOTween.KillAll();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            m_SandTableBar = transform.Find("Background/SandTableBar").GetComponent<SandTableBar>();
            buttonProcess = transform.Find("Background/TitleBar/Buttons/ButtonProcess").GetComponent<Button>();
            buttonDisplay = transform.Find("Background/TitleBar/Buttons/ButtonDisplay").GetComponent<Button>();
            buttonBack = transform.Find("Background/TitleBar/Buttons/ButtonBack").GetComponent<Button>();
            m_ProcSelectPanel = transform.Find("Background/ProcSelectPanel").GetComponent<ProcSelectPanel>();
            //stagePanel = transform.Find("Background/StagePanel").GetComponent<StagePanel>();
            //procedurePanel = transform.Find("Background/ProcedurePanel").GetComponent<ProcedurePanel>();
            button_CarbonStage = transform.Find("Background/ButtonCarbonStage").GetComponent<Button>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            m_SandTableBar.DisplaySandTable.AddListener(x => { m_SandTableControl.DisplaySandTable(x); });
            buttonProcess.onClick.AddListener(buttonProcess_onClick);
            buttonDisplay.onClick.AddListener(buttonDisplay_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            //stagePanel.OnStageClick.AddListener(stagePanel_OnStageClick);
            //procedurePanel.OnProcedureClick.AddListener(procedurePanel_OnProcedureClick);
            //processPanel.ClickProcess.AddListener(processSelect_OnClick);
           // m_ProcSelectPanel.OnSelected.AddListener(m_ProcSelectPanel_OnSelected);
            button_CarbonStage.onClick.AddListener(button_CarbonStage_onClick);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //stagePanel.Hide();
            //procedurePanel.Hide();
            m_ProcSelectPanel.Hide();
            m_SandTableControl = UnityEngine.GameObject.Find("操作仿真沙盘").GetComponent<SandTableController>();
            m_SandTableControl.StagePointClicked.AddListener(m_SandTableControl_StagePointClicked);
            this.Invoke(0.5f, () =>
            {
                m_SandTableControl.DisplaySandTable("1");
                buttonProcess_onClick();
            });

            m_Project = module.Project;
            //string path = Application.streamingAssetsPath + "/ProductionMain/ProductionMain.xml";
            //if (System.IO.File.Exists(path))
            //{
            //    module.InitData(path);
            //    stageElements = module.GetStageElements();
            //}
         
        }

        private void InitLevelInfo()
        {
            if (!NetworkManager.Instance.IsConnected)
                return;

            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
            sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
            levelInfoModule.CountLevelInfoByCondition(sqlConditions, ReceiveCountLevelInfoByConditionResp);
        }

        private void ReceiveCountLevelInfoByConditionResp(NetworkPackageInfo packageInfo)
        {
            CountLevelInfoByConditionResp resp = CountLevelInfoByConditionResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Count == 0)
            {
                Debug.LogFormat("用户名称:{0}的关卡信息不存在！", GlobalManager.user.UserName);
                LevelInfo levelInfo = new LevelInfo();
                levelInfo.UserId = GlobalManager.user.Id;
                levelInfo.Level = 1;
                levelInfo.Data = "";
                levelInfo.Remark = "";
                levelInfoModule.InsertLevelInfo(levelInfo, ReceiveInsertLevelInfoResp);
            }
            else
            {
                Debug.LogFormat("用户名称:{0}的关卡信息已存在！", GlobalManager.user.UserName);
                List<SqlCondition> sqlConditions = new List<SqlCondition>();
                sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
                sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
                levelInfoModule.GetLevelInfoByCondition(sqlConditions, ReceiveGetLevelInfoByConditionResp);
            }
        }

        private void ReceiveInsertLevelInfoResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                Debug.LogFormat("用户名称:{0}的关卡信息已添加！", GlobalManager.user.UserName);
                List<SqlCondition> sqlConditions = new List<SqlCondition>();
                sqlConditions.Add(new SqlCondition(Constants.SOFTWARE_ID, SqlOption.Equal, SqlType.String, App.Instance.SoftwareId));
                sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
                levelInfoModule.GetLevelInfoByCondition(sqlConditions, ReceiveGetLevelInfoByConditionResp);
            }
        }

        /// <summary>
        /// 根据用户id，获取关卡信息
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetLevelInfoByConditionResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                GetLevelInfoByConditionResp resp = GetLevelInfoByConditionResp.Parser.ParseFrom(packageInfo.Body);
                LevelInfo levelInfo = new LevelInfo();
                levelInfo.Id = resp.LevelInfo.Id;
                levelInfo.UserId = resp.LevelInfo.UserId;
                levelInfo.Level = resp.LevelInfo.Level;
                levelInfo.Data = resp.LevelInfo.Data;
                levelInfo.Remark = resp.LevelInfo.Remark;
                GlobalManager.levelInfo = levelInfo;
                Debug.LogFormat("用户名称:{0}的关卡信息已获取，关卡信息如下：{1}", GlobalManager.user.UserName, levelInfo.ToString());
                m_SandTableControl.SetLevelInfo(GlobalManager.levelInfo.Level);
            }
        }

        /// <summary>
        /// 工艺按钮点击时，触发
        /// </summary>
        private void buttonProcess_onClick()
        {
            //if (type == SandTableType.efavirenzSandTable)
            //{
            //    m_SandTableControl.DisplayCurrentProcess();
            //    //processPanel.MoveAnimation();
            //}
            //else if (type == SandTableType.PureWaterSandTable)
            //{

            //    m_SandTableControl.DisplayCurrentProcess();
            //}
            m_SandTableControl.DisplayProcess(0);
        }

        /// <summary>
        /// 流程选择按钮点击
        /// </summary>
        //private void processSelect_OnClick(int index)
        //{
        //    m_SandTableControl.CurrentIndex = index;
        //    m_SandTableControl.DisplayCurrentProcess();
        //}

        /// <summary>
        /// 沙盘展示按钮点击时，触发
        /// </summary>
        private void buttonDisplay_onClick()
        {
            m_SandTableControl.DisplaySandTable(m_SandTableControl.CurrentName);
        }

        /// <summary>
        /// 沙盘控制器工段点击时，触发
        /// </summary>
        /// <param name="name">工段名称</param>
        private void m_SandTableControl_StagePointClicked(string name)
        {
            name = name.Substring(name.IndexOf("\n") + 1);
            Debug.Log(name);
            Stage value = m_Project.GetStage(name + "工段");
            if (value == null)
                return;

            //  m_ProcSelectPanel.Show(value);
            m_ProcSelectPanel_OnSelected(value);

        }

        /// <summary>
        /// 工段面板点击时，触发
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="stage"></param>
        //private void stagePanel_OnStageClick(ProductionMode mode, StageElement stage)
        //{
        //    //procedurePanel.Show(mode, stage);
        //}

        /// <summary>
        /// 流程面板点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        //private void procedurePanel_OnProcedureClick(ProductionMode mode, StageType stageType, ProcedureType procedureType)
        //{
        //    Release();
        //    ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(stageType, mode, procedureType);
        //    if (type == SandTableType.efavirenzSandTable)
        //    {
        //        LevelManager.Instance.LoadSceneAsync(EnumSceneType.ProductionSimulationScene, sceneInfo);
        //    }
        //    else if(type == SandTableType.PureWaterSandTable)
        //    {
        //        LevelManager.Instance.LoadSceneAsync(EnumSceneType.PureWaterScene, sceneInfo);
        //    }
        //}

        /// <summary>
        /// 进入到操作场景
        /// </summary>
        private void m_ProcSelectPanel_OnSelected(Stage stage)
        {
            ProductionSimulationSceneInfo sceneInfo = null;
            sceneInfo = new ProductionSimulationSceneInfo(stage.Type, GlobalManager.DefaultMode);
            switch (stage.Type)
            {
                case StageType.CellExpansionStage:
                    SceneLoader.Instance.LoadSceneAsync(SceneType.CellExpansionStageScene, sceneInfo);
                    break;
                case StageType.CellCultivateStage:
                    SceneLoader.Instance.LoadSceneAsync(SceneType.CellCultivateStageScene, sceneInfo);
                    break;
                case StageType.CultureMediumPreparationStage:
                    SceneLoader.Instance.LoadSceneAsync(SceneType.CultureMediumPreparationScene, sceneInfo);
                    break;
                case StageType.ProteinPurificationStage:
                    SceneLoader.Instance.LoadSceneAsync(SceneType.ProteinPurificationStageScene, sceneInfo);
                    break;
                default:
                    break;
            }
           

        }

        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }
        private void button_CarbonStage_onClick()
        {
            Stage value = m_Project.GetStage("羰基化工段");
            if (value == null)
                return;

            m_ProcSelectPanel.Show(value);
        }
    }

    /// <summary>
    /// 当前沙盘的类型
    /// </summary>
    public enum SandTableType
    {
        /// <summary>
        /// 依非韦伦片剂流程沙盘
        /// </summary>
        efavirenzSandTable = 1,

        /// <summary>
        /// 制药用水系统沙盘
        /// </summary>
        PureWaterSandTable = 2,
    }

}
