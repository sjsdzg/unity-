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
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 用户操作日志数据管理Panel
    /// </summary>
    public class UserOperationLogPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_REALNAME = "RealName";//实验名称列
        public const string COLUMN_CREATETIME = "CreateTime";//创建时间列
        public const string COLUMN_UPDATETIME = "UpdateTime";//更新时间列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.UserOperationLogPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户操作日志模块
        /// </summary>
        private UserOperationLogModule operationLogModule;


        /// <summary>
        /// 用户操作日志表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 用户操作日志查询栏目
        /// </summary>
        private UserOperationLogQueryBar queryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

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
            queryBar = transform.Find("QueryBar").GetComponent<UserOperationLogQueryBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            batchActionBar.ButtonCellClick.AddListener(batchActionBar_ButtonCellClick);
            queryBar.OnQuery.AddListener(userQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            operationLogModule = ModuleManager.Instance.GetModule<UserOperationLogModule>();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageOperationLogPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageOperationLogPanel));
            addressBar.AddHyperButton(EnumPanelType.UserOperationLogPanel, PanelDefine.GetPanelComment(EnumPanelType.UserOperationLogPanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                User user = PanelParams[0] as User;
                queryBar.SetUser(user);
            }

            //分页查询
            operationLogModule.PageUserOperationLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserOperationLogByCondition);
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
            operationLogModule.PageUserOperationLogByCondition(currentPage, pageSize, queryBar.SqlConditions, ReceivePageUserOperationLogByCondition);
        }

        /// <summary>
        /// 用户操作日志查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void userQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            operationLogModule.PageUserOperationLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserOperationLogByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            UserOperationLog operationLog = row.Data.Tag as UserOperationLog;
            switch (type)
            {
                case ButtonCellType.Detail:
                    PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.OperationLogDetailPanel, operationLog);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            operationLogModule.DeleteUserOperationLog(operationLog.Id, ReceiveDeleteUserOperationLogResp);
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
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道用户操作日志吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> ids = new List<string>();
                            foreach (var row in rows)
                            {
                                UserOperationLog operationLog = row.Data.Tag as UserOperationLog;
                                ids.Add(operationLog.Id);
                            }
                            operationLogModule.BatchDeleteUserOperationLog(ids, ReceiveBatchDeleteUserOperationLogResp);
                        }
                    });

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageUserOperationLogByCondition(NetworkPackageInfo packageInfo)
        {
            PageUserOperationLogByConditionResp resp = PageUserOperationLogByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.UserOperationLogs != null)
            {
                pageBean.DataList = BuildDataSource(resp.UserOperationLogs);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="userProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<UserOperationLogProto> userProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < userProtos.Count; i++)
            {
                UserOperationLogProto proto = userProtos[i];
                UserOperationLog operationLog = new UserOperationLog();
                operationLog.Id = proto.Id;
                operationLog.UserId = proto.UserId;
                operationLog.SoftwareId = proto.SoftwareId;
                operationLog.CreateTime = DateTimeUtil.OfEpochMilli(proto.CreateTime);
                operationLog.UpdateTime = DateTimeUtil.OfEpochMilli(proto.UpdateTime);
                operationLog.SoftwareModule = proto.SoftwareModule;
                operationLog.SoftwareModuleDetail = proto.SoftwareModuleDetail;
                operationLog.Data = proto.Data;
                operationLog.Description = proto.Description;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = operationLog;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //用户操作日志名称
                rowData.CellValueDict.Add(COLUMN_REALNAME, operationLog.Description);
                //开始时间
                rowData.CellValueDict.Add(COLUMN_CREATETIME, DateTimeUtil.ToString(operationLog.CreateTime));
                //更新时间
                rowData.CellValueDict.Add(COLUMN_UPDATETIME, DateTimeUtil.ToString(operationLog.UpdateTime));
                dataSource.Add(rowData);
            }

            return dataSource;
        }



        /// <summary>
        /// 接受删除用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteUserOperationLogResp(NetworkPackageInfo packageInfo)
        { 
            DeleteUserOperationLogResp resp = DeleteUserOperationLogResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.Detail);

            //更新表格
            operationLogModule.PageUserOperationLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserOperationLogByCondition);
            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteUserOperationLogResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteUserOperationLogResp resp = BatchDeleteUserOperationLogResp.Parser.ParseFrom(packageInfo.Body);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
            operationLogModule.PageUserOperationLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserOperationLogByCondition);
        }
    }
}
