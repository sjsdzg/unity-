using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using XFramework.Component;

namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 化学合成药 漫游车间 
    /// </summary>
	public class SyntheticDrugWorkshop : BaseWorkshop {

        /// <summary>
        /// 漫游房间管理
        /// </summary>
        private RamingWorkshopManager ramingWorkshopManager;


        /// 相机模式
        /// </summary>
        private RamingCameraSwich m_CameraMode;

        /// <summary>
        /// 设备相机视角
        /// </summary>
        private MouseOrbit3D m_DeviceCamera;
        /// <summary>
        /// 车间主视角
        /// </summary>
        private FocusComponent workBestAngle;

        /// <summary>
        /// 人物
        /// </summary>
        private GameObject Player;

        /// <summary>
        /// 人物控制脚本
        /// </summary>
        private MyselfControl myselfCtrl;


        private MouseOrbit m_MainCamera;
        #region ///设备视角状态缓存
        /// <summary>
        /// 设备展示打开时的状态
        /// </summary>
        private ViewMode m_DeviceMode;
        #endregion
        protected override void OnAwake()
        {
            base.OnAwake();
            m_CameraMode = Camera.main.transform.GetComponent<RamingCameraSwich>();
            Messager.Instance.AddListener(MessageType.WorkshopOverallMsg, ReceiveOverallMsg);
            Messager.Instance.AddListener(MessageType.WorkshopDeviceMsg, ReceiveDeviceMsg);

            InitGameObject();
            InitEvent();
        }

        void InitGameObject()
        {
            ramingWorkshopManager = transform.Find("漫游房间管理器").GetComponent<RamingWorkshopManager>();
            workBestAngle = transform.Find("BestAngle").GetComponent<FocusComponent>();
            Player = GameObject.FindGameObjectWithTag("Player");
            myselfCtrl = Player.GetComponent<MyselfControl>();
            m_DeviceCamera = GameObject.Find("Cameras/UnitCamera").GetComponent<MouseOrbit3D>();
            m_MainCamera = Camera.main.GetComponent<MouseOrbit>();
            Player.gameObject.SetActive(false);

        }
        void InitEvent()
        {
            ramingWorkshopManager.OnInterOrExterEvent.AddListener(roomTriggerCtrl_OnInterOrExterEvent);
            ramingWorkshopManager.OnClickWorkshop.AddListener(ramingWorkshopManager_OnClickWorkshop);
        }
        private void Start()
        {
            this.Invoke(0.5f,()=> {
                workBestAngle.Focus();
            });
            m_CameraMode.Switch(ViewMode.Overlook);

        }
        protected override void OnRelease()
        {
            base.OnRelease();
            Messager.Instance.RemoveListener(MessageType.WorkshopOverallMsg, ReceiveOverallMsg);
            Messager.Instance.RemoveListener(MessageType.WorkshopDeviceMsg, ReceiveDeviceMsg);

        }

        /// <summary>
        /// 接受设备展示参数
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveDeviceMsg(Message message)
        {
            RamingUI2DeviceMsg msg = message.Content as RamingUI2DeviceMsg;
            if(msg.IsShowDevice)
            {
                ///主相机控制取消
                m_MainCamera.enabled = false;
                m_DeviceCamera.Reset();
            }
            else
            {
                ///如果是 Raming 不需要打开MouseOrBit
                if (m_CameraMode.CurrentMode== ViewMode.Overlook)
                {
                    ///主相机控制开启
                    m_MainCamera.enabled = true;
                }
                ///如果是 overall  则需要打开 mouseOrBit
            }
        }

        /// <summary>
        ///总体介绍消息接受
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveOverallMsg(Message message)
        {
            EnterWorkshopArgs enterArg = message.Content as EnterWorkshopArgs;

            m_CameraMode.Switch(ViewMode.Overlook);
            FocusTarget(enterArg.Name);
        }

        /// <summary>
        /// 看向目标 
        /// </summary>
        /// <param name="target"></param>
        private void FocusTarget(string target)
        {
            BestAngle bestAngle = null;
            if (!string.IsNullOrEmpty(target))
            {
                bestAngle = ramingWorkshopManager.GetRoomObject(target).GetComponentInChildren<BestAngle>();
            }
            if (bestAngle != null)
                bestAngle.Enter();
        }


        /// <summary>
        /// 接受 进出房间事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void roomTriggerCtrl_OnInterOrExterEvent(GameObject sender,RoomTriggerArg arg)
        {
            Message msg = new Message(MessageType.WorkshopInterOrExterMsg,this,arg);

            Messager.Instance.SendMessage(msg);

        }

        /// <summary>
        /// 房间点击事件
        /// </summary>
        /// <param name="name"></param>
        private void ramingWorkshopManager_OnClickWorkshop(string name)
        {
            EnterWorkshopArgs enterArg = new EnterWorkshopArgs();
            enterArg.Name = name;
            enterArg.isShowEnterButton = true;
            enterArg.EnterAction = () =>
            {
                Player.gameObject.SetActive(true);

                ///人物移动
                Transform pos = ramingWorkshopManager.roomList.FirstOrDefault(x => x.Name == name).InitialPosition;
                ///相机变换
                m_CameraMode.Switch(ViewMode.Roaming);
                MovePlayer(pos);
            };
            enterArg.ExitAction = () =>
            {
                ///相机变换
                m_CameraMode.Switch(ViewMode.Overlook);
                workBestAngle.Focus();
                Player.gameObject.SetActive(false);
                ///房间退出
                ramingWorkshopManager.SetRoomInfo(this.gameObject, new RoomTriggerArg(name, false));
            };
            ///房间变换
            ramingWorkshopManager.SetRoomInfo(this.gameObject, new RoomTriggerArg(name, true));
            Messager.Instance.SendMessage(new Message(MessageType.WorkshopOverallMsg, this, enterArg));
        }

        public override EnumWorkshopType GetWorkshopType()
        {
            throw new NotImplementedException();
        }


        private void MovePlayer(Transform pos)
        {
            Player.transform.position = pos.position;
            Player.transform.rotation = pos.rotation;
            Camera.main.transform.rotation = pos.rotation;
        }
    }
}