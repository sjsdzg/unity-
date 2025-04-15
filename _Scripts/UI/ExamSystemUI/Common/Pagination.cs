using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public class Pagination : MonoBehaviour
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

        private int currentPage;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                ChangePageState();
                ChangeButtonState();
            }
        }

        private int pageSize;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value;
                ChangePageDetail();
            }
        }

        private int totalRecords;
        /// <summary>
        /// 总记录
        /// </summary>
        public int TotalRecords
        {
            get { return totalRecords; }
            set
            {
                totalRecords = value;
                ChangePageDetail();
            }
        }

        private int totalPages;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return totalPages; }
            set
            {
                totalPages = value;
                ChangePageState();
            }
        }

        /// <summary>
        /// 首页按钮
        /// </summary>
        private Button buttonFirst;

        /// <summary>
        /// 上一页按钮
        /// </summary>
        private Button buttonPrevious;

        /// <summary>
        /// 下一页按钮
        /// </summary>
        private Button buttonNext;

        /// <summary>
        /// 末页按钮
        /// </summary>
        private Button buttonLast;

        /// <summary>
        /// 分页状态
        /// </summary>
        private Text pageState;

        /// <summary>
        /// 分页详细情况
        /// </summary>
        private Text pageDetail;

        void Awake()
        {
            buttonFirst = transform.Find("ButtonFirst").GetComponent<Button>();
            buttonPrevious = transform.Find("ButtonPrevious").GetComponent<Button>();
            buttonNext = transform.Find("ButtonNext").GetComponent<Button>();
            buttonLast = transform.Find("ButtonLast").GetComponent<Button>();
            pageState = transform.Find("pageState").GetComponent<Text>();
            pageDetail = transform.Find("pageDetail").GetComponent<Text>();

            buttonFirst.onClick.AddListener(buttonFirst_onClick);
            buttonPrevious.onClick.AddListener(buttonPrevious_onClick);
            buttonNext.onClick.AddListener(buttonNext_onClick);
            buttonLast.onClick.AddListener(buttonLast_onClick);
        }

        /// <summary>
        /// 首页按钮点击时，触发
        /// </summary>
        private void buttonFirst_onClick()
        {
            CurrentPage = 1;
            OnPaging.Invoke(CurrentPage, PageSize);
        }

        /// <summary>
        /// 上一页点击时，触发
        /// </summary>
        private void buttonPrevious_onClick()
        {
            CurrentPage -= 1;
            OnPaging.Invoke(CurrentPage, PageSize);
        }

        /// <summary>
        /// 下一页点击时，触发
        /// </summary>
        private void buttonNext_onClick()
        {
            CurrentPage += 1;
            OnPaging.Invoke(CurrentPage, PageSize);
        }

        /// <summary>
        /// 末页点击时，触发
        /// </summary>
        private void buttonLast_onClick()
        {
            CurrentPage = totalPages;
            OnPaging.Invoke(CurrentPage, PageSize);
        }

        private void ChangePageState()
        {
            pageState.text = CurrentPage + "/" + TotalPages;
        }

        private void ChangePageDetail()
        {
            pageDetail.text = string.Format("共{0}条记录，每页{1}条。", TotalRecords, PageSize);
        }

        private void ChangeButtonState()
        {
            buttonFirst.interactable = true;
            buttonPrevious.interactable = true;
            buttonNext.interactable = true;
            buttonLast.interactable = true;
            buttonFirst.GetComponent<HandCursorSupport>().AllowHandCursor = true;
            buttonPrevious.GetComponent<HandCursorSupport>().AllowHandCursor = true;
            buttonNext.GetComponent<HandCursorSupport>().AllowHandCursor = true;
            buttonLast.GetComponent<HandCursorSupport>().AllowHandCursor = true;

            if (currentPage < 0)
            {
                currentPage = 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            if (currentPage == 1)
            {
                buttonFirst.interactable = false;
                buttonPrevious.interactable = false;
                buttonFirst.GetComponent<HandCursorSupport>().AllowHandCursor = false;
                buttonPrevious.GetComponent<HandCursorSupport>().AllowHandCursor = false;
            }

            if (currentPage == totalPages)
            {
                buttonNext.interactable = false;
                buttonLast.interactable = false;
                buttonNext.GetComponent<HandCursorSupport>().AllowHandCursor = false;
                buttonLast.GetComponent<HandCursorSupport>().AllowHandCursor = false;
            }
        }
    }
}
