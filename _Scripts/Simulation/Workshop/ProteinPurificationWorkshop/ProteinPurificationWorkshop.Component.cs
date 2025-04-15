using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Common;
using XFramework.Actions;
using UnityEngine;
using XFramework.Component;

namespace XFramework.Simulation
{
    /// <summary>
    /// 蛋白纯化间
    /// </summary>
    public partial class ProteinPurificationWorkshop
    {
        // <summary>
        /// 相机切换
        /// </summary>
        private CameraSwitcher m_CameraSwitcher;
        /// <summary>
        /// MouseOrbit
        /// </summary>
        private MouseOrbit m_MouseOrbit;
        /// <summary>
        /// 软管001
        /// </summary>
        private Transform hose001;
        /// <summary>
        /// 软管001  桶盖
        /// </summary>
        private Transform hose001_BucketHat;
        /// <summary>
        /// 硅胶管001
        /// </summary>
        private Transform siliconeHose001;
        /// <summary>
        /// 硅胶管002
        /// </summary>
        private Transform siliconeHose002;
        /// <summary>
        /// 上样出口阀
        /// </summary>
        private Transform valve001;
        /// <summary>
        /// 进口阀
        /// </summary>
        private Transform valve002;
        /// <summary>
        /// 回流口阀
        /// </summary>
        private Transform valve003;
        /// <summary>
        /// 透出液出口阀
        /// </summary>
        private Transform valve004;
        /// <summary>
        /// 样品收集桶出液阀
        /// </summary>
        private Transform valve005;
        /// <summary>
        /// 洗脱液桶出液阀
        /// </summary>
        private Transform valve006;
        /// <summary>
        /// 样品收集桶
        /// </summary>
        private Transform sampleCollectBucket;
        /// <summary>
        /// 样品收集桶  桶盖
        /// </summary>
        private Transform sampleCollectBucketHat;
        /// <summary>
        /// 样品收集桶  发动机
        /// </summary>
        private Transform sampleCollectBucketMotor;
        /// <summary>
        /// 样品收集桶  放置处
        /// </summary>
        private Transform sampleCollectBucket_PutArea;
        /// <summary>
        /// 软管002
        /// </summary>
        private Transform hose002;
        /// <summary>
        /// 软管003
        /// </summary>
        private Transform hose003;
        /// <summary>
        /// 软管004
        /// </summary>
        private Transform hose004;
        /// <summary>
        /// 软管005
        /// </summary>
        private Transform hose005;
        /// <summary>
        /// 软管006
        /// </summary>
        private Transform hose006;
        /// <summary>
        /// 软管007
        /// </summary>
        private Transform hose007;
        /// <summary>
        /// 软管008
        /// </summary>
        private Transform hose008;
        /// <summary>
        /// 软管009
        /// </summary>
        private Transform hose009;
        /// <summary>
        /// 软管010
        /// </summary>
        private Transform hose010;
        /// <summary>
        /// 软管011
        /// </summary>
        private Transform hose011;
        /// <summary>
        /// 软管012
        /// </summary>
        private Transform hose012;
        /// <summary>
        /// 软管013
        /// </summary>
        private Transform hose013;
        /// <summary>
        /// 碳酸缓冲液桶
        /// </summary>
        private Transform carbonateBufferBucket;
        /// <summary>
        /// 碳酸缓冲液桶  盖子
        /// </summary>
        private Transform carbonateBufferBucketHat;
        /// <summary>
        /// 缓冲液桶液体特效
        /// </summary>
        private Transform carbonateBuffer_Effect;
        /// <summary>
        /// 量筒液体
        /// </summary>
        private FluidLevelComponent measuringCylinderFlu;
        /// <summary>
        /// 量筒
        /// </summary>
        private Transform measuringCylinder;
        /// <summary>
        /// 样品收集桶旁边量筒
        /// </summary>
        private Transform measuringCylinder02;
        /// <summary>
        /// 样品收集桶旁边量筒
        /// </summary>
        private FluidLevelComponent measuringCylinder02Flu;
        /// <summary>
        /// 蠕动泵01
        /// </summary>
        private Transform pump01;
        /// <summary>
        /// 蠕动泵01  电源关
        /// </summary>
        private Transform pump01Switch_OFF;
        /// <summary>
        /// 蠕动泵01  电源开
        /// </summary>
        private Transform pump01Switch_ON;
        /// <summary>
        /// 蠕动泵02
        /// </summary>
        private Transform pump02;
        /// <summary>
        /// 蠕动泵02  电源关
        /// </summary>
        private Transform pump02Switch_OFF;
        /// <summary>
        /// 蠕动泵02  电源开
        /// </summary>
        private Transform pump02Switch_ON;
        private Transform bucket04;
        private FluidLevelComponent bucket04Flu;
        private Transform bucket05;
        private FluidLevelComponent bucket05Flu;
        /// <summary>
        /// 上样液桶
        /// </summary>
        private Transform loadingBufferBucket;
        /// <summary>
        /// 上样液桶  液体
        /// </summary>
        private FluidLevelComponent loadingBufferBucketFlu;
        /// <summary>
        /// 上样液桶  桶身
        /// </summary>
        private Transform loadingBufferBucketSelf;
        /// <summary>
        /// 上样液桶出口阀
        /// </summary>
        private Transform loadingBufferValve;
        /// <summary>
        /// 离子交换层析柱
        /// </summary>
        private Transform chromatographicColumn;
        /// <summary>
        /// 桶盖 3_3_1
        /// </summary>
        private Transform bucketHat_3_3_1;
        /// <summary>
        /// 桶盖 3_3_2
        /// </summary>
        private Transform bucketHat_3_3_2;
        /// <summary>
        /// 洗脱液桶
        /// </summary>
        private Transform eluantBucket;
        /// <summary>
        /// 膜包
        /// </summary>
        private Transform membraneBag;
        /// <summary>
        /// 原液桶
        /// </summary>
        private Transform stosteBucket;
        /// <summary>
        /// PLC屏幕
        /// </summary>
        private Transform plcScreen;
        /// <summary>
        /// 菌体收集罐罐体
        /// </summary>
        private Transform collectTank;
        /// <summary>
        /// 菌体收集罐溶液
        /// </summary>
        private FluidLevelComponent collectTankFlu;

