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
using DG.Tweening;

namespace XFramework.UI
{
    public class ExamScorePanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_PAPERNAME = "PaperName";//考试名称列
        public const string COLUMN_USERNAME = "UserName";//用户名
        public const string COLUMN_REALNAME = "RealName";//姓名
        public const string COLUMN_SCORE = "Score";//考试分数
        public const string COLUMN_ENDTime = "EndTime";//交卷时间列

        #region 考试查询条件
        public const string NAME = "name";//关键词
        public const string CATEGORY_ID = "category_id";//试卷分类Id
        #endregion

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ExamScorePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamDataModule examDataModule;

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
        private ExamScoreQueryBar queryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 考试库列表
        /// </summary>
        private List<ExamCategory> m_ExamDataCategorys;

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
            queryBar = transform.Find("QueryBar").GetComponent<ExamScoreQueryBar>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            queryBar.OnQuery.AddListener(examDataQueryBar_OnQuery);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
        }

        protected override void OnStart()
        {
            base.OnStart();

            examDataModule = ModuleManager.Instance.GetModule<ExamDataModule>();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            //先获取所有考试库
            examCategoryModule.ListExamCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListExamCategoryByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManualGradingPanel, PanelDefine.GetPanelComment(EnumPanelType.ManualGradingPanel));
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
            examDataModule.PageExamDataByCondition(currentPage, pageSize, queryBar.SqlConditions, ReceivePageExamDataByCondition);
        }

        /// <summary>
        /// 考试查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void examDataQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            examDataModule.PageExamDataByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageExamDataByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            ExamData examData = row.Data.Tag as ExamData;
            switch (type)
            {
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            examDataModule.DeleteExamData(examData.Id, ReceiveDeleteExamDataResp);
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
                                ExamData examData = row.Data.Tag as ExamData;
                                list.Add(examData.Id);
                            }
                            examDataModule.BatchDeleteExamData(list, ReceiveBatchDeleteExamDataResp);
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
        private void ReceiveGetExamDataResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetExamDataResp resp = GetExamDataResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受获取题目的总数
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveCountExamDataResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            CountExamDataResp resp = CountExamDataResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.ToString());
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamDataResq(NetworkPackageInfo packageInfo)
        {
            PageExamDataResp resp = PageExamDataResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.ExamDatas != null)
            {
                pageBean.DataList = BuildDataSource(resp.ExamDatas);
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
                ExamCategoryProto examDataCategoryProto = resp.ExamCategorys[i];
                //examDataCategory
                ExamCategory examCategory = new ExamCategory();
                examCategory.Id = examDataCategoryProto.Id;
                examCategory.Name = examDataCategoryProto.Name;
                examCategory.Status = examDataCategoryProto.Status;
                examCategory.Poster = examDataCategoryProto.Poster;
                examCategory.CreateTime = Converter.NewDateTime(examDataCategoryProto.CreateTime);
                examCategory.Modifier = examDataCategoryProto.Modifier;
                examCategory.UpdateTime = Converter.NewDateTime(examDataCategoryProto.UpdateTime);
                examCategory.Remark = examDataCategoryProto.Remark;
                examCategorys.Add(examCategory);
            }
            //设置考试库下拉框
            queryBar.SetCategory(examCategorys);
            m_ExamDataCategorys = examCategorys;
            //获取完题库之后, 分页查询考试。
            examDataModule.PageExamDataByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageExamDataByCondition);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamDataByCondition(NetworkPackageInfo packageInfo)
        {
            PageExamDataByConditionResp resp = PageExamDataByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.ExamDatas != null)
            {
                pageBean.DataList = BuildDataSource(resp.ExamDatas);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="examDataProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<ExamDataProto> examDataProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < examDataProtos.Count; i++)
            {
                ExamDataProto examDataProto = examDataProtos[i];
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

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = examData;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //考试名称
                rowData.CellValueDict.Add(COLUMN_PAPERNAME, examData.ExamName);
                //用户名
                rowData.CellValueDict.Add(COLUMN_USERNAME, examData.UserName);
                //姓名
                rowData.CellValueDict.Add(COLUMN_REALNAME, examData.RealName);
                //分数
                rowData.CellValueDict.Add(COLUMN_SCORE, examData.Score);
                //交卷时间
                rowData.CellValueDict.Add(COLUMN_ENDTime, DateTimeUtil.ToString(examData.EndTime));
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteExamDataResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log("删除考试成功!");
            //更新表格
            examDataModule.PageExamDataByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageExamDataByCondition);
            MessageBoxEx.Show("考试删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteExamDataResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteExamDataResp resp = BatchDeleteExamDataResp.Parser.ParseFrom(packageInfo.Body);
            examDataModule.PageExamDataByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageExamDataByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }
}
