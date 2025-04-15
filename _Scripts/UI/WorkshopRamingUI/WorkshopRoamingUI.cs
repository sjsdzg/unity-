using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using UnityStandardAssets.ImageEffects;
using UIWidgets;
using XFramework.Simulation.Component;

namespace XFramework.UI
{
    /// <summary>
    /// 车间漫游界面
    /// </summary>
    public class WorkshopRoamingUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.WorkshopRoamingUI;
        }

        /// <summary>
        /// 生产车间地图栏目
        /// </summary>
        private WorkshopMapBar workshopMapBar;

        /// <summary>
        /// 小地图控制器
        /// </summary>
        private RamingMiniMapController MiniMapControl;

        /// <summary>
        /// 打开大地图按钮
        /// </summary>
        private Button buttonGlobal;

        /// <summary>
        /// 更衣流程栏
        /// </summary>
        private DressingPathBar dressingPathBar;

        ///// <summary>
        ///// 右侧UI窗口
        ///// </summary>
        //private RightUIGroup m_RightUIGroup;
        /// <summary>
        /// 介绍信息树（左侧列表）
        /// </summary>
        private IntroduceTreeBar introduceTreeBar;
        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 模糊组件
        /// </summary>
        private BlurComponent blurCom;


        /// <summary>
        /// 切换左侧面板
        /// </summary>
        private Button buttonChange;


        /// <summary>
        /// 返回沙盘模式（鸟瞰）
        /// </summary>
        private Button buttonReturn;
        /// <summary>
        /// 图形标识信息
        /// </summary>
        private GraphicInfoPanel graphicInfoPanel;

        /// <summary>
        /// 知识点列表
        /// </summary>
        private List<IntroductionPoint> m_IntroducePoints;

        /// <summary>
        /// 漫游模块module
        /// </summary>
        private WorkshopRoamingModule RoamingModule;

        /// <summary>
        /// 中间内容展示框(介绍窗口)
        /// </summary>
        private ContentDisplayPanel contentDisplayPanel;


        /// <summary>
        /// 总体介绍窗口
        /// </summary>
        private OverallInfoBar m_OverallInfoBar;

        /// <summary>
        /// 设备窗口
        /// </summary>
        private MiniDeviceInfoBar m_DeviceInfoBar;

        /// <summary>
        /// HUD label 标签
        /// </summary>
        private HUDLabelViewBar m_HudBar;

    

        /// <summary>
        /// 所在当前房间的介绍内容
        /// </summary>
        private List<IntroductionPoint> currentWorkshopList = new List<IntroductionPoint>();


        /// <summary>
        /// 当前房间进入时的参数
        /// </summary>
        private EnterWorkshopArgs currentEnterArgs;
        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<WorkshopRoamingModule>();
            RoamingModule = ModuleManager.Instance.GetModule<WorkshopRoamingModule>();

            Messager.Instance.AddListener("DeviceKnowledgePoint", ReceiveDeviceKnowledgePoint);
            Messager.Instance.AddListener(MessageType.WorkshopInterOrExterMsg,ReceiverRoomInfoMsg);
            Messager.Instance.AddListener(MessageType.WorkshopOverallMsg, ReceiverOverallShowMsg);

            InitGUI();
            InitEvent();
        }
        protected override void OnStart()
        {
            base.OnStart();
            //string xmlPath = Application.streamingAssetsPath + "/WorkshopRaming/IntroductionPoint.xml";
            //RoamingModule.InitData(xmlPath);
            m_IntroducePoints = RoamingModule.GetIntroducePoints();

            m_DeviceInfoBar.IsActive = false;
            m_OverallInfoBar.IsActive = false;
            //MiniMapControl = GameObject.Find("综合制剂生产车间/地图").GetComponent<RamingMiniMapController>();
            //MiniMapControl.OnGroundCollision.AddListener(MiniMapControl_OnGroundCollision);

        }


        protected override void OnRelease()
        {
            base.OnRelease();
            Messager.Instance.RemoveAllListener();
            RamingUIManager.Instance.CloseAllPanel();
            RamingUiInfo.Instance.RemoveAll();
            ModuleManager.Instance.Unregister<WorkshopRoamingModule>();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            buttonBack = transform.Find("Background/TitleBar/ButtonBack").GetComponent<Button>();
            introduceTreeBar = transform.Find("Background/IntroduceBar").GetComponent<IntroduceTreeBar>();
            blurCom = transform.Find("Background/MaskBar").GetComponent<BlurComponent>();
            buttonChange = transform.Find("Background/ChangeButton").GetComponent<Button>();
            buttonReturn = transform.Find("Background/ReturnButton").GetComponent<Button>();
            contentDisplayPanel = transform.Find("Background/IntroduceContentBar").GetComponent<ContentDisplayPanel>();
            m_DeviceInfoBar = transform.Find("Background/MiniDeviceInfoBar").GetComponent<MiniDeviceInfoBar>();
            m_OverallInfoBar = transform.Find("Background/OverallInfoBar").GetComponent<OverallInfoBar>();
            //m_HudBar = transform.Find("Background/HUDLabelViewUI").GetComponent<HUDLabelViewBar>();
              
            buttonChange.gameObject.SetActive(false);
            buttonReturn.gameObject.SetActive(false);

            RamingUiInfo.Instance.RegisterUI(RamingUI.IntroduceTree, introduceTreeBar);
            RamingUiInfo.Instance.RegisterUI(RamingUI.Device, m_DeviceInfoBar);
            RamingUiInfo.Instance.RegisterUI(RamingUI.Overall, m_OverallInfoBar);
            //RamingUiInfo.Instance.RegisterUI(RamingUI.HUDLabel, m_HudBar);

        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            buttonBack.onClick.AddListener(buttonBack_onClick);
            introduceTreeBar.NodeSelected.AddListener(introduceTreeBar_NodeSelected);
            buttonChange.onClick.AddListener(buttonChange_onClick);
            buttonReturn.onClick.AddListener(buttonReturn_OnClick);
            contentDisplayPanel.OnClosePanel.AddListener(contentDisplayPanel_OnClosePanel);
        }

        /// <summary>
        /// 总体介绍内容展示
        /// </summary>
        /// <param name="message"></param>
        private void ReceiverOverallShowMsg(Message message)
        {
            EnterWorkshopArgs _arg = message.Content as EnterWorkshopArgs;
            currentEnterArgs = _arg;

            IntroduceContent _content = RoamingModule.GetOverallInfo(_arg.Name);

            OverallUiArgs overaArg = new OverallUiArgs();
            overaArg.introduceInfo = _content;
            overaArg.IsShowEnterBtn = true;
            overaArg.EnterAction = _arg.EnterAction;
            overaArg.ExitAction = _arg.ExitAction;

            overaArg.CloseAction = () => { };
            RamingUIManager.Instance.OpenPanel(RamingUI.Overall,overaArg);


            buttonChange.gameObject.SetActive(true);
            ///返回飞行模式视角 的UI显示
            buttonReturn.gameObject.SetActive(true);

        }
        /// <summary>
        /// 中央窗口关闭
        /// </summary>
        private void contentDisplayPanel_OnClosePanel()
        {
            blurCom.Close();
        }

        /// <summary>
        /// 返回鸟瞰模式
        /// </summary>
        private void buttonReturn_OnClick()
        {
            CloseAllPanel();
            currentEnterArgs.ExitAction.Invoke();

            buttonChange.gameObject.SetActive(false);
            buttonReturn.gameObject.SetActive(false);
        }
        /// <summary>
        /// 左侧树状图切换按钮
        /// </summary>
        private void buttonChange_onClick()
        {
            introduceTreeBar.PlayListPanelAnimation();
        }
        /// <summary>
        /// 进出房间事件
        /// </summary>
        /// <param name="message"></param>
        private void ReceiverRoomInfoMsg(Message message)
        {
            RoomTriggerArg info = message.Content as RoomTriggerArg;
            if (info == null)
                return;

            ///进入房间
            if (info.IsInter)
            {
                ///介绍数据传入
                m_IntroducePoints.ForEach((x) =>
                {
                    if (x.Name == info.Name)
                    {
                        currentWorkshopList.Clear();
                        currentWorkshopList.Add(x);
                        introduceTreeBar.InitData(currentWorkshopList);
                        ///打开 介绍窗口
                        RamingUIManager.Instance.OpenPanel(RamingUI.IntroduceTree, new BaseUIArgs());
                    }
                });
            }
            ///退出房间
            else
            {
                ///关闭所有窗口
                CloseAllPanel();
            }
        }
    
        /// <summary>
        /// 设计信息面板点击时，触发
        /// </summary>
        /// <param name="node"></param>
        private void introduceTreeBar_NodeSelected(TreeNode<TreeViewItem> node)
        {
            object tag = node.Item.Tag;

            if (tag is IntroduceContent)//具体内容节点
            {
                IntroduceContent _Item = tag as IntroduceContent;

                ///“整体介绍”的内容
                if (_Item.Name == "整体介绍内容")
                {
                    Messager.Instance.SendMessage(new Message(MessageType.WorkshopOverallMsg, this, currentEnterArgs));
                }
                else 
                {
                    blurCom.Open();
                    contentDisplayPanel.ShowPanel(_Item.Text);
                }
            }
            //设备知识点
            else if (tag is MachineItem)
            {
                MachineItem _mItem = tag as MachineItem;
                if (_mItem != null)
                {
                    DeviceUiArgs deviceArgs = new DeviceUiArgs();
                    deviceArgs.machineInfo = _mItem;
                    deviceArgs.CloseAction = DeviceInfoBar_OnClosePanel;
                    ///打开UI 并发送 ui参数
                    RamingUIManager.Instance.OpenPanel(RamingUI.Device, deviceArgs);
                    ///向3D 发送设备打开参数
                    Messager.Instance.SendMessage(MessageType.WorkshopDeviceMsg ,this, new RamingUI2DeviceMsg(true));
                    blurCom.Open();

                }
            }
        }

        /// <summary>
        /// 关闭所有窗口
        /// </summary>
        private void CloseAllPanel()
        {
            ///GMP模块关闭
            //Messager.Instance.SendMessage(new Message(MessageType.WorkshopGmpMsg, this, new RamingUI2GmpMsg(false, null)));
            RamingUIManager.Instance.ClosePanel(RamingUI.Overall);
            RamingUIManager.Instance.ClosePanel(RamingUI.Device);
            RamingUIManager.Instance.ClosePanel(RamingUI.IntroduceTree);
            //RamingUIManager.Instance.ClosePanel(RamingUI.HUDLabel);

        }
        /// <summary>
        /// 小地图控制器-地面碰撞时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void MiniMapControl_OnGroundCollision(int index)
        {
            if (!workshopMapBar.gameObject.activeSelf && !dressingPathBar.gameObject.activeSelf)
            {
                workshopMapBar.SwitchMap(index);
            }
        }

        /// <summary>
        /// 接受设备知识点
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveDeviceKnowledgePoint(Message message)
        {
            string name = (string)message.Content;

            if (!string.IsNullOrEmpty(name))
            {

                for (int i = 0; i < m_IntroducePoints.Count; i++)
                {
                    for (int j = 0; j < m_IntroducePoints[i].MachinesList[0].MachineList.Count; j++)
                    {
                        MachineItem _mItem = m_IntroducePoints[i].MachinesList[0].MachineList[j];
                        if (_mItem.Name==name)
                        {
                            ///返回飞行模式视角 的UI显示
                            buttonReturn.gameObject.SetActive(true);
                            Messager.Instance.SendMessage(MessageType.WorkshopDeviceMsg, this,new RamingUI2DeviceMsg(true));
                    
                            DeviceUiArgs deviceArgs = new DeviceUiArgs();
                            deviceArgs.machineInfo = _mItem;
                            deviceArgs.CloseAction = DeviceInfoBar_OnClosePanel;
                            RamingUIManager.Instance.OpenPanel(RamingUI.Device,deviceArgs);

                            blurCom.Open();

                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 设备展示面板关闭事件
        /// </summary>
        private void DeviceInfoBar_OnClosePanel()
        {
            blurCom.Close();
            ///发送关闭设备展示窗口的消息
            Messager.Instance.SendMessage(MessageType.WorkshopDeviceMsg, this, new RamingUI2DeviceMsg(false));
        }


        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            ////厂区漫游场景信息
            //FactoryWalkSceneInfo sceneInfo = new FactoryWalkSceneInfo();
            //sceneInfo.FromWorkshop = true;
            SceneLoader.Instance.LoadSceneAsync(SceneType.FactoryWalkScene);
        }
    }
}
