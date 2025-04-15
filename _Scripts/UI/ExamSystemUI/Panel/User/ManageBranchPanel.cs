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
    public class ManageBranchPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//状态列
        public const string COLUMN_STATUS = "Status";//状态列
        public const string COLUMN_AMOUNT = "Amount";//专业数量列
        public const string COLUMN_POSTER = "Poster";//创建人列
        public const string COLUMN_CREATEDATE = "CreateDate";//创建日期
        public const string COLUMN_MODIFIER = "Modifier";//修改人
        public const string COLUMN_MODIFYDATE = "ModifyDate";//修改日期

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageBranchPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 专业模块
        /// </summary>
        private BranchModule branchModule;

        /// <summary>
        /// 专业表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 专业查询栏目
        /// </summary>
        private BranchQueryBar branchQueryBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            pageDataGrid = transform.Find("PageDataGrid").GetComponent<PageDataGrid>();
            batchActionBar = transform.Find("BatchActionBar").GetComponent<BatchActionBar>();
            branchQueryBar = transform.Find("QueryBar").GetComponent<BranchQueryBar>();
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
            branchQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            branchModule = ModuleManager.Instance.GetModule<BranchModule>();
            branchModule.PageBranchByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, branchQueryBar.SqlConditions, ReceivePageBranchByCondition);
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
            addressBar.AddHyperButton(EnumPanelType.ManageBranchPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageBranchPanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            branchModule.PageBranchByCondition(currentPage, pageSize, branchQueryBar.SqlConditions, ReceivePageBranchByCondition);
        }


        /// <summary>
        /// 专业查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            branchModule.PageBranchByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, branchQueryBar.SqlConditions, ReceivePageBranchByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Branch branch = row.Data.Tag as Branch;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyBranchPanel, OperationType.Modify, branch);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            if (branch.Amount > 0)
                            {
                                MessageBoxEx.Show("<color=red>专业中包含专业，无法删除。如需删除专业，请先清空专业中的专业。？</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                            }
                            else
                            {
                                branchModule.DeleteBranch(branch.Id, ReceiveDeleteBranchResp);
                            }
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
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyBranchPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageBranchResq(NetworkPackageInfo packageInfo)
        {
            PageBranchResp resp = PageBranchResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Branchs != null)
            {
                pageBean.DataList = BuildDataSource(resp.Branchs);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="branchProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<BranchProto> branchProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < branchProtos.Count; i++)
            {
                BranchProto branchProto = branchProtos[i];
                Branch branch = new Branch();
                branch.Id = branchProto.Id;
                branch.Name = branchProto.Name;
                branch.Status = branchProto.Status;
                branch.Poster = branchProto.Poster;
                branch.CreateTime = DateTimeUtil.OfEpochMilli(branchProto.CreateTime);
                branch.Modifier = branchProto.Modifier;
                branch.UpdateTime = DateTimeUtil.OfEpochMilli(branchProto.UpdateTime);
                branch.Amount = branchProto.Amount;
                branch.Remark = branchProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = branch;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, branch.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(branch.Status));
                rowData.CellValueDict.Add(COLUMN_AMOUNT, branch.Amount);
                rowData.CellValueDict.Add(COLUMN_POSTER, branch.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, branch.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, branch.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(branch.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePageBranchByCondition(NetworkPackageInfo packageInfo)
        {
            PageBranchByConditionResp resp = PageBranchByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Branchs != null)
            {
                pageBean.DataList = BuildDataSource(resp.Branchs);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除专业的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeleteBranchResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            branchModule.PageBranchByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, branchQueryBar.SqlConditions, ReceivePageBranchByCondition);
            MessageBoxEx.Show("专业删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
