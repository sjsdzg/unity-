using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.PLC
{
    public static class PLC_200LDispensingTankEvent
    {
        public static readonly string CurrentSolutionType = "PLC_200LDispensingTankEvent.CurrentSolutionType";
        public static readonly string ActiveButtonSetWater = "PLC_200LDispensingTankEvent.ActiveButtonSetWater";
        public static readonly string SetWaterFinish = "PLC_200LDispensingTankEvent.SetWaterFinish";
        public static readonly string ActiveMotorToggle = "PLC_200LDispensingTankEvent.ActiveMotorToggle";
        public static readonly string StartMotor = "PLC_200LDispensingTankEvent.StartMotor";
        public static readonly string StopMotor = "PLC_200LDispensingTankEvent.StopMotor";
        public static readonly string ActiveSanitaryPumpToggle = "PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle";
        public static readonly string StartSanitaryPump = "PLC_200LDispensingTankEvent.StartSanitaryPump";
        public static readonly string StopSanitaryPump = "PLC_200LDispensingTankEvent.StopSanitaryPump";
        public static readonly string ActiveButtonToReservoir = "PLC_200LDispensingTankEvent.ActiveButtonToReservoir";
        public static readonly string ToReservoir = "PLC_200LDispensingTankEvent.ToReservoir";
    }
    public class PLC_200LDispensingTank : MonoBehaviour
    {
        /// <summary>
        /// 溶液类型
        /// </summary>
        public enum SolutionType
        {
            C,
            D
        }

        static class PLC_200LDispensingTankData
        {
            public static readonly float waterVolume=90;
            public static readonly float blenderSpeed =400;

            public static readonly float waterVolume_2 = 90;
            public static readonly float blenderSpeed_2 = 400;
        }
        
        public PLCPipeFittingManager pipeFitting;

        #region Panel
        Transform setWaterPanel;
        #endregion

        #region Button&Toggle
        Button buttonSetWater;
        Button buttonConfirmWaterSet;
        Button buttonToReservoir;
        Toggle toggleMotor;
        Toggle toggleSanitaryPump;
        #endregion

        #region Image
        Image imageMotor;
        Image imageSanitaryPump;
        Image imageToReservoir;
        #endregion

        #region InputField
        InputField inputWaterVolume;
        InputField inputBlenderSpeed;
        #endregion

        #region Text
        Text textWaterVolume;
        Text textBlenderSpeed;
        [HideInInspector]
        public Text textTemp;
        [HideInInspector]
        public Text textLevel;
        #endregion

        SolutionType currentSolutionType;

        PLC_200LReservoirTank plc_100LReservoirTank;

        bool isReservoirReady = false;

        private void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            plc_100LReservoirTank = transform.parent.Find("储液罐").GetComponent<PLC_200LReservoirTank>();

            pipeFitting = transform.Find("阀门").GetComponent<PLCPipeFittingManager>();
            
            setWaterPanel = transform.Find("纯化水配置");

            buttonSetWater = transform.Find("其它/纯化水配置按钮").GetComponent<Button>();
            buttonConfirmWaterSet = setWaterPanel.Find("ButtonConfirm").GetComponent<Button>();
            buttonToReservoir = transform.Find("其它/去100L储液罐按钮").GetComponent<Button>();
            toggleMotor = transform.Find("其它/电机按钮").GetComponent<Toggle>();
            toggleSanitaryPump = transform.Find("其它/卫生泵按钮").GetComponent<Toggle>();

            imageToReservoir = buttonToReservoir.GetComponent<Image>();
            imageMotor = transform.Find("其它/电机按钮").GetComponent<Image>();
            imageSanitaryPump = transform.Find("其它/卫生泵按钮").GetComponent<Image>();

            inputWaterVolume = setWaterPanel.Find("纯化水添加量/InputField").GetComponent<InputField>();
            inputBlenderSpeed = setWaterPanel.Find("搅拌电机/InputField").GetComponent<InputField>();

            textWaterVolume= transform.Find("其它/纯化水添加量数值").GetComponent<Text>();
            textBlenderSpeed = transform.Find("其它/搅拌电机数值").GetComponent<Text>();
            textTemp = transform.Find("其它/温度数值").GetComponent<Text>();
            textLevel = transform.Find("其它/液位数值").GetComponent<Text>();
        }

        private void InitEvent()
        {
            buttonSetWater.onClick.AddListener(ButtonSetWaterClick);
            buttonConfirmWaterSet.onClick.AddListener(ButtonConfirmWaterSetClick);
            buttonToReservoir.onClick.AddListener(ButtonToReservoirClick);
            toggleMotor.onValueChanged.AddListener(ToggleMotorChanged);
            toggleSanitaryPump.onValueChanged.AddListener(ToggleSanitaryPumpChanged);
            EventDispatcher.RegisterEvent<SolutionType>(PLC_200LDispensingTankEvent.CurrentSolutionType, CurrentSolutionType);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.ActiveButtonSetWater, ActiveButtonSetWater);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle, ActiveMotorToggle);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle, ActiveSanitaryPumpToggle);
            EventDispatcher.RegisterEvent(PLC_200LDispensingTankEvent.ActiveButtonToReservoir, ActiveButtonToReservoir);
        }
        private void ButtonToReservoirClick()
        {
            plc_100LReservoirTank.gameObject.SetActive(true);
            plc_100LReservoirTank.OnOpen.Invoke();
        }

        private void ActiveButtonToReservoir()
        {
            imageToReservoir.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        
        private void CurrentSolutionType(SolutionType type)
        {
            currentSolutionType = type;
        }

        private void ActiveSanitaryPumpToggle()
        {
            imageSanitaryPump.DOColor(Color.yellow,0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void ToggleSanitaryPumpChanged(bool isOn)
        {
            imageSanitaryPump.DOKill();
            if (isOn)
            {
                imageSanitaryPump.color = Color.green;
                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.StartSanitaryPump);
            }
            else
            {
                imageSanitaryPump.color = Color.white;
                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.StopSanitaryPump);
            }
        }

        private void ActiveMotorToggle()
        {
            imageMotor.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void ActiveButtonSetWater()
        {
            Image image = buttonSetWater.GetComponent<Image>();
            image.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void ToggleMotorChanged(bool isOn)
        {
            imageMotor.DOKill();
            if (isOn)
            {
                imageMotor.color = Color.green;
                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.StartMotor);
            }
            else
            {
                imageMotor.color = new Color(255, 255, 255, 0);
                EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.StopMotor);
            }
        }

        private void ButtonConfirmWaterSetClick()
        {
            float waterVolume;
            float blenderSpeed;
            switch (currentSolutionType)
            {
                case SolutionType.C:
                    if (!float.TryParse(inputWaterVolume.text, out waterVolume) || waterVolume != PLC_200LDispensingTankData.waterVolume)
                    {
                        Transform screen = FindObjectOfType<CultureMediumPreparationWorkshop>().transform.Find("扩展/200L配液罐控制面板");
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, screen.transform, "纯化水添加量设置不正确", Color.red);
                        return;
                    }
                    if (!float.TryParse(inputBlenderSpeed.text, out blenderSpeed) || blenderSpeed != PLC_200LDispensingTankData.blenderSpeed)
                    {
                        Transform screen = FindObjectOfType<CultureMediumPreparationWorkshop>().transform.Find("扩展/200L配液罐控制面板");
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, screen.transform, "搅拌速度设置不正确", Color.red);
                        return;
                    }
                    break;
                case SolutionType.D:
                    if (!float.TryParse(inputWaterVolume.text, out waterVolume) || waterVolume != PLC_200LDispensingTankData.waterVolume_2)
                    {
                        Transform screen = FindObjectOfType<CultureMediumPreparationWorkshop>().transform.Find("扩展/200L配液罐控制面板");
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, screen.transform, "纯化水添加量设置不正确", Color.red);
                        return;
                    }
                    if (!float.TryParse(inputBlenderSpeed.text, out blenderSpeed) || blenderSpeed != PLC_200LDispensingTankData.blenderSpeed_2)
                    {
                        Transform screen = FindObjectOfType<CultureMediumPreparationWorkshop>().transform.Find("扩展/200L配液罐控制面板");
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, screen.transform, "搅拌速度设置不正确", Color.red);
                        return;
                    }
                    break;
                default:
                    break;
            }
            textWaterVolume.text = inputWaterVolume.text;
            textBlenderSpeed.text = inputBlenderSpeed.text;
            setWaterPanel.gameObject.SetActive(false);
            EventDispatcher.ExecuteEvent(PLC_200LDispensingTankEvent.SetWaterFinish);
        }

        private void ButtonSetWaterClick()
        {
            setWaterPanel.gameObject.SetActive(true);
            buttonSetWater.GetComponent<Image>().DOKill();
            buttonSetWater.GetComponent<Image>().color = new Color(255,255,255,0);
        }

        private void Start()
        {
            pipeFitting.Init("Simulation/Stage/培养基配制工段/培养基配制-流体配置表.xml", pipeFitting.transform);
            plc_100LReservoirTank.gameObject.SetActive(false);
            setWaterPanel.gameObject.SetActive(false);
            textTemp.text = "25";
            textLevel.text = "0";
        }
        public void ResetPLC()
        {
            textTemp.text = "25";
            textLevel.text = "0";
            plc_100LReservoirTank.gameObject.SetActive(false);
            setWaterPanel.gameObject.SetActive(false);
            buttonSetWater.DOKill();
            buttonSetWater.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            currentSolutionType = SolutionType.C;
            inputWaterVolume.text = "";
            inputBlenderSpeed.text = "";
            textWaterVolume.text = "";
            textBlenderSpeed.text = "";
            ToggleMotorChanged(false);
            ToggleSanitaryPumpChanged(false);
            imageToReservoir.DOKill();
            imageToReservoir.DOFade(0, 0);
            plc_100LReservoirTank.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            EventDispatcher.UnregisterEvent<SolutionType>(PLC_200LDispensingTankEvent.CurrentSolutionType, CurrentSolutionType);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.ActiveButtonSetWater, ActiveButtonSetWater);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.ActiveMotorToggle, ActiveMotorToggle);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.ActiveSanitaryPumpToggle, ActiveSanitaryPumpToggle);
            EventDispatcher.UnregisterEvent(PLC_200LDispensingTankEvent.ActiveButtonToReservoir, ActiveButtonToReservoir);
        }
    }

}
