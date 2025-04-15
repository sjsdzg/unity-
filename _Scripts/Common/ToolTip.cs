using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.Common
{
    /// <summary>
    /// 提示框
    /// </summary>
    public class ToolTip : MonoSingleton<ToolTip>
    {
        /// <summary>
        /// 当前状态是否显示
        /// </summary>
        private bool visible = true;

        /// <summary>
        /// m_RectTransform
        /// </summary>
        private RectTransform m_RectTransform = null;

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text = null;

        /// <summary>
        /// 内容缓存
        /// </summary>
        private string m_CacheText;

        /// <summary>
        /// 偏移
        /// </summary>
        [SerializeField]
        public Vector3 offset = new Vector3(0, -24, 0);
    
        /// <summary>
        /// 限制宽度
        /// </summary>
        [Header("Limit")]
        public bool widthLimit = false;

        /// <summary>
        /// 宽度
        /// </summary>
        public int width = 20;

        protected override void Init()
        {
            base.Init();
            m_RectTransform = transform as RectTransform;
            m_Text = transform.GetComponentInChildren<Text>();
            m_RectTransform.anchorMin = Vector2.zero;
            m_RectTransform.anchorMax = Vector2.zero;
            Hide();
        }

        void LateUpdate()
        {
            if (visible)
            {
                //Vector3 v = Input.mousePosition + offset;
                //m_RectTransform.anchoredPosition = v;
                SetPosition(Input.mousePosition);
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="mousePosition"></param>
        protected virtual void SetPosition(Vector3 mousePosition)
        {
            Vector3 pos = mousePosition + offset;
            var width = m_RectTransform.rect.width;
            var height = m_RectTransform.rect.height;

            float x = 0;
            float y = 0;
            //x轴位置
            if (pos.x > Screen.width - width * (1 - m_RectTransform.pivot.x))
            {
                x = Screen.width - width * (1 - m_RectTransform.pivot.x);
            }
            else if (pos.x < width * m_RectTransform.pivot.x)
            {
                x = width * m_RectTransform.pivot.x;
            }
            else
            {
                x = pos.x;
            }
            //y轴位置
            if (pos.y > Screen.height - height * (1 - m_RectTransform.pivot.x))
            {
                y = Screen.height - height * (1 - m_RectTransform.pivot.x);
            }
            else if (pos.y < height * m_RectTransform.pivot.y)
            {
                y = height * m_RectTransform.pivot.y;
            }
            else
            {
                y = pos.y;
            }
            //设置
            m_RectTransform.anchoredPosition = new Vector2(x, y);
        }

        /// <summary>
        /// 显示提示内容
        /// </summary>
        /// <param name="b">show?</param>
        public void Show(bool b, float wait = 0.0f, string t = "")
        {
            if (Instance == null)
                return;
            visible = b;
            m_CacheText = Limit(t);
            if (b)
            {
                if (string.IsNullOrEmpty(t))
                    return;
                Invoke("Show", wait);
            }
            else
            {
                Hide();
                CancelInvoke("Show");
            }
        }

        /// <summary>
        /// 限制宽度
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Limit(string text)
        {
            if (!widthLimit)
                return text;

            StringBuilder sb = new StringBuilder(text);
            for (int i = width; i < sb.Length; i += width + 1)
            {
                sb.Insert(i, "\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 显示
        /// </summary>
        private void Show()
        {
            gameObject.SetActive(true);
            m_Text.text = m_CacheText;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        private void Hide()
        {
            if (m_Text != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
