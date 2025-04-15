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
    public class StartExamPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.StartExamPanel;
        }

        /// <summary>
        /// 考试名称
        /// </summary>
        private Text textExamName;

        /// <summary>
        /// 考试时长
        /// </summary>
        private Text textDuration;

        /// <summary>
        /// 考试总分
        /// </summary>
        private Text textTotalScore;

        /// <summary>
        /// 及格分数
        /// </summary>
        private Text textPassScore;

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
        private RectTransform panel;

        /// <summary>
        /// 考试信息
        /// </summary>
        public Exam examInfo;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        /// <summary>
        /// 试卷模块
        /// </summary>
        private PaperModule paperModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            panel = transform.Find("Panel").GetComponent<RectTransform>();
            textExamName = transform.Find("Panel/Content/ExamName/Text").GetComponent<Text>();
            textDuration = transform.Find("Panel/Content/Duration/Text").GetComponent<Text>();
            textTotalScore = transform.Find("Panel/Content/TotalScore/Text").GetComponent<Text>();
            textPassScore = transform.Find("Panel/Content/PassScore/Text").GetComponent<Text>();
            buttonOK = transform.Find("Panel/Content/ButtonOK").GetComponent<Button>();
            buttonClose = transform.Find("Panel/TitleBar/ButtonClose").GetComponent<Button>();
        }

        private void InitEvent()
        {
            buttonOK.onClick.AddListener(buttonOK_onClick);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonOK_onClick()
        {
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            ExamPanelData data = new ExamPanelData();
            data.Again = false;
            data.ExamInfo = examInfo;
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.ExamPanel, data);
            PanelManager.Instance.ClosePanel(EnumPanelType.StartExamPanel);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.StartExamPanel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            paperModule = ModuleManager.Instance.GetModule<PaperModule>();
            panel.DOScale(0, 0.3f).From();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                StartExamPanelData data = PanelParams[0] as StartExamPanelData;
                examInfo = data.ExamInfo;
                textExamName.text = examInfo.Name;
                textDuration.text = examInfo.Duration + "分钟";
                //获取试卷
                paperModule.GetPaper(examInfo.PaperId, ReceiveGetPaperResp);
            }
        }

        /// <summary>
        /// 获取试卷的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetPaperResp(NetworkPackageInfo packageInfo)
        {
            GetPaperResp resp = GetPaperResp.Parser.ParseFrom(packageInfo.Body);
            PaperProto paperProto = resp.Paper;
            Paper paper = new Paper();
            paper.Id = paperProto.Id;
            paper.Name = paperProto.Name;
            paper.CategoryId = paperProto.CategoryId;
            paper.Status = paperProto.Status;
            paper.TotalScore = paperProto.TotalScore;
            paper.PassScore = paperProto.PassScore;
            paper.Poster = paperProto.Poster;
            paper.CreateTime = DateTimeUtil.OfEpochMilli(paperProto.CreateTime);
            paper.Modifier = paperProto.Modifier;
            paper.UpdateTime = DateTimeUtil.OfEpochMilli(paperProto.UpdateTime);
            paper.Remark = paperProto.Remark;
            paper.Data = paperProto.Data;
            //设置
            textTotalScore.text = paper.TotalScore.ToString();
            textPassScore.text = paper.PassScore.ToString();
        }

    }

    /// <summary>
    /// 考试考试面板数据类
    /// </summary>
    public class StartExamPanelData
    {
        /// <summary>
        /// 考试数据
        /// </summary>
        public Exam ExamInfo { get; set; }
    }
}
