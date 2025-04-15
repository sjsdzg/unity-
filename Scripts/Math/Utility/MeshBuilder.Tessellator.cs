using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LibTessDotNet;
using XFramework.Core;

namespace XFramework.Math
{
    public static class MeshBuilderExtendTessellator
    {
        /// <summary>
        /// 添加多边形
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="contour"></param>
        /// <param name="normal"></param>
        /// <param name="reverse"></param>
        /// <param name="color"></param>
        public static void AddPolygon(this MeshBuilder builder, IList<Vector3> contour, bool reverse, Vector3 normal,  Color color)
        {
            if (contour == null)
                throw new ArgumentNullException("contour");

            if (contour.Count < 3)
                return;

            int startIndex = builder.CurrentVertCount;

            // tess
            var tessellator = new Tessellator();
            tessellator.AddContour(contour, reverse);
            tessellator.Tessellate(normal: normal);

            // uvs
            Vector2[] uvs = Projection.PlanerProject(tessellator.Positions, null, normal);
            MathUtility.Normalize(uvs);

            // vertices
            for (int i = 0; i < tessellator.VertexCount; i++)
            {
                builder.AddVert(tessellator.Positions[i], color, uvs[i], normal);
            }

            // indices
            var indices = tessellator.Indices;
            for (int i = 0, length = indices.Length; i < length; i++)
            {
                indices[i] += startIndex;
            }

            builder.AddIndices(indices);
        }

        /// <summary>
        /// 添加Tessellator
        /// 注意：添加轮廓再调用
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="tessellator"></param>
        /// <param name="normal"></param>
        /// <param name="color"></param>
        public static void AddTessellator(this MeshBuilder builder, Tessellator tessellator, Vector3 normal, Color color)
        {
            // Tessellate
            tessellator.Tessellate(normal: normal);
            // uvs
            Vector2[] uvs = MathUtility.CalculateUVs(tessellator.Positions, normal);
            // vertices
            int startIndex = builder.CurrentVertCount;
            for (int i = 0, length = tessellator.VertexCount; i < length; i++)
            {
                builder.AddVert(tessellator.Positions[i], color, uvs[i], normal);
            }
            // indices
            var indices = tessellator.Indices;
            for (int i = 0, length = indices.Length; i < length; i++)
            {
                indices[i] += startIndex;
            }
            builder.AddIndices(indices);
        }


    }
}
