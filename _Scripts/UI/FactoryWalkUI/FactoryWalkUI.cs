using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    public class FactoryWalkUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.FactoryWalkUI;
        }

        private int m_CurrentCameraIndex = 1;
        /// <summary>
        /// 当前相机索引
        /// </summary>
        public int CurrentCameraIndex
        {
            get { return m_CurrentCameraIndex; }
            set { m_CurrentCameraIndex = value; }
        }

        /// <summary>
        /// 切换相机模式
        /// </summary>
        private SwitchCameraMode m_CameraSwitch;

        private CameraMode m_CurrentCameraMode = CameraMode.Overlook;
        /// <summary>
        /// 相机模式
        /// </summary>
        public CameraMode CurrentCameraMode
        {
            get { return m_CurrentCameraMode; }
            set { m_CurrentCameraMode = value; }
        }

        /// <summary>
        /// 相机文本
        /// </summary>
        private Text cameraText;

        /// <summary>
        /// 标签父物体
        /// </summary>
        private GameObject m_ObjectLabelParent;

        /// <summary>
        /// 规划数据面板
        /// </summary>
        private PlanningInfoBar planningInfoBar;

        /// <summary>
        /// 知识点控制器
        /// </summary>
        private KnowledgePointController m_KnowledgePointController;

        /// <summary>
        /// 知识点列表
        /// </summary>
        private List<KnowledgePoint> m_KnowledgePoints;

        /// <summary>
        /// 厂区展示按钮
        /// </summary>
        private Button buttonDisplay;

        /// <summary>
        /// 漫游按钮
        /// </summary>
        private Button buttonWalk;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 大地图栏
        /// </summary>
        private MiniMap normalMapBar;

        /// <summary>
        /// 主角
        /// </summary>
        private Transform m_Player;

        /// <summary>
        /// 传送点控制器
        /// </summary>
        private TransferPointController m_TransferPointController;

        /// <summary>
        /// 漫游模块
        /// </summary>
        private FactoryWalkModule module;

        /// <summary>
        /// 进入车间碰撞器
        /// </summary>
        private EnterWorkshopCollision m_Conllision;

        /// <summary>
        /// 进入车间碰撞器
        /// </summary>
        private EnterWorkshopCollision m_Conllision_1;

        /// <summary>
        /// 知识点栏
        /// </summary>
        private KnowledgePointBar m_KnowledgePointBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            Messager.Instance.AddListener("PostKnowledgePointMsg", ReceiveKnowledgePointMsg);
            ModuleManager.Instance.Register<FactoryWalkModule>();
            module = ModuleManager.Instance.GetModule<FactoryWalkModule>();

            InitGUI();
            InitEvent();
        }


        protected override void OnRelease()
        {
            base.OnRelease();
            Messager.Instance.RemoveListener("PostKnowledgePointMsg", ReceiveKnowledgePointMsg);
            ModuleManager.Instance.Unregister<FactoryWalkModule>();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            planningInfoBar = transform.Find("Background/PlanningInfoBar").GetComponent<PlanningInfoBar>();
            buttonDisplay = transform.Find("Background/TitleBar/Buttons/ButtonDisplay").GetComponent<Button>();
            buttonWalk = transform.Find("Background/TitleBar/Buttons/ButtonWalk").GetComponent<Button>();
            buttonBack = transform.Find("Background/TitleBar/Buttons/ButtonBack").GetComponent<Button>();
            normalMapBar = transform.Find("Background/NormalMapBar").GetComponent<MiniMap>();
            m_KnowledgePointBar = transform.Find("Background/KnowledgePointBar").GetComponent<KnowledgePointBar>();
        }

        private void InitEvent()
        {
            planningInfoBar.OnDisplayEffect.AddListener(planningInfoBar_OnDisplayEffect);
            planningInfoBar.OpenNormalMap.AddListener(planningInfoBar_OpenNormalMap);
            planningInfoBar.OnTransfer.AddListener(planningInfoBar_OnTransfer);
            buttonDisplay.onClick.AddListener(buttonDisplay_onClick);
            buttonWalk.onClick.AddListener(buttonWalk_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            normalMapBar.ItemClicked.AddListener(normalMapBar_ItemClicked);
        }

        void Start()
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").transform;
            //ToolTip.Instance.IsEnabled = true;
            normalMapBar.gameObject.SetActive(false);
            m_KnowledgePointBar.Close();

            //string xmlPath = Application.streamingAssetsPath + "/FactoryWalk/KnowledgePoint.xml";
            //module.InitData(xmlPath);
            m_KnowledgePoints = module.GetKnowledgePoints();

            m_KnowledgePointController = GameObject.Find("厂区/知识点").GetComponent<KnowledgePointController>();
            m_ObjectLabelParent = GameObject.Find("厂区/标签");
            m_TransferPointController = GameObject.Find("厂区/传送位置").GetComponent<TransferPointController>();
            m_Conllision = GameObject.Find("厂区/漫游点/进入生产厂房碰撞器").GetComponent<EnterWorkshopCollision>();
            //m_Conllision_1 = GameObject.Find("厂区/漫游点/进入生物制药车间碰撞器 (1)").GetComponent<EnterWorkshopCollision>();
            m_Conllision.OnCollision.AddListener(m_Conllision_OnCollision);
            //m_Conllision_1.OnCollision.AddListener(m_Conllision_OnCollision);

            m_CameraSwitch = Camera.main.GetComponent<SwitchCameraMode>();
            //解析场景信息
            this.Invoke(0.2f, ParseScenceInfo);
        }

        /// <summary>
        /// 解析场景信息
        /// </summary>
        private void ParseScenceInfo()
        {
            SceneParam param = SceneLoader.Instance.GetSceneParam(SceneType.FactoryWalkScene);
            if (param == null)
                return;

            FactoryWalkSceneInfo sceneInfo = param as FactoryWalkSceneInfo;
            if (sceneInfo != null && sceneInfo.FromWorkshop)
            {
                buttonWalk_onClick();
                Transform point = GameObject.Find("厂区/漫游点/生物制药车间").transform;
                m_Player.position = new Vector3(point.position.x, m_Player.position.y, point.position.z);
                m_Player.rotation = point.rotation;
                Camera.main.transform.rotation = point.rotation;
                normalMapBar.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 厂区漫游按钮点击时，触发
        /// </summary>
        private void buttonWalk_onClick()
        {
            m_KnowledgePointController.CloseAllKnowledgePoint();

            m_CameraSwitch.Switch(CameraMode.Roaming);
            planningInfoBar.SetTitle("漫游");
            planningInfoBar.SetDescription("", null);
            planningInfoBar.SetActiveOfPlanningData(false);
            planningInfoBar.SetActiveOfTransferData(true);
            m_ObjectLabelParent.SetActive(false);
        }

        /// <summary>
        /// 厂区展示按钮点击时，触发
        /// </summary>
        private void buttonDisplay_onClick()
        {
            m_CameraSwitch.Switch(CameraMode.Overlook);
            planningInfoBar.SetTitle("厂区展示");
            planningInfoBar.SetActiveOfPlanningData(true);
            planningInfoBar.SetActiveOfTransferData(false);
            m_ObjectLabelParent.SetActive(true);
        }

        /// <summary>
        /// 规划数据显示
        /// </summary>
        /// <param name="name"></param>
        private void planningInfoBar_OnDisplayEffect(string url, bool b)
        {
            KnowledgePoint point = m_KnowledgePoints.First(x => x.URL == url);
            if (point == null)
                return;

            if (b)
            {
                m_KnowledgePointController.DisplayKnowledgePoint(point.Name);
                planningInfoBar.SetDescription(point.Description, point.Sprite);
            }
            else
            {
                m_KnowledgePointController.CloseKnowledgePoint(point.Name);
                planningInfoBar.SetDescription("", null);
            }

        }

        /// <summary>
        /// 打开大地图
        /// </summary>
        private void planningInfoBar_OpenNormalMap()
        {
            normalMapBar.gameObject.SetActive(true);
        }

        /// <summary>
        /// 大地图标签点击时，触发
        /// </summary>
        /// <param name="marker"></param>
        private void normalMapBar_ItemClicked(MiniMapMarker marker)
        {
            MiniMapItem item = marker.Item;

            switch (item.Type)
            {
                case MapMarkerType.None:
                    break;
                case MapMarkerType.TransferPoint:
                    Transform point = m_TransferPointController.GetTransferPoint(item.name);
                    TransferPlayer(point);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 传送玩家事件
        /// </summary>
        /// <param name="name"></param>
        private void planningInfoBar_OnTransfer(string name)
        {
            Transform point = m_TransferPointController.GetTransferPoint(name);
            TransferPlayer(point);
        }

        /// <summary>
        /// 传送玩家
        /// </summary>
        /// <param name="point"></param>
        private void TransferPlayer(Transform point)
        {
            ///根据测试文档  （徐靖） 不只是在漫游模式下 也要传送
            //漫游状态切换 
            buttonWalk_onClick();
            //if (m_CameraSwitch.CurrentMode == CameraMode.Roaming)
            {
                m_Player.position = point.position;
                m_Player.rotation = point.rotation;
                Camera.main.transform.rotation = point.rotation;
                normalMapBar.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 进入车间碰撞器
        /// </summary>
        private void m_Conllision_OnCollision()
        {
            MessageBoxEx.Show("是否进入生产厂房？", "提示", MessageBoxExEnum.CommonDialog, x =>
            {
                bool b = (bool)x.Content;
                if (b)
                {
                    SceneLoader.Instance.LoadSceneAsync(SceneType.WorkshopRoamingScene);
                }
            });
        }

        /// <summary>
        /// 返回主界面
        /// </summary>
        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        /// <summary>
        /// 接收知识点消息
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveKnowledgePointMsg(Message message)
        {
            string name = message.Content as string;
            KnowledgePoint point = m_KnowledgePoints.FirstOrDefault(x => x.Name == name);

            if (point != null)
            {
                m_KnowledgePointBar.Display(point);
            }
        }

    }

    /// <summary>
    /// 场景漫游场景信息
    /// </summary>
    public class FactoryWalkSceneInfo : SceneParam
    {
        /// <summary>
        /// 是否来自车间
        /// </summary>
        public bool FromWorkshop { get; set; }
    }

}
