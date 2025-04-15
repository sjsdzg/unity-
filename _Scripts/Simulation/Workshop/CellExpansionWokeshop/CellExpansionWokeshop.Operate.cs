using BestHTTP.SocketIO;
using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Actions;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 细胞扩增车间--方法实现
    /// </summary>
    public partial class CellExpansionWokeshop
    {
        List<GameObject> objs = new List<GameObject>();
        private void InitializeOperate()
        {
            doorLeft.TriggerAction(EventTriggerType.PointerClick, DoorLeft_PointerClick);
            doorRight.TriggerAction(EventTriggerType.PointerClick, DoorRight_PointerClick);
            waterPot.TriggerAction(EventTriggerType.Drop, WaterPot_Drop);
            foreach (Transform item in mediumBottleParent)
            {
                item.TriggerAction(EventTriggerType.PointerClick, MediumBottlet_PointerClick);
                objs.Add(item.gameObject);
            }
            glassDoor.TriggerAction(EventTriggerType.PointerClick, GlassDoor_PointerClick);
            goodPutArea.TriggerAction(EventTriggerType.Drop, GoodPutArea_Drop);
            purpleSwitch.TriggerAction(EventTriggerType.PointerClick, PurpleSwitch_PointerClick);
            lightSwitch.TriggerAction(EventTriggerType.PointerClick, LightSwitch_PointerClick);
            windMachineSwitch.TriggerAction(EventTriggerType.PointerClick, WindMachineSwitch_PointerClick);
            waterPotBottle.TriggerAction(EventTriggerType.PointerClick, WaterPotBottle_PointerClick);
            windowDoor.TriggerAction(EventTriggerType.PointerClick, WindowDoor_PointerClick);
            windowDoorBox.TriggerAction(EventTriggerType.PointerClick, WindowDoorBox_PointerClick);
            deskBoxPutArea.TriggerAction(EventTriggerType.Drop, DeskBoxPutArea_Drop);
            deskBox.TriggerAction(EventTriggerType.PointerClick, DeskBox_PointerClick);
            centrifugeCap.TriggerAction(EventTriggerType.PointerClick, CentrifugeCap_PointerClick);
            centrifugeCap_Inner.TriggerAction(EventTriggerType.PointerClick, CentrifugeCap_Inner_PointerClick);
            centrifugeStartButton.TriggerAction(EventTriggerType.PointerClick, CentrifugeStartButton_PointerClick);
            gun_5ml.TriggerAction(EventTriggerType.PointerClick, Gun_5ml_PointerClick);
            gun_1ml.TriggerAction(EventTriggerType.PointerClick, Gun_1ml_PointerClick);
            centrifugeTube.TriggerAction(EventTriggerType.PointerClick, CentrifugeTube_PointerClick);
            greaterCentrifugeCap.TriggerAction(EventTriggerType.PointerClick, GreaterCentrifugeCap_PointerClick);
            greaterCentrifugeRotateCap.TriggerAction(EventTriggerType.PointerClick, GreaterCentrifugeRotateCap_PointerClick);
            greaterCentrifugeRotate.TriggerAction(EventTriggerType.Drop, GreaterCentrifugeRotate_Drop);
            greaterCentrifugeButton.TriggerAction(EventTriggerType.PointerClick, GreaterCentrifugeButton_PointerClick);
            bioMate.TriggerAction(EventTriggerType.PointerClick, BioMate_PointerClick);
            smallRockBottl.TriggerAction(EventTriggerType.PointerClick, SmallRockBottl_PointerClick);
            rockDoor.TriggerAction(EventTriggerType.PointerClick, RockDoor_PointerClick);
            rockPallent.TriggerAction(EventTriggerType.Drop, RockPallent_Drop);
            rock_SmallBottle.TriggerAction(EventTriggerType.PointerClick, Rock_SmallBottle_PointerClick);
            rockStartButton.TriggerAction(EventTriggerType.PointerClick, RockStartButton_PointerClick);
            bigRockBottl.TriggerAction(EventTriggerType.PointerClick, BigRockBottl_PointerClick);
            rock_BigBottle.TriggerAction(EventTriggerType.PointerClick, Rock_BigBottle_PointerClick);
            passWindowDoor.TriggerAction(EventTriggerType.PointerClick, PassWindowDoor_PointerClick);
            passWindow_PutArea.TriggerAction(EventTriggerType.Drop, PassWindow_PutArea_Drop);

            ObservationPointManager.Instance.OnFind.AddListener(ObservationPointManager_OnFind);
            this.Invoke(0.2f, () =>
            {
                Task.NewTask()
                    .Append(new InvokeCurrentGuideAction(1))
                    .Append(new InvokeFlashAction(true, doorRight.gameObject,doorLeft.gameObject))
                    .Execute();
            });
        }

        private void ObservationPointManager_OnFind(string name)
        {
           // LookPointManager.Instance.Enter(name);
            //FocusComponent m_FocusComponent = ObservationPointManager.Instance.GetFocusComponent(name);
            //if (m_FocusComponent != null)
            //{
            //    m_CameraSwitcher.Switch(CameraStyle.Look);
            //    m_FocusComponent.Focus();
            //}
        }
        /// <summary>
        /// 放入500ml摇瓶  3-3-14
        /// </summary>
        /// <param name="arg0"></param>
        private void PassWindow_PutArea_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-13", true))
                .Append(new CheckSmallAction("3-3-14", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_DYP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DYP_01.ToString(), UpdateType.Remove))
                .Append(new InvokeFlashAction(false, passWindow_PutArea.gameObject))
                .Append(new GameObjectAction(passWindowRockObj))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入摇瓶", Color.green))
                .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, 0), 1))
                .Append(new DOLocalRotaAction(passWindowDoorHand, new Vector3(0, 0, 0), 1))
                .Append(new InvokeCompletedAction(3,3))
                .Execute();
        }
        /// <summary>
        /// 打开传递窗  3-3-13
        /// </summary>
        /// <param name="arg0"></param>
        private void PassWindowDoor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            ///3-3-13
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-13", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, passWindowDoor.gameObject))
                .Append(new DOLocalRotaAction(passWindowDoorHand, new Vector3(0, 0, -90), 1))
                .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, 90), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开传递窗", Color.green))
                .Append(new UpdateSmallAction("3-3-13", true))
                .Append(new InvokeCurrentGuideAction(14))
                .Append(new InvokeFlashAction(true, passWindow_PutArea.gameObject))
                .Execute();
        }
        /// <summary>
        /// 取走摇瓶  3-3-12
        /// </summary>
        /// <param name="arg0"></param>
        private void Rock_BigBottle_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                 .Append(new CheckSmallAction("3-3-11", true))
                 .Append(new CheckSmallAction("3-3-12", false))
                 .Append(new UpdateSmallAction("3-3-12", true))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new InvokeFlashAction(false, rock_BigBottle.gameObject))
                 .Append(new UpdateGoodsAction(GoodsType.Cell_DYP_01.ToString(), UpdateType.Add))
                 .Append(new GameObjectAction(rock_BigBottle, false))
                 .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, 0.085f, -0.4211353f), 0.5f))
                 .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, -0.15f), 0.5f))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取走摇瓶", Color.green))
                 .Append(new InvokeCurrentGuideAction(13))
                 .Append(new InvokeFlashAction(true, passWindowDoor.gameObject))
                 .Execute();
        }
        /// <summary>
        /// 取走摇瓶   3-3-7
        /// </summary>
        /// <param name="arg0"></param>
        private void BigRockBottl_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                 .Append(new CheckSmallAction("3-3-6", true))
                 .Append(new CheckSmallAction("3-3-7", false))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new UpdateGoodsAction(GoodsType.Cell_DYP_01.ToString(), UpdateType.Add))
                 .Append(new GameObjectAction(bigRockBottl, false))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取走摇瓶", Color.green))
                 .Append(new UpdateSmallAction("3-3-7", true))
                 .Append(new InvokeCurrentGuideAction(8))
                 .Append(new InvokeFlashAction(true, rockDoor.gameObject))
                 .Execute();
        }
        /// <summary>
        /// 取走摇瓶  3-3-2
        /// </summary>
        /// <param name="arg0"></param>
        private void Rock_SmallBottle_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                 .Append(new CheckSmallAction("3-3-1", true))
                 .Append(new CheckSmallAction("3-3-2", false))
                 .Append(new UpdateSmallAction("3-3-2", true))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new UpdateGoodsAction(GoodsType.Cell_XYP_01.ToString(), UpdateType.Add))
                 .Append(new GameObjectAction(rock_SmallBottle,false))
                 .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, 0.085f, -0.4211353f), 0.5f))
                 .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, -0.15f), 0.5f))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取走摇瓶", Color.green))
                 .Append(new InvokeCurrentGuideAction(3))               
                 .Execute();
        }
        /// <summary>
        /// 启动摇床    3-2-5  3-3-10
        /// </summary>
        /// <param name="arg0"></param>
        private void RockStartButton_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
               .Append(new CheckSmallAction("3-2-4", true))
               .Append(new CheckSmallAction("3-2-5", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动摇床", Color.green))
               .Append(new GameObjectAction(redLight,true))
               .Append(new DOLocalMoveAction(rockPallent,new Vector3 (0.2f, 0.082f, -0.4211353f),0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.082f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.082f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.073f, -0.4211353f), 0.25f))
               .Append(new ScreenFaderAction("在37℃条件下，按照30-40r/min转速旋转培养5天", 3,1))
               .Append(new GameObjectAction(redLight, false))
               .Append(new InvokeCompletedAction(3,2))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new UpdateSmallAction("3-2-5", true))
               .Execute();
            Task.NewTask(eventData.pointerEnter)
             .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
             .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
             .Append(new CheckSmallAction("3-3-9", true))
             .Append(new CheckSmallAction("3-3-10", false))
             .Append(new InvokeCloseAllGuideAction())
             .Append(new InvokeFlashAction(false, rockStartButton.gameObject))
             .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动摇床", Color.green))
             .Append(new GameObjectAction(redLight, true))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.082f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.155f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.186f, 0.146f, -0.4211353f), 0.25f))
             .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.2f, 0.073f, -0.4211353f), 0.25f))
             .Append(new ScreenFaderAction("在37℃条件下，按照30-40r/min转速旋转培养5天", 3, 1))
             .Append(new GameObjectAction(redLight, false))
             .Append(new InvokeCurrentGuideAction(11))
             .Append(new UpdateSmallAction("3-3-10", true))
             .Append(new InvokeFlashAction(true, rockDoor.gameObject))
             .Execute();

        }
        /// <summary>
        /// 放入摇瓶   3-2-4    3-3-9
        /// </summary>
        /// <param name="arg0"></param>
        private void RockPallent_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                .Append(new CheckSmallAction("3-2-3", true))
                .Append(new CheckSmallAction("3-2-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_XYP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_XYP_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(rock_SmallBottle))
                .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, 0.085f, -0.4211353f), 0.5f))
                .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, -0.15f), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入摇瓶", Color.green))
                .Append(new GameObjectAction(rockBottleSetPanel.gameObject))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-8", true))
                .Append(new CheckSmallAction("3-3-9", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_DYP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DYP_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(rock_BigBottle))
                .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, 0.085f, -0.4211353f), 0.5f))
                .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, -0.15f), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入摇瓶", Color.green))
                .Append(new GameObjectAction(rockBottleSetPanel.gameObject))             
                .Execute();
        }
        /// <summary>
        /// 打开摇床  3-2-3   3-3-1    3-3-8   3-3-11
        /// </summary>
        /// <param name="arg0"></param>
        private void RockDoor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            ///3-3-11 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-10", true))
                .Append(new CheckSmallAction("3-3-11", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, rockDoor.gameObject))
                .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, 0.3f), 0.5f))
                .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, -0.4f, -0.4211353f), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开摇床", Color.green))
                .Append(new InvokeCurrentGuideAction(12))
                .Append(new UpdateSmallAction("3-3-11", true))
                .Append(new InvokeFlashAction(true, rock_BigBottle.gameObject))
                .Execute();
            ///3-3-8 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-7", true))
                .Append(new CheckSmallAction("3-3-8", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, rockDoor.gameObject))
                .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, 0.3f), 0.5f))
                .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, -0.4f, -0.4211353f), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开摇床", Color.green))
                .Append(new InvokeCurrentGuideAction(9))
                .Append(new UpdateSmallAction("3-3-8", true))
                .Append(new InvokeFlashAction(true, rockStartButton.gameObject))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                .Append(new CheckSmallAction("3-2-2", true))
                .Append(new CheckSmallAction("3-2-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, 0.3f), 0.5f))
                .Append(new DOLocalMoveAction(rockPallent,new Vector3 (0.1828086f, -0.4f, -0.4211353f),0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开摇床", Color.green))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new UpdateSmallAction("3-2-3", true))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, 0.3f), 0.5f))
               .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, -0.4f, -0.4211353f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开摇床", Color.green))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new UpdateSmallAction("3-3-1", true))
               .Execute();
        }
        /// <summary>
        /// 取走摇瓶  3-2-1
        /// </summary>
        /// <param name="arg0"></param>
        private void SmallRockBottl_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                .Append(new CheckSmallAction("3-2-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(smallRockBottl,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_XYP_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取走摇瓶", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new UpdateSmallAction("3-2-1",true))
                .Append(new InvokeFlashAction(true, glassDoor.gameObject))
                .OnCompleted(() =>
                 {
                     glassDoor.GetComponent<BoxCollider>().enabled = true;
                 })
                .Execute();
        }
        /// <summary>
        /// 点击移取培养基   3-1-17       点击移取细胞液  3-3-5    点击移取培养基  3-3-6
        /// </summary>
        /// <param name="arg0"></param>
        private void BioMate_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            ///  3-1-17
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-16", true))
                .Append(new CheckSmallAction("3-1-17", false))
                .Append(new UpdateSmallAction("3-1-17", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, bioMate.gameObject))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
                .Append(new DOLocalMoveAction(bioMate,new Vector3 (-0.0061f, -0.0055f, 0.2615f),0.5f))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 20, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, false))
                .Append(new GameObjectAction(bioMateNew))

                .Append(new DOLocalRotaAction(singlePipetteBag_01,new Vector3 (0,0,180),0.5f))
                .Append(new GameObjectAction(pipette_01,true))
                .Append(new GameObjectAction(singlePipetteSelf_01, false))
                .Append(new DOLocalMoveAction(pipette_01, new Vector3(-39.7319f, 8.2782f, -50.9078f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_01, new Vector3(-40.00143f, 8.5491f, -50.7495f), 0.5f))
                .Append(new DOLocalRotaAction(pipette_01,new Vector3(90,90,0),0.5f))
                .Append(new DOLocalMoveAction(pipette_01, new Vector3(-40.00143f, 8.5491f, -50.9769f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_01, new Vector3(-40.00143f, 8.597f, -50.9769f), 0.5f))
                .Append(new GameObjectAction(pipette_01, false))
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_01,new Vector3 (-39.495f, 8.346f, -50.908f),0.5f))
                .Append(new DOLocalMoveAction(singlePipette_01, new Vector3(-39.459f, 8.346f, -50.252f), 0.5f,true))
                .Append(new DOLocalRotaAction(singlePipette_01,new Vector3 (0,0,90),0.5f))
                .Append(new DOLocalMoveAction(singlePipette_01, new Vector3(-39.421f, 7.715f, -50.252f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

                 //电动移液器吸取培养基液体
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(180, -90, -90), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.193f), 0.5f))
                .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.2f, 0.5f,true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0.6f, 0.5f))

                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.523f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.523f, 0.193f), 0.5f))
                .Append(new CoroutineFluidLevelAction(smallRockBottlFlu.gameObject, 0.3f, 0.5f,true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0f, 0.5f))

                ///移液管01丢垃圾桶
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.523f, 0.375f), 0.5f))
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject,false))
                .Append(new GameObjectAction(wasterPipette01.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette01, new Vector3(-39.90438f, 8.458f, -51.289f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette01, new Vector3(-39.393f, 8.458f, -50.222f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette01, new Vector3(-39.492f, 7.739f, -50.309f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.0049f, 0.26f), 0.5f))
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(90, 0, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, true))
                .Append(new GameObjectAction(bioMateNew,false))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 0, 0), 0.5f))
                .Append(new DOLocalMoveAction(bioMate, new Vector3(0.009054688f, -0.005546875f, 0.0039375f), 0.5f))

                 //小摇瓶盖子归位             
                .Append(new DOLocalRotaAction(smallRockBottlCap, new Vector3(-90f, 0f, 90f), 0.5f,true))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.0323f), 0.5f))

                 //培养基塞子归位
                .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 0, 0f), 0.5f,true))
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0325f, 0.0178f), 0.5f))
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))       
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0f), 0.5f))

                

                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移取培养基", Color.green))
                .Append(new InvokeCompletedAction(3,1))
                .Append(new InvokeCurrentGuideAction(1))
                .Append(new InvokeFlashAction(true, smallRockBottl.gameObject))
                .OnCompleted(() =>
                {
                })
                .Execute();
            ///  3-3-5
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-4", true))
                .Append(new CheckSmallAction("3-3-5", false))
                .Append(new CheckSmallAction("点击移取细胞液", false))
                .Append(new UpdateSmallAction("点击移取细胞液", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, bioMate.gameObject))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
                .Append(new DOLocalMoveAction(bioMate, new Vector3(-0.0061f, -0.0055f, 0.2615f), 0.5f))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 20, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, false))
                .Append(new GameObjectAction(bioMateNew))

                .Append(new DOLocalRotaAction(singlePipetteBag_02, new Vector3(0, 0, 180), 0.5f))
                .Append(new GameObjectAction(pipette_02, true))
                .Append(new GameObjectAction(singlePipetteSelf_02, false))
                .Append(new DOLocalMoveAction(pipette_02, new Vector3(-39.7319f, 8.2782f, -50.8642f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_02, new Vector3(-40.00143f, 8.5491f, -50.7495f), 0.5f))
                .Append(new DOLocalRotaAction(pipette_02, new Vector3(90, 90, 0), 0.5f))
                .Append(new DOLocalMoveAction(pipette_02, new Vector3(-40.00143f, 8.5491f, -50.9769f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_02, new Vector3(-40.00143f, 8.597f, -50.9769f), 0.5f))
                .Append(new GameObjectAction(pipette_02, false))
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_02, new Vector3(-39.495f, 8.346f, -50.908f), 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_02, new Vector3(-39.459f, 8.346f, -50.252f), 0.5f, true))
                .Append(new DOLocalRotaAction(singlePipette_02, new Vector3(0, 0, 90), 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_02, new Vector3(-39.459f, 7.715f, -50.252f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

                  //小摇瓶盖子打开
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(180, -90, -90), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalRotaAction(smallRockBottlCap, new Vector3(-90f, 0f, -90f), 0.5f, true))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, -0.168f), 0.5f))
                //从小摇瓶里吸取液体
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.3382f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.3382f, 0.155f), 0.5f))
                .Append(new CoroutineFluidLevelAction(smallRockBottlFlu.gameObject, 0f, 0.5f, true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0.6f, 0.5f))

                ///从大摇瓶里倒入液体
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.145f, 0.3382f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bigRockBottl, new Vector3(-39.9132f, 8.508017f, -51.565f), 0.5f))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(0.0026f, 0f, 0.113f), 0.5f))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(-0.127f, 0f, 0.113f), 0.5f))
                .Append(new DOLocalRotaAction(bigRockBottlCap, new Vector3(-90f, 0f, -90f), 0.5f, true))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(-0.127f, 0f, -0.237f), 0.5f))

                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.45f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.308f), 0.5f))
                .Append(new CoroutineFluidLevelAction(bigRockBottlFlu.gameObject, 0.12f, 0.5f, true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0f, 0.5f))
               .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.45f), 0.5f))

               //移液管02丢入垃圾桶
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject, false))
                .Append(new GameObjectAction(wasterPipette02.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette02, new Vector3(-39.90438f, 8.458f, -51.289f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette02, new Vector3(-39.393f, 8.458f, -50.222f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette02, new Vector3(-39.459f, 7.715f, -50.252f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))


                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.0049f, 0.26f), 0.5f))
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(90, 0, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, true))
                .Append(new GameObjectAction(bioMateNew, false))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 0, 0), 0.5f))
                .Append(new DOLocalMoveAction(bioMate, new Vector3(0.009054688f, -0.005546875f, 0.0039375f), 0.5f))

               //小摇瓶盖子归位
                .Append(new DOLocalRotaAction(smallRockBottlCap, new Vector3(-90f, 0f, 90f), 0.5f, true))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.087f), 0.5f))
                .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.0323f), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移取细胞液", Color.green))
                .Append(new UpdateSmallAction("3-3-5", true))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new InvokeFlashAction(true, bioMate.gameObject))
                .OnCompleted(() =>
                {
                })
                .Execute();
            ///  3-3-6
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-5", true))
                .Append(new CheckSmallAction("3-3-6", false))
                .Append(new CheckSmallAction("电动移液器移取培养基", false))
                .Append(new UpdateSmallAction("电动移液器移取培养基", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, bioMate.gameObject))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))


                .Append(new DOLocalMoveAction(bioMate, new Vector3(-0.0061f, -0.0055f, 0.2615f), 0.5f))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 20, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, false))
                .Append(new GameObjectAction(bioMateNew))              

                .Append(new DOLocalRotaAction(singlePipetteBag_03, new Vector3(0, 0, 180), 0.5f))
                .Append(new GameObjectAction(pipette_03, true))
                .Append(new GameObjectAction(singlePipetteSelf_03, false))
                .Append(new DOLocalMoveAction(pipette_03, new Vector3(-39.7319f, 8.2782f, -50.8642f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_03, new Vector3(-40.00143f, 8.5491f, -50.7495f), 0.5f))
                .Append(new DOLocalRotaAction(pipette_03, new Vector3(90, 90, 0), 0.5f))
                .Append(new DOLocalMoveAction(pipette_03, new Vector3(-40.00143f, 8.5491f, -50.9769f), 0.5f))
                .Append(new DOLocalMoveAction(pipette_03, new Vector3(-40.00143f, 8.597f, -50.9769f), 0.5f))
                .Append(new GameObjectAction(pipette_03, false))
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_03, new Vector3(-39.495f, 8.346f, -50.8178f), 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_03, new Vector3(-39.459f, 8.346f, -50.252f), 0.5f, true))
                .Append(new DOLocalRotaAction(singlePipette_03, new Vector3(0, 0, 90), 0.5f))
                .Append(new DOLocalMoveAction(singlePipette_03, new Vector3(-39.459f, 7.715f, -50.252f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))


               //培养基塞子飞起
               //.Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.1589f, 8.4065f, -51.565f), 1))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))       
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0325f, 0.0178f), 0.5f))
               .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 180, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0325f, -0.0521f), 0.5f))
                 //电动移液器吸取培养基液体
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(180, -90, -90), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.375f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.193f), 0.5f))
                .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.15f, 0.5f, true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject,0.6f, 0.5f))
               .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.458f, 0.375f), 0.5f))
                ///从大摇瓶里倒入液体
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.45f), 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.308f), 0.5f))
                .Append(new CoroutineFluidLevelAction(bigRockBottlFlu.gameObject, 0.2f, 0.5f, true))
                .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0f, 0.5f))
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.1376f, 0.5558f, 0.45f), 0.5f))
                //移液管03丢垃圾桶
                .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject, false))
                .Append(new GameObjectAction(wasterPipette03.gameObject))
                .Append(new InvokeCameraLookPointAction("观察垃圾桶", 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette03, new Vector3(-39.90438f, 8.458f, -51.289f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette03, new Vector3(-39.393f, 8.458f, -50.222f), 0.5f))
                .Append(new DOLocalMoveAction(wasterPipette03, new Vector3(-39.375f, 7.715f, -50.252f), 0.5f))
                .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               //电动移液器归位
                .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.0049f, 0.26f), 0.5f))
                .Append(new DOLocalRotaAction(bioMateNew, new Vector3(90, 0, 0), 0.5f))
                .Append(new GameObjectAction(bioMate, true))
                .Append(new GameObjectAction(bioMateNew, false))
                .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 0, 0), 0.5f))
                .Append(new DOLocalMoveAction(bioMate, new Vector3(0.009054688f, -0.005546875f, 0.0039375f), 0.5f))

                 //培养基塞子归位
                .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 0, 0f), 0.5f, true))
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0325f, 0.0178f), 0.5f))
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))
                .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0f), 0.5f))
                .Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.115f, 8.4644f, -51.684f), 0.5f))

                //大摇瓶盖子归位
                .Append(new DOLocalRotaAction(bigRockBottlCap, new Vector3(-90f, 0f, 90f), 0.5f,true))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(-0.127f, 0f, 0.113f), 0.5f))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(0.0026f, 0f, 0.113f), 0.5f))
                .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(0.0026f, 0f, 0.0505f), 0.5f))

                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移取培养基", Color.green))
                .Append(new UpdateSmallAction("3-3-6", true))
                .Append(new InvokeCurrentGuideAction(7))
                .Append(new InvokeFlashAction(true, bigRockBottl.gameObject))
                .OnCompleted(() =>
                {
                })
                .Execute();
        }
        /// <summary>
        /// 开始离心  3-1-11
        /// </summary>
        /// <param name="arg0"></param>
        private void GreaterCentrifugeButton_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-10", true))
                .Append(new CheckSmallAction("3-1-11", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, greaterCentrifugeButton.gameObject))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已开始离心", Color.green))
                .Append(new DelayedAction(1))
                .Append(new ScreenFaderAction("离心2min后，离心管取出", 2, 1))
                .Append(new UpdateGoodsAction(GoodsType.Cell_LXG_01.ToString(), UpdateType.Add))
                .Append(new GameObjectAction(goodPutArea))
                .Append(new UpdateSmallAction("3-1-11", true))
                .Append(new InvokeCurrentGuideAction(12))
                .Append(new GameObjectAction(lookPointTwo,false))
                .Append(new InvokeFlashAction(true, goodPutArea.gameObject))
                .Execute();
        }
        /// <summary>
        /// 放入15ml离心管   3-1-9
        /// </summary>
        /// <param name="arg0"></param>
        private void GreaterCentrifugeRotate_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-8", true))
               .Append(new CheckSmallAction("3-1-9", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData, GoodsType.Cell_LXG_01))
               .Append(new UpdateGoodsAction(GoodsType.Cell_LXG_01.ToString(), UpdateType.Remove))
               .Append(new InvokeFlashAction(false, greaterCentrifugeRotate.gameObject))
               .Append(new GameObjectAction(greaterCentrifugeTube_01))
               .Append(new DOLocalRotaAction(greaterCentrifugeTube_01,new Vector3 (0,-30,0),0.5f))
               .Append(new DOLocalMoveAction(greaterCentrifugeTube_01,new Vector3 (0.178669f,0, 0.1610489f),1))
               .Append(new GameObjectAction(greateCentrifugeTube_02))
               .Append(new DOLocalRotaAction(greateCentrifugeTube_02, new Vector3(0, 15, 0), 0.5f))
               .Append(new DOLocalMoveAction(greateCentrifugeTube_02, new Vector3(-0.0226f, 0, 0.1621f), 1))
               .Append(new DOLocalMoveAction(greaterCentrifugeRotateCap, new Vector3(0.08f, 0, 0.1f), 1))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入15ml离心管", Color.green))
               .Append(new GameObjectAction(bigCentrifugeSetPanel.gameObject))
               .Execute();
        }
        /// <summary>
        /// 打开转子盖子  3-1-8
        /// </summary>
        /// <param name="arg0"></param>
        private void GreaterCentrifugeRotateCap_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-7", true))
                .Append(new CheckSmallAction("3-1-8", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, greaterCentrifugeRotateCap.gameObject))
                .Append(new DOLocalMoveAction(greaterCentrifugeRotateCap, new Vector3(0.08f, 0, 0.8f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开转子盖子", Color.green))
                .Append(new UpdateSmallAction("3-1-8", true))
                .Append(new InvokeCurrentGuideAction(9))
                .Append(new InvokeFlashAction(true, greaterCentrifugeRotate.gameObject))
                .Execute();
        }
        /// <summary>
        /// 打开大型离心机盖子    3-1-7   关闭离心机盖子  3-1-10
        /// </summary>
        /// <param name="arg0"></param>
        private void GreaterCentrifugeCap_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-6", true))
                .Append(new CheckSmallAction("3-1-7", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, greaterCentrifugeCap.gameObject))
                .Append(new DOLocalRotaAction(greaterCentrifugeCap,new Vector3 (90,0,0),1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开离心机盖子", Color.green))
                .Append(new UpdateSmallAction("3-1-7", true))
                .Append(new InvokeCurrentGuideAction(8))
                .Append(new InvokeFlashAction(true, greaterCentrifugeRotateCap.gameObject))
                .Execute();

            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-9", true))
               .Append(new CheckSmallAction("3-1-10", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, greaterCentrifugeCap.gameObject))
               .Append(new DOLocalRotaAction(greaterCentrifugeCap, new Vector3(0, 0, 0), 1))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭离心机盖子", Color.green))
               .Append(new UpdateSmallAction("3-1-10", true))
               .Append(new InvokeCurrentGuideAction(11))
               .Append(new GameObjectAction(lookPointTwo))
               .Append(new InvokeFlashAction(true, greaterCentrifugeButton.gameObject))
               .Execute();
        }
        /// <summary>
        /// 拾取离心管   3-1-6
        /// </summary>
        /// <param name="arg0"></param>
        private void CentrifugeTube_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-5", true))
                .Append(new CheckSmallAction("3-1-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(centrifugeTube,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_LXG_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拾取离心管", Color.green))
                .Append(new UpdateSmallAction("3-1-6", true))
                .Append(new InvokeCurrentGuideAction(7))
                .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.3f, 0.5f))
                .Execute();
        }
        /// <summary>
        /// 点击装枪    3-1-1    点击去冻存上清  3-1-2     点击细胞吹打  3-1-3    点击细胞清洗  3-1-4   点击移取培养基  3-1-5
        /// </summary>
        /// <param name="arg0"></param>
        private void Gun_1ml_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //3-1-1 
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-1", false))
               .Append(new CheckSmallAction("点击装枪", false))
               .Append(new UpdateSmallAction("点击装枪", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_1ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               //酒精灯点火
               .Append(new DOLocalMoveAction(alcoholCap, new Vector3(-0.0096f, 0.006f, 0.089f), 0.5f))
               .Append(new DOLocalMoveAction(alcoholCap, new Vector3(-0.0096f, -0.0528f, 0.089f), 0.5f))
               .Append(new DOLocalMoveAction(alcoholCap, new Vector3(-0.0096f, -0.0528f, -0.042f), 0.5f))
               .Append(new GameObjectAction(alcoholFire))

               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0.175f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.26f, 8.723f, -50.9422f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.723f, -51.3783f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.43f, -51.3783f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.723f, -51.206f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.006f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成装枪", Color.green))

               .Append(new UpdateSmallAction("3-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
               .Execute();
            //3-1-2 
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-1", true))
               .Append(new CheckSmallAction("3-1-2", false))
               .Append(new CheckSmallAction("去冻存上清", false))
               .Append(new UpdateSmallAction("去冻存上清", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_1ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, 0f, 0.057f), 0.5f))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, -0.037f, 0.057f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeCap, new Vector3(0f, 180, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, -0.037f, -0.0166f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(45, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))//枪杆按下去

               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-50, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.3994f, -51.2801f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f))// //吸取上清液
               .Append(new GameObjectAction(deskTubeLiquid_White, false))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(0, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2196f, 8.732f, -50.693f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2196f, 8.5f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))//倒废弃液
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2196f, 8.732f, -50.693f), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml, false))
               .Append(new GameObjectAction(wasterNeedle_01, true))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
             // .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0779f, 8.732f, -51.39f), 0.5f))

               //第二次装枪
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0.175f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.723f, -51.3884f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.43f, -51.3884f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0715f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))

               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.006f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成冻存上清", Color.green))
               .Append(new UpdateSmallAction("3-1-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
               .Execute();
            //3-1-3 
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-2", true))
               .Append(new CheckSmallAction("3-1-3", false))
               .Append(new CheckSmallAction("细胞吹打", false))
               .Append(new UpdateSmallAction("细胞吹打", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_1ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               .Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.0425f, 8.4644f, -51.4609f), 0.5f))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))       //培养基塞子飞起
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0252f, 0.0178f), 0.5f))
               .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 180, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0252f, -0.0521f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))

               //移液枪飞到培养基瓶上方
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.5f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.45f, 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(45, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-50, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.3994f, -51.2801f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))
               .Append(new GameObjectAction(deskTubeLiquid_Yellow, true))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f))
               .Append(new GameObjectAction(deskTubeLiquid_Yellow, false))
               // .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))
               //离心管登场
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-40.00639f, 8.542f, -51.8f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-39.9333f, 8.542f, -51.4101f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-39.9333f, 8.380333f, -51.4101f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeCap, new Vector3(0f, 180f, 0f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, -0.105f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(0, 0, 0), 0.5f))



               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.4059f, -51.2943f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.15f, 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))
                //丢弃针头
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml, false))
               .Append(new GameObjectAction(wasterNeedle_02, true))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0779f, 8.732f, -51.39f), 0.5f))

                //第三次装枪
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0.175f, 0.102f), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.723f, -51.4004f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.43f, -51.4004f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.723f, -51.4004f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.006f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))

               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成细胞吹打", Color.green))
               .Append(new UpdateSmallAction("3-1-3", true))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
               .Execute();
            //3-1-4  细胞清洗
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-3", true))
               .Append(new CheckSmallAction("3-1-4", false))
               .Append(new CheckSmallAction("细胞清洗", false))
               .Append(new UpdateSmallAction("细胞清洗", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_1ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.5f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.4f, 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(45, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-50, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.3994f, -51.2801f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))
               .Append(new GameObjectAction(deskTubeLiquid_Yellow, true))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f))
               .Append(new GameObjectAction(deskTubeLiquid_Yellow, false))

               //离心管登场
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0141f, 8.5474f, -51.1546f), 0.5f))
               .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(0, 0, 0), 0.5f))
              
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.4059f, -51.2943f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.3f, 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))  //丢弃针头
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml, false))
               .Append(new GameObjectAction(wasterNeedle_03, true))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0779f, 8.732f, -51.39f), 0.5f))

                //第四次装枪
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0.175f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.732f, -51.4119f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.43f, -51.4119f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.2515f, 8.732f, -51.4119f), 1f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.102f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap_01, new Vector3(0f, 0f, 0.006f), 0.5f))

               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成细胞清洗", Color.green))
               .Append(new UpdateSmallAction("3-1-4", true))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
               .Execute();
            //3-1-5  点击移取培养基
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-4", true))
               .Append(new CheckSmallAction("3-1-5", false))
               .Append(new CheckSmallAction("移取培养基", false))
               .Append(new UpdateSmallAction("移取培养基", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_1ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.5f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.35f, 0.5f))


               //离心管登场
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.4059f, -51.2943f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.08f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.4f, 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeCap, new Vector3(0f, 0f, 0f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0f), 0.5f))

               .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 0, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0325f, 0.0178f), 0.5f))    //培养基塞子复位
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0f), 0.5f))
               .Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.115f, 8.4644f, -51.684f), 0.5f))  //培养基瓶复位

               .Append(new DOLocalRotaAction(deskTubeCap, new Vector3(0f, 0, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, -0.037f, -0.0166f), 0.5f))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, -0.037f, 0.057f), 0.5f))
               .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, 0f, 0.012f), 0.5f))
               .Append(new DOLocalMoveAction(deskTube, new Vector3(-39.884f, 8.2974f, -51.369f), 0.5f))
               .Append(new DOLocalMoveAction(deskTube, new Vector3(-39.869f, 8.2974f, -51.815f), 0.5f))    //冻存管移到一边

               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))  //丢弃针头
               .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_1ml, false))
               .Append(new GameObjectAction(wasterNeedle_04, true))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.26f, 8.723f, -50.9422f), 0.5f))
               .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.26f, 8.427f, -50.9422f), 0.5f))

               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成移取培养基", Color.green))
               .Append(new UpdateSmallAction("3-1-5", true))
               .Append(new InvokeCurrentGuideAction(6))
               .Append(new InvokeFlashAction(true, greaterCentrifugeCap.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }
        /// <summary>
        /// 点击装枪    3-1-1   3-1-13          点击去离心上清   3-1-14   点击去细胞重悬  3-1-15   点击细胞清洗  3-1-16
        /// </summary>
        /// <param name="arg0"></param>
        private void Gun_5ml_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-1-16   点击细胞清洗
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-15", true))
               .Append(new CheckSmallAction("3-1-16", false))
               .Append(new CheckSmallAction("点击细胞清洗", false))
               .Append(new UpdateSmallAction("点击细胞清洗", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_5ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.56f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.25f, 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))

               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))  //倒入离心管
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.4531f, -51.2285f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f,true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.3f, 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f)) //再吸再打
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f, true))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true)) //再吸
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0f, 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))
               //倒入小摇瓶
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.732f, -51.53f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.58f, -51.53f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f,true))
               .Append(new CoroutineFluidLevelAction(smallRockBottlFlu.gameObject, 0.07f, 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.732f, -51.53f), 0.5f))
               //离心管盖子归位
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeCap, new Vector3(0f, 0f, 0f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-39.9333f, 8.7f, -51.4101f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-40.00639f, 8.7f, -51.8f), 0.5f))
               .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-40.00639f, 8.380333f, -51.8f), 0.5f))

               //丢弃针头
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml, false))
               .Append(new GameObjectAction(wasterNeedle_07, true))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.26f, 8.438686f, -51.0588f), 0.5f))
            
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成细胞清洗", Color.green))
               .Append(new UpdateSmallAction("3-1-16", true))
               .Append(new InvokeCurrentGuideAction(17))
               .Append(new InvokeFlashAction(true, bioMate.gameObject))
               .Execute();
            // 3-1-15   点击去细胞重悬
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-14", true))
               .Append(new CheckSmallAction("3-1-15", false))
               .Append(new CheckSmallAction("去细胞重悬", false))
               .Append(new UpdateSmallAction("去细胞重悬", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_5ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))

               .Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.0425f, 8.4644f, -51.4609f), 0.5f))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0.0178f), 0.5f))       //培养基塞子飞起
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0252f, 0.0178f), 0.5f))
               .Append(new DOLocalRotaAction(deskBottleCap, new Vector3(0f, 180, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, -0.0252f, -0.0521f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.56f, -51.463f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject, 0.3f, 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.028f, 8.732f, -51.463f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))  //倒入离心管
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.4531f, -51.2285f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f,true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0.3f, 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f)) //再吸再打
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true)) //再吸
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0f, 0.5f))
               //小摇瓶登场
               .Append(new DOLocalMoveAction(smallRockBottl, new Vector3(-39.9053f, 8.430017f, -51.575f), 0.5f))
               .Append(new DOLocalMoveAction(smallRockBottl, new Vector3(-39.9053f, 8.430017f, -51.531f), 0.5f))
               .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.087f), 0.5f))
               .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, 0.087f), 0.5f))
               .Append(new DOLocalRotaAction(smallRockBottlCap, new Vector3(-90f, 0f, -90f), 0.5f, true))
               .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(-0.096f, 0f, -0.14f), 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.732f, -51.53f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.58f, -51.53f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f,true))
               .Append(new CoroutineFluidLevelAction(smallRockBottlFlu.gameObject, 0.05f, 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.895f, 8.732f, -51.53f), 0.5f))


               //丢弃针头
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml, false))
               .Append(new GameObjectAction(wasterNeedle_06, true))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
               // .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0779f, 8.732f, -51.39f), 0.5f))

               //装枪
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.106f, 8.388041f, -51.25261f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0.252f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.256f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.5f, -51.256f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.206f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 1))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, -0.1523f), 1))
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.28911f, 8.388041f, -51.25261f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))

               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成去细胞重悬", Color.green))
               .Append(new UpdateSmallAction("3-1-15", true))
               .Append(new InvokeCurrentGuideAction(16))
               .Append(new InvokeFlashAction(true, gun_5ml.gameObject))
               .Execute();
            // 3-1-14   去离心上清
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-13", true))
               .Append(new CheckSmallAction("3-1-14", false))
               .Append(new CheckSmallAction("去离心上清", false))
               .Append(new UpdateSmallAction("去离心上清", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_5ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0.0481f), 0.5f))    //离心管盖子打开
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, 0.0481f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeCap, new Vector3(0f, 180, 0f), 0.5f, true))
               .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(-0.0414f, 0f, -0.105f), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(45, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))

               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-35, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.4109f, -51.2885f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
               .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0f, 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-39.9231f, 8.5078f, -51.1502f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f))
               .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f))
               //倒废弃液
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.2196f, 8.732f, -50.693f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.2196f, 8.5f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.0659f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.2196f, 8.732f, -50.693f), 1f))

               //丢弃针头
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f)) 
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.5f, -50.693f), 0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml, false))
               .Append(new GameObjectAction(wasterNeedle_05, true))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0421f, 8.732f, -50.693f), 0.5f))
               .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
              // .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0779f, 8.732f, -51.39f), 0.5f))

                //装枪
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.106f, 8.388041f, -51.25261f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0.252f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.256f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.5f, -51.256f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.206f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 1))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, -0.1523f), 1))
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.28911f, 8.388041f, -51.25261f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))

               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成去离心上清", Color.green))
               .Append(new UpdateSmallAction("3-1-14", true))
               .Append(new InvokeCurrentGuideAction(15))
               .Append(new InvokeFlashAction(true, gun_5ml.gameObject))
               .Execute();
            //3-1-13   点击装枪
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-12", true))
               .Append(new CheckSmallAction("3-1-13", false))
               .Append(new CheckSmallAction("点击装枪", true))
               .Append(new UpdateSmallAction("点击装枪", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, gun_5ml.gameObject))
               .Append(new InvokeCameraLookPointAction("观察生物安全柜", 10f))
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.106f, 8.388041f, -51.25261f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0.252f, 0f, 0.066f), 0.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.26f, 8.723f, -50.7978f), 1))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.206f), 1.5f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.5f, -51.206f), 1f))
               .Append(new DelayedAction(0.5f))
               .Append(new GameObjectAction(gunNeedle_5ml))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.0715f, 8.723f, -51.206f), 1f))
               .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.072f, 8.723f, -51.206f), 0.5f))
               .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 90, 0), 0.5f))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, 0.066f), 1))
               .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, -0.1523f), 1))
               .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.28911f, 8.388041f, -51.25261f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已完成装枪", Color.green))
               .Append(new UpdateSmallAction("3-1-13", true))
               .Append(new InvokeCurrentGuideAction(14))
               .Append(new InvokeFlashAction(true, gun_5ml.gameObject))
               .Execute();

      
           
        }
        /// <summary>
        /// 开始离心   2-1-10
        /// </summary>
        /// <param name="arg0"></param>
        private void CentrifugeStartButton_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-9", true))
                .Append(new CheckSmallAction("2-1-10", false))              
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, centrifugeStartButton.gameObject))
                .Append(new ScreenFaderAction("离心3min后，冻存管取出",3,1))
                .Append(new GameObjectAction(deskBox,false))
                .Append(new GameObjectAction(windowDoorBox, true))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "离心完成，已取出冻存管", Color.green))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCG_01.ToString(), UpdateType.Add))
                .Append(new UpdateSmallAction("2-1-10", true))
                .Append(new InvokeCurrentGuideAction(11))
                .Append(new InvokeFlashAction(true, goodPutArea.gameObject))
                .Execute();
        }
        /// <summary>
        /// 放入冻存管  2-1-8
        /// </summary>
        /// <param name="arg0"></param>
        private void CentrifugeCap_Inner_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-7", true))
                .Append(new CheckSmallAction("2-1-8", false))
                .Append(new CheckSmallAction("放入冻存管", false))
                .Append(new UpdateSmallAction("放入冻存管", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, centrifugeCap_Inner.gameObject))
                .Append(new GameObjectAction(tweezers,false))
                .Append(new DOLocalMoveAction(centrifugeCap_Inner, new Vector3(0f, -0.018f, 0.3f), 1))
                .Append(new InvokeCameraLookPointAction("观察水浴锅冻存管", 1f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.56f, 8.843f, -54.408f), 1))
                .Append(new InvokeCameraLookAndMovePointAction("观察离心冻存管", 3f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-39.198f, 8.843f, -54.408f), 3))
                .Append(new InvokeCameraLookAndMovePointAction("观察离心", 1))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-39.17159f, 8.843f, -54.5121f), 1))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-39.17159f, 8.6598f, -54.5349f), 1))
                .Append(new DOLocalRotaAction(centrifuge_Tube01, new Vector3(-135, 0, 0f), 1))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-39.17159f, 8.5762f, -54.4513f), 1))
                .Append(new GameObjectAction(centrifuge_Tube02))
                .Append(new DOLocalMoveAction(centrifuge_Tube02, new Vector3(-39.17159f, 8.6598f, -54.5349f), 1))
                .Append(new DOLocalRotaAction(centrifuge_Tube02, new Vector3(-45, 0, 0f), 1))
                .Append(new DOLocalMoveAction(centrifuge_Tube02, new Vector3(-39.17159f, 8.5787f, -54.616f), 1))
                .Append(new DOLocalMoveAction(centrifugeCap_Inner, new Vector3(0f, -0.018f, -0.05f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入冻存管", Color.green))               
                .Append(new GameObjectAction(smallCentrifugeSetPanel.gameObject))
                .OnCompleted(() =>
                {
                })
               .Execute();
        }
        /// <summary>
        /// 打开离心机盖子  2-1-7   关闭离心机盖子  2-1-9
        /// </summary>
        /// <param name="arg0"></param>
        private void CentrifugeCap_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-6", true))
                .Append(new CheckSmallAction("2-1-7", false))
                .Append(new UpdateSmallAction("2-1-7", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, centrifugeCap.gameObject))
                .Append(new DOLocalRotaAction(centrifugeCap, new Vector3(0, -90, 0f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开离心机盖子", Color.green))
                .Append(new InvokeCurrentGuideAction(8))
                .Append(new InvokeFlashAction(true, centrifugeCap_Inner.gameObject))
                .Execute();

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-8", true))
                .Append(new CheckSmallAction("2-1-9", false))
                .Append(new UpdateSmallAction("2-1-9", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, centrifugeCap.gameObject))
                .Append(new DOLocalRotaAction(centrifugeCap, new Vector3(0, 0, 0f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭离心机盖子", Color.green))
                .Append(new InvokeCurrentGuideAction(10))
                .Append(new InvokeFlashAction(true, centrifugeStartButton.gameObject))
                .Execute();
        }
        /// <summary>
        /// 点击核对冻存管信息后领取   2-1-5
        /// </summary>
        /// <param name="arg0"></param>
        private void DeskBox_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-4", true))
                .Append(new CheckSmallAction("2-1-5", false))
                .Append(new CheckSmallAction("核对冻存管信息", false))
                .Append(new UpdateSmallAction("核对冻存管信息", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, deskBox.gameObject))
                .Append(new DOLocalMoveAction(deskBox_Cap,new Vector3 (0,0,0.5f),1))
                .Append(new DOLocalMoveAction(deskBox_Tube,new Vector3 (-0.06230469f, -0.03f, 0.223f),1))
                .Append(new InvokeCameraLookPointAction("观察冻存管",1f,false))
                .Append(new DelayedAction(3))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已核对冻存管并领取", Color.green))
                .Append(new GameObjectAction(deskBox_Tube,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCG_01.ToString(), UpdateType.Add))
                .Append(new DOLocalMoveAction(deskBox_Cap, new Vector3(0, 0,-0.015f), 1))
                .Append(new UpdateSmallAction("2-1-5", true))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new InvokeFlashAction(true, waterPot.gameObject))
                .OnCompleted(() =>
                {
                })
                .Execute();
        }
        /// <summary>
        /// 放置冻存盒   2-1-4
        /// </summary>
        /// <param name="arg0"></param>
        private void DeskBoxPutArea_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-3", true))
                .Append(new CheckSmallAction("2-1-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(deskBoxPutArea, false))
                .Append(new GameObjectAction(deskBox, true))
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_DCH_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCH_01.ToString(), UpdateType.Remove))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置冻存盒", Color.green))
                .Append(new UpdateSmallAction("2-1-4", true))
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new InvokeFlashAction(true, deskBox.gameObject))
                .Execute();
        }
        /// <summary>
        /// 取冻存盒  2-1-2
        /// </summary>
        /// <param name="arg0"></param>
        private void WindowDoorBox_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-1", true))
                .Append(new CheckSmallAction("2-1-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(windowDoorBox,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCH_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取冻存盒", Color.green))
                .Append(new UpdateSmallAction("2-1-2", true))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, windowDoor.gameObject))
                .Execute();
        }
        /// <summary>
        /// 打开传递窗  2-1-1     关闭传递窗  2-1-3
        /// </summary>
        /// <param name="arg0"></param>
        private void WindowDoor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;         
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, windowDoor.gameObject))
                .Append(new DOLocalRotaAction(windowDoorHand, new Vector3(0, 0, -90), 1))
                .Append(new DOLocalRotaAction(windowDoor,new Vector3 (0,0,90),1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开传递窗", Color.green))
                .Append(new UpdateSmallAction("2-1-1", true))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new InvokeFlashAction(true, windowDoorBox.gameObject))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                  .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                  .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                  .Append(new CheckSmallAction("2-1-2", true))
                  .Append(new CheckSmallAction("2-1-3", false))
                  .Append(new InvokeCloseAllGuideAction())
                  .Append(new InvokeFlashAction(false, windowDoor.gameObject))
                  .OnCompleted(() =>
                  {
                      windowDoor.GetComponent<BoxCollider>().enabled = false;
                      Task.NewTask()
                          .Append(new DOLocalRotaAction(windowDoor, new Vector3(0, 0, 0), 1))
                          .Append(new DOLocalRotaAction(windowDoorHand, new Vector3(0, 0, 0), 1))
                          .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭传递窗", Color.green))
                          .Append(new UpdateSmallAction("2-1-3", true))
                          .Append(new InvokeCurrentGuideAction(4))
                          .Append(new GameObjectAction(deskBoxPutArea))
                          .Append(new InvokeFlashAction(true, deskBoxPutArea.gameObject))
                          .Execute();
                  })
                  .Execute();
        }
        /// <summary>
        /// 点击将培养基瓶擦拭领取  1-2-8
        /// </summary>
        /// <param name="arg0"></param>
        private void WaterPotBottle_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-7", true))
                .Append(new CheckSmallAction("1-2-8", false))
                .Append(new UpdateSmallAction("1-2-8", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(true, waterPotBottle.gameObject))
                .Append(new DOLocalMoveAction(waterPotBottle,new Vector3 (-37.563f, 8.875f, -54.40446f),1))
                .Append(new GameObjectAction(washCloth))
                .Append(new DOLocalMoveAction(washCloth, new Vector3(-37.478f, 8.796f, -54.353f), 0.5f))
                .Append(new DOLocalMoveAction(washCloth, new Vector3(-37.65f, 8.796f, -54.353f), 0.5f))
                .Append(new DOLocalMoveAction(washCloth, new Vector3(-37.478f, 8.796f, -54.353f), 0.5f))
                .Append(new DOLocalMoveAction(washCloth, new Vector3(-37.65f, 8.796f, -54.353f), 0.5f))
                .Append(new GameObjectAction(washCloth,false))
                .Append(new GameObjectAction(waterPotBottle, false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYJP_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将培养基瓶擦拭领取", Color.green))
                .Append(new InvokeFlashAction(true, glassDoor.gameObject))
                .Append(new InvokeCurrentGuideAction(9))
                .Execute();
        }
        /// <summary>
        /// 打开风机  1-2-7 
        /// </summary>
        /// <param name="arg0"></param>
        private void WindMachineSwitch_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-6", true))
                .Append(new CheckSmallAction("1-2-7", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开风机", Color.green))
                .Append(new UpdateSmallAction("1-2-7", true))
                .Append(new InvokeCurrentGuideAction(8))
                .Append(new GameObjectAction(lookPointOne,false))
                .Append(new InvokeFlashAction(true, waterPotBottle.gameObject))
                .Execute();
        }
        /// <summary>
        /// 打开照明  1-2-6
        /// </summary>
        /// <param name="arg0"></param>
        private void LightSwitch_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-5", true))
                .Append(new CheckSmallAction("1-2-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(whiteLight))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开照明", Color.green))
                .Append(new UpdateSmallAction("1-2-6", true))
                .Append(new InvokeCurrentGuideAction(7))
                .Append(new InvokeFlashAction(true, windMachineSwitch.gameObject))
                .Execute();
        }
        /// <summary>
        /// 打开紫外开关  1-2-4 关闭紫外开关  1-2-5
        /// </summary>
        /// <param name="arg0"></param>
        private void PurpleSwitch_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-4", true))
                .Append(new CheckSmallAction("1-2-5", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(purpleLight, false))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭紫外开关", Color.green))
                .Append(new UpdateSmallAction("1-2-5", true))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new InvokeFlashAction(true, lightSwitch.gameObject))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                 .Append(new CheckSmallAction("1-2-3", true))
                 .Append(new CheckSmallAction("1-2-4", false))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new InvokeFlashAction(false, purpleSwitch.gameObject))
                 .Append(new GameObjectAction(purpleLight,true))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开紫外开关", Color.green))
                 .Append(new ScreenFaderAction("紫外消毒30min后",2,1))
                 .Append(new UpdateSmallAction("1-2-4", true))
                 .Append(new InvokeCurrentGuideAction(5))
                 .Append(new InvokeFlashAction(true, purpleSwitch.gameObject))
                 .Execute();
        }
        /// <summary>
        /// 放入实验器材  1-2-2        放入培养基瓶 1-2-10    放入冻存管  2-1-12     放入离心管  3-1-12    放置摇瓶   3-3-4
        /// </summary>
        /// <param name="arg0"></param>
        private void GoodPutArea_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-2-2
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-1", true))
                .Append(new CheckSmallAction("1-2-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_SYQC_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_SYQC_01.ToString(), UpdateType.Remove))
                .Append(new InvokeFlashAction(false, goodPutArea.gameObject))
                .Append(new GameObjectAction(goodPutArea.gameObject,false))
                .Append(new GameObjectAction(goodsParent.gameObject))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入实验器材", Color.green))
                .Append(new GameObjectAction(wateringCan))
                .Append(new DOLocalMoveAction(wateringCan, new Vector3(wateringCan.localPosition.x, wateringCan.localPosition.y, wateringCan.localPosition.z - 0.7f), 1f))
                .Append(new DOLocalMoveAction(wateringCan, new Vector3(wateringCan.localPosition.x, wateringCan.localPosition.y, wateringCan.localPosition.z + 0.7f), 1f))
                .Append(new DOLocalMoveAction(wateringCan, new Vector3(wateringCan.localPosition.x, wateringCan.localPosition.y, wateringCan.localPosition.z - 0.7f), 1f))
                .Append(new DOLocalMoveAction(wateringCan, new Vector3(wateringCan.localPosition.x, wateringCan.localPosition.y, wateringCan.localPosition.z + 0.7f), 1f))
                .Append(new GameObjectAction(wateringCan, false))
                .Append(new UpdateSmallAction("1-2-2", true))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, glassDoor.gameObject))
                 .OnCompleted(() =>
                 {
                     glassDoor.GetComponent<BoxCollider>().enabled = true;
                 })
                .Execute();
            //1-2-10
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-9", true))
                .Append(new CheckSmallAction("1-2-10", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_PYJP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYJP_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(goodPutArea.gameObject, false))
                .Append(new GameObjectAction(deskBottle.gameObject))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入培养基瓶", Color.green))
                .Append(new UpdateSmallAction("1-2-10", true))           
                .Append(new InvokeCurrentGuideAction(11))
                .Append(new InvokeFlashAction(true, glassDoor.gameObject))
                 .OnCompleted(() =>
                 {
                     glassDoor.GetComponent<BoxCollider>().enabled = true;
                 })
                .Execute();
            //2-1-12
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-11", true))
                .Append(new CheckSmallAction("2-1-12", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_DCG_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCG_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(goodPutArea.gameObject, false))
                .Append(new GameObjectAction(deskTube.gameObject))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入冻存管", Color.green))
                .Append(new UpdateSmallAction("2-1-12", true))
                .Append(new InvokeCompletedAction(2,1))
                .Append(new InvokeCurrentGuideAction(1))
                .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
                .Execute();
            //3-1-12
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new CheckSmallAction("3-1-11", true))
                .Append(new CheckSmallAction("3-1-12", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_LXG_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_LXG_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(goodPutArea.gameObject, false))
                .Append(new GameObjectAction(centrifugeTube.gameObject))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入离心管", Color.green))
                .Append(new UpdateSmallAction("3-1-12", true))
                .Append(new InvokeCurrentGuideAction(13))
                .Append(new InvokeFlashAction(true, gun_5ml.gameObject))
                .Execute();
            //3-3-4
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-3", true))
                .Append(new CheckSmallAction("3-3-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_XYP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_XYP_01.ToString(), UpdateType.Remove))
                .Append(new InvokeFlashAction(false, goodPutArea.gameObject))
                .Append(new GameObjectAction(goodPutArea.gameObject, false))
                .Append(new GameObjectAction(smallRockBottl.gameObject))
                .Append(new DOLocalMoveAction(smallRockBottl,new Vector3 (-39.895f, 8.430017f, -51.347f),0.02f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入摇瓶", Color.green))
                .Append(new UpdateSmallAction("3-3-4", true))
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new InvokeFlashAction(true, bioMate.gameObject))
                .Execute();
        }
        /// <summary>
        /// 上拉玻璃挡板  1-2-1   1-2-9   2-1-11    3-3-3下拉玻璃挡板 1-2-3    1-2-11    3-2-2
        /// </summary>
        /// <param name="arg0"></param>
        private void GlassDoor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //3-3-3
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
                .Append(new CheckSmallAction("3-3-2", true))
                .Append(new CheckSmallAction("3-3-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0.25f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已上拉玻璃挡板", Color.green))
                .Append(new UpdateSmallAction("3-3-3", true))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new GameObjectAction(goodPutArea.gameObject))
                .Append(new InvokeFlashAction(true, goodPutArea.gameObject))
                .OnCompleted(() =>
                {
                    glassDoor.GetComponent<BoxCollider>().enabled = false;
                })
                .Execute();
            //2-1-11
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-10", true))
                .Append(new CheckSmallAction("2-1-11", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0.25f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已上拉玻璃挡板", Color.green))
                .Append(new UpdateSmallAction("2-1-11", true))
                .Append(new InvokeCurrentGuideAction(12))
                .Append(new GameObjectAction(goodPutArea.gameObject))
                 .OnCompleted(() =>
                 {
                     glassDoor.GetComponent<BoxCollider>().enabled = false;
                 })
                .Execute();
            //1-2-9
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-8", true))
                .Append(new CheckSmallAction("1-2-9", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0.25f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已上拉玻璃挡板", Color.green))
                .Append(new UpdateSmallAction("1-2-9", true))
                .Append(new InvokeCurrentGuideAction(10))
                .Append(new GameObjectAction(goodPutArea.gameObject))
                .Append(new InvokeFlashAction(true, goodPutArea.gameObject))
                 .OnCompleted(() =>
                 {
                     glassDoor.GetComponent<BoxCollider>().enabled = false;
                 })
                .Execute();
            //1-2-1
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                 .Append(new CheckSmallAction("1-2-1", false))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                 .Append(new DOLocalMoveAction(glassDoor,new Vector3 (0,0.25f,0),1))
                 .Append(new UpdateSmallAction("1-2-1", true))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已上拉玻璃挡板", Color.green))
                 .Append(new GameObjectAction(cloth))
                 .Append(new DOLocalMoveAction(cloth,new Vector3 (cloth.localPosition.x,cloth.localPosition.y,cloth.localPosition.z-0.5f),0.5f))
                 .Append(new DOLocalMoveAction(cloth, new Vector3(cloth.localPosition.x, cloth.localPosition.y, cloth.localPosition.z +0.5f), 0.5f))
                 .Append(new DOLocalMoveAction(cloth, new Vector3(cloth.localPosition.x, cloth.localPosition.y, cloth.localPosition.z - 0.5f), 0.5f))
                 .Append(new DOLocalMoveAction(cloth, new Vector3(cloth.localPosition.x, cloth.localPosition.y, cloth.localPosition.z + 0.5f), 0.5f))
                 .Append(new GameObjectAction(cloth,false))
                 .Append(new InvokeCurrentGuideAction(2))
                 .Append(new GameObjectAction(goodPutArea.gameObject))
                 .Append(new InvokeFlashAction(true, goodPutArea.gameObject))
                  .OnCompleted(() =>
                  {
                      glassDoor.GetComponent<BoxCollider>().enabled = false;
                  })
                 .Execute();
            //1-2-3
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-2", true))
                .Append(new CheckSmallAction("1-2-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已下拉玻璃挡板", Color.green))
                .Append(new GameObjectAction(ultravioletSetPanel.gameObject))             
                .Execute();
            //1-2-11
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-10", true))
                .Append(new CheckSmallAction("1-2-11", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已下拉玻璃挡板", Color.green))
                .Append(new UpdateSmallAction("1-2-11", true))
                .Append(new InvokeCompletedAction(1, 2))
                .Append(new InvokeCurrentGuideAction(1))
                .Append(new InvokeFlashAction(true, windowDoor.gameObject))
                .Execute();
            //3-2-2
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                .Append(new CheckSmallAction("3-2-1", true))
                .Append(new CheckSmallAction("3-2-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, glassDoor.gameObject))
                .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0f, 0), 1))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已下拉玻璃挡板", Color.green))
                .Append(new UpdateSmallAction("3-2-2", true))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, glassDoor.gameObject))
                .Execute();
        }
        /// <summary>
        /// 将培养基瓶放入水浴锅水浴  1-1-4   将冻存管放入水浴锅水浴  2-1-6
        /// </summary>
        /// <param name="arg0"></param>
        private void WaterPot_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-3", true))
                .Append(new CheckSmallAction("1-1-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_PYJP_01))
                .Append(new InvokeFlashAction(false, waterPot.gameObject))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYJP_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(waterPotBottle))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将培养基瓶放入水浴锅水浴", Color.green))
                .Append(new GameObjectAction(potSetPanel.gameObject))                
                .Execute();

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                .Append(new CheckSmallAction("2-1-5", true))
                .Append(new CheckSmallAction("2-1-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_DCG_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_DCG_01.ToString(), UpdateType.Remove))
                .Append(new InvokeFlashAction(false, waterPot.gameObject))
                .Append(new GameObjectAction(centrifuge_Tube01))
                .Append(new GameObjectAction(tweezers))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将冻存管放入水浴锅水浴", Color.green))
                .Append(new DOLocalMoveAction(centrifuge_Tube01,new Vector3 (-37.5257f, 8.5842f, -54.394f),0.25f,true))
                .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6264f, 8.6431f, -54.3927f), 0.25f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.5752f, 8.5842f, -54.394f), 0.25f, true))
                .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6759f, 8.6431f, -54.3927f), 0.25f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.5257f, 8.5842f, -54.394f), 0.25f, true))
                .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6264f, 8.6431f, -54.3927f), 0.25f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.5752f, 8.5842f, -54.394f), 0.25f, true))
                .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6759f, 8.6431f, -54.3927f), 0.25f))
                .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.549f, 8.5842f, -54.394f), 0.25f, true))
                .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6497f, 8.6431f, -54.3927f), 0.25f))
                .OnCompleted(() =>
                {
                    MessageBoxEx.Show("冻存液完全融化", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
                        SmallActionManager.Instance.UpdateSmallAction("2-1-6", true);
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        Task.NewTask()
                        .Append(new InvokeFlashAction(true, centrifugeCap_Inner.gameObject))
                        .Execute();
                    });

                })
                .Execute();
        }
        /// <summary>
        /// 取培养基瓶  1-1-2
        /// </summary>
        /// <param name="arg0"></param>
        private void MediumBottlet_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-1", true))
                .Append(new CheckSmallAction("1-1-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(eventData.pointerEnter,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYJP_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已领取培养基瓶", Color.green))
                .Append(new UpdateSmallAction("1-1-2", true))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, doorRight.gameObject, doorLeft.gameObject))
                .Execute();

        }
        /// <summary>
        /// 打开冰箱门  1-1-1  关闭冰箱门     1-1-3
        /// </summary>
        /// <param name="arg0"></param>
        private void DoorRight_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-1", false))
                .Append(new UpdateSmallAction("1-1-1", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, doorRight.gameObject, doorLeft.gameObject))
                .Append(new DOLocalRotaAction(doorLeft, new Vector3(0, 0, -90), 1, true))
                .Append(new DOLocalRotaAction(doorRight, new Vector3(0, 0, -90), 1, true))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开冰箱门", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new InvokeFlashAction(true, objs.ToArray()))               
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-2", true))
                .Append(new CheckSmallAction("1-1-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, doorRight.gameObject, doorLeft.gameObject))
                .Append(new DOLocalRotaAction(doorLeft, new Vector3(0, 0, 0), 1, true))
                .Append(new DOLocalRotaAction(doorRight, new Vector3(0, 0, 0), 1, true))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭冰箱门", Color.green))
                .Append(new UpdateSmallAction("1-1-3", true))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new InvokeFlashAction(true, waterPot.gameObject))
                .Execute();
        }
        /// <summary>
        /// 打开冰箱门  1-1-1      关闭冰箱门     1-1-3
        /// </summary>
        /// <param name="arg0"></param>
        private void DoorLeft_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-1", false))
                .Append(new UpdateSmallAction("1-1-1", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, doorRight.gameObject, doorLeft.gameObject))
                .Append(new DOLocalRotaAction(doorLeft, new Vector3(0, 0, -90), 1, true))
                .Append(new DOLocalRotaAction(doorRight, new Vector3(0, 0, -90), 1, true))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开冰箱门", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new InvokeFlashAction(true, objs.ToArray()))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
               .Append(new CheckSmallAction("1-1-2", true))
               .Append(new CheckSmallAction("1-1-3", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, doorRight.gameObject, doorLeft.gameObject))
               .Append(new DOLocalRotaAction(doorLeft, new Vector3(0, 0, 0), 1, true))
               .Append(new DOLocalRotaAction(doorRight, new Vector3(0, 0, 0), 1, true))
               .Append(new DelayedAction(1))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭冰箱门", Color.green))
               .Append(new UpdateSmallAction("1-1-3", true))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new InvokeFlashAction(true, waterPot.gameObject))
               .Execute();
        }
    }
}
