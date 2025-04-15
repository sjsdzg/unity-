using UnityEngine;
using System.Collections;
using XFramework.UIWidgets;
using UnityEngine.UI;
using System.Collections.Generic;

namespace XFramework.Diagram
{
    public class NodeConnectionWidget : UniversalWidget, ICanvasRaycastFilter
    {
        private NodeConnection connection;
        /// <summary>
        /// 连接
        /// </summary>
        public NodeConnection Connection
        {
            get { return connection; }
            set 
            { 
                connection = value;
                connection.VerticesChanged += Connection_VerticesChanged;
            }
        }

        [SerializeField]
        private float thickness = 2;
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

        private Vector2[] m_Points;
        /// <summary>
        /// 顶点列表
        /// </summary>
        public Vector2[] Points
        {
            get { return m_Points; }
        }

        private void Connection_VerticesChanged()
        {
            UpdatePoints();
            SetVerticesDirty();
        }

        protected void UpdatePoints()
        {
            var points = new List<Vector2>();
            points.Add(connection.GetSourcePosition());
            points.Add(connection.GetTargetPosition());
            m_Points = points.ToArray();

            float x = 0;
            float y = 0;
            foreach (var point in m_Points)
            {
                float abs_x = Mathf.Abs(point.x);
                float abs_y = Mathf.Abs(point.y);

                if (abs_x > x)
                    x = abs_x;

                if (abs_y > y)
                    y = abs_y;
            }

            rectTransform.sizeDelta = new Vector2(x, y);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            var lineThickness = thickness * Scale;
            if (lineThickness < 1)
                lineThickness = 1 / Scale;

            vh.Clear();
            vh.AddLineArrow(Points, lineThickness, new Vector2(lineThickness, lineThickness) * 4, color);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (Points == null || Points.Length < 2)
                return false;

            int length = Points.Length;
            float distance;
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            for (int i = 0; i < length - 1; i++)
            {
                Vector2 a = Points[i];
                Vector2 b = Points[i + 1];
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

        public override void Press()
        {
            base.Press();
            GraphMaster.Instance.SetSelectedUnit(connection);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    color = VisualManager.s_LineColor;
                    break;
                case SelectionState.Highlighted:
                    color = VisualManager.s_HighlightedColor;
                    break;
                case SelectionState.Selected:
                    color = VisualManager.s_SelectedColor;
                    break;
                default:
                    break;
            }
        }
    }
}

