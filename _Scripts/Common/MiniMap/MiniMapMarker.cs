using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 小地图上显示的标签
    /// </summary>
    public class MiniMapMarker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public class OnClickedEvent : UnityEvent<MiniMapMarker> { }

        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup = null;

        /// <summary>
        /// 要在小地图上显示的项
        /// </summary>
        public MiniMapItem Item { get; set; }

        /// <summary>
        /// 是否缩放
        /// </summary>
        public bool isScaling = true;

        /// <summary>
        /// Hover,缩放比例
        /// </summary>
        private Vector3 onHoverScale = new Vector3(1.3f, 1.3f, 1.3f);

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
            if (GetComponent<CanvasGroup>() != null)
            {
                m_CanvasGroup = transform.GetComponent<CanvasGroup>();
            }

            m_CanvasGroup.ignoreParentGroups = true;
            //m_CanvasGroup.alpha = 0;
        }

        //IEnumerator FadeMark(float delay)
        //{
        //    yield return new WaitForSeconds(delay);

        //    while (m_CanvasGroup.alpha < 1)
        //    {
        //        m_CanvasGroup.alpha += Time.deltaTime * 2;
        //        yield return new WaitForEndOfFrame();
        //    }
        //}

        ///// <summary>
        ///// 延迟渲染
        ///// </summary>
        ///// <param name="v"></param>
        //public void DelayRender(float v)
        //{
        //    StartCoroutine(FadeMark(v));
        //}

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClicked.Invoke(this);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isScaling)
            {
                transform.DOScale(onHoverScale, 0.1f);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isScaling)
            {
                transform.DOScale(Vector3.one, 0.3f);
            }
        }

    }
}
