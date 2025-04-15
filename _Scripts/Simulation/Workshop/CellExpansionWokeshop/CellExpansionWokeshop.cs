using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Common;
using XFramework.Actions;
using UnityEngine;
using XFramework.Module;

namespace XFramework.Simulation
{
    /// <summary>
    /// 细胞扩增车间
    /// </summary>
    public partial class CellExpansionWokeshop : CustomWorkshop
    {
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.CellExpansionWorkshop;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }
        /// <summary>
        /// 进入摇床面板设置次数
        /// </summary>
        private int enterRockBottleSetPanel = 0;
        /// <summary>
        ///   误操作 计数
        /// </summary>
        private int misoperationHandlerCount = 0;
        /// <summary>
        /// 紫外开关打开前参数设置完回调
        /// </summary>
        /// <param name="time"></param>
        private void UltravioletSetPanelSaveEvent_callBack(string time)
        {
            if (!time.Equals("30"))
            {
                misoperationHandlerCount++;
            }

            Task.NewTask()
                .Append(new UpdateSmallAction("1-2-3", true))
                .Append(new InvokeCurrentGuideAction(4))
                .Append(new GameObjectAction(lookPointOne))
                .Append(new InvokeFlashAction(true, purpleSwitch.gameObject))
                .Execute();
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
            }
            misoperationHandlerCount = 0;
        }

        private void RockBottleSetPanelSaveEvent_callBack(string speed, string time, string tem)
        {
            enterRockBottleSetPanel++;
           // int speedValue;
            bool isExitSpeed= int.TryParse(speed,out int speedValue);
            bool isSpeedRight = false;
            if (isExitSpeed && speedValue <= 40 && speedValue>=30)
            {
                isSpeedRight = true;
            }
            if (!tem.Equals("37"))
            {
                misoperationHandlerCount++;
            }
            if (!time.Equals("5"))
            {
                misoperationHandlerCount++;
            }
            if (!isSpeedRight)
            {
                misoperationHandlerCount++;
            }

            if (tem.Equals("37") && time.Equals("5") && isSpeedRight)
            {
                Task.NewTask()
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "培养温度设定为37℃，培养转速"+speed+ ",培养时间5d", Color.green))
                .Execute();
            }
            else
            {
                Task.NewTask()
                    .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "培养温度设定为" + tem + "℃，培养转速" + speed + "r/min,"+ "培养时间"+time+"d", Color.red))
                    .Execute();             
            }
            if (enterRockBottleSetPanel==1)
            {
                Task.NewTask()
                .Append(new InvokeCurrentGuideAction(5))
                .Append(new UpdateSmallAction("3-2-4", true))
                .Execute();
            }
            else if (enterRockBottleSetPanel == 2)
            {
                Task.NewTask()
                    .Append(new InvokeCurrentGuideAction(10))
                    .Append(new UpdateSmallAction("3-3-9", true))
                    .Execute();               
            }
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
            }
          

            misoperationHandlerCount = 0;
        }
        private void BigCentrifugeSaveEvent_callBack(string speed, string time,string tem)
        {
            if (!speed.Equals("1000"))
            {
                misoperationHandlerCount++;
            }
            if (!time.Equals("2"))
            {
                misoperationHandlerCount++;
            }
            if (!tem.Equals("25"))
            {
                misoperationHandlerCount++;
            }
            if (speed.Equals("1000") && time.Equals("3") && tem.Equals("25"))
            {
                Task.NewTask()
                    .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "离心转速设定为1000r/min，离心时间3min,离心温度25℃", Color.green))
                    .Execute();
            }
            else
            {
                Task.NewTask()
                   .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "离心转速设定为" + speed + "r/min，离心时间" + time + "min，离心温度" + tem + "℃", Color.red))
                   .Execute();
            }
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
            }
            misoperationHandlerCount = 0;
            Task.NewTask()
                .Append(new UpdateSmallAction("3-1-9", true))
                .Append(new InvokeCurrentGuideAction(10))
                .Append(new InvokeFlashAction(true, greaterCentrifugeCap.gameObject))
                .Execute();
            bigCentrifugeSpeedText.text = speed;
            bigCentrifugeSpeedTextSet.text = "设置转速：" + speed + "RPM";
            bigCentrifugeTimeText.text = time;
            bigCentrifugeTimeTextSet.text = "设置时间：" + time + "min";
            bigCentrifugeTemText.text = tem;
            bigCentrifugeTemTextSet.text = "设置温度：" + tem + "°C";

        }

        private void SmallCentrifugeSaveEvent_callBack(string speed, string time)
        {
            if (!speed.Equals("1000"))
            {
                misoperationHandlerCount++;
            }
            if (!time.Equals("3"))
            {
                misoperationHandlerCount++;
            }
            if (speed.Equals("1000") && time.Equals("3"))
            {
                Task.NewTask()
                .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "离心转速设定为1000r/min，离心时间3min", Color.green))
                .Execute();
            }
            else
            {
                Task.NewTask()
                   .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "离心转速设定为" + speed + "r/min，离心时间" + time + "min", Color.red))
                   .Execute();
            }
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
                Debug.Log("扣分啦");
            }
            Task.NewTask()
                .Append(new UpdateSmallAction("2-1-8", true))
                .Append(new InvokeCurrentGuideAction(9))
                .Append(new InvokeFlashAction(true, centrifugeCap.gameObject))
                .Execute();
            smallCentrifugeSpeedText.text = speed;
            smallCentrifugeTimeText.text = time;
           
            misoperationHandlerCount = 0;
        }
        private void SaveEvent_callBack(string tem, string time)
        {
            if (!tem.Equals("37"))
            {
                misoperationHandlerCount++;
            }
            if (!time.Equals("10"))
            {
                misoperationHandlerCount++;
            }
            if (tem.Equals("37") && time.Equals("10"))
            {
                Task.NewTask()
                    .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "水浴温度设定为37℃，水浴时间10min", Color.green))
                    .Execute();
            }
            else
            {
                Task.NewTask()
                   .Append(new ShowHUDTextAction(Utils.NewGameObject().transform, "水浴温度设定为" + tem + "℃，水浴时间" + time + "min", Color.red))
                   .Execute();
            }
            for (int i = 0; i < misoperationHandlerCount; i++)
            {
                MisoperationHandler("");
            }
            misoperationHandlerCount = 0;
            Task.NewTask()
               .Append(new UpdateSmallAction("1-1-4", true))
               .Append(new InvokeCompletedAction(1, 1))
               .Append(new InvokeCurrentGuideAction(1))
               .Append(new InvokeFlashAction(true, glassDoor.gameObject))
               .Execute();
            potTemText.text = tem;

           
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
            //m_CameraSwitcher.Switch(CameraStyle.Walk);
            Debug.Log("初始化大步骤 " + index);
            switch (index)
            {
                case 1:
                    foreach (Transform item in mediumBottleParent)
                    {
                        item.gameObject.SetActive(true);
                    }
                    glassDoor.GetComponent<BoxCollider>().enabled = true;
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, doorRight.gameObject, doorLeft.gameObject))
                        .Append(new DOLocalRotaAction(doorLeft, new Vector3(0, 0, 0), 0.5f, true))
                        .Append(new DOLocalRotaAction(doorRight, new Vector3(0, 0, 0), 0.5f, true))//冰箱门关闭
                        .Append(new GameObjectAction(waterPotBottle,false))  //水浴锅培养基消失
                        .Append(new DOLocalMoveAction(waterPotBottle,new Vector3 (-37.563f, 8.5984f, -54.40446f),0.5f,true))
                        .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0f, 0), 0.5f,true)) //玻璃挡板复位
                        .Append(new GameObjectAction(goodPutArea.gameObject, false))
                        .Append(new GameObjectAction(goodsParent.gameObject,false)) //实验器材消失
                        .Append(new GameObjectAction(wateringCan,false)) //喷雾器消失
                        .Append(new DOLocalMoveAction(wateringCan,new Vector3 (-40.06181f, 8.494331f, -51.4f),0.5f,true))
                        .Append(new GameObjectAction(purpleLight, false)) //紫外灯光消失
                        .Append(new GameObjectAction(washCloth,false))
                        .Append(new DOLocalMoveAction(washCloth, new Vector3(-37.65f, 8.796f, -54.353f), 0.5f,true))
                        .Append(new GameObjectAction(deskBottle.gameObject,false))
                        .Append(new GameObjectAction(smallCentrifugeSetPanel.gameObject,false))
                        .Append(new UpdateGoodsAction(GoodsType.Cell_SYQC_01.ToString(), UpdateType.Add))
                        .Execute();

                    break;
                case 2:
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, windowDoor.gameObject))
                        .Append(new GameObjectAction(goodsParent.gameObject, true)) 
                        .Append(new GameObjectAction(goodPutArea.gameObject, false))
                        .Append(new DOLocalRotaAction(windowDoor, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalRotaAction(windowDoorHand, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new GameObjectAction(deskBoxPutArea,false))
                        .Append(new GameObjectAction(windowDoorBox, true))
                        .Append(new GameObjectAction(deskBox, false))
                        .Append(new DOLocalMoveAction(deskBox_Cap, new Vector3(0, 0, -0.015f), 0.5f,true))
                        .Append(new GameObjectAction(deskBox_Tube, true))
                        .Append(new DOLocalMoveAction(deskBox_Tube,new Vector3(-0.06230545f, -0.02962494f, -0.01819135f),0.5f,true))
                        .Append(new GameObjectAction(centrifuge_Tube01,false))
                        .Append(new DOLocalMoveAction(centrifuge_Tube01, new Vector3(-37.549f, 8.5842f, -54.394f), 0.5f, true))
                        .Append(new DOLocalRotaAction(centrifuge_Tube01, new Vector3(-90, 0, 0f), 0.5f, true))
                        .Append(new GameObjectAction(tweezers,false))
                        .Append(new DOLocalMoveAction(tweezers, new Vector3(-37.6497f, 8.6431f, -54.3927f), 0.5f,true))
                        .Append(new DOLocalRotaAction(centrifugeCap, new Vector3(0, 0, 0f), 0.5f,true))  //关闭离心机盖子
                        .Append(new DOLocalMoveAction(centrifugeCap_Inner, new Vector3(0f, -0.018f, -0.05f), 0.5f,true))
                        .Append(new GameObjectAction(centrifuge_Tube02,false))
                        .Append(new DOLocalMoveAction(centrifuge_Tube02, new Vector3(-39.17159f, 8.843f, -54.5121f), 0.5f,true))
                        .Append(new DOLocalRotaAction(centrifuge_Tube02, new Vector3(-90, 0, 0f), 0.5f,true))
                        .Append(new GameObjectAction(deskTube.gameObject,false))
                        .Execute();
                    break;
                case 3:
                    enterRockBottleSetPanel = 0;
                    glassDoor.GetComponent<BoxCollider>().enabled = false;
                    bigRockBottlFlu.Value = 0;
                    deskBottleFlu.Value = 0.5f;
                    centrifugeTubeFlu.Value = 0f;
                    smallRockBottlFlu.Value = 0;
                    bioMate_PipetteFlu.Value = 0;
                    Task.NewTask()
                        .Append(new InvokeFlashAction(true, gun_1ml.gameObject))
                        .Append(new DOLocalMoveAction(glassDoor, new Vector3(0, 0.25f, 0), 0.5f,true))
                        .Append(new GameObjectAction(deskTube.gameObject, true))
                        .Append(new DOLocalRotaAction(gun_1ml, new Vector3(-90, 0, 0), 0.5f, true))
                        .Append(new DOLocalMoveAction(gun_1ml, new Vector3(-40.26f, 8.427f, -50.9422f), 0.5f, true))
                        .Append(new DOLocalMoveAction(gunCap_1ml, new Vector3(-0.00829899f, 0f, 0.096f), 0.5f, true))
                        .Append(new GameObjectAction(gunNeedle_1ml, false))
                        .Append(new DOLocalRotaAction(gun_5ml, new Vector3(-90, 0, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(gun_5ml, new Vector3(-40.26f, 8.438686f, -51.0588f), 0.5f, true))
                        .Append(new DOLocalMoveAction(gunCap_5ml, new Vector3(-0.00829899f, 0f, 0.08187581f), 0.5f, true))
                        .Append(new DOLocalMoveAction(alcoholCap, new Vector3(-0.0096f, 0.0058f, 0.017f), 0.5f,true))
                        .Append(new GameObjectAction(alcoholFire,false))
                        .Append(new DOLocalMoveAction(needleBoxCap, new Vector3(0f, 0f, -0.1523f), 0.5f,true))
                        .Append(new DOLocalMoveAction(needleBox02, new Vector3(-40.28911f, 8.388041f, -51.25261f), 0.5f,true))
                        .Append(new GameObjectAction(gunNeedle_5ml,false))
                        .Append(new DOLocalRotaAction(deskTubeCap, new Vector3(0f, 0, 0f), 0.5f, true))
                        .Append(new DOLocalMoveAction(deskTubeCap, new Vector3(0f, 0f, 0.01397461f), 0.5f,true))
                        .Append(new DOLocalRotaAction(deskTubeSelf, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(deskTube, new Vector3(-39.952f, 8.2974f, -51.369f), 0.5f, true))
                        .Append(new GameObjectAction(deskTubeLiquid_White, true))
                        .Append(new GameObjectAction(deskTubeLiquid_Yellow, false))
                        .Append(new GameObjectAction(wasterNeedle_01, false))
                        .Append(new GameObjectAction(wasterNeedle_02, false))
                        .Append(new GameObjectAction(wasterNeedle_03, false))
                        .Append(new GameObjectAction(wasterNeedle_04, false))
                        .Append(new GameObjectAction(wasterNeedle_05, false))
                        .Append(new GameObjectAction(wasterNeedle_06, false))
                        .Append(new GameObjectAction(wasterNeedle_07, false))
                        .Append(new GameObjectAction(deskBottle, true))
                        .Append(new DOLocalMoveAction(deskBottle, new Vector3(-40.115f, 8.4644f, -51.684f), 0.5f,true))
                        .Append(new DOLocalMoveAction(deskBottleCap, new Vector3(0f, 0f, 0f), 0.5f,true))       //培养基塞子复位
                        .Append(new DOLocalRotaAction(deskBottleCap,new Vector3 (0,0,0),0.5f,true))
                      //  .Append(new CoroutineFluidLevelAction(deskBottleFlu.gameObject,0.5f,0.5f,true))
                        //.Append(new GameObjectAction(centrifugeTube, false))
                        .Append(new DOLocalMoveAction(centrifugeTube, new Vector3(-40.00639f, 8.380333f, -51.8f), 0.5f,true))  //离心管复位          
                        .Append(new DOLocalMoveAction(centrifugeTubeCap, new Vector3(0f, 0f, 0f), 0.5f, true))
                        .Append(new DOLocalRotaAction(centrifugeTubeSelf, new Vector3(0, 0, 0), 0.5f, true))
                       // .Append(new CoroutineFluidLevelAction(centrifugeTubeFlu.gameObject, 0f, 0.5f, true))
                        .Append(new DOLocalRotaAction(greaterCentrifugeCap, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(greaterCentrifugeRotateCap, new Vector3(0.08f, 0, 0.1f), 0.5f,true))
                        .Append(new GameObjectAction(greaterCentrifugeTube_01,false))
                        .Append(new DOLocalRotaAction(greaterCentrifugeTube_01, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(greaterCentrifugeTube_01, new Vector3(0.0334f, 0, 0.4126f), 0.5f,true))
                        .Append(new GameObjectAction(greateCentrifugeTube_02,false))
                        .Append(new DOLocalRotaAction(greateCentrifugeTube_02, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(greateCentrifugeTube_02, new Vector3(0.044f, 0, 0.41f), 0.5f,true))
                        .Append(new GameObjectAction(goodPutArea,false))
                        .Append(new GameObjectAction(lookPointTwo, false))
                        .Append(new GameObjectAction(smallRockBottl, true))
                        .Append(new DOLocalMoveAction(smallRockBottl, new Vector3(-40.25f, 8.430017f, -51.592f), 0.5f,true))//小摇瓶复位
                        .Append(new DOLocalMoveAction(smallRockBottlCap, new Vector3(0.0025f, 0f, 0.0323f), 0.5f,true))
                        .Append(new DOLocalRotaAction(smallRockBottlCap, new Vector3(-90f, 0f, 90f), 0.5f, true))
                       // .Append(new CoroutineFluidLevelAction(smallRockBottlFlu.gameObject, 0f, 0.5f,true))

                        .Append(new DOLocalMoveAction(bioMate, new Vector3(0.009054688f, -0.005546875f, 0.0039375f), 0.5f,true))
                        .Append(new DOLocalRotaAction(bioMate, new Vector3(90, 0, 0), 0.5f,true))
                        .Append(new GameObjectAction(bioMate))
                        .Append(new GameObjectAction(bioMateNew,false))
                        .Append(new DOLocalMoveAction(bioMateNew, new Vector3(0.0011f, 0.0049f, 0.26f), 0.5f, true))
                        .Append(new DOLocalRotaAction(bioMateNew, new Vector3(90, 0, 0), 0.5f, true))

                        .Append(new DOLocalRotaAction(singlePipetteBag_01, new Vector3(-90, 0, 180), 0.5f,true))
                        .Append(new DOLocalRotaAction(singlePipetteBag_02, new Vector3(-90, 0, 180), 0.5f, true))
                        .Append(new DOLocalRotaAction(singlePipetteBag_03, new Vector3(-90, 0, 180), 0.5f, true))
                        .Append(new GameObjectAction(pipette_01, false))
                        .Append(new GameObjectAction(singlePipetteSelf_01, true))
                        .Append(new GameObjectAction(pipette_02, false))
                        .Append(new GameObjectAction(singlePipetteSelf_02, true))
                        .Append(new GameObjectAction(pipette_03, false))
                        .Append(new GameObjectAction(singlePipetteSelf_03, true))
                        .Append(new DOLocalMoveAction(pipette_01, new Vector3(-39.9716f, 8.2782f, -50.9078f), 0.5f,true))
                        .Append(new DOLocalRotaAction(pipette_01, new Vector3(180, 90, 0), 0.5f,true))
                        .Append(new DOLocalMoveAction(pipette_02, new Vector3(-39.9716f, 8.2782f, -50.8642f), 0.5f, true))
                        .Append(new DOLocalRotaAction(pipette_02, new Vector3(180, 90, 0), 0.5f, true))
                        .Append(new DOLocalMoveAction(pipette_03, new Vector3(-39.9716f, 8.2782f, -50.8178f), 0.5f, true))
                        .Append(new DOLocalRotaAction(pipette_03, new Vector3(180, 90, 0), 0.5f, true))
                        .Append(new DOLocalMoveAction(singlePipette_01, new Vector3(-40.068f, 8.2749f, -50.908f), 0.5f, true))
                        .Append(new DOLocalRotaAction(singlePipette_01, new Vector3(0, 0, 0), 0.5f))
                        .Append(new DOLocalMoveAction(singlePipette_02, new Vector3(-40.068f, 8.2749f, -50.863f), 0.5f, true))
                        .Append(new DOLocalRotaAction(singlePipette_02, new Vector3(0, 0, 0), 0.5f))
                        .Append(new DOLocalMoveAction(singlePipette_03, new Vector3(-40.068f, 8.2749f, -50.817f), 0.5f, true))
                        .Append(new DOLocalRotaAction(singlePipette_03, new Vector3(0, 0, 0), 0.5f))

                        .Append(new GameObjectAction(bioMate_PipetteFlu.gameObject,false))
                       // .Append(new CoroutineFluidLevelAction(bioMate_PipetteFlu.gameObject, 0f, 0.5f,true))
                        .Append(new GameObjectAction(wasterPipette01.gameObject,false))
                        .Append(new DOLocalRotaAction(wasterPipette01,new Vector3 (90,180,-90),0.5f,true))
                        .Append(new DOLocalMoveAction(wasterPipette01,new Vector3 (-39.90438f, 8.712f, -51.52956f),0.5f,true))
                        .Append(new GameObjectAction(wasterPipette02.gameObject, false))
                        .Append(new DOLocalRotaAction(wasterPipette02, new Vector3(90, 180, -90), 0.5f, true))
                        .Append(new DOLocalMoveAction(wasterPipette02, new Vector3(-39.91178f, 8.787001f, -51.56236f), 0.5f, true))
                        .Append(new GameObjectAction(wasterPipette03.gameObject, false))
                        .Append(new DOLocalRotaAction(wasterPipette03, new Vector3(90, 180, -90), 0.5f, true))
                        .Append(new DOLocalMoveAction(wasterPipette03, new Vector3(-39.91178f, 8.787001f, -51.56236f), 0.5f, true))
                        .Append(new GameObjectAction(rock_SmallBottle,false))
                        .Append(new DOLocalMoveAction(rockPallent, new Vector3(0.1828086f, 0.085f, -0.4211353f), 0.5f,true))
                        .Append(new DOLocalMoveAction(rockDoor, new Vector3(0.1797656f, -0.3469102f, -0.15f), 0.5f,true))
                        .Append(new GameObjectAction(redLight, false))
                        .Append(new GameObjectAction(bigRockBottl, true))
                        .Append(new DOLocalMoveAction(bigRockBottl, new Vector3(-40.25f, 8.508017f, -51.749f), 0.5f,true))//大摇瓶复位
                        .Append(new DOLocalMoveAction(bigRockBottlCap, new Vector3(0.0026f, 0f, 0.0505f), 0.5f,true))
                        .Append(new DOLocalRotaAction(bigRockBottlCap, new Vector3(-90f, 0f, 90f), 0.5f, true))
                       // .Append(new CoroutineFluidLevelAction(bigRockBottlFlu.gameObject, 0f, 0.5f, true))
                        .Append(new GameObjectAction(rock_BigBottle,false))
                        .Append(new DOLocalRotaAction(passWindowDoor, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new DOLocalRotaAction(passWindowDoorHand, new Vector3(0, 0, 0), 0.5f,true))
                        .Append(new GameObjectAction(passWindowRockObj,false))
                        .Execute();
                    break;
                default:
                    break;
            }
        }
        protected override void OnRelease()
        {
            base.OnRelease();
        }
    }
}
