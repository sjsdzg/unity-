using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Diagram
{

    /// <summary>
    /// 
    /// </summary>
    public class GraphAStar
    {
        /// <summary>
        /// 节点
        /// </summary>
        public class Point
        {
            /// <summary>
            /// 位置
            /// </summary>
            public Vector2 position;
            /// <summary>
            /// 寻路消耗
            /// </summary>
            public float f;
            /// <summary>
            /// 节点离起点的距离 （节点离父节点的距离 + 父节点离起点的距离）
            /// </summary>
            public float g;
            /// <summary>
            /// 节点离终点的距离
            /// </summary>
            public float h;
            /// <summary>
            /// 拐点数
            /// </summary>
            public float t;
            /// <summary>
            /// 父节点
            /// </summary>
            public Point parent;
        }

        private Node startNode;
        private Node endNode;

        public List<Point> FindPath(NodeConnection connection)
        {
            return null;

        }

        public void FindWalkableNode()
        {

        }

        public Vector2[] GetCornerPoints(Node node, float margin = 20)
        {
            Vector2 offset = node.SizeDelta * 0.5f + new Vector2(20, 20);
            // points
            Vector2[] points = new Vector2[0];
            // 左下
            points[0] = (Vector2)node.Position - offset;
            // 左上
            points[1] = (Vector2)node.Position + new Vector2(-offset.x, offset.y);
            // 右上
            points[2] = (Vector2)node.Position + offset;
            // 右下
            points[2] = (Vector2)node.Position + new Vector2(offset.x, -offset.y);

            return points;
        }
    }
}
