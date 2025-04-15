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
    public class AddOrModifyPositionPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyPositionPanel;
        }

        /// <summary>
        /// 班级模块
        /// </summary>
        private PositionModule positionModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 班级基本栏
        /// </summary>
        private PositionBasicBar positionBasicBar;

        /// <summary>
        /// 班级提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改班级
        /// </summary>
        private Position modifyPosition;

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
            positionBasicBar = transform.Find("PositionBasicBar").GetComponent<PositionBasicBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(positionSubmitBar_OnSumbit);
            submitBar.OnCancel.AddListener(positionSubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            positionModule = ModuleManager.Instance.GetModule<PositionModule>();
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
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyPositionPanel, "修改班级");
                    Position position = PanelParams[1] as Position;
                    InitModifyContent(position);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyPositionPanel, "添加班级");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="position"></param>
        private void InitModifyContent(Position position)
        {
            modifyPosition = position;
            positionBasicBar.BankName = position.Name;
            positionBasicBar.Status = position.Status;
            positionBasicBar.Remark = position.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void positionSubmitBar_OnSumbit()
        {
            if (positionBasicBar.Validate())
            {
                Position position = new Position();
                position.Name = positionBasicBar.BankName;
                position.Status = positionBasicBar.Status;
                position.Remark = positionBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        positionModule.InsertPosition(position, ReceiveInsertPositionResp);
                        break;
                    case OperationType.Modify:
                        position.Id = modifyPosition.Id;
                        positionModule.UpdatePosition(position, ReceiveUpdatePositionResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void positionSubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManagePositionPanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertPositionResp(NetworkPackageInfo packageInfo)
        {
            InsertPositionResp respProto = InsertPositionResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdatePositionResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改班级成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManagePositionPanel);
            });
        }

    }
}
