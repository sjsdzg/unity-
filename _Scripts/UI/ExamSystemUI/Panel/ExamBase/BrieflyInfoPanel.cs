using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;

namespace XFramework.UI
{
    public class BrieflyInfoPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.BrieflyInfoPanel;
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        private Button buttonOK;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        /// <summary>
        /// 标题文本
        /// </summary>
        private Text textTitle;

        /// <summary>
        /// 内容
        /// </summary>
        private Text content;

        /// <summary>
        /// 按钮显示文本
        /// </summary>
        private Text textButton;

        /// <summary>
        /// 
        /// </summary>
        private BrieflyInfoPanelData brieflyInfoPanelData;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            background = transform.Find("Background").GetComponent<RectTransform>();

            buttonOK = transform.Find("Background/Content/ButtonOK").GetComponent<Button>();
            buttonClose = transform.Find("Background/TitleBar/ButtonClose").GetComponent<Button>();
            textTitle = transform.Find("Background/TitleBar/Text").GetComponent<Text>();
            content = transform.Find("Background/Content/Text").GetComponent<Text>();
            textButton = transform.Find("Background/Content/ButtonOK/Text").GetComponent<Text>();
        }

        private void InitEvent()
        {
            buttonOK.onClick.AddListener(buttonOK_onClick);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonOK_onClick()
        {
            Transform middlePanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer];
            PanelManager.Instance.OpenPanelCloseOthers(middlePanelContainer, brieflyInfoPanelData.PanelType);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.BrieflyInfoPanel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            background.DOScale(0, 0.3f).From();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                BrieflyInfoPanelData data = PanelParams[0] as BrieflyInfoPanelData;
                brieflyInfoPanelData = data;
                textTitle.text = data.title;
                content.text = data.content;
                textButton.text = data.buttonText;
            }
        }

    }

    /// <summary>
    /// 考试考试面板数据类
    /// </summary>
    public class BrieflyInfoPanelData
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 按钮内容
        /// </summary>
        public string buttonText { get; set; }
        /// <summary>
        /// 要打开的面板
        /// </summary>
        public EnumPanelType PanelType { get; set; }
    }
}
