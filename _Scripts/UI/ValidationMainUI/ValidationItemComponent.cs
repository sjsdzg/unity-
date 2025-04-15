using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using UnityEngine.Events;
using System;

namespace XFramework.UI
{
    public class ValidationItemComponent : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        /// <summary>
        /// 工具栏
        /// </summary>
        public ValidationContentComponent Parent { get; private set; }

        /// <summary>
        /// 工具数据
        /// </summary>
        public ValidationItem data { get; private set; }

        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<ValidationItemComponent> { }

        private ClickEvent m_OnClick = new ClickEvent();
        /// <summary>
        /// 工具点击事件
        /// </summary>
        public ClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
            transform.GetComponent<Button>().onClick.AddListener(Button_onClick);
        }

        private void Button_onClick()
        {
            OnClick.Invoke(this);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(ValidationContentComponent bar, ValidationItem _data)
        {
            Parent = bar;
            data = _data;
            text.text = _data.Name;
        }
    }
}

