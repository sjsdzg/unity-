using DG.Tweening;
using LiquidVolumeFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;

namespace XFramework.Simulation
{
    /// <summary>
    /// 离心车间
    /// </summary>
    public partial class CultureMediumPreparationWorkshop : CustomWorkshop
    {
        public override EnumWorkshopType GetWorkshopType()
        {
            return EnumWorkshopType.CultureMediumPreparationWorkshop;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
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
            DOTween.KillAll();
            ContextMenuEx.Instance.Hide();
            ProgressPanel.Instance.ResetProgressPanel();
            isFeeding = false;
            switch (index)
            {
                case 1:
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_SB_01.ToString());
                    beaker.gameObject.SetActive(false);
                    tmp_balanceNum.text = "0";
                    medium_original.gameObject.SetActive(true);
                    medium_feed.gameObject.SetActive(false);
                    medium_feed.localEulerAngles = cache_medium_feed_LocalEulerAngles;
                    cover.localPosition = cache_cover_LocalPosition;
                    beakerContent.localScale = cache_beakerContent_localScale;
                    anim_spoon.Stop();
                    spoon.gameObject.SetActive(false);
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_200LMAT_01.ToString());
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_200LMAT_02.ToString());
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_200LMAT_03.ToString());
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_M20A_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_MB_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_WLT_01.ToString());
                    bucket.gameObject.SetActive(false);
                    tmp_bigBalanceNum.text = "0";
                    bucket_F001S_cover.localPosition = cache_bucket_F001S_cover_localPosition;
                    bucket_F001S_level.localPosition = cache_bucket_F001S_level_localPosition;
                    bucket_F001S_cover.gameObject.SetActive(true);
                    scoopParticle.Stop();
                    anim_scoop.Stop();
                    scoop.gameObject.SetActive(false);
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_F001S_01.ToString());
                    //EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_F001B_01.ToString());
                    FlashManager.Instance.ShowFlash(dropArea_balance.gameObject);
                    break;
                case 2:
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_SB_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_MB_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_WLT_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001S_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_F001B_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_02.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_03.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_M20A_01.ToString());
                    powerButton_2.localEulerAngles = cache_powerButton_2_LocalEulerAngles;
                    plcPipeFitting.CloseAllFitting();
                    plcPipeFitting_Reservoir.CloseAllFitting();
                    PipeFittingManager.Instance.CloseAll();
                    plc_100L.gameObject.SetActive(true);
                    plc_100LDispensing.gameObject.SetActive(true);
                    plc_100LReservoirTank.gameObject.SetActive(true);
                    plc_100LReservoirTank.ResetPLC();
                    plc_100LDispensing.ResetPLC();
                    plc_100L.gameObject.SetActive(false);
                    fluidLevel_100L.Value = 0;
                    liquidVolume_100L.level = 0;
                    plc_100LDispensing.textLevel.text = "75";
                    cover_100L.localEulerAngles = cache_cover_100L_LocalEulerAngles;
                    bucket_feedF001S.gameObject.SetActive(false);
                    liquidVolume_100L.liquidColor1 = Color.clear;
                    particle_feedF001S.Stop();
                    blender_100L.localEulerAngles = cache_blender_100L_LocalEulerAngles;
                    sanitaryPump_100L.localPosition = cache_sanitaryPump_100L_localPosition;
                    fluidLevel_100LReservoir.Value = 0;
                    fluidLevel_100LReservoir.GetComponent<LiquidVolume>().level = 0;
                    hasCloseDischargeValve_1 = false;
                    hasClosePump_1 = false;
                    hasCloseDischargeValve_2 = false;
                    hasClosePump_2 = false;
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_FILTER_01.ToString());
                    filter.SetActive(true);
                    filter_detect.SetActive(false);
                    connecter.SetActive(false);
                    plc_FilterDetector.ResetPLC();
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_BLP_01.ToString());
                    VirtualRealityComponent virtualReality_sampleBottle_100L = sampleBottle_100L.GetComponent<VirtualRealityComponent>();
                    if (virtualReality_sampleBottle_100L!=null)
                    {
                        virtualReality_sampleBottle_100L.IsChange = false;
                    }
                    sampleValve_100L.localEulerAngles = cache_sampleValve_100L_LocalEulerAngles;
                    FluidLevelComponent fluid_sampleBottle_100L = sampleBottle_100L.GetComponentInChildren<FluidLevelComponent>();
                    if (fluid_sampleBottle_100L!=null)
                    {
                        fluid_sampleBottle_100L.Value = 0;
                    }
                    sampleBottle_100L.transform.Find("瓶身").GetComponent<LiquidVolume>().level = 0;
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_BLP_02.ToString());
                    doorComponent.Opening(false);
                    passBoxBottle.SetActive(false);
                    FlashManager.Instance.ShowFlash(powerButton_1.gameObject);
                    break;
                case 3:
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_F001S_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_F001B_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_01.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_02.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_200LMAT_03.ToString());
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_M20A_01.ToString());
                    powerButton_2.localEulerAngles = cache_powerButton_2_LocalEulerAngles;
                    plcPipeFitting_2.CloseAllFitting();
                    plcPipeFitting_Reservoir_2.CloseAllFitting();
                    PipeFittingManager.Instance.CloseAll();
                    plc_200L.gameObject.SetActive(true);
                    plc_200LDispensing.gameObject.SetActive(true);
                    plc_200LReservoirTank.gameObject.SetActive(true);
                    plc_200LReservoirTank.ResetPLC();
                    plc_200LDispensing.ResetPLC();
                    plc_200L.gameObject.SetActive(false);
                    fluidLevel_200L.Value = 0;
                    liquidVolume_200L.level = 0;
                    plc_200LDispensing.textLevel.text = "75";
                    cover_200L.localEulerAngles = cache_cover_200L_LocalEulerAngles;
                    hasFeedMAT_01 = false;
                    hasFeedMAT_02 = false;
                    hasFeedMAT_03 = false;
                    bucket_feed200L.gameObject.SetActive(false);
                    liquidVolume_200L.liquidColor1 = Color.clear;
                    particle_feed200L.Stop();
                    blender_200L.localEulerAngles = cache_blender_200L_LocalEulerAngles;
                    sanitaryPump_200L.localPosition = cache_sanitaryPump_200L_localPosition;
                    fluidLevel_200LReservoir.Value = 0;
                    fluidLevel_200LReservoir.GetComponent<LiquidVolume>().level = 0;
                    hasCloseDischargeValve_3 = false;
                    hasClosePump_3 = false;
                    hasCloseDischargeValve_4 = false;
                    hasClosePump_4 = false;
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_FILTER_01.ToString());
                    filter.SetActive(true);
                    filter_detect.SetActive(false);
                    connecter.SetActive(false);
                    plc_FilterDetector.ResetPLC();
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.PYG_BLP_01.ToString());
                    VirtualRealityComponent virtualReality_sampleBottle_200L = sampleBottle_200L.GetComponent<VirtualRealityComponent>();
                    if (virtualReality_sampleBottle_200L != null)
                    {
                        virtualReality_sampleBottle_200L.IsChange = false;
                    }
                    sampleValve_200L.localEulerAngles = cache_sampleValve_200L_LocalEulerAngles;
                    FluidLevelComponent fluid_sampleBottle_200L = sampleBottle_200L.GetComponentInChildren<FluidLevelComponent>();
                    if (fluid_sampleBottle_200L!=null)
                    {
                       fluid_sampleBottle_200L.Value = 0;
                    }
                    sampleBottle_200L.transform.Find("瓶身").GetComponent<LiquidVolume>().level = 0;
                    EventDispatcher.ExecuteEvent(Events.Item.Goods.Remove, GoodsType.PYG_BLP_02.ToString());
                    doorComponent.Opening(false);
                    passBoxBottle.SetActive(false);
                    FlashManager.Instance.ShowFlash(powerButton_2.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
