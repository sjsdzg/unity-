using UnityEngine;
using System.Collections;
using XFramework.UIWidgets;
using UnityEngine.UI;

namespace XFramework.Diagram
{
    public class PointWidget : VisualWidget
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
                    SetVerticesDirty();
                }
            }
        }

        private Vector2 sizeDelta;
        /// <summary>
        /// width height
        /// </summary>
        public Vector2 SizeDelta
        {
            get { return sizeDelta; }
            set 
            {
                if (SetPropertyUtils.SetStruct(ref sizeDelta, value))
                {
                    rectTransform.sizeDelta = value;
                }
            }
        }

        public override void SetScaleDirty()
        {
            base.SetScaleDirty();
            rectTransform.sizeDelta = sizeDelta / Scale;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            var rect = GetPixelAdjustedRect();

            float width = m_StrokeWidth / Scale;

            vh.Clear();
            vh.AddCircle(rect, color, width, m_StrokeColor);
        }
    }
}
