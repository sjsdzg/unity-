using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class OptionComponent : MonoBehaviour
    {
        public class OnCloseEvent : UnityEvent<OptionComponent> { }

        private OnCloseEvent m_OnClosed = new OnCloseEvent();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public OnCloseEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        private string optionName = "";
        /// <summary>
        /// 选项名称
        /// </summary>
        public string OptionName
        {
            get
            {
                optionName = (text == null ? "" : text.text);
                return optionName;
            }
            set
            {
                optionName = value;
                if (text != null)
                {
                    text.text = value;
                }
            }
        }

        private bool _checked;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked
        {
            get
            {
                _checked = (toggle == null ? false : toggle.isOn);
                return _checked;
            }
            set
            {
                _checked = value;
                if (toggle != null)
                {
                    toggle.isOn = value;
                }
            }
        }

        private string optionContent = "";
        /// <summary>
        /// 选项内容
        /// </summary>
        public string OptionContent
        {
            get
            {
                optionContent = (inputField == null ? "" : inputField.text);
                return optionContent;
            }
            set
            {
                optionContent = value;
                if (inputField != null)
                {
                    inputField.text = value;
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
        /// toggle
        /// </summary>
        private Toggle toggle;

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
            toggle = transform.Find("Toggle").GetComponent<Toggle>();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isOn"></param>
        /// <param name="content"></param>
        /// <param name="active"></param>
        public void SetParams(string name, bool isOn, string content, bool active = true)
        {
            OptionName = name;
            Checked = isOn;
            OptionContent = content;
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
