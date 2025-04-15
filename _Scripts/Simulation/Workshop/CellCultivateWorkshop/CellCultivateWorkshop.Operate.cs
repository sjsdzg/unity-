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
using UnityEngine.AI;

namespace XFramework.Simulation
{
   public partial class CellCultivateWorkshop
    {
        private NPCControl QA_NpcCtrl;

        public NPCControl NpcCtrl
        {
            get
            {
                if (QA_NpcCtrl == null)
                {
                    QA_NpcCtrl = GameObject.FindGameObjectWithTag("NPC").transform.GetComponent<NPCControl>();
                }
                return QA_NpcCtrl;

            }
        }

        public MyselfControl MyselfControl
        {
            get
            {
                if (m_myselfControl == null)
                {
                     foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
                     {
                        if (rootObj.tag.Equals("Player"))
                        {
                            m_myselfControl = rootObj.transform.GetComponent<MyselfControl>(); ;
                            break;
                        }
                     }
                    //m_myselfControl = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MyselfControl>();
                }
                return m_myselfControl;
            }
        }
        private MyselfControl m_myselfControl;
        private void InitializeOperate()
        {
            motor.TriggerAction(EventTriggerType.PointerClick, Motor_PointerClick);
            PHPole.TriggerAction(EventTriggerType.PointerClick, PHPole_PointerClick);
            TPole.TriggerAction(EventTriggerType.PointerClick, TPole_PointerClick);
            DOPole.TriggerAction(EventTriggerType.PointerClick, DOPole_PointerClick);
            Tank3L.TriggerAction(EventTriggerType.PointerClick, Tank3L_PointerClick);
            autoclave_Cap.TriggerAction(EventTriggerType.PointerClick, Autoclave_Cap_PointerClick);
            autoclave.TriggerAction(EventTriggerType.Drop, Autoclave_Drop);
            autoclave_Tank3L.TriggerAction(EventTriggerType.PointerClick, Autoclave_Tank3L_PointerClick);
            desk.TriggerAction(EventTriggerType.Drop, Desk_Drop);
            passWindowDoor.TriggerAction(EventTriggerType.PointerClick, PassWindowDoor_PointerClick);
            passWindowBottle.TriggerAction(EventTriggerType.PointerClick, PassWindowBottle_PointerClick);
            infusionTube.TriggerAction(EventTriggerType.PointerClick, InfusionTube_PointerClick);
            infusionTube_02.TriggerAction(EventTriggerType.PointerClick, InfusionTube_02_PointerClick);
            pumpStartPower.TriggerAction(EventTriggerType.PointerClick, PumpStartPower_PointerClick);
            Tank3L_Power.TriggerAction(EventTriggerType.PointerClick, Tank3L_Power_PointerClick);
            injector.TriggerAction(EventTriggerType.PointerClick, Injector_PointerClick);
            Tank20L_PowerButton.TriggerAction(EventTriggerType.PointerClick, Tank20L_PowerButton_PointerClick);
            Tank20L_Screen.TriggerAction(EventTriggerType.PointerClick, Tank20L_Screen_PointerClick);
            sampleBottle_PutArea.TriggerAction(EventTriggerType.Drop, SampleBottle_PutArea_Drop);
            V011Hand.TriggerAction(EventTriggerType.PointerClick, V011Hand_PointerClick);
            V012Hand.TriggerAction(EventTriggerType.PointerClick, V012Hand_PointerClick);
            sampleBottle.TriggerAction(EventTriggerType.PointerClick, SampleBottle_PointerClick);
            Tank200L_PowerButton.TriggerAction(EventTriggerType.PointerClick, Tank200L_PowerButton_PointerClick);
            Tank200L_Screen.TriggerAction(EventTriggerType.PointerClick, Tank200L_Screen_PointerClick);
            sampleBottle_PutArea_200L.TriggerAction(EventTriggerType.Drop, SampleBottle_PutArea_200L_Drop);
            V030Hand.TriggerAction(EventTriggerType.PointerClick, V030Hand_PointerClick);
            V031Hand.TriggerAction(EventTriggerType.PointerClick, V031Hand_PointerClick);
            sampleBottle_200L.TriggerAction(EventTriggerType.PointerClick, SampleBottle_200L_PointerClick);
            V040Hand.TriggerAction(EventTriggerType.PointerClick, V040Hand_PointerClick);
            V041Hand.TriggerAction(EventTriggerType.PointerClick, V041Hand_PointerClick);
            motor_Hand.TriggerAction(EventTriggerType.PointerClick, Motor_Hand_PointerClick);


            this.Invoke(0.2f, () =>
            {
                Task.NewTask()
                    //.Append(new InvokeCompletedAction(1,4))
                    .Append(new InvokeCurrentGuideAction(1))
                    .Append(new InvokeFlashAction(true, motor.gameObject))
                    .OnCompleted(() =>
                    {
                    })
                    .Execute();
              //  QA_NpcCtrl.OnDropEvent.AddListener(QA_NpcCtrl_OnDropEvent);
                //workshopPath.Invoke(5, () => { QA_NpcCtrl.point = workshopPath.GetWaypoints("送检路径").points.ToArray(); });
            });
        }
        /// <summary>
        /// 启动电机   4-1-3
        /// </summary>
        /// <param name="arg0"></param>
        private void Motor_Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //启动电机  4-1-3
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-2", true))
               .Append(new CheckSmallAction("4-1-3", false))
               .Append(new UpdateSmallAction("4-1-3", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动电机", Color.green))
               .Append(new ShowProgressAction("板框过滤中...",3))
               .Append(new DelayedAction(3))             
               .OnCompleted(() =>
               {
                   MessageBoxEx.Show("上清液已全部输送至下一工段", "提示", MessageBoxExEnum.SingleDialog, X =>
                   {
                       Task.NewTask()
                       .Append(new InvokeCurrentGuideAction(4))
                       .Execute();
                   });
               })
               .Execute();
        }
        /// <summary>
        /// 打开板框过滤机出料阀        4-1-1    关闭板框过滤机出料阀  4-1-5
        /// </summary>
        /// <param name="arg0"></param>
        private void V041Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开板框过滤机出料阀  4-1-1
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V041Hand.gameObject))
               .Append(new DOLocalRotaAction(V041Hand,new Vector3 (0,-90,0),0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开板框过滤机出料阀", Color.green))
               .Append(new UpdateSmallAction("4-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, V040Hand.gameObject))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, "V040", true);
               })
               .Execute();
            //关闭板框过滤机出料阀  4-1-5
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-4", true))
               .Append(new CheckSmallAction("4-1-5", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V041Hand.gameObject))
               .Append(new DOLocalRotaAction(V041Hand, new Vector3(0, -90, 90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭板框过滤机出料阀", Color.green))            
               .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, "V040", false);
                    Task.NewTask()
                      .Append(new DelayedAction(2))
                     .Append(new InvokeCompletedAction(4, 1))
                    .Execute();
                })
               .Execute();
        }
        /// <summary>
        /// 打开板框过滤机进料阀  4-1-2   关闭板框过滤机进料阀  4-1-4
        /// </summary>
        /// <param name="arg0"></param>
        private void V040Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开板框过滤机进料阀  4-1-2
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-1", true))
               .Append(new CheckSmallAction("4-1-2", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V040Hand.gameObject))
               .Append(new DOLocalRotaAction(V040Hand, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开板框过滤机进料阀", Color.green))
               .Append(new InvokeFittingAction("4-1-2"))
               .Append(new UpdateSmallAction("4-1-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, V041Hand.gameObject))
               .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, "V039", true);
                })
               .Execute();
            //关闭板框过滤机进料阀  4-1-4
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-3", true))
               .Append(new CheckSmallAction("4-1-4", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V040Hand.gameObject))
               .Append(new DOLocalRotaAction(V040Hand, new Vector3(0, 0, 90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭板框过滤机进料阀", Color.green))
               .Append(new InvokeFittingAction("4-1-4"))
               .Append(new UpdateSmallAction("4-1-4", true))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new InvokeFlashAction(true, V041Hand.gameObject))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string>("200L培养罐液位参数变化", "0");
                   EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, "V040", false);
               })
               .Execute();

        }
        /// <summary>
        /// 拾取取样瓶   3-4-6
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleBottle_200L_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //拾取取样瓶  3-4-6
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
               .Append(new CheckSmallAction("3-4-5", true))
               .Append(new CheckSmallAction("3-4-6", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(sampleBottle_200L,false))
               .Append(new GameObjectAction(fireCircle_200L, false))
               .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_02.ToString(), UpdateType.Add))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拾取取样瓶", Color.green))
               .Append(new InvokeCurrentGuideAction(7))
               .Append(new UpdateSmallAction("3-4-6", true))
               .Append(new InvokeFlashAction(true, NpcCtrl.gameObject))
               .OnCompleted(() =>
               {
                   if (m_CameraSwitcher.CurrentStyle == CameraStyle.Look)
                   {
                       MyselfControl.transform.position = new Vector3(-32.27724f, 7.5f, -45.98007f);
                       MyselfControl.transform.eulerAngles = new Vector3(0, -88, 0);
                   }
               })

               .Execute();
        }
        /// <summary>
        /// 打开取样阀  3-4-5
        /// </summary>
        /// <param name="arg0"></param>
        private void V031Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开取样阀  3-4-5
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
               .Append(new CheckSmallAction("3-4-4", true))
               .Append(new CheckSmallAction("3-4-5", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V031Hand.gameObject))
               .Append(new DOLocalRotaAction(V031Hand, new Vector3(0, -90, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开取样阀", Color.green))
               .Append(new GameObjectAction(sampleBottle_PutArea_200L, false))
               .Append(new InvokeFittingAction("3-4-5-1"))
               .Append(new CoroutineFluidLevelAction(sampleBottleFlu_200L.gameObject, 0.45f, 2f))
               .Append(new InvokeFittingAction("3-4-5-2"))
               .Append(new UpdateSmallAction("3-4-5", true))
               .OnCompleted(() =>
               {
                   MessageBoxEx.Show("取样结束，关闭取样阀", "提示", MessageBoxExEnum.SingleDialog, (x) =>
                   {
                       Task.NewTask()
                           .Append(new DOLocalRotaAction(V030Hand, new Vector3(0, 0, 0), 0.5f, true))
                           .Append(new DOLocalRotaAction(V031Hand, new Vector3(0, 0, 0), 0.5f, true))
                           .Append(new InvokeCurrentGuideAction(6))
                           .Append(new InvokeFlashAction(true, sampleBottle_200L.gameObject))
                           .Execute();

                   });
               })
               .Execute();
        }
        /// <summary>
        /// 打开取样阀  3-4-4
        /// </summary>
        /// <param name="arg0"></param>
        private void V030Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开取样阀  3-4-4
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
               .Append(new CheckSmallAction("3-4-3", true))
               .Append(new CheckSmallAction("3-4-4", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V030Hand.gameObject))
               .Append(new DOLocalRotaAction(V030Hand, new Vector3(0, -90, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开取样阀", Color.green))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new UpdateSmallAction("3-4-4", true))
               .Append(new InvokeFlashAction(true, V031Hand.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }
        /// <summary>
        /// 放置火焰线圈  3-4-2      放置取样瓶  3-4-3
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleBottle_PutArea_200L_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //放置取样瓶  3-4-3
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
               .Append(new CheckSmallAction("3-4-2", true))
               .Append(new CheckSmallAction("3-4-3", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.Cell_QYP_01))
               .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置取样瓶", Color.green))
               .Append(new GameObjectAction(sampleBottle_200L))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new UpdateSmallAction("3-4-3", true))
               .Append(new InvokeFlashAction(true, V030Hand.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();
            //放置火焰线圈   3-4-2
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
               .Append(new CheckSmallAction("3-4-1", true))
               .Append(new CheckSmallAction("3-4-2", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.Cell_HYXQ_01))
               .Append(new UpdateGoodsAction(GoodsType.Cell_HYXQ_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置火焰线圈", Color.green))
               .Append(new GameObjectAction(fireCircle_200L))              
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new UpdateSmallAction("3-4-2", true))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }
        /// <summary>
        /// 拉近屏幕  200L
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank200L_Screen_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
              .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, true))
              .Append(new InvokeCameraLookPointAction("观察200L培养罐", 0.5f))
              .OnCompleted(() =>
              {
              })
              .Execute();
        }
        /// <summary>
        /// 打开200L培养罐电源   3-1-1
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank200L_PowerButton_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开200L培养罐电源   3-1-1
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, Tank200L_PowerButton.gameObject))
               .Append(new DOLocalRotaAction(Tank200L_PowerButton,new Vector3 (-90,90,90),0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开200L培养罐电源", Color.green))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new UpdateSmallAction("3-1-1", true))
               .Append(new GameObjectAction(Tank200L_PLC))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-1-2-1");
               })
               .Execute();
        }
        /// <summary>
        /// 拾取取样瓶   2-4-6
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleBottle_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //拾取取样瓶  2-4-6
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
               .Append(new CheckSmallAction("2-4-5", true))
               .Append(new CheckSmallAction("2-4-6", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(sampleBottle,false))
               .Append(new GameObjectAction(fireCircle, false))
               .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_02.ToString(), UpdateType.Add))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拾取取样瓶", Color.green))
               .Append(new InvokeCurrentGuideAction(7))
               .Append(new UpdateSmallAction("2-4-6", true))
               .Append(new InvokeFlashAction(true, NpcCtrl.gameObject))
               .OnCompleted(() =>
               {
                   if (m_CameraSwitcher.CurrentStyle == CameraStyle.Look)
                   {
                       MyselfControl.transform.position = new Vector3(-32.27724f, 7.5f, -45.98007f);
                       MyselfControl.transform.eulerAngles = new Vector3(0, -88, 0);
                   }
               })
               .Execute();
        }
        /// <summary>
        /// 打开取样阀  2-4-5
        /// </summary>
        /// <param name="arg0"></param>
        private void V012Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开取样阀  2-4-5
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
               .Append(new CheckSmallAction("2-4-4", true))
               .Append(new CheckSmallAction("2-4-5", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V012Hand.gameObject))
               .Append(new GameObjectAction(sampleBottle_PutArea,false))
               .Append(new DOLocalRotaAction(V012Hand, new Vector3(0, -90, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开取样阀", Color.green))
               .Append(new InvokeFittingAction("2-4-5-1"))
               .Append(new CoroutineFluidLevelAction(sampleBottleFlu.gameObject,0.45f,2f))
               .Append(new InvokeFittingAction("2-4-5-2"))
               .Append(new UpdateSmallAction("2-4-5", true))
              // .Append(new GameObjectAction(sampleBottle_PutArea,false))              
               .OnCompleted(() =>
               {
                   MessageBoxEx.Show("取样结束，关闭取样阀", "提示", MessageBoxExEnum.SingleDialog, (x) =>
                   {
                       Task.NewTask()
                           .Append(new DOLocalRotaAction(V012Hand, new Vector3(0, 0, 0), 0.5f, true))
                           .Append(new DOLocalRotaAction(V011Hand, new Vector3(0, 0, 0), 0.5f, true))
                           .Append(new InvokeCurrentGuideAction(6))
                           .Append(new InvokeFlashAction(true, sampleBottle.gameObject))
                           .Execute();

                   });
               })
               .Execute();
        }
        /// <summary>
        /// 打开取样阀  2-4-4
        /// </summary>
        /// <param name="arg0"></param>
        private void V011Hand_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开取样阀  2-4-4
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
               .Append(new CheckSmallAction("2-4-3", true))
               .Append(new CheckSmallAction("2-4-4", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, V011Hand.gameObject))
               .Append(new DOLocalRotaAction(V011Hand,new Vector3 (0,-90,0),0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开取样阀", Color.green))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new InvokeFlashAction(true, V012Hand.gameObject))
               .Append(new UpdateSmallAction("2-4-4", true))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }
        /// <summary>
        /// 放置火焰线圈    2-4-2          放置取样瓶  2-4-3
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleBottle_PutArea_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //放置取样瓶  2-4-3
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
               .Append(new CheckSmallAction("2-4-2", true))
               .Append(new CheckSmallAction("2-4-3", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.Cell_QYP_01))
              // .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_01.ToString(), UpdateType.Remove))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置取样瓶", Color.green))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(sampleBottle))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new UpdateSmallAction("2-4-3", true))
               .Append(new InvokeFlashAction(true, V011Hand.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();
            //放置火焰线圈  2-4-2
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
               .Append(new CheckSmallAction("2-4-1", true))
               .Append(new CheckSmallAction("2-4-2", false))
               .Append(new CheckGoodsAction(eventData,GoodsType.Cell_HYXQ_01))
               //.Append(new UpdateGoodsAction(GoodsType.Cell_HYXQ_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置火焰线圈", Color.green))
               .Append(new GameObjectAction(fireCircle))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new UpdateSmallAction("2-4-2", true))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }
        /// <summary>
        /// 拉近屏幕  20L
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank20L_Screen_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
              .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, true))
              .Append(new InvokeCameraLookPointAction("观察20L培养罐",0.5f))
              .OnCompleted(() =>
              {
              })
              .Execute();
        }
        /// <summary>
        /// 打开20L培养罐电源    2-1-1
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank20L_PowerButton_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //打开20L培养罐电源   2-1-1
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
               .Append(new CheckSmallAction("2-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, Tank20L_PowerButton.gameObject))
               .Append(new DOLocalRotaAction(Tank20L_PowerButton, new Vector3(-90, 90, 90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开20L培养罐电源", Color.green))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new UpdateSmallAction("2-1-1", true))
               .Append(new GameObjectAction(Tank20L_PLC))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string>("20L培养罐阀门点击前", "2-1-2-1");
               })
               .Execute();
        }
        /// <summary>
        /// 取样检测    1-4-1
        /// </summary>
        /// <param name="arg0"></param>
        private void Injector_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //取样检测  1-4-1
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, false))
               .Append(new CheckSmallAction("1-4-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, injector.gameObject))
               .Append(new DOLocalMoveAction(injector, new Vector3(0, 0, -0.002f), 0.5f))
               .Append(new GameObjectAction(injectorFlu))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已开启控制器", Color.green))
               .Append(new DelayedAction(2))
               .Append(new GameObjectAction(injectorParent,false))
               .Append(new UpdateGoodsAction(GoodsType.Cell_ZSQ_01.ToString(),UpdateType.Add))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new UpdateSmallAction("1-4-1", true))
               .Append(new InvokeFlashAction(true, NpcCtrl.gameObject))
               .OnCompleted(() =>
               {
                   if (m_CameraSwitcher.CurrentStyle==CameraStyle.Look)
                   {
                       MyselfControl.transform.position = new Vector3(-32.27724f, 7.5f, -45.98007f);
                       MyselfControl.transform.eulerAngles = new Vector3(0, -88, 0);
                   }
                 
               })
               .Execute();
        }
        /// <summary>
        /// 开启控制器    1-3-15
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank3L_Power_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //开启控制器  1-3-15
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-14", true))
               .Append(new CheckSmallAction("1-3-15", false))
               .Append(new UpdateSmallAction("1-3-15", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, Tank3L_Power.gameObject))
               .Append(new DOLocalRotaAction(Tank3L_Power, new Vector3(Tank3L_Power.localEulerAngles.x+90, Tank3L_Power.localEulerAngles.y, Tank3L_Power.localEulerAngles.z), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已开启控制器", Color.green))
               .Append(new GameObjectAction(Tank3L_PLC))
               .Append(new InvokeCameraLookPointAction("观察3L培养罐",0.5f))
               .Append(new InvokeCurrentGuideAction(16))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_3L, true);
               })
               .Execute();
        }
        /// <summary>
        /// 启动蠕动泵   1-3-6   1-3-9   1-3-12  2-2-2  关闭蠕动泵   1-3-7       1-3-10    1-3-13    2-2-3
        /// </summary>
        /// <param name="arg0"></param>
        private void PumpStartPower_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //关闭蠕动泵   2-2-3
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-2", true))
               .Append(new CheckSmallAction("2-2-3", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
               .Append(new GameObjectAction(pumpStartPowerOpen, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new GameObjectAction(softPipe,false))
               .Append(new GameObjectAction(tubeSealingMachinePanel))
               .Append(new GameObjectAction(tubeSealingMachineObj))
               .Append(new DelayedAction(6.5f))
               .Append(new GameObjectAction(tubeSealingMachinePanel,false))
               .Append(new GameObjectAction(tubeSealingMachineObj,false))
               .Append(new InvokeCompletedAction(2,2))
               .Append(new InvokeCurrentGuideAction())
               .Append(new UpdateSmallAction("2-2-3", true))
               .OnCompleted(() =>
                 {
                   EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_20L, true);
                   EventDispatcher.ExecuteEvent<string>("3L培养罐液位参数变化", "0");
                   EventDispatcher.ExecuteEvent<string>("20L培养罐液位参数变化", "80");
                 })
               .Execute();
            //关闭蠕动泵   1-3-7
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-6", true))
               .Append(new CheckSmallAction("1-3-7", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
               .Append(new GameObjectAction(pumpStartPowerOpen,false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))             
               .Append(new InvokeCurrentGuideAction(8))
               .Append(new InvokeFlashAction(true, infusionTube_02.gameObject))
               .Append(new UpdateSmallAction("1-3-7", true))
               .Execute();
            //关闭蠕动泵   1-3-10
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-9", true))
               .Append(new CheckSmallAction("1-3-10", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
               .Append(new GameObjectAction(pumpStartPowerOpen, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new InvokeCurrentGuideAction(11))
               .Append(new UpdateSmallAction("1-3-10", true))
               .Append(new InvokeFlashAction(true, infusionTube_02.gameObject))
               .Execute();
            //关闭蠕动泵   1-3-13
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-12", true))
               .Append(new CheckSmallAction("1-3-13", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
               .Append(new GameObjectAction(pumpStartPowerOpen, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new InvokeCurrentGuideAction(14))
               .Append(new UpdateSmallAction("1-3-13", true))
               .Append(new InvokeFlashAction(true, infusionTube_02.gameObject))
               .Execute();
            //启动蠕动泵   1-3-6 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-5", true))
                .Append(new CheckSmallAction("1-3-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
                .Append(new GameObjectAction(pumpStartPowerOpen))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
                .Append(new CoroutineFluidLevelAction(deskBottle_01Flu.gameObject,0.15f,5,true))
                .Append(new GameObjectAction(infusionTube_02Flu))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject,m_UpdateMaterialComponent.NewMaterial))
                .Append(new GameObjectAction(Tank3L_water))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject,true))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, true))
                .Append(new InvokeFluidAction(fluidComponent,1))
                .Append(new DelayedAction(0.2f))
                .Append(new CoroutineFluidLevelAction(Tank3L_Flu.gameObject, 0.2f, 5, true))
                .Append(new DelayedAction(5))
                .Append(new GameObjectAction(infusionTube_02Flu,false))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.OldMaterial))
                .Append(new InvokeFluidAction(fluidComponent, 0))
                .Append(new GameObjectAction(Tank3L_water,false))
                .Append(new GameObjectAction(infusionTubeLine_02, true))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                .Append(new InvokeCurrentGuideAction(7))
                .Append(new UpdateSmallAction("1-3-6", true))
                .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                .OnCompleted(() =>
                {
                     EventDispatcher.ExecuteEvent<string>("3L培养罐液位参数变化", "30");
                })
                .Execute();
            //启动蠕动泵   1-3-9 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-8", true))
                .Append(new CheckSmallAction("1-3-9", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
                .Append(new GameObjectAction(pumpStartPowerOpen))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
                .Append(new CoroutineFluidLevelAction(deskBottle_02Flu.gameObject, 0f, 5, true))
                .Append(new GameObjectAction(infusionTube_02Flu))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.NewMaterial))
                .Append(new GameObjectAction(Tank3L_water))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, true))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, true))
                .Append(new InvokeFluidAction(fluidComponent, 1))
                .Append(new DelayedAction(0.2f))
                .Append(new CoroutineFluidLevelAction(Tank3L_Flu.gameObject, 0.4f, 5, true))
                .Append(new DelayedAction(5))
                .Append(new GameObjectAction(infusionTube_02Flu, false))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.OldMaterial))
                .Append(new InvokeFluidAction(fluidComponent, 0))
                .Append(new GameObjectAction(Tank3L_water, false))
                .Append(new GameObjectAction(infusionTubeLine_02, true))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                .Append(new InvokeCurrentGuideAction(10))
                .Append(new UpdateSmallAction("1-3-9", true))
                .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string>("3L培养罐液位参数变化", "50");
                 })
                .Execute();
            //启动蠕动泵   1-3-12 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-11", true))
                .Append(new CheckSmallAction("1-3-12", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
                .Append(new GameObjectAction(pumpStartPowerOpen))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
                .Append(new CoroutineFluidLevelAction(deskBottle_01Flu.gameObject, 0f, 5, true))
                .Append(new GameObjectAction(infusionTube_02Flu))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.NewMaterial))
                .Append(new GameObjectAction(Tank3L_water))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, true))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, true))
                .Append(new InvokeFluidAction(fluidComponent, 1))
                .Append(new DelayedAction(0.2f))
                .Append(new CoroutineFluidLevelAction(Tank3L_Flu.gameObject, 0.6f, 5, true))
                .Append(new DelayedAction(5))
                .Append(new GameObjectAction(infusionTube_02Flu, false))
                .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.OldMaterial))
                .Append(new InvokeFluidAction(fluidComponent, 0))
                .Append(new GameObjectAction(Tank3L_water, false))
                .Append(new GameObjectAction(infusionTubeLine_02, true))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                .Append(new InvokeCurrentGuideAction(13))
                .Append(new UpdateSmallAction("1-3-12", true))
                .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string>("3L培养罐液位参数变化", "80");
                })
                .Execute();
            //启动蠕动泵   2-2-2 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
                .Append(new CheckSmallAction("2-2-1", true))
                .Append(new CheckSmallAction("2-2-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, pumpStartPower.gameObject))
                .Append(new GameObjectAction(pumpStartPowerOpen))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
                .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, true))
                .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, true))
                .Append(new GameObjectAction(Tank20L_WaterEffect))
                .Append(new UpdateMaterialAction(softPipe.gameObject, m_UpdateMaterialComponent.NewMaterial))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, true))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, true))
                .Append(new CoroutineFluidLevelAction(Tank3L_Flu.gameObject, 0f, 5, true))
                .Append(new CoroutineFluidLevelAction(Tank20LFlu.gameObject, 0.7f, 5, false))
                .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, false))
                .Append(new GameObjectAction(Tank20L_WaterEffect,false))
                .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                .Append(new UpdateMaterialAction(softPipe.gameObject, m_UpdateMaterialComponent.OldMaterial))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                .Append(new UpdateSmallAction("2-2-2", true))
                .Execute();
        }
        /// <summary>
        /// 输液管消毒    1-3-5    将输液管转入细胞培养瓶  1-3-8    将输液管转入培养基瓶     1-3-11    输液管消毒封口  1-3-14
        /// </summary>
        /// <param name="arg0"></param>
        private void InfusionTube_02_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //输液管消毒    1-3-5 
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-4", true))
                .Append(new CheckSmallAction("1-3-5", false))
                .Append(new UpdateSmallAction("1-3-5", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, infusionTube_02.gameObject))
                .Append(new DOLocalMoveAction(infusionTube_Paper02,new Vector3 (-0.9213f, 0.061f, 0.2603f),0.5f))
                .Append(new GameObjectAction(infusionTube_Paper02,false))
                .Append(new DOLocalMoveAction(alcoholLampCap,new Vector3 (-0.0098f, 0.0071f, 0.0669f),0.3f))
                .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0915f, 0.0071f, 0.0669f), 0.3f))
                .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0915f, 0.0071f, -0.0512f), 0.4f))
                .Append(new GameObjectAction(alcoholLampFire.gameObject))
                .Append(new InvokeTimeLineAction(infusionTube_02.gameObject,1,true,3.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已连接输液管", Color.green))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                .Execute();
            //将输液管转入细胞培养瓶  1-3-8
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-7", true))
               .Append(new CheckSmallAction("1-3-8", false))
               .Append(new UpdateSmallAction("1-3-8", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, infusionTube_02.gameObject))
               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.0026f, 0f, 0.14f), 0.3f))
               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.161f, 0f, 0.14f), 0.3f))
               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.161f, 0f, -0.226f), 0.3f,true))
               //.Append(new DOLocalRotaAction(deskBottle_02Cap, new Vector3(-90f, 180f, 90f), 0.3f))
               .Append(new InvokeTimeLineAction(infusionTube_02.gameObject, 1, true,2.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将输液管转入细胞培养瓶", Color.green))
               .Append(new InvokeCurrentGuideAction(9))
               .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
               .Execute();
            //将输液管转入培养基瓶  1-3-11
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-10", true))
               .Append(new CheckSmallAction("1-3-11", false))
               .Append(new UpdateSmallAction("1-3-11", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, infusionTube_02.gameObject))
               .Append(new InvokeTimeLineAction(infusionTube_02.gameObject, 1, true,2.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将输液管转入培养基瓶", Color.green))
               .Append(new InvokeCurrentGuideAction(12))
               .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
               .Execute();
            //输液管消毒封口  1-3-14
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-13", true))
               .Append(new CheckSmallAction("1-3-14", false))
               .Append(new UpdateSmallAction("1-3-14", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, infusionTube_02.gameObject))
               .Append(new InvokeTimeLineAction(infusionTube_02.gameObject, 1, true,2f))
               .Append(new GameObjectAction(infusionTube_Paper02))
               .Append(new DOLocalMoveAction(infusionTube_Paper02, new Vector3(-0.9213f, 0.061f, 0.2904f), 0.5f))
               .Append(new GameObjectAction(infusionTube, true))
               .Append(new GameObjectAction(infusionTubeLine, true))
               .Append(new GameObjectAction(infusionTube_02,false))
               .Append(new GameObjectAction(infusionTubeLine_02,false))
               .Append(new GameObjectAction(alcoholLampFire.gameObject,false))
               .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0915f, 0.0071f, 0.0669f), 0.3f))
               .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0098f, 0.0071f, 0.0669f), 0.3f))
               .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0098f, 0.0071f, 0.0141f), 0.4f))

               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.161f, 0f, 0.14f), 0.3f,true))
               //.Append(new DOLocalRotaAction(deskBottle_02Cap, new Vector3(-90f, 0f, 90f), 0.3f))
               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.0026f, 0f, 0.14f), 0.3f))
               //.Append(new DOLocalMoveAction(deskBottle_02Cap, new Vector3(0.0026f, 0f, 0.0505f), 0.3f))

               //.Append(new DOLocalMoveAction(deskBottle_01Cap,new Vector3 (0.115f,0, 0.093f),0.5f,true))
               //.Append(new DOLocalRotaAction(deskBottle_01Cap, new Vector3(-90f, 0f, 90f), 0.3f))
               //.Append(new DOLocalMoveAction(deskBottle_01Cap, new Vector3(0.002601624f, 0, 0.093f),0.5f))
               //.Append(new DOLocalMoveAction(deskBottle_01Cap, new Vector3(0.002601624f, 0, 0.0505f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已将输液管消毒封口", Color.green))
               .Append(new GameObjectAction(deskBottle_01,false))
               .Append(new GameObjectAction(deskBottle_02, false))
               .Append(new InvokeCurrentGuideAction(15))
               .Append(new InvokeFlashAction(true, Tank3L_Power.gameObject))
               .Execute();
        }
        /// <summary>
        /// 输液管连接  1-3-4      对输液管裸露部分包扎封口   1-1-8
        /// </summary>
        /// <param name="arg0"></param>
        private void InfusionTube_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-3", true))
                .Append(new CheckSmallAction("1-3-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, infusionTube.gameObject))
                .Append(new GameObjectAction(infusionTube, false))
                .Append(new GameObjectAction(infusionTubeLine, false))
                .Append(new GameObjectAction(infusionTube_02))
                .Append(new GameObjectAction(infusionTubeLine_02))           
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已连接输液管", Color.green))
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new InvokeFlashAction(true, infusionTube_02.gameObject))
                .Append(new UpdateSmallAction("1-3-4", true))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                 .Append(new CheckSmallAction("1-1-7", true))
                 .Append(new CheckSmallAction("1-1-8", false))
                 .Append(new InvokeCloseAllGuideAction())
                 .Append(new GameObjectAction(infusionTube_Paper))
                 .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已包扎输液管封口", Color.green))
                 .Append(new InvokeCurrentGuideAction(9))
                 .Append(new UpdateSmallAction("1-1-8", true))
                 .Append(new InvokeFlashAction(true, Tank3L.gameObject))
                 .Execute();
        }
        /// <summary>
        /// 领取细胞培养瓶   1-3-2
        /// </summary>
        /// <param name="arg0"></param>
        private void PassWindowBottle_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-1", true))
                .Append(new CheckSmallAction("1-3-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, passWindowBottle.gameObject))
                .Append(new GameObjectAction(passWindowBottle,false))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYP_01.ToString(), UpdateType.Add))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已领取细胞培养瓶", Color.green))
                .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, -180), 0.5f))
                .Append(new DOLocalRotaAction(passWindowHand, new Vector3(0, 0, 0), 0.5f))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, desk.gameObject))
                .Append(new UpdateSmallAction("1-3-2", true))
                .Execute();
        }
        /// <summary>
        /// 打开传递窗   1-3-1
        /// </summary>
        /// <param name="arg0"></param>
        private void PassWindowDoor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, passWindowDoor.gameObject))
                .Append(new DOLocalRotaAction(passWindowHand,new Vector3 (0,0,90),0.5f))
                .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, -270), 0.5f))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开传递窗", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new InvokeFlashAction(true, passWindowBottle.gameObject))
                .Append(new UpdateSmallAction("1-3-1", true))
                .Execute();
        }
        /// <summary>
        /// 放置培养罐  1-2-3   放置细胞培养瓶   1-3-3
        /// </summary>
        /// <param name="arg0"></param>
        private void Desk_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 放置培养罐  1-2-3
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-2", true))
                .Append(new CheckSmallAction("1-2-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_PYG_01))
                .Append(new InvokeFlashAction(false, desk.gameObject))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYG_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(Tank3L, true))
                .Append(new GameObjectAction(Tank3L_Paper, true))
                .Append(new GameObjectAction(PHPole, true))
                .Append(new GameObjectAction(TPole, true))
                .Append(new GameObjectAction(DOPole, true))
                .Append(new GameObjectAction(infusionTube, true))
                .Append(new GameObjectAction(infusionTubeLine, true))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置培养罐", Color.green))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new InvokeFlashAction(true, motor.gameObject))
                .Append(new UpdateSmallAction("1-2-3", true))
                .Execute();
            // 放置细胞培养瓶   1-3-3
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new CheckSmallAction("1-3-2", true))
                .Append(new CheckSmallAction("1-3-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_PYP_01))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYP_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(deskBottle_01, true))
               // .Append(new GameObjectAction(Tank3L_Paper, true))
                .Append(new GameObjectAction(deskBottle_02, true))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置细胞培养瓶", Color.green))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new InvokeFlashAction(true, infusionTube.gameObject))
                .Append(new UpdateSmallAction("1-3-3", true))
                .Execute();
        }
        /// <summary>
        /// 取出培养罐  1-2-2
        /// </summary>
        /// <param name="arg0"></param>
        private void Autoclave_Tank3L_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-1", true))
                .Append(new CheckSmallAction("1-2-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, autoclave_Tank3L.gameObject))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYG_01.ToString(), UpdateType.Add))
                .Append(new DOLocalMoveAction(autoclave_Tank3L, new Vector3(-34.758f, 8.67f, -45.601f), 1))
                .Append(new GameObjectAction(autoclave_Tank3L,false))
                .Append(new DOLocalRotaAction(autoclave_Cap, new Vector3(0, 0, 45), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取出培养罐", Color.green))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, desk.gameObject))
                .Append(new UpdateSmallAction("1-2-2", true))
                .Execute();
        }
        /// <summary>
        /// 放入培养罐  1-1-11
        /// </summary>
        /// <param name="arg0"></param>
        private void Autoclave_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-10", true))
                .Append(new CheckSmallAction("1-1-11", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckGoodsAction(eventData, GoodsType.Cell_PYG_01))
                .Append(new InvokeFlashAction(false, autoclave.gameObject))
                .Append(new UpdateGoodsAction(GoodsType.Cell_PYG_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(autoclave_Tank3L))
                .Append(new DOLocalMoveAction(autoclave_Tank3L, new Vector3(-34.758f, 8.038f, -45.601f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放入培养罐", Color.green))
                .Append(new InvokeCurrentGuideAction(12))
                .Append(new InvokeFlashAction(true, autoclave_Cap.gameObject))
                .Append(new UpdateSmallAction("1-1-11", true))
                .Execute();
        }
        /// <summary>
        /// 打开灭菌锅   1-1-10   1-2-1  关闭灭菌锅盖子  1-1-12
        /// </summary>
        /// <param name="arg0"></param>
        private void Autoclave_Cap_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, autoclave_Cap.gameObject))
                .Append(new DOLocalRotaAction(autoclave_Cap, new Vector3(0, 0, -70), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开灭菌锅盖子", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new InvokeFlashAction(true, autoclave_Tank3L.gameObject))
                .Append(new UpdateSmallAction("1-2-1", true))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-9", true))
                .Append(new CheckSmallAction("1-1-10", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, autoclave_Cap.gameObject))
                .Append(new DOLocalRotaAction(autoclave_Cap,new Vector3 (0,0,-70),1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开灭菌锅盖子", Color.green))
                .Append(new InvokeCurrentGuideAction(11))
                .Append(new InvokeFlashAction(true, autoclave.gameObject))
                .Append(new UpdateSmallAction("1-1-10", true))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-11", true))
                .Append(new CheckSmallAction("1-1-12", false))
                .Append(new UpdateSmallAction("1-1-12", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, autoclave_Cap.gameObject))
                .Append(new DOLocalRotaAction(autoclave_Cap, new Vector3(0, 0, 45), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭灭菌锅盖子", Color.green))
                .Append(new GameObjectAction(tank3LPanel))
                .Append(new CoroutineDigitalMeterAction(autoclaveDMComponent.gameObject,121,5))
                .Append(new GameObjectAction(autoclave_Tip))
                .Append(new DelayedAction(2))
                .Append(new GameObjectAction(tank3LPanel,false))               
                .OnCompleted(() =>
                {
                    MessageBoxEx.Show("121摄氏度下高压灭菌30分钟，灭菌完成", "提示", MessageBoxExEnum.SingleDialog, (X) =>
                    {
                     Task.NewTask()
                        .Append(new InvokeCompletedAction(1, 1))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, autoclave_Cap.gameObject))
                        .Execute();
                    });
                })
                .Execute();
        }
        /// <summary>
        ///   拆除电缆线  1-1-5   对裸露部分包扎封口   1-1-6    安装输液管  1-1-7   取出培养罐去灭菌  1-1-9   拆除包扎 1-2-5   连接各处控制器 1-2-6   使用硅胶管连接3L培养罐与20L培养罐  1-4-3
        /// </summary>
        /// <param name="arg0"></param>
        private void Tank3L_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-4-3  使用硅胶管连接3L培养罐与20L培养罐
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, false))
                .Append(new CheckSmallAction("1-4-2", true))
                .Append(new CheckSmallAction("1-4-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, Tank3L.gameObject))
                .Append(new GameObjectAction(fireCycle_20))
                .Append(new InvokeCameraLookPointAction("观察20L火焰线圈",0.5f))
                .Append(new GameObjectAction(softPipe))
                .Append(new DOLocalMoveAction(softPipe,new Vector3 (0,0,0),2,true))
                .Append(new DelayedAction(2))
                .Append(new GameObjectAction(fireCycle_20,false))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已连接3L培养罐与20L培养罐", Color.green))
                .Append(new GameObjectAction(receiveMachinePanel))
                .Append(new GameObjectAction(receiveMachineObj))
                .Append(new DelayedAction(9))              
                .OnCompleted(() =>
                {
                    receiveMachineAngle.position = new Vector3(-32.34361f, 8.812138f, -49.86015f);
                    receiveMachineAngle.eulerAngles = new Vector3(22.346f, -179.313f, 0f);
                    Task.NewTask()
                        .Append(new DelayedAction(2))
                        .Append(new GameObjectAction(receiveMachinePanel,false))
                        .Append(new GameObjectAction(receiveMachineObj,false))
                        .Append(new InvokeCompletedAction(1, 4))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, Tank20L_PowerButton.gameObject))
                        .Execute();
                })
                .Execute();
            //1-2-6  连接各处控制器
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-5", true))
                .Append(new CheckSmallAction("1-2-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(variousControllers))
                .Append(new GameObjectAction(infusionTube))
                .Append(new GameObjectAction(infusionTubeLine))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已连接各处控制器", Color.green))
                .Append(new InvokeCompletedAction(1,2))
                .Append(new InvokeCurrentGuideAction(1))
                .Append(new InvokeFlashAction(true, passWindowDoor.gameObject))
                .Append(new UpdateSmallAction("1-2-6", true))
                .Execute();
            //1-2-5  拆除包扎
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new CheckSmallAction("1-2-4", true))
                .Append(new CheckSmallAction("1-2-5", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(Tank3L_Paper,false))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除包扎", Color.green))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new UpdateSmallAction("1-2-5", true))
                .Execute();
            //1-1-7    安装输液管
            Task.NewTask(eventData.pointerEnter)
              .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
              .Append(new CheckSmallAction("1-1-6", true))
              .Append(new CheckSmallAction("1-1-7", false))
              .Append(new InvokeCloseAllGuideAction())
              .Append(new GameObjectAction(infusionTube))
              .Append(new GameObjectAction(infusionTubeLine))
              .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已安装输液管", Color.green))
              .Append(new InvokeCurrentGuideAction(8))
              .Append(new InvokeFlashAction(true, infusionTube.gameObject))
              .Append(new UpdateSmallAction("1-1-7", true))
              .Execute();
            //1-1-6    对裸露部分包扎封口
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-5", true))
                .Append(new CheckSmallAction("1-1-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(Tank3L_Paper))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已包扎封口", Color.green))
                .Append(new InvokeCurrentGuideAction(7))
                .Append(new UpdateSmallAction("1-1-6", true))
                .Execute();
            //1-1-9   取出培养罐去灭菌
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
               .Append(new CheckSmallAction("1-1-8", true))
               .Append(new CheckSmallAction("1-1-9", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(Tank3L,false))
               .Append(new GameObjectAction(Tank3L_Paper,false))
               .Append(new GameObjectAction(PHPole, false))
               .Append(new GameObjectAction(TPole, false))
               .Append(new GameObjectAction(DOPole, false))
               .Append(new GameObjectAction(infusionTube, false))
               .Append(new GameObjectAction(infusionTubeLine, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取出培养罐", Color.green))
               .Append(new UpdateGoodsAction(GoodsType.Cell_PYG_01.ToString(), UpdateType.Add))
               .Append(new InvokeCurrentGuideAction(10))
               .Append(new InvokeFlashAction(true, autoclave_Cap.gameObject))
               .Append(new UpdateSmallAction("1-1-9", true))
               .Execute();
            // 拆除电缆线  1-1-5
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-4", true))
                .Append(new CheckSmallAction("1-1-5", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new GameObjectAction(PHPoleLine, false))
                .Append(new GameObjectAction(TPoleLine, false))
                .Append(new GameObjectAction(DOPoleLine, false))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除电缆线", Color.green))
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new UpdateSmallAction("1-1-5", true))
                .Execute();

        }
        /// <summary>
        /// 安装溶氧电极  1-1-4
        /// </summary>
        /// <param name="arg0"></param>
        private void DOPole_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-3", true))
                .Append(new CheckSmallAction("1-1-4", false))
                .Append(new CheckSmallAction("安装溶氧电极", false))
                .Append(new UpdateSmallAction("安装溶氧电极", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, DOPole.gameObject))
                .Append(new DOLocalMoveAction(DOPole, new Vector3(-33.3838f, 8.921f, -49.9334f), 1))
                .Append(new DOLocalMoveAction(DOPole, new Vector3(-33.2683f, 8.921f, -50.0073f), 1))
                .Append(new DOLocalMoveAction(DOPole, new Vector3(-33.2683f, 8.6483f, -50.0073f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已安装溶氧电极", Color.green))
                .Append(new GameObjectAction(breaker,false))
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new InvokeFlashAction(true, Tank3L.gameObject))
                .Append(new UpdateSmallAction("1-1-4", true))
                .Execute();
        }
        /// <summary>
        /// 安装温度电极  1-1-3
        /// </summary>
        /// <param name="arg0"></param>
        private void TPole_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-2", true))
                .Append(new CheckSmallAction("1-1-3", false))
                .Append(new CheckSmallAction("安装温度电极", false))
                .Append(new UpdateSmallAction("安装温度电极", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, TPole.gameObject))
                .Append(new DOLocalMoveAction(TPole, new Vector3(-33.4055f, 8.921f, -49.9457f), 1))
                .Append(new DOLocalMoveAction(TPole, new Vector3(-33.2482f, 8.921f, -50.099f), 1))
                .Append(new DOLocalMoveAction(TPole, new Vector3(-33.2482f, 8.5909f, -50.099f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已安装温度电极", Color.green))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new InvokeFlashAction(true, DOPole.gameObject))
                .Append(new UpdateSmallAction("1-1-3", true))
                .Execute();
        }
        /// <summary>
        /// 安装PH电极  1-1-2
        /// </summary>
        /// <param name="arg0"></param>
        private void PHPole_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-1", true))
                .Append(new CheckSmallAction("1-1-2", false))
                .Append(new CheckSmallAction("安装PH电极", false))
                .Append(new UpdateSmallAction("安装PH电极", true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, PHPole.gameObject))
                .Append(new DOLocalMoveAction(PHPole, new Vector3(-33.429f, 8.921f, -49.9913f), 1))
                .Append(new DOLocalMoveAction(PHPole, new Vector3(-33.2765f, 8.921f, -50.0505f), 1))
                .Append(new DOLocalMoveAction(PHPole, new Vector3(-33.2765f, 8.6466f, -50.0505f), 1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已安装PH电极", Color.green))
                .Append(new InvokeCurrentGuideAction(3))
                .Append(new InvokeFlashAction(true, TPole.gameObject))
                .Append(new UpdateSmallAction("1-1-2", true))
                .Execute();
        }
        /// <summary>
        /// 取下搅拌电机   1-1-1  安装搅拌电机  1-2-4
        /// </summary>
        /// <param name="arg0"></param>
        private void Motor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
                .Append(new CheckSmallAction("1-1-1", false))
                .Append(new CheckSmallAction("取下搅拌电机", false))
                .Append(new UpdateSmallAction("取下搅拌电机", true))
                .Append(new InvokeCloseAllGuideAction())              
                .Append(new InvokeFlashAction(false, motor.gameObject))
                .Append(new InvokeTimeLineAction(motor.gameObject,-1))
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已取下搅拌电机", Color.green))
                .Append(new InvokeCurrentGuideAction(2))
                .Append(new UpdateSmallAction("1-1-1", true))
                .Append(new InvokeFlashAction(true, PHPole.gameObject))
                .Execute();
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
               .Append(new CheckSmallAction("1-2-3", true))
               .Append(new CheckSmallAction("1-2-4", false))
               .Append(new CheckSmallAction("取下搅拌电机", true))
               .Append(new UpdateSmallAction("取下搅拌电机", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, motor.gameObject))
               .Append(new InvokeTimeLineAction(motor.gameObject, 1))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已安装搅拌电机", Color.green))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new InvokeFlashAction(true, Tank3L.gameObject))
               .Append(new UpdateSmallAction("1-2-4", true))
               .Execute();
        }
    }
}
