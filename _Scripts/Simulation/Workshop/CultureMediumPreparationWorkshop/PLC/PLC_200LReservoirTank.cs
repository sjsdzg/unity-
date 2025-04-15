using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.PLC
{
    public static class PLC_200LReservoirTankEvent
    {
        public static readonly string ActiveToggleMotor = "PLC_200LReservoirTankEvent.ActiveToggleMotor";
        public static readonly string StartMotor = "PLC_200LReservoirTankEvent.StartMotor";
        public static readonly string StopMotor = "PLC_200LReservoirTankEvent.StopMotor";
    }
    public class PLC_200LReservoirTank : MonoBehaviour
    {
        public UnityEvent OnOpen;

        public PLCPipeFittingManager pipeFitting;

        #region Panel
        Transform setWaterPanel;
        #endregion

        #region Button&Toggle
        Button buttonSetWater;
        Button buttonConfirmWaterSet;
        Button buttonToDispensing;
        Toggle toggleMotor;
        Toggle toggleSanitaryPump;
        #endregion

        #region Image
        Image imageMotor;
        Image imageSanitaryPump;
        Image imageToReservoir;
        #endregion

        #region Text
        [HideInInspector]
        public Text textTemp;
        [HideInInspector]
        public Text textLevel;
        #endregion

        PLC_200LDispensingTank plc_100LDispensingTank;


        private void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            plc_100LDispensingTank = transform.parent.Find("配制罐").GetComponent<PLC_200LDispensingTank>();

            pipeFitting = transform.Find("阀门").GetComponent<PLCPipeFittingManager>();

            buttonToDispensing = transform.Find("其它/去100L配液罐按钮").GetComponent<Button>();
            toggleMotor = transform.Find("其它/电机按钮").GetComponent<Toggle>();

            imageMotor = transform.Find("其它/电机按钮").GetComponent<Image>();
            imageSanitaryPump = transform.Find("其它/卫生泵按钮").GetComponent<Image>();

            textTemp = transform.Find("其它/温度数值").GetComponent<Text>();
            textLevel = transform.Find("其它/液位数值").GetComponent<Text>();
        }

        private void InitEvent()
        {
            buttonToDispensing.onClick.AddListener(ButtonToDispensingClick);
            toggleMotor.onValueChanged.AddListener(ToggleMotorChanged);
            EventDispatcher.RegisterEvent(PLC_200LReservoirTankEvent.ActiveToggleMotor, ActiveToggleMotor);
        }
        private void ButtonToDispensingClick()
        {
            plc_100LDispensingTank.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }


        private void ActiveToggleMotor()
        {
            imageMotor.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void ToggleMotorChanged(bool isOn)
        {
            imageMotor.DOKill();
            if (isOn)
            {
                imageMotor.color = Color.green;
                EventDispatcher.ExecuteEvent(PLC_200LReservoirTankEvent.StartMotor);
            }
            else
            {
                imageMotor.color = new Color(255, 255, 255, 0);
                EventDispatcher.ExecuteEvent(PLC_200LReservoirTankEvent.StopMotor);
            }
        }

        

        private void Start()
        {
            pipeFitting.Init("Simulation/Stage/培养基配制工段/培养基配制-流体配置表.xml", pipeFitting.transform);
            textTemp.text = "25";
            textLevel.text = "0";
        }

        public void ResetPLC()
        {
            textTemp.text = "25";
            textLevel.text = "0";
            ToggleMotorChanged(false);
        }

        private void OnDestroy()
        {
            EventDispatcher.UnregisterEvent(PLC_200LReservoirTankEvent.ActiveToggleMotor, ActiveToggleMotor);
        }
    }

}
