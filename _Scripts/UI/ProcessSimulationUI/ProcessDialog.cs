using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺对话框
    /// </summary>
    public class ProcessDialog : MonoBehaviour
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 内容
        /// </summary>
        private Text content;

        /// <summary>
        /// 宽度
        /// </summary>
        private float m_Width;

        /// <summary>
        /// 自身
        /// </summary>
        private RectTransform m_Rect;

        void Awake()
        {
            title = transform.Find("Header/Text").GetComponent<Text>();
            content = transform.Find("ScrollView/Viewport/Content/Text").GetComponent<Text>();
            m_Rect = transform.GetComponent<RectTransform>();
            m_Width = m_Rect.sizeDelta.y;
        }
        
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_content"></param>
        public void SetValue(string _title,string _content)
        {
            title.text = _title;
            content.text = _content;

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                PlayEffect();
            }
        }

        /// <summary>
        /// 播放动画效果
        /// </summary>
        private void PlayEffect()
        {
            m_Rect.DOAnchorPosX(m_Rect.anchoredPosition.x + m_Width, 0.4f).From();
        }
    }
}
