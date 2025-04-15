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
    public class ManageExamCategoryPanel : AbstractPanel
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
            return EnumPanelType.ManageExamCategoryPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 考试分类模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 考试分类表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 考试分类查询栏目
        /// </summary>
        private ExamCategoryQueryBar examCategoryQueryBar;

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
            examCategoryQueryBar = transform.Find("QueryBar").GetComponent<ExamCategoryQueryBar>();
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
            examCategoryQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
            examCategoryModule.PageExamCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examCategoryQueryBar.SqlConditions, ReceivePageExamCategoryByCondition);
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
            addressBar.AddHyperButton(EnumPanelType.ManageExamCategoryPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageExamCategoryPanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            examCategoryModule.PageExamCategoryByCondition(currentPage, pageSize, examCategoryQueryBar.SqlConditions, ReceivePageExamCategoryByCondition);
        }


        /// <summary>
        /// 试题查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            examCategoryModule.PageExamCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examCategoryQueryBar.SqlConditions, ReceivePageExamCategoryByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            ExamCategory examCategory = row.Data.Tag as ExamCategory;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyExamCategoryPanel, OperationType.Modify, examCategory);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            if (examCategory.Amount > 0)
                            {
                                MessageBoxEx.Show("<color=red>考试分类中包含试题，无法删除。如需删除考试分类，请先清空考试分类中的试题。？</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                            }
                            else
                            {
                                examCategoryModule.DeleteExamCategory(examCategory.Id, ReceiveDeleteExamCategoryResp);
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
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyExamCategoryPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamCategoryResq(NetworkPackageInfo packageInfo)
        {
            PageExamCategoryResp resp = PageExamCategoryResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.ExamCategorys != null)
            {
                pageBean.DataList = BuildDataSource(resp.ExamCategorys);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="examCategoryProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<ExamCategoryProto> examCategoryProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < examCategoryProtos.Count; i++)
            {
                ExamCategoryProto examCategoryProto = examCategoryProtos[i];
                ExamCategory examCategory = new ExamCategory();
                examCategory.Id = examCategoryProto.Id;
                examCategory.Name = examCategoryProto.Name;
                examCategory.Status = examCategoryProto.Status;
                examCategory.Poster = examCategoryProto.Poster;
                examCategory.CreateTime = DateTimeUtil.OfEpochMilli(examCategoryProto.CreateTime);
                examCategory.Modifier = examCategoryProto.Modifier;
                examCategory.UpdateTime = DateTimeUtil.OfEpochMilli(examCategoryProto.UpdateTime);
                examCategory.Amount = examCategoryProto.Amount;
                examCategory.Remark = examCategoryProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = examCategory;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, examCategory.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(examCategory.Status));
                rowData.CellValueDict.Add(COLUMN_AMOUNT, examCategory.Amount);
                rowData.CellValueDict.Add(COLUMN_POSTER, examCategory.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, examCategory.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, examCategory.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(examCategory.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageExamCategoryByCondition(NetworkPackageInfo packageInfo)
        {
            PageExamCategoryByConditionResp resp = PageExamCategoryByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.ExamCategorys != null)
            {
                pageBean.DataList = BuildDataSource(resp.ExamCategorys);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除考试分类的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteExamCategoryResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            examCategoryModule.PageExamCategoryByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, examCategoryQueryBar.SqlConditions, ReceivePageExamCategoryByCondition);
            MessageBoxEx.Show("考试分类删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
