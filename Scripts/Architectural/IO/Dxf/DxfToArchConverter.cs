using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;
using XFramework.Math;
using UnityEngine;

namespace XFramework.Architectural
{
    public class DxfToArchConverter
    {
        /// <summary>
        /// 墙体厚度阈值
        /// </summary>
        private float wall_threshold = 0.35f;

        /// <summary>
        /// 柱子厚度阈值
        /// </summary>
        private float column_threshold = 0.65f;

        /// <summary>
        /// 房间面积阈值
        /// </summary>
        private float room_area_threshold = 2f;

        /// <summary>
        /// dxf
        /// </summary>
        private netDxf.DxfDocument document;

        /// <summary>
        /// 墙体层
        /// </summary>
        private List<string> wallLayers;

        /// <summary>
        /// 门层
        /// </summary>
        private List<string> doorLayers;

        /// <summary>
        /// 窗子层
        /// </summary>
        private List<string> windowLayers;

        /// <summary>
        /// 房间名称层
        /// </summary>
        private List<string> roomNameLayers;

        public DxfToArchConverter(netDxf.DxfDocument document, List<string> wallLayers, List<string> doorLayers, List<string> windowLayers, List<string> roomNameLayers)
        {
            this.document = document;
            this.wallLayers = wallLayers;
            this.doorLayers = doorLayers;
            this.windowLayers = windowLayers;
            this.roomNameLayers = roomNameLayers;
        }

        public Document Process()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // wall
            List<Segment2> wall_segments = DxfUtility.FindSegments(document, wallLayers);
            // door
            List<Arc2> door_arcs = DxfUtility.FindArcs(document, doorLayers);
            // window
            List<Segment2> window_segments = DxfUtility.FindWindowSegments(document, windowLayers);
            // room name
            List<netDxf.Entities.Text> roomNameTexts = DxfUtility.FindTexts(document, roomNameLayers);

            // 临时列表
            List<Segment2> segments_x = new List<Segment2>();
            List<Segment2> segments_y = new List<Segment2>();
            List<Segment2> paral_segments_x = new List<Segment2>();
            List<Segment2> paral_segments_y = new List<Segment2>();
            List<Segment2> wall_segments_x = new List<Segment2>();
            List<Segment2> wall_segments_y = new List<Segment2>();
            List<Segment2> wall_hole_paral_segments_x = new List<Segment2>();
            List<Segment2> wall_hole_paral_segments_y = new List<Segment2>();

            // 找到xy方向外轮廓
            // 水平方向
            //List<Segment2> segments_x = new List<Segment2>();
            foreach (var segment in wall_segments)
            {
                if (MathUtility.Appr(segment.direction.y, 0))
                {
                    float dot = Vector2.Dot(Vector2.right, segment.direction);
                    if (dot > 0)
                        segments_x.Add(new Segment2(segment.p1, segment.p2));
                    else
                        segments_x.Add(new Segment2(segment.p2, segment.p1));
                }
            }

            // 竖直方向
            //List<Segment2> segments_y = new List<Segment2>();
            foreach (var segment in wall_segments)
            {
                if (MathUtility.Appr(segment.direction.x, 0))
                {
                    float dot = Vector2.Dot(Vector2.up, segment.direction);
                    if (dot > 0)
                        segments_y.Add(new Segment2(segment.p1, segment.p2));
                    else
                        segments_y.Add(new Segment2(segment.p2, segment.p1));
                }
            }

            // 合并连接的线段
            // 水平方向
            for (int i = 0; i < segments_x.Count;)
            {
                Segment2 a = segments_x[i];
                bool flag = false;
                for (int j = 0; j < segments_x.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = segments_x[j];
                    if (a.Overlap(b, out Segment2 result))
                    {
                        bool exist_ab = a.Union(b, out Segment2 ab);
                        if (exist_ab)
                        {
                            segments_x[i] = ab;
                            segments_x.RemoveAt(j);
                        }
                        flag = true;

                        if (i > j)
                            i--;

                        break;
                    }
                }

                if (!flag)
                    i++;
            }
            // 竖直方向
            for (int i = 0; i < segments_y.Count;)
            {
                Segment2 a = segments_y[i];
                bool flag = false;
                for (int j = 0; j < segments_y.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = segments_y[j];
                    if (a.Overlap(b, out Segment2 result))
                    {
                        bool exist_ab = a.Union(b, out Segment2 ab);
                        if (exist_ab)
                        {
                            segments_y[i] = ab;
                            segments_y.RemoveAt(j);
                        }
                        flag = true;
                        if (i > j)
                            i--;

                        break;
                    }
                }

                if (!flag)
                    i++;
            }

