using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace XFramework.UIWidgets
{
    public class Expander : MonoBehaviour
    {
        /// <summary>
        /// 头文本
        /// </summary>
        [SerializeField]
        private Text m_Label;

        /// <summary>
        /// 头
        /// </summary>
        [SerializeField]
        private RectTransform m_Header;
        /// <summary>
        /// 内容
        /// </summary>
        [SerializeField]
        private RectTransform m_Content;


        private bool m_isExpanded;
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                m_isExpanded = value; 
            }
        }

        private string m_Title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set 
            { 
                m_Title = value;
                m_Label.text = m_Title;
            }
        }

        private void Awake()
        {
            var toggle = m_Header.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(toggle_onValueChanged);
        }

        private void toggle_onValueChanged(bool value)
        {
            m_Content.gameObject.SetActive(value);
            IsExpanded = value;
        }
    }
}

