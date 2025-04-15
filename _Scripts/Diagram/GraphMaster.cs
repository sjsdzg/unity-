
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.Diagram
{
    public class GraphMaster : Singleton<GraphMaster>, IUpdate
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 工具集
        /// </summary>
        private Dictionary<Type, GraphToolBase> m_Tools = new Dictionary<Type, GraphToolBase>();

        private GraphToolBase activeTool;
        /// <summary>
        /// 激活工具
        /// </summary>
        public GraphToolBase ActiveTool
        {
            get { return activeTool; }
            set
            {
                if (activeTool != null)
                    activeTool.Release();

                activeTool = value;
            }
        }

        /// <summary>
        /// 获取工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTool<T>() where T : GraphToolBase, new()
        {
            GraphToolBase tool;
            if (m_Tools.TryGetValue(typeof(T), out tool))
                return (T)tool;

            tool = new T();
            m_Tools.Add(typeof(T), tool);
            return (T)tool;
        }

        private Camera m_Camera;
        /// <summary>
        /// 相机
        /// </summary>
        public Camera Camera
        {
            get 
            {
                if (m_Camera == null)
                    m_Camera = Camera.main;

                return m_Camera; 
            }
        }

        private Graph currentGraph;
        /// <summary>
        /// 当前流程图
        /// </summary>
        public Graph CurrentGraph
        {
            get { return currentGraph; }
            set { currentGraph = value; }
        }

        private GraphViewer m_Viewer;
        /// <summary>
        /// Viewport
        /// </summary>
        public GraphViewer Viewer
        {
            get 
            {
                if (m_Viewer == null)
                {
                    m_Viewer = GameObject.FindObjectOfType<GraphViewer>();
                }

                return m_Viewer; 
            }
        }

        private float scale = 1;
        /// <summary>
        /// 缩放
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set  { SetScale(value); }
        }

        #region 创建连接
        /// <summary>
        /// 是否拖拽
        /// </summary>
        private bool draging = false;

        /// <summary>
        /// 是否能创建
        /// </summary>
        private bool canCreate = false;

        /// <summary>
        /// 节点连接
        /// </summary>
        private NodeConnection createConnection;

        /// <summary>
        /// 节点连接 Widget
        /// </summary>
        private NodeConnectionWidget connectionWidget;

        /// <summary>
        /// 捕捉点 Widget
        /// </summary>
        private PointWidget snapPointWidget;
        #endregion

        #region 捕捉

        /// <summary>
        /// 捕捉线 X轴 
        /// </summary>
        private LineWidget xSnapLineWidget;

        /// <summary>
        /// 捕捉线 Y轴 
        /// </summary>
        private LineWidget ySnapLineWidget;
        #endregion

        private Unit m_currentSelectedUnit;
        /// <summary>
        /// 当前选定的实体对象
        /// </summary>
        public Unit currentSelectedUnit
        {
            get { return m_currentSelectedUnit; }
            set { m_currentSelectedUnit = value; }
        }

        /// <summary>
        /// 选中 Unit 事件
        /// </summary>
        public event Action<Unit> OnUnitSelected;

        protected override void Init()
        {
            base.Init();
            MonoDriver.Attach(this);

            currentGraph = new Graph();

            CreateSnapWidget();
        }

        public override void Release()
        {
            base.Release();
            MonoDriver.Detach(this);
        }

        /// <summary>
        /// 更新缩放
        /// </summary>
        /// <param name="value"></param>
        public void SetScale(float value)
        {
            if (scale == value)
                return;

            scale = value;
            //currentGraph.UpdateUnitScale(value);
            VisualManager.Instance.UpdateWidgetScale(value);
        }

        private void CreateSnapWidget()
        {
            GameObject snapPointGameObject = VisualManager.Instance.CreatePointWidget(GraphUtils.GetWidgetID());
            snapPointWidget = snapPointGameObject.GetComponent<PointWidget>();
            snapPointWidget.raycastTarget = false;
            snapPointWidget.color = new Color32(138, 65, 65, 75);
            snapPointWidget.StrokeWidth = 1;
            snapPointWidget.StrokeColor = new Color32(138, 65, 65, 150);
            snapPointWidget.SizeDelta = new Vector2(32, 32);
            VisualManager.Instance.SetParentAndAlign(snapPointWidget.gameObject, Viewer.Assist.gameObject);
            snapPointWidget.gameObject.SetActive(false);

            GameObject xSnapLineGameObject = VisualManager.Instance.CreateLineWidget(GraphUtils.GetWidgetID());
            xSnapLineWidget = xSnapLineGameObject.GetComponent<LineWidget>();
            xSnapLineWidget.raycastTarget = false;
            xSnapLineWidget.color = new Color32(0, 136, 207, 255);
            xSnapLineWidget.Thickness = 1;
            VisualManager.Instance.SetParentAndAlign(xSnapLineWidget.gameObject, Viewer.Assist.gameObject);
            xSnapLineWidget.gameObject.SetActive(false);

            GameObject ySnapLineGameObject = VisualManager.Instance.CreateLineWidget(GraphUtils.GetWidgetID());
            ySnapLineWidget = ySnapLineGameObject.GetComponent<LineWidget>();
            ySnapLineWidget.raycastTarget = false;
            ySnapLineWidget.color = new Color32(0, 136, 207, 255);
            ySnapLineWidget.Thickness = 1;
            VisualManager.Instance.SetParentAndAlign(ySnapLineWidget.gameObject, Viewer.Assist.gameObject);
            ySnapLineWidget.gameObject.SetActive(false);
        }

        public void ShowSnapPoint(bool active, Vector2 pos)
        {
            snapPointWidget.gameObject.SetActive(active);
            snapPointWidget.rectTransform.localPosition = pos;
        }

        public void ShowSnapLine(Vector2 point)
        {
            if (!float.IsInfinity(point.x))
            {
                xSnapLineWidget.gameObject.SetActive(true);
                xSnapLineWidget.P1 = new Vector2(point.x, -3000);
                xSnapLineWidget.P2 = new Vector2(point.x, 3000);
            }
            else
            {
                xSnapLineWidget.gameObject.SetActive(false);
            }

            if (!float.IsInfinity(point.y))
            {
                ySnapLineWidget.gameObject.SetActive(true);
                ySnapLineWidget.P1 = new Vector2(-3000, point.y);
                ySnapLineWidget.P2 = new Vector2(3000, point.y);
            }
            else
            {
                ySnapLineWidget.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="unit"></param>
        public static void AddUnit(Unit unit)
        {
            Instance.currentGraph.AddUnit(unit);
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="unit"></param>
        public static void RemoveUnit(Unit unit)
        {
            using (new UndoGroup("remove unit"))
            {
                Instance.currentGraph.RemoveUnit(unit);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public static void ClearGraph()
        {
            using (new UndoGroup("clear graph"))
            {
                Instance.currentGraph.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<NodeConnection> GetConnectionsWithNode(Node node)
        {
            return Instance.currentGraph.GetConnectionsWithNode(node);
        }

        public void SetSelectedUnit(Unit selected)
        {
            if (selected == m_currentSelectedUnit)
                return;

            GameObject go;
            if (VisualManager.Instance.TryGetWidget(m_currentSelectedUnit, out go))
            {
                UniversalWidget widget = go.GetComponent<UniversalWidget>();
                widget.Deselect();
            }

            m_currentSelectedUnit = selected;

            if (VisualManager.Instance.TryGetWidget(m_currentSelectedUnit, out go))
            {
                UniversalWidget widget = go.GetComponent<UniversalWidget>();
                widget.Select();
            }

            if (OnUnitSelected != null)
            {
                OnUnitSelected.Invoke(selected);
            }
        }


        public void Update()
        {
            if (!Enable)
                return;

            if (ActiveTool != null)
            {
                ActiveTool.Update();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (ActiveTool != null)
                    ActiveTool.Cancel();
            }

            if (Viewer.Container != null)
            {
                CreateNodeConnection();
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (currentSelectedUnit == null)
                    return;

                RemoveUnit(currentSelectedUnit);
            }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        public void CreateNodeConnection()
        {
            if (!Viewer.IsPointerInside)
            {
                ShowSnapPoint(false, Vector2.zero);
                if (Input.GetMouseButtonUp(0))
                {
                    if (canCreate)
                    {
                        VisualManager.Instance.DestoryNodeConnectionWidget(createConnection);
                        canCreate = false;
                    }
                }

                return;
            }

            NodeAnchor nodeAnchor;
            Vector2 pointerToContainerPoint = PointerToContainerPoint();
            bool isSanp = currentGraph.SnapNodeAnchor(pointerToContainerPoint, out nodeAnchor);

            if (isSanp)
            {
                pointerToContainerPoint = GraphUtils.GetPosition(nodeAnchor.Node, nodeAnchor.Anchor);
                ShowSnapPoint(true, pointerToContainerPoint);
            }
            else
            {
                ShowSnapPoint(false, Vector2.zero);
            }

            if (Input.GetMouseButtonDown(0))
            {
                draging = true;
                if (!canCreate && isSanp)
                {
                    createConnection = new NodeConnection();
                    createConnection.Source = nodeAnchor.Node;
                    createConnection.SourcePoint = nodeAnchor.Anchor;
                    GameObject go = VisualManager.Instance.CreateNodeConnectionWidget(createConnection);
                    connectionWidget = go.GetComponent<NodeConnectionWidget>();
                    connectionWidget.raycastTarget = false;
                    VisualManager.Instance.SetParentAndAlign(go, Viewer.Container.gameObject);
                    canCreate = true;

                    VisualManager.Instance.CanDragWidget = false;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                draging = false;
                if (canCreate)
                {
                    if (isSanp && !nodeAnchor.Node.Equals(createConnection.Source))
                    {
                        createConnection.Target = nodeAnchor.Node;
                        createConnection.TargetPoint = nodeAnchor.Anchor;
                        connectionWidget.transform.SetAsFirstSibling();
                        connectionWidget.raycastTarget = true;

                        AddUnit(createConnection);
                    }
                    else
                    {
                        VisualManager.Instance.DestoryNodeConnectionWidget(createConnection);
                    }
                }

                canCreate = false;
                VisualManager.Instance.CanDragWidget = true;
            }

            if (canCreate && draging)
            {
                createConnection.TargetPoint = pointerToContainerPoint;
            }
        }

        /// <summary>
        /// 指针在 Container 中的位置
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public Vector2 PointerToContainerPoint()
        {
            Vector2 sp = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(Viewer.Container, sp, Camera, out Vector2 localPoint);
            return localPoint;
        }
    }
}
