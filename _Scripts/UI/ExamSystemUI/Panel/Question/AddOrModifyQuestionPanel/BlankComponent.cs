using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class BlankComponent : MonoBehaviour
    {
        public class OnCloseEvent : UnityEvent<BlankComponent> { }

        private OnCloseEvent m_OnClosed = new OnCloseEvent();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public OnCloseEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
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

        private bool buttonCloseActive;
        /// <summary>
        /// 关闭按钮是否显示
        /// </summary>
        public bool ButtonCloseActive
        {
            get
            {
                buttonCloseActive = (buttonClose == null ? false : buttonClose.gameObject.activeSelf);
                return buttonCloseActive;
            }
            set
            {
                buttonCloseActive = value;
                if (buttonClose != null)
                {
                    buttonClose.gameObject.SetActive(value);
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

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isOn"></param>
        /// <param name="content"></param>
        /// <param name="active"></param>
        public void SetParams(string text, string content, bool active = true)
        {
            BlankText = text;
            BlankContent = content;
            ButtonCloseActive = active;
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            OnClosed.Invoke(this);
        }
    }
}
