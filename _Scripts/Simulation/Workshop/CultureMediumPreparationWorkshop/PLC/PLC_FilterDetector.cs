using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.PLC
{
    public static class PLC_FilterDetectorEvent
    {
        public static readonly string ActiveButtonBubblePoint = "PLC_FilterDetectorEvent.ActiveButtonBubblePoint";
        public static readonly string ButtonBubblePointClick = "PLC_FilterDetectorEvent.ButtonBubblePointClick";
        public static readonly string ButtonOKClick = "PLC_FilterDetectorEvent.ButtonOKClick";
        public static readonly string ButtonStartClick = "PLC_FilterDetectorEvent.ButtonStartClick";
        public static readonly string Reset = "PLC_FilterDetectorEvent.Reset";
    }
    public class PLC_FilterDetector : MonoBehaviour
    {
        #region Panel
        Transform selectPanel;
        Transform configPanel;
        Transform startPanel;
        #endregion

        #region Button
        Button buttonBubblePoint;
        Button buttonOK;
        Button buttonStart;
        #endregion

        Slider slider;

        Text hint;

        bool isBubblePointActive;
        bool isOKActive;
        bool isStartActive;

        private void Awake()
        {
            InitGUI();
            InitEvent();
        }

        private void Start()
        {
            configPanel.gameObject.SetActive(false);
            startPanel.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            slider.interactable = false;
            hint.gameObject.SetActive(false);
        }

        private void InitGUI()
        {
            selectPanel = transform.Find("选择界面");
            buttonBubblePoint = selectPanel.Find("ButtonBubblePoint").GetComponent<Button>();
            configPanel = transform.Find("配置界面");
            buttonOK = configPanel.Find("ButtonOK").GetComponent<Button>();
            startPanel = transform.Find("启动界面");
            buttonStart = startPanel.Find("ButtonStart").GetComponent<Button>();
            slider= transform.Find("Slider").GetComponent<Slider>();
            hint = transform.Find("Hint").GetComponent<Text>();
        }

        private void InitEvent()
        {
            buttonBubblePoint.onClick.AddListener(ButtonBubblePointClick);
            buttonOK.onClick.AddListener(ButtonOKClick);
            buttonStart.onClick.AddListener(ButtonStarClick);
            EventDispatcher.RegisterEvent(PLC_FilterDetectorEvent.ActiveButtonBubblePoint, ActiveButtonBubblePoint);
            EventDispatcher.RegisterEvent(PLC_FilterDetectorEvent.Reset, ResetPLC);
        }

        public void ResetPLC()
        {
            TwinklingButton(buttonBubblePoint, false);
            TwinklingButton(buttonStart, false);
            TwinklingButton(buttonOK, false);
            isBubblePointActive = false;
            isOKActive = false;
            isStartActive = false;
            slider.DOKill();
            slider.value = 0;
            slider.gameObject.SetActive(false);
            hint.gameObject.SetActive(false);
            startPanel.gameObject.SetActive(false);
            configPanel.gameObject.SetActive(false);
        }

        private void ActiveButtonBubblePoint()
        {
            TwinklingButton(buttonBubblePoint, true);
            isBubblePointActive = true;
        }

        private void ButtonStarClick()
        {
            if (!isStartActive)
            {
                return;
            }
            isStartActive = false;
            EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.ButtonStartClick);
            TwinklingButton(buttonStart, false);
            slider.gameObject.SetActive(true);
            hint.gameObject.SetActive(true);
            hint.text = "正在进行泡点测试...";
            slider.DOValue(1, 5).OnComplete(() =>
            {
                hint.text = "泡点测试合格";
            });
        }

        private void ButtonOKClick()
        {
            EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.ButtonOKClick);
            if (!isOKActive)
            {
                return;
            }
            isOKActive = false;
            startPanel.gameObject.SetActive(true);
            TwinklingButton(buttonOK, false);
            TwinklingButton(buttonStart, true);
            isStartActive = true;
        }


        private void ButtonBubblePointClick()
        {
            EventDispatcher.ExecuteEvent(PLC_FilterDetectorEvent.ButtonBubblePointClick);
            if (!isBubblePointActive)
            {
                return;
            }
            isBubblePointActive = false;
            configPanel.gameObject.SetActive(true);
            TwinklingButton(buttonBubblePoint, false);
            TwinklingButton(buttonOK, true);
            isOKActive = true;
        }

        private void TwinklingButton(Button button,bool isOn)
        {
            Image image = button.GetComponent<Image>();
            if (isOn)
            {
                image.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                image.DOKill();
                image.DOFade(0, 0);
            }
        }
    }

}
