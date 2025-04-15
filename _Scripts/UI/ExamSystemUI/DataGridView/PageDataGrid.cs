using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 分页表格控件
    /// </summary>
    public class PageDataGrid : MonoBehaviour
    {
        public class PagingEvent : UnityEvent<int, int> { }

        private PagingEvent m_OnPaging = new PagingEvent();
        /// <summary>
        /// 翻页时，触发
        /// </summary>
        public PagingEvent OnPaging
        {
            get { return m_OnPaging; }
            set { m_OnPaging = value; }
        }

        public class ButtonCellClickEvent : UnityEvent<DataGridViewRow, ButtonCellType> { }

        private ButtonCellClickEvent m_ButtonCellClick = new ButtonCellClickEvent();
        /// <summary>
        /// 按钮单元进行点击
        /// </summary>
        public ButtonCellClickEvent ButtonCellClick
        {
            get { return m_ButtonCellClick; }
            set { m_ButtonCellClick = value; }
        }

        public class SwitchChangedEvent : UnityEvent<DataGridViewRow, bool> { }

        private SwitchChangedEvent m_OnSwitchChanged = new SwitchChangedEvent();
        /// <summary>
        /// 选中状态改变时，触发
        /// </summary>
        public SwitchChangedEvent OnSwitchChanged
        {
            get { return m_OnSwitchChanged; }
            set { m_OnSwitchChanged = value; }
        }

        /// <summary>
        /// 表格控件
        /// </summary>
        private DataGridView dataGridView;

        /// <summary>
        /// 翻页控件
        /// </summary>
        private Pagination pagination;

        /// <summary>
        /// 分页数据
        /// </summary>
        public PageBean PageBean { get; private set; }

        /// <summary>
        /// 翻页详情
        /// </summary>
        private Text pageDetail;


        private int currentPage = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set {
                currentPage = value;
                pagination.CurrentPage = currentPage;
            }
        }

        private int pageSize = 15;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            private set { pageSize = value; }
        }

        public int CheckedCount { get; private set; }

        void Awake()
        {
            dataGridView = transform.GetComponentInChildren<DataGridView>();
            pagination = transform.GetComponentInChildren<Pagination>();
            //事件
            pagination.OnPaging.AddListener(pagination_OnPaging);
            dataGridView.ButtonCellClick.AddListener((x, y) => ButtonCellClick.Invoke(x, y));
            dataGridView.OnSwitchChanged.AddListener((x, y) => OnSwitchChanged.Invoke(x, y));
            dataGridView.CheckedChanged.AddListener(component_CheckedChanged);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="pageBean"></param>
        public void SetValue(PageBean pageBean)
        {
            pagination.TotalPages = pageBean.TotalPages;
            pagination.TotalRecords = pageBean.TotalRecords;
            pagination.CurrentPage = pageBean.CurrentPage;
            pagination.PageSize = pageBean.PageSize;
            dataGridView.DataSource = pageBean.DataList;
        }

        /// <summary>
        /// 翻页时，触发
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        private void pagination_OnPaging(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            OnPaging.Invoke(currentPage, pageSize);
        }

        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="row"></param>
        public void RemoveDataGridViewRow(DataGridViewRow row)
        {
            dataGridView.Remove(row.Data);
        }

        /// <summary>
        /// 改变选中状态
        /// </summary>
        /// <param name="_checked"></param>
        public void ChangeCheckedState(bool _checked)
        {
            dataGridView.ChangeCheckedState(_checked);

            if (_checked)
            {
                CheckedCount = PageSize;
            }
            else
            {
                CheckedCount = 0;
            }
        }

        /// <summary>
        /// 获取选中的行
        /// </summary>
        /// <returns></returns>
        public List<DataGridViewRow> GetRowsByChecked()
        {
            return dataGridView.GetRowsByChecked();
        }

        public List<DataGridViewRow> GetVisibleRows()
        {
            return dataGridView.GetVisibleComponents();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void component_CheckedChanged(bool _checked)
        {
            if (_checked)
            {
                CheckedCount++;
                if (GetRowsByChecked().Count == PageSize)
                {
                    dataGridView.ChangeCheckedStateNotCause(true);
                }
            }
            else
            {
                CheckedCount--;
                dataGridView.ChangeCheckedStateNotCause(false);
            }
        }
    }
}
