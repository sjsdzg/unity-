using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 管理试题Panel
    /// </summary>
    public class ManageQuestionPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_QBANK = "QBank";//试题库列
        public const string COLUMN_TYPE = "Type";//试题类型列
        public const string COLUMN_STATUS = "Status";//试题状态列
        public const string COLUMN_CONTENT = "Content";//试题体感列
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageQuestionPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试题模块
        /// </summary>
        private QuestionModule questionModule;

        /// <summary>
        /// 试题库模块
        /// </summary>
        private QuestionBankModule questionBankModule;

        /// <summary>
        /// 试题表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 试题查询栏目
        /// </summary>
        private QuestionQueryBar questionQueryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 试题库列表
        /// </summary>
        private List<QuestionBank> m_QuestionBanks;

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
            questionQueryBar = transform.Find("QueryBar").GetComponent<QuestionQueryBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            questionQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            questionModule = ModuleManager.Instance.GetModule<QuestionModule>();
            questionBankModule = ModuleManager.Instance.GetModule<QuestionBankModule>();
            //先获取所有试题库
            questionBankModule.ListQuestionBankByCondition(SqlCondition.ListBySoftwareId(), ReceiveListQuestionBankByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageQuestionPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageQuestionPanel));
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
            questionModule.PageQuestionByCondition(currentPage, pageSize, questionQueryBar.SqlConditions, ReceivePageQuestionByCondition);
        }

        /// <summary>
        /// 试题查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            questionModule.PageQuestionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionQueryBar.SqlConditions, ReceivePageQuestionByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Question question = row.Data.Tag as Question;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyQuestionPanel, OperationType.Modify, question);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            questionModule.DeleteQuestion(question.Id, ReceiveDeleteQuestionResp);
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
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道试题吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> list = new List<string>();
                            foreach (var row in rows)
                            {
                                Question question = row.Data.Tag as Question;
                                list.Add(question.Id);
                            }
                            questionModule.BatchDeleteQuestion(list, ReceiveBatchDeleteQuestionResp);
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
        private void ReceiveGetQuestionResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetQuestionResp resp = GetQuestionResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受获取题目的总数
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveCountQuestionResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            CountQuestionResp resp = CountQuestionResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.ToString());
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageQuestionResq(NetworkPackageInfo packageInfo)
        {
            PageQuestionResp resp = PageQuestionResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Questions != null)
            {
               pageBean.DataList = BuildDataSource(resp.Questions);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 接受获取所有试题库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListQuestionBankByConditionResp(NetworkPackageInfo packageInfo)
        {
            ListQuestionBankByConditionResp resp = ListQuestionBankByConditionResp.Parser.ParseFrom(packageInfo.Body);
            List<QuestionBank> questionBanks = new List<QuestionBank>();
            for (int i = 0; i < resp.QuestionBanks.Count; i++)
            {
                QuestionBankProto questionBankProto = resp.QuestionBanks[i];
                //questionBank
                QuestionBank questionBank = new QuestionBank();
                questionBank.Id = questionBankProto.Id;
                questionBank.Name = questionBankProto.Name;
                questionBank.Status = questionBankProto.Status;
                questionBank.Poster = questionBankProto.Poster;
                questionBank.CreateTime = Converter.NewDateTime(questionBankProto.CreateTime);
                questionBank.Modifier = questionBankProto.Modifier;
                questionBank.UpdateTime = Converter.NewDateTime(questionBankProto.UpdateTime);
                questionBank.Remark = questionBankProto.Remark;
                questionBanks.Add(questionBank);
            }
            //设置试题库下拉框
            questionQueryBar.SetQBank(questionBanks);
            m_QuestionBanks = questionBanks;
            //获取完题库之后, 分页查询试题。
            questionModule.PageQuestionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionQueryBar.SqlConditions, ReceivePageQuestionByCondition);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageQuestionByCondition(NetworkPackageInfo packageInfo)
        {
            PageQuestionByConditionResp resp = PageQuestionByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Questions != null)
            {
                pageBean.DataList = BuildDataSource(resp.Questions);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="questionProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<QuestionProto> questionProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < questionProtos.Count; i++)
            {
                QuestionProto questionProto = questionProtos[i];
                Question question = new Question();
                question.Id = questionProto.Id;
                question.BankId = questionProto.BankId;
                question.Type = questionProto.Type;
                question.Level = questionProto.Level;
                question.From = questionProto.From;
                question.Status = questionProto.Status;
                question.Content = questionProto.Content;
                question.Key = questionProto.Key;
                question.Resolve = questionProto.Resolve;
                question.Poster = questionProto.Poster;
                question.CreateTime = Converter.NewDateTime(questionProto.CreateTime);
                question.Modifier = questionProto.Modifier;
                question.UpdateTime = Converter.NewDateTime(questionProto.UpdateTime);
                question.Data = questionProto.Data;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = question;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());

                string qBankName = m_QuestionBanks.FirstOrDefault(X => X.Id == question.BankId).Name;
                rowData.CellValueDict.Add(COLUMN_QBANK, qBankName);

                string qType = ExamSystemConstants.QuestionType.GetComment(question.Type);
                rowData.CellValueDict.Add(COLUMN_TYPE, qType);

                string qStatus = ExamSystemConstants.Status.GetComment(question.Status);
                rowData.CellValueDict.Add(COLUMN_STATUS, qStatus);

                rowData.CellValueDict.Add(COLUMN_CONTENT, question.Content);
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受删除试题的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteQuestionResp(NetworkPackageInfo packageInfo)
        { 
            Debug.Log("删除试题成功!");
            //更新表格
            questionModule.PageQuestionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionQueryBar.SqlConditions, ReceivePageQuestionByCondition);
            MessageBoxEx.Show("试题删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除试题的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteQuestionResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteQuestionResp resp = BatchDeleteQuestionResp.Parser.ParseFrom(packageInfo.Body);
            questionModule.PageQuestionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, questionQueryBar.SqlConditions, ReceivePageQuestionByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }
}
