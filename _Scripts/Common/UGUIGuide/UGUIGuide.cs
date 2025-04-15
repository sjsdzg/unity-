using UnityEngine;
using System.Collections;
using XFramework.Core;

namespace XFramework.Common
{
    public class UGUIGuide : MonoSingleton<UGUIGuide>
    {
        /// <summary>
        /// 目标
        /// </summary>
        [SerializeField]
        private RectTransform m_Target;

        /// <summary>
        /// 镂空Mask
        /// </summary>
        private HollowOutMask m_HollowOutMask;

        /// <summary>
        /// 引导手势
        /// </summary>
        private RectTransform m_Guide;

        /// <summary>
        /// 
        /// </summary>
        private RectTransform _cacheTrans = null;

        protected override void Init()
        {
            base.Init();
            _cacheTrans = transform.GetComponent<RectTransform>();
            _cacheTrans.offsetMin = Vector2.zero;
            _cacheTrans.offsetMax = Vector2.zero;
            m_HollowOutMask = transform.GetComponent<HollowOutMask>();
            m_HollowOutMask.enabled = true;
            m_Guide = transform.Find("Guide").GetComponent<RectTransform>();
            Hide();
        }

        public void Show(RectTransform target)
        {
            m_Target = target;
            if (null != m_Target)
            {
                gameObject.SetActive(true);
                Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_cacheTrans, m_Target);
                m_Guide.anchoredPosition = bounds.center;
                m_HollowOutMask.SetTarget(m_Target);
            }
        }


#if UNITY_EDITOR
        void Update()
        {
            if (null != m_Target)
            {
                Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_cacheTrans, m_Target);
                m_Guide.anchoredPosition = bounds.center;
                m_HollowOutMask.SetTarget(m_Target);
            }
        }
#endif
    }
}

