using UnityEngine;
using System.Collections;
using XFramework.Core;
using UnityEngine.UI;
using DG.Tweening;

namespace XFramework.Common
{
    public class ProgressBar : MonoSingleton<ProgressBar>
    {
        /// <summary>
        /// 进度图片
        /// </summary>
        private Image m_FillImage;

        /// <summary>
        /// 描述文本
        /// </summary>
        private Text m_Content;

        /// <summary>
        /// 背景
        /// </summary>
        private RectTransform m_Background;

        protected override void Init()
        {
            base.Init();
            m_Background = transform.Find("Background").GetComponent<RectTransform>();
            m_FillImage = transform.Find("Background/Panel/Slider/Fill").GetComponent<Image>();
            m_FillImage.type = Image.Type.Filled;
            m_FillImage.fillAmount = 0;
            m_Content = transform.Find("Background/Panel/Content").GetComponent<Text>();
            //隐藏
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public void Show(float progress, string info, RectTransform parent = null)
        {
            if (parent == null)
            {
                m_Background.SetParent(transform);
                gameObject.SetActive(true);
            }
            else
            {
                m_Background.SetParent(parent);
                m_Background.SetAsLastSibling();
            }
            m_Background.offsetMin = Vector2.zero;
            m_Background.offsetMax = Vector2.zero;
            m_FillImage.fillAmount = progress;
            m_Content.text = info;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public override void Hide()
        {
            m_FillImage.fillAmount = 0;
            m_Content.text = "";
            gameObject.SetActive(false);
        }
    }
}

