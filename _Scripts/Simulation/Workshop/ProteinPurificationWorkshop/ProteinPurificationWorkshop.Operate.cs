using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Common;
using XFramework.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 蛋白纯化间
    /// </summary>
   public partial class ProteinPurificationWorkshop
    {

        private void InitializeOperate()
        {
            hose001.TriggerAction(EventTriggerType.Drop, Hose001_Drop);
            siliconeHose001.TriggerAction(EventTriggerType.Drop, SiliconeHose001_Drop);
            siliconeHose001.TriggerAction(EventTriggerType.PointerClick, SiliconeHose001_PointerClick);
            valve001.TriggerAction(EventTriggerType.PointerClick, Valve001_PointerClick);
            sampleCollectBucket_PutArea.TriggerAction(EventTriggerType.Drop, sampleCollectBucket_PutArea_Drop);
            hose002.TriggerAction(EventTriggerType.Drop, Hose002_Drop);
            hose002.TriggerAction(EventTriggerType.PointerClick, Hose002_PointerClick);
            carbonateBufferBucket.TriggerAction(EventTriggerType.PointerClick, CarbonateBufferBucket_PointerClick);
            measuringCylinder.TriggerAction(EventTriggerType.PointerClick, MeasuringCylinder_PointerClick);
            sampleCollectBucket.TriggerAction(EventTriggerType.Drop, SampleCollectBucket_Drop);
            sampleCollectBucket.TriggerAction(EventTriggerType.PointerClick, SampleCollectBucket_PointerClick);
            hose003.TriggerAction(EventTriggerType.Drop, Hose003_Drop);
            hose003.TriggerAction(EventTriggerType.PointerClick, Hose003_PointerClick);
            hose004.TriggerAction(EventTriggerType.PointerClick, Hose004_PointerClick);
            hose005.TriggerAction(EventTriggerType.PointerClick, Hose005_PointerClick);
            hose006.TriggerAction(EventTriggerType.PointerClick, Hose006_PointerClick);
            hose007.TriggerAction(EventTriggerType.PointerClick, Hose007_PointerClick);
            pump01.TriggerAction(EventTriggerType.PointerClick, Pump01_PointerClick);
            sampleCollectBucketMotor.TriggerAction(EventTriggerType.PointerClick, SampleCollectBucketMotor_PointerClick);
            hose004.TriggerAction(EventTriggerType.Drop, Hose004_Drop);
            hose005.TriggerAction(EventTriggerType.Drop, Hose005_Drop);
            hose006.TriggerAction(EventTriggerType.Drop, Hose006_Drop);
            hose007.TriggerAction(EventTriggerType.Drop, Hose007_Drop);
            valve002.TriggerAction(EventTriggerType.PointerClick, Valve002_PointerClick);
            valve003.TriggerAction(EventTriggerType.PointerClick, Valve003_PointerClick);
            valve004.TriggerAction(EventTriggerType.PointerClick, Valve004_PointerClick);
            valve005.TriggerAction(EventTriggerType.PointerClick, Valve005_PointerClick);
            pump02.TriggerAction(EventTriggerType.PointerClick, Pump02_PointerClick);
            chromatographicColumn.TriggerAction(EventTriggerType.PointerClick, ChromatographicColumn_PointerClick);
            siliconeHose002.TriggerAction(EventTriggerType.Drop, SiliconeHose002_Drop);
           // loadingBufferValve.TriggerAction(EventTriggerType.PointerClick, LoadingBufferValve_PointerClick);
            hose010.TriggerAction(EventTriggerType.Drop, Hose010_Drop);
            hose011.TriggerAction(EventTriggerType.Drop, Hose011_Drop);
            eluantBucket.TriggerAction(EventTriggerType.PointerClick, EluantBucket_PointerClick);
            membraneBag.TriggerAction(EventTriggerType.PointerClick, MembraneBag_PointerClick);
            membraneBag.TriggerAction(EventTriggerType.Drop, MembraneBag_Drop);
            stosteBucket.TriggerAction(EventTriggerType.Drop, StosteBucket_Drop);
            valve006.TriggerAction(EventTriggerType.PointerClick, Valve006_PointerClick);
            plcScreen.TriggerAction(EventTriggerType.PointerClick, PlcScreen_PointerClick);
            this.Invoke(0.2f, () =>
            {
                Task.NewTask()
                    .Append(new ChangeVirtualRealityAction(hose001.gameObject, true))
                    .Append(new InvokeFlashAction(true, hose001.gameObject))
                    .Append(new InvokeCurrentGuideAction(1))
                    .OnCompleted(() =>
                    {
                    })
                    .Execute();
            });
        }
        /// <summary>
        /// 拉近PLC屏幕
        /// </summary>
        /// <param name="arg0"></param>
        private void PlcScreen_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            Task.NewTask(eventData.pointerEnter)
              .Append(new InvokeCameraLookAndMovePointAction("观察PLC界面",0.5f))
               .Execute();
        }

        /// <summary>
        ///   4-1-17 拆除所有软管
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose007_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  4-1-17    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-16", true))
               .Append(new CheckSmallAction("4-1-17", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(hose007, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new DelayedAction(3))
               .Append(new InvokeCompletedAction(4, 1))
               .Execute();
        }
        /// <summary>
        /// 4-1-10    打开洗脱液桶出液阀    4-1-13    关闭洗脱液桶出液阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve006_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 4-1-13    关闭洗脱液桶出液阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-12", true))
               .Append(new CheckSmallAction("4-1-13", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve006.gameObject))
               .Append(new DOLocalRotaAction(valve006, new Vector3(0, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭出液阀", Color.green))
               .Append(new UpdateSmallAction("4-1-13", true))
               .Append(new InvokeCurrentGuideAction(14))
               .Append(new InvokeFlashAction(true, valve002.gameObject))
               .Execute();
            // 4-1-10    打开洗脱液桶出液阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-9", true))
               .Append(new CheckSmallAction("4-1-10", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve006.gameObject))
               .Append(new DOLocalRotaAction(valve006, new Vector3(-90, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开出液阀", Color.green))
               .Append(new UpdateSmallAction("4-1-10", true))
               .Append(new InvokeCurrentGuideAction(11))
               .Append(new InvokeFlashAction(true, pump02.gameObject))
               .Execute();
        }

        /// <summary>
        ///  4-1-6   使用软管连接至原液桶   
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose007_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 4-1-6  使用软管连接至原液桶   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-5", true))
               .Append(new CheckSmallAction("4-1-6", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose007.gameObject))
               .Append(new ChangeVirtualRealityAction(hose007.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接至原液桶", Color.green))
               .Append(new UpdateSmallAction("4-1-6", true))
               .Append(new InvokeCurrentGuideAction(7))
               .Append(new InvokeFlashAction(true, valve002.gameObject))
               .Execute();
        }
        /// <summary>
        /// 4-1-5     放置原液桶
        /// </summary>
        /// <param name="arg0"></param>
        private void StosteBucket_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //   4-1-5     放置原液桶
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-4", true))
               .Append(new CheckSmallAction("4-1-5", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_YYT_01))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_YYT_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, stosteBucket.gameObject))
               .Append(new ChangeVirtualRealityAction(stosteBucket.gameObject,false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置原液桶", Color.green))
               .Append(new UpdateSmallAction("4-1-5", true))
               .Append(new InvokeCurrentGuideAction(6))
               .Append(new GameObjectAction(hose007))
               .Append(new ChangeVirtualRealityAction(hose007.gameObject, true))
               .Append(new InvokeFlashAction(true, hose007.gameObject))
               .Execute();
        }

        /// <summary>
        /// 4-1-2   更换为纳滤膜包
        /// </summary>
        /// <param name="arg0"></param>
        private void MembraneBag_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  4-1-2   更换为纳滤膜包
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-1", true))
               .Append(new CheckSmallAction("4-1-2", false))
               .Append(new CheckGoodsAction(eventData,GoodsType.DBCH_NLMB_01))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_NLMB_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, membraneBag.gameObject))
               .Append(new ChangeVirtualRealityAction(membraneBag.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已更换为纳滤膜包", Color.green))
               .Append(new UpdateSmallAction("4-1-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new GameObjectAction(hose004))
               .Append(new ChangeVirtualRealityAction(hose004.gameObject, true))
               .Append(new InvokeFlashAction(true, hose004.gameObject))
               .Execute();
        }

        /// <summary>
        /// 4-1-1   拆除膜包
        /// </summary>
        /// <param name="arg0"></param>
        private void MembraneBag_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  4-1-1   拆除膜包
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, membraneBag.gameObject))
               .Append(new ChangeVirtualRealityAction(membraneBag.gameObject,true))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除膜包", Color.green))
               .Append(new UpdateSmallAction("4-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, membraneBag.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-3-16     移至超滤装置旁
        /// </summary>
        /// <param name="arg0"></param>
        private void EluantBucket_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-3-16    移至超滤装置旁
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-15", true))
               .Append(new CheckSmallAction("3-3-16", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, eluantBucket.gameObject))
               .Append(new DOLocalMoveAction(eluantBucket, new Vector3(-4.060406f, -1.531f, 0f), 1f, true))
               .Append(new DOLocalRotaAction(eluantBucket, new Vector3(0f, 0f, -180f), 1f, false))
               .Append(new DOLocalMoveAction(eluantBucket, new Vector3(3.832f, -1.531f, 0f), 2f))
               .Append(new DOLocalMoveAction(eluantBucket, new Vector3(4.060403f, 0.2798906f, 0f), 1f, true))
               .Append(new DOLocalRotaAction(eluantBucket, new Vector3(0f, 0f, -90f), 1f, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移至超滤装置旁", Color.green))
               .Append(new UpdateSmallAction("3-3-16", true))
               .Append(new InvokeCompletedAction(3,3))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, membraneBag.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-3-2    使用软管连接
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose011_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-3-2    使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-1", true))
               .Append(new CheckSmallAction("3-3-2", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose011.gameObject))
               .Append(new DOLocalMoveAction(bucketHat_3_3_2, new Vector3(0.2243008f, -0.214f, 0.567f), 0.5f))
               .Append(new GameObjectAction(bucketHat_3_3_2, false))
               .Append(new ChangeVirtualRealityAction(hose011.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("3-3-2", true))
               .Append(new GameObjectAction(sampleCollectBucket_PutArea))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, sampleCollectBucket_PutArea.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-3-1    使用软管连接
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose010_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-3-1    使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeFlashAction(false, hose010.gameObject))
               .Append(new DOLocalMoveAction(bucketHat_3_3_1,new Vector3 (-0.2243008f, -0.214f, 0.567f),0.5f))
               .Append(new GameObjectAction(bucketHat_3_3_1, false))
               .Append(new ChangeVirtualRealityAction(hose010.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("3-3-1", true))
               .Append(new GameObjectAction(hose011))
               .Append(new ChangeVirtualRealityAction(hose011.gameObject, true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, hose011.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-2-2   安装硅胶管
        /// </summary>
        /// <param name="arg0"></param>
        private void SiliconeHose002_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-2-2   安装硅胶管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
               .Append(new CheckSmallAction("3-2-1", true))
               .Append(new CheckSmallAction("3-2-2", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_GJG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, siliconeHose002.gameObject))
               .Append(new ChangeVirtualRealityAction(siliconeHose002.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用硅胶管连接", Color.green))
               .Append(new UpdateSmallAction("3-2-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, valve005.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-2-1    拆除硅胶管
        /// </summary>
        /// <param name="arg0"></param>
        private void SiliconeHose001_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 3-2-1 拆除硅胶管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
               .Append(new CheckSmallAction("3-2-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, siliconeHose001.gameObject))
               .Append(new GameObjectAction(siliconeHose001, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除硅胶管", Color.green))
               .Append(new GameObjectAction(siliconeHose002, true))
               .Append(new ChangeVirtualRealityAction(siliconeHose002.gameObject,true))
               .Append(new UpdateSmallAction("3-2-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, siliconeHose002.gameObject))
               .Execute();
        }

        /// <summary>
        /// 3-1-1   更换为离子交换层析柱
        /// </summary>
        /// <param name="arg0"></param>
        private void ChromatographicColumn_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //3-1-1   更换为离子交换层析柱
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, false))
               .Append(new CheckSmallAction("3-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, chromatographicColumn.gameObject))
               .Append(new GameObjectAction(hose008, true))
               .Append(new GameObjectAction(hose009, true))
               .Append(new GameObjectAction(hose012,false))
               .Append(new GameObjectAction(hose013, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已更换为离子交换层析柱", Color.green))
               .Append(new UpdateSmallAction("3-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);
               })
               .Execute();
        }
        #region   2-2-19   4-1-17  拆除所有软管
        /// <summary>
        /// 2-2-19     4-1-17  拆除所有软管
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose006_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  2-2-19    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-18", true))
               .Append(new CheckSmallAction("2-2-19", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose006.gameObject))
               .Append(new GameObjectAction(hose003, false))
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose005, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(loadingBufferBucket, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new UpdateSmallAction("2-2-19", true))
               .Append(new InvokeCurrentGuideAction(20))
               .Append(new InvokeFlashAction(true, sampleCollectBucket.gameObject))
               .Execute();
            //  4-1-17    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-16", true))
               .Append(new CheckSmallAction("4-1-17", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(hose007, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new DelayedAction(3))
               .Append(new InvokeCompletedAction(4, 1))
               .Execute();
        }
        /// <summary>
        /// 2-2-19    拆除所有软管
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose005_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  2-2-19    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-18", true))
               .Append(new CheckSmallAction("2-2-19", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose003, false))
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose005, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(loadingBufferBucket, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new UpdateSmallAction("2-2-19", true))
               .Append(new InvokeCurrentGuideAction(20))
               .Execute();
        }
        /// <summary>
        /// 2-2-19      4-1-17 拆除所有软管
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose004_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  2-2-19    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-18", true))
               .Append(new CheckSmallAction("2-2-19", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose003, false))
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose005, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(loadingBufferBucket, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new UpdateSmallAction("2-2-19", true))
               .Append(new InvokeCurrentGuideAction(20))
               .Execute();
            //  4-1-17    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-16", true))
               .Append(new CheckSmallAction("4-1-17", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(hose007, false))          
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new DelayedAction(3))
               .Append(new InvokeCompletedAction(4,1))
               .Execute();
        }
        /// <summary>
        /// 2-2-19    拆除所有软管
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose003_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //  2-2-19    拆除所有软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-18", true))
               .Append(new CheckSmallAction("2-2-19", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new GameObjectAction(hose003,false))
               .Append(new GameObjectAction(hose004, false))
               .Append(new GameObjectAction(hose005, false))
               .Append(new GameObjectAction(hose006, false))
               .Append(new GameObjectAction(loadingBufferBucket, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除所有软管", Color.green))
               .Append(new UpdateSmallAction("2-2-19", true))
               .Append(new InvokeCurrentGuideAction(20))
               .Execute();
        }
        #endregion
        /// <summary>
        /// 2-2-12    4-1-11  点击启动蠕动泵      2-2-14    4-1-12 点击关闭蠕动泵
        /// </summary>
        /// <param name="arg0"></param>
        private void Pump02_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //4-1-12    点击关闭蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-11", true))
               .Append(new CheckSmallAction("4-1-12", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump02.gameObject))
               .Append(new GameObjectAction(pump02Switch_OFF, true))
               .Append(new GameObjectAction(pump02Switch_ON, false))
               .Append(new UpdateTransparencyAction(collectTank.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new UpdateSmallAction("4-1-12", true))
               .Append(new InvokeCurrentGuideAction(13))
               .Append(new InvokeFlashAction(true, valve006.gameObject))
               .Execute();
            //2-2-14    点击关闭蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-13", true))
               .Append(new CheckSmallAction("2-2-14", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump02.gameObject))
               .Append(new GameObjectAction(pump02Switch_OFF, true))
               .Append(new GameObjectAction(pump02Switch_ON, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new UpdateSmallAction("2-2-14", true))
               .Append(new UpdateTransparencyAction(loadingBufferBucketSelf.gameObject, false))
               .Append(new InvokeCurrentGuideAction(15))
               .Append(new InvokeFlashAction(true, valve005.gameObject))
               .Execute();
            //2-2-12    点击启动蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-11", true))
               .Append(new CheckSmallAction("2-2-12", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump02.gameObject))
               .Append(new GameObjectAction(pump02Switch_OFF, false))
               .Append(new GameObjectAction(pump02Switch_ON, true))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
               .Append(new UpdateTransparencyAction(loadingBufferBucketSelf.gameObject,true))
               .Append(new CoroutineFluidLevelAction(loadingBufferBucketFlu.gameObject,0.7f,3))
               .Append(new UpdateSmallAction("2-2-12", true))
               .Append(new InvokeCurrentGuideAction(13))
               .Append(new InvokeFlashAction(true, pump01.gameObject))
               .Execute();
            //4-1-11    点击启动蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-10", true))
               .Append(new CheckSmallAction("4-1-11", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump02.gameObject))
               .Append(new GameObjectAction(pump02Switch_OFF, false))
               .Append(new GameObjectAction(pump02Switch_ON, true))
               .Append(new UpdateTransparencyAction(collectTank.gameObject,true))
               .Append(new CoroutineFluidLevelAction(collectTankFlu.gameObject,1,3,true))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
               .Append(new ShowProgressAction("纳滤中...",3,true))
               .Append(new UpdateSmallAction("4-1-11", true))
               .Append(new InvokeCurrentGuideAction(12))
               .Append(new InvokeFlashAction(true, pump02.gameObject))
               .Execute();
        }

        /// <summary>
        /// 2-2-11   打开出液阀         3-2-3   打开样品收集桶出口阀       2-2-15  关闭出液阀   3-2-12   关闭样品收集桶出口阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve005_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;

            // 3-2-3   打开样品收集桶出口阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
               .Append(new CheckSmallAction("3-2-2", true))
               .Append(new CheckSmallAction("3-2-3", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve005.gameObject))
               .Append(new DOLocalRotaAction(valve005, new Vector3(-90, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开样品收集桶出口阀", Color.green))
               .Append(new UpdateSmallAction("3-2-3", true))
               .Append(new InvokeCurrentGuideAction(4))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "上样量L：", "150");
               })
               .Execute();
            // 3-2-12   关闭样品收集桶出口阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, false))
               .Append(new CheckSmallAction("3-2-11", true))
               .Append(new CheckSmallAction("3-2-12", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve005.gameObject))
               .Append(new DOLocalRotaAction(valve005, new Vector3(0, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭样品收集桶出口阀", Color.green))
               .Append(new UpdateSmallAction("3-2-12", true))
               .Append(new GameObjectAction(hose010))
               .Append(new ChangeVirtualRealityAction(hose010.gameObject, true))
               .Append(new InvokeCompletedAction(3, 2))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, hose010.gameObject))
               .Execute();
            // 2-2-11   打开出液阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-10", true))
               .Append(new CheckSmallAction("2-2-11", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve005.gameObject))
               .Append(new DOLocalRotaAction(valve005, new Vector3(-90, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开出液阀", Color.green))
               .Append(new UpdateSmallAction("2-2-11", true))
               .Append(new InvokeCurrentGuideAction(12))
               .Append(new InvokeFlashAction(true, pump02.gameObject))
               .Execute();
            // 2-2-15   关闭出液阀
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-14", true))
               .Append(new CheckSmallAction("2-2-15", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve005.gameObject))
               .Append(new DOLocalRotaAction(valve005, new Vector3(0, -90, -30), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭出液阀", Color.green))
               .Append(new UpdateSmallAction("2-2-15", true))
               .Append(new InvokeCurrentGuideAction(16))
               .Append(new InvokeFlashAction(true, valve002.gameObject))
               .Execute();
        }
        /// <summary>
        /// 2-2-10   4-1-9  打开透出液出口阀    2-2-18   4-1-16  关闭透出液出口阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve004_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 4-1-9   打开透出液出口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-8", true))
               .Append(new CheckSmallAction("4-1-9", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose001.gameObject))
               .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开透出液出口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-9", true))
               .Append(new InvokeCurrentGuideAction(10))
               .Append(new InvokeFlashAction(true, valve006.gameObject))
               .Execute();
            // 2-2-10   打开透出液出口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-9", true))
               .Append(new CheckSmallAction("2-2-10", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve004.gameObject))
               .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开透出液出口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-10", true))
               .Append(new InvokeCurrentGuideAction(11))
               .Append(new InvokeFlashAction(true, valve005.gameObject))
               .Execute();
            // 2-2-18   关闭透出液出口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-17", true))
               .Append(new CheckSmallAction("2-2-18", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve004.gameObject))
               .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭透出液出口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-18", true))
               .Append(new InvokeCurrentGuideAction(19))
               .Append(new InvokeFlashAction(true, hose006.gameObject))
               .Execute();
            // 4-1-16   关闭透出液出口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-15", true))
               .Append(new CheckSmallAction("4-1-16", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve004.gameObject))
               .Append(new DOLocalRotaAction(valve004, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭透出液出口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-16", true))
               .Append(new InvokeCurrentGuideAction(17))
               .Append(new InvokeFlashAction(true, hose006.gameObject))
               .Execute();
        }
        /// <summary>
        /// 2-2-9    4-1-8 打开回流口阀       2-2-17   4-1-15   关闭回流口阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve003_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 4-1-8   打开回流口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-7", true))
               .Append(new CheckSmallAction("4-1-8", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve003.gameObject))
               .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开回流口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-8", true))
               .Append(new InvokeCurrentGuideAction(9))
               .Append(new InvokeFlashAction(true, valve004.gameObject))
               .Execute();
            // 2-2-9   打开回流口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-8", true))
               .Append(new CheckSmallAction("2-2-9", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve003.gameObject))
               .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开回流口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-9", true))
               .Append(new InvokeCurrentGuideAction(10))
               .Append(new InvokeFlashAction(true, valve004.gameObject))
               .Execute();
            // 2-2-17  关闭回流口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-16", true))
               .Append(new CheckSmallAction("2-2-17", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve003.gameObject))
               .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭回流口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-17", true))
               .Append(new InvokeCurrentGuideAction(18))
               .Append(new InvokeFlashAction(true, valve004.gameObject))
               .Execute();
            // 4-1-15   关闭回流口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-14", true))
               .Append(new CheckSmallAction("4-1-15", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve003.gameObject))
               .Append(new DOLocalRotaAction(valve003, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭回流口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-15", true))
               .Append(new InvokeCurrentGuideAction(16))
               .Append(new InvokeFlashAction(true, valve004.gameObject))
               .Execute();
        }
        /// <summary>
        /// 2-2-8    4-1-7  打开进口阀         2-2-16  4-1-14  关闭进口阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve002_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 4-1-7   打开进口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-6", true))
               .Append(new CheckSmallAction("4-1-7", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve002.gameObject))
               .Append(new DOLocalRotaAction(valve002, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开进口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-7", true))
               .Append(new InvokeCurrentGuideAction(8))
               .Append(new InvokeFlashAction(true, valve003.gameObject))
               .Execute();
            // 2-2-8   打开进口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-7", true))
               .Append(new CheckSmallAction("2-2-8", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve002.gameObject))
               .Append(new DOLocalRotaAction(valve002,new Vector3 (0,0,-90),0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开进口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-8", true))
               .Append(new InvokeCurrentGuideAction(9))
               .Append(new InvokeFlashAction(true, valve003.gameObject))
               .Execute();
            // 2-2-16   关闭进口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-15", true))
               .Append(new CheckSmallAction("2-2-16", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve002.gameObject))
               .Append(new DOLocalRotaAction(valve002, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭进口阀", Color.green))
               .Append(new UpdateSmallAction("2-2-16", true))
               .Append(new InvokeCurrentGuideAction(17))
               .Append(new InvokeFlashAction(true, valve003.gameObject))
               .Execute();
            // 4-1-14   关闭进口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-13", true))
               .Append(new CheckSmallAction("4-1-14", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve002.gameObject))
               .Append(new DOLocalRotaAction(valve002, new Vector3(0, 0, 0), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭进口阀", Color.green))
               .Append(new UpdateSmallAction("4-1-14", true))
               .Append(new InvokeCurrentGuideAction(15))
               .Append(new InvokeFlashAction(true, valve003.gameObject))
               .Execute();
        }

        /// <summary>
        ///  2-2-7   使用软管连接   4-1-4    使用软管连接至洗脱液桶 
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose006_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-2-7  使用软管连接   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-6", true))
               .Append(new CheckSmallAction("2-2-7", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose007.gameObject))
               .Append(new ChangeVirtualRealityAction(hose006.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("2-2-7", true))
               .Append(new InvokeCurrentGuideAction(8))
               .Append(new InvokeFlashAction(true, valve002.gameObject))
               .Execute();            
            // 4-1-4    使用软管连接至洗脱液桶 
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-3", true))
               .Append(new CheckSmallAction("4-1-4", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose006.gameObject))
               .Append(new ChangeVirtualRealityAction(hose006.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "使用软管连接至洗脱液桶", Color.green))
               .Append(new UpdateSmallAction("4-1-4", true))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new GameObjectAction(stosteBucket))
               .Append(new ChangeVirtualRealityAction(stosteBucket.gameObject,true))
               .Append(new InvokeFlashAction(true, stosteBucket.gameObject))
               .Execute();
        }
        /// <summary>
        ///  2-2-6   使用软管连接    
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose005_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-2-6   使用软管连接   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-5", true))
               .Append(new CheckSmallAction("2-2-6", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose005.gameObject))
               .Append(new ChangeVirtualRealityAction(hose005.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("2-2-6", true))
               .Append(new GameObjectAction(hose006))
               .Append(new ChangeVirtualRealityAction(hose006.gameObject, true))
               .Append(new InvokeCurrentGuideAction(7))
               .Append(new InvokeFlashAction(true, hose006.gameObject))
               .Execute();

        }
        /// <summary>
        ///  2-2-5   使用软管连接    4-1-3    使用软管连接至超滤
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose004_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-2-5   使用软管连接   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-4", true))
               .Append(new CheckSmallAction("2-2-5", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose004.gameObject))
               .Append(new ChangeVirtualRealityAction(hose004.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("2-2-5", true))
               .Append(new GameObjectAction(hose005))
               .Append(new ChangeVirtualRealityAction(hose005.gameObject, true))
               .Append(new InvokeCurrentGuideAction(6))
               .Append(new InvokeFlashAction(true, hose005.gameObject))
               .Execute();
            // 4-1-3    使用软管连接至超滤   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 4, 1, false))
               .Append(new CheckSmallAction("4-1-2", true))
               .Append(new CheckSmallAction("4-1-3", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose004.gameObject))
               .Append(new ChangeVirtualRealityAction(hose004.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接至超滤", Color.green))
               .Append(new UpdateSmallAction("4-1-3", true))
               .Append(new GameObjectAction(hose006))
               .Append(new ChangeVirtualRealityAction(hose006.gameObject, true))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new InvokeFlashAction(true, hose006.gameObject))
               .Execute();

        }

        /// <summary>
        /// 2-2-4         点击开始搅拌
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleCollectBucketMotor_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //2-2-4         点击开始搅拌
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-3", true))
               .Append(new CheckSmallAction("2-2-4", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, sampleCollectBucketMotor.gameObject))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已开始搅拌", Color.green))
               .Append(new UpdateTransparencyAction(bucket04.gameObject,true))
               .Append(new UpdateTransparencyAction(bucket05.gameObject, true))
               .Append(new CoroutineFluidLevelAction(bucket04Flu.gameObject,0.5f,3,true))
               .Append(new CoroutineFluidLevelAction(bucket05Flu.gameObject, 0.7f, 3, false))
               .Append(new UpdateTransparencyAction(bucket04.gameObject, false))
               .Append(new UpdateTransparencyAction(bucket05.gameObject, false))
               .Append(new UpdateSmallAction("2-2-4", true))
               .Append(new GameObjectAction(hose004))
               .Append(new ChangeVirtualRealityAction(hose004.gameObject, true))
               .Append(new InvokeCurrentGuideAction(5))
               .Append(new InvokeFlashAction(true, hose005.gameObject))
               .Execute();
        }
        /// <summary>
        /// 2-2-3    点击启动蠕动泵  2-2-13     点击关闭蠕动泵
        /// </summary>
        /// <param name="arg0"></param>
        private void Pump01_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //2-2-3    点击启动蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-2", true))
               .Append(new CheckSmallAction("2-2-3", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump01.gameObject))
               .Append(new GameObjectAction(pump01Switch_OFF, false))
               .Append(new GameObjectAction(pump01Switch_ON, true))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已启动蠕动泵", Color.green))
               .Append(new UpdateSmallAction("2-2-3", true))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new InvokeFlashAction(true, sampleCollectBucketMotor.gameObject))
               .Execute();
            //2-2-13    点击关闭蠕动泵
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-12", true))
               .Append(new CheckSmallAction("2-2-13", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, pump01.gameObject))
               .Append(new GameObjectAction(pump01Switch_OFF, true))
               .Append(new GameObjectAction(pump01Switch_ON, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭蠕动泵", Color.green))
               .Append(new UpdateSmallAction("2-2-13", true))
               .Append(new InvokeCurrentGuideAction(14))
               .Append(new InvokeFlashAction(true, pump02.gameObject))
               .Execute();
        }

        /// <summary>
        ///  2-2-2   使用软管连接    
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose003_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-2-2   使用软管连接 2-2-2   使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-1", true))
               .Append(new CheckSmallAction("2-2-2", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose003.gameObject))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new ChangeVirtualRealityAction(hose003.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("2-2-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, pump01.gameObject))
               .Execute();

        }

        /// <summary>
        /// 2-2-1    移至超滤装置旁          2-2-20   移至层析系统旁
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleCollectBucket_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-2-1    移至超滤装置旁
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, sampleCollectBucket.gameObject))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(-4.060406f, -1.531f, 0f), 1f,true))
               .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, -180f), 1f, false))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(3.832f, -1.531f, 0f), 2f))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(4.060403f, 0.2798906f, 0f), 1f, true))
               .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, -90f), 1f, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移至超滤装置旁", Color.green))
               .Append(new UpdateSmallAction("2-2-1", true))
               .Append(new GameObjectAction(hose003))
               .Append(new GameObjectAction(hose002,false))
               .Append(new ChangeVirtualRealityAction(hose003.gameObject,true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, hose003.gameObject))
               .Execute();
            // 2-2-20    移至层析系统旁
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 2, false))
               .Append(new CheckSmallAction("2-2-19", true))
               .Append(new CheckSmallAction("2-2-20", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, sampleCollectBucket.gameObject))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(3.832f, -1.531f, 0f), 1f,true))
               .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, 0f), 1f, false))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(-0.7406797f, -1.531f, 0f), 1.5f))
               .Append(new DOLocalMoveAction(sampleCollectBucket, new Vector3(-0.7406797f, -0.09607813f, 0), 1f,true))
               .Append(new DOLocalRotaAction(sampleCollectBucket, new Vector3(0f, 0f, -90f), 1f, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已移至层析系统旁", Color.green))
               .Append(new UpdateSmallAction("2-2-20", true))
               .Append(new InvokeCompletedAction(2, 2))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, chromatographicColumn.gameObject))
               .Execute();
        }
    

        /// <summary>
        /// 2-1-3     加入碳酸缓冲液
        /// </summary>
        /// <param name="arg0"></param>
        private void SampleCollectBucket_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-1-3     加入碳酸缓冲液
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
               .Append(new CheckSmallAction("2-1-2", true))
               .Append(new CheckSmallAction("2-1-3", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData,GoodsType.DBCH_LT_01))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_LT_01.ToString(), UpdateType.Remove))
               .Append(new InvokeFlashAction(false, sampleCollectBucket.gameObject))
               .Append(new DOLocalMoveAction(sampleCollectBucketHat,new Vector3 (0.039f, -0.016f, 0.864f),0.5f))
               .Append(new GameObjectAction(measuringCylinder02, true))
               .Append(new CoroutineFluidLevelAction(measuringCylinder02Flu.gameObject,0,3))
               .Append(new GameObjectAction(measuringCylinder02, false))
               .Append(new DOLocalMoveAction(sampleCollectBucketHat, new Vector3(0.0388f, -0.016f, 0.3626f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已加入碳酸缓冲液", Color.green))
               .Append(new UpdateSmallAction("2-1-3", true))
               .Append(new InvokeCompletedAction(2,1))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, sampleCollectBucket.gameObject))
               .Execute();
        }


        /// <summary>
        /// 2-1-2    收起量筒
        /// </summary>
        /// <param name="arg0"></param>
        private void MeasuringCylinder_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 2-1-2    收起量筒
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
               .Append(new CheckSmallAction("2-1-1", true))
               .Append(new CheckSmallAction("2-1-2", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, measuringCylinder.gameObject))
               .Append(new GameObjectAction(measuringCylinder,false))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_LT_01.ToString(), UpdateType.Add))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已收起量筒", Color.green))
               .Append(new UpdateSmallAction("2-1-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .Append(new InvokeFlashAction(true, sampleCollectBucket.gameObject))
               .Execute();
        }

        /// <summary>
        /// 2-1-1      量取一定量碳酸缓冲液
        /// </summary>
        /// <param name="arg0"></param>
        private void CarbonateBufferBucket_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //2-1-1      量取一定量碳酸缓冲液
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 2, 1, false))
               .Append(new CheckSmallAction("2-1-1", false))
               .Append(new CheckSmallAction("2-1-1-1", false))
               .Append(new UpdateSmallAction("2-1-1-1", true))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, carbonateBufferBucket.gameObject))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat,new Vector3 (0.01349268f,0, 0.0354f),0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.0354f, 0, 0.0354f), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.0354f, 0, -0.031f), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucket, new Vector3(0.0626f, 0, 0.0582f), 0.5f))
               .Append(new DOLocalRotaAction(carbonateBufferBucket,new Vector3 (0,90,0),0.5f))
               .Append(new GameObjectAction(carbonateBuffer_Effect))
               .Append(new GameObjectAction(measuringCylinderFlu.gameObject))
               .Append(new CoroutineFluidLevelAction(measuringCylinderFlu.gameObject,1,3))
               .Append(new GameObjectAction(carbonateBuffer_Effect,false))
               .Append(new DOLocalRotaAction(carbonateBufferBucket, new Vector3(0, 0, 0), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucket, new Vector3(0f, 0, -0.002052246f), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.0354f, 0, 0.0354f), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.01349268f, 0, 0.0354f), 0.5f))
               .Append(new DOLocalMoveAction(carbonateBufferBucketHat, new Vector3(0.01349268f, 0, 0.02594605f), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已量取一定量碳酸缓冲液", Color.green))
               .Append(new UpdateSmallAction("2-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, measuringCylinder.gameObject))
               .Execute();
        }

        /// <summary>
        ///  1-3-13   拆除软管         3-3-15   拆除软管  
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose002_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-3-13   拆除软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-12", true))
               .Append(new CheckSmallAction("1-3-13", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose002.gameObject))
               .Append(new GameObjectAction(hose002.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除软管", Color.green))
               .Append(new UpdateSmallAction("1-3-13", true))
               .Append(new InvokeCompletedAction(1,3))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, carbonateBufferBucket.gameObject))
               .Execute();
            //3-3-15   拆除软管
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-14", true))
               .Append(new CheckSmallAction("3-3-15", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose002.gameObject))
               .Append(new GameObjectAction(hose002.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已拆除软管", Color.green))
               .Append(new UpdateSmallAction("3-3-15", true))
               .Append(new InvokeCurrentGuideAction(16))
               .Append(new InvokeFlashAction(true, eluantBucket.gameObject))
               .Execute();

        }
        /// <summary>
        ///  1-3-6   使用软管连接     3-3-4    使用软管连接
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose002_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-3-6   使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-5", true))
               .Append(new CheckSmallAction("1-3-6", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new GameObjectAction(hose002))
               .Append(new ChangeVirtualRealityAction(hose002.gameObject, false))
               .Append(new InvokeFlashAction(false, hose002.gameObject))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("1-3-6", true))
               .Append(new InvokeCurrentGuideAction(7))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", false);
               })
               .Execute();
            //3-3-4   使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-3", true))
               .Append(new CheckSmallAction("3-3-4", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, hose002.gameObject))
               .Append(new ChangeVirtualRealityAction(hose002.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("3-3-4", true))
               .Append(new InvokeCurrentGuideAction(5))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);
               })
               .Execute();


        }
        /// <summary>
        /// 1-3-5   放置样品收集桶        3-3-3   放置洗脱液桶
        /// </summary>
        /// <param name="arg0"></param>
        private void sampleCollectBucket_PutArea_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-3-5   放置样品收集桶
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 3, false))
               .Append(new CheckSmallAction("1-3-4", true))
               .Append(new CheckSmallAction("1-3-5", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_YPSJT_01))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_YPSJT_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, sampleCollectBucket_PutArea.gameObject))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置样品收集桶", Color.green))
               .Append(new GameObjectAction(sampleCollectBucket))
               .Append(new GameObjectAction(sampleCollectBucket_PutArea,false))
               .Append(new InvokeCurrentGuideAction(6))
               .Append(new UpdateSmallAction("1-3-5", true))
               .Append(new GameObjectAction(hose002))
               .Append(new ChangeVirtualRealityAction(hose002.gameObject,true))
               .Append(new InvokeFlashAction(true, hose002.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();

            //3-3-3   放置洗脱液桶
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 2, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 3, 3, false))
               .Append(new CheckSmallAction("3-3-2", true))
               .Append(new CheckSmallAction("3-3-3", false))
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_XTYT_01))
               .Append(new UpdateGoodsAction(GoodsType.DBCH_XTYT_01.ToString(), UpdateType.Remove))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, sampleCollectBucket_PutArea.gameObject))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已放置洗脱液桶", Color.green))
               .Append(new GameObjectAction(eluantBucket))
               .Append(new GameObjectAction(sampleCollectBucket_PutArea, false))
               .Append(new InvokeCurrentGuideAction(4))
               .Append(new UpdateSmallAction("3-3-3", true))
               .Append(new GameObjectAction(hose002))
               .Append(new ChangeVirtualRealityAction(hose002.gameObject,true))
               .Append(new InvokeFlashAction(true, hose002.gameObject))
               .OnCompleted(() =>
               {
               })
               .Execute();
        }

        /// <summary>
        /// 1-2-2   打开上清液出口阀     1-2-11   关闭上清液出口阀
        /// </summary>
        /// <param name="arg0"></param>
        private void Valve001_PointerClick(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            // 1-2-11   关闭上清液出口阀  
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
               .Append(new CheckSmallAction("1-2-10", true))
               .Append(new CheckSmallAction("1-2-11", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve001.gameObject))
               .Append(new DOLocalRotaAction(valve001, new Vector3(0, 0, -90), 0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已关闭上清液出口阀", Color.green))
               .Append(new UpdateSmallAction("1-2-11", true))
               .Append(new InvokeCompletedAction(1,2))
               .Append(new InvokeCurrentGuideAction(1))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);
               })
               .Execute();
            //1-2-2   打开上清液出口阀   
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
               .Append(new CheckSmallAction("1-2-1", true))
               .Append(new CheckSmallAction("1-2-2", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new InvokeFlashAction(false, valve001.gameObject))
               .Append(new DOLocalRotaAction(valve001,new Vector3 (0,0,0),0.5f))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已打开上清液出口阀", Color.green))
               .Append(new UpdateSmallAction("1-2-2", true))
               .Append(new InvokeCurrentGuideAction(3))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, string>("蛋白纯化参数设置", "上样量L：", "160");
               })
               .Execute();

        }

        /// <summary>
        /// 1-2-1   使用硅胶管连接    
        /// </summary>
        /// <param name="arg0"></param>
        private void SiliconeHose001_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-2-1   使用硅胶管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, true))
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 2, false))
               .Append(new CheckSmallAction("1-2-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_GJG_01))
               .Append(new InvokeFlashAction(false, siliconeHose001.gameObject))
               .Append(new GameObjectAction(siliconeHose001))
               .Append(new ChangeVirtualRealityAction(siliconeHose001.gameObject, false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用硅胶管连接", Color.green))
               .Append(new UpdateSmallAction("1-2-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .Append(new InvokeFlashAction(true, valve001.gameObject))
               .Execute();
           

        }
        

        /// <summary>
        /// 1-1-1  使用软管连接
        /// </summary>
        /// <param name="arg0"></param>
        private void Hose001_Drop(BaseEventData arg0)
        {
            PointerEventData eventData = arg0 as PointerEventData;
            if (eventData == null)
                return;
            //1-1-1  使用软管连接
            Task.NewTask(eventData.pointerEnter)
               .Append(new CheckMonitorAction(customStage.OperateMonitor, 1, 1, false))
               .Append(new CheckSmallAction("1-1-1", false))
               .Append(new InvokeCloseAllGuideAction())
               .Append(new CheckGoodsAction(eventData, GoodsType.DBCH_RG_01))
               .Append(new InvokeFlashAction(false, hose001.gameObject))
               .Append(new DOLocalMoveAction(hose001_BucketHat,new Vector3 (0,0,2.83f),0.5f))
               .Append(new GameObjectAction(hose001_BucketHat,false))
               .Append(new GameObjectAction(hose001, true))
               .Append(new ChangeVirtualRealityAction(hose001.gameObject,false))
               .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "已使用软管连接", Color.green))
               .Append(new UpdateSmallAction("1-1-1", true))
               .Append(new InvokeCurrentGuideAction(2))
               .OnCompleted(() =>
               {
                   EventDispatcher.ExecuteEvent<string, bool>("蛋白纯化阀门点击之前", "出口阀1", true);
               })
               .Execute();
        }
    }
}
