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
    /// <summary>
    /// 用户实验数据Panel
    /// </summary>
    public class UserEmpiricalDataPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_REALNAME = "RealName";//实验名称列
        public const string COLUMN_CREATETIME = "CreateTime";//创建时间列
        public const string COLUMN_UPDATETIME = "UpdateTime";//更新时间列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.UserEmpiricalDataPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 实验数据模块
        /// </summary>
        private EmpiricalDataRecordModule empiricalModule;


        /// <summary>
        /// 实验数据表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 实验数据查询栏目
        /// </summary>
        private UserEmpiricalDataQueryBar queryBar;

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
            queryBar = transform.Find("QueryBar").GetComponent<UserEmpiricalDataQueryBar>();
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
            empiricalModule = ModuleManager.Instance.GetModule<EmpiricalDataRecordModule>();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageEmpiricalPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageEmpiricalPanel));
            addressBar.AddHyperButton(EnumPanelType.UserEmpiricalDataPanel, PanelDefine.GetPanelComment(EnumPanelType.UserEmpiricalDataPanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                User user = PanelParams[0] as User;
                queryBar.SetUser(user);
            }

            //分页查询
            empiricalModule.PageEmpiricalDataRecordByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageEmpiricalDataRecordByCondition);
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
            empiricalModule.PageEmpiricalDataRecordByCondition(currentPage, pageSize, queryBar.SqlConditions, ReceivePageEmpiricalDataRecordByCondition);
        }

        /// <summary>
        /// 实验数据查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void userQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            empiricalModule.PageEmpiricalDataRecordByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageEmpiricalDataRecordByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            EmpiricalDataRecord record = row.Data.Tag as EmpiricalDataRecord;
            switch (type)
            {
                case ButtonCellType.Detail:
                    PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.EmpiricalDetailPanel, record);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            empiricalModule.DeleteEmpiricalDataRecord(record.Id, ReceiveDeleteEmpiricalDataRecordResp);
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
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道实验数据吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> ids = new List<string>();
                            foreach (var row in rows)
                            {
                                EmpiricalDataRecord record = row.Data.Tag as EmpiricalDataRecord;
                                ids.Add(record.Id);
                            }
                            empiricalModule.BatchDeleteEmpiricalDataRecord(ids, ReceiveBatchDeleteEmpiricalDataRecordResp);
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
        private void ReceivePageEmpiricalDataRecordByCondition(NetworkPackageInfo packageInfo)
        {
            PageEmpiricalDataRecordByConditionResp resp = PageEmpiricalDataRecordByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.EmpiricalDataRecords != null)
            {
                pageBean.DataList = BuildDataSource(resp.EmpiricalDataRecords);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="userProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<EmpiricalDataRecordProto> userProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < userProtos.Count; i++)
            {
                EmpiricalDataRecordProto proto = userProtos[i];
                EmpiricalDataRecord record = new EmpiricalDataRecord();
                record.Id = proto.Id;
                record.Name = proto.Name;
                record.RealName = proto.RealName;
                record.SoftwareId = proto.SoftwareId;
                record.UserId = proto.UserId;
                record.CreateTime = DateTimeUtil.OfEpochMilli(proto.CreateTime);
                record.UpdateTime = DateTimeUtil.OfEpochMilli(proto.UpdateTime);
                record.Data = proto.Data;
                record.Description = proto.Description;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = record;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //实验数据名称
                rowData.CellValueDict.Add(COLUMN_REALNAME, record.RealName);
                //开始时间
                rowData.CellValueDict.Add(COLUMN_CREATETIME, DateTimeUtil.ToString(record.CreateTime));
                //更新时间
                rowData.CellValueDict.Add(COLUMN_UPDATETIME, DateTimeUtil.ToString(record.UpdateTime));
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受删除实验数据的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteEmpiricalDataRecordResp(NetworkPackageInfo packageInfo)
        { 
            DeleteEmpiricalDataRecordResp resp = DeleteEmpiricalDataRecordResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.Detail);

            //更新表格
            empiricalModule.PageEmpiricalDataRecordByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageEmpiricalDataRecordByCondition);
            MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除实验数据的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteEmpiricalDataRecordResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteEmpiricalDataRecordResp resp = BatchDeleteEmpiricalDataRecordResp.Parser.ParseFrom(packageInfo.Body);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
            empiricalModule.PageEmpiricalDataRecordByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageEmpiricalDataRecordByCondition);
        }
    }
}
