using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using Newtonsoft.Json;
using XFramework.Network;
using XFramework.Common;

namespace XFramework.UI
{
    public class AddOrModifyExamPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyExamPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试卷模块
        /// </summary>
        private PaperModule paperModule;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        /// <summary>
        /// 考试分类模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试分类列表
        /// </summary>
        private List<ExamCategory> m_ExamCategorys;

        /// <summary>
        /// 考试基础栏
        /// </summary>
        private ExamBasicBar examBasicBar;

        /// <summary>
        /// 考试说明栏
        /// </summary>
        private ExamRemarkBar examRemarkBar;

        /// <summary>
        /// 考试提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

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

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            examBasicBar = transform.Find("ExamBasicBar").GetComponent<ExamBasicBar>();
            examRemarkBar = transform.Find("ExamRemarkBar").GetComponent<ExamRemarkBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(submitBar_OnSubmit);
            submitBar.OnCancel.AddListener(submitBar_OnCancel);
        }

        protected override void OnStart()
        {
            base.OnStart();

            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            paperModule = ModuleManager.Instance.GetModule<PaperModule>();
            //先获取所有考试库
            examCategoryModule.ListExamCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListExamCategoryByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                operationType = (OperationType)PanelParams[0];
                if (operationType == OperationType.Modify)
                {
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyExamPanel, "修改考试");
                    Exam exam = PanelParams[1] as Exam;

                    InitModifyContent(exam);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyExamPanel, "添加考试");
            }
        }

        /// <summary>
        /// 地址栏点击时，触发
        /// </summary>
        /// <param name="type"></param>
        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        /// <summary>
        /// 点击提交时，触发
        /// </summary>
        private void submitBar_OnSubmit()
        {
            bool basic = examBasicBar.Validate();
            bool remark = examRemarkBar.Validate();

            if (basic && remark)
            {
                Exam exam = new Exam();
                exam.Name = examBasicBar.ExamName;
                exam.PaperId = examBasicBar.Paper.Id;
                exam.CategoryId = examBasicBar.CategoryId;
                exam.Status = examBasicBar.Status;
                exam.Duration = examBasicBar.Durtation;
                exam.StartTime = examBasicBar.StartTime;
                exam.EndTime = examBasicBar.EndTime;
                exam.ShowTime = examBasicBar.ShowTime;
                exam.QuestionOrder = examBasicBar.QuestionOrder;
                exam.ShowKey = examBasicBar.ShowKey;
                exam.ShowMode = examBasicBar.ShowMode;
                exam.Remark = examRemarkBar.Content;

                switch (operationType)
                {
                    case OperationType.Add:
                        examModule.InsertExam(exam, ReceiveInsertExamResp);
                        break;
                    case OperationType.Modify:
                        exam.Id = modifyExam.Id;
                        examModule.UpdateExam(exam, ReceiveUpdateExamResp);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 取消提交时，触发
        /// </summary>
        private void submitBar_OnCancel()
        {

        }

        /// <summary>
        /// 修改考试
        /// </summary>
        private Exam modifyExam;

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="exam"></param>
        private void InitModifyContent(Exam exam)
        {
            modifyExam = exam;

            examBasicBar.ExamName = exam.Name;
            examBasicBar.CategoryId = exam.CategoryId;
            examBasicBar.Status = exam.Status;
            examBasicBar.Durtation = exam.Duration;
            examBasicBar.StartTime = exam.StartTime;
            examBasicBar.EndTime = exam.EndTime;
            examBasicBar.ShowTime = exam.ShowTime;
            examBasicBar.QuestionOrder = exam.QuestionOrder;
            examBasicBar.ShowKey = exam.ShowKey;
            examBasicBar.ShowMode = exam.ShowMode;
            examRemarkBar.Content = exam.Remark;
            //获取试卷
            paperModule.GetPaper(exam.PaperId, ReceiveGetPaperResp);
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
            paper.CreateTime = Converter.NewDateTime(paperProto.CreateTime);
            paper.Modifier = paperProto.Modifier;
            paper.UpdateTime = Converter.NewDateTime(paperProto.UpdateTime);
            paper.Remark = paperProto.Remark;
            paper.Data = paperProto.Data;

            examBasicBar.Paper= paper;
        }

        /// <summary>
        /// 接受所有考试仓库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListExamCategoryByConditionResp(NetworkPackageInfo packageInfo)
        {
            ListExamCategoryByConditionResp resp = ListExamCategoryByConditionResp.Parser.ParseFrom(packageInfo.Body);
            List<ExamCategory> examCategorys = new List<ExamCategory>();
            for (int i = 0; i < resp.ExamCategorys.Count; i++)
            {
                ExamCategoryProto examCategoryProto = resp.ExamCategorys[i];
                //examCategory
                ExamCategory examCategory = new ExamCategory();
                examCategory.Id = examCategoryProto.Id;
                examCategory.Name = examCategoryProto.Name;
                examCategory.Status = examCategoryProto.Status;
                examCategory.Poster = examCategoryProto.Poster;
                examCategory.CreateTime = Converter.NewDateTime(examCategoryProto.CreateTime);
                examCategory.Modifier = examCategoryProto.Modifier;
                examCategory.UpdateTime = Converter.NewDateTime(examCategoryProto.UpdateTime);
                examCategory.Remark = examCategoryProto.Remark;
                examCategorys.Add(examCategory);
            }
            //设置考试库下拉框
            examBasicBar.SetCategory(examCategorys);
        }

        /// <summary>
        /// 接受插入考试响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertExamResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                InsertExamResp respProto = InsertExamResp.Parser.ParseFrom(packageInfo.Body);
                if (respProto.Success)
                {
                    MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
                }
                else
                {
                    MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
        }

        /// <summary>
        /// 接受更新考试响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUpdateExamResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("考试修改成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult =>
                {
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageExamPanel);
                });
            }
        }
    }
}
