using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using Newtonsoft.Json;
using XFramework.Network;
using XFramework.Common;

namespace XFramework.UI
{
    public class PersonalDataPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.PersonalDataPanel;
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
        private PersonalDataBar personalDataBar;

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        /// <summary>
        /// 重置按钮
        /// </summary>
        private Button buttonReset;

        /// <summary>
        /// 专业
        /// </summary>
        private Branch m_Branch;

        /// <summary>
        /// 班级
        /// </summary>
        private Position m_Position;

        /// <summary>
        /// 用户角色
        /// </summary>
        private Role m_Role;

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
            personalDataBar = transform.Find("PersonalDataBar").GetComponent<PersonalDataBar>();
            buttonSubmit = transform.Find("ButtonSubmit").GetComponent<Button>();
            buttonReset = transform.Find("ButtonReset").GetComponent<Button>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
            buttonReset.onClick.AddListener(buttonReset_onClick);
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        private void buttonSubmit_onClick()
        {
            User user = GlobalManager.user;
            user.RealName = personalDataBar.RealName;
            user.Remark = personalDataBar.Remark;
            user.Sex = personalDataBar.Sex;
            user.UserNo = "";
            user.Phone = "";
            user.Email = "";
            //修改用户信息
            userModule.UpdateUser(user, ReceiveUpdateUserResp);
        }

        /// <summary>
        /// 点击重置按钮，触发
        /// </summary>
        private void buttonReset_onClick()
        {
            //设置信息
            personalDataBar.UserName = GlobalManager.user.UserName;
            personalDataBar.RealName = GlobalManager.user.RealName;
            personalDataBar.Remark = GlobalManager.user.Remark;
            personalDataBar.Sex = GlobalManager.user.Sex;
            personalDataBar.Grade = GlobalManager.user.Grade;
            personalDataBar.Branch = (m_Branch == null ? "" : m_Branch.Name);
            personalDataBar.Position = (m_Position == null ? "" : m_Position.Name);
            personalDataBar.Role = (m_Role == null ? "" : m_Role.Name);
        }


        protected override void OnStart()
        {
            base.OnStart();
            userModule = ModuleManager.Instance.GetModule<UserModule>();
            branchModule = ModuleManager.Instance.GetModule<BranchModule>();
            positionModule = ModuleManager.Instance.GetModule<PositionModule>();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            //先获取所有用户库
            branchModule.GetBranch(GlobalManager.user.BranchId, ReceiveGetBranchResp);
            positionModule.GetPosition(GlobalManager.user.PositionId, ReceiveGetPositionResp);
            roleModule.GetRole(GlobalManager.user.RoleId, ReceiveGetRoleResp);
            //设置信息
            personalDataBar.UserName = GlobalManager.user.UserName;
            personalDataBar.RealName = GlobalManager.user.RealName;
            personalDataBar.Remark = GlobalManager.user.Remark;
            personalDataBar.Sex = GlobalManager.user.Sex;
            personalDataBar.Grade = GlobalManager.user.Grade;
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.PersonalDataPanel, PanelDefine.GetPanelComment(EnumPanelType.PersonalDataPanel));
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
        /// 接受获取专业的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetBranchResp(NetworkPackageInfo packageInfo)
        {
            GetBranchResp resp = GetBranchResp.Parser.ParseFrom(packageInfo.Body);
            BranchProto branchProto = resp.Branch;
            if (branchProto == null)
                return;

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
            m_Branch = branch;
            //设置
            personalDataBar.Branch = m_Branch.Name;
        }

        /// <summary>
        /// 接受获取班级的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetPositionResp(NetworkPackageInfo packageInfo)
        {
            GetPositionResp resp = GetPositionResp.Parser.ParseFrom(packageInfo.Body);
            PositionProto branchProto = resp.Position;
            if (branchProto == null)
                return;

            //branch
            Position position = new Position();
            position.Id = branchProto.Id;
            position.Name = branchProto.Name;
            position.Status = branchProto.Status;
            position.Poster = branchProto.Poster;
            position.CreateTime = Converter.NewDateTime(branchProto.CreateTime);
            position.Modifier = branchProto.Modifier;
            position.UpdateTime = Converter.NewDateTime(branchProto.UpdateTime);
            position.Remark = branchProto.Remark;
            m_Position = position;
            //设置
            personalDataBar.Position = m_Position.Name;
        }

        private void ReceiveGetRoleResp(NetworkPackageInfo packageInfo)
        {
            GetRoleResp resp = GetRoleResp.Parser.ParseFrom(packageInfo.Body);
            RoleProto roleProto = resp.Role;
            if (roleProto == null)
                return;

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
            m_Role = role;
            //设置
            personalDataBar.Role = m_Role.Name;
        }

        private void ReceiveUpdateUserResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("个人资料修改成功", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }
    }
}
