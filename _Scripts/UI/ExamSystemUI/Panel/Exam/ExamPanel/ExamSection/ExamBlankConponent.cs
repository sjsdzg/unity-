using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ExamBlankConponent : MonoBehaviour
    {
        public class OnEditCompletedEvent : UnityEvent<bool> { }

        private OnEditCompletedEvent m_OnEditCompleted = new OnEditCompletedEvent();
        /// <summary>
        /// 编辑完成时，触发
        /// </summary>
        public OnEditCompletedEvent OnEditCompleted
        {
            get { return m_OnEditCompleted; }
            set { m_OnEditCompleted = value; }
        }

        private string blankText = "";
        /// <summary>
        /// 填空名称
        /// </summary>
        public string BlankText
        {
            get
            {
                blankText = (text == null ? "" : text.text);
                return blankText;
            }
            set
            {
                blankText = value;
                if (text != null)
                {
                    text.text = value;
                }
            }
        }

        private string blankContent = "";
        /// <summary>
        /// 填空内容
        /// </summary>
        public string BlankContent
        {
            get
            {
                blankContent = (inputField == null ? "" : inputField.text);
                return blankContent;
            }
            set
            {
                blankContent = value;
                if (inputField != null)
                {
                    inputField.text = blankContent;
                }
            }
        }

        /// <summary>
        /// 选项名称文本
        /// </summary>
        private Text text;

        /// <summary>
        /// 输入框
        /// </summary>
        private InputField inputField;

        void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            inputField.onEndEdit.AddListener(inputField_onEndEdit);
        }

        private void inputField_onEndEdit(string arg0)
        {
            if (!string.IsNullOrEmpty(arg0))
            {
                OnEditCompleted.Invoke(true);
            }
            else
            {
                OnEditCompleted.Invoke(false);
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="content"></param>
        public void SetParams(string text)
        {
            BlankText = text;
        }
    }
}
