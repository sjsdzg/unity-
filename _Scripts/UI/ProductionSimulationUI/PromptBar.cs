using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using DG.Tweening;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.UI
{
    public class PromptBar : MonoBehaviour
    {
        /// <summary>
        /// RectTransform
        /// </summary>
        private RectTransform m_RectTransform;

        /// <summary>
        /// text
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 过渡时间
        /// </summary>
        public float duration = 0.3f;

        /// <summary>
        /// 
        /// </summary>
        private bool visible;

        void Start()
        {
            m_RectTransform = transform as RectTransform;
            m_Text = transform.Find("Text").GetComponent<Text>();
            Hide();
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="message"></param>
        public void Show(string message)
        {
            m_Text.text = message;
            gameObject.SetActive(true);
            m_RectTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            m_RectTransform.DOScale(1, duration);
            visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Hide()
        {
            m_Text.text = "";
            gameObject.SetActive(false);
            visible = false;
        }
    }
}
