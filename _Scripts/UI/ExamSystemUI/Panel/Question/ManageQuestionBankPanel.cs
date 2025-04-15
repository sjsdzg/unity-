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
    public class ManageQuestionBankPanel : AbstractPanel
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
            return EnumPanelType.ManageQuestionBankPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 题库模块
        /// </summary>
        private QuestionBankModule questionBankModule;

        /// <summary>
        /// 题库表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 题库查询栏目
        /// </summary>
        private QuestionBankQueryBar questionBankQueryBar;

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
            questionBankQueryBar = transform.Find("QueryBar").GetComponent<QuestionBankQueryBar>();
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
            questionBankQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            questionBankModule = ModuleManager.Instance.GetModule<QuestionBankModule>();
            questionBankModule.PageQuestionBankByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionBankQueryBar.SqlConditions, ReceivePageQuestionBankByCondition);
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
            addressBar.AddHyperButton(EnumPanelType.ManageQuestionBankPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageQuestionBankPanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            questionBankModule.PageQuestionBankByCondition(currentPage, pageSize, questionBankQueryBar.SqlConditions, ReceivePageQuestionBankByCondition);
        }


        /// <summary>
        /// 试题查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            questionBankModule.PageQuestionBankByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionBankQueryBar.SqlConditions, ReceivePageQuestionBankByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            QuestionBank questionBank = row.Data.Tag as QuestionBank;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyQuestionBankPanel, OperationType.Modify, questionBank);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            if (questionBank.Amount > 0)
                            {
                                MessageBoxEx.Show("<color=red>题库中包含试题，无法删除。如需删除题库，请先清空题库中的试题。？</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                            }
                            else
                            {
                                questionBankModule.DeleteQuestionBank(questionBank.Id, ReceiveDeleteQuestionBankResp);
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
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyQuestionBankPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageQuestionBankResq(NetworkPackageInfo packageInfo)
        {
            PageQuestionBankResp resp = PageQuestionBankResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.QuestionBanks != null)
            {
                pageBean.DataList = BuildDataSource(resp.QuestionBanks);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="questionBankProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<QuestionBankProto> questionBankProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < questionBankProtos.Count; i++)
            {
                QuestionBankProto questionBankProto = questionBankProtos[i];
                QuestionBank questionBank = new QuestionBank();
                questionBank.Id = questionBankProto.Id;
                questionBank.Name = questionBankProto.Name;
                questionBank.Status = questionBankProto.Status;
                questionBank.Poster = questionBankProto.Poster;
                questionBank.CreateTime = DateTimeUtil.OfEpochMilli(questionBankProto.CreateTime);
                questionBank.Modifier = questionBankProto.Modifier;
                questionBank.UpdateTime = DateTimeUtil.OfEpochMilli(questionBankProto.UpdateTime);
                questionBank.Amount = questionBankProto.Amount;
                questionBank.Remark = questionBankProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = questionBank;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, questionBank.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(questionBank.Status));
                rowData.CellValueDict.Add(COLUMN_AMOUNT, questionBank.Amount);
                rowData.CellValueDict.Add(COLUMN_POSTER, questionBank.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, questionBank.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, questionBank.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(questionBank.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageQuestionBankByCondition(NetworkPackageInfo packageInfo)
        {
            PageQuestionBankByConditionResp resp = PageQuestionBankByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.QuestionBanks != null)
            {
                pageBean.DataList = BuildDataSource(resp.QuestionBanks);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除试题库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteQuestionBankResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            questionBankModule.PageQuestionBankByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionBankQueryBar.SqlConditions, ReceivePageQuestionBankByCondition);
            MessageBoxEx.Show("试题库删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
