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
    public class ManagePaperPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//试卷名称列
        public const string COLUMN_STATUS = "Status";//试卷状态列
        public const string COLUMN_TOTAL = "Total";//试卷总分列
        public const string COLUMN_POSTER = "Poster";//创建人列
        public const string COLUMN_CREATEDATE = "CreateDate";//创建日期列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManagePaperPanel;
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
        /// 试卷库模块
        /// </summary>
        private PaperCategoryModule paperCategoryModule;

        /// <summary>
        /// 试卷表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 试卷查询栏目
        /// </summary>
        private PaperQueryBar paperQueryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 试卷库列表
        /// </summary>
        private List<PaperCategory> m_PaperCategorys;

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
            paperQueryBar = transform.Find("QueryBar").GetComponent<PaperQueryBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            paperQueryBar.OnQuery.AddListener(paperQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            paperModule = ModuleManager.Instance.GetModule<PaperModule>();
            paperCategoryModule = ModuleManager.Instance.GetModule<PaperCategoryModule>();
            //先获取所有试卷库
            paperCategoryModule.ListPaperCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListPaperCategoryByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManagePaperPanel, PanelDefine.GetPanelComment(EnumPanelType.ManagePaperPanel));
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
            paperModule.PagePaperByCondition(currentPage, pageSize, paperQueryBar.SqlConditions, ReceivePagePaperByCondition);
        }

        /// <summary>
        /// 试卷查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void paperQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQueryBar.SqlConditions, ReceivePagePaperByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Paper paper = row.Data.Tag as Paper;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPaperPanel, OperationType.Modify, paper);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            paperModule.DeletePaper(paper.Id, ReceiveDeletePaperResp);
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
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道试卷吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> list = new List<string>();
                            foreach (var row in rows)
                            {
                                Paper paper = row.Data.Tag as Paper;
                                list.Add(paper.Id);
                            }
                            paperModule.BatchDeletePaper(list, ReceiveBatchDeletePaperResp);
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
        private void ReceiveGetPaperResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetPaperResp resp = GetPaperResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受获取题目的总数
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveCountPaperResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            CountPaperResp resp = CountPaperResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.ToString());
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperResq(NetworkPackageInfo packageInfo)
        {
            PagePaperResp resp = PagePaperResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Papers != null)
            {
                pageBean.DataList = BuildDataSource(resp.Papers);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 接受获取所有试卷库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
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
            //设置试卷库下拉框
            paperQueryBar.SetCategory(paperCategorys);
            m_PaperCategorys = paperCategorys;
            //获取完题库之后, 分页查询试卷。
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQueryBar.SqlConditions, ReceivePagePaperByCondition);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperByCondition(NetworkPackageInfo packageInfo)
        {
            PagePaperByConditionResp resp = PagePaperByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Papers != null)
            {
                pageBean.DataList = BuildDataSource(resp.Papers);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="paperProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<PaperProto> paperProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < paperProtos.Count; i++)
            {
                PaperProto paperProto = paperProtos[i];
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

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = paper;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //试卷名称
                //string categoryName = "";
                //PaperCategory paperCategory = m_PaperCategorys.FirstOrDefault(X => X.Id == paper.CategoryId);
                //if (paperCategory != null)
                //{
                //    categoryName = paperCategory.Name;
                //}
                rowData.CellValueDict.Add(COLUMN_NAME, paper.Name);
                //试卷状态
                string status = ExamSystemConstants.Status.GetComment(paper.Status);
                rowData.CellValueDict.Add(COLUMN_STATUS, status);
                //试卷总分
                string total = paper.TotalScore.ToString();
                rowData.CellValueDict.Add(COLUMN_TOTAL, paper.Poster);
                //创建人
                rowData.CellValueDict.Add(COLUMN_POSTER, paper.Poster);
                //创建日期
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, DateTimeUtil.ToString(paper.CreateTime));
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受删除试卷的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeletePaperResp(NetworkPackageInfo packageInfo)
        {
            DeletePaperResp resp = DeletePaperResp.Parser.ParseFrom(packageInfo.Body);
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQueryBar.SqlConditions, ReceivePagePaperByCondition);
            Debug.Log(resp.Detail);
            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除试卷的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeletePaperResp(NetworkPackageInfo packageInfo)
        {
            BatchDeletePaperResp resp = BatchDeletePaperResp.Parser.ParseFrom(packageInfo.Body);
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQueryBar.SqlConditions, ReceivePagePaperByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }
}
