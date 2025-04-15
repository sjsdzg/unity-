using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;

namespace XFramework.UI
{
    public class AddOrModifyRolePanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyRolePanel;
        }

        /// <summary>
        /// 用户角色模块
        /// </summary>
        private RoleModule roleModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户角色基本栏
        /// </summary>
        private RoleBasicBar roleBasicBar;

        /// <summary>
        /// 用户角色提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改用户角色
        /// </summary>
        private Role modifyRole;

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

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            roleBasicBar = transform.Find("RoleBasicBar").GetComponent<RoleBasicBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(roleSubmitBar_OnSumbit);
            submitBar.OnCancel.AddListener(roleSubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            base.OnStart();
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

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                operationType = (OperationType)PanelParams[0];
                if (operationType == OperationType.Modify)
                {
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyRolePanel, "修改角色");
                    Role role = PanelParams[1] as Role;
                    InitModifyContent(role);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyRolePanel, "添加角色");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="role"></param>
        private void InitModifyContent(Role role)
        {
            modifyRole = role;
            roleBasicBar.BankName = role.Name;
            roleBasicBar.Status = role.Status;
            roleBasicBar.Remark = role.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void roleSubmitBar_OnSumbit()
        {
            if (roleBasicBar.Validate())
            {
                Role role = new Role();
                role.Name = roleBasicBar.BankName;
                role.Status = roleBasicBar.Status;
                role.Remark = roleBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        roleModule.InsertRole(role, ReceiveInsertRoleResp);
                        break;
                    case OperationType.Modify:
                        role.Id = modifyRole.Id;
                        roleModule.UpdateRole(role, ReceiveUpdateRoleResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void roleSubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageRolePanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertRoleResp(NetworkPackageInfo packageInfo)
        {
            InsertRoleResp respProto = InsertRoleResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdateRoleResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改用户角色成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageRolePanel);
            });
        }

    }
}
