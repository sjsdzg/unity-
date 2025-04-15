using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Module;

namespace XFramework.UI
{
    public class QuestionComponent : MonoBehaviour
    {
        public class OnCloseEvent : UnityEvent<QuestionComponent> { }

        private OnCloseEvent m_OnClosed = new OnCloseEvent();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public OnCloseEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        private string content;
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content
        {
            get
            {
                content = textContent.text;
                return content;
            }
            set
            {
                content = value;
                textContent.text = content;
            }
        }

        /// <summary>
        /// 绑定试题
        /// </summary>
        public Question Question { get; private set; }

        /// <summary>
        /// 内容文本
        /// </summary>
        private Text textContent;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            textContent = transform.Find("Text").GetComponent<Text>();
            buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
            //事件
            buttonClose.onClick.AddListener(() => OnClosed.Invoke(this));
        }

        public void SetParams(Question question)
        {
            Question = question;
            Content = Question.Content;
        }
    }
}
