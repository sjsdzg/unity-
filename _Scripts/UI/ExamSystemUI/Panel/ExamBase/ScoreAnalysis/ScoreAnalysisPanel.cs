using UnityEngine;
using System.Collections;
using XFramework.Module;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using XFramework.Core;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 成绩分析
    /// </summary>
    public class ScoreAnalysisPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ScoreAnalysisPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 考试名称InputField
        /// </summary>
        private InputField inputFieldExam;

        /// <summary>
        /// 选择按钮
        /// </summary>
        private Button buttonSelect;

        /// <summary>
        /// 分数区间
        /// </summary>
        private ScoreRangeForm m_ScoreRangeForm;

        /// <summary>
        /// 可视化图表
        /// </summary>
        private PieChartForm m_PieChartForm;

        /// <summary>
        /// 考试基本模块
        /// </summary>
        private ExamBasicModule examBasicModule;

        /// <summary>
        /// 分数区间列表
        /// </summary>
        public List<ScoreRange> ScoreRanges { get; set; }

        private Exam exam = null;
        /// <summary>
        /// 考试
        /// </summary>
        public Exam Exam
        {
            get { return exam; }
            set
            {
                exam = value;
                if (exam != null)
                {
                    inputFieldExam.text = exam.Name;
                    OnSelectExam(exam);
                }
                else
                {
                    inputFieldExam.text = "";
                }
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            m_ScoreRangeForm = transform.Find("FormBar/List/ScoreRangeForm").GetComponent<ScoreRangeForm>();
            inputFieldExam = transform.Find("FormBar/List/Name/Value/InputField").GetComponent<InputField>();
            buttonSelect = transform.Find("FormBar/List/Name/Value/ButtonSelect").GetComponent<Button>();
            m_PieChartForm = transform.Find("FormBar/List/PieChartForm").GetComponent<PieChartForm>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
            m_ScoreRangeForm.OnAnalysisEvent.AddListener(m_ScoreRangeForm_OnAnalysis);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ScoreAnalysisPanel, PanelDefine.GetPanelComment(EnumPanelType.ScoreAnalysisPanel));
        }

        protected override void OnStart()
        {
            base.OnStart();
            examBasicModule = ModuleManager.Instance.GetModule<ExamBasicModule>();

            List<ScoreRange> scoreRanges = new List<ScoreRange>();
            scoreRanges.Add(new ScoreRange(0, 59));
            scoreRanges.Add(new ScoreRange(60, 79));
            scoreRanges.Add(new ScoreRange(80, 100));
            m_ScoreRangeForm.InitData(scoreRanges);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        /// <summary>
        /// 选择按钮点击时
        /// </summary>
        private void buttonSelect_onClick()
        {
            SelectExamPanelData data = new SelectExamPanelData();
            data.Panel = this;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.SelectExamPanel, data);
        }

        /// <summary>
        /// 选择考试
        /// </summary>
        /// <param name="exam"></param>
        private void OnSelectExam(Exam exam)
        {
            //examBasicModule.GetExamAnalysis(App.Instance.SoftwareId, exam.Id, ReceiveGetExamAnalysis);
        }

        /// <summary>
        /// 分析时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_ScoreRangeForm_OnAnalysis(ScoreRangeForm arg0)
        {
            ScoreRanges = arg0.ScoreRanges;
            examBasicModule.GetScoreAnalysis(App.Instance.SoftwareId, exam.Id, arg0.ScoreRanges, ReceiveGetScoreAnalysis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetScoreAnalysis(NetworkPackageInfo packageInfo)
        {
            ScoreAnalysisResp resp = ScoreAnalysisResp.Parser.ParseFrom(packageInfo.Body);

            List<PieChartData> pieChartDatas = new List<PieChartData>();
            for (int i = 0; i < resp.RangeRates.Count; i++)
            {
                RangeRateProto rangeRateProto = resp.RangeRates[i];
                ScoreRange scoreRange = ScoreRanges[rangeRateProto.Index];
                // PieChartData
                PieChartData pieChartData = new PieChartData();
                pieChartData.Name = string.Format("{0}-{1}(人数{2})", scoreRange.Min, scoreRange.Max, rangeRateProto.Number);
                pieChartData.Amount = rangeRateProto.Rate * 100;
                pieChartDatas.Add(pieChartData);
            }

            m_PieChartForm.InitData(pieChartDatas);
        }
    }
}

