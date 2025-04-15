using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Component;
using TMPro;

namespace XFramework.Simulation
{
    /// <summary>
    /// 细胞扩增车间--组件定义
    /// </summary>
    public partial class CellExpansionWokeshop
    {
        // <summary>
        /// 相机切换
        /// </summary>
        private CameraSwitcher m_CameraSwitcher;

        /// <summary>
        /// MouseOrbit
        /// </summary>
        private MouseOrbit m_MouseOrbit;
        private Transform lookPointOne;
        private Transform lookPointTwo;
        /// <summary>
        /// 冰箱左门
        /// </summary>
        private Transform doorLeft;
        /// <summary>
        /// 冰箱右门
        /// </summary>
        private Transform doorRight;
        /// <summary>
        /// 冰箱中培养基瓶子父对象
        /// </summary>
        private Transform mediumBottleParent;
        /// <summary>
        /// 水浴锅
        /// </summary>
        private Transform waterPot;
        /// <summary>
        /// 玻璃挡板
        /// </summary>
        private Transform glassDoor;
        /// <summary>
        /// 实验器材放置处
        /// </summary>
        private Transform goodPutArea;
        /// <summary>
        /// 实验桌面培养基瓶
        /// </summary>
        private Transform deskBottle;

        /// <summary>
        /// 实验桌面培养基瓶盖子
        /// </summary>
        private Transform deskBottleCap;
        /// <summary>
        /// 实验桌面培养基瓶液体
        /// </summary>
        private FluidLevelComponent deskBottleFlu;
        /// <summary>
        /// 实验器材
        /// </summary>
        private Transform goodsParent;
        /// <summary>
        /// 紫外开关
        /// </summary>
        private Transform purpleSwitch;
        /// <summary>
        /// 照明按钮
        /// </summary>
        private Transform lightSwitch;
        /// <summary>
        /// 风机按钮
        /// </summary>
        private Transform windMachineSwitch;
        /// <summary>
        /// 水浴锅内培养基瓶
        /// </summary>
        private Transform waterPotBottle;
        /// <summary>
        /// 擦拭布
        /// </summary>
        private Transform washCloth;
        /// <summary>
        /// 传递窗门
        /// </summary>
        private Transform windowDoor;
        /// <summary>
        /// 传递窗门把手
        /// </summary>
        private Transform windowDoorHand;
        /// <summary>
        /// 传递窗冻存盒
        /// </summary>
        private Transform windowDoorBox;
        /// <summary>
        /// 桌面冻存盒放置处
        /// </summary>
        private Transform deskBoxPutArea;
        /// <summary>
        /// 桌面冻存盒
        /// </summary>
        private Transform deskBox;
        /// <summary>
        /// 桌面冻存盒盖子
        /// </summary>
        private Transform deskBox_Cap;
        /// <summary>
        /// 桌面冻存盒试管
        /// </summary>
        private Transform deskBox_Tube;
        /// <summary>
        /// 紫色灯光
        /// </summary>
        private Transform purpleLight;
        /// <summary>
        /// 白色灯光
        /// </summary>
        private Transform whiteLight;
        /// <summary>
        /// 离心机冻存管
        /// </summary>
        private Transform centrifuge_Tube01;
        private Transform centrifuge_Tube02;
        /// <summary>
        /// 离心机盖子
        /// </summary>
        private Transform centrifugeCap;
        /// <summary>
        /// 离心机内部盖子
        /// </summary>
        private Transform centrifugeCap_Inner;
        /// <summary>
        /// 镊子
        /// </summary>
        private Transform tweezers;
        /// <summary>
        /// 小型离心机开始按钮
        /// </summary>
        private Transform centrifugeStartButton;
        /// <summary>
        /// 生物安全柜冻存管
        /// </summary>
        private Transform deskTube;
        /// <summary>
        /// 生物安全柜冻存管 液体
        /// </summary>
        private Transform deskTubeLiquid_White;
        /// <summary>
        /// 生物安全柜冻存管 黄色液体
        /// </summary>
        private Transform deskTubeLiquid_Yellow;
        /// <summary>
        /// 生物安全柜冻存管盖子
        /// </summary>
        private Transform deskTubeCap;
        /// <summary>
        /// 生物安全柜冻存管 试管
        /// </summary>
        private Transform deskTubeSelf;
        /// <summary>
        /// 1ml移液枪
        /// </summary>
        private Transform gun_1ml;
        /// <summary>
        /// 1ml移液枪 枪杆头
        /// </summary>
        private Transform gunCap_1ml;
        /// <summary>
        /// 1ml移液枪针头
        /// </summary>
        private Transform gunNeedle_1ml;
        /// <summary>
        /// 1ml移液枪针头盖子
        /// </summary>
        private Transform needleBoxCap_01;
        /// <summary>
        /// 5ml移液枪
        /// </summary>
        private Transform gun_5ml;
        /// <summary>
        /// 5ml移液枪 枪杆头
        /// </summary>
        private Transform gunCap_5ml;
        /// <summary>
        /// 5ml移液枪针头
        /// </summary>
        private Transform gunNeedle_5ml;
        /// <summary>
        /// 5ml移液枪针头盖子
        /// </summary>
        private Transform needleBoxCap;
        /// <summary>
        /// 5ml移液枪针头盒子02
        /// </summary>
        private Transform needleBox02;
        /// <summary>
        /// 酒精灯盖子
        /// </summary>
        private Transform alcoholCap;
        /// <summary>
        /// 酒精灯火焰
        /// </summary>
        private Transform alcoholFire;
        /// <summary>
        /// 废弃针头01;
        /// </summary>
        private Transform wasterNeedle_01;
        /// <summary>
        /// 废弃针头02;
        /// </summary>
        private Transform wasterNeedle_02;
        /// <summary>
        /// 废弃针头03;
        /// </summary>
        private Transform wasterNeedle_03;
        /// <summary>
        /// 废弃针头04;
        /// </summary>
        private Transform wasterNeedle_04;
        /// <summary>
        /// 废弃针头05;
        /// </summary>
        private Transform wasterNeedle_05;
        /// <summary>
        /// 废弃针头06;
        /// </summary>
        private Transform wasterNeedle_06;
        /// <summary>
        /// 废弃针头07;
        /// </summary>
        private Transform wasterNeedle_07;

