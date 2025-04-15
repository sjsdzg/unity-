using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.UI;
using XFramework.Module;
using DG.Tweening;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class ValidationHomePanel : MonoBehaviour
    {
        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<string> { }

        private ClickEvent m_OnClicked = new ClickEvent();
        /// <summary>
        /// 点击触发
        /// </summary>
        public ClickEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// buttons
        /// </summary>
        private Button[] buttons;

        private void Awake()
        {
            buttons = transform.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                string text = button.GetComponentInChildren<Text>().text;
                button.onClick.AddListener(() => button_onClick(text));
            }
        }

        public void Show()
        {
            //transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            //transform.DOScale(1, 0.2f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 工具点击时，触发事件
        /// </summary>
        private void button_onClick(string arg0)
        {
            m_OnClicked.Invoke(arg0);
        }
    }
}


