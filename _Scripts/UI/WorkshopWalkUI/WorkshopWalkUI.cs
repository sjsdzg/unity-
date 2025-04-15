using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 车间漫游界面
    /// </summary>
    public class WorkshopWalkUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生产车间地图栏目
        /// </summary>
        private WorkshopMapBar workshopMapBar;

        /// <summary>
        /// 小地图控制器
        /// </summary>
        private MiniMapController MiniMapControl;

        /// <summary>
        /// 打开大地图按钮
        /// </summary>
        private Button buttonGlobal;

        /// <summary>
        /// 更衣流程栏
        /// </summary>
        private DressingPathBar dressingPathBar;

        /// <summary>
        /// 设备知识点栏
        /// </summary>
        private MiniDeviceInfoBar miniDeviceInfoBar;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 图形标识信息
        /// </summary>
        private GraphicInfoPanel graphicInfoPanel;

        /// <summary>
        /// 知识点列表
        /// </summary>
        private List<KnowledgePoint> m_KnowledgePoints;

        /// <summary>
        /// 漫游模块
        /// </summary>
        private WorkshopWalkModule module;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<WorkshopWalkModule>();
            module = ModuleManager.Instance.GetModule<WorkshopWalkModule>();

            Messager.Instance.AddListener("DressingAssetMsg", ReceiveDressingAssetMsg);
            Messager.Instance.AddListener("DeviceKnowledgePoint", ReceiveDeviceKnowledgePoint);

            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Messager.Instance.RemoveListener("DressingAssetMsg", ReceiveDressingAssetMsg);
            Messager.Instance.RemoveListener("DeviceKnowledgePoint", ReceiveDeviceKnowledgePoint);
            ModuleManager.Instance.Unregister<WorkshopWalkModule>();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            buttonBack = transform.Find("Background/TitleBar/ButtonBack").GetComponent<Button>();
            workshopMapBar = transform.Find("Background/WorkshopMapBar").GetComponent<WorkshopMapBar>();
            buttonGlobal = transform.Find("Background/MiniMapBar/Buttons/Global").GetComponent<Button>();
            dressingPathBar = transform.Find("Background/DressingPathBar").GetComponent<DressingPathBar>();
            miniDeviceInfoBar = transform.Find("Background/MiniDeviceInfoBar").GetComponent<MiniDeviceInfoBar>();
            graphicInfoPanel = transform.Find("Background/GraphicInfoPanel").GetComponent<GraphicInfoPanel>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            workshopMapBar.OnMapChanged.AddListener(workshopMapBar_OnMapChanged);
            workshopMapBar.OnPartitionChanged.AddListener(workshopMapBar_OnPartitionChanged);
            workshopMapBar.OnGraphicClick.AddListener(workshopMapBar_OnGraphicClick);
            buttonGlobal.onClick.AddListener(buttonGlobal_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
        }

        void Start()
        {
            //ToolTip.Instance.IsEnabled = true;
            MiniMapControl = GameObject.Find("生物制药车间/地图").GetComponent<MiniMapController>();
            MiniMapControl.OnGroundCollision.AddListener(MiniMapControl_OnGroundCollision);

            workshopMapBar.gameObject.SetActive(false);
            dressingPathBar.gameObject.SetActive(false);
            miniDeviceInfoBar.gameObject.SetActive(false);
            graphicInfoPanel.Hide();

            string xmlPath = Application.streamingAssetsPath + "/WorkshopWalk/KnowledgePoint.xml";
            module.InitData(xmlPath);
            m_KnowledgePoints = module.GetKnowledgePoints();
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
        /// 车间地图改变时，触发
        /// </summary>
        /// <param name="mapIndex"></param>
        private void workshopMapBar_OnMapChanged(int mapIndex)
        {
            MiniMapControl.DisplayMap(mapIndex);
        }

        /// <summary>
        /// 洁净分区改变时，触发
        /// </summary>
        /// <param name="mapIndex"></param>
        /// <param name="partName"></param>
        /// <param name="b"></param>
        private void workshopMapBar_OnPartitionChanged(int mapIndex, string partName, bool b)
        {
            MiniMapControl.DisplayPartition(mapIndex, partName, b);
        }

        /// <summary>
        /// 打开大地图按钮点击时，触发
        /// </summary>
        private void buttonGlobal_onClick()
        {
            workshopMapBar.gameObject.SetActive(true);
        }

        /// <summary>
        /// 接受更衣流程资源消息
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveDressingAssetMsg(Message message)
        {
            DressingAsset asset = message.Content as DressingAsset;
            workshopMapBar.SwitchMap(asset.mapIndex);
            dressingPathBar.EnterDressingProcess(asset);
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
                KnowledgePoint point = m_KnowledgePoints.First(x => x.Name == name);
                //miniDeviceInfoBar.SetDeviceKnowledgePoint(point);
            }
        }

        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            //厂区漫游场景信息
            FactoryWalkSceneInfo sceneInfo = new FactoryWalkSceneInfo();
            sceneInfo.FromWorkshop = true;
            SceneLoader.Instance.LoadSceneAsync(SceneType.FactoryWalkScene, sceneInfo);
        }

        /// <summary>
        /// 图形标记按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void workshopMapBar_OnGraphicClick(string name)
        {
            KnowledgePoint point = m_KnowledgePoints.FirstOrDefault(x => x.Name == name);

            if (point != null)
            {
                graphicInfoPanel.Show(point.Name, point.Description);
            }
        }

    }
}