        /// <summary>
        /// 离心管
        /// </summary>
        private Transform centrifugeTube;

        /// <summary>
        /// 离心管盖子
        /// </summary>
        private Transform centrifugeTubeCap;
        /// <summary>
        /// 离心管液体
        /// </summary>
        private FluidLevelComponent centrifugeTubeFlu;
        /// <summary>
        /// 离心管试管本身
        /// </summary>
        private Transform centrifugeTubeSelf;

        /// <summary>
        /// 大型离心机盖子
        /// </summary>
        private Transform greaterCentrifugeCap;
        /// <summary>
        /// 大型离心机转子盖子
        /// </summary>
        private Transform greaterCentrifugeRotateCap;
        /// <summary>
        /// 大型离心机转子
        /// </summary>
        private Transform greaterCentrifugeRotate;
        /// <summary>
        /// 大型离心机离心按钮
        /// </summary>
        private Transform greaterCentrifugeButton;
        /// <summary>
        /// 大型离心机 离心管01
        /// </summary>
        private Transform greaterCentrifugeTube_01;
        /// <summary>
        /// 大型离心机 离心管02
        /// </summary>
        private Transform greateCentrifugeTube_02;
        /// <summary>
        /// 小摇瓶
        /// </summary>
        private Transform smallRockBottl;
        /// <summary>
        /// 小摇瓶盖子
        /// </summary>
        private Transform smallRockBottlCap;
        /// <summary>
        /// 小摇瓶液体
        /// </summary>
        private FluidLevelComponent smallRockBottlFlu;
        /// <summary>
        /// 电动移液器
        /// </summary>
        private Transform bioMate;
        /// <summary>
        /// 电动移液器 新
        /// </summary>
        private Transform bioMateNew;
        /// <summary>
        /// 电动移液器 移液管
        /// </summary>
        private FluidLevelComponent bioMate_PipetteFlu;
        /// <summary>
        /// 移液管
        /// </summary>
        private Transform pipette_01;
        /// <summary>
        /// 移液管
        /// </summary>
        private Transform pipette_02;
        /// <summary>
        /// 移液管
        /// </summary>
        private Transform pipette_03;
        /// <summary>
        /// 摇床门
        /// </summary>
        private Transform rockDoor;
        /// <summary>
        /// 摇床托盘
        /// </summary>
        private Transform rockPallent;
        /// <summary>
        /// 摇床里小摇瓶
        /// </summary>
        private Transform rock_SmallBottle;
        /// <summary>
        /// 摇床里大摇瓶
        /// </summary>
        private Transform rock_BigBottle;
        /// <summary>
        /// 摇床启动按钮
        /// </summary>
        private Transform rockStartButton;

