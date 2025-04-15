using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 
    /// </summary>
	public class WorkshopMainUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 数据
        /// </summary>
        private WorkshopRoamingModule module;

        /// <summary>
        /// 左侧按钮列表
        /// </summary>
        private ProcessBarUI processBarUI;
        /// <summary>
        /// 中央窗口
        /// </summary>
        private CeterWindowUI ceterWindowUI;

        /// <summary>
        /// 漫游 沙盘控制器
        /// </summary>
        private WorkSandTableController m_SandTableControl;


        /// <summary>
        /// 工艺流程按钮
        /// </summary>
        private Button buttonProcess;
        /// <summary>
        /// 流程按钮面板（页面底部）
        /// </summary>
        private ProcessPanel processPanel;

        /// <summary>
        /// 沙盘展示按钮（复位）
        /// </summary>
        private Button buttonDisplay;
        /// <summary>
        /// 介绍列表
        /// </summary>
        private List<IntroductionPoint> introductionList;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<WorkshopRoamingModule>();
            module = ModuleManager.Instance.GetModule<WorkshopRoamingModule>();

            InitGUI();
            InitEvent();
        }
        protected override void OnStart()
        {
            base.OnStart();
            string path = Application.streamingAssetsPath + "/WorkshopMain/IntroductionPoint.xml";
            //string path = Application.streamingAssetsPath+ "/WorkshopMain/IntroductionPoint.xml";
            //module.InitData(path);
            //introductionList = module.GetIntroducePoints();
            //m_SandTableControl.WorkPointClicked.AddListener();
        }

        protected override void OnRelease()
        {
            base.OnRelease();

            ModuleManager.Instance.Unregister<WorkshopRoamingModule>();
            DOTween.KillAll();
        }

        private void InitGUI()
        {
            ceterWindowUI = transform.Find("Background/CeterWindow").GetComponent<CeterWindowUI>();
            processBarUI = transform.Find("Background/ProcessBar").GetComponent<ProcessBarUI>();
            buttonProcess = transform.Find("Background/TitleBar/Buttons/ButtonProcess").GetComponent<Button>();
            buttonDisplay = transform.Find("Background/TitleBar/Buttons/ButtonDisplay").GetComponent<Button>();
            processPanel = transform.Find("Background/TitleBar/Process").GetComponent<ProcessPanel>();

            m_SandTableControl = GameObject.Find("沙盘_漫游模块").GetComponent<WorkSandTableController>();
            ///工段 item点击事件
            m_SandTableControl.WorkPointClicked.AddListener(SandTableControl_WorkPointClicked);
            this.Invoke(0.5f, () => { m_SandTableControl.DisplaySandTable("1");
                ////显示默认的片剂流程列表和沙盘流程
                processSelect_OnClick(0);
            });
           
        }
        private void InitEvent()
        {
            
            processBarUI.OnClickProcessBar.AddListener(ProcessBarUI_ClickEvent);
            buttonProcess.onClick.AddListener(buttonProcess_ClickEvent);
            processPanel.ClickProcess.AddListener(processSelect_OnClick);
            buttonDisplay.onClick.AddListener(buttonDisplay_OnClick);
        }

        /// <summary>
        /// 
        /// </summary>
        private void buttonDisplay_OnClick()
        {
            m_SandTableControl.DisplaySandTable(m_SandTableControl.CurrentName);

        }


        /// <summary>
        /// 沙盘流程 按钮点击事件
        /// </summary>
        private void SandTableControl_WorkPointClicked(string name)
        {
            try
            {
                IntroductionPoint Point = introductionList.First(x => x.Name == name.Trim());

                IntroduceContents item = Point.IntroduceList.First(U => U.Name == "整体介绍");

                ceterWindowUI.ShowWindow(Point.Name, item);
            }
            catch
            {
                Debug.Log("读取介绍节点出错:" + name);
            }
        }
        /// <summary>
        /// 流程选择按钮点击
        /// </summary>
        private void processSelect_OnClick(int index)
        {
            m_SandTableControl.CurrentIndex = index;
            ///显示当前沙盘
            m_SandTableControl.DisplaySandTable(m_SandTableControl.CurrentName);
            ///显示沙盘上的流程
            m_SandTableControl.DisplayCurrentProcess();
            //左侧显示列表
            processBarUI.ShowProcessList(index);
        }

        /// <summary>
        /// 流程选择
        /// </summary>
        private void buttonProcess_ClickEvent()
        {
            processPanel.MoveAnimation();
        }
        /// <summary>
        /// 流程显示面板
        /// </summary>
        /// <param name="name"></param>
        public void ProcessBarUI_ClickEvent(string name)
        {
            //print("Show panel"+"  name:"+name+"  "+introductionList.Count);
       

            ///沙盘个别地方高亮
            m_SandTableControl.SetHighlightByName(name);
            m_SandTableControl.SetBuildingTransparency();
        }
    }
}