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
using XFramework.Network;

namespace XFramework.UI
{
    public class MyExamPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//考试名称列
        public const string COLUMN_DURATION = "Duration";//考试时长列
        //public const string COLUMN_TOTALSCORE = "TotalScore";//考试总分列
        public const string COLUMN_EXAMDATE = "ExamDate";//创建日期列

        #region 考试查询条件
        public const string NAME = "name";//关键词
        public const string CATEGORY_ID = "category_id";//试卷分类Id
        #endregion

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
        /// 考试模块
        /// </summary>
        private ExamDataModule examDataModule;

        /// <summary>
        /// 考试表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 考试查询栏目
        /// </summary>
        private MyExamQueryBar examQueryBar;

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
            examQueryBar = transform.Find("QueryBar").GetComponent<MyExamQueryBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            examQueryBar.OnQuery.AddListener(examQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();

            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            examDataModule = ModuleManager.Instance.GetModule<ExamDataModule>();
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
                case ButtonCellType.StartExam:
                    PrepareExam(exam);//准备考试
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 准备的考试
        /// </summary>
        private Exam prepareExamInfo;

        /// <summary>
        /// 准备考试
        /// </summary>
        /// <param name="exam"></param>
        private void PrepareExam(Exam exam)
        {
            prepareExamInfo = exam;

            if (DateTime.Now < prepareExamInfo.StartTime)
            {
                MessageBoxEx.Show("还未到考试时间，无法考试！", "提示", MessageBoxExEnum.SingleDialog, null);
                return;
            }
            if (DateTime.Now > prepareExamInfo.EndTime)
            {
                MessageBoxEx.Show("已超过考试时间，无法考试！", "提示", MessageBoxExEnum.SingleDialog, null);
                return;
            }

            //Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            //StartExamPanelData data = new StartExamPanelData();
            //data.ExamInfo = prepareExamInfo;
            //PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.StartExamPanel, data);
            //examDataModule.StartExam(prepareExamInfo.Id, ReceiveStartExamResp);
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            sqlConditions.Add(new SqlCondition(Constants.EXAM_ID, SqlOption.Equal, SqlType.String, prepareExamInfo.Id));
            sqlConditions.Add(new SqlCondition(Constants.USER_ID, SqlOption.Equal, SqlType.String, GlobalManager.user.Id));
            examDataModule.CountExamDataByCondition(sqlConditions, ReceiveCountExamDataByConditionResp);
        }

        private void ReceiveCountExamDataByConditionResp(NetworkPackageInfo packageInfo)
        {
            CountExamDataByConditionResp resp = CountExamDataByConditionResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Count == 0)
            {
                Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
                StartExamPanelData data = new StartExamPanelData();
                data.ExamInfo = prepareExamInfo;
                PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.StartExamPanel, data);
            }
            else
            {
                //您已考过，不能重复考试。
                MessageBoxEx.Show("您已考试过，不能重复考试。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 接受根据用户ID和考试ID，判断是否有考试数据
        /// </summary>
        /// <param name="packageInfo"></param>
        //private void ReceiveStartExamResp(NetworkPackageInfo packageInfo)
        //{
        //    if (packageInfo.Header.Status == Status.OK)
        //    {
        //        StartExamResp resp = StartExamResp.Parser.ParseFrom(packageInfo.Body);
        //        if (resp.Can)//当前已经考试过
        //        {
        //            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
        //            StartExamPanelData data = new StartExamPanelData();
        //            data.ExamInfo = prepareExamInfo;
        //            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.StartExamPanel, data);
        //        }
        //        else
        //        {
        //            //您已考过，不能重复考试。
        //            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        //        }
        //    }
        //    else
        //    {
        //        MessageBoxEx.Show("<color=red>服务器出现异常，请联系管理员。</color>", "提示", MessageBoxExEnum.SingleDialog, null);
        //    }
        //}

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
                //考试时长
                rowData.CellValueDict.Add(COLUMN_DURATION, exam.Duration);
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
            Debug.Log("删除考试成功!");
            //更新表格
            examModule.PageExamByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examQueryBar.SqlConditions, ReceivePageExamByCondition);
            MessageBoxEx.Show("考试删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
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
