using UnityEngine;
using System.Collections;
using UIWidgets;
using UnityEngine.UI;
using XFramework.Module;
using System;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class KnowledgePointNotify : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<KnowledgePoint> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 详情按钮
        /// </summary>
        private Button buttonDetail;

        /// <summary>
        /// 知识点
        /// </summary>
        private KnowledgePoint knowledgePoint;

        /// <summary>
        /// 隐藏按钮
        /// </summary>
        private Button hideButton;

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        private void Awake()
        {
            buttonDetail = transform.Find("Background/Bottom/ButtonDetail").GetComponent<Button>();
            hideButton = transform.Find("Background/Header/HideButton").GetComponent<Button>();
            m_Text = transform.Find("Background/Content/Text").GetComponent<Text>();
            //事件
            buttonDetail.onClick.AddListener(buttonDetail_onClick);
            hideButton.onClick.AddListener(hideButton_onClick);
        }

        /// <summary>
        /// 了解详情按钮点击时，触发
        /// </summary>
        private void buttonDetail_onClick()
        {
            //KnowledgePointPanel.Instance.Show(knowledgePoint);
            OnClicked.Invoke(knowledgePoint);
        }

        /// <summary>
        /// 隐藏按钮点击时，触发
        /// </summary>
        private void hideButton_onClick()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="_knowledgePoint"></param>
        public void Show(KnowledgePoint _knowledgePoint)
        {
            knowledgePoint = _knowledgePoint;
            gameObject.SetActive(true);
            m_Text.text = knowledgePoint.Name;
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

