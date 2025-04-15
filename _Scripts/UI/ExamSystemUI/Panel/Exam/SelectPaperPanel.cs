using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Common;
using DG.Tweening;

namespace XFramework.UI
{
    public class SelectPaperPanel : AbstractPanel
    {
        public const string COLUMN_NUMBER = "Number";//序号列
        public const string COLUMN_Category = "Category";//考试分类列
        public const string COLUMN_NAME = "Name";//考试体感列
        public const string COLUMN_SWITCH = "SwitchCell";//考试体感列

        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.SelectPaperPanel;
        }

        /// <summary>
        /// 考试模块
        /// </summary>
        private PaperModule paperModule;

        /// <summary>
        /// 考试分类模块
        /// </summary>
        private PaperCategoryModule paperCategoryModule;

        /// <summary>
        /// 考试表格视图
        /// </summary>
        private PageDataGrid pageDataGrid;

        /// <summary>
        /// 考试查询栏目
        /// </summary>
        private PaperQuerysBar paperQuerysBar;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 考试分类列表
        /// </summary>
        private List<PaperCategory> m_PaperCategorys;

        /// <summary>
        /// 
        /// </summary>
        private RectTransform background;

        /// <summary>
        /// 当前章节
        /// </summary>
        private ExamBasicBar examBasicBar;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Parent.GetComponent<Image>().enabled = false;
        }

        private void InitGUI()
        {
            background = transform.Find("Background").GetComponent<RectTransform>();
            buttonClose = transform.Find("Background/Title/ButtonClose").GetComponent<Button>();
            pageDataGrid = transform.Find("Background/Panel/PageDataGrid").GetComponent<PageDataGrid>();
            paperQuerysBar = transform.Find("Background/Panel/QueryBar").GetComponent<PaperQuerysBar>();
        }

        private void InitEvent()
        {
            pageDataGrid.OnPaging.AddListener(pageDataGrid_OnPaging);
            pageDataGrid.ButtonCellClick.AddListener(pageDataGrid_ButtonCellClick);
            pageDataGrid.OnSwitchChanged.AddListener(pageDataGrid_OnSwitchChanged);
            paperQuerysBar.OnQuery.AddListener(paperQuerysBar_OnQuery);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            PanelManager.Instance.ClosePanel(EnumPanelType.SelectPaperPanel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            paperModule = ModuleManager.Instance.GetModule<PaperModule>();
            paperCategoryModule = ModuleManager.Instance.GetModule<PaperCategoryModule>();
            //先获取所有考试分类
            paperCategoryModule.ListPaperCategoryByCondition(SqlCondition.ListBySoftwareId(), ReceiveListPaperCategoryByConditionResp);
            //动态显示
            background.DOScale(0, 0.3f).From();
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                SelectPaperPanel_Data data = PanelParams[0] as SelectPaperPanel_Data;
                examBasicBar = data.ExamBasicBar;
                paperModule.PagePaperByCondition(1, pageDataGrid.PageSize, paperQuerysBar.SqlConditions, ReceivePagePaperByCondition);
            }
        }

        /// <summary>
        /// 分页表格翻页时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnPaging(int currentPage, int pageSize)
        {
            paperModule.PagePaperByCondition(currentPage, pageSize, paperQuerysBar.SqlConditions, ReceivePagePaperByCondition);
        }

        /// <summary>
        /// 考试查询栏目查询时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void paperQuerysBar_OnQuery()
        {
            pageDataGrid.CurrentPage = 1;
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQuerysBar.SqlConditions, ReceivePagePaperByCondition);
        }

        /// <summary>
        /// 分页表格行上的按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_ButtonCellClick(DataGridViewRow row, ButtonCellType type)
        {
            Paper paper = row.Data.Tag as Paper;
            switch (type)
            {
                case ButtonCellType.Update:
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.AddOrModifyPaperPanel, OperationType.Modify, paper);
                    break;
                case ButtonCellType.Delete:
                    MessageBoxEx.Show("<color=red>您确定要删除吗？</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            paperModule.DeletePaper(paper.Id, ReceiveDeletePaperResp);
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
                case ButtonCellType.SelectAll:
                    List<DataGridViewRow> rows = pageDataGrid.GetVisibleRows();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataGridViewRow row = rows[i];
                        row.SetData(COLUMN_SWITCH, false);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// switch改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void pageDataGrid_OnSwitchChanged(DataGridViewRow row, bool value)
        {
            if (value)
            {
                examBasicBar.Paper = null;
            }
            else
            {
                examBasicBar.Paper = row.Data.Tag as Paper;
            }

            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            PanelManager.Instance.ClosePanel(EnumPanelType.SelectPaperPanel);
        }

        /// <summary>
        /// 接受获取题目的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetPaperResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log(packageInfo.Header.ToString());
            GetPaperResp resp = GetPaperResp.Parser.ParseFrom(packageInfo.Body);
        }

        /// <summary>
        /// 接受分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperResq(NetworkPackageInfo packageInfo)
        {
            PagePaperResp resp = PagePaperResp.Parser.ParseFrom(packageInfo.Body);
            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Papers != null)
            {
                pageBean.DataList = BuildDataSource(resp.Papers);
            }

            pageDataGrid.SetValue(pageBean);
        }

        /// <summary>
        /// 接受获取所有考试分类的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListPaperCategoryByConditionResp(NetworkPackageInfo packageInfo)
        {
            ListPaperCategoryByConditionResp resp = ListPaperCategoryByConditionResp.Parser.ParseFrom(packageInfo.Body);
            List<PaperCategory> paperCategorys = new List<PaperCategory>();
            for (int i = 0; i < resp.PaperCategorys.Count; i++)
            {
                PaperCategoryProto paperCategoryProto = resp.PaperCategorys[i];
                //paperCategory
                PaperCategory paperCategory = new PaperCategory();
                paperCategory.Id = paperCategoryProto.Id;
                paperCategory.Name = paperCategoryProto.Name;
                paperCategory.Status = paperCategoryProto.Status;
                paperCategory.Poster = paperCategoryProto.Poster;
                paperCategory.CreateTime = Converter.NewDateTime(paperCategoryProto.CreateTime);
                paperCategory.Modifier = paperCategoryProto.Modifier;
                paperCategory.UpdateTime = Converter.NewDateTime(paperCategoryProto.UpdateTime);
                paperCategory.Remark = paperCategoryProto.Remark;
                paperCategorys.Add(paperCategory);
            }
            //设置考试分类下拉框
            paperQuerysBar.SetCategory(paperCategorys);
            m_PaperCategorys = paperCategorys;
            //获取完题库之后, 分页查询考试。
            //paperModule.PagePaper(1, pageDataGrid.PageSize, ReceivePagePaperResq);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceivePagePaperByCondition(NetworkPackageInfo packageInfo)
        {
            PagePaperByConditionResp resp = PagePaperByConditionResp.Parser.ParseFrom(packageInfo.Body);

            PageBean pageBean = new PageBean();
            pageBean.CurrentPage = resp.CurrentPage;
            pageBean.PageSize = resp.PageSize;
            pageBean.TotalPages = resp.TotalPages;
            pageBean.TotalRecords = resp.TotalRecords;

            if (resp.Papers != null)
            {
                pageBean.DataList = BuildDataSource(resp.Papers);
            }

            pageDataGrid.SetValue(pageBean);

            pageDataGrid.ChangeCheckedState(false);
        }

        /// <summary>
        /// 构建分页表格数据源
        /// </summary>
        /// <param name="paperProtos"></param>
        /// <returns></returns>
        private ObservableList<DataGridViewRowData> BuildDataSource(RepeatedField<PaperProto> paperProtos)
        {
            ObservableList<DataGridViewRowData> dataSource = new ObservableList<DataGridViewRowData>();
            for (int i = 0; i < paperProtos.Count; i++)
            {
                PaperProto paperProto = paperProtos[i];
                Paper paper = new Paper();
                paper.Id = paperProto.Id;
                paper.Name = paperProto.Name;
                paper.CategoryId = paperProto.CategoryId;
                paper.Status = paperProto.Status;
                paper.TotalScore = paperProto.TotalScore;
                paper.PassScore = paperProto.PassScore;
                paper.Poster = paperProto.Poster;
                paper.CreateTime = Converter.NewDateTime(paperProto.CreateTime);
                paper.Modifier = paperProto.Modifier;
                paper.UpdateTime = Converter.NewDateTime(paperProto.UpdateTime);
                paper.Remark = paperProto.Remark;
                paper.Data = paperProto.Data;

                DataGridViewRowData rowData = new DataGridViewRowData();
                rowData.Tag = paper;
                //序号
                rowData.CellValueDict.Add(COLUMN_NUMBER, ((pageDataGrid.CurrentPage - 1) * pageDataGrid.PageSize + i + 1).ToString());
                //试卷分类
                PaperCategory paperCategory = m_PaperCategorys.FirstOrDefault(X => X.Id == paper.CategoryId);
                string categoryName = paperCategory == null ? "" : paperCategory.Name;
                rowData.CellValueDict.Add(COLUMN_Category, categoryName);
                //试卷名称
                rowData.CellValueDict.Add(COLUMN_NAME, paper.Name);

                if (examBasicBar.Paper != null && examBasicBar.Paper.Id == paper.Id)
                {
                    rowData.CellValueDict.Add(COLUMN_SWITCH, false);//Switch
                }
                else
                {
                    rowData.CellValueDict.Add(COLUMN_SWITCH, true);//Switch
                }

                dataSource.Add(rowData);
            }

            return dataSource;

        }

        /// <summary>
        /// 接受删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveDeletePaperResp(NetworkPackageInfo packageInfo)
        {
            Debug.Log("删除考试成功!");
            //更新表格
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQuerysBar.SqlConditions, ReceivePagePaperByCondition);
            MessageBoxEx.Show("考试删除成功", "提示", MessageBoxExEnum.SingleDialog, null);
        }

        /// <summary>
        /// 接受批量删除考试的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchDeletePaperResp(NetworkPackageInfo packageInfo)
        {
            BatchDeletePaperResp resp = BatchDeletePaperResp.Parser.ParseFrom(packageInfo.Body);
            paperModule.PagePaperByCondition(pageDataGrid.CurrentPage, pageDataGrid.PageSize, paperQuerysBar.SqlConditions, ReceivePagePaperByCondition);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }
    }

    public class SelectPaperPanel_Data
    {
        /// <summary>
        /// 试卷章节组件
        /// </summary>
        public ExamBasicBar ExamBasicBar { get; set; }
    }
}
