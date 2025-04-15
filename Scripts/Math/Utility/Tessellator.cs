using LibTessDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Math
{
    public class Tessellator
    {
        /// <summary>
        /// tess
        /// </summary>
        private readonly Tess tess = new Tess { NoEmptyPolygons = true };

        /// <summary>
        /// if true, will remove empty (zero area) polygons.
        /// </summary>
        public bool NoEmptyPolygons
        {
            get { return tess.NoEmptyPolygons; }
            set { tess.NoEmptyPolygons = value; }
        }

        /// <summary>
        /// Vertices of the tessellated mesh.
        /// </summary>
        public ContourVertex[] Vertices { get { return tess.Vertices; } }


        /// <summary>
        /// Number of vertices in the tessellated mesh.
        /// </summary>
        public int VertexCount { get { return tess.VertexCount; } }

        /// <summary>
        /// Positions of the tessellated mesh.
        /// </summary>
        public Vector3[] Positions
        {
            get
            {
                Vector3[] positions = new Vector3[VertexCount];
                for (int i = 0; i < VertexCount; i++)
                {
                    Vec3 vertex = Vertices[i].Position;
                    positions[i] = new Vector3(vertex.X, vertex.Y, vertex.Z);
                }
                return positions;
            }
        }

        /// <summary>
        /// Indices of the tessellated mesh.
        /// </summary>
        public int[] Indices { get { return tess.Elements; } }

        /// <summary>
        /// Number of indices in the tessellated mesh.
        /// </summary>
        public int IndexCount { get { return tess.ElementCount; } }

        /// <summary>
        /// Adds a closed contour be tessellated.
        /// </summary>
        /// <param name="vertices">Vertices of the contour.</param>
        /// <param name="forceOrientation">Orientation of the contour.</param>
        public void AddContour(IList<Vector3> vertices, bool reverse = false)
        {
            int count = vertices.Count;
            var contour = new ContourVertex[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 vertex = vertices[i];
                contour[i].Position = new Vec3(vertex.x, vertex.y, vertex.z);
            }

            if (reverse)
            {
                Array.Reverse(contour);
            }

            tess.AddContour(contour);
        }

        /// <summary>
        /// Tessellates the input
        /// </summary>
        /// <param name="windingRule">Winding rule (环绕规则) used for tessellation.</param>
        /// <param name="elementType">Tessellation output type.</param>
        /// <param name="polySize">Number of vertices per polygon if output is polygons.</param>
        /// <param name="combineCallback">Interpolator used to determine the data payload of generated vertices</param>
        /// <param name="normal">Normal of the input contours. if set to zero, the normal will be calculated during tessellation.</param>
        public void Tessellate(WindingRule windingRule = WindingRule.EvenOdd, ElementType elementType = ElementType.Polygons, int polySize = 3,
            CombineCallback combineCallback = null, Vector3 normal = new Vector3())
        {
            tess.Tessellate(windingRule, elementType, polySize, combineCallback, new Vec3(normal.x, normal.y, normal.z));
        }
    }
}
