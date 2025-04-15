using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public static class MeshBuilderExtendAssist
    {
        /// <summary>
        /// 添加十字形状
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="normal"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="thickness"></param>
        /// <param name="color"></param>
        public static void AddCrossShape(this MeshBuilder builder, Vector3 position, Vector3 normal, float size, float thickness, Color color)
        {
            Vector3[] positions = new Vector3[12];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            float halfThickness = thickness * 0.5f;
            float halfSize = size * 0.5f;
            
            positions[0] = position + rotation * (new Vector3(-halfThickness, 0, -halfThickness));
            positions[1] = position + rotation * (new Vector3(-halfThickness, 0, halfThickness));
            positions[2] = position + rotation * (new Vector3(halfThickness, 0, halfThickness));
            positions[3] = position + rotation * (new Vector3(halfThickness, 0, -halfThickness));

            positions[4] = position + rotation * (new Vector3(-halfSize, 0, -halfThickness));
            positions[5] = position + rotation * (new Vector3(-halfSize, 0, halfThickness));
            positions[6] = position + rotation * (new Vector3(-halfThickness, 0, halfSize));
            positions[7] = position + rotation * (new Vector3(halfThickness, 0, halfSize));

            positions[8] = position + rotation * (new Vector3(halfSize, 0, halfThickness));
            positions[9] = position + rotation * (new Vector3(halfSize, 0, -halfThickness));
            positions[10] = position + rotation * (new Vector3(halfThickness, 0, -halfSize));
            positions[11] = position + rotation * (new Vector3(-halfThickness, 0, -halfSize));
            // uvs
            Vector2[] uv0s = MathUtility.CalculateUVs(positions, normal);
            //startIndex
            int startIndex = builder.CurrentVertCount;
            // vertices
            builder.AddVert(positions[0], color, uv0s[0], normal);
            builder.AddVert(positions[1], color, uv0s[1], normal);
            builder.AddVert(positions[2], color, uv0s[2], normal);
            builder.AddVert(positions[3], color, uv0s[3], normal);
            builder.AddVert(positions[4], color, uv0s[4], normal);
            builder.AddVert(positions[5], color, uv0s[5], normal);
            builder.AddVert(positions[6], color, uv0s[6], normal);
            builder.AddVert(positions[7], color, uv0s[7], normal);
            builder.AddVert(positions[8], color, uv0s[8], normal);
            builder.AddVert(positions[9], color, uv0s[9], normal);
            builder.AddVert(positions[10], color, uv0s[10], normal);
            builder.AddVert(positions[11], color, uv0s[11], normal);
            // indices
            builder.AddIndices(startIndex, startIndex + 1, startIndex + 2);
            builder.AddIndices(startIndex + 2, startIndex + 3, startIndex);
            builder.AddIndices(startIndex, startIndex + 4, startIndex + 5);
            builder.AddIndices(startIndex + 5, startIndex + 1, startIndex);
            builder.AddIndices(startIndex + 1, startIndex + 6, startIndex + 7);
            builder.AddIndices(startIndex + 7, startIndex + 2, startIndex + 1);
            builder.AddIndices(startIndex + 2, startIndex + 8, startIndex + 9);
            builder.AddIndices(startIndex + 9, startIndex + 3, startIndex + 2);
            builder.AddIndices(startIndex + 3, startIndex + 10, startIndex + 11);
            builder.AddIndices(startIndex + 11, startIndex, startIndex + 3);
        }

        public static void AddRectWithHole(this MeshBuilder builder, Vector3 position, Quaternion rotation, Vector2 size, float thickness, Color color)
        {
            Vector3[] positions = new Vector3[8];
            Vector3 sideways = rotation * new Vector3(size.x / 2, 0, 0);
            Vector3 up = rotation * new Vector3(0, 0, size.y / 2);
            Vector3 normal = Vector3.Cross(up, sideways);

            positions[0] = position - sideways - up;
            positions[1] = position - sideways + up;
            positions[2] = position + sideways + up;
            positions[3] = position + sideways - up;

            sideways = rotation * new Vector3(size.x / 2 - thickness, 0, 0);
            up = rotation * new Vector3(0, 0, size.y / 2 - thickness);
            positions[4] = position - sideways - up;
            positions[5] = position - sideways + up;
            positions[6] = position + sideways + up;
            positions[7] = position + sideways - up;

            float u = thickness / size.x;
            float v = thickness / size.y;

            // outline
            int startIndex = builder.CurrentVertCount;
            // vertices
            builder.AddVert(positions[0], color, new Vector2(0, 0), normal);
            builder.AddVert(positions[1], color, new Vector2(0, 1), normal);
            builder.AddVert(positions[2], color, new Vector2(1, 1), normal);
            builder.AddVert(positions[3], color, new Vector2(1, 0), normal);
            builder.AddVert(positions[4], color, new Vector2(u, v), normal);
            builder.AddVert(positions[5], color, new Vector2(u, 1 - v), normal);
            builder.AddVert(positions[6], color, new Vector2(1 - u, 1 - v), normal);
            builder.AddVert(positions[7], color, new Vector2(1 - u, v), normal);
            // indices
            builder.AddIndices(startIndex, startIndex + 1, startIndex + 5);
            builder.AddIndices(startIndex + 5, startIndex + 4, startIndex);
            builder.AddIndices(startIndex + 1, startIndex + 2, startIndex + 6);
            builder.AddIndices(startIndex + 6, startIndex + 5, startIndex + 1);
            builder.AddIndices(startIndex + 2, startIndex + 3, startIndex + 7);
            builder.AddIndices(startIndex + 7, startIndex + 6, startIndex + 2);
            builder.AddIndices(startIndex + 3, startIndex, startIndex + 4);
            builder.AddIndices(startIndex + 4, startIndex + 7, startIndex + 3);
        }
    }
}
