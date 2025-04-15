using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Component;
using XFramework.Simulation.Component;

namespace XFramework.Simulation
{
   public partial class CellCultivateWorkshop
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
        /// PH电极
        /// </summary>
        private Transform PHPole;
        /// <summary>
        /// PH电极线
        /// </summary>
        private Transform PHPoleLine;
        /// <summary>
        /// 温度电极
        /// </summary>
        private Transform TPole;
        /// <summary>
        /// 温度电极线
        /// </summary>
        private Transform TPoleLine;
        /// <summary>
        /// DO电极
        /// </summary>
        private Transform DOPole;
        /// <summary>
        /// DO电极线
        /// </summary>
        private Transform DOPoleLine;
        /// <summary>
        /// 搅拌电极
        /// </summary>
        private Transform motor;
        /// <summary>
        /// 3L培养罐
        /// </summary>
        private Transform Tank3L;
        /// <summary>
        /// 3L培养罐 液体
        /// </summary>
        private FluidLevelComponent Tank3L_Flu;
        /// <summary>
        /// 3L培养罐 流水特效
        /// </summary>
        private Transform Tank3L_water;
        /// <summary>
        /// 3L培养罐 夹套外壳
        /// </summary>
        private Transform Tank3L_Shell;
        /// <summary>
        /// 3L培养罐 电热毯
        /// </summary>
        private Transform Tank3L_Blanket;
        /// <summary>
        /// 3L锡纸
        /// </summary>
        private Transform Tank3L_Paper;
        /// <summary>
        /// 输液管
        /// </summary>
        private Transform infusionTube;
        /// <summary>
        /// 输液管连接线
        /// </summary>
        private Transform infusionTubeLine;
       
        /// <summary>
        /// 输液管锡纸
        /// </summary>
        private Transform infusionTube_Paper;
        /// <summary>
        /// 输液管
        /// </summary>
        private Transform infusionTube_02;
        /// <summary>
        /// 输液管 液体
        /// </summary>
        private Transform infusionTube_02Flu;
        /// <summary>
        /// 输液管连接线
        /// </summary>
        private Transform infusionTubeLine_02;
        /// <summary>
        /// 输液管连接线  材质
        /// </summary>
        private UpdateMaterialComponent m_UpdateMaterialComponent;

        /// <summary>
        /// 输液管锡纸
        /// </summary>
        private Transform infusionTube_Paper02;
        /// <summary>
        /// 灭菌锅
        /// </summary>
        private Transform autoclave;
        /// <summary>
        /// 灭菌锅盖子
        /// </summary>
        private Transform autoclave_Cap;
        /// <summary>
        /// 灭菌锅培养罐
        /// </summary>
        private Transform autoclave_Tank3L;

