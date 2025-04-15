using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace XFramework.Math
{
    public partial class MeshBuilder
    {
        private static readonly Vector3[] s_QuadPoints = new Vector3[4];
        private static readonly Vertex[] s_QuadVertices = new Vertex[4];

        /// <summary>
        /// 添加实体矩形
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public void AddRect(Vector3 position, Quaternion rotation, Vector2 size, Color color)
        {
            Vector3[] positions = new Vector3[4];
            Vector3 sideways = rotation * new Vector3(size.x / 2, 0, 0);
            Vector3 up = rotation * new Vector3(0, 0, size.y / 2);
            Vector3 normal = Vector3.Cross(up, sideways);

            positions[0] = position - sideways - up;
            positions[1] = position - sideways + up;
            positions[2] = position + sideways + up;
            positions[3] = position + sideways - up;

            int startIndex = CurrentVertCount;

            AddVert(positions[0], color, new Vector2(0, 0), normal);
            AddVert(positions[1], color, new Vector2(0, 1), normal);
            AddVert(positions[2], color, new Vector2(1, 1), normal);
            AddVert(positions[3], color, new Vector2(1, 0), normal);

            AddIndices(startIndex, startIndex + 1, startIndex + 2);
            AddIndices(startIndex + 2, startIndex + 3, startIndex);
        }

        public void AddRectWithOutline(Vector3 position, Quaternion rotation, Vector2 size, float thickness, Color faceColor, Color outlineColor)
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
            int startIndex = CurrentVertCount;
            // vertices
            AddVert(positions[0], outlineColor, new Vector2(0, 0), normal);
            AddVert(positions[1], outlineColor, new Vector2(0, 1), normal);
            AddVert(positions[2], outlineColor, new Vector2(1, 1), normal);
            AddVert(positions[3], outlineColor, new Vector2(1, 0), normal);
            AddVert(positions[4], outlineColor, new Vector2(u, v), normal);
            AddVert(positions[5], outlineColor, new Vector2(u, 1 - v), normal);
            AddVert(positions[6], outlineColor, new Vector2(1 - u, 1 - v), normal);
            AddVert(positions[7], outlineColor, new Vector2(1 - u, v), normal);
            // indices
            AddIndices(startIndex, startIndex + 1, startIndex + 5);
            AddIndices(startIndex + 5, startIndex + 4, startIndex);
            AddIndices(startIndex + 1, startIndex + 2, startIndex + 6);
            AddIndices(startIndex + 6, startIndex + 5, startIndex + 1);
            AddIndices(startIndex + 2, startIndex + 3, startIndex + 7);
            AddIndices(startIndex + 7, startIndex + 6, startIndex + 2);
            AddIndices(startIndex + 3, startIndex, startIndex + 4);
            AddIndices(startIndex + 4, startIndex + 7, startIndex + 3);
            // face
            startIndex = CurrentVertCount;
            // vertices
            AddVert(positions[4], faceColor, new Vector2(0, 0), normal);
            AddVert(positions[5], faceColor, new Vector2(0, 1), normal);
            AddVert(positions[6], faceColor, new Vector2(1, 1), normal);
            AddVert(positions[7], faceColor, new Vector2(1, 0), normal);
            // indices
            AddIndices(startIndex, startIndex + 1, startIndex + 2);
            AddIndices(startIndex + 2, startIndex + 3, startIndex);

        }

        /// <summary>
        /// 添加实体圆弧
        /// </summary>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void AddArc(Vector3 center, Vector3 normal, float startAngle, float sweepAngle, float radius, Color color)
        {
            int startIndex = CurrentVertCount;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
            sweepAngle = Mathf.Clamp(sweepAngle, -Mathf.PI * 2, Mathf.PI * 2);
            bool disc = Mathf.Abs(sweepAngle) == Mathf.PI * 2;
            float delta = sweepAngle / s_SegmentsAmount;

            // center
            Vector3 position = center;
            Vector2 uv0 = new Vector2(0.5f, 0.5f);
            AddVert(position, color, uv0, normal);

            // 1
            float angle = startAngle;
            Vector3 suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            position = center + radius * suface_normal;
            uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
            AddVert(position, color, uv0, normal);

            for (int i = 0; i < s_SegmentsAmount; i++)
            {
                angle += delta;
                suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                position = center + radius * suface_normal;
                uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
                AddVert(position, color, uv0, normal);
                AddIndices(startIndex, startIndex + i + 2, startIndex + i + 1);
            }
        }

        /// <summary>
        /// 添加实体圆
        /// </summary>
        /// <param name="center"></param>
        /// <param name="normal"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void AddCircle(Vector3 center, Vector3 normal, float radius, Color color)
        {
            AddArc(center, normal, 0, Mathf.PI * 2, radius, color);
        }

        /// <summary>
        /// 实体立方体
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public void AddCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            Vector3 extents = size * 0.5f;
            // quad vertices

            // color
            s_QuadVertices[0].color = color;
            s_QuadVertices[1].color = color;
            s_QuadVertices[2].color = color;
            s_QuadVertices[3].color = color;
            // uv0
            s_QuadVertices[0].uv0 = new Vector2(0, 0);
            s_QuadVertices[1].uv0 = new Vector2(0, 1);
            s_QuadVertices[2].uv0 = new Vector2(1, 1);
            s_QuadVertices[3].uv0 = new Vector2(1, 0);
            // 前：
            // 1 x 2
            // 0 x 3
            s_CubePointsCache[0] = position + rotation * new Vector3(-extents.x, -extents.y, -extents.z);
            s_CubePointsCache[1] = position + rotation * new Vector3(-extents.x, extents.y, -extents.z); 
            s_CubePointsCache[2] = position + rotation * new Vector3(extents.x, extents.y, -extents.z);
            s_CubePointsCache[3] = position + rotation * new Vector3(extents.x, -extents.y, -extents.z);
            // position
            s_QuadVertices[0].position = s_CubePointsCache[0];
            s_QuadVertices[1].position = s_CubePointsCache[1];
            s_QuadVertices[2].position = s_CubePointsCache[2];
            s_QuadVertices[3].position = s_CubePointsCache[3];
            // normal
            s_QuadVertices[0].normal = Vector3.back;
            s_QuadVertices[1].normal = Vector3.back;
            s_QuadVertices[2].normal = Vector3.back;
            s_QuadVertices[3].normal = Vector3.back;
            AddVertexQuad(s_QuadVertices);

            // 后：
            // 5 x 6
            // 4 x 7
            s_CubePointsCache[4] = position + rotation * new Vector3(extents.x, -extents.y, extents.z);
            s_CubePointsCache[5] = position + rotation * new Vector3(extents.x, extents.y, extents.z); 
            s_CubePointsCache[6] = position + rotation * new Vector3(-extents.x, extents.y, extents.z);
            s_CubePointsCache[7] = position + rotation * new Vector3(-extents.x, -extents.y, extents.z);
            // position
            s_QuadVertices[0].position = s_CubePointsCache[4];
            s_QuadVertices[1].position = s_CubePointsCache[5];
            s_QuadVertices[2].position = s_CubePointsCache[6];
            s_QuadVertices[3].position = s_CubePointsCache[7];
            // normal
            s_QuadVertices[0].normal = Vector3.forward;
            s_QuadVertices[1].normal = Vector3.forward;
            s_QuadVertices[2].normal = Vector3.forward;
            s_QuadVertices[3].normal = Vector3.forward;
            AddVertexQuad(s_QuadVertices);

            // 左：
            // 9 x 10
            // 8 x 11
            // position
            s_QuadVertices[0].position = s_CubePointsCache[7];
            s_QuadVertices[1].position = s_CubePointsCache[6];
            s_QuadVertices[2].position = s_CubePointsCache[1];
            s_QuadVertices[3].position = s_CubePointsCache[0];
            // normal
            s_QuadVertices[0].normal = Vector3.left;
            s_QuadVertices[1].normal = Vector3.left;
            s_QuadVertices[2].normal = Vector3.left;
            s_QuadVertices[3].normal = Vector3.left;
            AddVertexQuad(s_QuadVertices);
            // 右：
            // 13 x 14
            // 12 x 15
            // position
            s_QuadVertices[0].position = s_CubePointsCache[3];
            s_QuadVertices[1].position = s_CubePointsCache[2];
            s_QuadVertices[2].position = s_CubePointsCache[5];
            s_QuadVertices[3].position = s_CubePointsCache[4];
            // normal
            s_QuadVertices[0].normal = Vector3.right;
            s_QuadVertices[1].normal = Vector3.right;
            s_QuadVertices[2].normal = Vector3.right;
            s_QuadVertices[3].normal = Vector3.right;
            AddVertexQuad(s_QuadVertices);

            // 上：
            // 17 x 18
            // 16 x 19
            // position
            s_QuadVertices[0].position = s_CubePointsCache[1];
            s_QuadVertices[1].position = s_CubePointsCache[6];
            s_QuadVertices[2].position = s_CubePointsCache[5];
            s_QuadVertices[3].position = s_CubePointsCache[2];
            // normal
            s_QuadVertices[0].normal = Vector3.up;
            s_QuadVertices[1].normal = Vector3.up;
            s_QuadVertices[2].normal = Vector3.up;
            s_QuadVertices[3].normal = Vector3.up;
            AddVertexQuad(s_QuadVertices);
            // 下：
            // 22 x 23
            // 20 x 21
            // position
            s_QuadVertices[0].position = s_CubePointsCache[7];
            s_QuadVertices[1].position = s_CubePointsCache[0];
            s_QuadVertices[2].position = s_CubePointsCache[3];
            s_QuadVertices[3].position = s_CubePointsCache[4];
            // normal
            s_QuadVertices[0].normal = Vector3.down;
            s_QuadVertices[1].normal = Vector3.down;
            s_QuadVertices[2].normal = Vector3.down;
            s_QuadVertices[3].normal = Vector3.down;
            AddVertexQuad(s_QuadVertices);
        }
    
        /// <summary>
        /// 实体圆柱
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void AddCylinder(Vector3 position, Quaternion rotation, float radius, float height, Color color)
        {
            Vector3 normal = rotation * Vector3.up;
            // lower Disc
            Vector3 lowerCenter = position - normal * height * 0.5f;
            AddCircle(lowerCenter, -normal, radius, color);
            // upper Disc
            Vector3 upperCenter = position + normal * height * 0.5f;
            AddCircle(upperCenter, normal, radius, color);

            // Cylinder surface
            int startIndex = CurrentVertCount;
            float angle = 0;
            float delta = 2 * Mathf.PI / s_SegmentsAmount;
            Vector3 suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            // lower
            position = lowerCenter + radius * suface_normal;
            Vector2 uv0 = new Vector2(0, 0);
            AddVert(position, color, uv0, suface_normal);
            // upper
            position = upperCenter + radius * suface_normal;
            uv0 = new Vector2(0, 1);
            AddVert(position, color, uv0, suface_normal);

            for (int i = 0; i < s_SegmentsAmount; i++)
            {
                angle += delta;
                suface_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                // lower
                position = lowerCenter + radius * suface_normal;
                uv0 = new Vector2((i + 1) / s_SegmentsAmount, 0);
                AddVert(position, color, uv0, suface_normal);
                // upper
                position = upperCenter + radius * suface_normal;
                uv0 = new Vector2((i + 1) / s_SegmentsAmount, 1);
                AddVert(position, color, uv0, suface_normal);
                // triangles
                AddIndices(startIndex + i * 2, startIndex + i * 2 + 1, startIndex + i * 2 + 2);
                AddIndices(startIndex + i * 2 + 2, startIndex + i * 2 + 1, startIndex + i * 2 + 3);
            }

        }

        /// <summary>
        /// 实体圆锥
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void AddCone(Vector3 position, Quaternion rotation, float radius, float height, Color color)
        {
            Vector3 normal = rotation * Vector3.up;
            // lower Disc
            Vector3 lowerCenter = position - normal * height * 0.5f;
            AddCircle(lowerCenter, -normal, radius, color);
            // upper point
            Vector3 upperPoint = position + normal * height * 0.5f;
            // Cone
            int startIndex = CurrentVertCount;
            float angle = 0;
            float delta = 2 * Mathf.PI / s_SegmentsAmount;
            Vector3 disc_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            // lower Disc
            position = lowerCenter + radius * disc_normal;
            // suface_normal
            Vector3 direction = (upperPoint - position).normalized;
            Vector3 closestPoint = position + direction * Vector3.Dot(lowerCenter - position, direction);
            Vector3 suface_normal = closestPoint - lowerCenter;
            // uv
            Vector3 uv0 = new Vector2(0, 0);
            float uv_x = radius * Mathf.Sin(delta * 0.5f) * 2;
            float uv_y = Mathf.Sqrt(radius * Mathf.Cos(delta * 0.5f) * radius * Mathf.Cos(delta * 0.5f) + height * height);
            if (uv_x <= uv_y)
            {
                uv_x = uv_x / uv_y;
                uv_y = 1;
            }
            else
            {
                uv_x = 1;
                uv_y = uv_y / uv_x;
            }

            for (int i = 0; i < s_SegmentsAmount; i++)
            {
                // previous
                uv0 = new Vector2(0, 0);
                AddVert(position, color, uv0, suface_normal);
                // upper
                uv0 = new Vector2(uv_x * 0.5f, uv_y);
                AddVert(upperPoint, color, uv0, suface_normal);
                // next
                angle += delta;
                disc_normal = rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                // lower Disc
                position = lowerCenter + radius * disc_normal;
                // suface_normal
                direction = (upperPoint - position).normalized;
                closestPoint = position + direction * Vector3.Dot(lowerCenter - position, direction);
                suface_normal = closestPoint - lowerCenter;
                uv0 = new Vector2(uv_x, 0);
                AddVert(position, color, uv0, suface_normal);
                AddIndices(startIndex + i * 3, startIndex + i * 3 + 1, startIndex + i * 3 + 2);
            }
        }

        /// <summary>
        /// 实体球
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void AddSphere(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            int startIndex = CurrentVertCount;

            int verticalSegmentsAmount = s_SegmentsAmount / 2;
            int horizontalSegmentsAmount = s_SegmentsAmount;

            float verticalDelta = Mathf.PI / verticalSegmentsAmount;
            float horizontalDelta = Mathf.PI * 2 / horizontalSegmentsAmount;

            float horizontalAngle = 0;
            float verticalAngle = -Mathf.PI / 2;
            Vector3 point = Vector3.zero;
            Vector2 uv0 = Vector2.zero;
            Vector3 normal = Vector3.zero;
            // vertices
            for (int v = 0; v <= verticalSegmentsAmount; v++)
            {
                horizontalAngle = 0f;
                for (int h = 0; h <= horizontalSegmentsAmount; h++)
                {
                    float x = radius * Mathf.Cos(verticalAngle) * Mathf.Cos(horizontalAngle);
                    float y = radius * Mathf.Sin(verticalAngle);
                    float z = radius * Mathf.Cos(verticalAngle) * Mathf.Sin(horizontalAngle);
                    point = position + rotation * new Vector3(x, y, z);
                    uv0 = new Vector2(((float)h) / horizontalSegmentsAmount, ((float)v) / verticalSegmentsAmount);
                    normal = rotation * new Vector3(x, y, z);
                    AddVert(point, color, uv0, normal);
                    horizontalAngle += horizontalDelta;
                }
                verticalAngle += verticalDelta;
            }
            // triangles
            for (int v = 0; v < verticalSegmentsAmount; v++)
            {
                for (int h = 0; h < horizontalSegmentsAmount; h++)
                {
                    int lowerIndex = v * (horizontalSegmentsAmount + 1) + h;
                    int upperIndex = (v + 1) * (horizontalSegmentsAmount + 1) + h;
                    AddIndices(startIndex + lowerIndex, startIndex + upperIndex, startIndex + lowerIndex + 1);
                    AddIndices(startIndex + lowerIndex + 1, startIndex + upperIndex, startIndex + upperIndex + 1);
                }
            }
        }
    
        public void AddInternalCube(Vector3 position, Quaternion rotation, Vector3 size, Color color)
        {
            Vector3 extents = size * 0.5f;
            Vector3 normal = Vector3.right;
            // quad vertices

            // color
            s_QuadVertices[0].color = color;
            s_QuadVertices[1].color = color;
            s_QuadVertices[2].color = color;
            s_QuadVertices[3].color = color;
            // uv0
            s_QuadVertices[0].uv0 = new Vector2(0, 0);
            s_QuadVertices[1].uv0 = new Vector2(0, 1);
            s_QuadVertices[2].uv0 = new Vector2(1, 1);
            s_QuadVertices[3].uv0 = new Vector2(1, 0);
            // 前：
            // 1 x 2
            // 0 x 3
            s_CubePointsCache[0] = position + rotation * new Vector3(-extents.x, -extents.y, -extents.z);
            s_CubePointsCache[1] = position + rotation * new Vector3(-extents.x, extents.y, -extents.z);
            s_CubePointsCache[2] = position + rotation * new Vector3(extents.x, extents.y, -extents.z);
            s_CubePointsCache[3] = position + rotation * new Vector3(extents.x, -extents.y, -extents.z);
            // position
            s_QuadVertices[0].position = s_CubePointsCache[0];
            s_QuadVertices[1].position = s_CubePointsCache[3];
            s_QuadVertices[2].position = s_CubePointsCache[2];
            s_QuadVertices[3].position = s_CubePointsCache[1];
            // normal
            normal = rotation * Vector3.forward;
            s_QuadVertices[0].normal = normal;
            s_QuadVertices[1].normal = normal;
            s_QuadVertices[2].normal = normal;
            s_QuadVertices[3].normal = normal;
            AddVertexQuad(s_QuadVertices);

            // 后：
            // 5 x 6
            // 4 x 7
            s_CubePointsCache[4] = position + rotation * new Vector3(extents.x, -extents.y, extents.z);
            s_CubePointsCache[5] = position + rotation * new Vector3(extents.x, extents.y, extents.z);
            s_CubePointsCache[6] = position + rotation * new Vector3(-extents.x, extents.y, extents.z);
            s_CubePointsCache[7] = position + rotation * new Vector3(-extents.x, -extents.y, extents.z);
            // position
            s_QuadVertices[0].position = s_CubePointsCache[4];
            s_QuadVertices[1].position = s_CubePointsCache[7];
            s_QuadVertices[2].position = s_CubePointsCache[6];
            s_QuadVertices[3].position = s_CubePointsCache[5];
            // normal
            normal = rotation * Vector3.back;
            s_QuadVertices[0].normal = normal;
            s_QuadVertices[1].normal = normal;
            s_QuadVertices[2].normal = normal;
            s_QuadVertices[3].normal = normal;
            AddVertexQuad(s_QuadVertices);

            // 左：
            // 9 x 10
            // 8 x 11
            // position
            s_QuadVertices[0].position = s_CubePointsCache[7];
            s_QuadVertices[1].position = s_CubePointsCache[0];
            s_QuadVertices[2].position = s_CubePointsCache[1];
            s_QuadVertices[3].position = s_CubePointsCache[6];
            // normal
            normal = rotation * Vector3.right;
            s_QuadVertices[0].normal = normal;
            s_QuadVertices[1].normal = normal;
            s_QuadVertices[2].normal = normal;
            s_QuadVertices[3].normal = normal;
            AddVertexQuad(s_QuadVertices);
            // 右：
            // 13 x 14
            // 12 x 15
            // position
            s_QuadVertices[0].position = s_CubePointsCache[3];
            s_QuadVertices[1].position = s_CubePointsCache[5];
            s_QuadVertices[2].position = s_CubePointsCache[4];
            s_QuadVertices[3].position = s_CubePointsCache[2];
            // normal
            normal = rotation * Vector3.left;
            s_QuadVertices[0].normal = normal;
            s_QuadVertices[1].normal = normal;
            s_QuadVertices[2].normal = normal;
            s_QuadVertices[3].normal = normal;
            AddVertexQuad(s_QuadVertices);
        }

        public void AddQuad(IList<Vector3> points, Color color)
        {
            // s_QuadPoints
            s_QuadPoints[0] = points[0];
            s_QuadPoints[1] = points[1];
            s_QuadPoints[2] = points[2];
            s_QuadPoints[3] = points[3];
            // position
            s_QuadVertices[0].position = s_QuadPoints[0];
            s_QuadVertices[1].position = s_QuadPoints[1];
            s_QuadVertices[2].position = s_QuadPoints[2];
            s_QuadVertices[3].position = s_QuadPoints[3];

            // normal
            Vector3 surface_normal = MathUtility.Normal(s_QuadPoints[0], s_QuadPoints[1], s_QuadPoints[2]);
            s_QuadVertices[0].normal = surface_normal;
            s_QuadVertices[1].normal = surface_normal;
            s_QuadVertices[2].normal = surface_normal;
            s_QuadVertices[3].normal = surface_normal;

            // uv0
            Vector2[] uv0s = MathUtility.CalculateUVs(s_QuadPoints, surface_normal);
            s_QuadVertices[0].uv0 = uv0s[0];
            s_QuadVertices[1].uv0 = uv0s[1];
            s_QuadVertices[2].uv0 = uv0s[2];
            s_QuadVertices[3].uv0 = uv0s[3];

            // color
            s_QuadVertices[0].color = color;
            s_QuadVertices[1].color = color;
            s_QuadVertices[2].color = color;
            s_QuadVertices[3].color = color;
            // 
            AddVertexQuad(s_QuadVertices);
        }

        /// <summary>
        /// 多段面
        /// 0   1
        /// 0   1
        /// </summary>
        /// <param name="points"></param>
        /// <param name="normal"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void AddPolyFace(IList<Vector3> points, Vector3 normal, float height, Color color)
        {
            int length = points.Count;
            for (int i = 0; i < length; i++)
            {
                s_QuadPoints[0] = points[i];
                s_QuadPoints[1] = points[(i + 1) % length];
                s_QuadPoints[2] = points[(i + 1) % length] + height * normal;
                s_QuadPoints[3] = points[i] + height * normal;

                AddQuad(s_QuadPoints, color);
            }
        }
    }
}
