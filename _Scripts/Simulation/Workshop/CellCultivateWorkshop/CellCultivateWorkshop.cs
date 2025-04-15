using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Common;
using XFramework.Actions;
using UnityEngine;
using XFramework.Module;
using XFramework.PLC;
using XFramework.Component;

namespace XFramework.Simulation
{
   public partial class CellCultivateWorkshop:CustomWorkshop
    {
        /// <summary>
        /// 细胞培养车间
        /// </summary>
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.CellCultivateWorkshop;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            EventDispatcher.RegisterEvent<string,string,string,string>("参数设置完成", IOSetPanelSaveEvent_callBack);
            EventDispatcher.RegisterEvent<IOSetPanelType>(Events.PLC.PLCToWorkshop, ReceivePLCToWorkshopEvent_callBack);
            EventDispatcher.RegisterEvent<string>("2D阀门自动关闭", ValveAutoCloseEvent_callBack);
            EventDispatcher.RegisterEvent("20L罐加入培养基", AddMedium20LEvent_callBack);
            EventDispatcher.RegisterEvent("200L罐加入培养基", AddMedium200LEvent_callBack);
            EventDispatcher.RegisterEvent<IOSetPanelType,string>("设置培养罐加入培养基参数结束", AddMediumEndEvent_callBack);
            EventDispatcher.RegisterEvent<NPCControl, Goods>(Events.Entity.Drop, NPCControl_Drop);
        }
        protected override void ResetProduceInitialize(int index)
        {
            base.ResetProduceInitialize(index);
            MyselfControl.GetComponent<MyselfControl>().disable = false;
            NpcCtrl.transform.position = new Vector3(-33.13f, 7.5f, -45.68f);
            NpcCtrl.transform.eulerAngles = new Vector3(0, 180, 0);
            NpcCtrl.IsJumping = true;
            NpcCtrl.agent.isStopped = true;
            GameObject.Find("土建/细胞培养2_门/细胞培养2_门002/单扇门_门013").transform.localEulerAngles=new Vector3 (0,-180,0);
            switch (index)
            {
                case 1:
                    //MyselfControl.transform.position = new Vector3(-30, 7.5f, -47);
                    //MyselfControl.transform.eulerAngles = new Vector3(0, -135f, 0);
                   // Camera.main.transform.eulerAngles = MyselfControl.transform.eulerAngles;
                    EventDispatcher.ExecuteEvent<string>("3L培养罐液位参数变化", "0");
                    EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_3L, false);
                    receiveMachineAngle.position = new Vector3(-32.34361f, 8.812138f, -49.86015f);
                    receiveMachineAngle.eulerAngles = new Vector3(22.346f, -179.313f, 0f);
                    deskBottle_01Flu.Value = 0.3f;
                    Tank3L_Flu.Value = 0f;
                    
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, motor.gameObject))
                        .Append(new GameObjectAction(motor.gameObject, true))
                        .Append(new InvokeTimeLineAction(motor.gameObject, 1,true))
                        .Append(new DOLocalMoveAction(PHPole, new Vector3(-33.429f, 8.604f, -49.931f), 0.5f, true)) //ph电极复位
                        .Append(new DOLocalMoveAction(TPole, new Vector3(-33.4055f, 8.537f, -49.9457f), 0.5f, true))  //温度电极复位
                        .Append(new DOLocalMoveAction(DOPole, new Vector3(-33.3838f, 8.6018f, -49.9334f), 0.5f, true))     //溶氧电极复位
                        .Append(new GameObjectAction(PHPoleLine, true))
                        .Append(new GameObjectAction(TPoleLine, true))
                        .Append(new GameObjectAction(DOPoleLine, true))
                        .Append(new GameObjectAction(breaker, true))
                        .Append(new GameObjectAction(Tank3L_Paper, false))
                        .Append(new GameObjectAction(infusionTube, false))
                        .Append(new GameObjectAction(infusionTubeLine, false))
                        .Append(new GameObjectAction(infusionTube_Paper, false))
                        .Append(new DOLocalRotaAction(autoclave_Cap, new Vector3(0, 0, 45), 0.5f, true))
                        .Append(new GameObjectAction(tank3LPanel, false))
                        .Append(new CoroutineDigitalMeterAction(autoclaveDMComponent.gameObject, 0, 0.5f, true))
                        .Append(new GameObjectAction(autoclave_Tip, false))
                        .Append(new GameObjectAction(Tank3L, true))
                        .Append(new GameObjectAction(PHPole, true))
                        .Append(new GameObjectAction(TPole, true))
                        .Append(new GameObjectAction(DOPole, true))
                        .Append(new GameObjectAction(variousControllers, false))
                        .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, -180), 0.5f, true))
                        .Append(new DOLocalRotaAction(passWindowHand, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new GameObjectAction(passWindowBottle, true))
                        .Append(new GameObjectAction(deskBottle_01, false))
                        .Append(new GameObjectAction(deskBottle_02, false))
                        .Append(new GameObjectAction(infusionTube_02, false))
                        .Append(new GameObjectAction(infusionTubeLine_02, false))
                      //  .Append(new InvokeTimeLineAction(infusionTube_02.gameObject, -1, true))

                        .Append(new GameObjectAction(infusionTube_02Flu, false))
                        .Append(new UpdateMaterialAction(infusionTubeLine_02.gameObject, m_UpdateMaterialComponent.OldMaterial))
                        .Append(new InvokeFluidAction(fluidComponent, 0))
                        .Append(new GameObjectAction(Tank3L_water, false))
                        .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                        .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))

                        .Append(new GameObjectAction(pumpStartPowerOpen, false))
                        .Append(new GameObjectAction(alcoholLampFire.gameObject, false))
                        .Append(new DOLocalMoveAction(alcoholLampCap, new Vector3(-0.0098f, 0.0071f, 0.0141f), 0.5f, true))
                        .Append(new GameObjectAction(infusionTube_Paper02, false))
                        .Append(new DOLocalMoveAction(infusionTube_Paper02, new Vector3(-0.9213f, 0.0613f, 0.2904f), 0.5f, true))
                        .Append(new DOLocalRotaAction(Tank3L_Power, new Vector3(0, 90, 90), 0.5f, true))
                        .Append(new GameObjectAction(Tank3L_PLC, false))
                        .Append(new GameObjectAction(injectorParent, true))
                        .Append(new DOLocalMoveAction(injector, new Vector3(0, 0, 0.01219141f), 0.5f, true))
                        .Append(new DOLocalMoveAction(softPipe, new Vector3(0, 0.3f, 0), 0.5f, true))
                        .Append(new GameObjectAction(softPipe, false))
                        .Append(new GameObjectAction(fireCycle_20, false))
                        .Append(new GameObjectAction(receiveMachinePanel, false))
                        .Append(new GameObjectAction(receiveMachineObj, false))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_HYXQ_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_01.ToString(), UpdateType.Add))
                        .OnCompleted(() =>
                        {
                        })
                        .Execute();
                    break;
                case 2:
                    //MyselfControl.transform.position = new Vector3(-30, 7.5f, -47);
                    //MyselfControl.transform.eulerAngles = new Vector3(0, -135f, 0);
                   // Camera.main.transform.eulerAngles = MyselfControl.transform.eulerAngles;
                    if (pLC_20L.gameObject.activeSelf)
                    {
                        pLC_20L.ResetParameter();
                    }                   
                    EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_20L, false);
                    PipeFittingManager.Instance.CloseAll();
                    Tank3L_Flu.Value = 0.6f;
                    Tank20LFlu.Value = 0;
                    sampleBottleFlu.Value = 0;
                    softPipe.gameObject.SetActive(true);
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, Tank20L_PowerButton.gameObject))
                        .Append(new DOLocalRotaAction(Tank20L_PowerButton, new Vector3(0, 90, 90), 0.5f,true))
                        .Append(new GameObjectAction(Tank20L_PLC,false))
                        .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, false))
                        .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, false))
                        .Append(new GameObjectAction(Tank20L_WaterEffect_02, false))
                        .Append(new GameObjectAction(pumpStartPowerOpen, false))

                        .Append(new GameObjectAction(Tank20L_WaterEffect, false))
                        .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                        .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                        .Append(new GameObjectAction(softPipe))
                        .Append(new DOLocalMoveAction(softPipe, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new UpdateMaterialAction(softPipe.gameObject, m_UpdateMaterialComponent.OldMaterial))
                        .Append(new GameObjectAction(fireCircle,false))
                        .Append(new GameObjectAction(sampleBottle,false))
                        .Append(new DOLocalRotaAction(V012Hand, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new DOLocalRotaAction(V011Hand, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new GameObjectAction(tubeSealingMachinePanel, false))
                        .Append(new GameObjectAction(tubeSealingMachineObj, false))
                      
                        .Append(new UpdateGoodsAction(GoodsType.Cell_HYXQ_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_01.ToString(), UpdateType.Add))
                        .Execute();
                    break;
                case 3:
                    //MyselfControl.transform.position = new Vector3(-30, 7.5f, -47);
                    //MyselfControl.transform.eulerAngles = new Vector3(0, -135f, 0);
                    //Camera.main.transform.eulerAngles = MyselfControl.transform.eulerAngles;
                    pLC_200L.ResetParameter();
                    EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_200L, false);
                    PipeFittingManager.Instance.CloseAll();
                    Tank200LFlu.Value = 0;
                    sampleBottleFlu_200L.Value = 0;
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, Tank200L_PowerButton.gameObject))
                        .Append(new DOLocalRotaAction(Tank200L_PowerButton, new Vector3(0, 90, 90), 0.5f,true))
                        .Append(new GameObjectAction(Tank200L_PLC,false))
                        .Append(new GameObjectAction(softPipe,false))
                        .Append(new UpdateTransparencyAction(Tank200L_Shell.gameObject, false))
                        .Append(new GameObjectAction(Tank200L_WaterEffect, false))
                        .Append(new GameObjectAction(fireCircle_200L,false))
                        .Append(new GameObjectAction(sampleBottle_200L,false))
                        .Append(new DOLocalRotaAction(V030Hand, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new DOLocalRotaAction(V031Hand, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new GameObjectAction(sampleBottle_PutArea_200L, false))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_HYXQ_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_01.ToString(), UpdateType.Add))
                        .Execute();
                    break;
                case 4:
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, V041Hand.gameObject))
                        .Append(new GameObjectAction(softPipe, false))
                        .Append(new DOLocalRotaAction(V041Hand, new Vector3(0, -90, 90), 0.5f,true))
                        .Append(new DOLocalRotaAction(V040Hand, new Vector3(0, 0, 90), 0.5f,true))
                        .Execute();
                    
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///   误操作 计数
        /// </summary>
        private int misoperationHandlerCount = 0;
        private void IOSetPanelSaveEvent_callBack(string pH, string pD, string speed, string tem)
        {
            if (!pH.Equals("7.2"))
            {
                misoperationHandlerCount++;
            }
            if (!pD.Equals("20"))
            {
                misoperationHandlerCount++;
            }
            if (!speed.Equals("100"))
            {
                misoperationHandlerCount++;
            }
            if (!tem.Equals("37"))
            {
                misoperationHandlerCount++;
            }
            if (pH.Equals("7.2") && pD.Equals("20") && speed.Equals("100") && tem.Equals("37"))
            {
                Task.NewTask()
                    .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "pH7.2，溶氧饱和度20%，搅拌速率100rpm,温度37℃", Color.green))
                    .Execute();
            }
            else
            {
                Task.NewTask()
                   .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "pH" + pH + "溶氧饱和度" + pD + "%，搅拌速率" + speed + "rpm，温度"+tem+ "℃", Color.red))
                   .Execute();
            }
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
            }
            misoperationHandlerCount = 0;
        }
        /// <summary>
        /// 向QA提交注射器    1-4-2       将取样瓶交给QA   2-4-7   3-4-7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void NPCControl_Drop(NPCControl ctrl, Goods gooodsItem)
        {
            switch (gooodsItem.GoodsType)
            {
                case GoodsType.Cell_ZSQ_01:
                    Task.NewTask()
                      .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
                      .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, false))
                      .Append(new CheckSmallAction("1-4-1", true))
                      .Append(new CheckSmallAction("1-4-2", false))
                      .Append(new InvokeCloseAllGuideAction())
                      .Append(new InvokeFlashAction(false, ctrl.gameObject))
                      .Append(new UpdateGoodsAction(GoodsType.Cell_ZSQ_01.ToString(), UpdateType.Remove))
                      .OnCompleted(() =>
                      {
                          ctrl.Speak("请稍等，我将样品送检");
                          Transform[] paths = workshopPath.Points;
                          ctrl.StartRuning(paths,() =>
                          {
                              Task.NewTask()
                                  .Append(new InvokeCurrentGuideAction(3))
                                  .Append(new UpdateSmallAction("1-4-2", true))
                                  .Append(new InvokeFlashAction(true, Tank3L.gameObject))
                                  .Execute();
                          }, "浓度达到要求");
                      })
                      .Execute();   
                    break;
                case GoodsType.Cell_QYP_02:
                    ctrl.AffectedByNPC = true;
                    Task.NewTask()
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 3, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 4, false))
                        .Append(new CheckSmallAction("2-4-6", true))
                        .Append(new CheckSmallAction("2-4-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, ctrl.gameObject))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_02.ToString(), UpdateType.Remove))
                        .OnCompleted(() =>
                        {
                            ctrl.Speak("请稍等，我将样品送检");
                            Transform [] paths= workshopPath.Points;
                            ctrl.StartRuning(paths,() =>
                            {
                                Task.NewTask()
                                    .Append(new InvokeCurrentGuideAction(8))
                                    .Append(new UpdateSmallAction("2-4-7", true))
                                    .OnCompleted(() =>
                                    {
                                        EventDispatcher.ExecuteEvent<string>("20L培养罐阀门点击前", "2-4-8-1");
                                    })
                                    .Execute();
                            }, "浓度达到要求");
                        })
                        .Execute();
                    Task.NewTask()
                      .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
                      .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 4, false))
                      .Append(new CheckSmallAction("3-4-6", true))
                      .Append(new CheckSmallAction("3-4-7", false))
                      .Append(new InvokeCloseAllGuideAction())
                      .Append(new InvokeFlashAction(false, ctrl.gameObject))
                      .Append(new UpdateGoodsAction(GoodsType.Cell_QYP_02.ToString(), UpdateType.Remove))
                      .OnCompleted(() =>
                      {
                          ctrl.Speak("请稍等，我将样品送检");
                          Transform[] paths = workshopPath.Points;
                          ctrl.StartRuning(paths,() =>
                          {
                              Task.NewTask()
                                  .Append(new InvokeCurrentGuideAction(8))
                                  .Append(new UpdateSmallAction("3-4-7", true))
                                  .OnCompleted(() =>
                                  {
                                      EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-4-8-1");
                                  })
                                  .Execute();
                          }, "浓度达到要求");
                      })
                      .Execute();
                    break;
                default:
                    ctrl.Speak(gooodsItem.Description + " ?/n请将正确的样品交给我");
                    break;
            }
        }
        private void AddMediumEndEvent_callBack(IOSetPanelType type,string obj)
        {
            if (obj.Equals("15") && type.Equals(IOSetPanelType.Type_20L))
            {               
                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                EventDispatcher.ExecuteEvent<string>("20L培养罐阀门点击前", "2-2-1-1");
            }
            else if (obj.Equals("150") && type.Equals(IOSetPanelType.Type_200L))
            {
                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-2-1-1");
            }
        }

        private void AddMedium200LEvent_callBack()
        {
            Task.NewTask()
                .Append(new InvokeCameraLookPointAction("观察200L罐灭菌", 0.5f))
                .Append(new UpdateTransparencyAction(Tank200L_Shell.gameObject, true))
                .Append(new GameObjectAction(Tank200L_WaterEffect))
                .Append(new DelayedAction(2.5f))
                .Append(new CoroutineFluidLevelAction(Tank200LFlu.gameObject, 0.65f,5.5f, false))
                .Append(new UpdateTransparencyAction(Tank200L_Shell.gameObject, false))
                .Append(new GameObjectAction(Tank200L_WaterEffect,false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string>("200L培养罐液位参数变化", "75");
                })
                .Execute();
        }
        private void AddMedium20LEvent_callBack()
        {
            Task.NewTask()
                .Append(new InvokeCameraLookPointAction("观察20L罐灭菌", 0.5f))
                .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, true))
                .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, true))
                .Append(new GameObjectAction(Tank20L_WaterEffect_02))
                .Append(new DelayedAction(2.5f))
                .Append(new CoroutineFluidLevelAction(Tank20LFlu.gameObject, 0.5f, 5.5f, false))
                .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, false))
                .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, false))
                .Append(new GameObjectAction(Tank20L_WaterEffect_02, false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string>("20L培养罐液位参数变化", "75");
                })
                .Execute();
        }

        private void ValveAutoCloseEvent_callBack(string message)
        {
            switch (message)
            {
                case "2-1-2-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new InvokeCameraLookPointAction("观察20L罐灭菌", 0.5f))
                       .Append(new ShowProgressAction("正在灭菌中...",8))
                       .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, true))
                       .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, true))
                       .Append(new GameObjectAction(gasEffect_20L))
                       .Append(new DelayedAction(8f))
                       .Append(new UpdateTransparencyAction(Tank20L_Shell.gameObject, false))
                       .Append(new UpdateTransparencyAction(Tank20L_Self.gameObject, false))
                       .Append(new GameObjectAction(gasEffect_20L, false))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭灭菌相关阀门", Color.green))
                       .OnCompleted(() =>
                       {
                           MessageBoxEx.Show("灭菌完成，已关闭灭菌相关阀门", "提示", MessageBoxExEnum.SingleDialog, x =>
                              {
                                  ProductionGuideManager.Instance.ShowCurrentGuide(3);
                                  EventDispatcher.ExecuteEvent<string>("20L培养罐阀门点击前", "2-1-3-1");
                              });                         
                       })
                       .Execute();
                    break;
                case "2-1-3-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new ShowProgressAction("正在冷却中...", 8))
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭冷却相关阀门", Color.green))
                       .OnCompleted(() =>
                       {
                           MessageBoxEx.Show("冷却完成，已关闭冷却相关阀门", "提示", MessageBoxExEnum.SingleDialog, x =>
                           {
                               Completed(2, 1);
                               //设置20L培养罐加入培养基参数
                               EventDispatcher.ExecuteEvent<IOSetPanelType> ("设置培养罐加入培养基参数", IOSetPanelType.Type_20L);                             
                           });                         
                       })
                       .Execute();
                    break;
                case "2-2-1-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已停止加入培养基", Color.green))
                       .Append(new UpdateSmallAction("2-2-1", true))
                       .Append(new InvokeCurrentGuideAction(2))
                       .Append(new InvokeFlashAction(true, pumpStartPower.gameObject))
                       .OnCompleted(() =>
                       {
                       })
                       .Execute();
                    break;
                case "2-4-1-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已停止取样消毒", Color.green))
                       .Append(new UpdateSmallAction("2-4-1", true))
                       .Append(new InvokeCurrentGuideAction(2))
                       .Append(new InvokeFlashAction(true, sampleBottle_PutArea.gameObject))
                       .OnCompleted(() =>
                       {
                       })
                       .Execute();
                    break;
                case "2-4-8-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new InvokeCompletedAction(2, 4))
                       .Append(new InvokeCurrentGuideAction(1))
                       .Append(new InvokeFlashAction(true, Tank200L_PowerButton.gameObject))
                       .OnCompleted(() =>
                       {
                       })
                       .Execute();
                    break;
                case "3-1-2-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new InvokeCameraLookPointAction("观察200L罐灭菌", 0.5f))
                       .Append(new ShowProgressAction("正在灭菌中...", 8))
                       .Append(new UpdateTransparencyAction(Tank200L_Shell.gameObject, true))
                       .Append(new GameObjectAction(gasEffect_200L))
                       .Append(new DelayedAction(8))
                       .Append(new UpdateTransparencyAction(Tank200L_Shell.gameObject, false))
                       .Append(new GameObjectAction(gasEffect_200L, false))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭灭菌相关阀门", Color.green))
                       .Append(new InvokeCurrentGuideAction(3))
                       .OnCompleted(() =>
                       {
                           MessageBoxEx.Show("灭菌完成，已关闭灭菌相关阀门", "提示", MessageBoxExEnum.SingleDialog, x =>
                           {
                               EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-1-3-1");
                           });                         
                       })
                       .Execute();
                    break;
                case "3-1-3-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new ShowProgressAction("正在冷却中...", 8))
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭冷却相关阀门", Color.green))
                       .OnCompleted(() =>
                       {
                           MessageBoxEx.Show("冷却完成，已关闭冷却相关阀门", "提示", MessageBoxExEnum.SingleDialog, x =>
                           {
                               Completed(3, 1);
                               //设置200L培养罐加入培养基参数
                               EventDispatcher.ExecuteEvent<IOSetPanelType>("设置培养罐加入培养基参数", IOSetPanelType.Type_200L);
                           });
                       })
                       .Execute();
                    break;
                case "3-2-1-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已停止加入培养基", Color.green))
                       .Append(new UpdateSmallAction("3-2-1", true))
                       .Append(new InvokeCurrentGuideAction(2))
                       .OnCompleted(() =>
                       {
                           EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-2-2");
                       })
                       .Execute();
                    break;
                case "3-2-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开进料阀", Color.green))
                       .Append(new InvokeCurrentGuideAction(3))
                       .OnCompleted(() =>
                       {
                           EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-2-3");
                       })
                       .Execute();
                    break;
                case "3-2-3":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭进料阀", Color.green))
                       .Append(new InvokeCompletedAction(3,2))
                       .Append(new InvokeCurrentGuideAction())
                       .OnCompleted(() =>
                       {
                           EventDispatcher.ExecuteEvent<IOSetPanelType, bool>("控制参数设置面板", IOSetPanelType.Type_200L, true);
                           EventDispatcher.ExecuteEvent<string>("20L培养罐液位参数变化", "0");
                           EventDispatcher.ExecuteEvent<string>("200L培养罐液位参数变化", "80");
                       })
                       .Execute();
                    break;
                case "3-4-1-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new DelayedAction(8))
                       .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已停止取样消毒", Color.green))
                       .Append(new UpdateSmallAction("3-4-1", true))
                       .Append(new InvokeCurrentGuideAction(2))
                       .Append(new InvokeFlashAction(true, sampleBottle_PutArea_200L.gameObject))
                       .OnCompleted(() =>
                       {
                       })
                       .Execute();
                    break;
                case "3-4-8-2":
                    Task.NewTask()
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new InvokeCompletedAction(3, 4))
                       .Append(new InvokeCurrentGuideAction(1))
                       .Append(new InvokeFlashAction(true, V041Hand.gameObject))
                       .OnCompleted(() =>
                       {
                       })
                       .Execute();
                    break;
                default:
                    break;
            }
        }

        private void ReceivePLCToWorkshopEvent_callBack(IOSetPanelType type)
        {
            switch (type)
            {
                case IOSetPanelType.Type_3L:
                    Task.NewTask()
                        .Append(new UpdateSmallAction("1-3-16", true))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已设置培养参数", Color.green))                       
                        .Append(new InvokeCameraLookPointAction("观察注射器", 0.5f))
                        .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, true))
                        .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, true))
                        .Append(new LoopRotateAction(agitatorBlade3L.gameObject, true, RotationAxis.Z, 500))
                        .Append(new DelayedAction(3))
                        .Append(new UpdateTransparencyAction(Tank3L_Shell.gameObject, false))
                        .Append(new UpdateTransparencyAction(Tank3L_Blanket.gameObject, false))
                        .Append(new InvokeCompletedAction(1, 3))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, injector.gameObject))
                        .OnCompleted(() =>
                        {

                        })
                        .Execute();
                    break;
                case IOSetPanelType.Type_20L:
                    Task.NewTask()
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已设置培养参数", Color.green))
                        .Append(new InvokeCompletedAction(2, 3))
                        .Append(new InvokeCurrentGuideAction(1))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent<string>("20L培养罐阀门点击前", "2-4-1-1");
                        })
                        .Execute();
                    break;
                case IOSetPanelType.Type_200L:
                    Task.NewTask()
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已设置培养参数", Color.green))
                        .Append(new InvokeCompletedAction(3, 3))
                        .Append(new InvokeCurrentGuideAction(1))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent<string>("200L培养罐阀门点击前", "3-4-1-1");
                        })
                        .Execute();
                    break;
                default:
                    break;
            }
        }

        protected override void InitStandard()
        {
            base.InitStandard();
            InitializeComponent();
            InitializeOperate();
        }
        protected override void OnRelease()
        {
            base.OnRelease();
            EventDispatcher.UnregisterEvent<IOSetPanelType>(Events.PLC.PLCToWorkshop, ReceivePLCToWorkshopEvent_callBack);
            EventDispatcher.UnregisterEvent<string>("2D阀门自动关闭", ValveAutoCloseEvent_callBack);
            EventDispatcher.UnregisterEvent("20L罐加入培养基", AddMedium20LEvent_callBack);
            EventDispatcher.UnregisterEvent("200L罐加入培养基", AddMedium200LEvent_callBack);
            EventDispatcher.UnregisterEvent<NPCControl, Goods>(Events.Entity.Drop, NPCControl_Drop);
            EventDispatcher.UnregisterEvent<string, string, string, string>("参数设置完成", IOSetPanelSaveEvent_callBack);
        }
    }
}
