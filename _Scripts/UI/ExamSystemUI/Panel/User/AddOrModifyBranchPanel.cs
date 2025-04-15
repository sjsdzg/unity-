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
    public class AddOrModifyBranchPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyBranchPanel;
        }

        /// <summary>
        /// 专业模块
        /// </summary>
        private BranchModule branchModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 专业基本栏
        /// </summary>
        private BranchBasicBar branchBasicBar;

        /// <summary>
        /// 专业提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改专业
        /// </summary>
        private Branch modifyBranch;

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
            branchBasicBar = transform.Find("BranchBasicBar").GetComponent<BranchBasicBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(branchSubmitBar_OnSumbit);
            submitBar.OnCancel.AddListener(branchSubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            branchModule = ModuleManager.Instance.GetModule<BranchModule>();
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
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyBranchPanel, "修改专业");
                    Branch branch = PanelParams[1] as Branch;
                    InitModifyContent(branch);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyBranchPanel, "添加专业");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="branch"></param>
        private void InitModifyContent(Branch branch)
        {
            modifyBranch = branch;
            branchBasicBar.BankName = branch.Name;
            branchBasicBar.Status = branch.Status;
            branchBasicBar.Remark = branch.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void branchSubmitBar_OnSumbit()
        {
            if (branchBasicBar.Validate())
            {
                Branch branch = new Branch();
                branch.Name = branchBasicBar.BankName;
                branch.Status = branchBasicBar.Status;
                branch.Remark = branchBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        branchModule.InsertBranch(branch, ReceiveInsertBranchResp);
                        break;
                    case OperationType.Modify:
                        branch.Id = modifyBranch.Id;
                        branchModule.UpdateBranch(branch, ReceiveUpdateBranchResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void branchSubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageBranchPanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertBranchResp(NetworkPackageInfo packageInfo)
        {
            InsertBranchResp respProto = InsertBranchResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdateBranchResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改专业成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageBranchPanel);
            });
        }

    }
}
