using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 离心车间 定义
    /// </summary>
    public partial class CentrifugeWorkshop
    {
        /// <summary>
        /// 平板式离心机 内转鼓
        /// </summary>
        private Transform insideDrum;

        /// <summary>
        /// 平板式离心机 电源按钮
        /// </summary>
        private Transform powerSupply;
        /// <summary>
        /// 平板式离心机 停止按钮
        /// </summary>
        private Transform stopButton;
        /// <summary>
        /// 平板式离心机 高速按钮
        /// </summary>
        private Transform highSpeedButton;
        /// <summary>
        /// 平板式离心机 低速按钮
        /// </summary>
        private Transform lowSpeedButton;

        /// <summary>
        /// 上盖
        /// </summary>
        private Transform topCover;
        /// <summary>
        /// 传动轴一
        /// </summary>
        private Transform driveShaftOne;
        /// <summary>
        /// 传动轴二
        /// </summary>
        private Transform driveShaftTwo;

        /// <summary>
        /// 滤袋
        /// </summary>
        private Transform filterBag;
        /// <summary>
        /// 物料软管
        /// </summary>
        private Transform suppliesHosePipe;
        /// <summary>
        /// 纯化水软管
        /// </summary>
        private Transform purifiedWaterHosePipe;
        /// <summary>
        /// 氮气软管
        /// </summary>
        private Transform nitrogenHosePipe;

        /// <summary>
        /// 物料软管
        /// </summary>
        private Transform suppliesHosePipe2;
        /// <summary>
        /// 纯化水软管
        /// </summary>
        private Transform purifiedWaterHosePipe2;
        /// <summary>
        /// 氮气软管
        /// </summary>
        private Transform nitrogenHosePipe2;

        /// <summary>
        /// 物料桶
        /// </summary>
        private Transform bucket;

        /// <summary>
        /// 物料桶放置点
        /// </summary>
        private Transform BucketPutArea;

        /// <summary>
        /// 滤袋和物料
        /// </summary>
        private Transform supplies;

        /// <summary>
        /// 滤袋和物料的液位
        /// </summary>
        private Transform suppliesLiquidLevel;

        /// <summary>
        /// 物料桶内液体液位
        /// </summary>
        private LiquidVolume tankLiquidLevel;

        /// <summary>
        /// 物料桶内液体液位
        /// </summary>
        private Transform suppliesParticle;

        /// <summary>
        /// 离心机内壁结晶体
        /// </summary>
        private Transform crystal;

        /// <summary>
        /// 玻璃观察窗
        /// </summary>
        private UsableComponent glassWindow;

        /// <summary>
        /// 相机切换
        /// </summary>
        private CameraSwitcher m_CameraSwitcher;

        /// <summary>
        /// MouseOrbit
        /// </summary>
        private MouseOrbit m_MouseOrbit;

        protected void InitializeComponent()
        {
            insideDrum = transform.Find("平板式离心机/转股部件");

            powerSupply = transform.Find("平板式离心机控制柜/电源指示");
            stopButton = transform.Find("平板式离心机控制柜/停止");
            highSpeedButton = transform.Find("平板式离心机控制柜/高速");
            lowSpeedButton = transform.Find("平板式离心机控制柜/低速");

            topCover = transform.Find("平板式离心机/翻盖");
            driveShaftOne = transform.Find("平板式离心机/平衡杠部件01");
            driveShaftTwo = transform.Find("平板式离心机/平衡杠部件01/平衡杠部件02");

            filterBag = transform.Find("平板式离心机/滤袋");
            filterBag.gameObject.SetActive(false);

            crystal = transform.Find("平板式离心机/滤袋/滤袋_污尘");
            crystal.gameObject.SetActive(false);

            suppliesHosePipe = transform.Find("绕管道/物料软管");
            suppliesHosePipe.gameObject.SetActive(false);

            purifiedWaterHosePipe = transform.Find("绕管道/纯化水软管");
            purifiedWaterHosePipe.gameObject.SetActive(false);

            nitrogenHosePipe = transform.Find("绕管道/氮气软管");
            nitrogenHosePipe.gameObject.SetActive(false);

            suppliesHosePipe2 = transform.Find("绕管道/物料软管2");
            suppliesHosePipe2.gameObject.SetActive(false);

            purifiedWaterHosePipe2 = transform.Find("绕管道/纯化水软管2");
            purifiedWaterHosePipe2.gameObject.SetActive(false);

            nitrogenHosePipe2 = transform.Find("绕管道/氮气软管2");
            nitrogenHosePipe2.gameObject.SetActive(false);

            bucket = transform.Find("扩展/不锈钢桶");
            bucket.gameObject.SetActive(false);

            BucketPutArea = transform.Find("扩展/物料桶放置点");
            BucketPutArea.gameObject.SetActive(false);

            supplies = transform.Find("扩展/滤袋及物料");
            supplies.gameObject.SetActive(false);

            suppliesParticle = transform.Find("扩展/物料粒子效果");
            suppliesParticle.gameObject.SetActive(false);

            suppliesLiquidLevel = transform.Find("扩展/滤袋及物料/滤袋及物料内胆");

            tankLiquidLevel = transform.Find("扩展/不锈钢桶/桶/不锈钢桶_液位").GetComponent<LiquidVolume>();
            tankLiquidLevel.gameObject.SetActive(false);

            glassWindow = transform.Find("平板式离心机/翻盖/视镜").GetComponent<UsableComponent>();

            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            m_MouseOrbit = Camera.main.transform.GetComponent<MouseOrbit>();
            #region Manager
            Transform valveParent = transform.Find("打组阀门/打组阀门");//查找阀门父节点
            ValveManager.Instance.Init(RootDir + "Stage/离心工段/离心-阀门匹配表.xml", valveParent);
            Transform pipeParent = transform.Find("绕管道");//查找管道父节点
            Transform fluidParent = transform.Find("流体");//查找流体父节点
            PipeFittingManager.Instance.Init(RootDir + "Stage/离心工段/离心-流体配置表.xml", pipeParent, fluidParent);
            Transform guideParent = transform.Find("引导");//查找引导父节点
            ProductionGuideManager.Instance.Init(RootDir + "Stage/离心工段/离心-引导匹配表.xml", guideParent);
            EventDispatcher.ExecuteEvent<string>(Events.Status.Init, RootDir + "Stage/离心工段/离心-状态表.xml");
            #endregion
        }
    }
}
