using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    /// <summary>
    /// 视口 （平移 缩放）
    /// </summary>
    public class ViewportTest : ScrollRect
    {
        /// <summary>
        /// 是否缩放
        /// </summary>
        private bool m_Zooming = false;

        /// <summary>
        /// 缩放值默认是 1
        /// </summary>
        private float m_ZoomValue = 1f;

        /// <summary>
        /// 缩放增量
        /// </summary>
        private float m_ZoomSensitivity = 0.1f;

        /// <summary>
        /// 缩放最小值
        /// </summary>
        private float m_ZoomMin = 0.25f;

        /// <summary>
        /// 缩放最大值
        /// </summary>
        private float m_ZoomMax = 2.0f;

        public override void OnScroll(PointerEventData data)
        {
            if (m_Zooming)
            {
                RectTransformUtils.ScreenPointToAdjustPivot(content, viewport, Input.mousePosition, data.enterEventCamera);
                // 缩放
                Vector2 delta = data.scrollDelta;
                if (delta.y > 0)
                    ZoomIn();
                else
                    ZoomOut();
            }
            else
            {
                base.OnScroll(data);
            }
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                m_Zooming = true;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle()
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                m_Zooming = false;
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <returns></returns>
        public void ZoomIn()
        {
            if (m_ZoomValue > m_ZoomMax)
                return;

            m_ZoomValue += m_ZoomSensitivity;
            content.localScale = new Vector3(m_ZoomValue, m_ZoomValue, 1);
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <returns></returns>
        public void ZoomOut()
        {
            if (m_ZoomValue < m_ZoomMin)
                return;

            m_ZoomValue -= m_ZoomSensitivity;
            content.localScale = new Vector3(m_ZoomValue, m_ZoomValue, 1);
        }
    }
}
