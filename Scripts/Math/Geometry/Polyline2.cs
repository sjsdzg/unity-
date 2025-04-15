using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline2 : IEnumerable<Vector2>
    {
        protected List<Vector2> vertices;

        public Polyline2()
        {
            vertices = new List<Vector2>();
        }

        public Polyline2(IEnumerable<Vector2> s)
        {
            vertices = new List<Vector2>(s);
        }

        public int VertexCount
        {
            get { return vertices.Count; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public float Length
        {
            get
            {
                float length = 0;
                int N = vertices.Count;
                for (int i = 0; i < N - 1; i++)
                {
                    length += Vector2.Distance(vertices[i], vertices[i + 1]);
                }
                return length;
            }
        }

        public void AppendVertex(Vector2 v)
        {
            vertices.Add(v);
        }

        public void AppendVertices(IEnumerable<Vector2> s)
        {
            vertices.AddRange(s);
        }

        public List<Segment2> AsSegments()
        {
            List<Segment2> segments = new List<Segment2>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                segments.Add(new Segment2(vertices[i], vertices[i + 1]));
            }
            return segments;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return vertices.GetEnumerator();
        }
    }
}
