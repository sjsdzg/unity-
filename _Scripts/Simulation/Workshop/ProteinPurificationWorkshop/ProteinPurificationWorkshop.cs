using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Common;
using XFramework.Actions;
using UnityEngine;
using NPOI.XWPF.UserModel;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 蛋白纯化间
    /// </summary>
   public partial class ProteinPurificationWorkshop:CustomWorkshop
    {
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.ProteinPurificationWorkshop;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            EventDispatcher.RegisterEvent<int,string,string>("蛋白纯化参数设置完成", IOSetOnCompleted_callBack);
            EventDispatcher.RegisterEvent<string,int>("蛋白纯化阀门点击之后", ValveClickEnd_callBack);
        }

        protected override void InitStandard()
        {
            base.InitStandard();
            InitializeComponent();
            InitializeOperate();
        }
        protected override void ResetProduceInitialize(int index)
        {
            base.ResetProduceInitialize(index);          
            switch (index)
            {
                case 1:
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, hose001.gameObject))
                        .Append(new GameObjectAction(hose001, true))
                        .Append(new ChangeVirtualRealityAction(hose001.gameObject, true))
                        .Append(new GameObjectAction(hose001_BucketHat, true))
                        .Append(new GameObjectAction(siliconeHose001, false))
                        .Append(new DOLocalMoveAction(hose001_BucketHat, new Vector3(0, 0, 1.790851f), 0.5f,true))
                        .Append(new DOLocalRotaAction(valve001, new Vector3(0, 0, -90), 0.5f,true))
                        .Append(new GameObjectAction(sampleCollectBucket,false))
                        .Append(new GameObjectAction(sampleCollectBucket_PutArea, false))
                        .Append(new GameObjectAction(hose002, false))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_GJG_01.ToString(),UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_RG_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_YPSJT_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_XTYT_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_YYT_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_NLMB_01.ToString(), UpdateType.Add))
                        .OnCompleted(()=> {
                            EventDispatcher.ExecuteEvent<int>("重置2D阀门点击次数", 0);                           
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "A1进口阀",ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口泵A", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "出口阀1", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "关闭进口阀B2", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "关闭进口泵B", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "关闭出口阀1", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "关闭出口阀3", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<bool,int>("重置参数设置面板", false,0);
                        })
                        .Execute();
                    break;
                case 2:
                   // sampleCollectBucket.localPosition = new Vector3(-4.060406f, -0.2798906f, 0f);
                   // sampleCollectBucket.localEulerAngles = new Vector3(0,0,-90);
                    loadingBufferBucketFlu.Value = 0f;
                    measuringCylinderFlu.Value = 0f;
                    bucket04Flu.Value = 0f;
                    bucket05Flu.Value = 0f;
                    Task.NewTask()
                      .Append(new InvokeFlashAction(true, carbonateBufferBucket.gameObject))
                      .Append(new GameObjectAction(sampleCollectBucket, true))
                      .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(-4.060406f, -0.2798906f, 0f), 0.5f, true))
                      .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, -90f), 0.5f, true))
                      .Append(new ChangeVirtualRealityAction(hose001.gameObject, false))
                      .Append(new GameObjectAction(measuringCylinderFlu.gameObject,false))
                      .Append(new GameObjectAction(carbonateBuffer_Effect, false))
                      .Append(new DOLocalRotaAction(carbonateBufferBucket, new Vector3(0, 0, 0), 0.5f,true))
                      .Append(new DOLocalMoveAction(carbonateBufferBucket, new Vector3(0f, 0, -0.002052246f), 0.5f,true))
                      .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.01349268f, 0, 0.02594605f), 0.5f,true))
                      .Append(new GameObjectAction(measuringCylinder02, false))
                      .Append(new DOLocalMoveAction(sampleCollectBucketHat, new Vector3(0.0388f, -0.016f, 0.3626f), 0.5f,true))
                      .Append(new GameObjectAction(hose003, false))
                      .Append(new GameObjectAction(pump01Switch_OFF, true))
                      .Append(new GameObjectAction(pump01Switch_ON, false))
                      .Append(new UpdateTransparencyAction(bucket04.gameObject, false))
                      .Append(new UpdateTransparencyAction(bucket05.gameObject, false))
                      .Append(new GameObjectAction(hose004, false))
                      .Append(new GameObjectAction(hose005, false))
                      .Append(new GameObjectAction(hose006, false))
                      .Append(new GameObjectAction(pump02Switch_OFF, true))
                      .Append(new GameObjectAction(pump02Switch_ON, false))
                      .Append(new UpdateTransparencyAction(loadingBufferBucketSelf.gameObject, false))
                      .Append(new DOLocalRotaAction(valve005, new Vector3(0, -90, -30), 0.5f,true))
                      .Append(new DOLocalRotaAction(valve002, new Vector3(0, 0, 0), 0.5f,true))
                      .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, 0), 0.5f,true))
                      .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, 0), 0.5f,true))
                      .Append(new UpdateGoodsAction(GoodsType.DBCH_GJG_01.ToString(), UpdateType.Add))
                      .Append(new UpdateGoodsAction(GoodsType.DBCH_RG_01.ToString(), UpdateType.Add))
                      .Append(new UpdateGoodsAction(GoodsType.DBCH_XTYT_01.ToString(), UpdateType.Add))
                      .Append(new UpdateGoodsAction(GoodsType.DBCH_YYT_01.ToString(), UpdateType.Add))
                      .Append(new UpdateGoodsAction(GoodsType.DBCH_NLMB_01.ToString(), UpdateType.Add))
                      .OnCompleted(() => {
                          EventDispatcher.ExecuteEvent<int>("重置2D阀门点击次数", 20);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "A1进口阀", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口泵A", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "出口阀1", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口阀B2", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口泵B", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "出口阀3", ValveState.OFF);
                          EventDispatcher.ExecuteEvent<bool, int>("重置参数设置面板", false, 5);
                      })
                      .Execute();
                    break;
                case 3:
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, chromatographicColumn.gameObject))
                        .Append(new ChangeVirtualRealityAction(hose001.gameObject, false))
                        .Append(new GameObjectAction(sampleCollectBucket, true))
                        .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(-0.7406797f, -0.09607813f, 0), 0.5f, true))
                        .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, -90f), 0.5f, true))
                        .Append(new GameObjectAction(hose002, false))
                        .Append(new GameObjectAction(hose008, false))
                        .Append(new GameObjectAction(hose009, false))
                        .Append(new GameObjectAction(hose010, false))
                        .Append(new GameObjectAction(hose011, false))
                        .Append(new GameObjectAction(hose012, true))
                        .Append(new GameObjectAction(hose013, true))
                        .Append(new GameObjectAction(siliconeHose001, true))
                        .Append(new GameObjectAction(siliconeHose002, false))
                        .Append(new GameObjectAction(eluantBucket,false))
                        .Append(new GameObjectAction(sampleCollectBucket_PutArea, false))
                        .Append(new DOLocalMoveAction(eluantBucket, new Vector3(-4.060406f, -0.28f, 0), 0.5f, true))
                        .Append(new DOLocalRotaAction(eluantBucket, new Vector3(0, 0, -90), 0.5f, true))
                        .Append(new DOLocalRotaAction(valve005, new Vector3(0, -90, -30), 0.5f,true))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_GJG_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_RG_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_XTYT_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_YYT_01.ToString(), UpdateType.Add))
                        .Append(new UpdateGoodsAction(GoodsType.DBCH_NLMB_01.ToString(), UpdateType.Add))
                        .OnCompleted(() => {
                            EventDispatcher.ExecuteEvent<int>("重置2D阀门点击次数", 20);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "A1进口阀", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口泵A", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "出口阀1", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口阀B2", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口泵B", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "出口阀3", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口阀A2", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<string, ValveState>("重置2D阀门状态", "进口阀A4", ValveState.OFF);
                            EventDispatcher.ExecuteEvent<bool, int>("重置参数设置面板", false, 5);
                        })
                        .Execute();
                    break;
                case 4:
                    collectTankFlu.Value = 0f;
                    Task.NewTask()
                       .Append(new InvokeFlashAction(true, membraneBag.gameObject))
                       .Append(new ChangeVirtualRealityAction(hose001.gameObject, false))
                       .Append(new GameObjectAction(eluantBucket, true))
                       .Append(new DOLocalMoveAction(eluantBucket, new Vector3(4.060403f, 0.2798906f, 0f), 0.5f, true))
                       .Append(new DOLocalRotaAction(eluantBucket, new Vector3(0f, 0f, -90f), 0.5f, true))
                       .Append(new ChangeVirtualRealityAction(membraneBag.gameObject, false))
                       .Append(new GameObjectAction(hose004, false))
                       .Append(new GameObjectAction(hose006, false))
                       .Append(new GameObjectAction(hose007, false))
                       .Append(new ChangeVirtualRealityAction(stosteBucket.gameObject, false))
                       .Append(new GameObjectAction(stosteBucket, false))
                       .Append(new GameObjectAction(pump02Switch_OFF, true))
                       .Append(new GameObjectAction(pump02Switch_ON, false))
                       .Append(new UpdateTransparencyAction(collectTank.gameObject, false))
                       .Append(new DOLocalRotaAction(valve006, new Vector3(0, -90, -30), 0.5f,true))
                       .Append(new DOLocalRotaAction(valve002, new Vector3(0, 0, 0), 0.5f,true))
                       .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, 0), 0.5f,true))
                       .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, 0), 0.5f,true))
                       .Append(new UpdateGoodsAction(GoodsType.DBCH_GJG_01.ToString(), UpdateType.Add))
                       .Append(new UpdateGoodsAction(GoodsType.DBCH_RG_01.ToString(), UpdateType.Add))
                       .Append(new UpdateGoodsAction(GoodsType.DBCH_YYT_01.ToString(), UpdateType.Add))
                       .Append(new UpdateGoodsAction(GoodsType.DBCH_NLMB_01.ToString(), UpdateType.Add))
                       .OnCompleted(() => {
                       })
                       .Execute();
                    break;
                default:
                    break;
            }
        }
        private void ValveClickEnd_callBack(string valveName,int clickSum)
        {

            switch (valveName)
            {
                case "出口阀1":                   
                    if (clickSum==1)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 6) //1-1-8
                    {
                        Task.NewTask()
                            .Append(new InvokeCompletedAction(1,1))
                            .Append(new InvokeCurrentGuideAction(1))
                            .Append(new GameObjectAction(siliconeHose001))
                            .Append(new ChangeVirtualRealityAction(siliconeHose001.gameObject,true))
                            .Append(new InvokeFlashAction(true, siliconeHose001.gameObject))
                            .Execute();
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                    }
                    else if (clickSum == 7) //1-2-6
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(6);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀B2", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 12) //1-2-11
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                        Task.NewTask()
                            .Append(new UpdateSmallAction("1-2-10",true))
                            .Append(new InvokeCurrentGuideAction(11))
                            .Append(new InvokeFlashAction(true, valve001.gameObject))
                            .Execute();
                    }
                    else if (clickSum == 13) //1-3-2
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(2);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 16) //1-3-8
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(8);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀3", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                    }
                    else if (clickSum == 21) //3-1-3
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 26) //3-1-9
                    {
                        Completed(3, 1);
                        ProductionGuideManager.Instance.ShowCurrentGuide(1);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                    }
                    else if (clickSum == 27) //3-2-7
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀B2", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 32) 
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                        Task.NewTask()
                           .Append(new UpdateSmallAction("3-2-11", true))
                           .Append(new InvokeCurrentGuideAction(12))
                           .Append(new InvokeFlashAction(true, valve005.gameObject))
                           .Execute();
                    }
                    else if (clickSum == 33) //3-3-6
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(6);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀A2", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀1", LogType.Log);
                    }
                    else if (clickSum == 37) //3-3-10
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(10);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀A4", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀1", LogType.Log);
                    }
                    break;
                case "A1进口阀":
                    if (clickSum == 2)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(4);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开A1进口阀", LogType.Log);
                    }
                    else if (clickSum == 4) //1-1-6
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭A1进口阀", LogType.Log);
                    }
                    else if (clickSum == 14) //1-3-3
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(3);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开A1进口阀", LogType.Log);
                    }
                    else if (clickSum == 18) //1-3-11
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(11);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭A1进口阀", LogType.Log);
                    }
                    else if (clickSum == 22) //3-1-4
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(4);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开A1进口阀", LogType.Log);
                    }
                    else if (clickSum == 24) //3-1-7
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭A1进口阀", LogType.Log);
                    }
                    break;
                case "进口泵A":
                    if (clickSum == 3)
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(5);
                        EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "180");
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵A", LogType.Log);
                    }
                    else if (clickSum == 5) //1-1-7
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(8);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵A", LogType.Log);
                    }
                    else if (clickSum == 15) //1-3-4
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(4);
                        EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "180");
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵A", LogType.Log);
                    }
                    else if (clickSum == 19) //1-3-12
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(12);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀3", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵A", LogType.Log);
                    }
                    else if (clickSum == 23) //3-1-5
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(5);
                        EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "180");
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵A", LogType.Log);
                    }
                    else if (clickSum == 25) //3-1-8
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(8);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵A", LogType.Log);
                    }
                    else if (clickSum == 35) //3-3-8
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵A", LogType.Log);
                        Task.NewTask()
                            .Append(new ShowProgressAction("洗脱中...", 3,true))
                            .Append(new InvokeCurrentGuideAction(8))
                            .OnCompleted(() =>
                            {
                                EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀A2", false);
                            })
                            .Execute();
                    }
                    else if (clickSum == 41) //3-3-14
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵A", LogType.Log);
                        ProductionGuideManager.Instance.ShowCurrentGuide(14);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀3", false);
                    }
                    break;
                case "进口阀B2":
                    if (clickSum == 8) //1-2-7
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵B", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口阀B2", LogType.Log);
                    }
                   else if (clickSum == 10) //1-2-9
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(9);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵B", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口阀B2", LogType.Log);
                    }
                    else if (clickSum == 28) //3-2-8
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(8);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵B", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口阀B2", LogType.Log);
                    }
                    else if (clickSum == 30) //3-2-10
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(10);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵B", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口阀B2", LogType.Log);
                    }
                    break;
                case "进口泵B":
                    if (clickSum == 9) //1-2-8
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵B", LogType.Log);
                        Task.NewTask()
                      .Append(new ShowProgressAction("上样中...", 3,true))
                      .Append(new InvokeCurrentGuideAction(8))
                      .OnCompleted(() =>
                      {
                          EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀B2", false);
                      })
                      .Execute();
                    }
                    else if (clickSum == 11) //1-2-10
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵B", LogType.Log);
                        ProductionGuideManager.Instance.ShowCurrentGuide(10);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
                    }
                    else if (clickSum == 29) //3-2-9
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口泵B", LogType.Log);
                        Task.NewTask()
                           .Append(new ShowProgressAction("吸附中...", 3,true))
                           .Append(new InvokeCurrentGuideAction(9))
                           .OnCompleted(() =>
                           {
                               EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀B2", false);
                           })
                           .Execute();
                    }
                    else if (clickSum == 31) //3-2-11
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(11);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口泵B", LogType.Log);
                    }
                    break;
                case "出口阀3":
                    if (clickSum == 17) //1-3-9
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(9);
                        EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "130");
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀3", LogType.Log);
                    }
                    else if (clickSum == 20) //1-3-13
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀3", LogType.Log);
                        Task.NewTask()
                             .Append(new UpdateSmallAction("1-3-12", true))
                             //.Append(new InvokeCompletedAction(1, 3))
                             .Append(new InvokeCurrentGuideAction(13))
                             .Append(new InvokeFlashAction(true, hose002.gameObject))
                             .Execute();
                    }
                    else if (clickSum == 39) //3-3-12
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开出口阀3", LogType.Log);
                        Task.NewTask()
                             .Append(new ShowProgressAction("洗脱中...", 3,true))
                             .Append(new InvokeCurrentGuideAction(12))
                             .OnCompleted(() =>
                             {
                                 EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口阀A4", false);
                             })
                             .Execute();
                    }
                    else if (clickSum == 42) //1-3-13
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭出口阀3", LogType.Log);
                        Task.NewTask()
                             .Append(new UpdateSmallAction("3-3-14", true))
                             .Append(new InvokeCurrentGuideAction(15))
                             .Append(new InvokeFlashAction(true, hose002.gameObject))
                             .Execute();
                    }
                    break;
                case "进口阀A2":
                    if (clickSum == 34) //3-3-7
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(7);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", true);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口阀A2", LogType.Log);
                    }
                    else if (clickSum == 36) //3-3-9
                    {
                        ProductionGuideManager.Instance.ShowCurrentGuide(9);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口阀A2", LogType.Log);
                    }
                    break;
                case "进口阀A4":
                    if (clickSum == 38) //3-3-11
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已打开进口阀A4", LogType.Log);
                        ProductionGuideManager.Instance.ShowCurrentGuide(11);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀3", true);
                    }
                    else if (clickSum == 40) //3-3-13
                    {
                        EventDispatcher.ExecuteEvent(Events.LogInfo.Show, "已关闭进口阀A4", LogType.Log);
                        ProductionGuideManager.Instance.ShowCurrentGuide(13);
                        EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "进口泵A", false);
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            EventDispatcher.UnregisterEvent<int,string,string>("蛋白纯化参数设置完成", IOSetOnCompleted_callBack);
            EventDispatcher.UnregisterEvent<string, int>("蛋白纯化阀门点击之后", ValveClickEnd_callBack);
        }
        private int misoperationHandlerCount = 0;
        private void IOSetOnCompleted_callBack(int obj,string inputDate,string result)
        {
            if (!inputDate.Equals(result))
            {
                MisoperationHandler("");
            }
            switch (obj)
            {
                case 1:
                    Task.NewTask()
                        .Append(new ShowProgressAction("平衡中...", 3,true))
                        .Append(new InvokeCurrentGuideAction(6))
                        .OnCompleted(() =>
                        {
                            EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", false);
                        })
                        .Execute();

                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate+"L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速180L/h", Color.green))
                            .Execute();
                    }
                    break;
                case 2:
                    ProductionGuideManager.Instance.ShowCurrentGuide(4);
                    EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "130");
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置上样量" + inputDate + "L", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置上样量160L", Color.green))
                            .Execute();

                    }
                    break;
                case 3:
                    ProductionGuideManager.Instance.ShowCurrentGuide(5);
                    EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);  //1-2-5
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate + "L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速130L/h", Color.green))
                            .Execute();

                    }
                    break;
                case 4:
                    Task.NewTask()
                            .Append(new UpdateSmallAction("1-3-4", true))
                            .Append(new InvokeCurrentGuideAction(5))
                            .Append(new GameObjectAction(sampleCollectBucket_PutArea))
                            .Append(new InvokeFlashAction(true, sampleCollectBucket_PutArea.gameObject))
                            .Execute();
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate + "L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速180L/h", Color.green))
                            .Execute();

                    }
                    break;
                case 5:
             Task.NewTask()
                 .Append(new ShowProgressAction("洗脱中...", 3,true))
                 .Append(new InvokeCurrentGuideAction(10))
                 .OnCompleted(() =>
                 {
                     EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", false);  //1-3-10
                 })
                 .Execute();
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate + "L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速130L/h", Color.green))
                            .Execute();

                    }
                    break;
                case 6:
             Task.NewTask()
                .Append(new ShowProgressAction("平衡中...", 3,true))
                .Append(new InvokeCurrentGuideAction(6))
                .OnCompleted(() =>
                {
                    EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "A1进口阀", false);  //3-1-6
                })
                .Execute();
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate + "L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速180L/h", Color.green))
                            .Execute();

                    }
                    break;
                case 7:
                    ProductionGuideManager.Instance.ShowCurrentGuide(5);
                    EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "流速L/h：", "150");  //3-2-5
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置上样量" + inputDate + "L", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置上样量150L", Color.green))
                            .Execute();

                    }
                    break;
                case 8:
                    ProductionGuideManager.Instance.ShowCurrentGuide(6);
                    EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);  //3-2-6
                    if (!inputDate.Equals(result))
                    {
                        Task.NewTask()
                           .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速" + inputDate + "L/h", Color.red))
                           .Execute();
                    }
                    else
                    {
                        Task.NewTask()
                            .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "设置流速150L/h", Color.green))
                            .Execute();

                    }
                    break;
                default:
                    break;
            }
        }
    }
}
