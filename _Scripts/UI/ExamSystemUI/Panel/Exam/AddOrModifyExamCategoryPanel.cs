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
    public class AddOrModifyExamCategoryPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyExamCategoryPanel;
        }

        /// <summary>
        /// 考试分类模块
        /// </summary>
        private ExamCategoryModule examCategoryModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 考试分类基本栏
        /// </summary>
        private ExamCategoryBasicBar examCategoryBasicBar;

        /// <summary>
        /// 考试分类提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改考试分类
        /// </summary>
        private ExamCategory modifyExamCategory;

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
            examCategoryBasicBar = transform.Find("ExamCategoryBasicBar").GetComponent<ExamCategoryBasicBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(examCategorySubmitBar_OnSumbit);
            submitBar.OnCancel.AddListener(examCategorySubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            examCategoryModule = ModuleManager.Instance.GetModule<ExamCategoryModule>();
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
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyExamCategoryPanel, "修改考试分类");
                    ExamCategory examCategory = PanelParams[1] as ExamCategory;
                    InitModifyContent(examCategory);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyExamCategoryPanel, "添加考试分类");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="examCategory"></param>
        private void InitModifyContent(ExamCategory examCategory)
        {
            modifyExamCategory = examCategory;
            examCategoryBasicBar.BankName = examCategory.Name;
            examCategoryBasicBar.Status = examCategory.Status;
            examCategoryBasicBar.Remark = examCategory.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void examCategorySubmitBar_OnSumbit()
        {
            if (examCategoryBasicBar.Validate())
            {
                ExamCategory examCategory = new ExamCategory();
                examCategory.Name = examCategoryBasicBar.BankName;
                examCategory.Status = examCategoryBasicBar.Status;
                examCategory.Remark = examCategoryBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        examCategoryModule.InsertExamCategory(examCategory, ReceiveInsertExamCategoryResp);
                        break;
                    case OperationType.Modify:
                        examCategory.Id = modifyExamCategory.Id;
                        examCategoryModule.UpdateExamCategory(examCategory, ReceiveUpdateExamCategoryResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void examCategorySubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageExamCategoryPanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertExamCategoryResp(NetworkPackageInfo packageInfo)
        {
            InsertExamResp respProto = InsertExamResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdateExamCategoryResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改考试分类成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageExamCategoryPanel);
            });
        }

    }
}
