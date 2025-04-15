using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Common;
using DG.Tweening;

namespace XFramework.UI
{
    public class SelectExamPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_Category = "Category";//考试分类列
        public const string COLUMN_NAME = "Name";//考试体感列
        public const string COLUMN_SWITCH = "SwitchCell";//考试体感列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.SelectExamPanel;
        }

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        /// <summary>
        /// 考试分类模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 考试查询栏目
        /// </summary>
        private ExamQuerysBar examQuerysBar;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 考试分类列表
        /// </summary>
        private List<ExamCategory> m_ExamCategorys;

        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        /// <summary>
        /// 
        /// </summary>
        private SelectExamPanelData m_PanelData;

        /// <summary>
        /// 
        /// </summary>
        private Exam passedExam;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Parent.GetComponent<Image>().enabled = false;
        }

        private void InitGUI()
        {
            background = transform.Find("Background").GetComponent<RectTransform>();
            buttonClose = transform.Find("Background/Title/ButtonClose").GetComponent<Button>();
            pageDataGrid = transform.Find("Background/Panel/PageDataGrid").GetComponent<PageDataGrid>();
            examQuerysBar = transform.Find("Background/Panel/QueryBar").GetComponent<ExamQuerysBar>();
        }

        private void InitEvent()
        {
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            pageDataGrid.OnSwitchChanged.AddListener(pageDataGrid_OnSwitchChanged);
            examQuerysBar.OnQuery.AddListener(examQuerysBar_OnQuery);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.SelectExamPanel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            //先获取所有考试分类
            examCategoryModule.ListExamCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListExamCategoryByConditionResp);
            //动态显示
            background.DOScale(0, 0.3f).From();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                m_PanelData = PanelParams[0] as SelectExamPanelData;
                if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ExamAnalysisPanel)
                {
                    ExamAnalysisPanel examAnalysisPanel = m_PanelData.Panel as ExamAnalysisPanel;
                    passedExam = examAnalysisPanel.Exam;
                }
                else if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ScoreAnalysisPanel)
                {
                    ScoreAnalysisPanel scoreAnalysisPanel = m_PanelData.Panel as ScoreAnalysisPanel;
                    passedExam = scoreAnalysisPanel.Exam;
                }  
            }
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            examModule.PageExamByCondition(currentPage, pageSize, examQuerysBar.SqlConditions, ReceivePageExamByCondition);
        }

        /// <summary>
        /// 考试查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void examQuerysBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQuerysBar.SqlConditions, ReceivePageExamByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Exam exam = row.Data.Tag as Exam;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyExamPanel, OperationType.Modify, exam);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            examModule.DeleteExam(exam.Id, ReceiveDeleteExamResp);
                        }
                    });
                    break;
                default:
                    break;
            }
        }


        private void batchActionBar_ButtonCellClick(BatchActionBar arg0, ButtonCellType type)
        {
            switch (type)
            {
                case ButtonCellType.SelectAll:
                    List<DataGridViewRow> rows = pageDataGrid.GetVisibleRows();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataGridViewRow row = rows[i];
                        row.SetData(COLUMN_SWITCH, false);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// switch改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnSwitchChanged(DataGridViewRow row, bool value)
        {
            if (value)
            {
                if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ExamAnalysisPanel)
                {
                    ExamAnalysisPanel examAnalysisPanel = m_PanelData.Panel as ExamAnalysisPanel;
                    examAnalysisPanel.Exam = null;
                }
                else if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ScoreAnalysisPanel)
                {
                    ScoreAnalysisPanel scoreAnalysisPanel = m_PanelData.Panel as ScoreAnalysisPanel;
                    scoreAnalysisPanel.Exam = null;
                }
            }
            else
            {
                if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ExamAnalysisPanel)
                {
                    ExamAnalysisPanel examAnalysisPanel = m_PanelData.Panel as ExamAnalysisPanel;
                    examAnalysisPanel.Exam = row.Data.Tag as Exam;
                }
                else if (m_PanelData.Panel.GetPanelType() == EnumPanelType.ScoreAnalysisPanel)
                {
                    ScoreAnalysisPanel scoreAnalysisPanel = m_PanelData.Panel as ScoreAnalysisPanel;
                    scoreAnalysisPanel.Exam = row.Data.Tag as Exam;
                }
            }

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            PanelManager.Instance.ClosePanel(EnumPanelType.SelectExamPanel);
        }

        /// <summary>
        /// 接受获取题目的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetExamResp resp = GetExamResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamResq(NetworkPackageInfo packageInfo)
        {
            PageExamResp resp = PageExamResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Exams != null)
            {
                pageBean.DataList = BuildDataSource(resp.Exams);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 接受获取所有考试分类的响应
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
            //设置考试分类下拉框
            examQuerysBar.SetCategory(examCategorys);
            m_ExamCategorys = examCategorys;
            //获取完题库之后, 分页查询考试。
            examModule.PageExamByCondition(1, pageDataGrid.PageSize, examQuerysBar.SqlConditions, ReceivePageExamByCondition);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamByCondition(NetworkPackageInfo packageInfo)
        {
            PageExamByConditionResp resp = PageExamByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Exams != null)
            {
                pageBean.DataList = BuildDataSource(resp.Exams);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="examProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<ExamProto> examProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < examProtos.Count; i++)
            {
                ExamProto examProto = examProtos[i];
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

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = exam;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //考试分类
                ExamCategory examCategory = m_ExamCategorys.FirstOrDefault(X => X.Id == exam.CategoryId);
                string categoryName = examCategory == null ? "" : examCategory.Name;
                rowData.CellValueDict.Add(COLUMN_Category, categoryName);
                //考试名称
                rowData.CellValueDict.Add(COLUMN_NAME, exam.Name);

                if (passedExam != null && passedExam.Id == exam.Id)
                {
                    rowData.CellValueDict.Add(COLUMN_SWITCH, false);//Switch
                }
                else
                {
                    rowData.CellValueDict.Add(COLUMN_SWITCH, true);//Switch
                }

                dataSource.Add(rowData);
            }

            return dataSource;

        }

        /// <summary>
        /// 接受删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteExamResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log("删除考试成功!");
            //更新表格
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQuerysBar.SqlConditions, ReceivePageExamByCondition);
            MessageBoxEx.Show("考试删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteExamResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteExamResp resp = BatchDeleteExamResp.Parser.ParseFrom(packageInfo.Body);
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQuerysBar.SqlConditions, ReceivePageExamByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }

    public class SelectExamPanelData
    {
        /// <summary>
        /// 面板
        /// </summary>
        public AbstractPanel Panel { get; set; }
    }
}
