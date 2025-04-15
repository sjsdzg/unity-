using UnityEngine;
using System.Collections;
using XFramework.Core;
using UnityEngine.UI;
using DG.Tweening;

namespace XFramework.Common
{
    public class LoadingBar : MonoSingleton<LoadingBar>
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// 进度图片
        /// </summary>
        private Image m_FillImage;

        /// <summary>
        /// 进度文本
        /// </summary>
        private Text m_ProgressText;

        /// <summary>
        /// 描述文本
        /// </summary>
        private Text m_DescriptionText;

        /// <summary>
        /// 背景
        /// </summary>
        private RectTransform m_Background;

        protected override void Init()
        {
            base.Init();
            m_Background = transform.Find("Background").GetComponent<RectTransform>();
            m_CanvasGroup = transform.Find("Background").GetComponent<CanvasGroup>();
            m_CanvasGroup.alpha = 0;
            m_FillImage = transform.Find("Background/Rect/FillImage").GetComponent<Image>();
            m_FillImage.type = Image.Type.Filled;
            m_FillImage.fillAmount = 0;
            m_ProgressText = transform.Find("Background/Rect/ProgressText").GetComponent<Text>();
            m_DescriptionText = transform.Find("Background/Rect/DescriptionText").GetComponent<Text>();
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
                gameObject.SetActive(true);
                m_Background.SetParent(transform);
            }
            else
            {
                m_Background.SetParent(parent);
                m_Background.SetAsLastSibling();
            }
            m_Background.offsetMin = Vector2.zero;
            m_Background.offsetMax = Vector2.zero;
            m_CanvasGroup.DOFade(1, 0.2f);
            m_FillImage.fillAmount = progress;
            m_ProgressText.text = (progress * 100).ToString("F0") + "%";
            m_DescriptionText.text = info;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            m_CanvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                m_FillImage.fillAmount = 0;
                m_ProgressText.text = "";
                m_DescriptionText.text = "";
                gameObject.SetActive(false);
            });
        }
    }
}

