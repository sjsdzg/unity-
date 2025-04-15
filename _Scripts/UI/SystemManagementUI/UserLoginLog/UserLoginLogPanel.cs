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
    public class UserLoginLogPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_REALNAME = "RealName";//实验名称列
        public const string COLUMN_TYPE = "Type";//类型列 0:登录 1:退出 2:重连 
        public const string COLUMN_CREATETIME = "CreateTime";//创建时间列
        public const string COLUMN_IP = "Ip";//IP列
        public const string COLUMN_ADDRESS = "Address";//Address列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.UserLoginLogPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户操作日志模块
        /// </summary>
        private UserLoginLogModule userLoginLogModule;


        /// <summary>
        /// 用户操作日志表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 用户操作日志查询栏目
        /// </summary>
        private UserLoginLogQueryBar queryBar;

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
            queryBar = transform.Find("QueryBar").GetComponent<UserLoginLogQueryBar>();
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
            userLoginLogModule = ModuleManager.Instance.GetModule<UserLoginLogModule>();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageLoginLogPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageLoginLogPanel));
            addressBar.AddHyperButton(EnumPanelType.UserLoginLogPanel, PanelDefine.GetPanelComment(EnumPanelType.UserLoginLogPanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                User user = PanelParams[0] as User;
                queryBar.SetUser(user);
            }

            //分页查询
            userLoginLogModule.PageUserLoginLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserLoginLogByCondition);
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
            userLoginLogModule.PageUserLoginLogByCondition(currentPage, pageSize, queryBar.SqlConditions, ReceivePageUserLoginLogByCondition);
        }

        /// <summary>
        /// 用户操作日志查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void userQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            userLoginLogModule.PageUserLoginLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserLoginLogByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            UserLoginLog operationLog = row.Data.Tag as UserLoginLog;
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
                            userLoginLogModule.DeleteUserLoginLog(operationLog.Id, ReceiveDeleteUserLoginLogResp);
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
                                UserLoginLog operationLog = row.Data.Tag as UserLoginLog;
                                ids.Add(operationLog.Id);
                            }
                            userLoginLogModule.BatchDeleteUserLoginLog(ids, ReceiveBatchDeleteUserLoginLogResp);
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
        private void ReceivePageUserLoginLogByCondition(NetworkPackageInfo packageInfo)
        {
            PageUserLoginLogByConditionResp resp = PageUserLoginLogByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.UserLoginLogs != null)
            {
                pageBean.DataList = BuildDataSource(resp.UserLoginLogs);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="userProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<UserLoginLogProto> userProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < userProtos.Count; i++)
            {
                UserLoginLogProto proto = userProtos[i];
                UserLoginLog userLoginLog = new UserLoginLog();
                userLoginLog.Id = proto.Id;
                userLoginLog.UserName = proto.UserName;
                userLoginLog.SoftwareId = proto.SoftwareId;
                userLoginLog.CreateTime = DateTimeUtil.OfEpochMilli(proto.CreateTime);
                userLoginLog.Type = proto.Type;
                userLoginLog.Ip = proto.Ip;
                userLoginLog.Address = proto.Address;
                userLoginLog.Remark = proto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = userLoginLog;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //类型
                string comment = "";
                switch (userLoginLog.Type)
                {
                    case 0:
                        comment = "<color=#008080ff>登录系统</color>";
                        break;
                    case 1:
                        comment = "<color=#808080ff>退出系统</color>";
                        break;
                    case 2:
                        comment = "<color=#008080ff>重连系统</color>";
                        break;
                    case 3:
                        comment = "<color=#808080ff>强制下线</color>";
                        break;
                    default:
                        break;
                }
                rowData.CellValueDict.Add(COLUMN_TYPE, comment);
                //开始时间
                rowData.CellValueDict.Add(COLUMN_CREATETIME, DateTimeUtil.ToString(userLoginLog.CreateTime));
                //ip
                rowData.CellValueDict.Add(COLUMN_IP, userLoginLog.Ip);
                //登录地址
                AddressJson addressJson = AddressJson.Parser.ParseJson(userLoginLog.Address);
                rowData.CellValueDict.Add(COLUMN_ADDRESS, addressJson.ToString());
                dataSource.Add(rowData);
            }

            return dataSource;
        }



        /// <summary>
        /// 接受删除用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteUserLoginLogResp(NetworkPackageInfo packageInfo)
        { 
            DeleteUserLoginLogResp resp = DeleteUserLoginLogResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.Detail);

            //更新表格
            userLoginLogModule.PageUserLoginLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserLoginLogByCondition);
            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除用户操作日志的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteUserLoginLogResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteUserLoginLogResp resp = BatchDeleteUserLoginLogResp.Parser.ParseFrom(packageInfo.Body);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
            userLoginLogModule.PageUserLoginLogByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserLoginLogByCondition);
        }
    }
}
