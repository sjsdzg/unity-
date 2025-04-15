using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 子工艺项
    /// </summary>
    public class Subprocess : MonoBehaviour
    {
        /// <summary>
        /// 子工艺内容
        /// </summary>
        private Text Content;

        /// <summary>
        /// 子工艺信息
        /// </summary>
        public SubprocessInfo Data { get; private set; }

        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<Subprocess> { }

        private ClickEvent m_OnClick = new ClickEvent();
        /// <summary>
        /// 子工艺点击事件
        /// </summary>
        public ClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        void Awake()
        {
            Content = transform.Find("Text").GetComponent<Text>();

            transform.GetComponent<Toggle>().onValueChanged.AddListener(transform_onValueChanged);
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="number"></param>
        /// <param name="content"></param>
        public void SetValue(SubprocessInfo info)
        {
            Data = info;
            Content.text = info.Name;
        }

        /// <summary>
        /// 点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void transform_onValueChanged(bool b)
        {
            if (b)
            {
                OnClick.Invoke(this);
            }
        }

        /// <summary>
        /// 选中
        /// </summary>
        public void Select()
        {
            transform.GetComponent<Toggle>().isOn = true;
        }

    }
}