        /// <summary>
        /// 大摇瓶
        /// </summary>
        private Transform bigRockBottl;
        /// <summary>
        /// 大摇瓶盖子
        /// </summary>
        private Transform bigRockBottlCap;
        /// <summary>
        /// 大摇瓶液体
        /// </summary>
        private FluidLevelComponent bigRockBottlFlu;
        /// <summary>
        /// 传递窗门
        /// </summary>
        private Transform passWindowDoor;
        /// <summary>
        /// 传递窗门把手
        /// </summary>
        private Transform passWindowDoorHand;
        /// <summary>
        /// 传递窗物品放置处
        /// </summary>
        private Transform passWindow_PutArea;
        /// <summary>
        /// 传递窗物品
        /// </summary>
        private Transform passWindowRockObj;

        /// <summary>
        /// 水浴锅参数设置面板
        /// </summary>
        private PotSetPanel potSetPanel;
        /// <summary>
        /// 小型离心机参数设置面板
        /// </summary>
        private SmallCentrifugeSetPanel smallCentrifugeSetPanel;

        private TextMeshPro smallCentrifugeSpeedText;
        private TextMeshPro smallCentrifugeTimeText;
        /// <summary>
        /// 大型离心机参数设置面板
        /// </summary>
        private CentrifugeSetPanel bigCentrifugeSetPanel;
        private TextMeshPro bigCentrifugeSpeedText;
        private TextMeshPro bigCentrifugeTimeText;
        private TextMeshPro bigCentrifugeTemText;

        private TextMeshPro bigCentrifugeSpeedTextSet;
        private TextMeshPro bigCentrifugeTimeTextSet;
        private TextMeshPro bigCentrifugeTemTextSet;
        /// <summary>
        /// 摇床参数设置面板
        /// </summary>
        private RockBottleSetPanel rockBottleSetPanel;
        /// <summary>
        /// 紫外参数设置面板
        /// </summary>
        private UltravioletSetPanel ultravioletSetPanel;

        private TextMeshPro potTemText;
        /// <summary>
        /// 乙醇抹布
        /// </summary>
        private Transform cloth;
        /// <summary>
        /// 酒精喷壶
        /// </summary>
        private Transform wateringCan;
        /// <summary>
        /// 一次性移液管01
        /// </summary>
        private Transform singlePipette_01;
        /// <summary>
        /// 一次性移液管02
        /// </summary>
        private Transform singlePipette_02;
        /// <summary>
        /// 一次性移液管03
        /// </summary>
        private Transform singlePipette_03;
        /// <summary>
        /// 一次性移液管01 袋子口
        /// </summary>
        private Transform singlePipetteBag_01;
        /// <summary>
        /// 一次性移液管02 袋子口
        /// </summary>
        private Transform singlePipetteBag_02;
        /// <summary>
        /// 一次性移液管03 袋子口
        /// </summary>
        private Transform singlePipetteBag_03;
        /// <summary>
        /// 一次性移液管01 管子
        /// </summary>
        private Transform singlePipetteSelf_01;
        /// <summary>
        /// 一次性移液管02 管子
        /// </summary>
        private Transform singlePipetteSelf_02;
        /// <summary>
        /// 一次性移液管03 管子
        /// </summary>
        private Transform singlePipetteSelf_03;
        /// <summary>
        /// 丢弃移液管01
        /// </summary>
        private Transform wasterPipette01;
        /// <summary>
        /// 丢弃移液管02
        /// </summary>
        private Transform wasterPipette02;
        /// <summary>
        /// 丢弃移液管03
        /// </summary>
        private Transform wasterPipette03;
        /// <summary>
        /// 摇床红色指示灯
        /// </summary>
        private Transform redLight;

