using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Math
{
    public partial class MeshBuilder
    {
        private static readonly int s_SegmentsAmount = 32;
        private static readonly Vector3[] s_CubePointsCache = new Vector3[8];

        /// <summary>
        /// 添加线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="color"></param>
        public void AddLine(Vector3 p0, Vector3 p1, Color color)
        {
            int startIndex = CurrentVertCount;
            AddVert(p0, color);
            AddVert(p1, color);
            AddIndices(startIndex, startIndex + 1);
        }

        /// <summary>
        /// 添加多段线
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="close"></param>
        public void AddPolyline(List<Vertex> verts, bool close = true)
        {
            if (verts == null)
                return;

            int length = verts.Count;
            int startIndex = CurrentVertCount;
            for (int i = 0; i < length - 1; i++)
            {
                AddVert(verts[i]);
                AddIndices(startIndex + i, startIndex + i + 1);
            }

            AddVert(verts[length - 1]);
            if (close && length > 1)
            {
                AddIndices(startIndex + length - 1, startIndex);
            }
        }

        /// <summary>
        /// 添加短划线框
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="pattern"></param>
        /// <param name="color"></param>
        public void AddDashLine(Vector3 p0, Vector3 p1, float[] pattern, Color color)
        {
            if (pattern == null || pattern.Length < 2)
                return;

            int startIndex = CurrentVertCount;
            float dist = Vector3.Distance(p0, p1);
            Vector3 direction = (p1 - p0).normalized;
            float length = 0;
            Vector3 current = p0;
            Vector3 next = p0;
            while (length < dist)
            {
                for (int i = 0; i < pattern.Length; i += 2)
                {
                    startIndex = CurrentVertCount;
                    length += pattern[i];
                    if (length < dist)
                    {
                        AddVert(current, color);
                        next = p0 + direction * length;
                        AddVert(next, color);
                        AddIndices(startIndex, startIndex + 1);
                    }
                    else
                    {
                        AddVert(current, color);
                        AddVert(p1, color);
                        AddIndices(startIndex, startIndex + 1);
                        break;
                    }

                    length += pattern[(i + 1) % pattern.Length];
                    current = p0 + direction * length;
                }
            }
        }

        /// <summary>
        /// 添加一个虚线矩形
        /// </summary>
        /// <param name="alignment"></param>
        /// <param name="size">x : length ; y : width</param>
        /// <param name="color"></param>
        public void AddWireRect(Vector3 position, Quaternion rotation, Vector2 size, Color color)
        {
            Vector3[] positions = new Vector3[4];
            Vector3 sideways = rotation * new Vector3(size.x / 2, 0, 0);
            Vector3 up = rotation * new Vector3(0, 0, size.y / 2);

            positions[0] = position + sideways + up;
            positions[1] = position + sideways - up;
            positions[2] = position - sideways - up;
            positions[3] = position - sideways + up;

            int length = positions.Length;
            int startIndex = CurrentVertCount;
            for (int i = 0; i < positions.Length; i++)
            {
                AddVert(positions[i], color);
                AddIndices(startIndex + i, startIndex + (i + 1) % length);
            }
        }

        /// <summary>
        /// 添加弧线
        /// </summary>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void AddWireArc(Vector3 center, Vector3 normal, float startAngle, float sweepAngle, float radius, Color color)
        {
            int startIndex = CurrentVertCount;

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            float angle = startAngle;
            float delta = sweepAngle / s_SegmentsAmount;
            // point
            Vector3 suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 position = center + radius * suface_normal;
            AddVert(position, color);

            for (int i = 0; i < s_SegmentsAmount; i++)
            {
                angle += delta;
                suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                position = center + radius * suface_normal;
                AddVert(position, color);
                AddIndices(startIndex + i, startIndex + i + 1);
            }
        }

        /// <summary>
        /// 添加线框圆
        /// </summary>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void AddWireCircle(Vector3 center, Vector3 normal, float radius, Color color)
        {
            AddWireArc(center, normal, 0, 2 * Mathf.PI, radius, color);
        }

        /// <summary>
        /// 添加线框立方体
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public void AddWireCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            Vector3 extents = size * 0.5f;
            s_CubePointsCache[0] = position + rotation * new Vector3(-extents.x, -extents.y, -extents.z);
            s_CubePointsCache[1] = position + rotation * new Vector3(-extents.x, -extents.y, extents.z);
            s_CubePointsCache[2] = position + rotation * new Vector3(-extents.x, extents.y, -extents.z);
            s_CubePointsCache[3] = position + rotation * new Vector3(-extents.x, extents.y, extents.z);
            s_CubePointsCache[4] = position + rotation * new Vector3(extents.x, -extents.y, -extents.z);
            s_CubePointsCache[5] = position + rotation * new Vector3(extents.x, -extents.y, extents.z);
            s_CubePointsCache[6] = position + rotation * new Vector3(extents.x, extents.y, -extents.z);
            s_CubePointsCache[7] = position + rotation * new Vector3(extents.x, extents.y, extents.z);

            AddLine(s_CubePointsCache[0], s_CubePointsCache[1], color);
            AddLine(s_CubePointsCache[2], s_CubePointsCache[3], color);
            AddLine(s_CubePointsCache[4], s_CubePointsCache[5], color);
            AddLine(s_CubePointsCache[6], s_CubePointsCache[7], color);

            AddLine(s_CubePointsCache[0], s_CubePointsCache[2], color);
            AddLine(s_CubePointsCache[1], s_CubePointsCache[3], color);
            AddLine(s_CubePointsCache[4], s_CubePointsCache[6], color);
            AddLine(s_CubePointsCache[5], s_CubePointsCache[7], color);

            AddLine(s_CubePointsCache[0], s_CubePointsCache[4], color);
            AddLine(s_CubePointsCache[1], s_CubePointsCache[5], color);
            AddLine(s_CubePointsCache[2], s_CubePointsCache[6], color);
            AddLine(s_CubePointsCache[3], s_CubePointsCache[7], color);
        }

        
    }
}
