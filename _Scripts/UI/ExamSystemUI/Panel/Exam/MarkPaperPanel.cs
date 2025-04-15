using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Network;

namespace XFramework.UI
{
    public class MarkPaperPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.MarkPaperPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        private MarkSectionBar markSectionBar;

        /// <summary>
        /// 序号索引视图
        /// </summary>
        private MarkViewBar markViewBar;

        /// <summary>
        /// 考试标题
        /// </summary>
        private MarkTitleBar markTitleBar;

        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 考试信息
        /// </summary>
        public Exam examInfo;

        /// <summary>
        /// 试卷信息
        /// </summary>
        public Paper paperInfo;

        /// <summary>
        /// 考试数据信息
        /// </summary>
        public ExamData examDataInfo;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 考试库模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamDataModule examDataModule;

        /// <summary>
        /// 考试基本模块
        /// </summary>
        private ExamBasicModule examBasicModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            markSectionBar = transform.Find("Background/MarkSectionBar").GetComponent<MarkSectionBar>();
            markViewBar = transform.Find("Background/MarkViewBar").GetComponent<MarkViewBar>();
            markTitleBar = transform.Find("Background/MarkSectionBar/Viewport/Content/TitleBar").GetComponent<MarkTitleBar>();
            background = transform.Find("Background").GetComponent<RectTransform>();
            buttonClose = transform.Find("Background/TitleBar/ButtonClose").GetComponent<Button>();
        }

        private void InitEvent()
        {
            markSectionBar.OnCompleted.AddListener(examSectionBar_OnCompleted);
            markSectionBar.OnInitialized.AddListener(markSectionBar_OnInitialized);
            markViewBar.OnClicked.AddListener(examViewBar_OnClicked);
            markViewBar.OnSubmit.AddListener(examViewBar_OnSubmit);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            examDataModule = ModuleManager.Instance.GetModule<ExamDataModule>();
            userModule = ModuleManager.Instance.GetModule<UserModule>();
            examBasicModule = ModuleManager.Instance.GetModule<ExamBasicModule>();
            background.DOScale(0, 0.3f).From();
        }

        /// <summary>
        /// 考试章节试题完成时，触发
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void examSectionBar_OnCompleted(int sectionId, int number, bool value)
        {
            markViewBar.TurnOn(sectionId, number, value);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.MarkPaperPanel);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                MarkPaperPanelData data = PanelParams[0] as MarkPaperPanelData;
                examDataInfo = data.ExamDataInfo;
                //获取考试试卷
                examBasicModule.GetExamPaper(examDataInfo.PaperId, ReceiveGetExamPaperResp);
                //获取考试
                examModule.GetExam(examDataInfo.ExamId, ReceiveGetExamResp);
                //设置
                markTitleBar.SetTimeInfo(examDataInfo.StartTime, examDataInfo.EndTime, examDataInfo.Score);
                markTitleBar.SetUserInfo(examDataInfo.UserName, examDataInfo.RealName);
            }
        }

        /// <summary>
        /// 接受获取考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamResp(NetworkPackageInfo packageInfo)
        {
            GetExamResp resp = GetExamResp.Parser.ParseFrom(packageInfo.Body);
            ExamProto examProto = resp.Exam;

            Exam exam = new Exam();
            exam.Id = examProto.Id;
            exam.Name = examProto.Name;
            exam.PaperId = examProto.PaperId;
            exam.CategoryId = examProto.CategoryId;
            exam.Status = examProto.Status;
            exam.Duration = examProto.Duration;
            exam.StartTime = DateTimeUtil.OfEpochMilli(examProto.StartTime);
            exam.EndTime = DateTimeUtil.OfEpochMilli(examProto.EndTime);
            exam.ShowTime = DateTimeUtil.OfEpochMilli(examProto.ShowTime);
            exam.Poster = examProto.Poster;
            exam.CreateTime = DateTimeUtil.OfEpochMilli(examProto.CreateTime);
            exam.Modifier = examProto.Modifier;
            exam.UpdateTime = DateTimeUtil.OfEpochMilli(examProto.UpdateTime);
            exam.QuestionOrder = examProto.QuestionOrder;
            exam.ShowKey = examProto.ShowKey;
            exam.ShowMode = examProto.ShowMode;
            exam.Remark = examProto.Remark;
            //设置
            examInfo = exam;
            //设置考试信息
            markTitleBar.SetExamInfo(examInfo.StartTime, examInfo.EndTime, examInfo.Duration);
        }

        /// <summary>
        /// 初始化用户答案和分数
        /// </summary>
        private void markSectionBar_OnInitialized()
        {
            markSectionBar.ExamSectionJsons = JsonConvert.DeserializeObject<List<ExamSectionJson>>(examDataInfo.Data);
        }

        private void examViewBar_OnClicked(int sectionId, int number)
        {
            markSectionBar.Center(sectionId, number);
        }

        /// <summary>
        /// 考试试图点击提交时，触发
        /// </summary>
        private void examViewBar_OnSubmit()
        {
            SubmitGrading();
        }

        /// <summary>
        /// 提交评卷内容
        /// </summary>
        private void SubmitGrading()
        {
            List<ExamSectionJson> examSectionJsons = markSectionBar.ExamSectionJsons;
            examDataInfo.Data = JsonConvert.SerializeObject(examSectionJsons);

            //统计分数
            int score = 0;
            foreach (var examSectionJson in examSectionJsons)
            {
                foreach (var examQuesJson in examSectionJson.QuesJsons)
                {
                    score += examQuesJson.Score;
                }
            }
            examDataInfo.Score = score;
            //提交试卷
            examDataModule.UpdateExamData(examDataInfo, ReceiveUpdateExamDataResp);
        }

        /// <summary>
        /// 接受获取考试试卷的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamPaperResp(NetworkPackageInfo packageInfo)
        {
            GetExamPaperResp resp = GetExamPaperResp.Parser.ParseFrom(packageInfo.Body);
            Paper paper = new Paper();
            paper.Id = resp.ExamPaper.Id;
            paper.Name = resp.ExamPaper.Name;
            paper.CategoryId = resp.ExamPaper.CategoryId;
            paper.Status = resp.ExamPaper.Status;
            paper.TotalScore = resp.ExamPaper.TotalScore;
            paper.PassScore = resp.ExamPaper.PassScore;
            paper.Remark = resp.ExamPaper.Remark;

            foreach (ExamPaperSectionProto sectionProto in resp.ExamPaper.Sections)
            {
                PaperSection section = new PaperSection();
                section.Id = sectionProto.Id;
                section.Name = sectionProto.Name;
                section.Remark = sectionProto.Remark;
                section.QType = sectionProto.QType;
                section.Number = sectionProto.Number;
                section.Score = sectionProto.Score;

                foreach (ExamQuestionProto quesProto in sectionProto.Questions)
                {
                    //设置答题内容
                    switch (quesProto.Type)
                    {
                        case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                            QuestionSingleChoice questionSingleChoice = new QuestionSingleChoice();
                            List<Option> singleChoiceOptions = JsonConvert.DeserializeObject<List<Option>>(quesProto.Data);
                            questionSingleChoice.Options = singleChoiceOptions;
                            BuildQuestion(quesProto, questionSingleChoice);
                            section.Questions.Add(questionSingleChoice);
                            break;
                        case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                            QuestionMultipleChoice questionMultipleChoice = new QuestionMultipleChoice();
                            List<Option> multipleChoiceOptions = JsonConvert.DeserializeObject<List<Option>>(quesProto.Data);
                            questionMultipleChoice.Options = multipleChoiceOptions;
                            BuildQuestion(quesProto, questionMultipleChoice);
                            section.Questions.Add(questionMultipleChoice);
                            break;
                        case ExamSystemConstants.QuestionType.BLANK_FILL:
                            QuestionBlankFill questionBlankFill = new QuestionBlankFill();
                            QBlankFillData data = JsonConvert.DeserializeObject<QBlankFillData>(quesProto.Data);
                            questionBlankFill.Blanks = data.Blanks;
                            questionBlankFill.IsComplex = data.IsComplex;
                            BuildQuestion(quesProto, questionBlankFill);
                            section.Questions.Add(questionBlankFill);
                            break;
                        case ExamSystemConstants.QuestionType.JUDGMENT:
                        case ExamSystemConstants.QuestionType.EXPLAIN:
                        case ExamSystemConstants.QuestionType.ESSAY:
                        case ExamSystemConstants.QuestionType.OPERATION:
                            Question question = new Question();
                            BuildQuestion(quesProto, question);
                            section.Questions.Add(question);
                            break;
                        default:
                            break;
                    }
                }

                paper.Sections.Add(section);
            }
            //试卷
            paperInfo = paper;
            //试卷章节列表
            markSectionBar.PaperSections = paperInfo.Sections;
            markViewBar.PaperSections = paperInfo.Sections;
            markTitleBar.SetPaperName(paperInfo.Name);
            markTitleBar.SetScoreInfo(paperInfo.TotalScore, paperInfo.PassScore);
        }

        public void BuildQuestion(ExamQuestionProto quesProto, Question question)
        {
            question.Id = quesProto.Id;
            question.BankId = quesProto.BankId;
            question.Type = quesProto.Type;
            question.Level = quesProto.Level;
            question.From = quesProto.From;
            question.Status = quesProto.Status;
            question.Content = quesProto.Content;
            question.Key = quesProto.Key;
            question.Resolve = quesProto.Resolve;
            question.Data = quesProto.Data;
        }

        /// <summary>
        /// 接受修改考试数据的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUpdateExamDataResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                PanelManager.Instance.ClosePanel(EnumPanelType.MarkPaperPanel);
                MessageBoxEx.Show("提交成功。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

    }

    /// <summary>
    /// 考试面板数据类
    /// </summary>
    public class MarkPaperPanelData
    {
        /// <summary>
        /// 考试数据
        /// </summary>
        public ExamData ExamDataInfo { get; set; }
    }
}