        private DigitalMeterComponent autoclaveDMComponent;
        private Transform autoclave_Tip;
        private Transform tank3LPanel;
        /// <summary>
        /// 试验台桌面
        /// </summary>
        private Transform desk;
        /// <summary>
        /// 各种控制器
        /// </summary>
        private Transform variousControllers;
        /// <summary>
        /// 传递窗门
        /// </summary>
        private Transform passWindowDoor;
        /// <summary>
        /// 传递窗把手
        /// </summary>
        private Transform passWindowHand;
        /// <summary>
        /// 传递窗细胞培养液瓶
        /// </summary>
        private Transform passWindowBottle;
        /// <summary>
        /// 桌面培养基瓶
        /// </summary>
        private Transform deskBottle_01;
        /// <summary>
        /// 桌面培养基瓶 液体
        /// </summary>
        private FluidLevelComponent deskBottle_01Flu;
        /// <summary>
        /// 桌面培养基瓶 盖子
        /// </summary>
        private Transform deskBottle_01Cap;
        /// <summary>
        /// 桌面细胞液瓶
        /// </summary>
        private Transform deskBottle_02;
        /// <summary>
        /// 桌面细胞液瓶 液体
        /// </summary>
        private FluidLevelComponent deskBottle_02Flu;
        /// <summary>
        /// 桌面细胞液瓶 盖子
        /// </summary>
        private Transform deskBottle_02Cap;
        /// <summary>
        /// 蠕动泵开关
        /// </summary>
        private Transform pumpStartPower;
        /// <summary>
        /// 蠕动泵开关  自发光
        /// </summary>
        private Transform pumpStartPowerOpen;
        /// <summary>
        /// 酒精灯灯帽
        /// </summary>
        private Transform alcoholLampCap;
        /// <summary>
        /// 酒精灯火焰特效
        /// </summary>
        private Transform alcoholLampFire;
        /// <summary>
        /// 输液管流体
        /// </summary>
        private FluidComponent fluidComponent;
        /// <summary>
        /// 电极烧杯
        /// </summary>
        private Transform breaker;
        /// <summary>
        /// 3L培养罐控制柜开关按钮
        /// </summary>
        private Transform Tank3L_Power;
        /// <summary>
        /// 3L培养罐控制柜 plc
        /// </summary>
        private Transform Tank3L_PLC;
        /// <summary>
        /// 20L培养罐控制柜 plc
        /// </summary>
        private Transform Tank20L_PLC;
        /// <summary>
        /// 200L培养罐控制柜 plc
        /// </summary>
        private Transform Tank200L_PLC;
        /// <summary>
        /// 注射器
        /// </summary>
        private Transform injector;
        /// <summary>
        /// 注射器
        /// </summary>
        private Transform injectorParent;
        /// <summary>
        /// 注射器液体
        /// </summary>
        private Transform injectorFlu;
        /// <summary>
        /// QA  路径管理
        /// </summary>
        private WorkShopPathManager workshopPath;
        /// <summary>
        /// 取样及空气软管
        /// </summary>
        private Transform softPipe;
        /// <summary>
        /// 取样瓶
        /// </summary>
        private Transform sampleBottle;
        /// <summary>
        /// 取样区域
        /// </summary>
        private Transform sampleBottle_PutArea;
        /// <summary>
        /// 火焰线圈
        /// </summary>
        private Transform fireCircle;

        private FluidLevelComponent sampleBottleFlu;
        /// <summary>
        /// 取样阀上
        /// </summary>
        private Transform V011Hand;
        /// <summary>
        /// 取样阀下
        /// </summary>
        private Transform V012Hand;

        /// <summary>
        /// 20L培养罐夹套
        /// </summary>
        private Transform Tank20L_Shell;
        /// <summary>
        /// 20L培养罐主体
        /// </summary>
        private Transform Tank20L_Self;

        private FluidLevelComponent Tank20LFlu;
        private Transform Tank20L_WaterEffect;
        private Transform Tank20L_WaterEffect_02;

        private Transform Tank200L_PowerButton;
        private Transform Tank20L_PowerButton;
        private Transform Tank20L_Screen;
        private Transform Tank200L_Screen;

        /// <summary>
        /// 200L培养罐夹套
        /// </summary>
        private Transform Tank200L_Shell;


        private FluidLevelComponent Tank200LFlu;
        private Transform Tank200L_WaterEffect;


        /// <summary>
        /// 200L取样瓶
        /// </summary>
        private Transform sampleBottle_200L;
        /// <summary>
        /// 200L取样区域
        /// </summary>
        private Transform sampleBottle_PutArea_200L;
        /// <summary>
        /// 200L火焰线圈
        /// </summary>
        private Transform fireCircle_200L;

        private FluidLevelComponent sampleBottleFlu_200L;
        /// <summary>
        /// 200L取样阀上方
        /// </summary>
        private Transform V030Hand;
        /// <summary>
        /// 200L取样阀下方
        /// </summary>
        private Transform V031Hand;
        /// <summary>
        /// 进料阀
        /// </summary>
        private Transform V040Hand;
        /// <summary>
        /// 出料阀
        /// </summary>
        private Transform V041Hand;
        /// <summary>
        /// 电机
        /// </summary>
        private Transform motor_Hand;
        /// <summary>
        /// 20L火焰线圈
        /// </summary>
        private Transform fireCycle_20;
        private Transform agitatorBlade3L;
        /// <summary>
        /// 20L罐子灭菌特效
        /// </summary>
        private Transform gasEffect_20L;
        /// <summary>
        /// 200L罐子灭菌特效
        /// </summary>
        private Transform gasEffect_200L;
        private PLC_20L pLC_20L;
        private PLC_200L pLC_200L;
        /// <summary>
        /// 接管机
        /// </summary>
        private Transform receiveMachinePanel;
        private Transform receiveMachineObj;
        private Transform receiveMachineAngle;

