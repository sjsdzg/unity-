using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 超按钮
    /// </summary>
    public class HyperButton : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<HyperButton> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 鼠标点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// Panel类型
        /// </summary>
        public EnumPanelType PanelType { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        private Text text;
        /// <summary>
        /// 按钮
        /// </summary>
        private Button button;
        /// <summary>
        /// 手势鼠标支持
        /// </summary>
        private HandCursorSupport handCursor;

        private bool interactable = false;
        /// <summary>
        /// 是否交互
        /// </summary>
        public bool Interactable
        {
            get { return interactable; }
            set
            {
                interactable = value;
                if (button != null)
                {
                    button.interactable = interactable;
                    handCursor.AllowHandCursor = interactable;
                }
            }
        }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        void Awake()
        {
            button = transform.GetComponent<Button>();
            text = transform.GetComponentInChildren<Text>();
            button.onClick.AddListener(button_onClick);
            handCursor = transform.GetComponent<HandCursorSupport>();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="href"></param>
        /// <param name="text"></param>
        public void SetValue(EnumPanelType type, string text)
        {
            this.PanelType = type;
            this.text.text = text;
        }

        private void button_onClick()
        {
            OnClicked.Invoke(this);
        }
    }
}