            // 排序
            // 水平方向
            segments_x.Sort((x, y) => x.p1.y.CompareTo(y.p1.y));
            // 竖直方向
            segments_y.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));


            // 找被门窗打断的墙体，相对的线段
            // 水平方向

            for (int i = 0; i < segments_x.Count; i++)
            {
                Segment2 a = segments_x[i];
                float distance, thickness;

                for (int j = i + 1; j < segments_x.Count; j++)
                {
                    Segment2 b = segments_x[j];
                    distance = b.p1.y - a.p1.y;
                    thickness = a.p2.x - a.p1.x;

                    if (MathUtility.Appr(a.p1.x, b.p1.x)
                        && MathUtility.Appr(a.p2.x, b.p2.x)
                        && distance > wall_threshold
                        && thickness < wall_threshold)
                    {
                        // 是否相对应
                        bool echo = true;

                        Segment2 c = new Segment2((a.p1.x + a.p2.x) * 0.5f, a.p1.y, (b.p1.x + b.p2.x) * 0.5f, b.p1.y);
                        foreach (var segment in segments_x)
                        {
                            if (segment.Equals(a) || segment.Equals(b))
                                continue;

                            if (segment.Intersect(c, out Vector2 point))
                            {
                                echo = false;
                                break;
                            }
                        }

                        Segment2 ab1 = new Segment2(a.p1, b.p1);
                        Segment2 ab2 = new Segment2(a.p2, b.p2);
                        foreach (var segment in segments_y)
                        {
                            if (!echo)
                                break;

                            if (segment.Equals(ab1))
                            {
                                echo = false;
                                break;
                            }

                            if (segment.Equals(ab2))
                            {
                                echo = false;
                                break;
                            }
                        }

                        if (echo)
                        {
                            paral_segments_x.Add(a);
                            paral_segments_x.Add(b);
                            break;
                        }
                    }
                }
            }
            // 竖直方向

            for (int i = 0; i < segments_y.Count; i++)
            {
                Segment2 a = segments_y[i];
                float distance, thickness;

                for (int j = i + 1; j < segments_y.Count; j++)
                {
                    Segment2 b = segments_y[j];
                    distance = b.p1.x - a.p1.x;
                    thickness = a.p2.y - a.p1.y;

                    if (MathUtility.Appr(a.p1.y, b.p1.y)
                        && MathUtility.Appr(a.p2.y, b.p2.y)
                        && distance > wall_threshold
                        && thickness < wall_threshold)
                    {
                        // 连接线是否和竖直线段有交点
                        bool echo = true;
                        Segment2 c = new Segment2(a.p1.x, (a.p1.y + a.p2.y) * 0.5f, b.p1.x, (b.p1.y + b.p2.y) * 0.5f);
                        foreach (var segment in segments_y)
                        {
                            if (segment.Equals(a) || segment.Equals(b))
                                continue;

                            if (segment.Intersect(c, out Vector2 point))
                            {
                                echo = false;
                                break;
                            }
                        }

                        Segment2 ab1 = new Segment2(a.p1, b.p1);
                        Segment2 ab2 = new Segment2(a.p2, b.p2);
                        foreach (var segment in segments_x)
                        {
                            if (!echo)
                                break;

                            if (segment.Equals(ab1))
                            {
                                echo = false;
                                break;
                            }

                            if (segment.Equals(ab2))
                            {
                                echo = false;
                                break;
                            }
                        }

                        if (echo)
                        {
                            paral_segments_y.Add(a);
                            paral_segments_y.Add(b);
                            break;
                        }
                    }
                }
            }

            // 合并被门窗打断的线段
            // 水平方向
            for (int i = 0; i < paral_segments_x.Count - 1; i += 2)
            {
                Segment2 a = paral_segments_x[i];
                Segment2 b = paral_segments_x[i + 1];

                bool[] _exists = new bool[4];
                Segment2[] _segments = new Segment2[4];
                foreach (var segment in segments_y)
                {
                    if (!_exists[0] && a.p1.Equals(segment.p2))
                    {
                        _segments[0] = segment;
                        _exists[0] = true;
                    }

                    if (!_exists[1] && a.p2.Equals(segment.p2))
                    {
                        _segments[1] = segment;
                        _exists[1] = true;
                    }

                    if (!_exists[2] && b.p1.Equals(segment.p1))
                    {
                        _segments[2] = segment;
                        _exists[2] = true;
                    }

                    if (!_exists[3] && b.p2.Equals(segment.p1))
                    {
                        _segments[3] = segment;
                        _exists[3] = true;
                    }
                }

                if (_exists[0] && _exists[1] && _exists[2] && _exists[3])
                {
                    bool exist_ab1 = _segments[0].Union(_segments[2], out Segment2 ab1);
                    bool exist_ab2 = _segments[1].Union(_segments[3], out Segment2 ab2);
                    if (exist_ab1 && exist_ab2)
                    {
                        // x
                        segments_x.Remove(a);
                        segments_x.Remove(b);
                        wall_hole_paral_segments_x.Add(a);
                        wall_hole_paral_segments_x.Add(b);
                        // y
                        foreach (var _segment in _segments)
                        {
                            segments_y.Remove(_segment);
                        }
                        segments_y.Add(ab1);
                        segments_y.Add(ab2);

                    }
                }
            }
            // 竖直方向
            for (int i = 0; i < paral_segments_y.Count - 1; i += 2)
            {
                Segment2 a = paral_segments_y[i];
                Segment2 b = paral_segments_y[i + 1];

                bool[] _exists = new bool[4];
                Segment2[] _segments = new Segment2[4];
                foreach (var segment in segments_x)
                {
                    if (!_exists[0] && a.p1.Equals(segment.p2))
                    {
                        _segments[0] = segment;
                        _exists[0] = true;
                    }

                    if (!_exists[1] && a.p2.Equals(segment.p2))
                    {
                        _segments[1] = segment;
                        _exists[1] = true;
                    }

                    if (!_exists[2] && b.p1.Equals(segment.p1))
                    {
                        _segments[2] = segment;
                        _exists[2] = true;
                    }

                    if (!_exists[3] && b.p2.Equals(segment.p1))
                    {
                        _segments[3] = segment;
                        _exists[3] = true;
                    }
                }

                if (_exists[0] && _exists[1] && _exists[2] && _exists[3])
                {
                    bool exist_ab1 = _segments[0].Union(_segments[2], out Segment2 ab1);
                    bool exist_ab2 = _segments[1].Union(_segments[3], out Segment2 ab2);
                    if (exist_ab1 && exist_ab2)
                    {
                        // y
                        segments_y.Remove(a);
                        segments_y.Remove(b);
                        wall_hole_paral_segments_y.Add(a);
                        wall_hole_paral_segments_y.Add(b);
                        // x
                        foreach (var _segment in _segments)
                        {
                            segments_x.Remove(_segment);
                        }
                        segments_x.Add(ab1);
                        segments_x.Add(ab2);
                    }
                }
            }
            // 排序
            // 水平方向
            segments_x.Sort((x, y) => x.p1.y.CompareTo(y.p1.y));
            // 竖直方向
            segments_y.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));


            // 合并被柱子打断的线段
            // 水平方向
            for (int i = 0; i < segments_x.Count;)
            {
                Segment2 a = segments_x[i];

                bool connect = false;
                for (int j = 0; j < segments_x.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = segments_x[j];
                    if (!MathUtility.Appr(a.p1.y, b.p1.y))
                        continue;

                    if (a.p2.x > b.p1.x)
                        continue;

                    float distance = b.p1.x - a.p2.x;
                    if (distance < column_threshold)
                    {
                        // 合并
                        bool ab_union = a.Union(b, out Segment2 ab);
                        if (ab_union)
                        {
                            segments_x[i] = ab;
                            segments_x.Remove(b);
                            connect = true;

                            if (j < i)
                                i--;

                            break;
                        }
                    }
                }

                if (!connect)
                    i++;
            }

            // 竖直方向
            for (int i = 0; i < segments_y.Count;)
            {
                Segment2 a = segments_y[i];

                bool connect = false;
                for (int j = 0; j < segments_y.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = segments_y[j];
                    if (!MathUtility.Appr(a.p1.x, b.p1.x))
                        continue;

                    if (a.p2.y > b.p1.y)
                        continue;

                    float distance = b.p1.y - a.p2.y;
                    if (distance < column_threshold)
                    {
                        // 合并
                        bool ab_union = a.Union(b, out Segment2 ab);
                        if (ab_union)
                        {
                            segments_y[i] = ab;
                            segments_y.Remove(b);
                            connect = true;

                            if (j < i)
                                i--;

                            break;
                        }
                    }
                }

                if (!connect)
                    i++;
            }

            // 排序
            // 水平方向
            segments_x.Sort((x, y) => x.p1.y.CompareTo(y.p1.y));
            // 竖直方向
            segments_y.Sort((x, y) => x.p1.x.CompareTo(y.p1.x));

            // 找墙体中线（根据找出的轮廓线，算中线）
            // 水平方向
            //List<Segment2> wall_segments_x = new List<Segment2>();
            List<float> wall_thickness_x = new List<float>();
            bool[] invalids_x = new bool[segments_x.Count];
            for (int i = 0; i < segments_x.Count; i++)
            {
                if (invalids_x[i])
                    continue;

                Segment2 a = segments_x[i];
                List<Segment2> matches = new List<Segment2>();
                for (int j = i + 1; j < segments_x.Count; j++)
                {
                    if (invalids_x[j])
                        continue;

                    Segment2 b = segments_x[j];
                    if (MathUtility.Appr(a.p1.y, b.p1.y))
                        continue;

                    float thickness = b.p1.y - a.p1.y;
                    if (thickness > wall_threshold)
                        continue;

                    bool overlap = false;
                    foreach (var match in matches)
                    {
                        float dis = match.p1.y - b.p1.y;
                        if (match.Move(dis).Overlap(b, out Segment2 seg))
                        {
                            if (seg.magnitude > 0)
                            {
                                overlap = true;
                                break;
                            }
                        }
                    }

                    if (overlap)
                        continue;

                    Segment2 m1 = a.Move(thickness * 0.5f);
                    Segment2 m2 = b.Move(-thickness * 0.5f);
                    if (m1.Overlap(m2, out Segment2 result))
                    {
                        if (result.magnitude > 0)
                        {
                            wall_segments_x.Add(result);
                            wall_thickness_x.Add(thickness);
                            matches.Add(b);

                            if (result.Equals(m1))
                            {
                                invalids_x[i] = true;
                            }

                            if (result.Equals(m2))
                            {
                                invalids_x[j] = true;
                            }
                        }
                    }
                }
            }
            // 竖直方向
            //List<Segment2> wall_segments_y = new List<Segment2>();
            List<float> wall_thickness_y = new List<float>();
            bool[] invalids_y = new bool[segments_y.Count];
            for (int i = 0; i < segments_y.Count; i++)
            {
                if (invalids_y[i])
                    continue;

                Segment2 a = segments_y[i];
                List<Segment2> matches = new List<Segment2>();
                for (int j = i + 1; j < segments_y.Count; j++)
                {
                    if (invalids_y[j])
                        continue;

                    Segment2 b = segments_y[j];
                    if (MathUtility.Appr(a.p1.x, b.p1.x))
                        continue;

                    float thickness = b.p1.x - a.p1.x;
                    if (thickness > wall_threshold)
                        continue;

                    bool overlap = false;
                    foreach (var match in matches)
                    {
                        float dis = match.p1.x - b.p1.x;
                        if (match.Move(dis).Overlap(b, out Segment2 seg))
                        {
                            if (seg.magnitude > 0)
                            {
                                overlap = true;
                                break;
                            }
                        }
                    }

                    if (overlap)
                        continue;

                    Segment2 m1 = a.Move(-thickness * 0.5f);
                    Segment2 m2 = b.Move(thickness * 0.5f);
                    if (m1.Overlap(m2, out Segment2 result))
                    {
                        if (result.magnitude > 0)
                        {
                            wall_segments_y.Add(result);
                            wall_thickness_y.Add(thickness);
                            matches.Add(b);
                        }

                        if (result.Equals(m1))
                        {
                            invalids_y[i] = true;
                        }

                        if (result.Equals(m2))
                        {
                            invalids_y[j] = true;
                        }
                    }
                }
            }

            // 延伸墙体中线, 
            // 水平方向
            for (int i = 0; i < wall_segments_x.Count; i++)
            {
                Segment2 a = wall_segments_x[i];

                Segment2 min1 = wall_segments_y.Min(x => x.GetDistanceToPoint(a.p1));
                float delta1 = 0;
                int index1 = wall_segments_y.IndexOf(min1);
                if (index1 != -1)
                {
                    float thickness = wall_thickness_y[index1];
                    delta1 = a.p1.x - min1.p1.x + thickness * 0.5f;
                }

                Segment2 min2 = wall_segments_y.Min(x => x.GetDistanceToPoint(a.p2));
                float delta2 = 0;
                int index2 = wall_segments_y.IndexOf(min2);
                if (index2 != -1)
                {
                    float thickness = wall_thickness_y[index2];
                    delta2 = min2.p1.x - a.p2.x + thickness * 0.5f;
                }

                wall_segments_x[i] = a.Extend(delta1, delta2);
            }
            // 竖直方向
            for (int i = 0; i < wall_segments_y.Count; i++)
            {
                Segment2 a = wall_segments_y[i];

                Segment2 min1 = wall_segments_x.Min(x => x.GetDistanceToPoint(a.p1));
                float delta1 = 0;
                int index1 = wall_segments_x.IndexOf(min1);
                if (index1 != -1)
                {
                    float thickness = wall_thickness_x[index1];
                    delta1 = a.p1.y - min1.p1.y + thickness * 0.5f;
                }

                Segment2 min2 = wall_segments_x.Min(x => x.GetDistanceToPoint(a.p2));
                float delta2 = 0;
                int index2 = wall_segments_x.IndexOf(min2);
                if (index2 != -1)
                {
                    float thickness = wall_thickness_x[index2];
                    delta2 = min2.p1.y - a.p2.y + thickness * 0.5f;
                }

                wall_segments_y[i] = a.Extend(delta1, delta2);
            }


            // 合并墙体中心线，去除重复并存储
            // 水平方向
            for (int i = 0; i < wall_segments_x.Count;)
            {
                Segment2 a = wall_segments_x[i];
                bool flag = false;
                for (int j = 0; j < wall_segments_x.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = wall_segments_x[j];
                    if (a.Overlap(b, out Segment2 result))
                    {
                        bool exist_ab = a.Union(b, out Segment2 ab);
                        if (exist_ab)
                        {
                            wall_segments_x[i] = ab;
                            wall_segments_x.RemoveAt(j);
                            wall_thickness_x.RemoveAt(j);
                        }
                        flag = true;

                        if (i > j)
                            i--;

                        break;
                    }
                }

                if (!flag)
                    i++;
            }
            // 竖直方向， 
            for (int i = 0; i < wall_segments_y.Count;)
            {
                Segment2 a = wall_segments_y[i];
                bool flag = false;
                for (int j = 0; j < wall_segments_y.Count; j++)
                {
                    if (i == j)
                        continue;

                    Segment2 b = wall_segments_y[j];
                    if (a.Overlap(b, out Segment2 result))
                    {
                        bool exist_ab = a.Union(b, out Segment2 ab);
                        if (exist_ab)
                        {
                            wall_segments_y[i] = ab;
                            wall_segments_y.RemoveAt(j);
                            wall_thickness_y.RemoveAt(j);
                        }
                        flag = true;
                        if (i > j)
                            i--;

                        break;
                    }
                }

                if (!flag)
                    i++;
            }

            // 文档记录
            Document doc = new Document();
            doc.AddFloor();

            // 搜索交点作为直线端点，重新矢量化中线
            // 水平方向
            for (int i = 0; i < wall_segments_x.Count; i++)
            {
                Segment2 a = wall_segments_x[i];
                List<Corner> corners = new List<Corner>();
                for (int j = 0; j < wall_segments_y.Count; j++)
                {
                    Segment2 b = wall_segments_y[j];
                    if (a.Intersect(b, out Vector2 point))
                    {
                        bool flag = doc.CurrentFloor.TryGetCorner(point.XOZ(), out Corner corner);
                        // 不存在 corner
                        if (!flag)
                        {
                            corner = new Corner();
                            corner.Position = point.XOZ();
                            doc.CurrentFloor.Corners.Add(corner);
                        }
                        // 添加
                        corners.Add(corner);
                    }
                }
                if (corners.Count < 2)
                {
                    Debug.LogWarningFormat("corners count : {0} < 2 at segment : {1}", corners.Count, a);
                    continue;
                }
                // 添加墙体
                for (int j = 0; j < corners.Count - 1; j++)
                {
                    Wall wall = new Wall(corners[j], corners[j + 1]);
                    wall.Thickness = wall_thickness_x[i];
                    doc.CurrentFloor.Walls.Add(wall);
                }
            }

            // 竖直方向
            for (int i = 0; i < wall_segments_y.Count; i++)
            {
                Segment2 a = wall_segments_y[i];
                List<Corner> corners = new List<Corner>();
                for (int j = 0; j < wall_segments_x.Count; j++)
                {
                    Segment2 b = wall_segments_x[j];
                    if (a.Intersect(b, out Vector2 point))
                    {
                        bool flag = doc.CurrentFloor.TryGetCorner(point.XOZ(), out Corner corner);
                        // 不存在 corner
                        if (!flag)
                        {
                            corner = new Corner();
                            corner.Position = point.XOZ();
                            doc.CurrentFloor.Corners.Add(corner);
                        }
                        // 添加
                        corners.Add(corner);
                    }
                }
                if (corners.Count < 2)
                {
                    Debug.LogWarningFormat("corners count : {0} < 2 at segment : {1}", corners.Count, a);
                    continue;
                }
                // 添加墙体
                for (int j = 0; j < corners.Count - 1; j++)
                {
                    Wall wall = new Wall(corners[j], corners[j + 1]);
                    wall.Thickness = wall_thickness_y[i];
                    doc.CurrentFloor.Walls.Add(wall);
                }
            }

            // 添加门窗
            // 水平方向
            for (int i = 0; i < wall_hole_paral_segments_x.Count; i += 2)
            {
                Segment2 a = wall_hole_paral_segments_x[i];
                Segment2 b = wall_hole_paral_segments_x[i + 1];

                Segment2 ab1 = new Segment2(a.p1, b.p1);
                ab1 = ab1.Move(0.1f);
                Segment2 ab2 = new Segment2(a.p2, b.p2);
                ab2 = ab2.Move(-0.1f);
                float length = b.p1.y - a.p1.y;
                Vector2 center = new Vector2((a.p1.x + a.p2.x) * 0.5f, (a.p1.y + b.p1.y) * 0.5f);

                // 找寻墙体
                Wall wall = doc.CurrentFloor.Walls.Min(x => x.ToSegment2(x.Corner0).GetDistanceToPoint(center));
                if (wall == null)
                    continue;

                Vector3 point = wall.ToLine2(wall.Corner0).ClosestPointOnLine(center).XOZ();

                // 门
                int ab1_cross_door_count = 0;
                int ab2_cross_door_count = 0;
                List<Vector2> ab1_cross_points = new List<Vector2>();
                List<Vector2> ab2_cross_points = new List<Vector2>();

                foreach (var arc in door_arcs)
                {
                    int state = GeometryUtils.SegmentIntersectArc(ab1, arc, out List<Vector2> result1);
                    if (state == 1)
                    {
                        ab1_cross_points.AddRange(result1);
                        ab1_cross_door_count++;
                    }

                    state = GeometryUtils.SegmentIntersectArc(ab2, arc, out List<Vector2> result2);
                    if (state == 1)
                    {
                        ab2_cross_points.AddRange(result2);
                        ab2_cross_door_count++;
                    }
                }

                // 房间类型
                DoorType doorType = DoorType.Single;
                // 翻转
                int flip = 0;
                // 判断是否门
                if (ab1_cross_door_count == 1) // 单开门
                {
                    doorType = DoorType.Single;
                    if (ab1_cross_points[0].y - ab1.p1.y > ab1.p2.y - ab1_cross_points[0].y)
                    {
                        flip = 0;
                    }
                    else
                    {
                        flip = 3;
                    }
                }
                else if (ab1_cross_door_count == 2) // 双开门
                {
                    doorType = DoorType.Double;
                    flip = 0;
                }
                else if (ab2_cross_door_count == 1) // 单开门
                {
                    doorType = DoorType.Single;
                    if (ab2_cross_points[0].y - ab2.p1.y > ab2.p2.y - ab2_cross_points[0].y)
                    {
                        flip = 1;
                    }
                    else
                    {
                        flip = 2;
                    }
                }
                else if (ab2_cross_door_count == 2) // 双开门
                {
                    doorType = DoorType.Double;
                    flip = 1;
                }
                else if (ab1_cross_door_count == 4
                    && ab2_cross_door_count == 4) // 旋转门
                {
                    doorType = DoorType.Revolving;
                }

                if (ab1_cross_door_count > 0 || ab2_cross_door_count > 0)
                {
                    Door door = new Door();
                    door.DoorType = doorType;
                    door.Flip = flip;
                    door.Length = length;
                    door.Height = ArchitectSettings.Door.height;
                    door.Thickness = ArchitectSettings.Door.thickness;
                    door.Bottom = ArchitectSettings.Door.bottom;
                    door.Position = new Vector3(point.x, wall.Corner0.Position.y + door.Bottom + door.Height * 0.5f, point.z);
                    door.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                    door.Wall = wall;
                    doc.CurrentFloor.Doors.Add(door);
                    continue;
                }

                // 窗子
                int cross_window_count = 0;
                Segment2 cross_segment;
                foreach (var segment in window_segments)
                {
                    if (segment.Overlap(ab1, out cross_segment))
                    {
                        if (MathUtility.Appr(cross_segment.magnitude, length))
                        {
                            cross_window_count++;
                        }
                    }

                    if (segment.Overlap(ab2, out cross_segment))
                    {
                        if (MathUtility.Appr(cross_segment.magnitude, length))
                        {
                            cross_window_count++;
                        }
                    }
                }

                // 判断是否窗子
                if (cross_window_count == 2)
                {
                    Window window = new Window();
                    window.Length = length;
                    window.Height = ArchitectSettings.Window.height;
                    window.Thickness = ArchitectSettings.Window.thickness;
                    window.Bottom = ArchitectSettings.Window.bottom;
                    window.Position = new Vector3(point.x, wall.Corner0.Position.y + window.Bottom + window.Height * 0.5f, point.z);
                    window.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                    window.Wall = wall;
                    doc.CurrentFloor.Windows.Add(window);
                    // TODO
                    continue;
                }

                // 垭口
                Pass pass = new Pass();
                pass.Length = length;
                pass.Height = ArchitectSettings.Pass.height;
                pass.Thickness = ArchitectSettings.Pass.thickness;
                pass.Bottom = ArchitectSettings.Pass.bottom;
                pass.Position = new Vector3(point.x, wall.Corner0.Position.y + pass.Bottom + pass.Height * 0.5f, point.z);
                pass.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                pass.Wall = wall;
                doc.CurrentFloor.Passes.Add(pass);
            }
            // 竖直方向
            for (int i = 0; i < wall_hole_paral_segments_y.Count; i += 2)
            {
                Segment2 a = wall_hole_paral_segments_y[i];
                Segment2 b = wall_hole_paral_segments_y[i + 1];

                Segment2 ab1 = new Segment2(a.p1, b.p1);
                ab1 = ab1.Move(-0.1f);
                Segment2 ab2 = new Segment2(a.p2, b.p2);
                ab2 = ab2.Move(0.1f);
                float length = b.p1.x - a.p1.x;
                Vector2 center = new Vector2((a.p1.x + b.p1.x) * 0.5f, (a.p1.y + a.p2.y) * 0.5f);

                // 找寻墙体
                Wall wall = doc.CurrentFloor.Walls.Min(x => x.ToSegment2(x.Corner0).GetDistanceToPoint(center));
                if (wall == null)
                    continue;

                // 墙体上的点
                Vector3 point = wall.ToLine2(wall.Corner0).ClosestPointOnLine(center).XOZ();

                // 门
                int ab1_cross_door_count = 0;
                int ab2_cross_door_count = 0;
                List<Vector2> ab1_cross_points = new List<Vector2>();
                List<Vector2> ab2_cross_points = new List<Vector2>();
                foreach (var arc in door_arcs)
                {
                    int state = GeometryUtils.SegmentIntersectArc(ab1, arc, out List<Vector2> result1);
                    if (state == 1)
                    {
                        ab1_cross_points.AddRange(result1);
                        ab1_cross_door_count++;
                    }

                    state = GeometryUtils.SegmentIntersectArc(ab2, arc, out List<Vector2> result2);
                    if (state == 1)
                    {
                        ab2_cross_points.AddRange(result2);
                        ab2_cross_door_count++;
                    }
                }

                // 房间类型
                DoorType doorType = DoorType.Single;
                // 翻转
                int flip = 0;
                // 判断是否门
                if (ab1_cross_door_count == 1) // 单开门
                {
                    doorType = DoorType.Single;
                    if (ab1_cross_points[0].x - ab1.p1.x > ab1.p2.x - ab1_cross_points[0].x)
                    {
                        flip = 1;
                    }
                    else
                    {
                        flip = 2;
                    }
                }
                else if (ab1_cross_door_count == 2) // 双开门
                {
                    doorType = DoorType.Double;
                    flip = 1;
                }
                else if (ab2_cross_door_count == 1) // 单开门
                {
                    doorType = DoorType.Single;
                    if (ab2_cross_points[0].x - ab2.p1.x > ab2.p2.x - ab2_cross_points[0].x)
                    {
                        flip = 0;
                    }
                    else
                    {
                        flip = 3;
                    }
                }
                else if (ab2_cross_door_count == 2) // 双开门
                {
                    doorType = DoorType.Double;
                    flip = 0;
                }
                else if (ab1_cross_door_count == 4
                    && ab2_cross_door_count == 4) // 旋转门
                {
                    doorType = DoorType.Revolving;
                }

                if (ab1_cross_door_count > 0 || ab2_cross_door_count > 0)
                {
                    Door door = new Door();
                    door.DoorType = doorType;
                    door.Flip = flip;
                    door.Length = length;
                    door.Height = ArchitectSettings.Door.height;
                    door.Thickness = ArchitectSettings.Door.thickness;
                    door.Bottom = ArchitectSettings.Door.bottom;
                    door.Position = new Vector3(point.x, wall.Corner0.Position.y + door.Bottom + door.Height * 0.5f, point.z);
                    door.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                    door.Wall = wall;
                    doc.CurrentFloor.Doors.Add(door);
                    continue;
                }

                // 窗子
                int cross_window_count = 0;
                Segment2 cross_segment;
                foreach (var segment in window_segments)
                {
                    if (segment.Overlap(ab1, out cross_segment))
                    {
                        if (MathUtility.Appr(cross_segment.magnitude, length))
                        {
                            cross_window_count++;
                        }
                    }

                    if (segment.Overlap(ab2, out cross_segment))
                    {
                        if (MathUtility.Appr(cross_segment.magnitude, length))
                        {
                            cross_window_count++;
                        }
                    }
                }

                // 判断是否窗子
                if (cross_window_count == 2)
                {
                    Window window = new Window();
                    window.Length = length;
                    window.Height = ArchitectSettings.Window.height;
                    window.Thickness = ArchitectSettings.Window.thickness;
                    window.Bottom = ArchitectSettings.Window.bottom;
                    window.Position = new Vector3(point.x, wall.Corner0.Position.y + window.Bottom + window.Height * 0.5f, point.z);
                    window.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                    window.Wall = wall;
                    doc.CurrentFloor.Windows.Add(window);
                    // TODO
                    continue;
                }

                // 垭口
                Pass pass = new Pass();
                pass.Length = length;
                pass.Height = ArchitectSettings.Pass.height;
                pass.Thickness = ArchitectSettings.Pass.thickness;
                pass.Bottom = ArchitectSettings.Pass.bottom;
                pass.Position = new Vector3(point.x, wall.Corner0.Position.y + pass.Bottom + pass.Height * 0.5f, point.z);
                pass.Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(wall.Corner0).XOZ());
                pass.Wall = wall;
                doc.CurrentFloor.Passes.Add(pass);
            }

            // 查询房间
            ArchitectUtility.SearchRoom(doc.CurrentFloor.Walls, out List<Room> rooms);
            foreach (var room in rooms)
            {
                if (room.Area < room_area_threshold)
                {
                    room.Active = false;
                }
                // 添加房间
                doc.CurrentFloor.Rooms.Add(room);

                List<List<Vector2>> contours = new List<List<Vector2>>();
                contours.Add(room.Contour);
                contours.AddRange(room.InnerContours);

                string roomName = string.Empty;
                ComplexPolygon2 polygon = new ComplexPolygon2(contours);
                foreach (var text in roomNameTexts)
                {
                    Vector2 pos = new Vector2((float)text.Position.X / 1000, (float)text.Position.Y / 1000);
                    if (polygon.Inside(pos))
                    {
                        roomName = text.Value;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(roomName))
                {
                    room.Name = roomName;
                }
            }

            stopwatch.Stop();
            Debug.LogFormat("--- 将 dxf 转换成 arch，耗时：{0}【毫秒】---", stopwatch.ElapsedMilliseconds);
            // 返回
            return doc;
        }
    }
}
