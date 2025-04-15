using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public static class VertexHelperUtils
    {
        public static readonly float Epsilon = 0.000001f;

        public static readonly Vector2[] quadUvs = new Vector2[4] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };

        /// <summary>
        /// 分割数量
        /// </summary>
        public static float s_SegmentAmount = 30;

        public static bool IntersectLines(Vector2 line1Pt1, Vector2 line1Pt2, Vector2 line2Pt1, Vector2 line2Pt2, out Vector2 point)
        {
            var a1 = line1Pt2.y - line1Pt1.y;
            var b1 = line1Pt1.x - line1Pt2.x;

            var a2 = line2Pt2.y - line2Pt1.y;
            var b2 = line2Pt1.x - line2Pt2.x;

            var det = a1 * b2 - a2 * b1;
            if (Mathf.Abs(det) <= Epsilon) // Parallel, no intersection
            {
                point = Vector2.positiveInfinity;
                return false;
            }

            var c1 = a1 * line1Pt1.x + b1 * line1Pt1.y;
            var c2 = a2 * line2Pt1.x + b2 * line2Pt1.y;
            var detInv = 1.0f / det;
            point = new Vector2((b2 * c1 - b1 * c2) * detInv, (a1 * c2 - a2 * c1) * detInv);
            return true;
        }

        public static void AddGrid(this VertexHelper vh, Rect rect, float cellSize, float lineWidth, Color gridColor)
        {
            float halfWidth = rect.width * 0.5f;
            float halfHeight = rect.height * 0.5f;
            int halfHorizontalCount = Mathf.FloorToInt(halfHeight / cellSize);
            int halfVerticalCount = Mathf.FloorToInt(halfWidth / cellSize);

            // x 轴
            Vector3 p1 = new Vector3(-halfWidth, 0, 0);
            Vector3 p2 = new Vector3(halfWidth, 0, 0);
            vh.AddLine(p1, p2, lineWidth, gridColor);

            // y 轴
            p1 = new Vector3(0, -halfHeight, 0);
            p2 = new Vector3(0, halfHeight, 0);
            vh.AddLine(p1, p2, lineWidth, gridColor);

            Color color = gridColor;
            for (int i = 1; i <= halfHorizontalCount; i++)
            {
                if (i % 4 == 0)
                {
                    color = gridColor;
                }
                else
                {
                    color = gridColor;
                    color.a *= 0.5f;
                }

                // x 轴 - 上
                p1 = new Vector3(-halfWidth, cellSize * i, 0);
                p2 = new Vector3(halfWidth, cellSize * i, 0);
                vh.AddLine(p1, p2, lineWidth, color);

                // x 轴 - 下
                p1 = new Vector3(-halfWidth, -cellSize * i, 0);
                p2 = new Vector3(halfWidth, -cellSize * i, 0);
                vh.AddLine(p1, p2, lineWidth, color);
            }

            for (int i = 1; i <= halfVerticalCount; i++)
            {
                if (i % 4 == 0)
                {
                    color = gridColor;
                }
                else
                {
                    color = gridColor;
                    color.a *= 0.5f;
                }

                // x 轴 - 左
                p1 = new Vector3(-cellSize * i, -halfHeight, 0);
                p2 = new Vector3(-cellSize * i, halfHeight, 0);
                vh.AddLine(p1, p2, lineWidth, color);

                // x 轴 - 右
                p1 = new Vector3(cellSize * i, -halfHeight, 0);
                p2 = new Vector3(cellSize * i, halfHeight, 0);
                vh.AddLine(p1, p2, lineWidth, color);
            }
        }

        public static void AddLine(this VertexHelper vh, Vector3 p0, Vector3 p1, float thickness, Color color)
        {
            Vector3 direction = p1 - p0;
            Vector3 normal = Vector3.Cross(direction, Vector3.back);
            normal = normal.normalized;
            float half = thickness * 0.5f;

            UIVertex[] vertices = new UIVertex[4];
            vertices[0].position = p0 - normal * half;
            vertices[0].uv0 = new Vector2(0, 0);
            vertices[0].color = color;

            vertices[1].position = p0 + normal * half;
            vertices[1].uv0 = new Vector2(0, 1);
            vertices[1].color = color;

            vertices[2].position = p1 + normal * half;
            vertices[2].uv0 = new Vector2(1, 1);
            vertices[2].color = color;

            vertices[3].position = p1 - normal * half;
            vertices[3].uv0 = new Vector2(1, 0);
            vertices[3].color = color;

            vh.AddUIVertexQuad(vertices);
        }

        public static void AddLine(this VertexHelper vh, Vector2[] points, float thickness, Color color)
        {
            if (points == null || points.Length < 2)
                return;

            var segments = new List<UIVertex[]>();
            for (int i = 0; i < points.Length - 1; i++)
            {
                var p0 = points[i];
                var p1 = points[i + 1];

                segments.Add(CreateUIVertexQuad(p0, p1, thickness, color));
            }

            for (int i = 0; i < segments.Count; i++)
            {
                if (i < segments.Count - 1)
                {
                    var vec1 = segments[i][1].position - segments[i][2].position;
                    var vec2 = segments[i + 1][2].position - segments[i + 1][1].position;
                    var angle = Vector2.Angle(vec1, vec2) * Mathf.Deg2Rad;

                    var sign = Mathf.Sign(Vector3.Cross(vec1.normalized, vec2.normalized).z);

                    // Calculate the miter point
                    var miterDistance = thickness / (2 * Mathf.Tan(angle / 2));
                    var miterPointA = segments[i][2].position - vec1.normalized * miterDistance * sign;
                    var miterPointB = segments[i][3].position + vec1.normalized * miterDistance * sign;

                    if (angle > 15 * Mathf.Deg2Rad)
                    {
                        segments[i][2].position = miterPointA;
                        segments[i][3].position = miterPointB;
                        segments[i + 1][0].position = miterPointB;
                        segments[i + 1][1].position = miterPointA;
                    }
                    else
                    {
                        var join = new UIVertex[] { segments[i][2], segments[i][3], segments[i + 1][0], segments[i + 1][1] };
                        vh.AddUIVertexQuad(join);
                    }
                }

                vh.AddUIVertexQuad(segments[i]);
            }           
        }

        public static void AddLineArrow(this VertexHelper vh, Vector2[] points, float lineThickness, Vector2 arrowSizeDelta, Color color)
        {
            if (points == null || points.Length < 2)
                return;

            int length = points.Length;
            Vector2 lastPoint = points[length - 1];
            Vector2 secondLastPoint = points[length - 2];
            Vector2 vec = lastPoint - secondLastPoint;
            Vector2 offset = new Vector2(secondLastPoint.y - lastPoint.y, lastPoint.x - secondLastPoint.x).normalized * arrowSizeDelta.y * 0.5f;

            Vector2[] copyPoints = new Vector2[length];
            points.CopyTo(copyPoints, 0);
            copyPoints[length - 1] = lastPoint - vec.normalized * arrowSizeDelta.x;

            //if (vec.magnitude < arrowSizeDelta.x)
            //{
            //    copyPoints[length - 2] = secondLastPoint - vec.normalized * lineThickness;
            //}

            vh.AddLine(copyPoints, lineThickness, color);

            int startIndex = vh.currentVertCount;
            var v1 = copyPoints[length - 1] - offset;
            var v2 = copyPoints[length - 1] + offset;

            vh.AddVert(v1, color, new Vector2(0, 0));
            vh.AddVert(v2, color, new Vector2(1, 0));
            vh.AddVert(lastPoint, color, new Vector2(1, 0));
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }

        public static UIVertex[] CreateUIVertexQuad(Vector2 p0, Vector2 p1, float thickness, Color color)
        {
            Vector2 offset = new Vector2(p0.y - p1.y, p1.x - p0.x).normalized * thickness * 0.5f;
            Vector2 v1 = p0 - offset;
            Vector2 v2 = p0 + offset;
            Vector2 v3 = p1 + offset;
            Vector2 v4 = p1 - offset;

            return CreateUIVerts(new Vector2[] { v1, v2, v3, v4 }, quadUvs, color);
        }

        public static UIVertex[] CreateUIVerts(Vector2[] vertices, Vector2[] uvs, Color color)
        {
            int length = vertices.Length;
            UIVertex[] verts = new UIVertex[length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                verts[i] = vert;
            }
            return verts;
        }

        public static void AddRect(this VertexHelper vh, Rect rect, Color fillColor, float strokeWidth, Color strokeColor)
        {
            var strokeMax = rect.width < rect.height ? rect.width : rect.height;
            if (strokeWidth > strokeMax)
                strokeWidth = strokeMax;

            var halfStrokeWidth = strokeWidth * 0.5f;

            var v1 = new Vector4(rect.x + halfStrokeWidth, rect.y + halfStrokeWidth, rect.x + rect.width - halfStrokeWidth, rect.y + rect.height - halfStrokeWidth);
            var v2 = new Vector4(rect.x - halfStrokeWidth, rect.y - halfStrokeWidth, rect.x + rect.width + halfStrokeWidth, rect.y + rect.height + halfStrokeWidth);
            
            int startIndex = vh.currentVertCount;
            vh.AddVert(new Vector3(v1.x, v1.y), fillColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v1.x, v1.w), fillColor, new Vector2(0f, 1f));
            vh.AddVert(new Vector3(v1.z, v1.w), fillColor, new Vector2(1f, 1f));
            vh.AddVert(new Vector3(v1.z, v1.y), fillColor, new Vector2(1f, 0f));

            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);

            startIndex = vh.currentVertCount;
            vh.AddVert(new Vector3(v1.x, v1.y), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v1.x, v1.w), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v1.z, v1.w), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v1.z, v1.y), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v2.x, v2.y), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v2.x, v2.w), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v2.z, v2.w), strokeColor, new Vector2(0f, 0f));
            vh.AddVert(new Vector3(v2.z, v2.y), strokeColor, new Vector2(0f, 0f));

            vh.AddTriangle(startIndex, startIndex + 4, startIndex + 5);
            vh.AddTriangle(startIndex, startIndex + 5, startIndex + 1);
            vh.AddTriangle(startIndex + 1, startIndex + 5, startIndex + 6);
            vh.AddTriangle(startIndex + 1, startIndex + 6, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 6, startIndex + 7);
            vh.AddTriangle(startIndex + 2, startIndex + 7, startIndex + 3);
            vh.AddTriangle(startIndex + 3, startIndex + 7, startIndex + 4);
            vh.AddTriangle(startIndex + 3, startIndex + 4, startIndex + 0);
        }
    
        public static void AddCircle(this VertexHelper vh, Rect rect, Color fillColor, float strokeWidth, Color strokeColor)
        {
            float diameter = rect.width < rect.height ? rect.width : rect.height;
            float inner = (diameter - strokeWidth) * 0.5f;
            float outer = (diameter + strokeWidth) * 0.5f;

            int startIndex = vh.currentVertCount;

            float delta = Mathf.PI * 2 / s_SegmentAmount;
            Vector2 pos = Vector2.zero;
            Vector2 uv0 = new Vector2(0.5f, 0.5f);
            vh.AddVert(pos, fillColor, uv0);

            float angle = 2 * Mathf.PI;
            // inner
            pos = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
            vh.AddVert(pos, fillColor, uv0);

            for (int i = 0; i < s_SegmentAmount; i++)
            {
                angle -= delta;
                // inner
                pos = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
                vh.AddVert(pos, fillColor, uv0);
                vh.AddTriangle(startIndex, startIndex + i + 1, startIndex + i + 2);
            }

            // 轮廓
            angle = 2 * Mathf.PI;
            UIVertex[] UIVerts = new UIVertex[4];
            // 0
            UIVerts[0].position = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            UIVerts[0].color = strokeColor;
            UIVerts[0].uv0 = new Vector2(0, 0);
            // 1
            UIVerts[1].position = outer * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            UIVerts[1].color = strokeColor;
            UIVerts[1].uv0 = new Vector2(0, 1);

            for (int i = 0; i < s_SegmentAmount; i++)
            {
                angle -= delta;
                // 2
                UIVerts[2].position = outer * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                UIVerts[2].color = strokeColor;
                UIVerts[2].uv0 = new Vector2(1, 1);
                // 3
                UIVerts[3].position = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                UIVerts[3].color = strokeColor;
                UIVerts[3].uv0 = new Vector2(1, 0);
                vh.AddUIVertexQuad(UIVerts);

                UIVerts[0].position = UIVerts[3].position;
                UIVerts[1].position = UIVerts[2].position;
            }
        }
    }
}
