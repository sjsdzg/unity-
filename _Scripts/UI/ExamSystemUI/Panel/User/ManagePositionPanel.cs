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
    public class ManagePositionPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_NAME = "Name";//状态列
        public const string COLUMN_STATUS = "Status";//状态列
        public const string COLUMN_AMOUNT = "Amount";//班级数量列
        public const string COLUMN_POSTER = "Poster";//创建人列
        public const string COLUMN_CREATEDATE = "CreateDate";//创建日期
        public const string COLUMN_MODIFIER = "Modifier";//修改人
        public const string COLUMN_MODIFYDATE = "ModifyDate";//修改日期

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManagePositionPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 班级模块
        /// </summary>
        private PositionModule positionModule;

        /// <summary>
        /// 班级表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 批量操作栏
        /// </summary>
        private BatchActionBar batchActionBar;

        /// <summary>
        /// 班级查询栏目
        /// </summary>
        private PositionQueryBar positionQueryBar;

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
            positionQueryBar = transform.Find("QueryBar").GetComponent<PositionQueryBar>();
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
            positionQueryBar.OnQuery.AddListener(questionQueryBar_OnQuery);
        }

        protected override void OnStart()
        {
            base.OnStart();
            positionModule = ModuleManager.Instance.GetModule<PositionModule>();
            positionModule.PagePositionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, positionQueryBar.SqlConditions, ReceivePagePositionByCondition);
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
            addressBar.AddHyperButton(EnumPanelType.ManagePositionPanel, PanelDefine.GetPanelComment(EnumPanelType.ManagePositionPanel));
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            positionModule.PagePositionByCondition(currentPage, pageSize, positionQueryBar.SqlConditions, ReceivePagePositionByCondition);
        }


        /// <summary>
        /// 班级查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionQueryBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            positionModule.PagePositionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, positionQueryBar.SqlConditions, ReceivePagePositionByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Position position = row.Data.Tag as Position;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPositionPanel, OperationType.Modify, position);
                    break;
                case ButtonCellType.Delete:


                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            if (position.Amount > 0)
                            {
                                MessageBoxEx.Show("<color=red>班级中包含用户，无法删除。如需删除用户，请先清空班级中的班级。？</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                            }
                            else
                            {
                                positionModule.DeletePosition(position.Id, ReceiveDeletePositionResp);
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
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPositionPanel);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePositionResq(NetworkPackageInfo packageInfo)
        {
            PagePositionResp resp = PagePositionResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Positions != null)
            {
                pageBean.DataList = BuildDataSource(resp.Positions);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="positionProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<PositionProto> positionProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < positionProtos.Count; i++)
            {
                PositionProto positionProto = positionProtos[i];
                Position position = new Position();
                position.Id = positionProto.Id;
                position.Name = positionProto.Name;
                position.Status = positionProto.Status;
                position.Poster = positionProto.Poster;
                position.CreateTime = DateTimeUtil.OfEpochMilli(positionProto.CreateTime);
                position.Modifier = positionProto.Modifier;
                position.UpdateTime = DateTimeUtil.OfEpochMilli(positionProto.UpdateTime);
                position.Amount = positionProto.Amount;
                position.Remark = positionProto.Remark;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = position;
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());//COLUMN_NAME
                rowData.CellValueDict.Add(COLUMN_NAME, position.Name);
                rowData.CellValueDict.Add(COLUMN_STATUS, ExamSystemConstants.Status.GetComment(position.Status));
                rowData.CellValueDict.Add(COLUMN_AMOUNT, position.Amount);
                rowData.CellValueDict.Add(COLUMN_POSTER, position.Poster);
                rowData.CellValueDict.Add(COLUMN_CREATEDATE, position.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                rowData.CellValueDict.Add(COLUMN_MODIFIER, position.Modifier);
                rowData.CellValueDict.Add(COLUMN_MODIFYDATE, DateTimeUtil.ToString(position.UpdateTime));

                dataSource.Add(rowData);
            }

            return dataSource;
        }

        /// <summary>
        /// 接受根据条件分页查询的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePositionByCondition(NetworkPackageInfo packageInfo)
        {
            PagePositionByConditionResp resp = PagePositionByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Positions != null)
            {
                pageBean.DataList = BuildDataSource(resp.Positions);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 接受删除班级的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeletePositionResp(NetworkPackageInfo packageInfo)
        {
            //更新表格
            positionModule.PagePositionByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, positionQueryBar.SqlConditions, ReceivePagePositionByCondition);
            MessageBoxEx.Show("班级删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

    }
}
