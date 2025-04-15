using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using Newtonsoft.Json;
using XFramework.Network;
using XFramework.Common;

namespace XFramework.UI
{
    public class AddOrModifyUserPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyUserPanel;
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
        /// 用户库模块
        /// </summary>
        private BranchModule branchModule;

        /// <summary>
        /// 班级模块
        /// </summary>
        private PositionModule positionModule;

        /// <summary>
        /// 题目基础栏
        /// </summary>
        private UserBasicBar basicBar;

        /// <summary>
        /// 用户提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

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

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            basicBar = transform.Find("UserBasicBar").GetComponent<UserBasicBar>();;
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(sumbitBar_OnSumbit);
            submitBar.OnCancel.AddListener(sumbitBar_OnCancel);
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

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                operationType = (OperationType)PanelParams[0];
                if (operationType == OperationType.Modify)
                {
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyUserPanel, "修改用户");
                    User user = PanelParams[1] as User;

                    InitModifyContent(user);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyUserPanel, "添加用户");
            }
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
        /// 修改题目
        /// </summary>
        private User modifyUser;

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="user"></param>
        private void InitModifyContent(User user)
        {
            modifyUser = user;
            basicBar.UserName = user.UserName;
            basicBar.UserPassword = user.UserPassword;
            basicBar.RoleId = user.RoleId;
            basicBar.RealName = user.RealName;
            basicBar.Remark = user.Remark;
            basicBar.Sex = user.Sex;
            basicBar.BranchId = user.BranchId;
            basicBar.Grade = user.Grade;
            basicBar.PositionId = user.PositionId;
            basicBar.Status = user.Status;
            basicBar.PositionId = user.PositionId;
        }

        /// <summary>
        /// 点击提交时，触发
        /// </summary>
        private void sumbitBar_OnSumbit()
        {
            bool basic = basicBar.Validate();

            if (basic)
            {
                User user = new User();
                user.UserName = basicBar.UserName;
                user.UserPassword = basicBar.UserPassword;
                user.RoleId = basicBar.RoleId;
                user.RealName = basicBar.RealName;
                user.Remark = basicBar.Remark;
                user.Sex = basicBar.Sex;
                user.BranchId = basicBar.BranchId;
                user.Grade = basicBar.Grade;
                user.PositionId = basicBar.PositionId;
                user.Status = basicBar.Status;
                user.PositionId = basicBar.PositionId;
                user.UserNo = "";
                user.Phone = "";
                user.Email = "";

                switch (operationType)
                {
                    case OperationType.Add:
                        userModule.InsertUser(user, ReceiveInsertUserResp);
                        break;
                    case OperationType.Modify:
                        user.Id = modifyUser.Id;
                        userModule.UpdateUser(user, ReceiveUpdateUserResp);
                        break;
                    default:
                        break;
                } 
            }
        }

        /// <summary>
        /// 点击取消时，触发
        /// </summary>
        private void sumbitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageUserPanel);
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
            basicBar.SetBranchs(branchs);
            m_Branchs = branchs;
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
            basicBar.SetPositions(positions);
            m_Positions = positions;
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
            basicBar.SetRoles(roles);
            m_Roles = roles;
        }


        /// <summary>
        /// 接受添加题目的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertUserResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                InsertUserResp respProto = InsertUserResp.Parser.ParseFrom(packageInfo.Body);
                if (respProto.Success)
                {
                    MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
                }
                else
                {
                    MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
        }


        private void ReceiveUpdateUserResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("用户修改成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult =>
                {
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageUserPanel);
                });
            }
        }

    }
}