        private void InitializeComponent()
        {
            collectTank = transform.Find("流动泵_原液桶_超滤/原液桶/菌体收集罐_罐体");
            collectTankFlu = transform.Find("流动泵_原液桶_超滤/原液桶/菌体收集罐_溶液").GetComponent<FluidLevelComponent>();


            plcScreen = transform.Find("扩展/PLC屏幕");
            stosteBucket = transform.Find("流动泵_原液桶_超滤/原液桶");
            stosteBucket.gameObject.SetActive(false);
            membraneBag = transform.Find("扩展/膜包");
            eluantBucket = transform.Find("5个油拌桶/洗脱液桶");
            eluantBucket.gameObject.SetActive(false);
            bucketHat_3_3_1 = transform.Find("4个储液罐/储液罐盖001");
            bucketHat_3_3_2 = transform.Find("4个储液罐/储液罐盖002");

            loadingBufferBucket = transform.Find("5个油拌桶/上样液桶");
            loadingBufferValve = transform.Find("5个油拌桶/上样液桶/上样液桶出口阀");
            loadingBufferBucketFlu = transform.Find("5个油拌桶/上样液桶/搅拌桶_溶液002").GetComponent<FluidLevelComponent>();
            loadingBufferBucketSelf = transform.Find("5个油拌桶/上样液桶/搅拌桶_桶身006");

            chromatographicColumn = transform.Find("扩展/离子交换层析柱");
            pump02 = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵2/蠕动泵开关");
            pump02Switch_OFF = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵2/蠕动泵开关/蠕动泵关");
            pump02Switch_ON = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵2/蠕动泵开关/蠕动泵开");
            pump02Switch_ON.gameObject.SetActive(false);
            valve002 = transform.Find("流动泵_原液桶_超滤/超滤/进口阀");
            valve003 = transform.Find("流动泵_原液桶_超滤/超滤/回流口阀");
            valve004 = transform.Find("流动泵_原液桶_超滤/超滤/透出液出口阀");
            valve005 = transform.Find("5个油拌桶/样品收集桶/样品收集桶出液阀");
            valve006 = transform.Find("5个油拌桶/洗脱液桶/洗脱液桶出液阀");
            bucket04 = transform.Find("5个油拌桶/样品收集桶/搅拌桶_桶身008");
            bucket04Flu = transform.Find("5个油拌桶/样品收集桶/搅拌桶_溶液004").GetComponent<FluidLevelComponent>();
            bucket05 = transform.Find("5个油拌桶/搅拌桶005/搅拌桶_桶身009");
            bucket05Flu = transform.Find("5个油拌桶/搅拌桶005/搅拌桶_溶液005").GetComponent<FluidLevelComponent>();
            pump01 = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵 1/蠕动泵开关");
            pump01Switch_OFF = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵 1/蠕动泵开关/蠕动泵关");
            pump01Switch_ON = transform.Find("流动泵_原液桶_超滤/蠕动泵/蠕动泵 1/蠕动泵开关/蠕动泵开");
            pump01Switch_ON.gameObject.SetActive(false);
            measuringCylinder02 = transform.Find("扩展/收集桶旁量筒");
            measuringCylinder02.gameObject.SetActive(false);
            measuringCylinder02Flu = transform.Find("扩展/收集桶旁量筒/量筒溶液").GetComponent<FluidLevelComponent>();
            measuringCylinderFlu = transform.Find("操作台周边/量筒/量筒溶液").GetComponent<FluidLevelComponent>();
            measuringCylinderFlu.gameObject.SetActive(false);
            measuringCylinder = transform.Find("操作台周边/量筒");
            carbonateBuffer_Effect = transform.Find("操作台周边/碳酸缓冲液桶/液体特效");
            carbonateBuffer_Effect.gameObject.SetActive(false);
            carbonateBufferBucket = transform.Find("操作台周边/碳酸缓冲液桶/油桶_主体");
            carbonateBufferBucketHat = transform.Find("操作台周边/碳酸缓冲液桶/油桶_盖子");
            hose002 = transform.Find("软管和夹子/hose002");
            hose002.gameObject.SetActive(false);
            hose003 = transform.Find("软管和夹子/hose003");
            hose003.gameObject.SetActive(false);
            hose004 = transform.Find("软管和夹子/hose004");
            hose004.gameObject.SetActive(false);
            hose005 = transform.Find("软管和夹子/hose005");
            hose005.gameObject.SetActive(false);
            hose006 = transform.Find("软管和夹子/hose006");
            hose006.gameObject.SetActive(false);
            hose007 = transform.Find("软管和夹子/hose007");
            hose007.gameObject.SetActive(false);
            hose008 = transform.Find("软管和夹子/hose008");
            hose008.gameObject.SetActive(false);
            hose009 = transform.Find("软管和夹子/hose009");
            hose009.gameObject.SetActive(false);
            hose010 = transform.Find("软管和夹子/hose010");
            hose010.gameObject.SetActive(false);
            hose011 = transform.Find("软管和夹子/hose011");
            hose011.gameObject.SetActive(false);
            hose012 = transform.Find("软管和夹子/hose012");
            hose013 = transform.Find("软管和夹子/hose013");
            sampleCollectBucket_PutArea = transform.Find("扩展/样品收集桶放置处");
            sampleCollectBucket_PutArea.gameObject.SetActive(false);
            sampleCollectBucket = transform.Find("5个油拌桶/样品收集桶");
            sampleCollectBucket.gameObject.SetActive(false);
            sampleCollectBucketHat = transform.Find("5个油拌桶/样品收集桶/桶盖");
            sampleCollectBucketMotor = transform.Find("5个油拌桶/样品收集桶/搅拌桶_发动机008");

            valve001 = transform.Find("钢架平台周边/上样阀/卡箍手动球阀(DN25)002/打开上清液出口阀");
            siliconeHose001 = transform.Find("软管和夹子/硅胶管");
            siliconeHose001.gameObject.SetActive(false);
            siliconeHose002 = transform.Find("软管和夹子/硅胶管02");
            siliconeHose002.gameObject.SetActive(false);
            hose001_BucketHat = transform.Find("4个储液罐/NaCl桶/桶盖");
            hose001 = transform.Find("软管和夹子/hose001");
            #region 镜头拉近
            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            Transform bestAngleParent = transform.Find("最佳视角");//查找最佳视角父节点
            LookPointManager.Instance.Init(bestAngleParent);
            #endregion
            #region Manager
            Transform guideParent = transform.Find("引导");//查找引导父节点
            ProductionGuideManager.Instance.Init(RootDir + "Stage/蛋白纯化工段/蛋白纯化-引导匹配表.xml", guideParent);
            CameraLookPointManager.Instance.Init(transform.Find("观察点"));

            #endregion
        }
    }
}
