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
    /// 管理实验Panel
    /// </summary>
    public class ManageEmpiricalPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_USERNAME = "UserName";//用户名称列
        public const string COLUMN_REALNAME = "RealName";//用户姓名列
        public const string COLUMN_ROLE_ID = "Role";//用户状态列
        public const string COLUMN_BRANCH = "Branch";//专业列
        public const string COLUMN_GRADE = "Grade";//年级列
        public const string COLUMN_POSITION = "Position";//班级列
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageEmpiricalPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 用户角色模块
        /// </summary>
        private RoleModule roleModule;

        /// <summary>
        /// 专业模块
        /// </summary>
        private BranchModule branchModule;

        /// <summary>
        /// 班级模块
        /// </summary>
        private PositionModule positionModule;

        /// <summary>
        /// 用户表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 实验数据用户查询栏目
        /// </summary>
        private EmpiricalQueryBar queryBar;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 专业列表
        /// </summary>
        private List<Branch> m_Branchs;

        /// <summary>
        /// 班级列表
        /// </summary>
        private List<Position> m_Positions;

        /// <summary>
        /// 用户角色列表
        /// </summary>
        private List<Role> m_Roles;

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
            queryBar = transform.Find("QueryBar").GetComponent<EmpiricalQueryBar>();
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
            userModule = ModuleManager.Instance.GetModule<UserModule>();
            branchModule = ModuleManager.Instance.GetModule<BranchModule>();
            positionModule = ModuleManager.Instance.GetModule<PositionModule>();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            //先获取所有用户库
            branchModule.ListAllBranch(ReceiveListAllBranchResp);
            positionModule.ListAllPosition(ReceiveListAllPositionResp);
            roleModule.ListAllRole(ReceiveListAllRoleResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageEmpiricalPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageEmpiricalPanel));
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
            userModule.PageUserByCondition(currentPage, pageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
        }

        /// <summary>
        /// 用户查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void userQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            User user = row.Data.Tag as User;
            switch (type)
            {
                case ButtonCellType.Detail:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.UserEmpiricalDataPanel, user);
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
                    MessageBoxEx.Show("<color=red>您确定要删除这" + rows.Count + "道用户吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            List<string> list = new List<string>();
                            foreach (var row in rows)
                            {
                                User user = row.Data.Tag as User;
                                list.Add(user.Id);
                            }
                            userModule.BatchDeleteUser(list, ReceiveBatchDeleteUserResp);
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
        /// 接受获取用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetUserResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetUserResp resp = GetUserResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受获取用户的总数
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveCountUserResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            CountUserResp resp = CountUserResp.Parser.ParseFrom(packageInfo.Body);
            Debug.Log(resp.ToString());
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageUserResq(NetworkPackageInfo packageInfo)
        {
            PageUserResp resp = PageUserResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Users != null)
            {
               pageBean.DataList = BuildDataSource(resp.Users);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 接受获取所有专业的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListAllBranchResp(NetworkPackageInfo packageInfo)
        {
            ListAllBranchResp resp = ListAllBranchResp.Parser.ParseFrom(packageInfo.Body);
            List<Branch> branchs = new List<Branch>();
            for (int i = 0; i < resp.Branchs.Count; i++)
            {
                BranchProto branchProto = resp.Branchs[i];
                //branch
                Branch branch = new Branch();
                branch.Id = branchProto.Id;
                branch.Name = branchProto.Name;
                branch.Status = branchProto.Status;
                branch.Poster = branchProto.Poster;
                branch.CreateTime = Converter.NewDateTime(branchProto.CreateTime);
                branch.Modifier = branchProto.Modifier;
                branch.UpdateTime = Converter.NewDateTime(branchProto.UpdateTime);
                branch.Remark = branchProto.Remark;
                branchs.Add(branch);
            }
            //设置用户库下拉框
            queryBar.SetBranchs(branchs);
            m_Branchs = branchs;
            //获取完专业之后, 分页查询用户。
            if (m_Positions != null && m_Roles != null)
            {
                userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
            }
        }

        private void ReceiveListAllRoleResp(NetworkPackageInfo packageInfo)
        {
            ListAllRoleResp resp = ListAllRoleResp.Parser.ParseFrom(packageInfo.Body);
            List<Role> roles = new List<Role>();
            for (int i = 0; i < resp.Roles.Count; i++)
            {
                RoleProto roleProto = resp.Roles[i];
                //branch
                Role role = new Role();
                role.Id = roleProto.Id;
                role.Name = roleProto.Name;
                role.Status = roleProto.Status;
                role.Privilege = roleProto.Privilege;
                role.Poster = roleProto.Poster;
                role.CreateTime = Converter.NewDateTime(roleProto.CreateTime);
                role.Modifier = roleProto.Modifier;
                role.UpdateTime = Converter.NewDateTime(roleProto.UpdateTime);
                role.Remark = roleProto.Remark;
                roles.Add(role);
            }
            //设置用户库下拉框
            //userQueryBar.SetRoles(roles);
            m_Roles = roles;
            if (m_Branchs != null && m_Positions != null)
            {
                userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
            }
        }

        /// <summary>
        /// 接受获取所有班级的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListAllPositionResp(NetworkPackageInfo packageInfo)
        {
            ListAllPositionResp resp = ListAllPositionResp.Parser.ParseFrom(packageInfo.Body);
            List<Position> positions = new List<Position>();
            for (int i = 0; i < resp.Positions.Count; i++)
            {
                PositionProto branchProto = resp.Positions[i];
                //branch
                Position branch = new Position();
                branch.Id = branchProto.Id;
                branch.Name = branchProto.Name;
                branch.Status = branchProto.Status;
                branch.Poster = branchProto.Poster;
                branch.CreateTime = Converter.NewDateTime(branchProto.CreateTime);
                branch.Modifier = branchProto.Modifier;
                branch.UpdateTime = Converter.NewDateTime(branchProto.UpdateTime);
                branch.Remark = branchProto.Remark;
                positions.Add(branch);
            }
            //设置用户库下拉框
            queryBar.SetPositions(positions);
            m_Positions = positions;
            //获取完班级之后, 分页查询用户。
            if (m_Branchs != null && m_Roles != null)
            {
                userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
            }
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageUserByCondition(NetworkPackageInfo packageInfo)
        {
            PageUserByConditionResp resp = PageUserByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Users != null)
            {
                pageBean.DataList = BuildDataSource(resp.Users);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="userProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<UserProto> userProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < userProtos.Count; i++)
            {
                UserProto userProto = userProtos[i];
                User user = new User();
                user.Id = userProto.Id;
                user.UserName = userProto.UserName;
                user.UserPassword = userProto.UserPassword;
                user.RoleId = userProto.RoleId;
                user.Sex = userProto.Sex;
                user.BranchId = userProto.BranchId;
                user.Grade = userProto.Grade;
                user.PositionId = userProto.PositionId;
                user.RealName = userProto.RealName;
                user.Status = userProto.Status;
                user.Modifier = userProto.Modifier;
                user.Poster = userProto.Poster;
                user.CreateTime = DateTimeUtil.OfEpochMilli(userProto.CreateTime);
                user.Modifier = userProto.Modifier;
                user.UpdateTime = DateTimeUtil.OfEpochMilli(userProto.UpdateTime);
                user.Remark = userProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = user;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //用户名称
                rowData.CellValueDict.Add(COLUMN_USERNAME, user.UserName);
                //用户姓名
                rowData.CellValueDict.Add(COLUMN_REALNAME, user.RealName);
                //角色
                Role role = m_Roles.FirstOrDefault(X => X.Id == user.RoleId);
                string roleName = (role == null ? "" : role.Name);
                rowData.CellValueDict.Add(COLUMN_ROLE_ID, roleName);
                //string status = ExamSystemConstants.Status.GetComment(user.UserStatus);
                //rowData.CellValueDict.Add(COLUMN_STATUS, status);
                //专业
                Branch branch = m_Branchs.FirstOrDefault(X => X.Id == user.BranchId);
                string branchName = (branch == null ? "" : branch.Name);
                rowData.CellValueDict.Add(COLUMN_BRANCH, branchName);
                //年级
                rowData.CellValueDict.Add(COLUMN_GRADE, user.Grade);
                //班级
                Position position = m_Positions.FirstOrDefault(X => X.Id == user.PositionId);
                string positionName = (position == null ? "" : position.Name);
                rowData.CellValueDict.Add(COLUMN_POSITION, positionName);
                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受删除用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteUserResp(NetworkPackageInfo packageInfo)
        { 
            Debug.Log("删除用户成功!");
            //更新表格
            userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
            MessageBoxEx.Show("用户删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeleteUserResp(NetworkPackageInfo packageInfo)
        {
            BatchDeleteUserResp resp = BatchDeleteUserResp.Parser.ParseFrom(packageInfo.Body);
            userModule.PageUserByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, queryBar.SqlConditions, ReceivePageUserByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }
}
