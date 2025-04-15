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
    public class ManageRolePanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//状态列
        public const string COLUMN_STATUS = "Status";//状态列
        public const string COLUMN_POSTER = "Poster";//创建人列
        public const string COLUMN_CREATEDATE = "CreateDate";//创建日期
        public const string COLUMN_MODIFIER = "Modifier";//修改人
        public const string COLUMN_MODIFYDATE = "ModifyDate";//修改日期

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageRolePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户角色模块
        /// </summary>
        private RoleModule roleModule;

        /// <summary>
        /// 用户角色表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 用户角色查询栏目
        /// </summary>
        private RoleQueryBar roleQueryBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面UserRole
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            pageDataGrid = transform.Find("PageDataGrid").GetComponent<PageDataGrid>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
            roleQueryBar = transform.Find("QueryBar").GetComponent<RoleQueryBar>();
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
            roleQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            roleModule.PageRoleByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, roleQueryBar.SqlConditions, ReceivePageRoleByCondition);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageRolePanel, PanelDefine.GetPanelComment(EnumPanelType.ManageRolePanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            roleModule.PageRoleByCondition(currentPage, pageSize, roleQueryBar.SqlConditions, ReceivePageRoleByCondition);
        }


        /// <summary>
        /// 用户角色查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            roleModule.PageRoleByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, roleQueryBar.SqlConditions, ReceivePageRoleByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Role role = row.Data.Tag as Role;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyRolePanel, OperationType.Modify, role);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            roleModule.DeleteRole(role.Id, ReceiveDeleteRoleResp);
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
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyRolePanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageRoleResq(NetworkPackageInfo packageInfo)
        {
            PageRoleResp resp = PageRoleResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Roles != null)
            {
                pageBean.DataList = BuildDataSource(resp.Roles);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="roleProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<RoleProto> roleProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < roleProtos.Count; i++)
            {
                RoleProto roleProto = roleProtos[i];
                Role role = new Role();
                role.Id = roleProto.Id;
                role.Name = roleProto.Name;
                role.Status = roleProto.Status;
                role.Poster = roleProto.Poster;
                role.CreateTime = DateTimeUtil.OfEpochMilli(roleProto.CreateTime);
                role.Modifier = roleProto.Modifier;
                role.UpdateTime = DateTimeUtil.OfEpochMilli(roleProto.UpdateTime);
                role.Remark = roleProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = role;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, role.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(role.Status));
                rowData.CellValueDict.Add(COLUMN_POSTER, role.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, role.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, role.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(role.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageRoleByCondition(NetworkPackageInfo packageInfo)
        {
            PageRoleByConditionResp resp = PageRoleByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Roles != null)
            {
                pageBean.DataList = BuildDataSource(resp.Roles);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除用户角色的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteRoleResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            roleModule.PageRoleByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, roleQueryBar.SqlConditions, ReceivePageRoleByCondition);
            MessageBoxEx.Show("用户角色删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
