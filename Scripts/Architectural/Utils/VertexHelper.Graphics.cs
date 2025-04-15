using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Runtime;
using XFramework.Math;

namespace XFramework.Architectural
{
    public static class MeshBuilderExtendGraphics
    {
        public static void AddWall2D(this MeshBuilder builder, Wall wall, Color color)
        {
            float y = wall.Corner0.Position.y;
            Vector2 direction = wall.ToVector2(wall.Corner0).normalized;
            // 从对象池获取列表
            List<Segment2> holeSegments0 = ListPool<Segment2>.Get();
            List<Segment2> holeSegments1 = ListPool<Segment2>.Get();
            List<IndexFloat> indexFloats = ListPool<IndexFloat>.Get();

            for (int i = 0; i < wall.Holes.Count; i++)
            {
                WallHole hole = wall.Holes[i];

                Segment2 segment = new Segment2();
                segment.p1 = hole.ToPoint2() - direction * hole.Length * 0.5f;
                segment.p2 = hole.ToPoint2() + direction * hole.Length * 0.5f;

                IndexFloat indexFloat = new IndexFloat();
                indexFloat.index = i;
                indexFloat.value = Vector2.Distance(hole.ToPoint2(), wall.Corner0.ToPoint2());
                indexFloats.Add(indexFloat);

                Segment2 segment0 = segment.Move(wall.Thickness * 0.5f);
                Segment2 segment1 = segment.Move(-wall.Thickness * 0.5f);
                holeSegments0.Add(segment0);
                holeSegments1.Add(segment1);
            }

            // Corner0 - Corner1 排序
            indexFloats.Sort();

            // 绘制墙体2D
            Segment2 wallSegment0 = wall.Segments[0];
            Segment2 wallSegment1 = wall.Segments[1];

            int length = holeSegments0.Count;
            if (length == 0)
            {
                foreach (var segment in wall.Segments)
                {
                    builder.AddLine(segment.p1.XOZ(y), segment.p2.XOZ(y), color);
                }
            }
            else if (length > 0)
            {
                builder.AddLine(wallSegment0.p1.XOZ(y), holeSegments0[indexFloats[0].index].p1.XOZ(y), color);
                builder.AddLine(wallSegment1.p1.XOZ(y), holeSegments1[indexFloats[0].index].p1.XOZ(y), color);
                builder.AddLine(holeSegments0[indexFloats[0].index].p1.XOZ(y), holeSegments1[indexFloats[0].index].p1.XOZ(y), color);

                if (length > 1)
                {
                    for (int i = 0; i < length - 1; i++)
                    {
                        builder.AddLine(holeSegments0[indexFloats[i].index].p2.XOZ(y), holeSegments1[indexFloats[i].index].p2.XOZ(y), color);
                        builder.AddLine(holeSegments1[indexFloats[i].index].p2.XOZ(y), holeSegments1[indexFloats[i + 1].index].p1.XOZ(y), color);
                        builder.AddLine(holeSegments0[indexFloats[i + 1].index].p1.XOZ(y), holeSegments1[indexFloats[i + 1].index].p1.XOZ(y), color);
                        builder.AddLine(holeSegments0[indexFloats[i].index].p2.XOZ(y), holeSegments0[indexFloats[i + 1].index].p1.XOZ(y), color);
                    }
                }

                builder.AddLine(holeSegments0[indexFloats[length - 1].index].p2.XOZ(y), wallSegment0.p2.XOZ(y), color);
                builder.AddLine(holeSegments1[indexFloats[length - 1].index].p2.XOZ(y), wallSegment1.p2.XOZ(y), color);
                builder.AddLine(holeSegments0[indexFloats[length - 1].index].p2.XOZ(y), holeSegments1[indexFloats[length - 1].index].p2.XOZ(y), color);

                // 补充两边线段
                for (int i = 2; i < wall.Segments.Count; i++)
                {
                    Segment2 segment = wall.Segments[i];
                    builder.AddLine(segment.p1.XOZ(y), segment.p2.XOZ(y), color);
                }
            }

            // 释放列表
            ListPool<Segment2>.Release(holeSegments0);
            ListPool<Segment2>.Release(holeSegments1);
            ListPool<IndexFloat>.Release(indexFloats);
        }

