using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.UIWidgets;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.Diagram
{
    public class NodeWidget : UniversalWidget, ICanvasRaycastFilter, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Node node;
        /// <summary>
        /// 节点
        /// </summary>
        public Node Node
        {
            get { return node; }
            set 
            {
                node = value;
                node.ActiveChanged += Node_ActiveChanged;
                node.NameChanged += Node_NameChanged;
                node.TransformChanged += Node_TransformChanged;
                node.VerticesChanged += Node_VerticesChanged;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public Text Text { get; set; }

        [SerializeField]
        private float m_StrokeWidth = 2;
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
        private Color m_StrokeColor = new Color32(20, 20, 20, 255);
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

        private List<PointWidget> m_TerminalPoints = new List<PointWidget>();
        /// <summary>
        /// 端点
        /// </summary>
        public List<PointWidget> TerminalPoints
        {
            get { return m_TerminalPoints; }
            set { m_TerminalPoints = value; }
        }

        /// <summary>
        /// 指针位置
        /// </summary>
        private Vector2 pointerLocalPosition;

        /// <summary>
        /// widget 位置
        /// </summary>
        private Vector2 widgetLocalPosition;

        private void Node_ActiveChanged()
        {
            gameObject.SetActive(node.Active);
        }

        private void Node_NameChanged()
        {
            Text.text = node.Name;
        }

        private void Node_TransformChanged()
        {
            RectTransform rectTransform = transform.GetComponent<RectTransform>();
            rectTransform.localPosition = node.Position;
            rectTransform.sizeDelta = node.SizeDelta;

            int length = m_TerminalPoints.Count;
            for (int i = 0; i < length; i++)
            {
                var pos = GraphUtils.GetPosition(node, node.Points[i]);
                m_TerminalPoints[i].GetComponent<RectTransform>().localPosition = pos;
            }
        }

        private void Node_VerticesChanged()
        {
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var rect = GetPixelAdjustedRect();
            var width = m_StrokeWidth * Scale;

            if (width < 1)
                width = 1 / Scale;

            vh.Clear();
            vh.AddRect(rect, color, width, m_StrokeColor);
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (!isActiveAndEnabled)
                return true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out Vector2 localPoint);
            var expandRect = RectTransformUtils.Expand(rectTransform.rect, new Vector2(16, 16));
            return expandRect.Contains(localPoint);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    foreach (var point in m_TerminalPoints)
                    {
                        point.gameObject.SetActive(false);
                    }
                    StrokeColor = VisualManager.s_StrokeColor;
                    break;
                case SelectionState.Highlighted:
                    foreach (var point in m_TerminalPoints)
                    {
                        point.gameObject.SetActive(true);
                    }
                    StrokeColor = VisualManager.s_HighlightedColor;
                    break;
                case SelectionState.Selected:
                    foreach (var point in m_TerminalPoints)
                    {
                        point.gameObject.SetActive(true);
                    }
                    StrokeColor = VisualManager.s_SelectedColor;
                    break;
                default:
                    break;
            }
        }

        public override void Press()
        {
            base.Press();
            GraphMaster.Instance.SetSelectedUnit(node);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!VisualManager.Instance.CanDragWidget)
                return;

            //widgetLocalPosition = rectTransform.localPosition;
            widgetLocalPosition = node.Position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent, eventData.position, eventData.pressEventCamera, out pointerLocalPosition);
            // Undo
            UndoManager.Instance.RecordObject(node, ReflectionUtils.GetPropertyInfo(node, "Position"));
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetNodePosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetNodePosition(eventData);
            GraphMaster.Instance.ShowSnapLine(Vector2.negativeInfinity);
        }

        public void SetNodePosition(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!VisualManager.Instance.CanDragWidget)
                return;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Parent, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                Vector2 offset = localPoint - pointerLocalPosition;
                //node.Position = widgetLocalPosition + offset;

                var pos = widgetLocalPosition + offset;
                node.Position = pos;

                GraphMaster.Instance.CurrentGraph.SnapAxis(node, out Vector2 axis);
                GraphMaster.Instance.ShowSnapLine(axis);

                if (!float.IsInfinity(axis.x))
                    pos.x = axis.x;

                if (!float.IsInfinity(axis.y))
                    pos.y = axis.y;

                node.Position = pos;
            }
        }
    }
}