        /// <summary>
        /// 封管机
        /// </summary>
        private Transform tubeSealingMachinePanel;
        private Transform tubeSealingMachineObj;
        private void InitializeComponent()
        {
            tubeSealingMachinePanel = transform.Find("扩展/TubeSealingMachinePanel");
            tubeSealingMachinePanel.gameObject.SetActive(false);
            tubeSealingMachineObj = transform.Find("扩展/封管机");
            tubeSealingMachineObj.gameObject.SetActive(false);

            receiveMachinePanel = transform.Find("扩展/ReceiverMachinePanel");
            receiveMachinePanel.gameObject.SetActive(false);
            receiveMachineObj = transform.Find("扩展/接管机");
            receiveMachineObj.gameObject.SetActive(false);
            receiveMachineAngle = transform.Find("小视角/接管机视角");

            pLC_20L = transform.Find("20L主机/20L屏幕/20LPLC").GetComponent<PLC_20L>();
            pLC_200L = transform.Find("200L主机/200L屏幕001/200LPLC").GetComponent<PLC_200L>();
            gasEffect_20L = transform.Find("20L罐体/气体特效");
            gasEffect_20L.gameObject.SetActive(false);
            gasEffect_200L = transform.Find("200L罐体/气体特效");
            gasEffect_200L.gameObject.SetActive(false);
            agitatorBlade3L = transform.Find("3LPLC_装置/3L罐体/3L搅拌桨");


            fireCycle_20 = transform.Find("扩展/20L火焰线圈");
            fireCycle_20.gameObject.SetActive(false);


            V040Hand = transform.Find("细胞培养2_阀门/V040/进料阀handle");
            V041Hand = transform.Find("细胞培养2_阀门/V041/出料阀handle");

            motor_Hand = transform.Find("板框式压滤机/板框压滤机泵/循环泵002");


            V030Hand = transform.Find("细胞培养2_阀门/V030/卡箍隔膜阀(DN25)_handle003");
            V031Hand = transform.Find("细胞培养2_阀门/V031/卡箍隔膜阀(DN25)_handle002");



            sampleBottle_200L = transform.Find("扩展/200L取样瓶");
            sampleBottle_200L.gameObject.SetActive(false);
            sampleBottle_PutArea_200L = transform.Find("扩展/200L取样管取样处");
            fireCircle_200L = transform.Find("扩展/200L火焰线圈");
            fireCircle_200L.gameObject.SetActive(false);
            sampleBottleFlu_200L = transform.Find("扩展/200L取样瓶/适龄药瓶-主体").GetComponent<FluidLevelComponent>();

            Tank200L_PowerButton = transform.Find("200L主机/200L旋钮");
            Tank20L_PowerButton = transform.Find("20L主机/20L旋钮");
            Tank20L_Screen = transform.Find("20L主机/20L屏幕");
            Tank200L_Screen = transform.Find("200L主机/200L屏幕001");
            Tank20L_PLC = transform.Find("20L主机/20L屏幕/20LPLC");
           
            Tank200L_PLC = transform.Find("200L主机/200L屏幕001/200LPLC");
           

            Tank200L_Shell = transform.Find("200L罐体/200L培养罐");
            Tank200LFlu = transform.Find("200L罐体/液体").GetComponent<FluidLevelComponent>();
            Tank200L_WaterEffect = transform.Find("200L罐体/流体特效");
            Tank200L_WaterEffect.gameObject.SetActive(false);
         


            Tank20L_Shell = transform.Find("20L罐体/20L夹套002");
            Tank20L_Self = transform.Find("20L罐体/20L主体002");
            Tank20LFlu = transform.Find("20L罐体/液体").GetComponent<FluidLevelComponent>();
            Tank20L_WaterEffect = transform.Find("20L罐体/流体特效");
            Tank20L_WaterEffect.gameObject.SetActive(false);
            Tank20L_WaterEffect_02 = transform.Find("20L罐体/培养基流体特效");
            Tank20L_WaterEffect_02.gameObject.SetActive(false);


            workshopPath = transform.Find("路径管理").GetComponent<WorkShopPathManager>();
            motor = transform.Find("3L搅拌电机/3L搅拌电机/3L搅拌电机");

            PHPole = transform.Find("扩展/电极/3LPH电极");
            PHPoleLine = transform.Find("扩展/电极/3LPH电极/3LPH电极线");
            TPole = transform.Find("扩展/电极/3L温度电极");
            TPoleLine = transform.Find("扩展/电极/3L温度电极/3L温度电极线");
            DOPole = transform.Find("扩展/电极/3LDO电极");
            DOPoleLine = transform.Find("扩展/电极/3LDO电极/3LDO电极线");

            Tank3L = transform.Find("3LPLC_装置/3L罐体");
            Tank3L_Paper = transform.Find("扩展/3L锡纸");
            Tank3L_Paper.gameObject.SetActive(false);
            Tank3L_Flu = transform.Find("3LPLC_装置/3L罐体/罐体内胆").GetComponent<FluidLevelComponent>();
            Tank3L_Shell = transform.Find("3LPLC_装置/3L罐体/3L夹套");
            Tank3L_Blanket = transform.Find("3LPLC_装置/3L罐体/电热毯");
            Tank3L_water = transform.Find("3LPLC_装置/3L罐体/流体特效");
            Tank3L_water.gameObject.SetActive(false);

            infusionTube = transform.Find("扩展/输液管玻璃段");
            infusionTube.gameObject.SetActive(false);
            infusionTubeLine = transform.Find("扩展/输液管连接线");
            infusionTubeLine.gameObject.SetActive(false);
            infusionTube_Paper = transform.Find("扩展/输液管玻璃段/输液管锡纸");
            infusionTube_Paper.gameObject.SetActive(false);

            infusionTube_02 = transform.Find("扩展/输液管玻璃段2/输液管玻璃段2");
           
            infusionTube_02.gameObject.SetActive(false);
            infusionTube_02Flu = transform.Find("扩展/输液管玻璃段2/输液管玻璃段2/液体");
            infusionTube_02Flu.gameObject.SetActive(false);

            infusionTubeLine_02 = transform.Find("扩展/输液管连接线2");
            m_UpdateMaterialComponent = transform.Find("扩展/输液管连接线2").GetComponent<UpdateMaterialComponent>();
            infusionTubeLine_02.gameObject.SetActive(false);
            infusionTube_Paper02 = transform.Find("扩展/输液管玻璃段2/输液管玻璃段2/输液管锡纸");
            //infusionTube_Paper02.gameObject.SetActive(false);

            autoclave = transform.Find("立式压力蒸汽灭菌器");
            autoclave_Cap = transform.Find("立式压力蒸汽灭菌器/立式压力蒸汽灭菌器_盖子");

            autoclave_Tank3L = transform.Find("扩展/培养罐");
            autoclave_Tank3L.gameObject.SetActive(false);
            autoclaveDMComponent = transform.Find("立式压力蒸汽灭菌器/立式压力蒸汽灭菌器_屏幕/PVSet").GetComponent<DigitalMeterComponent>();
            autoclave_Tip = transform.Find("立式压力蒸汽灭菌器/立式压力蒸汽灭菌器_屏幕/Tip");
            autoclave_Tip.gameObject.SetActive(false);
            tank3LPanel = transform.Find("扩展/Tank3LPanel");
            tank3LPanel.gameObject.SetActive(false);

            desk = transform.Find("培养间实验边台001");
            variousControllers = transform.Find("扩展/各种控制器");
            variousControllers.gameObject.SetActive(false);

            passWindowDoor = transform.Find("传递窗01/传递箱门2");
            passWindowHand = transform.Find("传递窗01/传递箱门2/传递箱把手2");
            passWindowBottle = transform.Find("传递窗01/传递窗细胞培养瓶");

            deskBottle_01 = transform.Find("扩展/培养基瓶");
            deskBottle_01.gameObject.SetActive(false);
            deskBottle_02 = transform.Find("扩展/细胞液瓶");
            deskBottle_02.gameObject.SetActive(false);

            deskBottle_01Flu = transform.Find("扩展/培养基瓶/瓶子").GetComponent<FluidLevelComponent>();
            deskBottle_01Cap = transform.Find("扩展/培养基瓶/盖子");
            deskBottle_02Flu = transform.Find("扩展/细胞液瓶/瓶子").GetComponent<FluidLevelComponent>();
            deskBottle_02Cap = transform.Find("扩展/细胞液瓶/盖子");

            pumpStartPower = transform.Find("3LPLC_装置/蠕动泵/启动按钮");
            pumpStartPowerOpen = transform.Find("3LPLC_装置/蠕动泵/启动按钮/自发光");
            pumpStartPowerOpen.gameObject.SetActive(false);

            alcoholLampCap = transform.Find("扩展/酒精灯/酒精灯_灯帽");
            alcoholLampFire = transform.Find("扩展/酒精灯/酒精灯_灯芯/火焰");
            alcoholLampFire.gameObject.SetActive(false);

            fluidComponent = transform.Find("扩展/输液管连接线2").GetComponent<FluidComponent>();
            breaker = transform.Find("扩展/1000L烧杯001");

            Tank3L_Power = transform.Find("3LPLC_装置/3L旋钮");
            Tank3L_PLC = transform.Find("3LPLC_装置/3LPLC主机/3LPLC");
         //   Tank3L_PLC.gameObject.SetActive(false);


            injectorParent = transform.Find("3LPLC_装置/3L罐体/注射器");
            injector = transform.Find("3LPLC_装置/3L罐体/注射器/取样器主体2");
            injectorFlu = transform.Find("3LPLC_装置/3L罐体/注射器/液体");
            injectorFlu.gameObject.SetActive(false);
            softPipe = transform.Find("扩展/取样及空气软管连接线接口");
            softPipe.gameObject.SetActive(false);

            fireCircle = transform.Find("扩展/火焰线圈");
            fireCircle.gameObject.SetActive(false);

            sampleBottle = transform.Find("扩展/取样瓶");
            sampleBottle.gameObject.SetActive(false);
            sampleBottleFlu = transform.Find("扩展/取样瓶/适龄药瓶-主体").GetComponent<FluidLevelComponent>();

            sampleBottle_PutArea = transform.Find("扩展/20L取样管取样处");

            V011Hand = transform.Find("细胞培养2_阀门/V011/卡箍隔膜阀(DN15)_handle001");
            V012Hand = transform.Find("细胞培养2_阀门/V012/卡箍隔膜阀(DN15)_handle002");




            #region 镜头拉近
            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            #endregion
            #region Manager
            Transform bestAngleParent = transform.Find("最佳视角");//查找最佳视角父节点
            LookPointManager.Instance.Init(bestAngleParent);
            Transform guideParent = transform.Find("引导");//查找引导父节点
            ProductionGuideManager.Instance.Init(RootDir + "Stage/细胞培养/细胞培养工段-引导匹配表.xml", guideParent);
            CameraLookPointManager.Instance.Init(transform.Find("镜头"));
            Transform valveParent = transform.Find("细胞培养2_阀门");//查找管道父节点
            ValveManager.Instance.Init(RootDir + "Stage/细胞培养/细胞培养_阀门匹配表.xml", valveParent);

            Transform pipeParent = transform.Find("管道");//查找管道父节点
            Transform fluidParent = transform.Find("流体");//查找流体父节点
            PipeFittingManager.Instance.Init(RootDir + "Stage/细胞培养/细胞培养_流体配置表.xml", pipeParent, fluidParent);

            EventDispatcher.ExecuteEvent<string>(Events.Status.Init, RootDir + "Stage/细胞培养/细胞培养-状态表.xml");
            #endregion
        }
    }
}
