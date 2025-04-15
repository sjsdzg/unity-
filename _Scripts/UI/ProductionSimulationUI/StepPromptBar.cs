using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XFramework.Module;
using XFramework.Core;

namespace XFramework.UI
{
    public class StepPromptBar : MonoBehaviour
    {
        public AudioClip audioClip;

        private CanvasGroup canvasGroup;

        private Image image;

        private Text text;

        private void Awake()
        {
            image = GetComponent<Image>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            text = transform.Find("Text").GetComponent<Text>();
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="message"></param>
        public void Show(string content)
        {
            text.text = content;
            gameObject.SetActive(true);
            canvasGroup.DOFade(0, 0);
            image.rectTransform.DOAnchorPosX(0, 0);
            if (audioClip!=null)
            {
                ProductionAudioController.Instance.Play(audioClip);
            }

            image.rectTransform.DOAnchorPosX(-Screen.width * 0.5f, 1).SetEase(Ease.OutBack).From();
            canvasGroup.DOFade(1, 1).OnComplete(() =>
            {
                CoroutineManager.Instance.Invoke(1, () =>
                {
                    canvasGroup.DOFade(0, 0.5f);
                    image.rectTransform.DOAnchorPosX(+Screen.width * 0.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        canvasGroup.DOFade(0, 0);
                        image.rectTransform.DOAnchorPosX(0, 0);
                    });
                });
            });
        }
    }
    
}
