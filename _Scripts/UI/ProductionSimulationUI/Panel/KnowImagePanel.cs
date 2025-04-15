using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    /// <summary>
    /// 文本面板
    /// </summary>
    public class KnowImagePanel : MonoBehaviour, IHide
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 内容
        /// </summary>
        private Image content;

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
            content = transform.Find("Image").GetComponent<Image>();
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
        public void Show(string _title, Sprite _content)
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;
            transform.GetComponent<RectTransform>().DOScale(1, 0.2f);
            gameObject.SetActive(true);
            title.text = _title;
            content.sprite = _content;
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
