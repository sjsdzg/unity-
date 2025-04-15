using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.Common
{
    /// <summary>
    /// Marker
    /// </summary>
    public class Marker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public class OnClickedEvent : UnityEvent<Marker> { }

        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup = null;

        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text = null;

        /// <summary>
        /// 要在小地图上显示的项
        /// </summary>
        public MarkerObject Item { get; set; }

        /// <summary>
        /// 是否缩放
        /// </summary>
        public bool canScale = true;

        /// <summary>
        /// Hover,缩放比例
        /// </summary>
        private Vector3 onHoverScale = new Vector3(1.1f, 1.1f, 1.1f);

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 标签点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        void Awake()
        {
            m_CanvasGroup = transform.GetOrAddComponent<CanvasGroup>();

            m_CanvasGroup.ignoreParentGroups = true;
            m_CanvasGroup.alpha = 0;
        }

        IEnumerator FadeMarker(float delay)
        {
            yield return new WaitForSeconds(delay);

            while (m_CanvasGroup.alpha < 1)
            {
                m_CanvasGroup.alpha += Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 延迟渲染
        /// </summary>
        /// <param name="v"></param>
        public void DelayRender(float v)
        {
            StartCoroutine(FadeMarker(v));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked.Invoke(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canScale)
            {
                transform.DOScale(onHoverScale, 0.1f);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (canScale)
            {
                transform.DOScale(Vector3.one, 0.3f);
            }
        }
    }
}
