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
    /// 考试选项
    /// </summary>
    public class UserOptionComponent : MonoBehaviour
    {
        public class OnValueChangedEvent : UnityEvent<bool> { }

        private OnValueChangedEvent m_OnValueChanged = new OnValueChangedEvent();
        /// <summary>
        /// 选择改变时，触发
        /// </summary>
        public OnValueChangedEvent OnValueChanged
        {
            get { return m_OnValueChanged; }
            set { m_OnValueChanged = value; }
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

        private string optionContent = "";
        /// <summary>
        /// 选项内容
        /// </summary>
        public string OptionContent
        {
            get
            {
                optionContent = (content == null ? "" : content.text);
                return optionContent;
            }
            set
            {
                optionContent = value;
                if (content != null)
                {
                    content.text = value;
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
        private Text content;

        void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
            content = transform.Find("Content").GetComponent<Text>();
        }

        private void toggle_onValueChanged(bool value)
        {
            OnValueChanged.Invoke(value);
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
            OptionContent = content;
        }

    }
}
