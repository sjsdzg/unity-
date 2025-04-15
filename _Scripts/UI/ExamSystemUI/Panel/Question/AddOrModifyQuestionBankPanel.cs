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
    public class AddOrModifyQuestionBankPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyQuestionBankPanel;
        }

        /// <summary>
        /// 试题库模块
        /// </summary>
        private QuestionBankModule questionBankModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试题库基本栏
        /// </summary>
        private QuestionBankBasicBar questionBankBasicBar;

        /// <summary>
        /// 试题库提交栏
        /// </summary>
        private QuestionBankSubmitBar questionBankSubmitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改试题库
        /// </summary>
        private QuestionBank modifyQuestionBank;

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
            questionBankBasicBar = transform.Find("QuestionBankBasicBar").GetComponent<QuestionBankBasicBar>();
            questionBankSubmitBar = transform.Find("QuestionBankSubmitBar").GetComponent<QuestionBankSubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            questionBankSubmitBar.OnSubmit.AddListener(questionBankSubmitBar_OnSumbit);
            questionBankSubmitBar.OnCancel.AddListener(questionBankSubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            questionBankModule = ModuleManager.Instance.GetModule<QuestionBankModule>();
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
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                operationType = (OperationType)PanelParams[0];
                if (operationType == OperationType.Modify)
                {
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyQuestionBankPanel, "修改题库");
                    QuestionBank questionBank = PanelParams[1] as QuestionBank;
                    InitModifyContent(questionBank);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyQuestionBankPanel, "添加题库");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="questionBank"></param>
        private void InitModifyContent(QuestionBank questionBank)
        {
            modifyQuestionBank = questionBank;
            questionBankBasicBar.BankName = questionBank.Name;
            questionBankBasicBar.Status = questionBank.Status;
            questionBankBasicBar.Remark = questionBank.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void questionBankSubmitBar_OnSumbit()
        {
            if (questionBankBasicBar.Validate())
            {
                QuestionBank questionBank = new QuestionBank();
                questionBank.Name = questionBankBasicBar.BankName;
                questionBank.Status = questionBankBasicBar.Status;
                questionBank.Remark = questionBankBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        questionBankModule.InsertQuestionBank(questionBank, ReceiveInsertQuestionBankResp);
                        break;
                    case OperationType.Modify:
                        questionBank.Id = modifyQuestionBank.Id;
                        questionBankModule.UpdateQuestionBank(questionBank, ReceiveUpdateQuestionBankResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void questionBankSubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageQuestionBankPanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertQuestionBankResp(NetworkPackageInfo packageInfo)
        {
            InsertQuestionBankResp respProto = InsertQuestionBankResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdateQuestionBankResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改试题库成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageQuestionBankPanel);
            });
        }

    }
}
