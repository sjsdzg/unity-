using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 文本面板
    /// </summary>
    public class KnowTextPanel : MonoBehaviour, IHide
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 内容
        /// </summary>
        private Text content;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        private UnityEvent m_OnClosed = new UnityEvent();
        /// <summary>
        /// 关闭
        /// </summary>
        public UnityEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        void Awake()
        {
            title = transform.Find("Header/Text").GetComponent<Text>();
            content = transform.Find("Content/Text").GetComponent<Text>();
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            OnClosed.Invoke();
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_content"></param>
        public void Show(string _title, string _content)
        {
            gameObject.SetActive(true);
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;
            transform.GetComponent<RectTransform>().DOScale(1, 0.2f);
            title.text = _title;
            content.text = _content;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