        private void InitializeComponent()
        {
            redLight = transform.Find("扩展/摇床灯");
            redLight.gameObject.SetActive(false);
            wasterPipette01 = transform.Find("扩展/实验器材/丢弃移液管01");
            wasterPipette02 = transform.Find("扩展/实验器材/丢弃移液管02");
            wasterPipette03 = transform.Find("扩展/实验器材/丢弃移液管03");
            wasterPipette01.gameObject.SetActive(false);
            wasterPipette02.gameObject.SetActive(false);
            wasterPipette03.gameObject.SetActive(false);
            singlePipette_01 = transform.Find("扩展/实验器材/一次性移液管01");
            singlePipette_02 = transform.Find("扩展/实验器材/一次性移液管02");
            singlePipette_03 = transform.Find("扩展/实验器材/一次性移液管03");
            singlePipetteBag_01 = transform.Find("扩展/实验器材/一次性移液管01/移液管_袋子口子");
            singlePipetteBag_02 = transform.Find("扩展/实验器材/一次性移液管02/移液管_袋子口子");
            singlePipetteBag_03 = transform.Find("扩展/实验器材/一次性移液管03/移液管_袋子口子");
            singlePipetteSelf_01 = transform.Find("扩展/实验器材/一次性移液管01/移液管");
            singlePipetteSelf_02 = transform.Find("扩展/实验器材/一次性移液管02/移液管");
            singlePipetteSelf_03 = transform.Find("扩展/实验器材/一次性移液管03/移液管");

            doorLeft = transform.Find("扩展/冰箱/左门/左门");
            doorRight = transform.Find("扩展/冰箱/右门/右门");
            mediumBottleParent = transform.Find("扩展/培养基冰箱");
            waterPot = transform.Find("实验设备/水浴锅/水浴锅_箱体");
            waterPotBottle = transform.Find("扩展/水浴锅培养基");
            waterPotBottle.gameObject.SetActive(false);

            glassDoor = transform.Find("扩展/玻璃窗/玻璃窗");
            goodPutArea = transform.Find("扩展/实验器材放置处");
            goodPutArea.gameObject.SetActive(false);

            goodsParent = transform.Find("扩展/实验器材");
            goodsParent.gameObject.SetActive(false);
            deskBottle = transform.Find("扩展/实验器材/培养基");
            deskBottle.gameObject.SetActive(false);

            deskBottleCap = transform.Find("扩展/实验器材/培养基/具塞三角瓶塞子");
            deskBottleFlu = transform.Find("扩展/实验器材/培养基/具塞三角瓶瓶身").GetComponent<FluidLevelComponent>();
            deskBottleFlu.Value = 0.5f;

            purpleSwitch = transform.Find("扩展/紫外开关");
            lightSwitch = transform.Find("扩展/照明开关");
            windMachineSwitch = transform.Find("扩展/风机开关");
            purpleLight = transform.Find("扩展/紫外灯开/灯");
            purpleLight.gameObject.SetActive(false);
            whiteLight = transform.Find("扩展/白灯开/灯");
            whiteLight.gameObject.SetActive(false);

            washCloth = transform.Find("扩展/擦拭布");
            washCloth.gameObject.SetActive(false);
            windowDoor = transform.Find("扩展/传递窗/传递窗门");
            windowDoorHand = transform.Find("扩展/传递窗/传递箱把手");
            windowDoorBox = transform.Find("扩展/冻存管盒");
            deskBoxPutArea = transform.Find("扩展/冻存管盒放置处");
            deskBoxPutArea.gameObject.SetActive(false);
            deskBox = transform.Find("扩展/桌面放置冻存管盒");
            deskBox.gameObject.SetActive(false);
            deskBox_Cap = transform.Find("扩展/桌面放置冻存管盒/冻存盒盖子");
            deskBox_Tube = transform.Find("扩展/桌面放置冻存管盒/1.8L_细胞冻存管020");

            centrifugeCap = transform.Find("实验设备/小型离心机/离心机盖子/离心机盖子");
            centrifugeCap_Inner = transform.Find("实验设备/小型离心机/转子盖子002");

            centrifuge_Tube01 = transform.Find("扩展/水浴锅冻存管01");
            centrifuge_Tube01.gameObject.SetActive(false);
            centrifuge_Tube02 = transform.Find("扩展/水浴锅冻存管02");
            centrifuge_Tube02.gameObject.SetActive(false);

            tweezers = transform.Find("扩展/镊子");
            tweezers.gameObject.SetActive(false);
            centrifugeStartButton = transform.Find("扩展/小型离心机开始按钮");
            deskTube = transform.Find("扩展/生物安全柜冻存管");
            deskTube.gameObject.SetActive(false);

            deskTubeLiquid_White = transform.Find("扩展/生物安全柜冻存管/1.8L_细胞冻存管020_管子/液体");
            deskTubeLiquid_Yellow = transform.Find("扩展/生物安全柜冻存管/1.8L_细胞冻存管020_管子/黄色液体");

            deskTubeCap = transform.Find("扩展/生物安全柜冻存管/1.8L_细胞冻存管020_盖子");
            deskTubeSelf = transform.Find("扩展/生物安全柜冻存管/1.8L_细胞冻存管020_管子");

            gun_5ml = transform.Find("扩展/实验器材/5ML移液枪");
            gun_5ml.GetOrAddComponent<ToolTipComponent>().CatchToolTip = "5ml移液枪";
            gunCap_5ml = transform.Find("扩展/实验器材/5ML移液枪/5ML移液枪_枪杆");

            gunNeedle_5ml = transform.Find("扩展/实验器材/5ML移液枪/5mlTip头");
            gunNeedle_5ml.gameObject.SetActive(false);
            needleBoxCap = transform.Find("扩展/实验器材/移液吸头盒2/5MLTIP盖子");

            gun_1ml = transform.Find("扩展/实验器材/1000ml移液枪");
            gun_1ml.GetOrAddComponent<ToolTipComponent>().CatchToolTip = "1ml移液枪";
            gunCap_1ml = transform.Find("扩展/实验器材/1000ml移液枪/100-1000ml移液枪_枪杆");

            gunNeedle_1ml = transform.Find("扩展/实验器材/1000ml移液枪/1ml枪头");
            gunNeedle_1ml.gameObject.SetActive(false);
            needleBoxCap_01 = transform.Find("扩展/实验器材/移液吸头盒1/1mlTIP盒盖子");


            alcoholCap = transform.Find("扩展/实验器材/酒精灯/酒精灯_灯帽");
            alcoholFire = transform.Find("扩展/实验器材/酒精灯/酒精灯_灯芯托/火焰");
            alcoholFire.gameObject.SetActive(false);
            needleBox02 = transform.Find("扩展/实验器材/移液吸头盒2");

            wasterNeedle_01 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头01");
            wasterNeedle_01.gameObject.SetActive(false);
            wasterNeedle_02 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头02");
            wasterNeedle_02.gameObject.SetActive(false);
            wasterNeedle_03 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头03");
            wasterNeedle_03.gameObject.SetActive(false);
            wasterNeedle_04 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头04");
            wasterNeedle_04.gameObject.SetActive(false);
            wasterNeedle_05 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头05");
            wasterNeedle_05.gameObject.SetActive(false);

            wasterNeedle_06 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头06");
            wasterNeedle_06.gameObject.SetActive(false);

            wasterNeedle_07 = transform.Find("扩展/实验器材/1000L烧杯/废弃针头07");
            wasterNeedle_07.gameObject.SetActive(false);


            centrifugeTube = transform.Find("扩展/实验器材/离心管");
            centrifugeTubeCap = transform.Find("扩展/实验器材/离心管/5ml离心管盖子");
            centrifugeTubeSelf = transform.Find("扩展/实验器材/离心管/5ml离心管");
            centrifugeTubeFlu = transform.Find("扩展/实验器材/离心管/5ml离心管").GetComponent<FluidLevelComponent>();
            centrifugeTubeFlu.Value = 0;

            greaterCentrifugeCap = transform.Find("实验设备/大型离心机/离心机盖子/离心机盖子");
            greaterCentrifugeRotateCap = transform.Find("实验设备/大型离心机/离心机转子盖子");
            greaterCentrifugeRotate = transform.Find("实验设备/大型离心机/离心机转子");
            greaterCentrifugeButton = transform.Find("实验设备/大型离心机/离心机 (2)_073_屏幕/离心机开始按钮");


            greaterCentrifugeTube_01 = transform.Find("实验设备/大型离心机/大型离心机_离心管1");
            greaterCentrifugeTube_01.gameObject.SetActive(false);
            greateCentrifugeTube_02 = transform.Find("实验设备/大型离心机/大型离心机_离心管2");
            greateCentrifugeTube_02.gameObject.SetActive(false);

            smallRockBottl = transform.Find("扩展/实验器材/摇瓶小");
            smallRockBottlCap = transform.Find("扩展/实验器材/摇瓶小/盖子");
            smallRockBottlFlu = transform.Find("扩展/实验器材/摇瓶小/瓶子").GetComponent<FluidLevelComponent>();
            smallRockBottlFlu.Value = 0;

            bioMate = transform.Find("扩展/实验器材/电动移液器/电动移液器");
            bioMateNew = transform.Find("扩展/实验器材/电动移液器/电动移液器新");
            bioMateNew.gameObject.SetActive(false);
            bioMate_PipetteFlu = transform.Find("扩展/实验器材/电动移液器/电动移液器新/电动移液器/移液管").GetComponent<FluidLevelComponent>();
            bioMate_PipetteFlu.gameObject.SetActive(false);
            pipette_01 = transform.Find("扩展/实验器材/移液管01");
            pipette_02 = transform.Find("扩展/实验器材/移液管02");
            pipette_03 = transform.Find("扩展/实验器材/移液管03");
            pipette_01.gameObject.SetActive(false);
            pipette_02.gameObject.SetActive(false);
            pipette_03.gameObject.SetActive(false);

            rockDoor = transform.Find("实验设备/摇床/门");
            rock_SmallBottle = transform.Find("实验设备/摇床/托盘/摇瓶小");
            rock_BigBottle = transform.Find("实验设备/摇床/托盘/摇瓶大");
            rockPallent = transform.Find("实验设备/摇床/托盘");
            rockStartButton = transform.Find("实验设备/摇床/启动按钮");

            bigRockBottl = transform.Find("扩展/实验器材/摇瓶大");
            bigRockBottlCap = transform.Find("扩展/实验器材/摇瓶大/盖子");
            bigRockBottlFlu = transform.Find("扩展/实验器材/摇瓶大/瓶子").GetComponent<FluidLevelComponent>();
            bigRockBottlFlu.Value = 0;

            passWindowDoor  = transform.Find("实验设备/传递窗/传递窗门1");
            passWindowDoorHand = transform.Find("实验设备/传递窗/传递窗门1/传递窗把手");
            passWindow_PutArea = transform.Find("实验设备/传递窗/传递窗箱体");

            passWindowRockObj = transform.Find("实验设备/传递窗/摇瓶");
            passWindowRockObj.gameObject.SetActive(false);

            CameraLookPointManager.Instance.Init(transform.Find("镜头"));

           // ObservationPointManager.Instance.Init(transform.Find("视角拉近"));
            m_CameraSwitcher = Camera.main.transform.GetComponent<CameraSwitcher>();
            lookPointOne = transform.Find("视角拉近/观察生物安全柜开关按钮");
            lookPointOne.gameObject.SetActive(false);
            lookPointTwo = transform.Find("视角拉近/观察大型离心机面板");
            lookPointTwo.gameObject.SetActive(false);

            potSetPanel = transform.Find("扩展/PotSetPanel").GetComponent<PotSetPanel>();
            potSetPanel.SaveEvent.AddListener(SaveEvent_callBack);
            potSetPanel.gameObject.SetActive(false);
            potTemText = transform.Find("扩展/水浴锅温度值").GetComponent<TextMeshPro>();

            smallCentrifugeSetPanel = transform.Find("扩展/SmallCentrifugeSetPanel").GetComponent<SmallCentrifugeSetPanel>();
            smallCentrifugeSetPanel.SaveEvent.AddListener(SmallCentrifugeSaveEvent_callBack);
            smallCentrifugeSetPanel.gameObject.SetActive(false);
            smallCentrifugeSpeedText = transform.Find("扩展/小型离心机参数值/转速").GetComponent<TextMeshPro>();
            smallCentrifugeTimeText = transform.Find("扩展/小型离心机参数值/时间").GetComponent<TextMeshPro>();




            bigCentrifugeSetPanel = transform.Find("扩展/BigCentrifugeSetPanel").GetComponent<CentrifugeSetPanel>();
            bigCentrifugeSetPanel.SaveEvent.AddListener(BigCentrifugeSaveEvent_callBack);
            bigCentrifugeSetPanel.gameObject.SetActive(false);

            bigCentrifugeSpeedText = transform.Find("扩展/大型离心机参数值/转速").GetComponent<TextMeshPro>();
            bigCentrifugeTimeText = transform.Find("扩展/大型离心机参数值/时间").GetComponent<TextMeshPro>();
            bigCentrifugeTemText = transform.Find("扩展/大型离心机参数值/温度").GetComponent<TextMeshPro>();

            bigCentrifugeSpeedTextSet = transform.Find("扩展/大型离心机参数值/转速设置").GetComponent<TextMeshPro>();
            bigCentrifugeTimeTextSet = transform.Find("扩展/大型离心机参数值/时间设置").GetComponent<TextMeshPro>();
            bigCentrifugeTemTextSet = transform.Find("扩展/大型离心机参数值/温度设置").GetComponent<TextMeshPro>();

            rockBottleSetPanel = transform.Find("扩展/RockBottleSetPanel").GetComponent<RockBottleSetPanel>();
            rockBottleSetPanel.SaveEvent.AddListener(RockBottleSetPanelSaveEvent_callBack);
            rockBottleSetPanel.gameObject.SetActive(false);

            ultravioletSetPanel = transform.Find("扩展/UltravioletSetPanel").GetComponent<UltravioletSetPanel>();
            ultravioletSetPanel.SaveEvent.AddListener(UltravioletSetPanelSaveEvent_callBack);
            ultravioletSetPanel.gameObject.SetActive(false);


            cloth = transform.Find("扩展/乙醇抹布");
            cloth.gameObject.SetActive(false);
            wateringCan = transform.Find("扩展/喷雾器");
            wateringCan.gameObject.SetActive(false);

            #region Manager
            Transform bestAngleParent = transform.Find("最佳视角");//查找最佳视角父节点
            LookPointManager.Instance.Init(bestAngleParent);
            Transform guideParent = transform.Find("引导");//查找引导父节点
            ProductionGuideManager.Instance.Init(RootDir + "Stage/细胞扩增工段/细胞扩增工段-引导匹配表.xml", guideParent);
          
            #endregion
        }

    }
}
