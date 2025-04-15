using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Module;
using XFramework.UIWidgets;

namespace XFramework.Diagram
{
    public class NodeCreateToolArgs : GraphToolBaseArgs
    {
        public string Name { get; set; }

        public Vector2 SizeDelta { get; set; }

        public List<Variable> Variables { get; set; }

        public bool IsDrag { get; set; }

        public NodeCreateToolArgs(string name, Vector2 sizeDelta)
        {
            Name = name;
            SizeDelta = sizeDelta;
        }
    }

    public class NodeCreateTool : GraphToolBase
    {
        private bool hasSumbit = false;

        private Node m_Node;
        private GameObject m_NodeGameObject;
        private NodeWidget m_NodeWidget;
        private GraphViewer m_Viewport;

        private RectTransform m_ViewportRectTransform;
        private RectTransform m_CanvasRectTransform;
        private RectTransform m_ContainerRectTransform;
        private RectTransform m_AssetRectTransform;
        private RectTransform m_NodeParent;

        private Vector2 screenPoint;
        private bool insideViewport;
        private float scale;
        private bool isDrag;

        public override void Init(GraphToolBaseArgs t)
        {
            base.Init(t);
            hasSumbit = false;
            m_NodeParent = null;

            m_Viewport = VisualManager.Instance.Viewer;

            m_ViewportRectTransform = m_Viewport.GetComponent<RectTransform>();
            m_CanvasRectTransform = m_Viewport.Canvas.GetComponent<RectTransform>();
            m_ContainerRectTransform = m_Viewport.Container;
            m_AssetRectTransform = m_Viewport.Assist;

            NodeCreateToolArgs args = (NodeCreateToolArgs)t;
            m_Node = new Node();
            m_Node.Name = args.Name;
            m_Node.SizeDelta = args.SizeDelta;
            m_Node.Variables = args.Variables;
            m_Node.Points = new List<Vector2>()
            {
                // 左
                new Vector2(0, 0.5f),
                // 右
                new Vector2(1f, 0.5f),
                // 上
                new Vector2(0.5f, 1f),
                // 下
                new Vector2(0.5f, 0f),
            };

            isDrag = args.IsDrag;

            m_NodeGameObject = VisualManager.Instance.CreateNodeWidget(m_Node);
            m_NodeWidget = m_NodeGameObject.GetComponent<NodeWidget>();
            m_NodeWidget.raycastTarget = false;
            //VisualManager.Instance.SetParentAndAlign(m_NodeGameObject, m_CanvasRectTransform.gameObject);
        }

        public override void Update()
        {
            base.Update();
            screenPoint = Input.mousePosition;
            scale = m_Viewport.Scale;

            //RectTransformUtility.ScreenPointToLocalPointInRectangle(m_CanvasRectTransform, screenPoint, null, out Vector2 localPoint);
            //m_Node.Position = localPoint;
            UpdateNodeParentAndAlign();

            if (insideViewport)
            {
                if (isDrag)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        Submit();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Submit();
                    }
                }
            }
        }

        public void UpdateNodeParentAndAlign()
        {
            insideViewport = RectTransformUtility.RectangleContainsScreenPoint(m_ViewportRectTransform, screenPoint, GraphMaster.Instance.Camera);
            RectTransform rectTransform;
            if (insideViewport)
            {
                rectTransform = m_ContainerRectTransform;
                //m_Node.LossyScale = scale;
                m_NodeWidget.Scale = scale;
            }
            else
            {
                rectTransform = m_CanvasRectTransform;
            }

            if (m_NodeParent == null || rectTransform != m_NodeParent)
            {
                m_NodeParent = rectTransform;
                VisualManager.Instance.SetParentAndAlign(m_NodeGameObject, m_NodeParent.gameObject);
                m_NodeGameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_NodeParent, screenPoint, GraphMaster.Instance.Camera, out Vector2 localPoint);
            if (insideViewport)
            {
                GraphMaster.Instance.CurrentGraph.SnapAxis(localPoint, out Vector2 axis);
                GraphMaster.Instance.ShowSnapLine(axis);

                if (!float.IsInfinity(axis.x))
                    localPoint.x = axis.x;

                if (!float.IsInfinity(axis.y))
                    localPoint.y = axis.y;
            }

            m_Node.Position = localPoint;
        }

        public override void Submit()
        {
            base.Submit();

            // Terminal Points
            //foreach (var point in m_Node.Points)
            //{
            //    GameObject pointGameObject = VisualManager.Instance.CreatePointWidget(GraphUtils.GetWidgetID());
            //    PointWidget pointWidget = pointGameObject.GetComponent<PointWidget>();

            //    pointWidget.raycastTarget = false;
            //    pointWidget.color = Color.white;
            //    pointWidget.StrokeWidth = 1;
            //    pointWidget.StrokeColor = new Color32(138, 65, 65, 255);
            //    pointWidget.SizeDelta = new Vector2(8, 8);

            //    VisualManager.Instance.SetParentAndAlign(pointWidget.gameObject, m_AssetRectTransform.gameObject);
            //    pointGameObject.GetComponent<RectTransform>().localPosition = GraphUtils.GetPosition(m_Node, point);
            //    m_NodeWidget.TerminalPoints.Add(pointWidget);
            //}

            hasSumbit = true;

            m_NodeWidget.raycastTarget = true;
            GraphMaster.AddUnit(m_Node);
            GraphMaster.Instance.ActiveTool = null;
        }

        public override void Cancel()
        {
            base.Cancel();
            GraphMaster.Instance.ActiveTool = null;
        }

        public override void Release()
        {
            base.Release();
            GraphMaster.Instance.ShowSnapLine(Vector2.negativeInfinity);
            if (!hasSumbit && m_Node != null)
            {
                VisualManager.Instance.DestoryNodeWidget(m_Node);
            }
            this.m_Node = null;
        }
    }
}
