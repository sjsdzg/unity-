using UnityEngine;
using XFramework.Common;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Actions;
using XFramework.UI;
using XFramework.Module;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.PLC;
using System.Collections.Generic;
using XFramework.Component;

namespace XFramework.Simulation
{
    public partial class CultureMediumPreparationWorkshop
    {
        #region 辅助变量
        /// <summary>
        /// 是否打开电源按钮
        /// </summary>
        //private bool isNeedIndicator_100L;
        #endregion
        private void InitializeOperate()
        {
            dropArea_balance.TriggerAction(EventTriggerType.Drop, DropArea_balance_Drop);
            button_couAff.onClick.AddListener(ButtonCouAff_onClick);
            medium_original.TriggerAction(EventTriggerType.PointerClick, MediumOriginal_PointerClick);
            //glassWindow.OnUsable.AddListener(GlassWindow_OnUsable);
            m_CameraSwitcher.OnCameraSwitch.AddListener(m_CameraSwitcher_OnCameraSwitch);
           // m_MouseOrbit.OnFocusCompleted.AddListener(m_MouseOrbit_OnFocusCompleted);
            ValveManager.Instance.OnClicked.AddListener(ValveManager_OnClicked);
            dropArea_bigBalance.TriggerAction(EventTriggerType.Drop, DropArea_bigBalance_Drop);
            button_couAff2.onClick.AddListener(ButtonCouAff2_onClick);
            bucket_F001S.TriggerAction(EventTriggerType.PointerClick, BucketF001S_PointerClick);
            powerButton_1.TriggerAction(EventTriggerType.PointerClick, PowerButton_1_PointerClick);
            cover_100L.TriggerAction(EventTriggerType.PointerClick, Cover_100L_PointerClick);
            entrance_100L.TriggerAction(EventTriggerType.Drop, Entrance_100L_Drop);
            filter.TriggerAction(EventTriggerType.PointerClick, Filter_PointerClick);
            console.TriggerAction(EventTriggerType.Drop, Console_Drop);
            filter_detect.TriggerAction(EventTriggerType.PointerClick, Filter_detect_PointerClick);
            filter.TriggerAction(EventTriggerType.Drop, Filter_Drop);
            sampleBottle_100L.TriggerAction(EventTriggerType.Drop, SampleBottle_100L_Drop);
            passBox.TriggerAction(EventTriggerType.Drop, PassBox_Drop);
            sampleValve_100L.TriggerAction(EventTriggerType.PointerClick, SampleValve_100L_PointerClick);
            powerButton_2.TriggerAction(EventTriggerType.PointerClick, PowerButton_2_PointerClick);
            cover_200L.TriggerAction(EventTriggerType.PointerClick, Cover_200L_PointerClick);
            entrance_200L.TriggerAction(EventTriggerType.Drop, Entrance_200L_Drop);
            sampleBottle_200L.TriggerAction(EventTriggerType.Drop, SampleBottle_200L_Drop);
            sampleValve_200L.TriggerAction(EventTriggerType.PointerClick, SampleValve_200L_PointerClick);
            filter_2.TriggerAction(EventTriggerType.PointerClick, Filter_2_PointerClick);
            filter_2.TriggerAction(EventTriggerType.Drop, Filter_2_Drop);

            plc_100LReservoirTank.OnOpen.AddListener(plc_100LReservoirTank_OnOpen);
            usable_100L.OnUsable.AddListener(Usable100LOnUse);
            usable_100L.OnExit.AddListener(Usable100LOnExit);
            plc_200LReservoirTank.OnOpen.AddListener(plc_200LReservoirTank_OnOpen);
            usable_200L.OnUsable.AddListener(Usable200LOnUse);
            usable_200L.OnExit.AddListener(Usable200LOnExit);
            usable_detector.OnUsable.AddListener(UsableDetectorOnUse);
            usable_detector.OnExit.AddListener(UsableDetectorOnExit);
            plcPipeFitting.ValveAfterEvent.AddListener(OnClickedAllValves);
            plcPipeFitting_Reservoir.ValveAfterEvent.AddListener(OnClickedAllValves);
            plcPipeFitting_2.ValveAfterEvent.AddListener(OnClickedAllValves_2);
            plcPipeFitting_Reservoir_2.ValveAfterEvent.AddListener(OnClickedAllValves_2);

            doorComponent.onOpened.AddListener(doorComponent_onOpened);

            EventDispatcher.RegisterEvent(PLC_100LDispensingTankEvent.SetWaterFinish, SetWaterFinish);
            EventDispatcher.RegisterEvent(PLC_100LDispensingTankEvent.StartMotor, StartMotor);
            EventDispatcher.RegisterEvent(PLC_100LDispensingTankEvent.StopMotor, StopMotor);
            EventDispatcher.RegisterEvent(PLC_100LDispensingTankEvent.StartSanitaryPump, StartSanitaryPump);
            EventDispatcher.RegisterEvent(PLC_100LDispensingTankEvent.StopSanitaryPump, StopSanitaryPump);
            EventDispatcher.RegisterEvent(PLC_FilterDetectorEvent.ButtonBubblePointClick, ButtonBubblePointClick);
            EventDispatcher.RegisterEvent(PLC_FilterDetectorEvent.ButtonOKClick, ButtonOKClick);
            EventDispatcher.RegisterEvent(PLC_FilterDetectorEvent.ButtonStartClick, ButtonStartClick);
            EventDispatcher.RegisterEvent(PLC_100LReservoirTankEvent.StartMotor, StartReservoirMotor);
            EventDispatcher.RegisterEvent(PLC_100LReservoirTankEvent.StopMotor, StopReservoirMotor);
            EventDispatcher.RegisterEvent(PLC_200LReservoirTankEvent.StartMotor, StartReservoirMotor_2);
            EventDispatcher.RegisterEvent(PLC_200LReservoirTankEvent.StopMotor, StopReservoirMotor_2);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.SetWaterFinish, SetWaterFinish_2);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.StartMotor, StartMotor_2);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.StopMotor, StopMotor_2);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.StartSanitaryPump, StartSanitaryPump_2);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.StopSanitaryPump, StopSanitaryPump_2);
            this.Invoke(0.2f, () =>
            {
                //Completed(1, 2);
                //SmallActionManager.Instance.UpdateSmallAction("2-4-10", true);
                //sampleBottle_100L.gameObject.SetActive(true);
                ProductionGuideManager.Instance.ShowCurrentGuide(1);

                
                FlashManager.Instance.ShowFlash(dropArea_balance.gameObject);
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001S_01.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001B_01.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_01.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_02.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_03.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_M20A_01.ToString());
                //EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001S_01.ToString());
                //plc_100L.gameObject.SetActive(true);
                //EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.CurrentSolutionType, PLC_100LDispensingTank.SolutionType.D);
                //filter_detect.gameObject.SetActive(true);
            });
            //this.Invoke(1f, () =>
            //{
            //    PipeFittingManager.Instance.TestPipeFitting();
            //});
        }

        private void plc_200LReservoirTank_OnOpen()
        {
            Task.NewTask()
                .Append(new CheckSmallAction("3-3-9", true))
                .Append(new CheckSmallAction("3-4-1", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_200LReservoirTankEvent.ActiveToggleMotor);
                }).Execute();
        }

        private void plc_100LReservoirTank_OnOpen()
        {
            Task.NewTask()
                .Append(new CheckSmallAction("2-3-9", true))
                .Append(new CheckSmallAction("2-4-1", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_100LReservoirTankEvent.ActiveToggleMotor);
                }).Execute();
            
        }

        private void Usable200LOnExit()
        {
            if (SmallActionManager.Instance.CheckSmallAction("3-1-1", true) && SmallActionManager.Instance.CheckSmallAction("3-1-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-2");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-2", true) && SmallActionManager.Instance.CheckSmallAction("3-1-3", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-3");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-5", true) && SmallActionManager.Instance.CheckSmallAction("3-1-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-6");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-6", true) && SmallActionManager.Instance.CheckSmallAction("3-1-7", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-7");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-7", true) && SmallActionManager.Instance.CheckSmallAction("3-1-8", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-8");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-8", true) && SmallActionManager.Instance.CheckSmallAction("3-1-9", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-9");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-1-9", true) && SmallActionManager.Instance.CheckSmallAction("3-1-10", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-1-10");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }

            if (SmallActionManager.Instance.CheckSmallAction("3-2-9", true) && SmallActionManager.Instance.CheckSmallAction("3-3-1", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-1");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-1", true) && SmallActionManager.Instance.CheckSmallAction("3-3-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-2");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-4", true) && SmallActionManager.Instance.CheckSmallAction("3-3-5", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-5");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-5", true) && SmallActionManager.Instance.CheckSmallAction("3-3-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-6");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-6", true) && SmallActionManager.Instance.CheckSmallAction("3-3-7", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-7");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-7", true) && SmallActionManager.Instance.CheckSmallAction("3-3-8", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-8");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-8", true) && SmallActionManager.Instance.CheckSmallAction("3-3-9", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-3-9");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-3-9", true) && SmallActionManager.Instance.CheckSmallAction("3-4-1", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-4-1");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-4-1", true) && SmallActionManager.Instance.CheckSmallAction("3-4-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-4-2");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-4-2", true) && SmallActionManager.Instance.CheckSmallAction("3-4-3", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-4-3");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-4-3", true) && SmallActionManager.Instance.CheckSmallAction("3-4-4", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-4-4");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-4-9", true) && SmallActionManager.Instance.CheckSmallAction("3-4-10", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-4-10");
                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
            }
        }

        private void Usable200LOnUse()
        {
            FocusComponent m_FocusComponent = usable_200L.transform.GetComponentInChildren<FocusComponent>();
            m_CameraSwitcher.Switch(CameraStyle.Look);
            Debug.Log("Usable200LOnUse");
            FlashManager.Instance.CloseFlash(usable_200L.gameObject);
            m_FocusComponent.Focus(() =>
            {
                Task.NewTask()
                .Append(new CheckSmallAction("3-1-1", true))
                .Append(new CheckSmallAction("3-1-2", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveButtonSetWater);
                })
                .Execute();
                Task.NewTask()
                .Append(new CheckSmallAction("3-2-9", true))
                .Append(new CheckSmallAction("3-3-1", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveButtonSetWater);
                })
                .Execute();
            });
            ProductionGuideManager.Instance.CloseGuideByName("200L控制面板");
        }

        private void StopReservoirMotor_2()
        {
            Task.NewTask(plc_200L.gameObject)
                       .Append(new CheckSmallAction("3-4-1", true))
                        .Append(new CheckSmallAction("3-4-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-4-2", true))
                       .Append(new ShowHUDTextAction(plc_200L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("200L储液罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_200LReservoir.DOKill();
                           blender_200LReservoir.DOLocalRotate(new Vector3(blender_200LReservoir.localEulerAngles.x, blender_200LReservoir.localEulerAngles.y, blender_200LReservoir.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_200LReservoir, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       .OnCompleted(() =>
                       {
                           plcPipeFitting_Reservoir_2.InvokeFitting("3-4-3", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_200L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(3);
                               FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                           }
                       })
                       .Execute();
        }

        private void StartReservoirMotor_2()
        {
            Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-3-9", true))
                        .Append(new CheckSmallAction("3-4-1", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-4-1", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("200L储液罐"))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_200LReservoir.DOLocalRotate(new Vector3(blender_200LReservoir.localEulerAngles.x, blender_200LReservoir.localEulerAngles.y, blender_200LReservoir.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LReservoirTankEvent.ActiveToggleMotor);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(2);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
        }

        private void SampleValve_200L_PointerClick(BaseEventData arg0)
        {
            //3-4-7 关闭取样阀
            Task.NewTask(sampleValve_200L.gameObject)
                        .Append(new CheckSmallAction("3-4-6", true))
                        .Append(new CheckSmallAction("3-4-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, sampleValve_200L.gameObject))
                        .Append(new UpdateSmallAction("3-4-7", true))
                        .Append(new ShowHUDTextAction(sampleValve_200L.transform, "关闭取样阀", Color.blue))
                        .Append(new NewUnityAction(() =>
                        {
                            sampleValve_200L.DORotate(new Vector3(90, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
                            plcPipeFitting_Reservoir_2.InvokeFitting("3-4-7", false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new InvokeFocusAction("100L储液取样"))
                        .Append(new InvokeFittingAction("3-4-7"))
                        .Append(new GameObjectAction(sampleBottle_200L, false))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_02.ToString(), UpdateType.Add))
                        .Append(new DelayedAction(1))
                        //.Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(8))
                        .Append(new InvokeFlashAction(true, passboxDoor))
                        .Execute();
            //3-4-6 打开取样阀
            Task.NewTask(sampleValve_200L.gameObject)
                        .Append(new CheckSmallAction("3-4-5", true))
                        .Append(new CheckSmallAction("3-4-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, sampleValve_200L.gameObject))
                        .Append(new UpdateSmallAction("3-4-6", true))
                        .Append(new ShowHUDTextAction(sampleValve_200L.transform, "打开取样阀", Color.blue))
                        .Append(new NewUnityAction(() =>
                        {
                            sampleValve_200L.DORotate(new Vector3(-90, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
                            plcPipeFitting_Reservoir_2.InvokeFitting("3-4-6", false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new InvokeFocusAction("100L储液取样"))
                        .Append(new InvokeFittingAction("3-4-6"))
                        .Append(new CoroutineFluidLevelAction(sampleBottle_200L.GetComponentInChildren<FluidLevelComponent>().gameObject, 0.75f, 2))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(7))
                        .Append(new InvokeFlashAction(true, sampleValve_200L.gameObject))
                        .Execute();
        }

        private void SampleBottle_200L_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            Task.NewTask(sampleBottle_200L)
                            .Append(new CheckSmallAction("3-4-4", true))
                            .Append(new CheckSmallAction("3-4-5", false))
                            .Append(new CheckGoodsAction(eventData,GoodsType.PYG_BLP_01))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new InvokeFlashAction(false, sampleBottle_200L))
                            .Append(new UpdateSmallAction("3-4-5", true))
                            .Append(new ChangeVirtualRealityAction(sampleBottle_200L, false))
                            .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_01.ToString(),UpdateType.Remove))
                            .Append(new ShowHUDTextAction(sampleBottle_200L.transform, "放置补料瓶", Color.blue))
                            .Append(new InvokeCurrentGuideAction(6))
                            .Append(new InvokeFlashAction(true, sampleValve_200L.gameObject))
                            .OnCompleted(() =>
                            {
                            })
                            .Execute();
        }

        private void Filter_2_Drop(BaseEventData arg0)
        {
            Task.NewTask(filter_2.gameObject)
                        .Append(new CheckSmallAction("3-2-8", true))
                        .Append(new CheckSmallAction("3-2-9", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, filter_2))
                        .Append(new UpdateSmallAction("3-2-9", true))
                        .Append(new ChangeVirtualRealityAction(filter_2, false))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(), UpdateType.Remove))
                        .Append(new ShowHUDTextAction(filter_2.transform, "安装除菌过滤器", Color.blue))
                        .Append(new InvokeCompletedAction(3, 2))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, usable_200L.gameObject))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.CurrentSolutionType, PLC_200LDispensingTank.SolutionType.D);
                        })
                        .Execute();
        }

        private void Filter_2_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            Task.NewTask(filter_2)
                        .Append(new CheckSmallAction("3-1-10", true))
                        .Append(new CheckSmallAction("3-2-1", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("拆    除",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new UpdateSmallAction("3-2-1", true))
                                    .Append(new GameObjectAction(filter_2, false))
                                    .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(),UpdateType.Add))
                                    .Append(new ShowHUDTextAction(filter_2.transform, "拆除除菌过滤器", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(2))
                                    .Append(new InvokeFlashAction(true,console))
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter_2.gameObject, parameters);
                        }).Execute();
        }

        bool hasClosePump_3;
        bool hasCloseDischargeValve_3;
        bool hasClosePump_4;
        bool hasCloseDischargeValve_4;

        private void StopSanitaryPump_2()
        {
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-1-9", true))
                        .Append(new CheckSmallAction("3-1-10", false))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "关闭出料阀及卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_200L, false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            hasClosePump_3 = true;
                            if (hasCloseDischargeValve_3 && hasClosePump_3)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("3-1-10", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 3, 1);
                                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                FlashManager.Instance.ShowFlash(filter_2);
                            }
                            else
                            {
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                        })
                        .Execute();
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-3-8", true))
                        .Append(new CheckSmallAction("3-3-9", false))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "关闭出料阀及卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_200L, false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            hasClosePump_4 = true;
                            if (hasCloseDischargeValve_4 && hasClosePump_4)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("3-3-9", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 3, 3);
                                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveButtonToReservoir);
                                this.Invoke(0.5f, () =>
                                {
                                    EventDispatcher.ExecuteEvent(PLC_200LReservoirTankEvent.ActiveToggleMotor);
                                });

                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                            else
                            {
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                        })
                        .Execute();
        }

        private void StartSanitaryPump_2()
        {
            Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-8", true))
                        .Append(new CheckSmallAction("3-1-9", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-1-9", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "启动卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_200L, true);
                        }))
                        .Append(new DelayedAction(2))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f, 0.5f, true))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 1, 0.25f, 0.5f, false))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200L.gameObject, 0, 5, true))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200LReservoir.gameObject, 0.33f, 5, false))
                        .Append(new NewUnityAction(() => 
                        {
                            plc_200LDispensing.textLevel.text = "0";
                            plc_200LReservoirTank.textLevel.text = "90";
                        }))
                        .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            plcPipeFitting_2.InvokeFitting("3-1-10", true, PLCValveComponent.Shake.ColorChange);
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-3-7", true))
                        .Append(new CheckSmallAction("3-3-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-3-8", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "启动卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_200L, true);
                        }))
                        .Append(new DelayedAction(2))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f, 0.5f, true))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 1, 0.25f, 0.5f, false))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200L.gameObject, 0, 5, true))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200LReservoir.gameObject, 0.53f, 5, false))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_200LDispensing.textLevel.text = "0";
                            plc_200LReservoirTank.textLevel.text = "180";
                        }))
                        .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            plcPipeFitting_2.InvokeFitting("3-3-9", true, PLCValveComponent.Shake.ColorChange);
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
        }

        private void StopMotor_2()
        {
            Task.NewTask(plc_200L.gameObject)
                       .Append(new CheckSmallAction("3-1-6", true))
                       .Append(new CheckSmallAction("3-1-7", false))
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new UpdateSmallAction("3-1-7", true))
                       .Append(new ShowHUDTextAction(plc_200L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("200L配制罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_200L.DOKill();
                           blender_200L.DOLocalRotate(new Vector3(blender_200L.localEulerAngles.x, blender_200L.localEulerAngles.y, blender_200L.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       .OnCompleted(() =>
                       {
                           plcPipeFitting_2.InvokeFitting("3-1-9", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_200L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(8);
                               FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                           }
                       })
                       .Execute();
            Task.NewTask(plc_200L.gameObject)
                       .Append(new CheckSmallAction("3-3-5", true))
                       .Append(new CheckSmallAction("3-3-6", false))
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new UpdateSmallAction("3-3-6", true))
                       .Append(new ShowHUDTextAction(plc_200L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("200L配制罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_200L.DOKill();
                           blender_200L.DOLocalRotate(new Vector3(blender_200L.localEulerAngles.x, blender_200L.localEulerAngles.y, blender_200L.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       //.Append(new InvokeCurrentGuideAction(7))
                       .OnCompleted(() =>
                       {
                           plcPipeFitting_2.InvokeFitting("3-3-8", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_200L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(7);
                               FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                           }
                       })
                       .Execute();
        }

        private void StartMotor_2()
        {
            Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-5", true))
                        .Append(new CheckSmallAction("3-1-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-1-6", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_200L.DOLocalRotate(new Vector3(blender_200L.localEulerAngles.x, blender_200L.localEulerAngles.y, blender_200L.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(7);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-3-4", true))
                        .Append(new CheckSmallAction("3-3-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-3-5", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_200L.DOLocalRotate(new Vector3(blender_200L.localEulerAngles.x, blender_200L.localEulerAngles.y, blender_200L.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(6);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
        }

        private void Entrance_200L_Drop_onComplete()
        {
            if (hasFeedMAT_01&& hasFeedMAT_02&& hasFeedMAT_03)
            {
                SmallActionManager.Instance.UpdateSmallAction("3-1-5", true);
                cover_200L.DOLocalRotate(new Vector3(cover_200L.localEulerAngles.x, cover_200L.localEulerAngles.y, cover_200L.localEulerAngles.z + 135), 0.5f).OnComplete(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle);
                    if (!usable_200L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(6);
                        FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                    }
                });
            }
            else
            {
                ProductionGuideManager.Instance.ShowCurrentGuide(5);
                FlashManager.Instance.ShowFlash(entrance_200L);
            }
        }
        bool hasFeedMAT_01;
        bool hasFeedMAT_02;
        bool hasFeedMAT_03;
        bool isFeeding;
        //3-1-5 加入物料CDFortiCHO、poloxamer188、葡萄糖
        private void Entrance_200L_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            if (isFeeding)
            {
                return;
            }
            if (component!=null)
            {
                Goods item = component.Item as Goods;
                switch (item.GoodsType)
                {
                    case GoodsType.PYG_200LMAT_01:
                        Task.NewTask(entrance_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-4", true))
                        .Append(new CheckSmallAction("3-1-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false,entrance_200L))
                        .Append(new NewUnityAction(()=> { isFeeding = true; }))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_200LMAT_01.ToString(), UpdateType.Remove))
                        //.Append(new UpdateSmallAction("3-1-5", true))
                        .Append(new GameObjectAction(bucket_feed200L, true))
                        .Append(new NewUnityAction(() => particle_feed200L.Play()))
                        .Append(new DelayedAction(3))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feed200L.Stop();
                            liquidVolume_200L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feed200L, false))
                        .Append(new ShowHUDTextAction(entrance_200L.transform, "已加入物料CDFortiCHO", Color.blue))
                        .OnCompleted(() =>
                        {
                            hasFeedMAT_01 = true;
                            Entrance_200L_Drop_onComplete();
                            isFeeding = false;
                        })
                        .Execute();
                        break;
                    case GoodsType.PYG_200LMAT_02:
                        Task.NewTask(entrance_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-4", true))
                        .Append(new CheckSmallAction("3-1-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false,entrance_200L))
                        .Append(new NewUnityAction(() => { isFeeding = true; }))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_200LMAT_02.ToString(), UpdateType.Remove))
                        //.Append(new UpdateSmallAction("3-1-5", true))
                        .Append(new GameObjectAction(bucket_feed200L, true))
                        .Append(new NewUnityAction(() => particle_feed200L.Play()))
                        .Append(new DelayedAction(3))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feed200L.Stop();
                            liquidVolume_200L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feed200L, false))
                        .Append(new ShowHUDTextAction(entrance_100L.transform, "已加入物料poloxamer188", Color.blue))
                        .OnCompleted(() =>
                        {
                            hasFeedMAT_02 = true;
                            Entrance_200L_Drop_onComplete();
                            isFeeding = false;
                        })
                        .Execute();

                        break;
                    case GoodsType.PYG_200LMAT_03:
                        Task.NewTask(entrance_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-4", true))
                        .Append(new CheckSmallAction("3-1-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false,entrance_200L))
                        .Append(new NewUnityAction(() => { isFeeding = true; }))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_200LMAT_03.ToString(), UpdateType.Remove))
                        //.Append(new UpdateSmallAction("3-1-5", true))
                        .Append(new GameObjectAction(bucket_feed200L, true))
                        .Append(new NewUnityAction(() => particle_feed200L.Play()))
                        .Append(new DelayedAction(3))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feed200L.Stop();
                            liquidVolume_200L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feed200L, false))
                        .Append(new ShowHUDTextAction(entrance_100L.transform, "已加入葡萄糖", Color.blue))
                        .OnCompleted(() =>
                        {
                            hasFeedMAT_03 = true;
                            Entrance_200L_Drop_onComplete();
                            isFeeding = false;
                        })
                        .Execute();
                        break;
                    case GoodsType.PYG_M20A_01:
                        Task.NewTask(entrance_200L.gameObject)
                        .Append(new CheckSmallAction("3-3-3", true))
                        .Append(new CheckSmallAction("3-3-4", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, entrance_200L))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_M20A_01.ToString(), UpdateType.Remove))
                        .Append(new UpdateSmallAction("3-3-4", true))
                        .Append(new GameObjectAction(bucket_feed200L, true))
                        .Append(new NewUnityAction(() => particle_feed200L.Play()))
                        .Append(new DelayedAction(5))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feed200L.Stop();
                            liquidVolume_200L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feed200L, false))
                        .Append(new ShowHUDTextAction(entrance_200L.transform, "已加入M20A", Color.blue))
                        .OnCompleted(() =>
                        {
                            cover_200L.DOLocalRotate(new Vector3(cover_200L.localEulerAngles.x, cover_200L.localEulerAngles.y, cover_200L.localEulerAngles.z + 135), 0.5f).OnComplete(() =>
                            {
                                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle);
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(5);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            });
                        })
                        .Execute();
                        break;
                    default:
                        break;
                }
            }
            
        }

        //3-1-4 3-3-3 打开投料盖
        private void Cover_200L_PointerClick(BaseEventData arg0)
        {
            Task.NewTask(cover_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-3", true))
                        .Append(new CheckSmallAction("3-1-4", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, cover_200L.gameObject))
                        .Append(new UpdateSmallAction("3-1-4", true))
                        .Append(new DOLocalRotaAction(cover_200L, new Vector3(cover_200L.localEulerAngles.x, cover_200L.localEulerAngles.y, cover_200L.localEulerAngles.z - 135), 0.5f))
                        .Append(new ShowHUDTextAction(cover_200L.transform, "打开投料盖", Color.blue))
                        .Append(new InvokeCurrentGuideAction(5))
                        .Append(new InvokeFlashAction(true, entrance_200L))
                        .Execute();
            Task.NewTask(cover_200L.gameObject)
                        .Append(new CheckSmallAction("3-3-2", true))
                        .Append(new CheckSmallAction("3-3-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-3-3", true))
                        .Append(new DOLocalRotaAction(cover_200L, new Vector3(cover_200L.localEulerAngles.x, cover_200L.localEulerAngles.y, cover_200L.localEulerAngles.z - 135), 0.5f))
                        .Append(new ShowHUDTextAction(cover_200L.transform, "打开投料盖", Color.blue))
                        .Append(new InvokeCurrentGuideAction(4))
                        .Append(new InvokeFlashAction(true, entrance_200L))
                        .Execute();
        }

        private void SetWaterFinish_2()
        {
            //3-1-2 设置纯化水加入量
            Task.NewTask(plc_100L.gameObject)
                .Append(new CheckSmallAction("3-1-1", true))
                .Append(new CheckSmallAction("3-1-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new UpdateSmallAction("3-1-2", true))
                .Append(new ShowHUDTextAction(plc_200L.transform, "纯化水加入量设置完毕", Color.blue))
                .OnCompleted(() =>
                {
                    //isNeedIndicator_100L = true;
                    plcPipeFitting_2.InvokeFitting("3-1-3-1", true, PLCValveComponent.Shake.ColorChange);
                    if (!usable_200L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        FlashManager.Instance.ShowFlash(usable_200L.gameObject);

                    }
                })
                .Execute();
            //3-3-1 设置纯化水加入量
            Task.NewTask(plc_100L.gameObject)
                .Append(new CheckSmallAction("3-2-9", true))
                .Append(new CheckSmallAction("3-3-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new UpdateSmallAction("3-3-1", true))
                .Append(new ShowHUDTextAction(plc_200L.transform, "纯化水加入量设置完毕", Color.blue))
                .OnCompleted(() =>
                {
                    plcPipeFitting_2.InvokeFitting("3-3-2-1", true, PLCValveComponent.Shake.ColorChange);

                    if (!usable_200L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        FlashManager.Instance.ShowFlash(usable_200L.gameObject);

                    }
                })
                .Execute();
        }

        private void OnClickedAllValves_2(string arg0)
        {
            switch (arg0)
            {
                case "3-1-3-1":
                    Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-2", true))
                        .Append(new CheckSmallAction("3-1-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-1-3", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "打开纯化水阀门", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200L.gameObject, 0.4f, 5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_200LDispensing.textLevel.text = "90";
                        }))
                        .Append(new InvokeFittingAction("3-1-3-2"))
                        .Append(new NewUnityAction(() => { plcPipeFitting_2.InvokeFitting("3-1-3-2", false); }))
                        .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(4))
                        .Append(new InvokeFlashAction(true,cover_200L.gameObject))
                        .Execute();
                    break;
                case "3-1-9":
                    //3-1-8 打开出料阀
                    Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-1-7", true))
                        .Append(new CheckSmallAction("3-1-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-1-8", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "打开出料阀", Color.blue))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "3-1-10":
                    //3-1-10 关闭出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-1-9", true))
                        .Append(new CheckSmallAction("3-1-10", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new ShowHUDTextAction(plc_200L.transform, "关闭出料阀", Color.blue))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            hasCloseDischargeValve_3 = true;
                            if (hasCloseDischargeValve_3 && hasClosePump_3)
                            {
                                SmallActionManager.Instance.UpdateSmallAction("3-1-10", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 3, 1);
                                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                FlashManager.Instance.ShowFlash(filter_2);
                            }
                            else
                            {
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                        })
                        .Execute();
                    break;
                case "3-3-2-1":
                    //3-3-2 打开纯化水阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-3-1", true))
                        .Append(new CheckSmallAction("3-3-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-3-2", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "打开纯化水阀门", Color.blue))
                        .Append(new InvokeFocusAction("200L配制罐"))
                        .Append(new GameObjectFadeAction(belly_200L, 1, 0.25f))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new NewUnityAction(() =>
                        {
                            liquidVolume_200L.liquidColor1 = waterColor;
                        }))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200L.gameObject, 0.4f, 5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_200LDispensing.textLevel.text = "90";
                        }))
                        .Append(new InvokeFittingAction("3-3-2-2"))
                        .Append(new NewUnityAction(() => { plcPipeFitting_2.InvokeFitting("3-3-2-2", false); }))
                        .Append(new GameObjectFadeAction(belly_200L, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(3))
                        .Append(new InvokeFlashAction(true, cover_200L.gameObject))
                        .Execute();
                    break;
                case "3-3-8":
                    //3-3-7 打开出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-3-6", true))
                        .Append(new CheckSmallAction("3-3-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-3-7", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "打开出料阀", Color.blue))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(8))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(8);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "3-3-9":
                    //3-3-9 关闭出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-3-8", true))
                        .Append(new CheckSmallAction("3-3-9", false))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "关闭出料阀", Color.blue))
                        .Append(new InvokeFocusAction("200L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            hasCloseDischargeValve_4 = true;
                            if (hasCloseDischargeValve_4 && hasClosePump_4)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("3-3-9", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 3, 3);
                                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.ActiveButtonToReservoir);
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                            else
                            {
                                if (!usable_200L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                    FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                                }
                            }
                        })
                        .Execute();
                    break;
                case "3-4-3":
                    //3-4-3 启动取样灭菌相关阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("3-4-2", true))
                        .Append(new CheckSmallAction("3-4-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-4-3", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "启动取样灭菌相关阀门", Color.blue))
                        .Append(new InvokeFocusAction("200L储液灭菌"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new ShowProgressAction("持续灭菌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            plcPipeFitting_Reservoir_2.InvokeFitting("3-4-4", true, PLCValveComponent.Shake.ColorChange);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(4);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "3-4-4":
                    //3-4-4 关闭取样灭菌相关阀门
                    Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-4-3", true))
                        .Append(new CheckSmallAction("3-4-4", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-4-4", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "启动取样灭菌相关阀门", Color.blue))
                        .Append(new InvokeFocusAction("200L储液灭菌"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_01.ToString(),UpdateType.Add))
                        .Append(new GameObjectAction(sampleBottle_200L, true))
                        .Append(new ChangeVirtualRealityAction(sampleBottle_200L, true))
                        .Append(new InvokeCurrentGuideAction(5))
                        .Append(new InvokeFlashAction(true, sampleBottle_200L))
                        .Execute();
                    break;
                case "3-4-10":
                    //3-4-10 打开出料阀、出料泵
                    Task.NewTask(plc_200L.gameObject)
                        .Append(new CheckSmallAction("3-4-9", true))
                        .Append(new CheckSmallAction("3-4-10", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-4-10", true))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "打开出料阀、出料泵", Color.blue))
                        .Append(new InvokeFocusAction("200L出料"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 1, 0.25f))
                        .Append(new ShowProgressAction("正在出料。。。", 5))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_200LReservoir.gameObject, 0, 5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_200LReservoirTank.textLevel.text = "0";
                        }))
                        .Append(new ShowHUDTextAction(plc_200L.transform, "出料完毕", Color.blue))
                        .Append(new GameObjectFadeAction(belly_200LReservoir, 0.25f, 1))
                        .Append(new InvokeFittingAction("3-4-11"))
                        .Append(new NewUnityAction(() =>
                        {
                            plcPipeFitting_Reservoir_2.InvokeFitting("3-4-11", false);
                        }))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCompletedAction(3, 4))
                        .Execute();
                    break;
                default:
                    break;
            }
        }

        //3-1-1 打开200L配制罐电源
        private void PowerButton_2_PointerClick(BaseEventData arg)
        {
            Task.NewTask(powerButton_2.gameObject)
                //.Append(new CheckSmallAction("2-4-10", true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor,2,4))
                .Append(new CheckSmallAction("3-1-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false,powerButton_2.gameObject))
                .Append(new UpdateSmallAction("3-1-1", true))
                .Append(new DOLocalRotaAction(powerButton_2, new Vector3(powerButton_2.localEulerAngles.x, powerButton_2.localEulerAngles.y + 90, powerButton_2.localEulerAngles.z), 0.5f))
                .Append(new GameObjectAction(plc_200L.gameObject, true))
                .Append(new ShowHUDTextAction(usable_200L.transform, "打开200L配制罐电源", Color.blue))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.CurrentSolutionType, PLC_200LDispensingTank.SolutionType.C);
                    if (!usable_200L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                    }
                })
                .Execute();
        }

        //2-4-9 放置补料瓶
        private void PassBox_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;


            Task.NewTask(passBox)
                        .Append(new CheckSmallAction("2-4-8", true))
                        .Append(new CheckSmallAction("2-4-9", false))
                        .Append(new CheckGoodsAction(eventData,GoodsType.PYG_BLP_02))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, passBox))
                        .Append(new UpdateSmallAction("2-4-9", true))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_02.ToString(),UpdateType.Remove))
                        .Append(new GameObjectAction(passBoxBottle, true))
                        .Append(new ShowHUDTextAction(passBox.transform, "放置补料瓶", Color.blue))
                        .OnCompleted(() =>
                        {
                            doorComponent.Opening(false);
                            plcPipeFitting_Reservoir.InvokeFitting("2-4-10", true, PLCValveComponent.Shake.ColorChange);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(passBox)
                        .Append(new CheckSmallAction("3-4-8", true))
                        .Append(new CheckSmallAction("3-4-9", false))
                        .Append(new CheckGoodsAction(eventData, GoodsType.PYG_BLP_02))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, passboxDoor))
                        .Append(new UpdateSmallAction("3-4-9", true))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_02.ToString(), UpdateType.Remove))
                        .Append(new GameObjectAction(passBoxBottle_2, true))
                        .Append(new ShowHUDTextAction(passBox.transform, "放置补料瓶", Color.blue))
                        .OnCompleted(() =>
                        {
                            doorComponent.Opening(false);
                            plcPipeFitting_Reservoir_2.InvokeFitting("3-4-10", true, PLCValveComponent.Shake.ColorChange);
                            if (!usable_200L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                FlashManager.Instance.ShowFlash(usable_200L.gameObject);
                            }
                        })
                        .Execute();
        }

        private void doorComponent_onOpened()
        {
            //2-4-8 打开传递窗
            Task.NewTask(passboxDoor)
                        .Append(new CheckSmallAction("2-4-7", true))
                        .Append(new CheckSmallAction("2-4-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false,passboxDoor))
                        .Append(new UpdateSmallAction("2-4-8", true))
                        .Append(new ShowHUDTextAction(passBox.transform, "打开传递窗", Color.blue))
                        .Append(new InvokeCurrentGuideAction(9))
                        .Append(new InvokeFlashAction(true, passBox))
                        .OnCompleted(() =>
                        {
                            

                        })
                        .Execute();
            //3-4-8 打开传递窗
            Task.NewTask(passBox)
                        .Append(new CheckSmallAction("3-4-7", true))
                        .Append(new CheckSmallAction("3-4-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, passboxDoor))
                        .Append(new UpdateSmallAction("3-4-8", true))
                        .Append(new ShowHUDTextAction(passBox.transform, "打开传递窗", Color.blue))
                        .Append(new InvokeCurrentGuideAction(9))
                        .Append(new InvokeFlashAction(true, passBox))
                        .OnCompleted(() =>
                        {


                        })
                        .Execute();
        }

        private void SampleValve_100L_PointerClick(BaseEventData arg0)
        {
            //2-4-7 关闭取样阀
            Task.NewTask(passBox)
                        .Append(new CheckSmallAction("2-4-6", true))
                        .Append(new CheckSmallAction("2-4-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, sampleValve_100L.gameObject))
                        .Append(new UpdateSmallAction("2-4-7", true))
                        .Append(new ShowHUDTextAction(filter.transform, "关闭取样阀", Color.blue))
                        .Append(new NewUnityAction(() =>
                        {
                            sampleValve_100L.DORotate(new Vector3(90, 0, 0), 0.5f, RotateMode.WorldAxisAdd);
                            plcPipeFitting_Reservoir.InvokeFitting("2-4-7", false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new InvokeFocusAction("100L储液取样"))
                        .Append(new InvokeFittingAction("2-4-7"))
                        .Append(new GameObjectAction(sampleBottle_100L, false))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_02.ToString(), UpdateType.Add))
                        .Append(new DelayedAction(1))
                        //.Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(8))
                        .Append(new InvokeFlashAction(true, passboxDoor))
                        .Execute();
            //2-4-6 打开取样阀
            Task.NewTask(passBox)
                        .Append(new CheckSmallAction("2-4-5", true))
                        .Append(new CheckSmallAction("2-4-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, sampleValve_100L.gameObject))
                        .Append(new UpdateSmallAction("2-4-6", true))
                        .Append(new ShowHUDTextAction(filter.transform, "打开取样阀", Color.blue))
                        .Append(new NewUnityAction(()=> 
                        {
                            sampleValve_100L.DORotate(new Vector3(-90, 0, 0), 0.5f,RotateMode.WorldAxisAdd);
                            plcPipeFitting_Reservoir.InvokeFitting("2-4-6", false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new InvokeFocusAction("100L储液取样"))
                        .Append(new InvokeFittingAction("2-4-6"))
                        .Append(new CoroutineFluidLevelAction(sampleBottle_100L.GetComponentInChildren<FluidLevelComponent>().gameObject, 0.75f, 2))
                        .Append(new DelayedAction(0.5f))
                        //.Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(7))
                        .Append(new InvokeFlashAction(true, sampleValve_100L.gameObject))
                        .Execute();
        }

        

        //2-4-5 放置补料瓶
        private void SampleBottle_100L_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            Task.NewTask(sampleBottle_100L)
                        .Append(new CheckSmallAction("2-4-4", true))
                        .Append(new CheckSmallAction("2-4-5", false))
                        .Append(new CheckGoodsAction(eventData,GoodsType.PYG_BLP_01))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, sampleBottle_100L))
                        .Append(new UpdateSmallAction("2-4-5", true))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_BLP_01.ToString(),UpdateType.Remove))
                        .Append(new ChangeVirtualRealityAction(sampleBottle_100L, false))
                        .Append(new ShowHUDTextAction(sampleBottle_100L.transform, "放置补料瓶", Color.blue))
                        .Append(new InvokeCurrentGuideAction(6))
                        .Append(new InvokeFlashAction(true, sampleValve_100L.gameObject))
                        .OnCompleted(() =>
                        {
                            //plcPipeFitting_Reservoir.InvokeFitting("2-4-6", false);
                            //plcPipeFitting_Reservoir.InvokeFitting("2-4-6", true, PLCValveComponent.Shake.ColorChange);
                            //if (!usable_100L.IsUsing)
                            //{
                            //    ProductionGuideManager.Instance.ShowCurrentGuide(6);
                            //}
                        })
                        .Execute();
        }

        private void StopReservoirMotor()
        {
            Task.NewTask(plc_100L.gameObject)
                       .Append(new CheckSmallAction("2-4-1", true))
                        .Append(new CheckSmallAction("2-4-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-4-2", true))
                       .Append(new ShowHUDTextAction(plc_100L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("100L储液罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_100LReservoir.DOKill();
                           blender_100LReservoir.DOLocalRotate(new Vector3(blender_100LReservoir.localEulerAngles.x, blender_100LReservoir.localEulerAngles.y, blender_100LReservoir.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_100LReservoir, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       .OnCompleted(() =>
                       {
                           plcPipeFitting_Reservoir.InvokeFitting("2-4-3", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_100L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(3);
                               FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                           }
                       })
                       .Execute();
        }

        private void StartReservoirMotor()
        {
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-9", true))
                        .Append(new CheckSmallAction("2-4-1", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-4-1", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("100L储液罐"))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_100LReservoir.DOLocalRotate(new Vector3(blender_100LReservoir.localEulerAngles.x, blender_100LReservoir.localEulerAngles.y, blender_100LReservoir.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LReservoirTankEvent.ActiveToggleMotor);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(2);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
        }

        private void Filter_Drop(BaseEventData arg0)
        {
            Task.NewTask(filter.gameObject)
                        .Append(new CheckSmallAction("2-2-8", true))
                        .Append(new CheckSmallAction("2-2-9", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, filter))
                        .Append(new UpdateSmallAction("2-2-9", true))
                        .Append(new ChangeVirtualRealityAction(filter,false))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(), UpdateType.Remove))
                        .Append(new ShowHUDTextAction(filter.transform, "安装除菌过滤器", Color.blue))
                        .Append(new InvokeCompletedAction(2,2))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, usable_100L.gameObject))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.CurrentSolutionType, PLC_100LDispensingTank.SolutionType.D);
                        })
                        .Execute();
        }

        private void ButtonStartClick()
        {
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("2-2-6", true))
                        .Append(new CheckSmallAction("2-2-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-2-7", true))
                        .Append(new ShowHUDTextAction(usable_detector.transform, "开始泡点测试", Color.blue))
                        .Append(new ShowProgressAction("正在进行泡点测试。。。", 5,true))
                        .Append(new InvokeCurrentGuideAction(8))
                        .Append(new InvokeFlashAction(true,filter_detect))
                        .Execute();
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("3-2-6", true))
                        .Append(new CheckSmallAction("3-2-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-2-7", true))
                        .Append(new ShowHUDTextAction(usable_detector.transform, "开始泡点测试", Color.blue))
                        .Append(new ShowProgressAction("正在进行泡点测试。。。", 5,true))
                        .Append(new InvokeCurrentGuideAction(8))
                        .Append(new InvokeFlashAction(true, filter_detect))
                        .Execute();
        }

        private void ButtonOKClick()
        {
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("2-2-5", true))
                        .Append(new CheckSmallAction("2-2-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-2-6", true))
                        .Append(new ShowHUDTextAction(usable_detector.transform, "确认泡点测试", Color.blue))
                        .OnCompleted(() =>
                        {
                            if (!usable_detector.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(7);
                            }
                        })
                        .Execute();
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("3-2-5", true))
                        .Append(new CheckSmallAction("3-2-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-2-6", true))
                        .Append(new ShowHUDTextAction(usable_detector.transform, "确认泡点测试", Color.blue))
                        .OnCompleted(() =>
                        {
                            if (!usable_detector.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(7);
                            }
                        })
                        .Execute();
        }

        private void ButtonBubblePointClick()
        {
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("2-2-4", true))
                        .Append(new CheckSmallAction("2-2-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-2-5", true))
                        .Append(new ShowHUDTextAction(plc_FilterDetector.transform, "进入泡点测试", Color.blue))
                        .OnCompleted(() =>
                        {
                            if (!usable_detector.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(6);
                            }
                        })
                        .Execute();
            Task.NewTask(usable_detector.gameObject)
                        .Append(new CheckSmallAction("3-2-4", true))
                        .Append(new CheckSmallAction("3-2-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("3-2-5", true))
                        .Append(new ShowHUDTextAction(plc_FilterDetector.transform, "进入泡点测试", Color.blue))
                        .OnCompleted(() =>
                        {
                            if (!usable_detector.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(6);
                            }
                        })
                        .Execute();
        }

        private void UsableDetectorOnExit()
        {
            if (SmallActionManager.Instance.CheckSmallAction("2-2-4", true) && SmallActionManager.Instance.CheckSmallAction("2-2-5", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-2-5");
                FlashManager.Instance.ShowFlash(usable_detector.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-2-5", true) && SmallActionManager.Instance.CheckSmallAction("2-2-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-2-6");
                FlashManager.Instance.ShowFlash(usable_detector.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-2-4", true) && SmallActionManager.Instance.CheckSmallAction("3-2-5", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-2-5");
                FlashManager.Instance.ShowFlash(usable_detector.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-2-5", true) && SmallActionManager.Instance.CheckSmallAction("3-2-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-2-6");
                FlashManager.Instance.ShowFlash(usable_detector.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("3-2-6", true) && SmallActionManager.Instance.CheckSmallAction("3-2-7", false))
            {
                ProductionGuideManager.Instance.ShowGuide("3-2-7");
                FlashManager.Instance.ShowFlash(usable_detector.gameObject);
            }
        }

        private void UsableDetectorOnUse()
        {
            FocusComponent m_FocusComponent = usable_detector.transform.GetComponentInChildren<FocusComponent>();
            m_CameraSwitcher.Switch(CameraStyle.Look);
            ProductionGuideManager.Instance.CloseGuideByName("检测仪控制面板");
            FlashManager.Instance.CloseFlash(usable_detector.gameObject);
            m_FocusComponent.Focus(()=> 
            {
                //2-2-4 进入检测仪操作界面
                Task.NewTask()
                            .Append(new CheckSmallAction("2-2-3", true))
                            .Append(new CheckSmallAction("2-2-4", false))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new UpdateSmallAction("2-2-4", true))
                            //.Append(new GameObjectAction(filter, false))
                            .Append(new InvokeCurrentGuideAction(5))
                            .OnCompleted(() =>
                            {
                                EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.ActiveButtonBubblePoint);
                            })
                            .Execute();
                //3-2-4 进入检测仪操作界面
                Task.NewTask()
                            .Append(new CheckSmallAction("3-2-3", true))
                            .Append(new CheckSmallAction("3-2-4", false))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new UpdateSmallAction("3-2-4", true))
                            //.Append(new GameObjectAction(filter, false))
                            .Append(new InvokeCurrentGuideAction(5))
                            .OnCompleted(() =>
                            {
                                EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.ActiveButtonBubblePoint);
                            })
                            .Execute();
            });
            
        }

        private void Console_Drop(BaseEventData arg0)
        {
            Task.NewTask(console)
                        .Append(new CheckSmallAction("2-2-1", true))
                        .Append(new CheckSmallAction("2-2-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, console))
                        .Append(new UpdateSmallAction("2-2-2", true))
                        .Append(new GameObjectAction(filter_detect,true))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(),UpdateType.Remove))
                        .Append(new ShowHUDTextAction(console.transform, "放置除菌过滤器", Color.blue))
                        .Append(new InvokeCurrentGuideAction(3))
                        .Append(new InvokeFlashAction(true, filter_detect))
                        .Execute();

            Task.NewTask(console)
                        .Append(new CheckSmallAction("3-2-1", true))
                        .Append(new CheckSmallAction("3-2-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, console))
                        .Append(new UpdateSmallAction("3-2-2", true))
                        .Append(new GameObjectAction(filter_detect, true))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(), UpdateType.Remove))
                        .Append(new ShowHUDTextAction(console.transform, "放置除菌过滤器", Color.blue))
                        .Append(new InvokeCurrentGuideAction(3))
                        .Append(new InvokeFlashAction(true, filter_detect))
                        .Execute();
        }

        private void Filter_detect_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Task.NewTask(filter_detect.gameObject)
                        .Append(new CheckSmallAction("2-2-2", true))
                        .Append(new CheckSmallAction("2-2-3", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("连    接",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new InvokeFlashAction(false, filter_detect))
                                    .Append(new UpdateSmallAction("2-2-3", true))
                                    .Append(new GameObjectAction(connecter,true))
                                    .Append(new ShowHUDTextAction(filter_detect.transform, "连接过滤器和检测仪", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(4))
                                    .Append(new InvokeFlashAction(true, usable_detector.gameObject))
                                    .OnCompleted(() =>
                                    {
                                        EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.Reset);
                                    })
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter_detect.gameObject, parameters);
                        })
                        .Execute();
                    Task.NewTask(filter_detect.gameObject)
                        .Append(new CheckSmallAction("2-2-7", true))
                        .Append(new CheckSmallAction("2-2-8", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("拆    除",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new InvokeFlashAction(false,filter_detect))
                                    .Append(new UpdateSmallAction("2-2-8", true))
                                    .Append(new GameObjectAction(filter_detect,false))
                                    .Append(new GameObjectAction(connecter,false))
                                    .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(),UpdateType.Add))
                                    .Append(new ShowHUDTextAction(filter_detect.transform, "拆除除菌过滤器", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(9))
                                    .Append(new InvokeFlashAction(true,filter))
                                    .Append(new GameObjectAction(filter,true))
                                    .Append(new ChangeVirtualRealityAction(filter,true))
                                    .OnCompleted(() =>
                                    {

                                    })
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter_detect.gameObject, parameters);
                        })
                        .Execute();
                    Task.NewTask(filter_detect.gameObject)
                        .Append(new CheckSmallAction("3-2-2", true))
                        .Append(new CheckSmallAction("3-2-3", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("连    接",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new InvokeFlashAction(false, filter_detect))
                                    .Append(new UpdateSmallAction("3-2-3", true))
                                    .Append(new GameObjectAction(connecter,true))
                                    .Append(new ShowHUDTextAction(filter_detect.transform, "连接过滤器和检测仪", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(4))
                                    .Append(new InvokeFlashAction(true, usable_detector.gameObject))
                                    .OnCompleted(() =>
                                    {
                                        EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.Reset);
                                    })
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter_detect.gameObject, parameters);
                        })
                        .Execute();
                    Task.NewTask(filter_detect.gameObject)
                        .Append(new CheckSmallAction("3-2-7", true))
                        .Append(new CheckSmallAction("3-2-8", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("拆    除",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new InvokeFlashAction(false,filter_detect))
                                    .Append(new UpdateSmallAction("3-2-8", true))
                                    .Append(new GameObjectAction(filter_detect,false))
                                    .Append(new GameObjectAction(connecter,false))
                                    .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(),UpdateType.Add))
                                    .Append(new ShowHUDTextAction(filter_detect.transform, "拆除除菌过滤器", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(9))
                                    .Append(new InvokeFlashAction(true,filter_2))
                                    .Append(new GameObjectAction(filter_2,true))
                                    .Append(new ChangeVirtualRealityAction(filter_2,true))
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter_detect.gameObject, parameters);
                        })
                        .Execute();
                    break;
                case PointerEventData.InputButton.Right:
                    
                    break;
                default:
                    break;
            }
        }

        //2-2-1 拆除除菌过滤器
        private void Filter_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData.button!=PointerEventData.InputButton.Left)
            {
                return;
            }
            Task.NewTask(filter)
                        .Append(new CheckSmallAction("2-1-10", true))
                        .Append(new CheckSmallAction("2-2-1", false))
                        .OnCompleted(() =>
                        {
                            List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                            {
                                new ContextMenuParameter("拆    除",X=>
                                {
                                    Task.NewTask()
                                    .Append(new InvokeCloseAllGuideAction())
                                    .Append(new InvokeFlashAction(false,filter.gameObject))
                                    .Append(new UpdateSmallAction("2-2-1", true))
                                    .Append(new GameObjectAction(filter, false))
                                    .Append(new UpdateGoodsAction(GoodsType.PYG_FILTER_01.ToString(),UpdateType.Add))
                                    .Append(new ShowHUDTextAction(filter.transform, "拆除除菌过滤器", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(2))
                                    .Append(new InvokeFlashAction(true,console))
                                    .Execute();
                                }),
                                new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                            };
                            ContextMenuEx.Instance.Show(filter.gameObject, parameters);
                        }).Execute();
        }

        bool hasCloseDischargeValve_2;
        bool hasClosePump_2;

        //2-1-10 2-3-9 关闭出料阀及卫生泵
        private void StopSanitaryPump()
        {
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-9", true))
                        .Append(new CheckSmallAction("2-1-10", false))
                        //.Append(new InvokeCloseAllGuideAction())
                        //.Append(new UpdateSmallAction("2-1-10", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "关闭出料阀及卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_100L, false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCompletedAction(2, 1))
                        //.Append(new InvokeCurrentGuideAction(1))
                        .OnCompleted(() =>
                        {
                            hasClosePump_1 = true;
                            if (hasCloseDischargeValve_1&&hasClosePump_1)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("2-1-10", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 2, 1);
                                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                FlashManager.Instance.ShowFlash(filter);
                            }
                            else
                            {
                                if (!usable_100L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                    FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                                }
                            }
                        })
                        .Execute();
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-8", true))
                        .Append(new CheckSmallAction("2-3-9", false))
                        //.Append(new InvokeCloseAllGuideAction())
                        //.Append(new UpdateSmallAction("2-3-9", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "关闭出料阀及卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_100L, false);
                        }))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCompletedAction(2, 3))
                        //.Append(new InvokeCurrentGuideAction(1))
                        .OnCompleted(() =>
                        {
                            hasClosePump_2 = true;
                            if (hasCloseDischargeValve_2 && hasClosePump_2)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("2-3-9", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 2, 3);
                                EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveButtonToReservoir);
                                
                                if (!usable_100L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                    FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                                }
                            }
                            
                        })
                        .Execute();
        }

        //2-1-9 2-3-8 启动卫生泵
        private void StartSanitaryPump()
        {
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-8", true))
                        .Append(new CheckSmallAction("2-1-9", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-1-9", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "启动卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_100L, true);
                        }))
                        .Append(new DelayedAction(2))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f, 0.5f, true))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 1, 0.25f, 0.5f, false))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100L.gameObject, 0, 5, true))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100LReservoir.gameObject, 0.33f, 5, false))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_100LDispensing.textLevel.text = "0";
                            plc_100LReservoirTank.textLevel.text = "75";
                        }))
                        //.Append(new NewUnityAction(() =>
                        //{
                        //    plcPipeFitting.InvokeFitting("2-1-10", false);
                        //}))
                        //.Append(new InvokeFittingAction("2-1-10"))
                        .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(10))
                        .OnCompleted(() =>
                        {
                            plcPipeFitting.InvokeFitting("2-1-10", true,PLCValveComponent.Shake.ColorChange);
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-7", true))
                        .Append(new CheckSmallAction("2-3-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-3-8", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "启动卫生泵", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐卫生泵"))
                        .Append(new NewUnityAction(() =>
                        {
                            ShakePump(sanitaryPump_100L, true);
                        }))
                        .Append(new DelayedAction(2))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f, 0.5f, true))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 1, 0.25f, 0.5f, false))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100L.gameObject, 0, 5, true))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100LReservoir.gameObject, 0.53f, 5, false))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_100LDispensing.textLevel.text = "0";
                            plc_100LReservoirTank.textLevel.text = "90";
                        }))
                        //.Append(new NewUnityAction(() =>
                        //{
                        //    plcPipeFitting_Reservoir.InvokeFitting("2-3-9", false);
                        //}))
                        //.Append(new InvokeFittingAction("2-3-9"))
                        .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            plcPipeFitting.InvokeFitting("2-3-9", true, PLCValveComponent.Shake.ColorChange);
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
        }

        bool hasCloseDischargeValve_1;
        bool hasClosePump_1;

        private void OnClickedAllValves(string arg0)
        {
            switch (arg0)
            {
                case "2-1-3-1":
                    //2-1-3 打开纯化水阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-2", true))
                        .Append(new CheckSmallAction("2-1-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-1-3", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "打开纯化水阀门", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100L.gameObject, 0.33f, 5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_100LDispensing.textLevel.text = "75";
                        }))
                        .Append(new InvokeFittingAction("2-1-3-2"))
                        .Append(new NewUnityAction(() => { plcPipeFitting.InvokeFitting("2-1-3-2", false); }))
                        .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(4))
                        .Append(new InvokeFlashAction(true, cover_100L.gameObject))
                        .Execute();
                    break;
                case "2-1-9":
                    //2-1-8 打开出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-7", true))
                        .Append(new CheckSmallAction("2-1-8", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-1-8", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "打开出料阀", Color.blue))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(9);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "2-1-10":
                    //2-1-10 关闭出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-9", true))
                        .Append(new CheckSmallAction("2-1-10", false))
                        .Append(new InvokeCloseAllGuideAction())
                        //.Append(new UpdateSmallAction("2-1-10", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "关闭出料阀", Color.blue))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            hasCloseDischargeValve_1 = true;
                            if (hasCloseDischargeValve_1 && hasClosePump_1)
                            {
                                SmallActionManager.Instance.UpdateSmallAction("2-1-10", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 2, 1);
                                ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                FlashManager.Instance.ShowFlash(filter);
                            }
                            else
                            {
                                if (!usable_100L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(10);
                                    FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                                }
                            }
                        })
                        .Execute();
                    break;
                case "2-3-2-1":
                    //2-3-2 打开纯化水阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-1", true))
                        .Append(new CheckSmallAction("2-3-2", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-3-2", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "打开纯化水阀门", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new NewUnityAction(() =>
                        {
                            liquidVolume_100L.liquidColor1 = waterColor;
                        }))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100L.gameObject, 0.2f, 5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_100LDispensing.textLevel.text = "15";
                        }))
                        .Append(new InvokeFittingAction("2-3-2-2"))
                        .Append(new NewUnityAction(() => { plcPipeFitting.InvokeFitting("2-3-2-2", false); }))
                        .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                        .Append(new DelayedAction(0.5f))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCurrentGuideAction(3))
                        .Append(new InvokeFlashAction(true,cover_100L.gameObject))
                        .Execute();
                    break;
                case "2-3-8":
                    //2-3-7 打开出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-6", true))
                        .Append(new CheckSmallAction("2-3-7", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-3-7", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "打开出料阀", Color.blue))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(8))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveSanitaryPumpToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(8);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "2-3-9":
                    //2-3-9 关闭出料阀
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-8", true))
                        .Append(new CheckSmallAction("2-3-9", false))
                        //.Append(new InvokeCloseAllGuideAction())
                        //.Append(new UpdateSmallAction("2-1-10", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "关闭出料阀", Color.blue))
                        .Append(new InvokeFocusAction("100L物料转移"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(9))
                        .OnCompleted(() =>
                        {
                            hasCloseDischargeValve_2 = true;
                            if (hasCloseDischargeValve_2 && hasClosePump_2)
                            {
                                ProductionGuideManager.Instance.CloseAllGuide();
                                SmallActionManager.Instance.UpdateSmallAction("2-3-9", true);
                                EventDispatcher.ExecuteEvent(Events.Procedure.Completed, 2, 3);
                                EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveButtonToReservoir);
                                if (!usable_100L.IsUsing)
                                {
                                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                                    FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                                }
                            }
                        })
                        .Execute();
                    break;
                case "2-4-3":
                    //2-4-3 启动取样灭菌相关阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-4-2", true))
                        .Append(new CheckSmallAction("2-4-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-4-3", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "启动取样灭菌相关阀门", Color.blue))
                        .Append(new InvokeFocusAction("100L储液灭菌"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new ShowProgressAction("持续灭菌中。。。",5,true))
                        .Append(new CloseFocusAction())
                        .OnCompleted(() =>
                        {
                            plcPipeFitting_Reservoir.InvokeFitting("2-4-4", true,PLCValveComponent.Shake.ColorChange);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(4);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
                    break;
                case "2-4-4":
                    //2-4-4 关闭取样灭菌相关阀门
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-4-3", true))
                        .Append(new CheckSmallAction("2-4-4", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-4-4", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "启动取样灭菌相关阀门", Color.blue))
                        .Append(new InvokeFocusAction("100L储液灭菌"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new DelayedAction(2))
                        .Append(new CloseFocusAction())
                        .Append(new GameObjectAction(sampleBottle_100L,true))
                        .Append(new ChangeVirtualRealityAction(sampleBottle_100L,true))
                        .Append(new InvokeCurrentGuideAction(5))
                        .Append(new InvokeFlashAction(true, sampleBottle_100L))
                        .Execute();
                    break;
                case "2-4-10":
                    //2-4-4 打开出料阀、出料泵
                    Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-4-9", true))
                        .Append(new CheckSmallAction("2-4-10", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-4-10", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "打开出料阀、出料泵", Color.blue))
                        .Append(new InvokeFocusAction("100L出料"))
                        .Append(new InvokeFittingAction(arg0))
                        .Append(new GameObjectFadeAction(belly_100LReservoir,1,0.25f))
                        .Append(new ShowProgressAction("正在出料。。。",5))
                        .Append(new CoroutineFluidLevelAction(fluidLevel_100LReservoir.gameObject,0,5))
                        .Append(new NewUnityAction(() =>
                        {
                            plc_100LReservoirTank.textLevel.text = "0";
                        }))
                        .Append(new ShowHUDTextAction(plc_100L.transform,"出料完毕",Color.blue))
                        .Append(new GameObjectFadeAction(belly_100LReservoir, 0.25f, 1))
                        .Append(new InvokeFittingAction("2-4-11"))
                        .Append(new NewUnityAction(()=> 
                        {
                            plcPipeFitting_Reservoir.InvokeFitting("2-4-11", false);
                        }))
                        .Append(new CloseFocusAction())
                        .Append(new InvokeCompletedAction(2,4))
                        .Append(new InvokeCurrentGuideAction(1))
                        .Append(new InvokeFlashAction(true, powerButton_2.gameObject))
                        .Execute();
                    break;

                
                default:
                    break;
            }
        }
        Dictionary<Transform, Vector3> cachePumpPositions = new Dictionary<Transform, Vector3>();
        private void ShakePump(Transform pump, bool isOn)
        {
            if (isOn)
            {
                if (!cachePumpPositions.ContainsKey(pump))
                {
                    cachePumpPositions.Add(pump, pump.position);
                }
                pump.DOLocalMoveZ(0.005f, 0.1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                pump.DOKill();
                Vector3 position = new Vector3();
                if (cachePumpPositions.TryGetValue(pump,out position))
                {
                    pump.position = position;
                }
            }
        }

        //2-1-7 2-3-6 关闭搅拌电机
        private void StopMotor()
        {
            Task.NewTask(plc_100L.gameObject)
                       .Append(new CheckSmallAction("2-1-6", true))
                       .Append(new CheckSmallAction("2-1-7", false))
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new UpdateSmallAction("2-1-7", true))
                       .Append(new ShowHUDTextAction(plc_100L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("100L配制罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_100L.DOKill();
                           blender_100L.DOLocalRotate(new Vector3(blender_100L.localEulerAngles.x, blender_100L.localEulerAngles.y, blender_100L.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       //.Append(new InvokeCurrentGuideAction(8))
                       .OnCompleted(() =>
                       {
                           plcPipeFitting.InvokeFitting("2-1-9", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_100L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(8);
                               FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                           }
                       })
                       .Execute();
            Task.NewTask(plc_100L.gameObject)
                       .Append(new CheckSmallAction("2-3-5", true))
                       .Append(new CheckSmallAction("2-3-6", false))
                       .Append(new InvokeCloseAllGuideAction())
                       .Append(new UpdateSmallAction("2-3-6", true))
                       .Append(new ShowHUDTextAction(plc_100L.transform, "关闭搅拌电机", Color.blue))
                       .Append(new InvokeFocusAction("100L配制罐"))
                       .Append(new NewUnityAction(() =>
                       {
                           blender_100L.DOKill();
                           blender_100L.DOLocalRotate(new Vector3(blender_100L.localEulerAngles.x, blender_100L.localEulerAngles.y, blender_100L.localEulerAngles.z + 720), 3, RotateMode.FastBeyond360);
                       }))
                       .Append(new DelayedAction(3.5f))
                       .Append(new GameObjectFadeAction(belly_100L, 0.25f, 1))
                       .Append(new DelayedAction(0.5f))
                       .Append(new CloseFocusAction())
                       //.Append(new InvokeCurrentGuideAction(7))
                       .OnCompleted(() =>
                       {
                           plcPipeFitting.InvokeFitting("2-3-8", true, PLCValveComponent.Shake.ColorChange);
                           if (!usable_100L.IsUsing)
                           {
                               ProductionGuideManager.Instance.ShowCurrentGuide(7);
                               FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                           }
                       })
                       .Execute();
        }

        //2-1-6  2-3-5 开启搅拌电机
        private void StartMotor()
        {
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-5", true))
                        .Append(new CheckSmallAction("2-1-6", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-1-6", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_100L.DOLocalRotate(new Vector3(blender_100L.localEulerAngles.x, blender_100L.localEulerAngles.y, blender_100L.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        //.Append(new GameObjectFadeAction(belly_100L, 0.25f, 1, 0))
                        //.Append(new NewUnityAction(() => blender_100L.DOKill()))
                        //.Append(new InvokeCurrentGuideAction(7))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(7);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(plc_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-4", true))
                        .Append(new CheckSmallAction("2-3-5", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new UpdateSmallAction("2-3-5", true))
                        .Append(new ShowHUDTextAction(plc_100L.transform, "开启搅拌电机", Color.blue))
                        .Append(new InvokeFocusAction("100L配制罐"))
                        .Append(new GameObjectFadeAction(belly_100L, 1, 0.25f))
                        .Append(new NewUnityAction(() =>
                        {
                            blender_100L.DOLocalRotate(new Vector3(blender_100L.localEulerAngles.x, blender_100L.localEulerAngles.y, blender_100L.localEulerAngles.z + 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
                        }))
                        .Append(new ShowProgressAction("持续搅拌中。。。", 5, true))
                        .Append(new CloseFocusAction())
                        //.Append(new InvokeCurrentGuideAction(6))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(6);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
        }

        //2-1-5 2-3-4 加入物料F001S
        private void Entrance_100L_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            Task.NewTask(entrance_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-4", true))
                        .Append(new CheckSmallAction("2-1-5", false))
                        .Append(new CheckGoodsAction(eventData, GoodsType.PYG_F001S_01))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, entrance_100L))
                        .Append(new InvokeFlashAction(false, cover_100L.gameObject))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_F001S_01.ToString(), UpdateType.Remove))
                        .Append(new UpdateSmallAction("2-1-5", true))
                        .Append(new GameObjectAction(bucket_feedF001S, true))
                        .Append(new NewUnityAction(() => particle_feedF001S.Play()))
                        .Append(new DelayedAction(3))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feedF001S.Stop();
                            liquidVolume_100L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feedF001S, false))
                        .Append(new ShowHUDTextAction(entrance_100L.transform, "已加入物料F001S", Color.blue))
                        .Append(new DOLocalRotaAction(cover_100L, new Vector3(cover_100L.localEulerAngles.x, cover_100L.localEulerAngles.y, cover_100L.localEulerAngles.z + 135), 0.5f))
                        //.Append(new InvokeCurrentGuideAction(6))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(6);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
            Task.NewTask(entrance_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-3", true))
                        .Append(new CheckSmallAction("2-3-4", false))
                        .Append(new CheckGoodsAction(eventData, GoodsType.PYG_F001B_01))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, entrance_100L))
                        .Append(new UpdateGoodsAction(GoodsType.PYG_F001B_01.ToString(), UpdateType.Remove))
                        .Append(new UpdateSmallAction("2-3-4", true))
                        .Append(new GameObjectAction(bucket_feedF001S, true))
                        .Append(new NewUnityAction(() => particle_feedF001S.Play()))
                        .Append(new DelayedAction(3))
                        .Append(new NewUnityAction(() =>
                        {
                            particle_feedF001S.Stop();
                            liquidVolume_100L.liquidColor1 = Color.yellow;

                        }))
                        .Append(new GameObjectAction(bucket_feedF001S, false))
                        .Append(new ShowHUDTextAction(entrance_100L.transform, "已加入物料F001S", Color.blue))
                        .Append(new DOLocalRotaAction(cover_100L, new Vector3(cover_100L.localEulerAngles.x, cover_100L.localEulerAngles.y, cover_100L.localEulerAngles.z + 135), 0.5f))
                        //.Append(new InvokeCurrentGuideAction(5))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveMotorToggle);
                            if (!usable_100L.IsUsing)
                            {
                                ProductionGuideManager.Instance.ShowCurrentGuide(5);
                                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                            }
                        })
                        .Execute();
        }
        //2-1-4 2-3-3 打开投料盖
        private void Cover_100L_PointerClick(BaseEventData arg0)
        {
            Task.NewTask(cover_100L.gameObject)
                        .Append(new CheckSmallAction("2-1-3", true))
                        .Append(new CheckSmallAction("2-1-4", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, cover_100L.gameObject))
                        .Append(new UpdateSmallAction("2-1-4", true))
                        .Append(new DOLocalRotaAction(cover_100L, new Vector3(cover_100L.localEulerAngles.x, cover_100L.localEulerAngles.y, cover_100L.localEulerAngles.z - 135), 0.5f))
                        .Append(new ShowHUDTextAction(cover_100L.transform, "打开投料盖", Color.blue))
                        .Append(new InvokeCurrentGuideAction(5))
                        .Append(new InvokeFlashAction(true, entrance_100L))
                        .Execute();
            Task.NewTask(cover_100L.gameObject)
                        .Append(new CheckSmallAction("2-3-2", true))
                        .Append(new CheckSmallAction("2-3-3", false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeFlashAction(false, cover_100L.gameObject))
                        .Append(new UpdateSmallAction("2-3-3", true))
                        .Append(new DOLocalRotaAction(cover_100L, new Vector3(cover_100L.localEulerAngles.x, cover_100L.localEulerAngles.y, cover_100L.localEulerAngles.z - 135), 0.5f))
                        .Append(new ShowHUDTextAction(cover_100L.transform, "打开投料盖", Color.blue))
                        .Append(new InvokeCurrentGuideAction(4))
                        .Append(new InvokeFlashAction(true, entrance_100L))
                        .Execute();
        }



        
        private void SetWaterFinish()
        {
            //2-1-2 设置纯化水加入量
            Task.NewTask(plc_100L.gameObject)
                .Append(new CheckSmallAction("2-1-1", true))
                .Append(new CheckSmallAction("2-1-2", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new UpdateSmallAction("2-1-2", true))
                .Append(new ShowHUDTextAction(plc_100L.transform, "纯化水加入量设置完毕", Color.blue))
                .OnCompleted(() =>
                {
                    //isNeedIndicator_100L = true;
                    plcPipeFitting.InvokeFitting("2-1-3-1", true, PLCValveComponent.Shake.ColorChange);
                    if (!usable_100L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                    }
                })
                .Execute();
            //2-3-1 设置纯化水加入量
            Task.NewTask(plc_100L.gameObject)
                .Append(new CheckSmallAction("2-2-9", true))
                .Append(new CheckSmallAction("2-3-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new UpdateSmallAction("2-3-1", true))
                .Append(new ShowHUDTextAction(plc_100L.transform, "纯化水加入量设置完毕", Color.blue))
                .OnCompleted(() =>
                {
                    plcPipeFitting.InvokeFitting("2-3-2-1", true, PLCValveComponent.Shake.ColorChange);

                    if (!usable_100L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                    }
                })
                .Execute();
        }

        private void Usable100LOnExit()
        {
            if (SmallActionManager.Instance.CheckSmallAction("2-1-1", true) && SmallActionManager.Instance.CheckSmallAction("2-1-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-2");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-2", true) && SmallActionManager.Instance.CheckSmallAction("2-1-3", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-3");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-5", true) && SmallActionManager.Instance.CheckSmallAction("2-1-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-6");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-6", true) && SmallActionManager.Instance.CheckSmallAction("2-1-7", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-7");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-7", true) && SmallActionManager.Instance.CheckSmallAction("2-1-8", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-8");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-8", true) && SmallActionManager.Instance.CheckSmallAction("2-1-9", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-9");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-1-9", true) && SmallActionManager.Instance.CheckSmallAction("2-1-10", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-1-10");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            
            if (SmallActionManager.Instance.CheckSmallAction("2-2-9", true) && SmallActionManager.Instance.CheckSmallAction("2-3-1", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-1");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-1", true) && SmallActionManager.Instance.CheckSmallAction("2-3-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-2");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-4", true) && SmallActionManager.Instance.CheckSmallAction("2-3-5", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-5");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-5", true) && SmallActionManager.Instance.CheckSmallAction("2-3-6", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-6");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-6", true) && SmallActionManager.Instance.CheckSmallAction("2-3-7", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-7");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-7", true) && SmallActionManager.Instance.CheckSmallAction("2-3-8", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-8");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-8", true) && SmallActionManager.Instance.CheckSmallAction("2-3-9", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-3-9");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-3-9", true) && SmallActionManager.Instance.CheckSmallAction("2-4-1", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-4-1");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-4-1", true) && SmallActionManager.Instance.CheckSmallAction("2-4-2", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-4-2");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-4-2", true) && SmallActionManager.Instance.CheckSmallAction("2-4-3", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-4-3");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-4-3", true) && SmallActionManager.Instance.CheckSmallAction("2-4-4", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-4-4");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
            if (SmallActionManager.Instance.CheckSmallAction("2-4-9", true) && SmallActionManager.Instance.CheckSmallAction("2-4-10", false))
            {
                ProductionGuideManager.Instance.ShowGuide("2-4-10");
                FlashManager.Instance.ShowFlash(usable_100L.gameObject);
            }
        }

        private void Usable100LOnUse()
        {
            FocusComponent m_FocusComponent = usable_100L.transform.GetComponentInChildren<FocusComponent>();
            m_CameraSwitcher.Switch(CameraStyle.Look);
            Debug.Log("Usable100LOnUse");
            FlashManager.Instance.CloseFlash(usable_100L.gameObject);
            m_FocusComponent.Focus(()=>
            {
                Task.NewTask()
                .Append(new CheckSmallAction("2-1-1", true))
                .Append(new CheckSmallAction("2-1-2", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveButtonSetWater);
                })
                .Execute();
                Task.NewTask()
                .Append(new CheckSmallAction("2-2-9", true))
                .Append(new CheckSmallAction("2-3-1", false))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.ActiveButtonSetWater);
                })
                .Execute();
            });
            ProductionGuideManager.Instance.CloseGuideByName("100L控制面板");
        }

        //2-1-1 打开100L配制罐电源
        private void PowerButton_1_PointerClick(BaseEventData arg)
        {
            Task.NewTask(powerButton_1.gameObject)
                //.Append(new CheckSmallAction("1-2-4", true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor,1,2))
                .Append(new CheckSmallAction("2-1-1", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, powerButton_1.gameObject))
                .Append(new UpdateSmallAction("2-1-1", true))
                .Append(new DOLocalRotaAction(powerButton_1, new Vector3(powerButton_1.localEulerAngles.x, powerButton_1.localEulerAngles.y + 90, powerButton_1.localEulerAngles.z), 0.5f))
                .Append(new GameObjectAction(plc_100L.gameObject, true))
                .Append(new ShowHUDTextAction(usable_100L.transform, "打开100L配制罐电源", Color.blue))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent(PLC_100LDispensingTankEvent.CurrentSolutionType, PLC_100LDispensingTank.SolutionType.C);
                    if (!usable_100L.IsUsing)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        FlashManager.Instance.ShowFlash(usable_100L.gameObject);
                    }
                })
                .Execute();
        }

        private void BucketF001S_PointerClick(BaseEventData arg)
        {
            bucket_F001S_cover = bucket_F001S.Find("盖子");
            bucket_F001S_level = bucket.Find("物料");
            scoopParticle = scoop.Find("ParticleSystem").GetComponent<ParticleSystem>();
            //1-2-4 称量F001S至指定重量
            Task.NewTask(bucket_F001S.gameObject)
                .Append(new CheckSmallAction("1-2-3", true))
                .Append(new CheckSmallAction("1-2-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, bucket_F001S.gameObject))
                .Append(new UpdateSmallAction("1-2-4", true))
                .Append(new DOLocalMoveAction(bucket_F001S_cover, new Vector3(bucket_F001S_cover.localPosition.x - 0.5f, bucket_F001S_cover.localPosition.y, bucket_F001S_cover.localPosition.z), 1, false))
                .Append(new GameObjectAction(bucket_F001S_cover, false))
                .OnCompleted((() =>
                {
                    scoop.gameObject.SetActive(true);
                    anim_scoop.Play();
                    StartCoroutine(WaitAnimEnd(anim_scoop, () =>
                    {
                        StartCoroutine(ChangeTMPText(tmp_bigBalanceNum, 5000, 1, () =>
                        {

                        }));
                        Task.NewTask()
                        .Append(new DOLocalMoveAction(bucket_F001S_level, new Vector3(bucket_F001S_level.localPosition.x, bucket_F001S_level.localPosition.y - 0.011f, bucket_F001S_level.localPosition.z), 1, false))
                        .Append(new NewUnityAction(() =>
                        {
                            scoopParticle.Stop();
                            anim_scoop.Stop();
                            scoop.gameObject.SetActive(true);
                            this.Invoke(0.5f, () =>
                            {
                                anim_scoop.Play();
                                StartCoroutine(WaitAnimEnd(anim_scoop, () =>
                                {
                                    StartCoroutine(ChangeTMPText(tmp_bigBalanceNum, 11250, 1));
                                    Task.NewTask()
                                    .Append(new DOLocalMoveAction(bucket_F001S_level, new Vector3(bucket_F001S_level.localPosition.x, bucket_F001S_level.localPosition.y - 0.011f, bucket_F001S_level.localPosition.z), 1, false))
                                    .Append(new NewUnityAction(() =>
                                    {
                                        scoopParticle.Stop();
                                        anim_scoop.Stop();
                                        scoop.gameObject.SetActive(false);

                                    }))
                                    .Append(new GameObjectAction(bucket_F001S_cover, true))
                                    .Append(new DOLocalMoveAction(bucket_F001S_cover, new Vector3(bucket_F001S_cover.localPosition.x + 0.5f, bucket_F001S_cover.localPosition.y, bucket_F001S_cover.localPosition.z), 1, false))
                                    .Append(new ShowHUDTextAction(tmp_bigBalanceNum.transform, "称量至指定重量", Color.blue))
                                    .Append(new GameObjectAction(bucket, false))
                                    .Append(new UpdateGoodsAction(GoodsType.PYG_F001S_01.ToString(), UpdateType.Add))
                                    .Append(new InvokeCompletedAction(1, 2))
                                    .Append(new InvokeCurrentGuideAction(1))
                                    .Append(new InvokeFlashAction(true, powerButton_1.gameObject))
                                    .OnCompleted(() =>
                                    {
                                        tmp_bigBalanceNum.text = "0";
                                        MessageBoxEx.Show("后续继续称量750g F001B", "提示", MessageBoxExEnum.SingleDialog, x=> 
                                        {
                                            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001B_01.ToString());
                                        });
                                    })
                                    .Execute();
                                }));

                            });
                        }))
                        .Execute();
                    }));

                }))
                .Execute();
        }

        private void ButtonCouAff2_onClick()
        {
            //1-2-3 物料桶去皮
            Task.NewTask(button_couAff2.gameObject)
                .Append(new CheckSmallAction("1-2-2", true))
                .Append(new CheckSmallAction("1-2-3", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, dropArea_bigBalance.gameObject))
                .Append(new UpdateSmallAction("1-2-3", true))
                .Append(new ShowHUDTextAction(tmp_bigBalanceNum.transform, "已去皮", Color.blue))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new InvokeFlashAction(true, bucket_F001S.gameObject))
                .OnCompleted(() =>
                {
                    tmp_bigBalanceNum.text = "0";
                })
                .Execute();
        }

        private void DropArea_bigBalance_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;
            ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            if (component != null)
            {
                Goods item = component.Item as Goods;
                switch (item.GoodsType)
                {
                    case GoodsType.PYG_MB_01:
                        //1-2-1 清洁天平
                        Task.NewTask(dropArea_bigBalance.gameObject)
                            .Append(new CheckSmallAction("1-1-6", true))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new InvokeFlashAction(false, dropArea_bigBalance.gameObject))
                            .Append(new UpdateSmallAction("1-2-1", true))
                            .Append(new UpdateGoodsAction(GoodsType.PYG_MB_01.ToString(), UpdateType.Remove))
                            .OnCompleted(() =>
                            {
                                scrubEffect_bigBalance.OnExit.AddListener(() =>
                                {
                                    Task.NewTask()
                                    .Append(new ShowHUDTextAction(dropArea_balance, "已清洁天平", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(2))
                                    .Append(new InvokeFlashAction(true, dropArea_bigBalance.gameObject))
                                    .Execute();
                                });
                                scrubEffect_bigBalance.IsOn = true;
                            })
                            .Execute();
                        break;
                    case GoodsType.PYG_WLT_01:
                        //1-2-2 放置物料桶
                        Task.NewTask(dropArea_bigBalance.gameObject)
                            .Append(new CheckSmallAction("1-2-1", true))
                            .Append(new CheckSmallAction("1-2-2", false))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new InvokeFlashAction(false, dropArea_bigBalance.gameObject))
                            .Append(new UpdateSmallAction("1-2-2", true))
                            .Append(new UpdateGoodsAction(GoodsType.PYG_WLT_01.ToString(), UpdateType.Remove))
                            .Append(new GameObjectAction(bucket, true))
                            .Append(new ShowHUDTextAction(dropArea_bigBalance, "已放置物料桶", Color.blue))
                            .Append(new NewUnityAction(() => { tmp_bigBalanceNum.text = "200"; }))
                            .Append(new InvokeCurrentGuideAction(3))
                            .Execute();
                        break;
                    default:
                        break;
                }
            }
        }

        private void MediumOriginal_PointerClick(BaseEventData arg)
        {
            
            //1-1-6 称量培养基(第二次)
            Task.NewTask(medium_original.gameObject)
                .Append(new CheckSmallAction("1-1-5", true))
                .Append(new CheckSmallAction("1-1-6", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, medium_original.gameObject))
                .Append(new UpdateSmallAction("1-1-6", true))
                .Append(new GameObjectAction(medium_original, false))
                .Append(new GameObjectAction(medium_feed, true))
                .Append(new GameObjectAction(cover.gameObject, false))
                .Append(new DOLocalRotaAction(medium_feed, new Vector3(medium_feed.localEulerAngles.x, medium_feed.localEulerAngles.y, medium_feed.localEulerAngles.z + 45), 1, false))
                .Append(new NewUnityAction(() =>
                {
                    spoon.gameObject.SetActive(true);
                    anim_spoon.Play();
                    StartCoroutine(WaitAnimEnd(anim_spoon, () =>
                     {
                         Task.NewTask(medium_original.gameObject)
                         .Append(new NewUnityAction(() =>
                         {
                             //particle.Play();
                             StartCoroutine(ChangeTMPText(tmp_balanceNum, 1800, 1));
                         }))
                         .Append(new DelayedAction(1))
                         .Append(new GameObjectAction(spoon, false))
                         .Append(new DOLocalRotaAction(medium_feed, new Vector3(medium_feed.localEulerAngles.x, medium_feed.localEulerAngles.y, medium_feed.localEulerAngles.z - 45), 0.5f, false))
                         .Append(new GameObjectAction(cover, true))
                         .Append(new DOLocalMoveAction(cover, new Vector3(cover.localPosition.x, cover.localPosition.y - 0.05f, cover.localPosition.z), 0.25f))
                         .Append(new GameObjectAction(medium_feed, false))
                         .Append(new GameObjectAction(medium_original, true))
                         .Append(new InvokeFocusAction("天平示数"))
                         .Append(new DelayedAction(1))
                         .Append(new ShowHUDTextAction(tmp_balanceNum.transform, "称量至指定重量", Color.blue))
                         .Append(new CloseFocusAction())
                         .Append(new InvokeCompletedAction(1, 1))
                         .Append(new InvokeCurrentGuideAction(1))
                         .Append(new InvokeFlashAction(true, dropArea_bigBalance.gameObject))
                         .OnCompleted(() =>
                         {
                             beaker.gameObject.SetActive(false);
                             tmp_balanceNum.text = "0";
                             EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_01.ToString());
                             MessageBoxEx.Show("后续继续称量90g poloxamer188,450g葡萄糖,1800g M20A", "提示", MessageBoxExEnum.SingleDialog, x =>
                             {
                                 
                                 EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_02.ToString());
                                 EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_03.ToString());
                                 EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_M20A_01.ToString());
                             });
                             
                         })
                         .Execute();
                     }));
                }))
                .Execute();
            //1-1-5 称量培养基(第一次)
            Task.NewTask(medium_original.gameObject)
                .Append(new CheckSmallAction("1-1-4", true))
                .Append(new CheckSmallAction("1-1-5", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new InvokeFlashAction(false, medium_original.gameObject))
                .Append(new UpdateSmallAction("1-1-5", true))
                .Append(new GameObjectAction(medium_original, false))
                .Append(new GameObjectAction(medium_feed, true))
                .Append(new DOLocalMoveAction(cover, new Vector3(cover.localPosition.x, cover.localPosition.y + 0.05f, cover.localPosition.z), 0.25f))
                .Append(new GameObjectAction(cover.gameObject, false))
                .Append(new DOLocalRotaAction(medium_feed, new Vector3(medium_feed.localEulerAngles.x, medium_feed.localEulerAngles.y, medium_feed.localEulerAngles.z + 60), 1, false))
                .Append(new NewUnityAction(() => { StartCoroutine(ChangeTMPText(tmp_balanceNum, 1750, 5)); }))
                //.Append(new CoroutineFluidLevelAction(liquid_beaker.gameObject, 0.33f, 5))
                .Append(new NewUnityAction(() => 
                {
                    beakerContent.gameObject.SetActive(true);
                    beakerContent.DOScaleZ(beakerContent.localScale.z + 0.1f, 5);
                }))
                .Append(new DelayedAction(5))
                .Append(new DOLocalRotaAction(medium_feed, new Vector3(medium_feed.localEulerAngles.x, medium_feed.localEulerAngles.y, medium_feed.localEulerAngles.z), 0.5f, false))
                .Append(new GameObjectAction(medium_feed, false))
                .Append(new GameObjectAction(medium_original, true))
                .Append(new InvokeFocusAction("天平示数"))
                .Append(new DelayedAction(1))
                .Append(new ShowHUDTextAction(tmp_balanceNum.transform, "完成初步称量", Color.blue))
                .Append(new CloseFocusAction())
                .Append(new InvokeCurrentGuideAction(6))
                .Append(new InvokeFlashAction(true, medium_original.gameObject))
                .Execute();
        }

        private void ButtonCouAff_onClick()
        {
            //1-1-4 烧杯去皮
            Task.NewTask(button_couAff.gameObject)
                .Append(new CheckSmallAction("1-1-3", true))
                .Append(new CheckSmallAction("1-1-4", false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new UpdateSmallAction("1-1-4", true))
                .Append(new ShowHUDTextAction(tmp_balanceNum.transform, "已去皮", Color.blue))
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new InvokeFlashAction(true, medium_original.gameObject))
                .OnCompleted(() =>
                {
                    tmp_balanceNum.text = "0";
                })
                .Execute();
        }

        private void DropArea_balance_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;
            ProductionItemElement component = eventData.pointerDrag.GetComponent<ProductionItemElement>();
            if (component != null)
            {
                Goods item = component.Item as Goods;
                switch (item.GoodsType)
                {
                    case GoodsType.PYG_MB_01:
                        //1-1-1 清洁天平
                        Task.NewTask(dropArea_balance.gameObject)
                            .Append(new CheckSmallAction("1-1-1", false))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new InvokeFlashAction(false,dropArea_balance.gameObject))
                            .Append(new UpdateSmallAction("1-1-1", true))
                            //.Append(new UpdateGoodsAction(GoodsType.PYG_MB_01.ToString(), UpdateType.Remove))
                            .OnCompleted(() =>
                            {
                                scrubEffect_balance.OnExit.AddListener(() =>
                                {
                                    Task.NewTask()
                                    .Append(new ShowHUDTextAction(dropArea_balance, "已清洁天平", Color.blue))
                                    .Append(new InvokeCurrentGuideAction(3))
                                    .Append(new InvokeFlashAction(true, dropArea_balance.gameObject))
                                    .Execute();
                                });
                                scrubEffect_balance.IsOn = true;
                            })
                            .Execute();
                        break;
                    case GoodsType.PYG_SB_01:
                        //1-1-3 放置烧杯
                        Task.NewTask(dropArea_balance.gameObject)
                            .Append(new CheckSmallAction("1-1-1", true))
                            .Append(new CheckSmallAction("1-1-3", false))
                            .Append(new InvokeCloseAllGuideAction())
                            .Append(new InvokeFlashAction(false, dropArea_balance.gameObject))
                            .Append(new UpdateSmallAction("1-1-3", true))
                            .Append(new UpdateGoodsAction(GoodsType.PYG_SB_01.ToString(), UpdateType.Remove))
                            .Append(new GameObjectAction(beaker, true))
                            .Append(new ShowHUDTextAction(beaker, "已放置烧杯", Color.blue))
                            .Append(new NewUnityAction(() => { tmp_balanceNum.text = "15.6"; }))
                            .Append(new DelayedAction(0.25f))
                            .Append(new InvokeFocusAction("天平示数"))
                            .Append(new DelayedAction(1))
                            .Append(new CloseFocusAction())
                            .Append(new InvokeCurrentGuideAction(4))
                            .Execute();
                        break;
                    default:
                        break;
                }
            }
        }

        IEnumerator WaitAnimEnd(Animation anim, UnityAction callback = null)
        {
            while (anim.isPlaying)
            {
                yield return null;
            }
            if (callback != null)
            {
                callback.Invoke();
            }
        }

        IEnumerator ChangeTMPText(TextMeshProUGUI text, float endValue, float duration, UnityAction callback = null)
        {
            float startValue = float.Parse(text.text);
            FormatTMPText(text, startValue);
            float speed = (endValue - startValue) / duration;
            if (startValue > endValue)
            {
                while (startValue > endValue)
                {
                    startValue -= speed * Time.deltaTime;
                    FormatTMPText(text, startValue);
                    yield return null;
                }
            }
            if (startValue < endValue)
            {
                while (startValue < endValue)
                {
                    startValue += speed * Time.deltaTime;
                    FormatTMPText(text, startValue);
                    yield return null;
                }
            }
            startValue = endValue;
            FormatTMPText(text, startValue);
        }

        private void FormatTMPText(TextMeshProUGUI text, float value)
        {
            if (value == (int)value)
            {
                text.text = value.ToString();
            }
            else
            {
                text.text = value.ToString("F1");
            }
        }

        #region 相机聚焦
        /// <summary>
        /// 使用观察窗（F）
        /// </summary>
        private void GlassWindow_OnUsable()
        {
            //Task.NewTask(glassWindow.gameObject)
            //    .Append(new CheckValveAction(ValveManager.Instance.GetValve("GD-R203A-101"), ValveState.ON, "请先打开洁净釜罐底阀"))
            //    .Append(new CheckBoolAction(isFinishedFeed, false))
            //    .OnCompleted(() => {
            //        FocusComponent m_FocusComponent = glassWindow.GetComponentInChildren<FocusComponent>();
            //        m_CameraSwitcher.Switch(CameraStyle.Focus);
            //        glassWindow.GetComponent<BoxCollider>().enabled = false;
            //        m_FocusComponent.Focus();
            //    }).Execute();
        }

        /// <summary>
        /// 相机切换
        /// </summary>
        /// <param name="arg0"></param>
        private void m_CameraSwitcher_OnCameraSwitch(CameraStyle arg0)
        {
            switch (arg0)
            {
                case CameraStyle.Walk:
                    usable_100L.collider.enabled = true;
                    plc_100L.GetComponent<GraphicRaycaster>().enabled = false;
                    usable_detector.collider.enabled = true;
                    plc_FilterDetector.GetComponent<GraphicRaycaster>().enabled = false;
                    usable_200L.collider.enabled = true;
                    plc_200L.GetComponent<GraphicRaycaster>().enabled = false;
                    break;
                case CameraStyle.Look:
                    usable_100L.collider.enabled = false;
                    plc_100L.GetComponent<GraphicRaycaster>().enabled = true;
                    usable_detector.collider.enabled = false;
                    plc_FilterDetector.GetComponent<GraphicRaycaster>().enabled = true;
                    usable_200L.collider.enabled = false;
                    plc_200L.GetComponent<GraphicRaycaster>().enabled = true;
                    break;
                default:
                    break;
            };
        }

        /// <summary>
        /// 聚焦完成
        /// </summary>
        private void m_MouseOrbit_OnFocusCompleted()
        {

        }
        #endregion

        /// <summary>
        /// 阀门控制
        /// </summary>
        /// <param name="arg">阀门</param>
        /// <param name="name">阀门ID</param>
        private void ValveManager_OnClicked(GameObject arg, string name)
        {
            switch (name)
            {
                #region 氮气进气阀（VQ-S210-101）
                case "VQ-S210-101":
                    //关闭氮气进气阀VQ-S210-101
                    //Task.NewTask(arg)
                    //    .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, true))
                    //    .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                    //    .Append(new CheckValveAction(arg, ValveState.ON))
                    //    .Append(new UpdateValveAction(arg, ValveState.OFF))
                    //    .Append(new ShowLogInfoAction("已关闭氮气进气阀", LogType.Log))
                    //    .Append(new InvokeCloseAllGuideAction())
                    //    .Append(new InvokeCurrentFittingAction(2))
                    //    .Append(new InvokeCompletedAction(2, 1))
                    //    .OnCompleted(() =>
                    //    {
                    //        ProductionGuideManager.Instance.ShowCurrentGuide();
                    //    }).Execute();
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.SetWaterFinish, SetWaterFinish);
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.StartMotor, StartMotor);
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.StopMotor, StopMotor);
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.StartSanitaryPump, StartSanitaryPump);
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.StopSanitaryPump, StopSanitaryPump);
            EventDispatcher.UnregisterEvent(PLC_FilterDetectorEvent.ButtonBubblePointClick, ButtonBubblePointClick);
            EventDispatcher.UnregisterEvent(PLC_FilterDetectorEvent.ButtonOKClick, ButtonOKClick);
            EventDispatcher.UnregisterEvent(PLC_FilterDetectorEvent.ButtonStartClick, ButtonStartClick);
            EventDispatcher.UnregisterEvent(PLC_100LReservoirTankEvent.StartMotor, StartReservoirMotor);
            EventDispatcher.UnregisterEvent(PLC_100LReservoirTankEvent.StopMotor, StopReservoirMotor);
            EventDispatcher.UnregisterEvent(PLC_200LReservoirTankEvent.StartMotor, StartReservoirMotor_2);
            EventDispatcher.UnregisterEvent(PLC_200LReservoirTankEvent.StopMotor, StopReservoirMotor_2);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.SetWaterFinish, SetWaterFinish_2);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.StartMotor, StartMotor_2);
            EventDispatcher.UnregisterEvent(PLC_100LDispensingTankEvent.StopMotor, StopMotor_2);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.StartSanitaryPump, StartSanitaryPump_2);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.StopSanitaryPump, StopSanitaryPump_2);
        }
    }
}
