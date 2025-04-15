using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;

namespace XFramework.UI
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageBean
    {
        private int currentPage;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        private int pageSize;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int totalRecords;
        /// <summary>
        /// 总记录
        /// </summary>
        public int TotalRecords
        {
            get { return totalRecords; }
            set { totalRecords = value; }
        }

        private int totalPages;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; }
        }

        private ObservableList<DataGridViewRowData> dataList = new ObservableList<DataGridViewRowData>();
        /// <summary>
        /// 显示的数据列表
        /// </summary>
        public ObservableList<DataGridViewRowData> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }
    }
}
