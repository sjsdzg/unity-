using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 扩展Item
    /// </summary>
    public class ExpansionItemComponent : MonoBehaviour
    {
        public class PartItemClickEvent : UnityEvent<ExpansionItemComponent> { }

        private PartItemClickEvent m_OnClicked = new PartItemClickEvent();
        /// <summary>
        /// 部件Item点击触发
        /// </summary>
        public PartItemClickEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 按钮
        /// </summary>
        private Button m_Button;

        /// <summary>
        /// Image
        /// </summary>
        private Image m_Image;

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;


        /// <summary>
        /// 扩展Item信息
        /// </summary>
        public ExtensionItemInfo Data { get; set; }

        private void Awake()
        {
            m_Image = transform.Find("Image").GetComponent<Image>();
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_Button = transform.GetComponent<Button>();
            //Event
            m_Button.onClick.AddListener(m_Button_onClick);
        }

        public void SetValue(ExtensionItemInfo data, Sprite sprite)
        {
            Data = data;
            m_Image.sprite = sprite;
            m_Text.text = data.Name;
        }

        /// <summary>
        /// 按钮点击
        /// </summary>
        private void m_Button_onClick()
        {
            OnClicked.Invoke(this);
        }
    }



}
