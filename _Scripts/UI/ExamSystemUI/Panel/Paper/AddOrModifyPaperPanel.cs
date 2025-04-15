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
    public class AddOrModifyPaperPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyPaperPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试题模块
        /// </summary>
        private PaperModule paperModule;

        /// <summary>
        /// 试卷分类模块
        /// </summary>
        private PaperCategoryModule paperCategoryModule;

        /// <summary>
        /// 试卷分类列表
        /// </summary>
        private List<PaperCategory> m_PaperCategorys;

        /// <summary>
        /// 试卷基础栏
        /// </summary>
        private PaperBasicBar paperBasicBar;

        /// <summary>
        /// 试卷验证栏
        /// </summary>
        private PaperRemarkBar paperRemarkBar;

        /// <summary>
        /// 试卷设置栏
        /// </summary>
        private PaperSettingBar paperSettingBar;

        /// <summary>
        /// 试题提交栏
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
            paperBasicBar = transform.Find("PaperBasicBar").GetComponent<PaperBasicBar>();
            paperRemarkBar = transform.Find("PaperRemarkBar").GetComponent<PaperRemarkBar>();
            paperSettingBar = transform.Find("PaperSettingBar").GetComponent<PaperSettingBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            paperSettingBar.OnCalculateTotal.AddListener(paperSettingBar_OnCalculateTotal);
            submitBar.OnSubmit.AddListener(submitBar_OnSubmit);
            submitBar.OnCancel.AddListener(submitBar_OnCancel);
        }

        protected override void OnStart()
        {
            base.OnStart();

            paperModule = ModuleManager.Instance.GetModule<PaperModule>();
            paperCategoryModule = ModuleManager.Instance.GetModule<PaperCategoryModule>();
            //先获取所有试题库
            paperCategoryModule.ListPaperCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListPaperCategoryByConditionResp);
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
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyPaperPanel, "修改试卷");
                    Paper paper = PanelParams[1] as Paper;

                    InitModifyContent(paper);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyPaperPanel, "添加试卷");
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
        /// 计算总分时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void paperSettingBar_OnCalculateTotal(int value)
        {
            paperBasicBar.TotalScore = value;
        }

        /// <summary>
        /// 点击提交时，触发
        /// </summary>
        private void submitBar_OnSubmit()
        {
            //计算总分
            paperSettingBar.CalculateTotalScore();

            bool basic = paperBasicBar.Validate();
            bool content = paperRemarkBar.Validate();
            bool paperSetting = paperSettingBar.Validate();

            if (basic && content && paperSetting)
            {
                Paper paper = new Paper();
                paper.Name = paperBasicBar.PaperName;
                paper.CategoryId = paperBasicBar.CategoryId;
                paper.Status = paperBasicBar.Status;
                paper.TotalScore = paperBasicBar.TotalScore;
                paper.PassScore = paperBasicBar.PassScore;
                paper.Remark = paperRemarkBar.Content;

                List<PaperSectionJson> sectionJsons = new List<PaperSectionJson>();
                List<PaperSection> paperSections = paperSettingBar.PaperSections;
                for (int i = 0; i < paperSections.Count; i++)
                {
                    PaperSectionJson sectionJson = new PaperSectionJson();
                    PaperSection section = paperSections[i];
                    sectionJson.Id = section.Id;
                    sectionJson.Name = section.Name;
                    sectionJson.Remark = section.Remark;
                    sectionJson.QType = section.QType;
                    sectionJson.Number = section.Number;
                    sectionJson.Score = section.Score;
                    for (int j = 0; j < section.Questions.Count; j++)
                    {
                        SectionQuesJson quesJson = new SectionQuesJson();
                        Question ques = section.Questions[j];
                        quesJson.Id = ques.Id;
                        quesJson.Type = ques.Type;
                        quesJson.Content = ques.Content;
                        sectionJson.QuesJsons.Add(quesJson);
                    }
                    sectionJsons.Add(sectionJson);
                }

                paper.Data = JsonConvert.SerializeObject(sectionJsons);

                switch (operationType)
                {
                    case OperationType.Add:
                        paperModule.InsertPaper(paper, ReceiveInsertPaperResp);
                        break;
                    case OperationType.Modify:
                        paper.Id = modifyPaper.Id;
                        paperModule.UpdatePaper(paper, ReceiveUpdatePaperResp);
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
        /// 修改试卷
        /// </summary>
        private Paper modifyPaper;

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="paper"></param>
        private void InitModifyContent(Paper paper)
        {
            modifyPaper = paper;
            //设置
            paperBasicBar.PaperName = paper.Name;
            paperBasicBar.CategoryId = paper.CategoryId;
            paperBasicBar.Status = paper.Status;
            paperBasicBar.TotalScore = paper.TotalScore;
            paperBasicBar.PassScore = paper.PassScore;
            paperRemarkBar.Content = paper.Remark;

            List<PaperSectionJson> sectionJsons = JsonConvert.DeserializeObject<List<PaperSectionJson>>(paper.Data);
            List<PaperSection> paperSections = new List<PaperSection>();
            for (int i = 0; i < sectionJsons.Count; i++)
            {
                PaperSection section = new PaperSection();
                PaperSectionJson sectionJson = sectionJsons[i];
                section.Id = sectionJson.Id;
                section.Name = sectionJson.Name;
                section.Remark = sectionJson.Remark;
                section.QType = sectionJson.QType;
                section.Number = sectionJson.Number;
                section.Score = sectionJson.Score;
                for (int j = 0; j < sectionJson.QuesJsons.Count; j++)
                {
                    SectionQuesJson quesJson = sectionJson.QuesJsons[j]; ;
                    Question ques = new Question();
                    ques.Id = quesJson.Id;
                    ques.Type = quesJson.Type;
                    ques.Content = quesJson.Content;
                    section.Questions.Add(ques);
                }
                paperSections.Add(section);
            }

            paperSettingBar.PaperSections = paperSections;
        }

        private void ReceiveListPaperCategoryByConditionResp(NetworkPackageInfo packageInfo)
        {
            ListPaperCategoryByConditionResp resp = ListPaperCategoryByConditionResp.Parser.ParseFrom(packageInfo.Body);
            List<PaperCategory> paperCategorys = new List<PaperCategory>();
            for (int i = 0; i < resp.PaperCategorys.Count; i++)
            {
                PaperCategoryProto paperCategoryProto = resp.PaperCategorys[i];
                //paperCategory
                PaperCategory paperCategory = new PaperCategory();
                paperCategory.Id = paperCategoryProto.Id;
                paperCategory.Name = paperCategoryProto.Name;
                paperCategory.Status = paperCategoryProto.Status;
                paperCategory.Poster = paperCategoryProto.Poster;
                paperCategory.CreateTime = Converter.NewDateTime(paperCategoryProto.CreateTime);
                paperCategory.Modifier = paperCategoryProto.Modifier;
                paperCategory.UpdateTime = Converter.NewDateTime(paperCategoryProto.UpdateTime);
                paperCategory.Remark = paperCategoryProto.Remark;
                paperCategorys.Add(paperCategory);
            }
            //设置试题库下拉框
            paperBasicBar.SetCategory(paperCategorys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertPaperResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                InsertPaperResp respProto = InsertPaperResp.Parser.ParseFrom(packageInfo.Body);
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

        private void ReceiveUpdatePaperResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("试题修改成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult =>
                {
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManagePaperPanel);
                });
            }
        }

    }
}
