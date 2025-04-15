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
    public class VisualManager : Singleton<VisualManager>
    {
        /// <summary>
        /// 字体颜色
        /// </summary>
        public static Color s_TextColor = new Color32(50, 50, 50, 255);
        /// <summary>
        /// 线段颜色
        /// </summary>
        public static Color s_LineColor = new Color32(20, 20, 20, 255);
        /// <summary>
        /// 边框颜色
        /// </summary>
        public static Color s_StrokeColor = new Color32(20, 20, 20, 255);
        /// <summary>
        /// 默认颜色
        /// </summary>
        public static Color s_NormalColor = new Color32(255, 255, 255, 255);

        /// <summary>
        /// 高亮颜色
        /// </summary>
        public static Color s_HighlightedColor = new Color32(19, 111, 210, 255);

        /// <summary>
        /// 选择颜色
        /// </summary>
        public static Color s_SelectedColor = new Color32(19, 111, 210, 255);

        /// <summary>
        /// 可视化对象字典
        /// </summary>
        private Dictionary<string, GameObject> m_VisualIndexSet = new Dictionary<string, GameObject>();

        /// <summary>
        /// 对象池
        /// </summary>
        private Dictionary<string, GameObjectPool> m_Pools = new Dictionary<string, GameObjectPool>();

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

        private bool canDragWidget = true;
        /// <summary>
        /// 是否能拖拽 Widget
        /// </summary>
        public bool CanDragWidget
        {
            get { return canDragWidget; }
            set { canDragWidget = value; }
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public GameObjectPool GetOrAddPool<T>()
        {
            Type type = typeof(T);
            string key = type.ToString();

            if (!m_Pools.TryGetValue(key, out GameObjectPool pool))
            {
                pool = new GameObjectPool(Viewer.Unused);
                m_Pools.Add(key, pool);
            }

            return pool;
        }

        public void AddWidget(GameObject go)
        {
            m_VisualIndexSet.Add(go.name, go);
        }

        public void RemoveWidget(string key)
        {
            m_VisualIndexSet.Remove(key);
        }

        public void UpdateWidgetScale(float scale)
        {
            foreach (var go in m_VisualIndexSet.Values)
            {
                VisualWidget widget = go.GetComponent<VisualWidget>();
                if (widget != null)
                {
                    widget.Scale = scale;
                }
            }
        }

        /// <summary>
        /// 是否有小部件
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool HasWidget(Unit unit)
        {
            return TryGetWidget(unit, out GameObject go);
        }

        public bool TryGetWidget(Unit unit, out GameObject go)
        {
            string key = string.Empty;
            if (unit is Node)
            {
                key = "Node#" + unit.Id;
            }
            else if (unit is NodeConnection)
            {
                key = "NodeConnection#" + unit.Id;
            }

            return TryGetWidget(key, out go);
        }

        public bool TryGetWidget(string key, out GameObject go)
        {
            return m_VisualIndexSet.TryGetValue(key, out go);
        }

        public void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        private void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
            {
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
            }
        }

        public GameObject CreateUIObject(string name, GameObject parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go, parent);
            return go;
        }

        public GameObject CreateWidget(Unit unit)
        {
            if (unit is Node)
            {
                return CreateNodeWidget(unit as Node);
            }
            else if (unit is NodeConnection)
            {
                return CreateNodeConnectionWidget(unit as NodeConnection);
            }

            return null;
        }

        public void DestoryWidget(Unit unit)
        {
            if (unit is Node)
            {
                DestoryNodeWidget(unit as Node);
            }
            else if (unit is NodeConnection)
            {
                DestoryNodeConnectionWidget(unit as NodeConnection);
            }
        }

        public GameObject CreateNodeWidget(Node node)
        {
            GameObjectPool pool = GetOrAddPool<NodeWidget>();
            // create
            GameObject go = pool.Spawn();
            go.name = "Node#" + node.Id;

            NodeWidget widget = go.GetComponent<NodeWidget>();
            if (widget == null)
            {
                widget = go.AddComponent<NodeWidget>();

                GameObject childText = CreateUIObject("Text", go);

                Text text = childText.AddComponent<Text>();
                text.text = node.Name;
                text.alignment = TextAnchor.MiddleCenter;
                text.color = s_TextColor;
                text.font = Viewer.Font;
                text.fontSize = 16;
                text.raycastTarget = false;

                RectTransform textRectTransform = childText.GetComponent<RectTransform>();
                textRectTransform.anchorMin = Vector2.zero;
                textRectTransform.anchorMax = Vector2.one;
                textRectTransform.sizeDelta = Vector2.zero;
            }

            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = node.SizeDelta;
            widget.Text = go.GetComponentInChildren<Text>();
            widget.Node = node;

            // Terminal Points
            foreach (var point in node.Points)
            {
                GameObject pointGameObject = CreatePointWidget(GraphUtils.GetWidgetID());
                PointWidget pointWidget = pointGameObject.GetComponent<PointWidget>();
                

                pointWidget.raycastTarget = false;
                pointWidget.color = Color.white;
                pointWidget.StrokeWidth = 1;
                pointWidget.StrokeColor = new Color32(138, 65, 65, 255);
                pointWidget.SizeDelta = new Vector2(8, 8);

                SetParentAndAlign(pointWidget.gameObject, Viewer.Assist.gameObject);
                pointGameObject.GetComponent<RectTransform>().localPosition = GraphUtils.GetPosition(node, point);
                widget.TerminalPoints.Add(pointWidget);

                pointGameObject.SetActive(false);
            }

            AddWidget(go);

            return go;
        }

        public void DestoryNodeWidget(Node node)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<NodeWidget>();
            // destory
            string key = "Node#" + node.Id;
            if (TryGetWidget(key, out GameObject go))
            {
                NodeWidget widget = go.GetComponent<NodeWidget>();
                foreach (var pointWidget in widget.TerminalPoints)
                {
                    DestoryPointWidget(GraphUtils.GetWidgetID(pointWidget.gameObject));
                }
                widget.TerminalPoints.Clear();
                pool.Despawn(go);
                RemoveWidget(key);

                List<NodeConnection> connections = GraphMaster.GetConnectionsWithNode(node);
                foreach (var connection in connections)
                {
                    DestoryNodeConnectionWidget(connection);
                }
            }
        }

        public GameObject CreateNodeConnectionWidget(NodeConnection connection)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<NodeConnectionWidget>();
            // create
            GameObject go = pool.Spawn();
            go.name = "NodeConnection#" + connection.Id;
            // widget
            NodeConnectionWidget widget = go.GetComponent<NodeConnectionWidget>();
            if (widget == null)
            {
                widget = go.AddComponent<NodeConnectionWidget>();
            }
            widget.color = s_LineColor;
            widget.Connection = connection;

            AddWidget(go);

            return go;
        }

        public void DestoryNodeConnectionWidget(NodeConnection connection)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<NodeConnectionWidget>();
            // destory
            string key = "NodeConnection#" + connection.Id;
            if (TryGetWidget(key, out GameObject go))
            {
                pool.Despawn(go);
                RemoveWidget(key);
            }
        }

        public GameObject CreatePointWidget(string id)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<PointWidget>();
            // create
            GameObject go = pool.Spawn();
            go.name = "PointWidget#" + id;
            PointWidget widget = go.GetComponent<PointWidget>();
            if (widget == null)
            {
                widget = go.AddComponent<PointWidget>();
            }

            widget.GetComponent<RectTransform>().sizeDelta = new Vector2(8, 8);

            AddWidget(go);

            return go;
        }

        public void DestoryPointWidget(string id)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<PointWidget>();
            // destory
            string key = "PointWidget#" + id;
            if (TryGetWidget(key, out GameObject go))
            {
                pool.Despawn(go);
                RemoveWidget(key);
            }
        }

        public GameObject CreateLineWidget(string id)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<LineWidget>();
            // create
            GameObject go = pool.Spawn();
            go.name = "LineWidget#" + id;
            LineWidget widget = go.GetComponent<LineWidget>();

            if (widget == null)
            {
                widget = go.AddComponent<LineWidget>();
            }

            AddWidget(go);

            return go;
        }

        public void DestoryLineWidget(string id)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<PointWidget>();
            // destory
            string key = "LineWidget#" + id;
            if (TryGetWidget(key, out GameObject go))
            {
                pool.Despawn(go);
                RemoveWidget(key);
            }
        }
    }
}
