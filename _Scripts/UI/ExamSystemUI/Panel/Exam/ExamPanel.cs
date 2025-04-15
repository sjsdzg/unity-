using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using XFramework.Simulation;

namespace XFramework.UI
{
    public class ExamPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ExamPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        private ExamSectionBar examSectionBar;

        /// <summary>
        /// 序号索引视图
        /// </summary>
        private ExamViewBar examViewBar;

        /// <summary>
        /// 考试标题
        /// </summary>
        private ExamTitleBar examTitleBar;

        /// <summary>
        /// 自动隐藏通知模块
        /// </summary>
        private Notify notify;

        /// <summary>
        /// Text时间
        /// </summary>
        private Text textTime;

        /// <summary>
        /// 提示时间(分钟)
        /// </summary>
        private const int prompting_time = 5;

        /// <summary>
        /// 考试传入的信息（针对操作题）
        /// </summary>
        private ExamTransmitInfo examTransmitInfo;

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
        /// 考试基本模块
        /// </summary>
        private ExamBasicModule examBasicModule;

        /// <summary>
        /// 考试库模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamDataModule examDataModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            examSectionBar = transform.Find("Background/ExamSectionBar").GetComponent<ExamSectionBar>();
            examViewBar = transform.Find("Background/ExamViewBar").GetComponent<ExamViewBar>();
            examTitleBar = transform.Find("Background/ExamSectionBar/Viewport/Content/TitleBar").GetComponent<ExamTitleBar>();
            notify = transform.Find("Background/NotifyContainer/NotifyTemplate").GetComponent<Notify>();
            textTime = transform.Find("Background/ExamViewBar/Time/Text").GetComponent<Text>();
        }

        private void InitEvent()
        {
            examSectionBar.OnCompleted.AddListener(examSectionBar_OnCompleted);
            examSectionBar.OnEnterScene.AddListener(examSectionBar_OnEnterScene);
            examViewBar.OnClicked.AddListener(examViewBar_OnClicked);
            examViewBar.OnSubmit.AddListener(examViewBar_OnSubmit);
            examSectionBar.OnInitialized.AddListener(examSectionBar_OnInitialized);
        }

        private void examSectionBar_OnInitialized()
        {
            examSectionBar.ExamSectionJsons = JsonConvert.DeserializeObject<List<ExamSectionJson>>(examDataInfo.Data);

            if (examTransmitInfo != null)
            {
                ExamQuestion examQuestion = examSectionBar.GetExamQuestion(examTransmitInfo.SectionId, examTransmitInfo.Number);
                examQuestion.Key = JsonConvert.SerializeObject(examTransmitInfo.OperationPointJsons);
                //开始倒计时
                StartCoroutine(CutDown(examTransmitInfo.LeftTime));
            }
            else
            {
                //开始倒计时
                StartCoroutine(CutDown(examInfo.Duration * 60));
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            examBasicModule = ModuleManager.Instance.GetModule<ExamBasicModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            examDataModule = ModuleManager.Instance.GetModule<ExamDataModule>();
        }

        /// <summary>
        /// 考试章节试题完成时，触发
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void examSectionBar_OnCompleted(int sectionId, int number, bool value)
        {
            examViewBar.TurnOn(sectionId, number, value);
        }

        /// <summary>
        /// 是否切换场景
        /// </summary>
        private bool prepareChangeScene = false;

        /// <summary>
        /// 章节Id
        /// </summary>
        private int sectionId;

        /// <summary>
        /// 序号Id
        /// </summary>
        private int number;

        /// <summary>
        /// 跳场景准备工作
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void examSectionBar_OnEnterScene(int _sectionId, int _number)
        {
            prepareChangeScene = true;
            sectionId = _sectionId;
            number = _number;
            SubmitPaper();//提交试卷
        }


        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                ExamPanelData data = PanelParams[0] as ExamPanelData;
                examInfo = data.ExamInfo;
                if (data.Again)//是不是再一次进入
                {
                    List<SqlCondition> sqlConditions = new List<SqlCondition>();
                    sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
                    sqlConditions.Add(new SqlCondition(Constants.EXAM_ID, SqlOption.Equal, SqlType.String, examInfo.Id));
                    examDataModule.GetExamDataByCondition(sqlConditions, ReceiveGetExamDataByConditionResp);
                    if (data.ExamTransmitInfo != null)//从操作场景携带的考试数据
                    {
                        examTransmitInfo = data.ExamTransmitInfo;
                    }
                }
                else
                {
                    FirstEnter();
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void FirstEnter()
        {
            Debug.Log("初始化考试");
            //开始提交试卷（确定开始考试时间）
            ExamData examData = new ExamData();
            examData.UserId = GlobalManager.user.Id;
            examData.ExamId = examInfo.Id;
            examData.StartTime = DateTime.Now;
            examData.EndTime = DateTime.Now;
#if UNITY_WEBGL
            examData.Ip = "";
#else
            //examData.Ip = UnityEngine.Network.player.ipAddress;
            examData.Ip = "";
#endif
            examData.Data = "";
            examData.Check = "";
            examData.Remark = "";
            examData.Status = 0;//未交卷
            examData.Score = 0;
            examDataModule.InsertExamData(examData, ReceiveInsertExamDataResp);
        }

        private void examViewBar_OnClicked(int sectionId, int number)
        {
            examSectionBar.Center(sectionId, number);
        }

        /// <summary>
        /// 考试试图点击提交时，触发
        /// </summary>
        private void examViewBar_OnSubmit()
        {
            SubmitPaper();
        }

        /// <summary>
        /// 交卷
        /// </summary>
        private void SubmitPaper()
        {
            examDataInfo.EndTime = DateTime.Now;
            List<ExamSectionJson> examSectionJsons = examSectionBar.ExamSectionJsons;
            examDataInfo.Data = JsonConvert.SerializeObject(examSectionJsons);
            examDataInfo.Status = 1;//交卷
            examDataInfo.Check = "";
            examDataInfo.Remark = "";
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
            examDataModule.UpdateExamData(examDataInfo, ReceiveUpdateExamDataResp);
        }

        private void ReceiveUpdateExamDataResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                if (prepareChangeScene)//跳入操作场景
                {
                    ExamQuestion examQuestion = examSectionBar.GetExamQuestion(sectionId, number);
                    if (examQuestion.Question.Type == 7)
                    {
                        ExamTransmitInfo examTransmitInfo = new ExamTransmitInfo();
                        examTransmitInfo.ExamInfo = examInfo;
                        examTransmitInfo.SectionId = sectionId;
                        examTransmitInfo.Number = number;
                        examTransmitInfo.LeftTime = LeftTime;//剩余时间

                        //进入操作场景
                        string[] strs = examQuestion.Question.Data.Split(',');
                        if (strs.Length == 3)
                        {
                            StageStyle stageStyle = (StageStyle)Enum.Parse(typeof(StageStyle), strs[0]);
                            StageType stageType = (StageType)Enum.Parse(typeof(StageType), strs[1]);
                            //加载
                            StageSceneExamInfo sceneInfo = null;
                            switch (stageStyle)
                            {
                                case StageStyle.Standard:
                                    ProcedureType procedureType = (ProcedureType)Enum.Parse(typeof(ProcedureType), strs[2]);
                                    sceneInfo = new StageSceneExamInfo(stageType, ProductionMode.Examine, procedureType, examTransmitInfo);
                                    break;
                                case StageStyle.Fault:
                                    string faultID = strs[2];
                                    sceneInfo = new StageSceneExamInfo(stageType, ProductionMode.Examine, faultID, examTransmitInfo);
                                    break;
                                default:
                                    break;
                            }
                            // 指针恢复默认
                            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                            SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionSimulationScene, sceneInfo);
                        }
                    }
                }
                else
                {
                    //交卷之后，关闭窗口
                    PanelManager.Instance.ClosePanel(EnumPanelType.ExamPanel);
                    MessageBoxEx.Show("交卷成功。", "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
        }

        /// <summary>
        /// 接受添加考试数据的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertExamDataResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                Debug.Log("初始化考试成功！");
                List<SqlCondition> sqlConditions = new List<SqlCondition>();
                sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
                sqlConditions.Add(new SqlCondition(Constants.EXAM_ID, SqlOption.Equal, SqlType.String, examInfo.Id));
                examDataModule.GetExamDataByCondition(sqlConditions, ReceiveGetExamDataByConditionResp);
            }
        }

        /// <summary>
        /// 接受根据用户名和考试ID的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamDataByConditionResp(NetworkPackageInfo packageInfo)
        {
            GetExamDataByConditionResp resp = GetExamDataByConditionResp.Parser.ParseFrom(packageInfo.Body);
            ExamDataProto examDataProto = resp.ExamData;
            ExamData examData = new ExamData();
            examData.Id = examDataProto.Id;
            examData.ExamId = examDataProto.ExamId;
            examData.UserId = examDataProto.UserId;
            examData.StartTime = DateTimeUtil.OfEpochMilli(examDataProto.StartTime);
            examData.EndTime = DateTimeUtil.OfEpochMilli(examDataProto.EndTime);
            examData.Ip = examDataProto.Ip;
            examData.Score = examDataProto.Score;
            examData.Status = examDataProto.Status;
            examData.Data = examDataProto.Data;
            examData.Check = examDataProto.Check;
            examData.Remark = examDataProto.Remark;
            examData.TotalScore = examDataProto.TotalScore;
            examData.PassScore = examDataProto.PassScore;
            examData.ExamName = examDataProto.ExamName;
            examData.UserName = examDataProto.UserName;
            examData.RealName = examDataProto.RealName;
            examData.PaperId = examDataProto.PaperId;
            //设置
            examDataInfo = examData;
            //设置标题
            examTitleBar.SetExamInfo(examInfo.StartTime, examInfo.EndTime, examInfo.Duration);
            //获取考试试卷
            examBasicModule.GetExamPaper(examInfo.PaperId, ReceiveGetExamPaperResp);

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
            examSectionBar.PaperSections = paper.Sections;
            examViewBar.PaperSections = paper.Sections;
            examTitleBar.SetPaperName(paper.Name);
            examTitleBar.SetScoreInfo(paper.TotalScore, paper.PassScore);
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
        /// 剩余多少时间
        /// </summary>
        private int LeftTime = 0;
        /// <summary>
        /// 倒计时协程
        /// </summary>
        /// <param name="Length">秒</param>
        /// <returns></returns>
        IEnumerator CutDown(int Length)
        {
            TimeSpan tmspan = new TimeSpan(0, 0, Length);
            LeftTime = (int)tmspan.TotalSeconds;
            while (tmspan.TotalSeconds > 0)
            {
                LeftTime -= 1;
                tmspan = new TimeSpan(0, 0, LeftTime);
                textTime.text = tmspan.ToString();
                yield return new WaitForSeconds(1);
                //考试最后时间，提醒考生注意。
                if (LeftTime == prompting_time * 60)
                {
                    notify.Template().Show("注意还有5分钟！\n时间结束后，如果您没有交卷，试卷将自动提交。", 15f);
                }
            }

            //考试试卷完成，提交试卷
            SubmitPaper();
        }
    }

    /// <summary>
    /// 考试面板数据类
    /// </summary>
    public class ExamPanelData
    {
        /// <summary>
        /// 是否是再一次
        /// </summary>
        public bool Again = false;
        /// <summary>
        /// 考试数据
        /// </summary>
        public Exam ExamInfo { get; set; }
        /// <summary>
        /// 考试传送的关键消息
        /// </summary>
        public ExamTransmitInfo ExamTransmitInfo { get; set; }
    }

    /// <summary>
    /// 考试传送的关键消息
    /// </summary>
    public class ExamTransmitInfo
    {
        /// <summary>
        /// 考试数据
        /// </summary>
        public Exam ExamInfo { get; set; }
        /// <summary>
        /// 章节Id
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// 序号Id
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 剩余时间
        /// </summary>
        public int LeftTime { get; set; }

        /// <summary>
        /// 操作点Json列表
        /// </summary>
        public List<OperationPointJson> OperationPointJsons = new List<OperationPointJson>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("章节Id:" + SectionId + "    ");
            sb.Append("序号Id:" + Number + "    ");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 操作点
    /// </summary>
    public class OperationPointJson
    {
        /// <summary>
        /// 考核ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
    }

}
