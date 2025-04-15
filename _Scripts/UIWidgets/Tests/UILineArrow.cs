using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace XFramework.UIWidgets
{
    public class UILineArrow : MaskableGraphic, ICanvasRaycastFilter
    {
        [SerializeField]
        private Vector2[] m_Points;
        /// <summary>
        /// 顶点列表
        /// </summary>
        public Vector2[] Points
        {
            get { return m_Points; }
            set 
            {
                if (m_Points == value)
                    return;

                m_Points = value;
                SetVerticesDirty();
            }
        }

        [SerializeField]
        private float thickness = 1;
        /// <summary>
        /// 厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref thickness, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private Vector2 arrowSizeDelta = new Vector2(4, 4);
        /// <summary>
        /// 箭头（width, height）
        /// </summary>
        public Vector2 ArrowSizeDelta
        {
            get { return arrowSizeDelta; }
            set { arrowSizeDelta = value; }
        }


        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            vh.Clear();
            vh.AddLineArrow(m_Points, thickness, arrowSizeDelta, color);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (m_Points == null || m_Points.Length < 2)
                return false;

            int length = m_Points.Length;
            float distance;
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            for (int i = 0; i < length - 1; i++)
            {
                Vector2 a = m_Points[i];
                Vector2 b = m_Points[i + 1];
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out Vector2 localPoint);
                distance = PointToLineDistance(localPoint, a, b);
                if (distance < Thickness + 4)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float PointToLineDistance(Vector2 point, Vector2 a, Vector2 b)
        {
            float x = a.x;
            float y = a.y;
            float dx = b.x - x;
            float dy = b.y - y;

            if (dx != 0 || dy != 0)
            {
                float t = ((point.x - x) * dx + (point.y - y) * dy) / (dx * dx + dy * dy);
                if (t > 1)
                {
                    x = b.x;
                    y = b.y;
                }
                else if (t > 0)
                {
                    x += dx * t;
                    y += dy * t;
                }
            }

            dx = point.x - x;
            dy = point.y - y;

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}

