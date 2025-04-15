using System.Collections.Generic;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Room : EntityObject
    {
        public override EntityType Type
        {
            get { return EntityType.Room; }
        }

        public float Area
        {
            get
            {
                float s = Polygon2.CalculateArea(Contour);
                foreach (List<Vector2> contour in InnerContours)
                {
                    s += Polygon2.CalculateArea(contour);
                }
                //Debug.Log($"<color=#00ffffff>计算面积{Mathf.Abs(s).ToString("F0")}</color>");
                return Mathf.Abs(s);
            }
        }

        private ObservableCollection<Wall> m_Walls = new ObservableCollection<Wall>();

        public ObservableCollection<Wall> Walls
        {
            get { return m_Walls; }
        }

        private ObservableCollection<Wall> m_InnerWalls = new ObservableCollection<Wall>();
        /// <summary>
        /// 内部墙体列表
        /// </summary>
        public ObservableCollection<Wall> InnerWalls
        {
            get { return m_InnerWalls; }
        }

        public List<Vector2> OutContour { get; set; }

        private List<Vector2> m_Contour;
        /// <summary>
        /// 轮廓
        /// </summary>
        public List<Vector2> Contour
        {
            get { return m_Contour; }
            set { m_Contour = value; }
        }

        private List<List<Vector2>> m_InnerContours = new List<List<Vector2>>();
        /// <summary>
        /// 内部轮廓
        /// </summary>
        public List<List<Vector2>> InnerContours
        {
            get { return m_InnerContours; }
            set { m_InnerContours = value; }
        }

        public Vector3 GetVisualPoint()
        {
            List<List<Vector2>> contours = new List<List<Vector2>>();
            contours.Add(Contour);
            foreach (List<Vector2> contour in InnerContours)
            {
                contours.Add(contour);
            }
            Vector2 visualPoint = new ComplexPolygon2(contours).GetVisualPoint(1f);
            return visualPoint.XOZ();
        }

        public Room()
        {
            Special = "建筑";
            Name = "未命名";
        }

        public Room(List<Wall> walls, List<Wall> innerWalls)
        {
            Special = "建筑";
            Name = "未命名";

            SetWalls(walls, innerWalls);
        }

        public void SetWalls(List<Wall> walls, List<Wall> innerWalls)
        {
            this.Walls.AddRange(walls);
            this.InnerWalls.AddRange(innerWalls);

            foreach (var wall in walls)
            {
                wall.AddRoom(this, 1);
            }

            foreach (var wall in innerWalls)
            {
                wall.AddRoom(this, 0);
            }

            m_Walls.CollectionChanged += Walls_CollectionChanged;
            m_InnerWalls.CollectionChanged += InnerWalls_CollectionChanged;
        }

        public bool Contains(Wall wall)
        {
            bool flag = false;
            flag = m_Walls.Contains(wall);
            if (flag)
                return false;
            else
                return m_InnerWalls.Contains(wall);
        }

        private void Walls_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        Wall wall = (Wall)item;
                        wall.AddRoom(this, 1);
                    }
                    break;
                case CollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Wall wall = (Wall)item;
                        wall.RemoveRoom(this);
                    }
                    break;
                case CollectionChangedAction.Replace:
                    break;
                case CollectionChangedAction.Move:
                    break;
                case CollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
            SyncContour();
        }

        private void InnerWalls_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        Wall wall = (Wall)item;
                        wall.AddRoom(this, 0);
                    }
                    break;
                case CollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Wall wall = (Wall)item;
                        wall.RemoveRoom(this);
                    }
                    break;
                case CollectionChangedAction.Replace:
                    break;
                case CollectionChangedAction.Move:
                    break;
                case CollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
            SyncInnerContour();
        }

        public void SyncContour()
        {
            List<Segment2> segments = ListPool<Segment2>.Get();
            foreach (Wall wall in m_Walls)
            {
                foreach (Segment2 segment in wall.Segments)
                {
                    segments.Add(segment);
                }
            }

            List<List<Vector2>> innerContours = ListPool<List<Vector2>>.Get();
            int i = segments.Count - 1;
            while (i >= 0)
            {
                List<Segment2> loop = ListPool<Segment2>.Get();
                Segment2 origin = segments[i];
                segments.RemoveAt(i);
                loop.Add(origin);
                DeepSyncContour(origin, segments, loop);
                int count = loop.Count;
                i -= count;
                if (loop.Count < 3 || !loop[0].OverlapPoint(loop[count - 1]))
                {
                    continue;
                }

                List<Vector2> contour = new List<Vector2>();
                for (int p = 0, q = loop.Count - 1; p < loop.Count; q = p++)
                {
                    Segment2 prevSegment = loop[q];
                    Segment2 segment = loop[p];

                    Vector2 point;
                    if (prevSegment.OverlapPoint(segment, out point))
                    {
                        contour.Add(point);
                    }
                }
                // 判断墙角点，是否在轮廓之外。
                bool outside = false;
                Polygon2 polygon = new Polygon2(contour);
                foreach (var wall in m_Walls)
                {
                    Vector2 corner = wall.Corner0.ToPoint2();
                    if (polygon.Outside(corner))
                    {
                        outside = true;
                        break;
                    }
                }
                if (outside)
                {
                    if (polygon.Area < 0f)
                    {
                        contour.Reverse();
                    }
                    innerContours.Add(contour);
                }
                else // 外轮廓只有一个
                {

                }
                // 释放
                ListPool<Segment2>.Release(loop);
            }

            // 轮廓 可能多个内轮廓
            float s = 0;
            foreach (var inner in innerContours)
            {
                float temp = Polygon2.CalculateArea(inner);
                if (temp >= s)
                {
                    m_Contour = inner;
                    s = temp;
                }
            }

            // 释放
            ListPool<List<Vector2>>.Release(innerContours);
            ListPool<Segment2>.Release(segments);
            // Changed
            OnTransformChanged();
            OnVerticesChanged();
        }

        // 线段索引与墙的对应关系
        private readonly Dictionary<Segment2, Wall> m_SegmentWallMap = new Dictionary<Segment2, Wall>();

        public void SyncInnerContour()
        {
            // 线段索引与墙的对应关系
            m_SegmentWallMap.Clear();
            InnerContours.Clear();

            // 线段列表
            List<Segment2> segments = ListPool<Segment2>.Get();
            foreach (var wall in this.InnerWalls)
            {
                foreach (var segment in wall.Segments)
                {
                    segments.Add(segment);
                    m_SegmentWallMap.Add(segment, wall);
                }
            }

            // 查询的房间列表
            for (int i = segments.Count - 1; i >= 0;)
            {
                List<Segment2> loop = ListPool<Segment2>.Get();
                Segment2 origin = segments[i];
                segments.RemoveAt(i);
                loop.Add(origin);
                DeepSyncContour(origin, segments, loop);

                int count = loop.Count;
                i -= count;
                if (loop.Count < 3 || !loop[0].OverlapPoint(loop[count - 1]))
                    continue;

                //HashSet<>
                List<Vector2> contour = new List<Vector2>();
                for (int p = 0, q = loop.Count - 1; p < loop.Count; q = p++)
                {
                    Segment2 prevSegment = loop[q];
                    Segment2 segment = loop[p];

                    Vector2 point;
                    if (prevSegment.OverlapPoint(segment, out point))
                    {
                        contour.Add(point);
                    }
                }

                // 判断墙角点，是否在轮廓之外。
                Polygon2 polygon = new Polygon2(contour);
                bool outside = false;
                foreach (var segment in loop)
                {
                    Vector2 corner = m_SegmentWallMap[segment].Corner0.ToPoint2();
                    if (polygon.Outside(corner))
                    {
                        outside = true;
                        break;
                    }
                }
                if (!outside)
                {
                    if (polygon.Area > 0f)
                    {
                        contour.Reverse();
                    }
                    InnerContours.Add(contour);
                }

                // 回收
                ListPool<Segment2>.Release(loop);
            }

            ListPool<Segment2>.Release(segments);

            // Changed
            OnTransformChanged();
            OnVerticesChanged();
        }

        public void DeepSyncContour(Segment2 origin, List<Segment2> segments, List<Segment2> loop)
        {
            Segment2 last = loop[loop.Count - 1];
            int index = -1;
            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i].OverlapPoint(last))
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                Segment2 segment = segments[index];
                segments.RemoveAt(index);
                loop.Add(segment);
                if (loop.Count < 3 || !origin.OverlapPoint(segment))
                {
                    DeepSyncContour(origin, segments, loop);
                }
            }
        }

        public bool Overlap(Room other, out float ratio)
        {
            //return Polygon2.Overlap(Contour, other.Contour, out ratio);
            ratio = Mathf.Abs(Polygon2.CalculateArea(Contour) / Polygon2.CalculateArea(other.Contour));

            bool flag = true;
            int insideCount = 0;
            if (ratio <= 1)
            {
                foreach (var point in Contour)
                {
                    int side = Polygon2.Side(point, other.Contour);
                    if (side == 1)
                    {
                        flag = false;
                        break;
                    }
                    else if (side == 0)
                    {
                        insideCount++;
                    }
                }
            }
            else
            {
                foreach (var point in other.Contour)
                {
                    int side = Polygon2.Side(point, Contour);
                    if (side == 1)
                    {
                        flag = false;
                        break;
                    }
                    else if (side == 0)
                    {
                        insideCount++;
                    }
                }
            }

            if (insideCount < 2)
            {
                flag = false;
            }

            return flag;

        }

        /// <summary>
        /// 更新到目标房间
        /// </summary>
        /// <param name="target"></param>
        public void Merge(Room target)
        {
            List<Wall> results = ListPool<Wall>.Get();
            // 合并墙体
            this.Walls.Intersect(target.Walls, ref results);
            // 移除墙体
            for (int i = this.Walls.Count - 1; i >= 0; i--)
            {
                var item = this.Walls[i];
                if (!results.Contains(item))
                {
                    this.Walls.Remove(item);
                    item.RemoveRoom(this);
                }
            }
            // 添加墙体
            for (int i = target.Walls.Count - 1; i >= 0; i--)
            {
                var item = target.Walls[i];
                if (!results.Contains(item))
                {
                    this.Walls.Add(item);
                    item.AddRoom(this);
                }
            }
            // 轮廓
            this.Contour = target.Contour;

            results.Clear();
            // 合并内部墙体
            this.InnerWalls.Intersect(target.InnerWalls, ref results);
            // 移除墙体
            for (int i = this.InnerWalls.Count - 1; i >= 0; i--)
            {
                var item = this.InnerWalls[i];
                if (!results.Contains(item))
                {
                    this.InnerWalls.Remove(item);
                    item.RemoveRoom(this);
                }
            }
            // 添加墙体
            for (int i = target.InnerWalls.Count - 1; i >= 0; i--)
            {
                var item = target.InnerWalls[i];
                if (!results.Contains(item))
                {
                    this.InnerWalls.Add(item);
                    item.AddRoom(this);
                }
            }
            // 内部轮廓
            this.InnerContours.Clear();
            this.InnerContours.AddRange(target.InnerContours);

            // 释放
            ListPool<Wall>.Release(results);
        }

        public override object Clone()
        {
            //List<Wall> cloneWalls = new List<Wall>();
            //foreach (var wall in Walls)
            //{
            //    cloneWalls.Add((Wall)wall.Clone());
            //}

            //List<Wall> cloneInnerWalls = new List<Wall>();
            //foreach (var wall in InnerWalls)
            //{
            //    cloneInnerWalls.Add((Wall)wall.Clone());
            //}

            Room entity = new Room()
            {
                Active = this.Active,
                Name = this.Name,
                m_Contour = this.m_Contour,
                m_InnerContours = this.m_InnerContours,
            };

            return entity;
        }
    }
}                                                  