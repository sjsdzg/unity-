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
    public class ManagePaperCategoryPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//状态列
        public const string COLUMN_STATUS = "Status";//状态列
        public const string COLUMN_AMOUNT = "Amount";//试题数量列
        public const string COLUMN_POSTER = "Poster";//创建人列
        public const string COLUMN_CREATEDATE = "CreateDate";//创建日期
        public const string COLUMN_MODIFIER = "Modifier";//修改人
        public const string COLUMN_MODIFYDATE = "ModifyDate";//修改日期

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManagePaperCategoryPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试卷分类模块
        /// </summary>
        private PaperCategoryModule paperCategoryModule;

        /// <summary>
        /// 试卷分类表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 试卷分类查询栏目
        /// </summary>
        private PaperCategoryQueryBar paperCategoryQueryBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            pageDataGrid = transform.Find("PageDataGrid").GetComponent<PageDataGrid>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
            paperCategoryQueryBar = transform.Find("QueryBar").GetComponent<PaperCategoryQueryBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            paperCategoryQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            paperCategoryModule = ModuleManager.Instance.GetModule<PaperCategoryModule>();
            paperCategoryModule.PagePaperCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperCategoryQueryBar.SqlConditions, ReceivePagePaperCategoryByCondition);
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
            addressBar.AddHyperButton(EnumPanelType.ManagePaperCategoryPanel, PanelDefine.GetPanelComment(EnumPanelType.ManagePaperCategoryPanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            paperCategoryModule.PagePaperCategoryByCondition(currentPage, pageSize, paperCategoryQueryBar.SqlConditions, ReceivePagePaperCategoryByCondition);
        }


        /// <summary>
        /// 试题查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            paperCategoryModule.PagePaperCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperCategoryQueryBar.SqlConditions, ReceivePagePaperCategoryByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            PaperCategory paperCategory = row.Data.Tag as PaperCategory;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPaperCategoryPanel, OperationType.Modify, paperCategory);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            if (paperCategory.Amount > 0)
                            {
                                MessageBoxEx.Show("<color=red>试卷分类中包含试题，无法删除。如需删除试卷分类，请先清空试卷分类中的试题。？</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                            }
                            else
                            {
                                paperCategoryModule.DeletePaperCategory(paperCategory.Id, ReceiveDeletePaperCategoryResp);
                            }
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
                case ButtonCellType.Insert:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPaperCategoryPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperCategoryResq(NetworkPackageInfo packageInfo)
        {
            PagePaperCategoryResp resp = PagePaperCategoryResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.PaperCategorys != null)
            {
                pageBean.DataList = BuildDataSource(resp.PaperCategorys);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="paperCategoryProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<PaperCategoryProto> paperCategoryProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < paperCategoryProtos.Count; i++)
            {
                PaperCategoryProto paperCategoryProto = paperCategoryProtos[i];
                PaperCategory paperCategory = new PaperCategory();
                paperCategory.Id = paperCategoryProto.Id;
                paperCategory.Name = paperCategoryProto.Name;
                paperCategory.Status = paperCategoryProto.Status;
                paperCategory.Poster = paperCategoryProto.Poster;
                paperCategory.CreateTime = DateTimeUtil.OfEpochMilli(paperCategoryProto.CreateTime);
                paperCategory.Modifier = paperCategoryProto.Modifier;
                paperCategory.UpdateTime = DateTimeUtil.OfEpochMilli(paperCategoryProto.UpdateTime);
                paperCategory.Amount = paperCategoryProto.Amount;
                paperCategory.Remark = paperCategoryProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = paperCategory;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, paperCategory.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(paperCategory.Status));
                rowData.CellValueDict.Add(COLUMN_AMOUNT, paperCategory.Amount);
                rowData.CellValueDict.Add(COLUMN_POSTER, paperCategory.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, paperCategory.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, paperCategory.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(paperCategory.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperCategoryByCondition(NetworkPackageInfo packageInfo)
        {
            PagePaperCategoryByConditionResp resp = PagePaperCategoryByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.PaperCategorys != null)
            {
                pageBean.DataList = BuildDataSource(resp.PaperCategorys);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除试卷分类的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeletePaperCategoryResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            paperCategoryModule.PagePaperCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperCategoryQueryBar.SqlConditions, ReceivePagePaperCategoryByCondition);
            MessageBoxEx.Show("试卷分类删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
