using UnityEngine;
using System.Collections;
using XFramework.Module;
using UnityEngine.UI;
using System;
using XFramework.Core;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 考试分析
    /// </summary>
    public class ExamAnalysisPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ExamAnalysisPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 分数统计
        /// </summary>
        private ScoreForm scoreForm;

        /// <summary>
        /// 用户统计
        /// </summary>
        private UserForm userForm;

        /// <summary>
        /// 时间统计
        /// </summary>
        private TimeForm timeForm;

        /// <summary>
        /// 考试名称InputField
        /// </summary>
        private InputField inputFieldExam;

        /// <summary>
        /// 选择按钮
        /// </summary>
        private Button buttonSelect;

        /// <summary>
        /// 考试基本模块
        /// </summary>
        private ExamBasicModule examBasicModule;

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
                    ResetForm();
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
            inputFieldExam = transform.Find("FormBar/List/Name/Value/InputField").GetComponent<InputField>();
            buttonSelect = transform.Find("FormBar/List/Name/Value/ButtonSelect").GetComponent<Button>();
            scoreForm = transform.Find("FormBar/List/Result/Value/ScoreForm").GetComponent<ScoreForm>();
            userForm = transform.Find("FormBar/List/Result/Value/UserForm").GetComponent<UserForm>();
            timeForm = transform.Find("FormBar/List/Result/Value/TimeForm").GetComponent<TimeForm>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ExamAnalysisPanel, PanelDefine.GetPanelComment(EnumPanelType.ExamAnalysisPanel));
        }

        protected override void OnStart()
        {
            base.OnStart();
            examBasicModule = ModuleManager.Instance.GetModule<ExamBasicModule>();
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
            examBasicModule.GetExamAnalysis(App.Instance.SoftwareId, exam.Id, ReceiveGetExamAnalysis);
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamAnalysis(NetworkPackageInfo packageInfo)
        {
            ExamAnalysisResp resp = ExamAnalysisResp.Parser.ParseFrom(packageInfo.Body);
            scoreForm.SetValues(resp.ScoreForm.Max, resp.ScoreForm.Min, resp.ScoreForm.Average);
            userForm.SetValues(resp.UserForm.PassNumber, resp.UserForm.PassRate, resp.UserForm.ShouldNumber, resp.UserForm.AttendNumber, resp.UserForm.AttendRate, resp.UserForm.AbsentNumber);
            timeForm.SetValues(resp.TimeForm.Earliest, resp.TimeForm.Latest, resp.TimeForm.Longest, resp.TimeForm.Shortest);
        }

        private void ResetForm()
        {
            scoreForm.SetValues("-", "-", "-");
            userForm.SetValues(0, 0, 0, 0, 0, 0);
            timeForm.SetValues("-", "-", "-", "-");
        }
    }
}