        public static void AddWall3D(this MeshBuilder builder, Wall wall, Color color)
        {
            List<Vector3> lowerPoints = ListPool<Vector3>.Get();
            List<Vector3> upperPoints = ListPool<Vector3>.Get();
            List<Vector3> quadPoints = ListPool<Vector3>.Get();
            int count = wall.Points.Count;
            for (int i = 0; i < count; i++)
            {
                lowerPoints.Add(wall.Points[i]);
                upperPoints.Add(wall.Points[i] + new Vector3(0, wall.Height, 0));
            }
            // lower
            builder.AddPolygon(lowerPoints, true, Vector3.down,  color);
            // upper
            builder.AddPolygon(upperPoints, false, Vector3.up,  color);
            // hole
            for (int i = 0; i < count - 1; i++)
            {
                Vector3 direction = (lowerPoints[i + 1] - lowerPoints[i]).normalized;
                Vector3 normal = Vector3.Cross(direction, Vector3.up).normalized;
                // quadPoints
                quadPoints.Clear();
                quadPoints.Add(lowerPoints[i]);
                quadPoints.Add(lowerPoints[i + 1]);
                quadPoints.Add(upperPoints[i + 1]);
                quadPoints.Add(upperPoints[i]);

                if (i == 1 || i == 4)
                {
                    var tessellator = new Tessellator();
                    tessellator.AddContour(quadPoints);
                    foreach (var hole in wall.Holes)
                    {
                        quadPoints.Clear();
                        Vector3 point = hole.Position + normal * wall.Thickness * 0.5f;
                        quadPoints.Add(point - direction * hole.Length * 0.5f - Vector3.up * hole.Height * 0.5f);
                        quadPoints.Add(point + direction * hole.Length * 0.5f - Vector3.up * hole.Height * 0.5f);
                        quadPoints.Add(point + direction * hole.Length * 0.5f + Vector3.up * hole.Height * 0.5f);
                        quadPoints.Add(point - direction * hole.Length * 0.5f + Vector3.up * hole.Height * 0.5f);
                        tessellator.AddContour(quadPoints);

                        // 添加墙洞内部面
                        if (i == 1)
                        {
                            builder.AddPolyFace(quadPoints, -normal, wall.Thickness, color);
                        }
                    }

                    builder.AddTessellator(tessellator, normal, color);
                }
                else
                {
                    builder.AddQuad(quadPoints, color);
                }
            }

            ListPool<Vector3>.Release(lowerPoints);
            ListPool<Vector3>.Release(upperPoints);
            ListPool<Vector3>.Release(quadPoints);
        }

        public static void AddDoor2D(this MeshBuilder builder, Door door, Color color)
        {
            float length = door.Length;
            float thickness = ArchitectSettings.Door.thickness;
            Vector2 size = new Vector2(thickness, length);
            switch (door.DoorType)
            {
                case DoorType.Single:
                    builder.AddWireRect(new Vector3(length * -0.5f + thickness * 0.5f, 0, length * 0.5f), Quaternion.identity, size, color);
                    builder.AddWireArc(new Vector3(length * -0.5f, 0, 0), Vector3.up, 0, Mathf.PI * 0.5f, length, color);
                    break;
                case DoorType.Double:
                    float half = length / 2;
                    size = new Vector2(thickness, half);
                    builder.AddWireRect(new Vector3(length * -0.5f + thickness * 0.5f, 0, half * 0.5f), Quaternion.identity, size, color);
                    builder.AddWireRect(new Vector3(length * 0.5f - thickness * 0.5f, 0, half * 0.5f), Quaternion.identity, size, color);
                    builder.AddWireArc(new Vector3(length * -0.5f, 0, 0), Vector3.up, 0, Mathf.PI * 0.5f, half, color);
                    builder.AddWireArc(new Vector3(length * 0.5f, 0, 0), Vector3.up, Mathf.PI * 0.5f, Mathf.PI * 0.5f, half, color);
                    break;
                case DoorType.Revolving:
                    half = length / 2;
                    builder.AddWireCircle(new Vector3(0, 0, 0), Vector3.up, 0.1f, color);
                    builder.AddWireArc(new Vector3(0, 0, 0), Vector3.up, 2 * Mathf.PI / 3, 2 * Mathf.PI / 3, half, color);
                    builder.AddWireArc(new Vector3(0, 0, 0), Vector3.up, -Mathf.PI / 3, 2 * Mathf.PI / 3, half, color);
                    builder.AddWireArc(new Vector3(0, 0, 0), Vector3.up, 2 * Mathf.PI / 3, 2 * Mathf.PI / 3, half - 0.15f, color);
                    builder.AddWireArc(new Vector3(0, 0, 0), Vector3.up, -Mathf.PI / 3, 2 * Mathf.PI / 3, half - 0.15f, color);

                    // 直线两点
                    Vector3 p1, p2;

                    // 十字线
                    float theta = Mathf.PI / 4;
                    float radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = 3 * Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = -3 * Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = -Mathf.PI / 4;
                    radius = 0.1f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half - 0.15f;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    // 圆弧连接线
                    theta = Mathf.PI / 3;
                    radius = half - 0.15f;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = 2 * Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = -2 * Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    theta = -Mathf.PI / 3;
                    radius = half - 0.15f; ;
                    p1 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    radius = half;
                    p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
                    builder.AddLine(p1, p2, color);

                    break;
                default:
                    break;
            }

        }

