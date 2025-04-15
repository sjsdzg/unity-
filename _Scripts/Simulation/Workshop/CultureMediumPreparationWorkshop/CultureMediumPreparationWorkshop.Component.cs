using Simulation.Component;
using UnityEngine;
using XFramework.Common;
using UnityEngine.UI;
using TMPro;
using LiquidVolumeFX;
using XFramework.PLC;
using XFramework.Component;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 离心车间 定义
    /// </summary>
    public partial class CultureMediumPreparationWorkshop
    {
        /// <summary>
        /// 玻璃观察窗
        /// </summary>
        //private UsableComponent glassWindow;

        /// <summary>
        /// 相机切换
        /// </summary>
        private CameraSwitcher m_CameraSwitcher;

        /// <summary>
        /// MouseOrbit
        /// </summary>
        private MouseOrbit m_MouseOrbit;
        /// <summary>
        /// 勺子动画
        /// </summary>
        Animation anim_spoon;
        /// <summary>
        /// 电子天平的称量区域
        /// </summary>
        Transform dropArea_balance;
        /// <summary>
        /// 烧杯
        /// </summary>
        Transform beaker;
        /// <summary>
        /// 烧杯内容物
        /// </summary>
        Transform beakerContent;
        /// <summary>
        /// 天平清洁效果
        /// </summary>
        ScrubEffectComponent scrubEffect_balance;
        /// <summary>
        /// 天平称量数值
        /// </summary>
        TextMeshProUGUI tmp_balanceNum;
        /// <summary>
        /// 天平去皮按钮
        /// </summary>
        Button button_couAff;
        /// <summary>
        /// 培养基
        /// </summary>
        Transform medium_original;
        /// <summary>
        /// 培养基盖子
        /// </summary>
        Transform cover;
        /// <summary>
        /// 称量加料用的培养基
        /// </summary>
        Transform medium_feed;
        /// <summary>
        /// 烧杯LiquidVolume组件
        /// </summary>
        LiquidVolume liquid_beaker;
        /// <summary>
        /// 勺子
        /// </summary>
        Transform spoon;
        /// <summary>
        /// 电子秤称量区域
        /// </summary>
        Transform dropArea_bigBalance;
        /// <summary>
        /// 电子秤清洁效果
        /// </summary>
        ScrubEffectComponent scrubEffect_bigBalance;
        /// <summary>
        /// 电子秤示数
        /// </summary>
        TextMeshProUGUI tmp_bigBalanceNum;
        /// <summary>
        /// 电子秤去皮按钮
        /// </summary>
        Button button_couAff2;
        /// <summary>
        /// 物料桶
        /// </summary>
        Transform bucket;
        /// <summary>
        /// F001S物料桶
        /// </summary>
        Transform bucket_F001S;
        /// <summary>
        /// F001S物料桶盖子
        /// </summary>
        Transform bucket_F001S_cover;
        /// <summary>
        /// F001S物料桶盖子
        /// </summary>
        Transform bucket_F001S_level;
        /// <summary>
        /// 舀子
        /// </summary>
        Transform scoop;
        /// <summary>
        /// 舀子动画
        /// </summary>
        Animation anim_scoop;
        /// <summary>
        /// 舀子效果
        /// </summary>
        ParticleSystem scoopParticle;


        /// <summary>
        /// 控制台1电源按钮
        /// </summary>
        Transform powerButton_1;

        GameObject plc_100L;

         PLC_100LDispensingTank plc_100LDispensing;
        /// <summary>
        /// 100L配置罐控制面板
        /// </summary>
        UsableComponent usable_100L;

        PLCPipeFittingManager plcPipeFitting;
        /// <summary>
        /// 100L配制罐肚子
        /// </summary>
        GameObject belly_100L;

        Transform blender_100L;

        FluidLevelComponent fluidLevel_100L;

        LiquidVolume liquidVolume_100L;

        Transform cover_100L;

        GameObject entrance_100L;

        GameObject bucket_feedF001S;

        ParticleSystem particle_feedF001S;

        Transform sanitaryPump_100L;

        /// <summary>
        /// 100L储液罐肚子
        /// </summary>
        GameObject belly_100LReservoir;
        
        FluidLevelComponent fluidLevel_100LReservoir;
        /// <summary>
        /// 除菌过滤器
        /// </summary>
        GameObject filter;

        GameObject console;

        /// <summary>
        /// 检测处除菌过滤器
        /// </summary>
        GameObject filter_detect;

        /// <summary>
        /// 除菌过滤器检测仪控制面板
        /// </summary>
        UsableComponent usable_detector;

        PLC_FilterDetector plc_FilterDetector;

        Transform blender_100LReservoir;

        PLC_100LReservoirTank plc_100LReservoirTank;

        PLCPipeFittingManager plcPipeFitting_Reservoir;

        GameObject sampleBottle_100L;

        FluidLevelComponent fluid_sampleBottle_100L;
        
        /// <summary>
        /// 传递窗
        /// </summary>
        GameObject passBox;

        GameObject passBoxBottle;

        Transform sampleValve_100L;

        DoorComponent doorComponent;

        GameObject boxSampleBottle_1;

        Color waterColor;

        GameObject connecter;

        GameObject filter_2;

        /// <summary>
        /// 控制台1电源按钮
        /// </summary>
        Transform powerButton_2;

        GameObject plc_200L;

        PLC_200LDispensingTank plc_200LDispensing;

        PLC_200LReservoirTank plc_200LReservoirTank;
        /// <summary>
        /// 100L配置罐控制面板
        /// </summary>
        UsableComponent usable_200L;

        PLCPipeFittingManager plcPipeFitting_2;

        PLCPipeFittingManager plcPipeFitting_Reservoir_2;

        /// <summary>
        /// 200L配制罐肚子
        /// </summary>
        GameObject belly_200L;

        Transform blender_200L;

        FluidLevelComponent fluidLevel_200L;

        LiquidVolume liquidVolume_200L;

        Transform cover_200L;

        GameObject entrance_200L;

        GameObject bucket_feed200L;

        ParticleSystem particle_feed200L;

        Transform sanitaryPump_200L;

        /// <summary>
        /// 200L储液罐肚子
        /// </summary>
        GameObject belly_200LReservoir;

        FluidLevelComponent fluidLevel_200LReservoir;

        GameObject sampleBottle_200L;

        Transform sampleValve_200L;

        Transform blender_200LReservoir;

        GameObject passBoxBottle_2;

        FluidLevelComponent fluid_passBoxBottle_2;

        GameObject passboxDoor;


        #region 记住相关物体的初始状态
        Vector3 cache_cover_LocalPosition;
        Vector3 cache_medium_feed_LocalEulerAngles;
        Vector3 cache_beakerContent_localScale;
        Vector3 cache_bucket_F001S_cover_localPosition;
        Vector3 cache_bucket_F001S_level_localPosition;
        Vector3 cache_powerButton_1_LocalEulerAngles;
        Vector3 cache_cover_100L_LocalEulerAngles;
        Vector3 cache_blender_100L_LocalEulerAngles;
        Vector3 cache_sanitaryPump_100L_localPosition;
        Vector3 cache_sampleValve_100L_LocalEulerAngles;
        Vector3 cache_powerButton_2_LocalEulerAngles;
        Vector3 cache_cover_200L_LocalEulerAngles;
        Vector3 cache_blender_200L_LocalEulerAngles;
        Vector3 cache_sanitaryPump_200L_localPosition;
        Vector3 cache_sampleValve_200L_LocalEulerAngles;
        #endregion

        protected void InitializeComponent()
        {
            dropArea_balance = transform.Find("扩展/电子天平称量区域");
            beaker = transform.Find("扩展/烧杯");
            beakerContent = beaker.Find("内容物");
            scrubEffect_balance = transform.Find("扩展/天平洁净效果").GetComponent<ScrubEffectComponent>();
            tmp_balanceNum = transform.Find("扩展/电子天平面板/Bg/称量值").GetComponent<TextMeshProUGUI>();
            button_couAff = transform.Find("扩展/电子天平面板/ButtonCouAff").GetComponent<Button>();
            medium_original = transform.Find("设备/压力称及其周边/培养基");
            medium_feed = transform.Find("扩展/培养基");
            cover = medium_feed.Find("盖子");
            liquid_beaker = beaker.Find("杯体").GetComponent<LiquidVolume>();
            spoon = transform.Find("扩展/勺子");
            anim_spoon = spoon.GetComponent<Animation>();
            dropArea_bigBalance = transform.Find("扩展/电子秤称量区域");
            scrubEffect_bigBalance = transform.Find("扩展/电子秤洁净效果").GetComponent<ScrubEffectComponent>();
            tmp_bigBalanceNum = transform.Find("扩展/电子秤面板/Bg/称量值").GetComponent<TextMeshProUGUI>();
            button_couAff2 = transform.Find("扩展/电子秤面板/ButtonCouAff").GetComponent<Button>();
            bucket = transform.Find("扩展/物料桶");
            bucket_F001S = transform.Find("设备/F001S");
            bucket_F001S_cover = bucket_F001S.Find("盖子");
            bucket_F001S_level = bucket.Find("物料");
            scoop = transform.Find("扩展/舀子");
            anim_scoop = scoop.GetComponent<Animation>();
            scoopParticle = scoop.Find("ParticleSystem").GetComponent<ParticleSystem>();
            powerButton_1 = transform.Find("设备/两个控制台组/控制台1/开关旋钮");
            plc_100L = transform.Find("PLC/100L配制罐").gameObject;
            plc_100LDispensing = transform.Find("PLC/100L配制罐/配制罐").GetComponent<PLC_100LDispensingTank>();
            usable_100L = transform.Find("扩展/100L配液罐控制面板").GetComponent<UsableComponent>();
            plcPipeFitting = plc_100LDispensing.pipeFitting;
            belly_100L = transform.Find("设备/罐子004/肚子").gameObject;
            fluidLevel_100L = transform.Find("设备/罐子004/4号培养管液体").GetComponent<FluidLevelComponent>();
            liquidVolume_100L = fluidLevel_100L.GetComponent<LiquidVolume>();
            waterColor = liquidVolume_100L.liquidColor1;
            cover_100L = transform.Find("设备/罐子004/投料口盖子004");
            blender_100L = transform.Find("设备/罐子004/4号培养罐_螺旋桨");
            entrance_100L = transform.Find("设备/罐子004/投料口").gameObject;
            bucket_feedF001S = transform.Find("扩展/F001S投料").gameObject;
            particle_feedF001S = transform.Find("扩展/F001S投料效果").GetComponent<ParticleSystem>();
            sanitaryPump_100L = transform.Find("设备/离心泵组/卫生离心泵");
            belly_100LReservoir = transform.Find("设备/罐子003/肚子").gameObject;
            fluidLevel_100LReservoir = transform.Find("设备/罐子003/3号培养管液体").GetComponent<FluidLevelComponent>();
            filter = transform.Find("设备/过滤器组/过滤器016").gameObject;
            console = transform.Find("设备/除菌过滤器组/操作台009").gameObject;
            filter_detect = transform.Find("设备/除菌过滤器组/过滤器091").gameObject;
            usable_detector = transform.Find("扩展/检测仪控制面板").GetComponent<UsableComponent>();
            plc_FilterDetector = transform.Find("PLC/检测仪").GetComponent<PLC_FilterDetector>();
            blender_100LReservoir = transform.Find("设备/罐子003/3号培养罐_螺旋桨");
            blender_200LReservoir = transform.Find("设备/罐子001/1号培养罐_螺旋桨");
            plc_100LReservoirTank = transform.Find("PLC/100L配制罐/储液罐").GetComponent<PLC_100LReservoirTank>();
            plcPipeFitting_Reservoir = plc_100LReservoirTank.pipeFitting;
            sampleBottle_100L = transform.Find("扩展/100L储液取样瓶").gameObject;
            passBox = transform.Find("设备/传递窗/箱体").gameObject;
            passboxDoor = transform.Find("设备/传递窗/窗门1").gameObject;
            passBoxBottle = transform.Find("扩展/传递窗内取样瓶1").gameObject;
            passBoxBottle_2 = transform.Find("扩展/传递窗内取样瓶2").gameObject;
            sampleValve_100L = transform.Find("阀门/培养基配制V029/培养基配制V0128");
            doorComponent = transform.Find("设备/传递窗").GetComponent<DoorComponent>();
            boxSampleBottle_1 = transform.Find("扩展/传递窗内取样瓶1").gameObject;
            fluid_sampleBottle_100L = transform.Find("扩展/传递窗内取样瓶1/瓶身").GetComponent<FluidLevelComponent>();
            connecter = transform.Find("设备/除菌过滤器组/连接线").gameObject;
            powerButton_2 = transform.Find("设备/两个控制台组/控制台002/开关旋钮001");
            plc_200L = transform.Find("PLC/200L配制罐").gameObject;
            plc_200LDispensing = transform.Find("PLC/200L配制罐/配制罐").GetComponent<PLC_200LDispensingTank>();
            plc_200LReservoirTank = transform.Find("PLC/200L配制罐/储液罐").GetComponent<PLC_200LReservoirTank>();
            usable_200L = transform.Find("扩展/200L配液罐控制面板").GetComponent<UsableComponent>();
            plcPipeFitting_2 = plc_200LDispensing.pipeFitting;
            plcPipeFitting_Reservoir_2 = plc_200LReservoirTank.pipeFitting;
            belly_200L = transform.Find("设备/罐子002/肚子").gameObject;
            belly_200LReservoir = transform.Find("设备/罐子001/肚子").gameObject;
            liquidVolume_200L = transform.Find("设备/罐子002/2号培养管液体").GetComponent<LiquidVolume>();
            fluidLevel_200L = transform.Find("设备/罐子002/2号培养管液体").GetComponent<FluidLevelComponent>();
            cover_200L = transform.Find("设备/罐子002/投料口盖子002");
            entrance_200L = transform.Find("设备/罐子002/投料口").gameObject;
            bucket_feed200L = transform.Find("扩展/200L投料").gameObject;
            particle_feed200L = transform.Find("扩展/200L投料效果").GetComponent<ParticleSystem>();
            blender_200L = transform.Find("设备/罐子002/2号培养罐_螺旋桨");
            sanitaryPump_200L = transform.Find("设备/离心泵组/卫生离心泵003");
            fluidLevel_200LReservoir = transform.Find("设备/罐子001/1号培养罐_液体").GetComponent<FluidLevelComponent>();
            filter_2 = transform.Find("设备/过滤器组/过滤器031").gameObject;
            sampleBottle_200L = transform.Find("扩展/200L储液取样瓶").gameObject;
            sampleValve_200L = transform.Find("阀门/培养基配制V047/培养基配制V0110");
            fluid_passBoxBottle_2 = passBoxBottle_2.transform.Find("瓶身").GetComponent<FluidLevelComponent>();

            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
           //m_MouseOrbit = Camera.main.transform.GetComponent<MouseOrbit>();
            #region Manager
            Transform valveParent = transform.Find("阀门");//查找阀门父节点
            ValveManager.Instance.Init(RootDir + "Stage/培养基配制工段/培养基配制-阀门匹配表.xml", valveParent);
            Transform pipeParent = transform.Find("管道");//查找管道父节点
            Transform fluidParent = transform.Find("流体");//查找流体父节点
            PipeFittingManager.Instance.Init(RootDir + "Stage/培养基配制工段/培养基配制-流体配置表.xml", pipeParent, fluidParent);
            Transform guideParent = transform.Find("引导");//查找引导父节点
            ProductionGuideManager.Instance.Init(RootDir + "Stage/培养基配制工段/培养基配制-引导匹配表.xml", guideParent);
            Transform bestAngleParent = transform.Find("最佳视角");//查找最佳视角父节点
            LookPointManager.Instance.Init(bestAngleParent);
            EventDispatcher.ExecuteEvent<string>(Events.Status.Init, RootDir + "Stage/培养基配制工段/培养基配制-状态表.xml");
            FocusManager.Instance.Init(transform.Find("镜头"));
            #endregion
            #region 初始状态
            tmp_balanceNum.text = "0";
            tmp_bigBalanceNum.text = "0";
            beaker.gameObject.SetActive(false);
            medium_feed.gameObject.SetActive(false);
            //liquid_beaker.level = 0;
            spoon.gameObject.SetActive(false);
            bucket.gameObject.SetActive(false);
            scoop.gameObject.SetActive(false);
            plc_100L.GetComponent<GraphicRaycaster>().enabled = false;
            plc_100L.SetActive(false);
            fluidLevel_100L.Value = 0;
            bucket_feedF001S.gameObject.SetActive(false);
            fluidLevel_100LReservoir.Value = 0;
            filter_detect.gameObject.SetActive(false);
            plc_FilterDetector.GetComponent<GraphicRaycaster>().enabled = false;
            sampleBottle_100L.gameObject.SetActive(false);
            passBoxBottle.gameObject.SetActive(false);
            fluid_sampleBottle_100L.Value = 0.7f;
            connecter.gameObject.SetActive(false);
            plc_200L.GetComponent<GraphicRaycaster>().enabled = false;
            plc_200L.SetActive(false);
            sampleBottle_200L.SetActive(false);
            fluid_passBoxBottle_2.Value = 0.7f;
            bucket_feed200L.gameObject.SetActive(false);
            passBoxBottle_2.gameObject.SetActive(false);
            beakerContent.localScale = new Vector3(1,1,0);
            #endregion

            #region 记住相关物体的初始状态
            cache_cover_LocalPosition = cover.localPosition;
            cache_medium_feed_LocalEulerAngles = medium_feed.localEulerAngles;
            cache_beakerContent_localScale = beakerContent.localScale;
            beakerContent.gameObject.SetActive(false);
            cache_bucket_F001S_cover_localPosition = bucket_F001S_cover.localPosition;
            cache_bucket_F001S_level_localPosition = bucket_F001S_level.localPosition;
            cache_powerButton_1_LocalEulerAngles = powerButton_1.localEulerAngles;
            cache_cover_100L_LocalEulerAngles = cover_100L.localEulerAngles;
            cache_blender_100L_LocalEulerAngles = blender_100L.localEulerAngles;
            cache_sanitaryPump_100L_localPosition = sanitaryPump_100L.localPosition;
            cache_sampleValve_100L_LocalEulerAngles = sampleValve_100L.localEulerAngles;
            cache_powerButton_2_LocalEulerAngles = powerButton_2.localEulerAngles;
            cache_cover_200L_LocalEulerAngles = cover_200L.localEulerAngles;
            cache_blender_200L_LocalEulerAngles = blender_200L.localEulerAngles;
            cache_sanitaryPump_200L_localPosition = sanitaryPump_200L.localPosition;
            cache_sampleValve_200L_LocalEulerAngles = sampleValve_200L.localEulerAngles;
            #endregion
        }
    }
}
