using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace XFramework.UIWidgets
{
    public class UIRect : UIPrimitiveBase
    {
        [SerializeField]
        private float m_StrokeWidth = 1;
        /// <summary>
        /// 轮廓宽度
        /// </summary>
        public float StrokeWidth
        {
            get { return m_StrokeWidth; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref m_StrokeWidth, value))
                {
                    m_StrokeWidth = value;
                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private Color m_StrokeColor = Color.black;
        /// <summary>
        /// 轮廓颜色
        /// </summary>
        public Color StrokeColor
        {
            get { return m_StrokeColor; }
            set 
            {
                if (SetPropertyUtils.SetColor(ref m_StrokeColor, value))
                {
                    m_StrokeColor = value;
                    SetVerticesDirty();
                }
            }
        }

        private float m_LossyScale = 1;
        /// <summary>
        /// 缩放值
        /// </summary>
        public float LossyScale
        {
            get { return m_LossyScale; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref m_LossyScale, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var rect = GetPixelAdjustedRect();
            var strokeWidth = m_StrokeWidth;
            var width = m_StrokeWidth * m_LossyScale;

            if (width < 1)
                strokeWidth = 1 / m_LossyScale;

            vh.Clear();
            vh.AddRect(rect, color, strokeWidth, m_StrokeColor);
        }

        private void LateUpdate()
        {
            LossyScale = rectTransform.lossyScale.x;
        }
    }
}

