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
    public class AddOrModifyPaperCategoryPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyPaperCategoryPanel;
        }

        /// <summary>
        /// 试试卷分类模块
        /// </summary>
        private PaperCategoryModule paperCategoryModule;

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 试试卷分类基本栏
        /// </summary>
        private PaperCategoryBasicBar paperCategoryBasicBar;

        /// <summary>
        /// 试试卷分类提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        /// <summary>
        /// 修改试试卷分类
        /// </summary>
        private PaperCategory modifyPaperCategory;

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
            paperCategoryBasicBar = transform.Find("PaperCategoryBasicBar").GetComponent<PaperCategoryBasicBar>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            submitBar.OnSubmit.AddListener(paperCategorySubmitBar_OnSumbit);
            submitBar.OnCancel.AddListener(paperCategorySubmitBar_OnCancel);
        }

        protected override void OnStart()
        {
            paperCategoryModule = ModuleManager.Instance.GetModule<PaperCategoryModule>();
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
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyPaperCategoryPanel, "修改试卷分类");
                    PaperCategory paperCategory = PanelParams[1] as PaperCategory;
                    InitModifyContent(paperCategory);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyPaperCategoryPanel, "添加试卷分类");
            }
        }

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="paperCategory"></param>
        private void InitModifyContent(PaperCategory paperCategory)
        {
            modifyPaperCategory = paperCategory;
            paperCategoryBasicBar.BankName = paperCategory.Name;
            paperCategoryBasicBar.Status = paperCategory.Status;
            paperCategoryBasicBar.Remark = paperCategory.Remark;
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void paperCategorySubmitBar_OnSumbit()
        {
            if (paperCategoryBasicBar.Validate())
            {
                PaperCategory paperCategory = new PaperCategory();
                paperCategory.Name = paperCategoryBasicBar.BankName;
                paperCategory.Status = paperCategoryBasicBar.Status;
                paperCategory.Remark = paperCategoryBasicBar.Remark;

                switch (operationType)
                {
                    case OperationType.Add:
                        paperCategoryModule.InsertPaperCategory(paperCategory, ReceiveInsertPaperCategoryResp);
                        break;
                    case OperationType.Modify:
                        paperCategory.Id = modifyPaperCategory.Id;
                        paperCategoryModule.UpdatePaperCategory(paperCategory, ReceiveUpdatePaperCategoryResp);
                        break;
                    default:
                        break;
                }
            }
        }

        private void paperCategorySubmitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManagePaperCategoryPanel);
        }

        /// <summary>
        /// 接受插入实体卡
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertPaperCategoryResp(NetworkPackageInfo packageInfo)
        {
            InsertPaperCategoryResp respProto = InsertPaperCategoryResp.Parser.ParseFrom(packageInfo.Body);
            if (respProto.Success)
            {
                MessageBoxEx.Show(respProto.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                MessageBoxEx.Show("<color=red>" + respProto.Detail + "</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        private void ReceiveUpdatePaperCategoryResp(NetworkPackageInfo packageInfo)
        {
            MessageBoxEx.Show("修改试卷分类成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult => 
            {
                PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManagePaperCategoryPanel);
            });
        }

    }
}