        public static void AddWindow2D(this MeshBuilder builder, Window window, Color color)
        {
            float length = window.Length;
            float thickness = window.Wall.Thickness;
            Vector2 size = new Vector2(length, thickness);
            builder.AddWireRect(Vector3.zero, Quaternion.identity, size, color);
            builder.AddLine(new Vector3(length * -0.5f, 0, -thickness / 6), new Vector3(length * 0.5f, 0, -thickness / 6), color);
            builder.AddLine(new Vector3(length * -0.5f, 0, thickness / 6), new Vector3(length * 0.5f, 0, thickness / 6), color);
        }

        public static void AddPass2D(this MeshBuilder builder, Pass pass, Color color)
        {
            float length = pass.Length;
            float thickness = pass.Wall.Thickness;
            //builder.AddDashLine(new Vector3(length * -0.5f, 0, -thickness / 2), new Vector3(length * 0.5f, 0, -thickness / 2), new float[2] { 0.1f, 0.1f }, color);
            //builder.AddDashLine(new Vector3(length * -0.5f, 0, thickness / 2), new Vector3(length * 0.5f, 0, thickness / 2), new float[2] { 0.1f, 0.1f }, color);
            builder.AddLine(new Vector3(length * -0.5f, 0, -thickness / 2), new Vector3(length * 0.5f, 0, -thickness / 2), color);
            builder.AddLine(new Vector3(length * -0.5f, 0, thickness / 2), new Vector3(length * 0.5f, 0, thickness / 2), color);
        }

        public static void AddRoom2D(this MeshBuilder builder, Room room, Color color)
        {
            if (room.Walls.Count <= 0)
                return;

            // y
            float y = room.Walls[0].Corner0.Position.y;
            // tess
            var tessellator = new Tessellator();
            
            // 房间轮廓
            List<Vector3> contour = ListPool<Vector3>.Get();
            foreach (var point in room.Contour)
            {
                contour.Add(point.XOZ(y));
            }
            tessellator.AddContour(contour);

            // 内部轮廓
            foreach (var innerContour in room.InnerContours)
            {
                contour.Clear();
                foreach (var point in innerContour)
                {
                    contour.Add(point.XOZ(y));
                }
                tessellator.AddContour(contour);
            }

            // normal
            Vector3 normal = Vector3.up;
            // Tessellate
            tessellator.Tessellate(normal: normal);
            // uvs
            Vector2[] uvs = MathUtility.CalculateUVs(tessellator.Positions, normal);
            // vertices
            int startIndex = builder.CurrentVertCount;
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

            // 释放
            ListPool<Vector3>.Release(contour);
        }

        public static void AddRoom3D(this MeshBuilder builder, Room room, Color color)
        {
            if (room.Walls.Count <= 0)
                return;

            // tess
            var tessellator = new Tessellator();
            // y
            float y = room.Walls[0].Corner0.Position.y;

            // 房间轮廓
            List<Vector3> contour = ListPool<Vector3>.Get();
            foreach (var point in room.Contour)
            {
                contour.Add(point.XOZ(y));
            }
            tessellator.AddContour(contour);

            // 内部轮廓
            foreach (var innerContour in room.InnerContours)
            {
                contour.Clear();
                foreach (var point in innerContour)
                {
                    contour.Add(point.XOZ(y));
                }
                tessellator.AddContour(contour);
            }

            // 地板
            int startIndex = builder.CurrentVertCount;
            // normal
            Vector3 normal = Vector3.up;
            // Tessellate
            tessellator.Tessellate(normal: normal);
            Vector3[] positions = tessellator.Positions;
            // uvs
            Vector2[] uvs = MathUtility.CalculateUVs(positions, normal);
            // vertices
            for (int i = 0; i < tessellator.VertexCount; i++)
            {
                builder.AddVert(positions[i], color, uvs[i], normal);
            }
            // indices
            var indices = tessellator.Indices;
            for (int i = 0, length = indices.Length; i < length; i++)
            {
                indices[i] += startIndex;
            }
            builder.AddIndices(indices);

            // 顶面 TODO
            startIndex = builder.CurrentVertCount;
            float height = room.Walls[0].Height;
            // normal
            normal = Vector3.down;
            // vertices
            for (int i = 0; i < tessellator.VertexCount; i++)
            {
                builder.AddVert(tessellator.Positions[i] + new Vector3(0, height, 0), color, uvs[i], normal);
            }
            // indices
            for (int i = 0, length = indices.Length; i < length; i += 3)
            {
                int temp = indices[i + 1];
                indices[i + 1] = indices[i + 2];
                indices[i + 2] = temp;

                indices[i] += startIndex;
                indices[i + 1] += startIndex;
                indices[i + 2] += startIndex;
            }
            builder.AddIndices(indices);

            // 释放
            ListPool<Vector3>.Release(contour);
        }

    
        public static void AddArea2D(this MeshBuilder builder, Area area, Color color)
        {
            float size = RuntimeHandles.GetHandleSize(area.Contour[0]) * 2f;
            for (int i = 0; i < area.Contour.Count - 1; i++)
            {
                builder.AddDashLine(area.Contour[i], area.Contour[i + 1], new float[] { size, size }, color);
            }
        }
    }
}
