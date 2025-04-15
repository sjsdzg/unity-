using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace XFramework.UI
{
    public class AssemblyHintBar : MonoBehaviour
    {
        /// <summary>
        /// 正确内容
        /// </summary>
        private RectTransform m_Correct;

        /// <summary>
        /// 错误内容
        /// </summary>
        private RectTransform m_Error;

        private void Awake()
        {
            m_Correct = transform.Find("Correct").GetComponent<RectTransform>();
            m_Error = transform.Find("Error").GetComponent<RectTransform>();
            CanvasGroup canvasGroupCorrect = m_Correct.gameObject.AddComponent<CanvasGroup>();
            CanvasGroup canvasGroupError = m_Error.gameObject.AddComponent<CanvasGroup>();
            canvasGroupCorrect.alpha = 0;
            canvasGroupCorrect.blocksRaycasts = false;
            canvasGroupError.alpha = 0;
            canvasGroupError.blocksRaycasts = false;
        }


        public void DisplayCorrect()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_Correct.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
            sequence.Join(m_Correct.DOScale(1.5f, 0.2f));
            sequence.Append(m_Correct.DOScale(1, 0.5f));
            sequence.Append(m_Correct.GetComponent<CanvasGroup>().DOFade(0, 0.5f));
        }

        public void DisplayError()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_Error.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
            sequence.Join(m_Error.DOScale(1.5f, 0.2f));
            sequence.Append(m_Error.DOScale(1, 0.5f));
            sequence.Append(m_Error.GetComponent<CanvasGroup>().DOFade(0, 0.5f));
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.B))
        //    {
        //        DisplayCorrect();
        //    }

        //    if (Input.GetKeyDown(KeyCode.N))
        //    {
        //        DisplayError();
        //    }
        //}
    }
}

