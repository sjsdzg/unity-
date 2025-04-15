using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 总体介绍信息窗口
    /// </summary>
	public class OverallInfoBar : BaseRamingUI
    {
        /// <summary>
        /// 内容信息
        /// </summary>
        private Text m_Content;

        /// <summary>
        /// 标题 
        /// </summary>
        private Text titleText;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button closeButton;

        /// <summary>
        /// 进入房间按钮
        /// </summary>
        private Button EnterButton;

        /// <summary>
        /// 背景遮挡
        /// </summary>
        private Image MaskImage;


        private bool isBackGround;
        /// <summary>
        /// 是否有背景
        /// </summary>
        public bool IsBackGround
        {
            get { return isBackGround; }
            set
            {
                isBackGround = value;
                MaskImage.enabled = value;
            }
        }

        private int SpeakSpeed = 20;

        /// <summary>
        /// 进入房间
        /// </summary>
        private Action EnterAction;

        /// <summary>
        /// 关闭窗口的动作(总体介绍)
        /// </summary>
        private Action CloseAction;

        /// <summary>
        ///  退出事件
        /// </summary>
        private Action ExitAction;
        void Awake()
        {
            m_Content = transform.Find("Panel/ScrollView/Viewport/Content/Principle").GetComponent<Text>();
            titleText = transform.Find("Panel/Title/Text").GetComponent<Text>();
            closeButton = transform.Find("Panel/CloseButton").GetComponent<Button>();
            EnterButton = transform.Find("Panel/EnterButton").GetComponent<Button>();

            MaskImage = transform.GetComponent<Image>();

            closeButton.onClick.AddListener(HideWindow);
            EnterButton.onClick.AddListener(Enter_Click);

            ShowMode = UIWindowShowMode.HideOther;
        }

        /// <summary>
        /// 进入按钮点击事件
        /// </summary>
        private void Enter_Click()
        {
            if (EnterAction != null)
            {
                EnterButton.gameObject.SetActive(false);
                EnterAction.Invoke();
            }
        }

        /// <summary>
        /// 总体介绍内容显示
        /// </summary>
        /// <param name="value"></param>
        public void OverallContentShow(IntroduceContent value, bool isShowEnter, Action _enter, Action _close, Action _exit)
        {
            IsActive = true;
            if (isShowEnter)
                EnterButton.gameObject.SetActive(true);
            else
                EnterButton.gameObject.SetActive(false);
            EnterAction = _enter;
            CloseAction = _close;
            ExitAction = _exit;
            IsBackGround = false;
            //打字
            m_Content.DOKill();
            m_Content.text = string.Empty;

            if (value != null)
            {
                string _value = value.Text.Replace("\\n", "\n");
                int count = _value.Length;
                m_Content.DOText(_value, count / SpeakSpeed);
                ///播放音频
                //RamingAudioController.Instance.PlayAudio(value.Mp3Url);
                RamingAudioController.Instance.Play(value.Mp3Url);
            }

            titleText.text = "总体介绍";
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public override void Show(BaseUIArgs uiParams)
        {
            base.Show(uiParams);
            //transform.doloca
            print("显示 Overall");
            OverallUiArgs arg = uiParams as OverallUiArgs;
            OverallContentShow(arg.introduceInfo, arg.IsShowEnterBtn, arg.EnterAction, arg.CloseAction, arg.ExitAction);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public override void Hide(BaseUIArgs uiParams)
        {
            base.Show(uiParams);


            if (ExitAction != null)
            {
                //ExitAction.Invoke();
            }
            HideWindow();
            print("关闭 Overall");

        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void HideWindow()
        {
            IsActive = false;
            if (CloseAction != null)
            {
                CloseAction.Invoke();
            }
            ///停止播放音频
            RamingAudioController.Instance.Stop();
        }


    }

}