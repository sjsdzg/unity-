using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;

namespace XFramework.UI
{
    public class ManageExamPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//考试名称列
        public const string COLUMN_STATUS = "Status";//考试状态列
        public const string COLUMN_CATEGORY = "Category";//考试总分列
        public const string COLUMN_EXAMDATE = "ExamDate";//创建日期列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageExamPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        /// <summary>
        /// 考试库模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 考试查询栏目
        /// </summary>
        private ExamQueryBar examQueryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 考试库列表
        /// </summary>
        private List<ExamCategory> m_ExamCategorys;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            pageDataGrid = transform.Find("PageDataGrid").GetComponent<PageDataGrid>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
            examQueryBar = transform.Find("QueryBar").GetComponent<ExamQueryBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            examQueryBar.OnQuery.AddListener(examQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            //先获取所有考试库
            examCategoryModule.ListExamCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListExamCategoryByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageExamPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageExamPanel));
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
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            examModule.PageExamByCondition(currentPage, pageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
        }

        /// <summary>
        /// 考试查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void examQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
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
                case ButtonCellType.BatchDelete:
                    List<DataGridViewRow> rows = pageDataGrid.GetRowsByChecked();
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道考试吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> list = new List<string>();
                            foreach (var row in rows)
                            {
                                Exam exam = row.Data.Tag as Exam;
                                list.Add(exam.Id);
                            }
                            examModule.BatchDeleteExam(list, ReceiveBatchDeleteExamResp);
                        }
                    });

                    break;
                case ButtonCellType.Export:
                    break;
                default:
                    break;
            }
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
        /// 接受获取题目的总数
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveCountExamResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            CountExamResp resp = CountExamResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.ToString());
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
        /// 接受获取所有考试库的响应
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
            examQueryBar.SetCategory(examCategorys);
            m_ExamCategorys = examCategorys;
            //获取完题库之后, 分页查询考试。
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
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
                //考试名称

                rowData.CellValueDict.Add(COLUMN_NAME, exam.Name);
                //考试状态
                string status = ExamSystemConstants.Status.GetComment(exam.Status);
                rowData.CellValueDict.Add(COLUMN_STATUS, status);
                //考试分类
                ExamCategory examCategory = m_ExamCategorys.FirstOrDefault(X => X.Id == exam.CategoryId);
                string categoryName = "";
                if (examCategory != null)
                {
                    categoryName = examCategory.Name;
                }
                rowData.CellValueDict.Add(COLUMN_CATEGORY, categoryName);
                //创建日期
                rowData.CellValueDict.Add(COLUMN_EXAMDATE, DateTimeUtil.ToString(exam.StartTime) + " ~ " + DateTimeUtil.ToString(exam.EndTime));
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
            DeleteExamResp resp = DeleteExamResp.Parser.ParseFrom(packageInfo.Body);
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
            Debug.Log(resp.Detail);
            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteExamResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteExamResp resp = BatchDeleteExamResp.Parser.ParseFrom(packageInfo.Body);
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }
}
