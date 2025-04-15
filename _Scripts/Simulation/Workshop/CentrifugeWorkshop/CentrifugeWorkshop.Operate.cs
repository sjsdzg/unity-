using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Actions;
using XFramework.Common;
using XFramework.Module;
using DG.Tweening;
using XFramework.Component;
using XFramework.UI;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    public partial class CentrifugeWorkshop
    {
        #region 辅助变量
        /// <summary>
        /// 是否打开电源按钮
        /// </summary>
        private bool isOpenPowerSupply = false;
        /// <summary>
        /// 是否已经甩紧滤布(1_2_2 Over)
        /// </summary>
        private bool isOverFilterBag = false;
        /// <summary>
        /// 是否完成安装物料软管
        /// </summary>
        private bool isFinishedSuppliesHosePipe = false;
        /// <summary>
        /// 是否完成安装纯化水软管
        /// </summary>
        private bool isFinishedPurifiedWaterHosePipe = false;
        /// <summary>
        /// 是否完成安装氮气软管
        /// </summary>
        private bool isFinishedNitrogenHosePipe = false;
        /// <summary>
        /// 是否完全启动离心机
        /// </summary>
        private bool isStartCentrifuge = false;
        /// <summary>
        /// 是否第一次聚焦看向离心机内
        /// </summary>
        private bool isFristLooking = false;
        /// <summary>
        /// 是否完成进料
        /// </summary>
        private bool isFinishedFeed = false;

        /// <summary>
        /// 是否完成4_1
        /// </summary>
        private bool isFinished4_1 = false;

        /// <summary>
        /// 是否完成4_2
        /// </summary>
        private bool isFinished4_2 = false;

        /// <summary>
        /// 是否完成6_1
        /// </summary>
        private bool isFinished6_1 = false;

        /// <summary>
        /// 是否完成6_3_1
        /// </summary>
        private bool isFinished6_3_1 = false;

        /// <summary>
        /// 是否完成6_3_2
        /// </summary>
        private bool isFinished6_3_2 = false;

        /// <summary>
        /// 是否完成6_3_3
        /// </summary>
        private bool isFinished6_3_3 = false;
        /// <summary>
        /// 是否能够关闭纯化水阀门
        /// </summary>
        private bool isCanClosePW_107 = false;
        #endregion
        private void InitializeOperate()
        {
            filterBag.TriggerAction(EventTriggerType.Drop, FilterBag_Drop);
            filterBag.TriggerAction(EventTriggerType.PointerClick, FilterBag_PointerClick);
            suppliesHosePipe.TriggerAction(EventTriggerType.Drop, SuppliesHosePipe_Drop);
            purifiedWaterHosePipe.TriggerAction(EventTriggerType.Drop, PurifiedWaterHosePipe_Drop);
            nitrogenHosePipe.TriggerAction(EventTriggerType.Drop, NitrogenHosePipe_Drop);
            powerSupply.TriggerAction(EventTriggerType.PointerClick, PowerSupply_PointerClick);
            lowSpeedButton.TriggerAction(EventTriggerType.PointerClick, LowSpeedButton_PointerClick);
            highSpeedButton.TriggerAction(EventTriggerType.PointerClick, HighSpeedButton_PointerClick);
            stopButton.TriggerAction(EventTriggerType.PointerClick, StopButton_PointerClick);
            topCover.TriggerAction(EventTriggerType.PointerClick, TopCover_PointerClick);
            BucketPutArea.TriggerAction(EventTriggerType.Drop, BucketPutArea_Drop);
            bucket.TriggerAction(EventTriggerType.Drop, Bucket_Drop);
            glassWindow.OnUsable.AddListener(GlassWindow_OnUsable);
            m_CameraSwitcher.OnCameraSwitch.AddListener(m_CameraSwitcher_OnCameraSwitch);
            m_MouseOrbit.OnFocusCompleted.AddListener(m_MouseOrbit_OnFocusCompleted);
            ValveManager.Instance.OnClicked.AddListener(ValveManager_OnClicked);
            this.Invoke(0.2f, () =>
            {
                ProductionGuideManager.Instance.ShowCurrentGuide();
                glassWindow.Disable = true;
                Task.NewTask()
                    .Append(new GameObjectAction(filterBag.gameObject, true))
                    .Append(new ChangeVirtualRealityAction(filterBag.gameObject, true))
                    .Execute();
            });
            //this.Invoke(1f, () =>
            //{
            //    PipeFittingManager.Instance.TestPipeFitting();
            //});
        }

        /// <summary>
        /// 将滤饼倒入物料桶中
        /// </summary>
        /// <param name="arg0"></param>
        private void Bucket_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(BucketPutArea.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 3, false))
                .Append(new CheckBoolAction(isFinished6_3_3, false))
                .Append(new CheckBoolAction(isFinished6_3_2, true, "请先放置好物料桶"))
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_LDLB_01))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_LDLB_01.ToString(), UpdateType.Remove))
                .Append(new FeedingAction(supplies.gameObject, new Vector3(90, 0, 0), 9.5f,true))
                .Append(new DelayedAction(3))
                .Append(new GameObjectAction(tankLiquidLevel.gameObject,true))
                .Append(new CoroutineFluidLevelAction(tankLiquidLevel.gameObject,0.85f,4.5f,false))
                .Append(new GameObjectAction(suppliesLiquidLevel.gameObject,false))
                //缺少倒入物料的动作
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已完成收集物料"))
                .OnCompleted(() => {
                    isFinished6_3_3 = true;
                    Task.NewTask()
                        .Append(new DelayedAction(3))
                        .OnCompleted((UnityAction)(() =>
                         {
                             base.Completed(6, 3);
                             MessageBoxEx.Show("提示：此步骤为重复操作步骤", "重复离心", MessageBoxExEnum.SingleDialog, (Action<DialogResult>)(result =>
                             {
                                 bool flag = (bool)result.Content;
                                 if (flag)
                                 {
                                     Task.NewTask()
                                         .Append(new DelayedAction(3))
                                         .Append(new InvokeCompletedAction(7,1))
                                         .OnCompleted(() =>
                                         {
                                             ShowHUDText(Utils.NewGameObject().transform, "离心工段已完成", Color.green);
                                         }).Execute();
                                 }
                             }));
                         })).Execute();
                }).Execute();
        }

        /// <summary>
        /// 物料桶放置
        /// </summary>
        /// <param name="arg"></param>
        private void BucketPutArea_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(BucketPutArea.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 3, false))
                .Append(new CheckBoolAction(isFinished6_3_2, false))
                .Append(new CheckBoolAction(isFinished6_3_1, true,"请先取出滤袋和滤饼"))
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_WUT_01))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_WUT_01.ToString(), UpdateType.Remove))
                .Append(new GameObjectAction(bucket.gameObject, true))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已放置好物料桶"))
                .Append(new GameObjectAction(BucketPutArea.gameObject, false))
                .OnCompleted(() => {
                    isFinished6_3_2 = true;
                    ProductionGuideManager.Instance.ShowCurrentGuide(3);
                }).Execute();
        }
        #region 相机聚焦
        /// <summary>
        /// 使用观察窗（F）
        /// </summary>
        private void GlassWindow_OnUsable()
        {
            Task.NewTask(glassWindow.gameObject)
                .Append(new CheckValveAction(ValveManager.Instance.GetValve("GD-R203A-101"), ValveState.ON, "请先打开洁净釜罐底阀"))
                .Append(new CheckBoolAction(isFinishedFeed, false))
                .OnCompleted((UnityAction)(() => {
                    FocusComponent m_FocusComponent = glassWindow.GetComponentInChildren<FocusComponent>();
                    m_CameraSwitcher.Switch((CameraStyle)CameraStyle.Look);
                    glassWindow.GetComponent<BoxCollider>().enabled = false;
                    m_FocusComponent.Focus();
                })).Execute();
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
                    glassWindow.GetComponent<BoxCollider>().enabled = true;
                    break;
                case CameraStyle.Look:
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
            Task.NewTask(glassWindow.gameObject)
                .Append(new CheckValveAction(ValveManager.Instance.GetValve("GD-R203A-101"), ValveState.ON, "请先打开洁净釜罐底阀"))
                .Append(new CheckBoolAction(isFinishedFeed, false))
                .OnCompleted(() => {
                    Task.NewTask()
                        .Append(new CheckBoolAction(isFristLooking,false))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new ShowProgressAction("正在进料...",10))
                        .OnCompleted(() =>
                        {
                            isFristLooking = true;
                            Task.NewTask()//缺少进料效果和液体上升效果
                                .Append(new DelayedAction(10))
                                .Append(new ShowLogInfoAction("已进料完成",LogType.Log))
                                .OnCompleted(() =>
                                {
                                    isFinishedFeed = true;
                                    glassWindow.Disable = true;
                                    ProductionGuideManager.Instance.ShowCurrentGuide(4);
                                }).Execute();
                        }).Execute();
                }).Execute();
        }
        #endregion

        private bool isOpendTopCover = true;
        /// <summary>
        /// 上盖操作
        /// </summary>
        /// <param name="arg0"></param>
        private void TopCover_PointerClick(BaseEventData arg)
        {
            //关闭上盖
            Task.NewTask(topCover.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckBoolAction(isOpendTopCover, true))
                //.Append(new RotateAction(driveShaftOne.gameObject,new Vector3(0,3,0),1))//旋转中心错了，需要更改
                .OnCompleted(()=> {
                    isOpendTopCover = false;
                    driveShaftOne.DOBlendableLocalRotateBy(new Vector3(3, 0, 0), 1);
                    driveShaftTwo.DOBlendableLocalMoveBy(new Vector3(-0.0036f, -0.002f, 0.0753f), 1);//旋转轴错了，需要更改
                    Task.NewTask()
                        .Append(new RotateAction(topCover.gameObject, new Vector3(0, 45, 0), 1))
                        .Append(new DelayedAction(1))
                        .Append(new ShowLogInfoAction("已关闭上盖", LogType.Log))
                        .Append(new InvokeCompletedAction(1,3))
                        .Append(new GameObjectAction(suppliesHosePipe.gameObject,true))
                        .Append(new GameObjectAction(purifiedWaterHosePipe.gameObject, true))
                        .Append(new GameObjectAction(nitrogenHosePipe.gameObject, true))
                        .Append(new ChangeVirtualRealityAction(suppliesHosePipe.gameObject, true))
                        .Append(new ChangeVirtualRealityAction(purifiedWaterHosePipe.gameObject, true))
                        .Append(new ChangeVirtualRealityAction(nitrogenHosePipe.gameObject, true))
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowGuide("1-4-1", false);
                            ProductionGuideManager.Instance.ShowGuide("1-4-2", false);
                            ProductionGuideManager.Instance.ShowGuide("1-4-3", false);
                        }).Execute();
                }).Execute();

            //打开上盖
            Task.NewTask(topCover.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 1, true,"请等待停止转动"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 2, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new CheckBoolAction(isOpendTopCover,false))
                .Append(new GameObjectAction(suppliesHosePipe.gameObject, false))
                .Append(new GameObjectAction(purifiedWaterHosePipe.gameObject, false))
                .Append(new GameObjectAction(nitrogenHosePipe.gameObject, false))
                //.Append(new RotateAction(driveShaftOne.gameObject, new Vector3(0, 0, 0), 1))//旋转中心错了，需要更改
                .OnCompleted(() => {
                    isOpendTopCover = true;
                    driveShaftOne.DOBlendableLocalRotateBy(new Vector3(-3, 0, 0), 1);
                    driveShaftTwo.DOBlendableLocalMoveBy(new Vector3(0.0036f, 0.002f, -0.0753f), 1);//旋转轴错了，需要更改
                    Task.NewTask()
                        .Append(new RotateAction(topCover.gameObject, new Vector3(0, -45, 0), 1))
                        .Append(new DelayedAction(1))
                        .Append(new GameObjectAction(suppliesHosePipe2.gameObject, true))
                        .Append(new GameObjectAction(purifiedWaterHosePipe2.gameObject, true))
                        .Append(new GameObjectAction(nitrogenHosePipe2.gameObject, true))
                        .Append(new ShowLogInfoAction("已打开上盖", LogType.Log))
                        .Append(new InvokeCompletedAction(6, 2))
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowCurrentGuide(1);
                        }).Execute();
                }).Execute();
        }

        #region 拖动图标进行滤布和三个软管的安装
        /// <summary>
        /// 滤布的安装
        /// </summary>
        /// <param name="arg"></param>
        private void FilterBag_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(filterBag.gameObject)
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_LB_01))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_LB_01.ToString(), UpdateType.Remove))
                .Append(new ChangeVirtualRealityAction(filterBag.gameObject, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已铺入滤袋"))
                .Append(new InvokeCompletedAction(1,1))
                .OnCompleted(() => {
                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                }).Execute();
        }

        /// <summary>
        /// 滤布的收取
        /// </summary>
        /// <param name="arg"></param>
        private void FilterBag_PointerClick(BaseEventData arg)
        {
            Task.NewTask(filterBag.gameObject)
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 2, true))
                 .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 3, false))
                 .Append(new CheckBoolAction(isFinished6_3_1, false))
                 .OnCompleted(() => {
                     List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                     {
                        new ContextMenuParameter("收    集",X=>
                        {
                            Task.NewTask()
                                .Append(new GameObjectAction(filterBag.gameObject,false))
                                .Append(new ShowLogInfoAction("已收集好滤袋和滤饼"))
                                .Append(new UpdateGoodsAction(GoodsType.LXJ_LDLB_01.ToString(), UpdateType.Add))
                                .Append(new InvokeCloseAllGuideAction())
                                .Append(new GameObjectAction(BucketPutArea.gameObject,true))
                                .OnCompleted(()=> 
                                {
                                    isFinished6_3_1 = true;
                                    ProductionGuideManager.Instance.ShowCurrentGuide(2);
                                }).Execute();
                        }),
                        new ContextMenuParameter("关    闭", x => { ContextMenuEx.Instance.Hide(); })
                     };
                    //显示上下文菜单
                    ContextMenuEx.Instance.Show(filterBag.gameObject, Input.mousePosition, parameters);
                 }).Execute();
        }

        /// <summary>
        /// 物料软管的安装
        /// </summary>
        /// <param name="arg"></param>
        private void SuppliesHosePipe_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(suppliesHosePipe.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor,1,3,true,"请关闭上盖"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor,1,4,false,"操作错误"))
                .Append(new CheckBoolAction(isFinishedSuppliesHosePipe,false))
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_RG_01))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_RG_01.ToString(), UpdateType.Remove))
                .Append(new ChangeVirtualRealityAction(suppliesHosePipe.gameObject, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已安装物料软管"))
                .OnCompleted(() => {
                    isFinishedSuppliesHosePipe = true;
                    IsFinishedAllHosePipe();
                }).Execute();
        }

        /// <summary>
        /// 纯化水软管的安装
        /// </summary>
        /// <param name="arg"></param>
        private void PurifiedWaterHosePipe_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(purifiedWaterHosePipe.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true, "请关闭上盖"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, false, "操作错误"))
                .Append(new CheckBoolAction(isFinishedPurifiedWaterHosePipe, false))
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_RG_02))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_RG_02.ToString(), UpdateType.Remove))
                .Append(new ChangeVirtualRealityAction(purifiedWaterHosePipe.gameObject, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已安装纯化水软管"))
                .OnCompleted(() => {
                    isFinishedPurifiedWaterHosePipe = true;
                    IsFinishedAllHosePipe();
                }).Execute();
        }

        /// <summary>
        /// 氮气软管的安装
        /// </summary>
        /// <param name="arg"></param>
        private void NitrogenHosePipe_Drop(BaseEventData arg)
        {
            PointerEventData eventData = arg as PointerEventData;
            if (eventData == null)
                return;

            Task.NewTask(nitrogenHosePipe.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true, "请关闭上盖"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, false, "操作错误"))
                .Append(new CheckBoolAction(isFinishedNitrogenHosePipe, false))
                .Append(new CheckGoodsAction(eventData, GoodsType.LXJ_RG_03))
                .Append(new UpdateGoodsAction(GoodsType.LXJ_RG_03.ToString(), UpdateType.Remove))
                .Append(new ChangeVirtualRealityAction(nitrogenHosePipe.gameObject, false))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已安装氮气软管"))
                .OnCompleted(() => {
                    isFinishedNitrogenHosePipe = true;
                    IsFinishedAllHosePipe();
                }).Execute();
        }


        /// <summary>
        /// 是否完成三个软管的安装
        /// </summary>
        private void IsFinishedAllHosePipe()
        {
            //判断虚化的物料软管引导是否显示
            Task.NewTask()
                .Append(new CheckBoolAction(isFinishedSuppliesHosePipe, false))
                .OnCompleted(() =>
                {
                    ProductionGuideManager.Instance.ShowGuide("1-4-1", false);
                }).Execute();
            //判断虚化的物料软管引导是否显示
            Task.NewTask()
                .Append(new CheckBoolAction(isFinishedPurifiedWaterHosePipe, false))
                .OnCompleted(() =>
                {
                    ProductionGuideManager.Instance.ShowGuide("1-4-2", false);
                }).Execute();
            //判断虚化的氮气软管引导是否显示
            Task.NewTask()
                .Append(new CheckBoolAction(isFinishedNitrogenHosePipe, false))
                .OnCompleted(() =>
                {
                    ProductionGuideManager.Instance.ShowGuide("1-4-3", false);
                }).Execute();
            //判断三个软管是否全部安装完成
            Task.NewTask()
                .Append(new CheckBoolAction(isFinishedSuppliesHosePipe, true))
                .Append(new CheckBoolAction(isFinishedPurifiedWaterHosePipe, true))
                .Append(new CheckBoolAction(isFinishedNitrogenHosePipe, true))
                .Append(new ShowLogInfoAction("三个软管全部安装完毕", LogType.Log))
                .OnCompleted(() =>
                {
                    Completed(1, 4);
                    ProductionGuideManager.Instance.ShowCurrentGuide(1);
                }).Execute();
        }
        #endregion

        #region 电源，高速，低速，停止 四个按钮点击
        /// <summary>
        /// 电源按钮点击
        /// </summary>
        /// <param name="arg0"></param>
        private void PowerSupply_PointerClick(BaseEventData arg)
        {
            Task.NewTask(powerSupply.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true, "请先铺入滤袋"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false, "操作错误"))
                .Append(new CheckBoolAction(isOpenPowerSupply,false,"操作错误"))
                .Append(new SetPowerButtonAction(powerSupply, true, Color.red))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已开启电源", LogType.Log))
                .OnCompleted(() =>
                {
                    isOpenPowerSupply = true;
                    ProductionGuideManager.Instance.ShowCurrentGuide(2);
                }).Execute();
        }

        /// <summary>
        /// 低速按钮点击
        /// </summary>
        /// <param name="arg0"></param>
        private void LowSpeedButton_PointerClick(BaseEventData arg)
        {
            Task.NewTask(lowSpeedButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
                .Append(new CheckBoolAction(isOpenPowerSupply,true,"请先打开电源按钮"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
                .Append(new SetPowerButtonAction(lowSpeedButton, true, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已开启低速模式", LogType.Log))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 500))
                .OnCompleted(() =>
                {
                    filterBag.GetComponent<Renderer>().materials[0].SetTexture("_BumpMap", null);
                    Completed(3, 1);
                    Task.NewTask()
                        .Append(new ShowProgressAction("正在启动离心机...", 5))
                        .Append(new DelayedAction(5))
                        .OnCompleted(() =>
                        {
                            isStartCentrifuge = true;
                            ProductionGuideManager.Instance.ShowCurrentGuide(1);
                        }).Execute();
                }).Execute();

            Task.NewTask(lowSpeedButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckBoolAction(isOpenPowerSupply, true, "请先打开电源按钮"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new SetPowerButtonAction(lowSpeedButton, true, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已开启低速模式", LogType.Log))
                .Append(new ShowProgressAction("正在甩紧滤袋...", 5))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 500))
                .Append(new DelayedAction(5))
                .OnCompleted(() =>
                {
                    filterBag.GetComponent<Renderer>().materials[0].SetTexture("_BumpMap", null);
                    isOverFilterBag = true;
                    ProductionGuideManager.Instance.ShowCurrentGuide(3);
                }).Execute();
        }

        /// <summary>
        /// 高速按钮点击
        /// </summary>
        /// <param name="arg0"></param>
        private void HighSpeedButton_PointerClick(BaseEventData arg)
        {
            Task.NewTask(highSpeedButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, true))
                .Append(new CheckBoolAction(isFinished4_1, true, "请先按下停止按钮"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 2, false))
                .Append(new SetPowerButtonAction(highSpeedButton, true, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已开启高速模式", LogType.Log))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 1000))
                .OnCompleted(() =>
                {
                    Completed(4, 2);
                    Task.NewTask()
                        .Append(new ShowProgressAction("正在离心...", 10))
                        .Append(new DelayedAction(10))
                        .Append(new GameObjectAction(crystal.gameObject,true))
                        .OnCompleted(() =>
                        {
                            isFinished4_2 = true;
                            ProductionGuideManager.Instance.ShowCurrentGuide(1);
                        }).Execute();
                }).Execute();
        }

        /// <summary>
        /// 停止按钮点击
        /// </summary>
        /// <param name="arg"></param>
        private void StopButton_PointerClick(BaseEventData arg)
        {
            Task.NewTask(stopButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 5, 1, true))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 6, 1, false))
                .Append(new SetPowerButtonAction(highSpeedButton, false, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已按下停止按钮", LogType.Log))
                .Append(new ShowProgressAction("正在停止转动...",5))               
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 500))
                .Append(new DelayedAction(1f))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 400))
                .Append(new DelayedAction(1f))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 300))
                .Append(new DelayedAction(1f))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 200))
                .Append(new DelayedAction(1f))
                .Append(new LoopRotateAction(filterBag.gameObject, true, RotationAxis.Z, 100))
                .Append(new DelayedAction(1f))
                .Append(new LoopRotateAction(filterBag.gameObject, false, RotationAxis.Z, 100))
                .Append(new InvokeCompletedAction(6, 1))
                .OnCompleted(() =>
                {
                    ShowHUDText(stopButton.transform, "离心机已完全停止", Color.green);
                    isFinished6_1 = true;
                    ProductionGuideManager.Instance.ShowCurrentGuide();
                }).Execute();

            Task.NewTask(stopButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
                .Append(new CheckBoolAction(isOverFilterBag, true, "请等待滤布紧贴内转鼓后操作"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
                .Append(new SetPowerButtonAction(lowSpeedButton, false, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已停止转动", LogType.Log))
                .Append(new LoopRotateAction(filterBag.gameObject, false, RotationAxis.Z, 500))
                .Append(new InvokeCompletedAction(4, 1))
                .OnCompleted(() =>
                {
                    isFinished4_1 = true;
                    ProductionGuideManager.Instance.ShowCurrentGuide();
                }).Execute();

            Task.NewTask(stopButton.gameObject)
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
                .Append(new CheckBoolAction(isOverFilterBag, true, "请等待滤布紧贴内转鼓后操作"))
                .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
                .Append(new SetPowerButtonAction(lowSpeedButton, false, Color.green))
                .Append(new InvokeCloseAllGuideAction())
                .Append(new ShowLogInfoAction("已停止离心", LogType.Log))
                .Append(new LoopRotateAction(filterBag.gameObject, false, RotationAxis.Z, 500)) 
                .Append(new InvokeCompletedAction(1,2))               
                .OnCompleted(() =>
                {                    
                    ProductionGuideManager.Instance.ShowCurrentGuide();
                }).Execute();
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
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                        .Append(new CheckValveAction(arg, ValveState.ON))
                        .Append(new UpdateValveAction(arg, ValveState.OFF))
                        .Append(new ShowLogInfoAction("已关闭氮气进气阀",LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeCurrentFittingAction(2))
                        .Append(new InvokeCompletedAction(2,1))
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowCurrentGuide();
                        }).Execute();

                    //打开氮气进气阀VQ-S210-101
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 4, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
                        .Append(new CheckValveAction(arg, ValveState.OFF))
                        .Append(new UpdateValveAction(arg, ValveState.ON))
                        .Append(new ShowLogInfoAction("已打开氮气进气阀", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeCurrentFittingAction(1))
                        .Append(new ShowProgressAction("正在通入氮气...", 5))
                        .Append(new DelayedAction(5))
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        }).Execute();
                    break;
                #endregion
                #region 离心机进料阀（VQ-S210-102）
                case "VQ-S210-102":
                    //关闭结晶釜（R203A）罐底阀（GD-R203A-101）
                    Task.NewTask(arg)
                        .Append(new CheckValveAction(arg, ValveState.ON))
                        .Append(new CheckValveAction(ValveManager.Instance.GetValve("GD-R203A-101"), ValveState.OFF, "请关闭结晶釜罐底阀"))
                        .Append(new UpdateValveAction(arg, ValveState.OFF))
                        .Append(new GameObjectAction(suppliesParticle.gameObject, false))
                        .Append(new ShowLogInfoAction("已关闭结晶釜罐底阀", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeCurrentFittingAction(2))
                        .Append(new InvokeCompletedAction(3,2))
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowCurrentGuide();
                        }).Execute();

                    //打开离心机进料阀（VQ-S210-102）
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                        .Append(new CheckValveAction(arg, ValveState.OFF))
                        .Append(new CheckBoolAction(isStartCentrifuge, true, "请等待离心机完全启动"))
                        .Append(new UpdateValveAction(arg, ValveState.ON))
                        .Append(new ShowLogInfoAction("已打开离心机进料阀", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .OnCompleted(() =>
                        {
                            ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        }).Execute();
                    break;
                #endregion
                #region 结晶釜（R203A）罐底阀（GD-R203A-101）
                case "GD-R203A-101":
                    //关闭结晶釜（R203A）罐底阀（GD-R203A-101）
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                        .Append(new CheckValveAction(arg, ValveState.ON))
                        .Append(new CheckBoolAction(isFinishedFeed, true, "请等待投料完成"))
                        .Append(new UpdateValveAction(arg, ValveState.OFF))
                        .Append(new ShowLogInfoAction("已关闭结晶釜罐底阀", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .OnCompleted(() =>
                        {
                            isFinishedFeed = false;
                            ProductionGuideManager.Instance.ShowCurrentGuide(5);
                        }).Execute();

                    //打开结晶釜（R203A）罐底阀（GD-R203A-101）
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
                        .Append(new CheckBoolAction(isStartCentrifuge, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
                        .Append(new CheckValveAction(ValveManager.Instance.GetValve("VQ-S210-102"),ValveState.ON,"请先打开离心机进料阀"))
                        .Append(new CheckValveAction(arg, ValveState.OFF))
                        .Append(new UpdateValveAction(arg, ValveState.ON))
                        .Append(new GameObjectAction(suppliesParticle.gameObject,true))
                        .Append(new ShowLogInfoAction("已打开结晶釜罐底阀", LogType.Log))
                        .Append(new InvokeCurrentFittingAction(1))
                        .Append(new InvokeCloseAllGuideAction())
                        .OnCompleted(() =>
                        {
                            isStartCentrifuge = false;
                            glassWindow.Disable = false;
                            ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        }).Execute();
                    break;
                #endregion
                #region 纯化水阀门（VG-PW-107）
                case "VG-PW-107":
                    //关闭纯化水阀门（VG-PW-107）
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 2, true))
                        .Append(new CheckBoolAction(isCanClosePW_107, true))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 5, 1, false))
                        .Append(new CheckValveAction(arg, ValveState.ON))
                        .Append(new UpdateValveAction(arg, ValveState.OFF))
                        .Append(new ShowLogInfoAction("已关闭纯化水阀门", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeCurrentFittingAction(2))
                        .Append(new ShowProgressAction("正在离心...", 10))
                        .Append(new DelayedAction(10))                        
                        .OnCompleted(() =>
                        {
                            ShowHUDText(arg.transform, "离心已达到工艺要求", Color.green);
                            Completed(5, 1);
                            ProductionGuideManager.Instance.ShowCurrentGuide();
                        }).Execute();

                    //打开纯化水阀门（VG-PW-107）
                    Task.NewTask(arg)
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 2, true))
                        .Append(new CheckBoolAction(isFinished4_2, true, "请等待离心机离心完成"))
                        .Append(new CheckBoolAction(isCanClosePW_107, false))
                        .Append(new CheckMonitorAction(customStage.OperateMonitor, 5, 1, false))
                        .Append(new CheckValveAction(arg, ValveState.OFF))
                        .Append(new UpdateValveAction(arg, ValveState.ON))
                        .Append(new ShowLogInfoAction("已打开纯化水阀门", LogType.Log))
                        .Append(new InvokeCloseAllGuideAction())
                        .Append(new InvokeCurrentFittingAction(1))
                        .Append(new ShowProgressAction("正在洗涤...",5))
                        .Append(new DelayedAction(5))
                        .OnCompleted(() =>
                        {
                            isCanClosePW_107 = true;
                            ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        }).Execute();
                    break;
                #endregion
                default:
                    break;
            }
        }
    }
}
