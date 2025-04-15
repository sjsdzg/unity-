using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Diagram
{
    /// <summary>
    /// 图表
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// 添加 Unit 事件
        /// </summary>
        public event Action<Unit> OnUnitAdded;

        /// <summary>
        /// 移除 Unit 事件
        /// </summary>
        public event Action<Unit> OnUnitRemoved;

        private float m_SnapEpsilon = 8;
        /// <summary>
        /// 捕捉值
        /// </summary>
        public float SnapEpsilon
        {
            get { return m_SnapEpsilon; }
            set { m_SnapEpsilon = value; }
        }

        private List<Node> m_Nodes = new List<Node>();
        /// <summary>
        /// 流程节点列表
        /// </summary>
        public List<Node> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }


        private List<NodeConnection> m_Connections = new List<NodeConnection>();
        /// <summary>
        /// 流程节点链接
        /// </summary>
        public List<NodeConnection> Connections
        {
            get { return m_Connections; }
            set { m_Connections = value; }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="unit"></param>
        public void AddUnit(Unit unit)
        {
            if (!VisualManager.Instance.HasWidget(unit))
            {
                GameObject go = VisualManager.Instance.CreateWidget(unit);
                VisualManager.Instance.SetParentAndAlign(go, VisualManager.Instance.Viewer.Container.gameObject);
                if (unit is NodeConnection)
                {
                    go.transform.SetAsFirstSibling();
                }
            }

            if (unit is Node)
            {
                Node node = (Node)unit;
                m_Nodes.Add(node);
            }
            else if (unit is NodeConnection)
            {
                NodeConnection connection = (NodeConnection)unit;
                m_Connections.Add(connection);
            }

            // added
            OnUnitAdded?.Invoke(unit);

            // Undo
            UndoManager.Instance.Push(RemoveUnit, unit, "undo add unit");
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="unit"></param>
        public void RemoveUnit(Unit unit)
        {
            if (unit != null && unit.Equals(GraphMaster.Instance.currentSelectedUnit))
            {
                GraphMaster.Instance.SetSelectedUnit(null);
            }

            if (VisualManager.Instance.HasWidget(unit))
            {
                VisualManager.Instance.DestoryWidget(unit);
            }

            if (unit is Node)
            {
                Node node = (Node)unit;
                
                // 移除关联 NodeConnection
                List<NodeConnection> connections = GetConnectionsWithNode(node);
                foreach (var connection in connections)
                {
                    RemoveUnit(connection);
                }

                m_Nodes.Remove(node);
            }
            else if (unit is NodeConnection)
            {
                NodeConnection connection = (NodeConnection)unit;
                m_Connections.Remove(connection);
            }

            // removed
            OnUnitRemoved?.Invoke(unit);

            // Undo
            UndoManager.Instance.Push(AddUnit, unit, "undo remove unit");
        }

        public void Clear()
        {
            for (int i = m_Connections.Count - 1; i >= 0; i--)
            {
                RemoveUnit(m_Connections[i]);
            }

            for (int i= m_Nodes.Count - 1; i >= 0; i--)
            {
                RemoveUnit(m_Nodes[i]);
            }
        }

        /// <summary>
        /// 获取和节点关联的链接
        /// </summary>
        /// <param name="node"></param>
        public List<NodeConnection> GetConnectionsWithNode(Node node)
        {
            List<NodeConnection> list = new List<NodeConnection>();
            foreach (var connection in m_Connections)
            {
                if (connection.Source.Equals(node))
                {
                    list.Add(connection);
                }

                if (connection.Target.Equals(node))
                {
                    list.Add(connection);
                }
            }

            return list;
        }

        /// <summary>
        /// 捕捉节点 Anchor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="nodeAnchor"></param>
        public bool SnapNodeAnchor(Vector2 position, out NodeAnchor nodeAnchor)
        {
            nodeAnchor = new NodeAnchor();
            int length = m_Nodes.Count;
            bool flag = false;
            float k = m_SnapEpsilon;

            for (int i = 0; i < length; i++)
            {
                Node node = m_Nodes[i];
                if (node.Points == null)
                    continue;

                for (int j = 0; j < node.Points.Count; j++)
                {
                    Vector2 point = node.Points[j];
                    Vector2 pos = GraphUtils.GetPosition(node, point);
                    float distance = Vector2.Distance(position, pos);
                    if (distance < k)
                    {
                        flag = true;
                        nodeAnchor.Node = node;
                        nodeAnchor.Anchor = point;
                    }
                }
            }

            return flag;
        }

        public void SnapAxis(Vector2 position, out Vector2 axis)
        {
            axis = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            int length = m_Nodes.Count;

            float k = m_SnapEpsilon;
            for (int i = 0; i < length; i++)
            {
                Node node = m_Nodes[i];
                float temp = Mathf.Abs(node.Position.x - position.x);
                if (temp < k)
                {
                    k = temp;
                    axis.x = node.Position.x;
                }
            }

            k = m_SnapEpsilon;
            for (int i = 0; i < length; i++)
            {
                Node node = m_Nodes[i];
                float temp = Mathf.Abs(node.Position.y - position.y);
                if (temp < k)
                {
                    k = temp;
                    axis.y = node.Position.y;
                }
            }
        }

        public void SnapAxis(Node target, out Vector2 axis)
        {
            var position = target.Position;
            axis = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            int length = m_Nodes.Count;

            float k = m_SnapEpsilon;
            for (int i = 0; i < length; i++)
            {
                Node node = m_Nodes[i];
                if (node.Equals(target))
                    continue;

                float temp = Mathf.Abs(node.Position.x - position.x);
                if (temp < k)
                {
                    k = temp;
                    axis.x = node.Position.x;
                }
            }

            k = m_SnapEpsilon;
            for (int i = 0; i < length; i++)
            {
                Node node = m_Nodes[i];
                if (node.Equals(target))
                    continue;

                float temp = Mathf.Abs(node.Position.y - position.y);
                if (temp < k)
                {
                    k = temp;
                    axis.y = node.Position.y;
                }
            }
        }
    }
}
