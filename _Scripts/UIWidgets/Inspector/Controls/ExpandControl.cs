using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public class ExpandControl : ControlBase
    {
        public override string GetKey()
        {
            return typeof(ExpandControl).ToString();
        }

        /// <summary>
        /// 头文本
        /// </summary>
        [SerializeField]
        private Text m_Label;

        [SerializeField]
        private RectTransform m_Header;
        /// <summary>
        /// 头
        /// </summary>
        public RectTransform Header { get { return m_Header; } }

        [SerializeField]
        private RectTransform m_Content;
        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content { get { return m_Content; } }

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

        protected override void Awake()
        {
            base.Awake();
            var toggle = m_Header.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(toggle_onValueChanged);
        }

        protected override void UpdateControl(object value)
        {
            base.UpdateControl(value);

            m_Label.text = value.ToString();
        }

        private void toggle_onValueChanged(bool value)
        {
            m_Content.gameObject.SetActive(value);
            IsExpanded = value;
        }
    }
}

