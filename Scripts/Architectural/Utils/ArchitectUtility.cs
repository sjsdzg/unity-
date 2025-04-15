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
    public class SegmentCastWallHit
    {
        public Wall Wall { get; set; }
        public Vector2 Point { get; set; }
    }

    public class ArchitectUtility
    {
        /// <summary>
        /// 沿着线段投射墙体，返回被击中墙体的信息
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="walls"></param>
        /// <returns></returns>
        public static List<SegmentCastWallHit> SegmentCastAllWalls(Segment2 origin, List<Wall> walls)
        {
            List<SegmentCastWallHit> hits = new List<SegmentCastWallHit>();
            Segment2 segment = new Segment2();
            foreach (var wall in walls)
            {
                segment.p1 = wall.Corner0.ToPoint2();
                segment.p2 = wall.Corner1.ToPoint2();
                if (origin.Intersect(segment, out Vector2 point))
                {
                    SegmentCastWallHit hit = new SegmentCastWallHit();
                    hit.Wall = wall;
                    hit.Point = point;
                    hits.Add(hit);
                }
            }

            return hits;
        }

        /// <summary>
        /// 添加墙体的处理方法
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="wall"></param>
        public static void AddWallHandler(Floor floor, Wall target)
        {
            List<Vector2> points = ListPool<Vector2>.Get();
            List<Wall> addWalls = ListPool<Wall>.Get();
            List<Wall> deleteWalls = ListPool<Wall>.Get();
            // 交叉
            Segment2 origin = target.ToSegment2(target.Corner0);
            List<SegmentCastWallHit> hits = SegmentCastAllWalls(origin, floor.Walls);
            foreach (var hit in hits)
            {
                hit.Wall.Split(new List<Vector2>() { hit.Point }, out List<Wall> hitWalls);
                if (hitWalls.Count > 0)
                {
                    addWalls.AddRange(hitWalls);
                    deleteWalls.Add(hit.Wall);
                }
                points.Add(hit.Point);
            }
            // 重合
            Segment2 segment = new Segment2();
            foreach (var wall in floor.Walls)
            {
                segment.p1 = wall.Corner0.ToPoint2();
                segment.p2 = wall.Corner1.ToPoint2();
                if (origin.Overlap(segment, out Segment2 seg))
                {
                    wall.Split(new List<Vector2>() { seg.p1, seg.p2 }, out List<Wall> overlapWalls);
                    if (overlapWalls.Count > 0)
                    {
                        addWalls.AddRange(overlapWalls);
                        deleteWalls.Add(wall); 
                    }
                    points.Add(seg.p1);
                    points.Add(seg.p2);
                }
            }
            // 移除墙体
            foreach (var wall in deleteWalls)
            {
                floor.Walls.Remove(wall);
                GraphicManager.DestoryGraphic(wall);
            }
            // 添加墙体
            foreach (var wall in addWalls)
            {
                floor.Walls.Add(wall);
                GraphicManager.CreateGraphic(wall);
            }
            // 墙体 target 拆分
            addWalls.Clear();
            target.Split(points, out List<Wall> walls);
            if (walls.Count == 0)
            {
                addWalls.Add(target);
            }
            else
            {
                addWalls.AddRange(walls);
            }
            // 添加墙体
            foreach (var wall in addWalls)
            {
                if (floor.TryGetWall(wall.Corner0, wall.Corner1, out Wall result))
                {
                    wall.Corner0.RemoveWall(wall);
                    wall.Corner1.RemoveWall(wall);
                }
                else
                {
                    floor.Walls.Add(wall);
                    GraphicManager.CreateGraphic(wall);
                }
            }
            // 释放
            ListPool<Vector2>.Release(points);
            ListPool<Wall>.Release(addWalls);
            ListPool<Wall>.Release(deleteWalls);
        }

        // 线段索引与墙的对应关系
        private static Dictionary<Segment2, Wall> m_SegmentWallMap = new Dictionary<Segment2, Wall>();
        // 线段索引与墙的对应关系
        private static Dictionary<List<Vector2>, List<Wall>> m_ContourWallMap = new Dictionary<List<Vector2>, List<Wall>>();

        /// <summary>
        /// 寻找闭合空间（即房间）
        /// </summary>
        public static void SearchAndMergeRoom(Floor floor)
        {
            //// 线段索引与墙的对应关系
            //m_SegmentWallMap.Clear();
            //m_ContourWallMap.Clear();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //// 线段列表
            //List<Segment2> segments = ListPool<Segment2>.Get();
            //foreach (var wall in floor.Walls)
            //{
            //    foreach (var segment in wall.Segments)
            //    {
            //        try
            //        {
            //            segments.Add(segment);
            //            m_SegmentWallMap.Add(segment, wall);
            //        }
            //        catch (Exception ex)
            //        {
            //            Debug.Log("wall id : " + wall.Id);
            //        }
            //    }
            //}
            //// 查询轮廓
            //List<Room> searchRooms = ListPool<Room>.Get();
            //List<List<Vector2>> innerContours = ListPool<List<Vector2>>.Get();
            //List<List<Vector2>> outerContours = ListPool<List<Vector2>>.Get();
            //int i = segments.Count - 1;
            //while (i >= 0)
            //{
            //    List<Segment2> loop = ListPool<Segment2>.Get();
            //    Segment2 origin = segments[i];
            //    segments.RemoveAt(i);
            //    loop.Add(origin);
            //    DeepSearchRoom(origin, segments, loop);

            //    int loop_count = loop.Count;
            //    i -= loop_count;

            //    if (loop_count < 3 || !loop[0].OverlapPoint(loop[loop_count - 1]))
            //        continue;

            //    //HashSet<>
            //    List<Vector2> contour = new List<Vector2>();
            //    List<Wall> walls = new List<Wall>();
            //    for (int p = 0, q = loop_count - 1; p < loop_count; q = p++)
            //    {
            //        Segment2 prevSegment = loop[q];
            //        Segment2 segment = loop[p];

            //        Vector2 point;
            //        if (prevSegment.OverlapPoint(segment, out point))
            //        {
            //            contour.Add(point);
            //        }

            //        Wall wall = m_SegmentWallMap[segment];
            //        if (!walls.Contains(wall))
            //        {
            //            walls.Add(wall);
            //        }
            //    }

            //    // 判断墙角点，是否在轮廓之外。
            //    bool outside = false;
            //    foreach (var wall in walls)
            //    {
            //        Vector2 corner = wall.Corner0.ToPoint2();
            //        if (Polygon2.Outside(corner, contour))
            //        {
            //            outside = true;
            //            break;
            //        }
            //    }
            //    if (outside)
            //    {
            //        innerContours.Add(contour);
            //        m_ContourWallMap.Add(contour, walls);
            //    }
            //    else
            //    {
            //        outerContours.Add(contour);
            //        m_ContourWallMap.Add(contour, walls);
            //    }

            //    // 回收
            //    ListPool<Segment2>.Release(loop);
            //}

            //// 查找房间
            //foreach (var inner in innerContours)
            //{
            //    // 内轮廓包含的所有外轮廓列表
            //    List<List<Vector2>> contains = ListPool<List<Vector2>>.Get();
            //    foreach (var outerContour in outerContours)
            //    {
            //        if (Polygon2.Inside(outerContour[0], inner))
            //        {
            //            contains.Add(outerContour);
            //        }
            //    }

            //    // 内轮廓包含的有效外轮廓列表
            //    for (int index = contains.Count - 1; index >= 0; index--)
            //    {
            //        // 原轮廓
            //        List<Vector2> origin = contains[index];
            //        // 是否存在包围原轮廓的轮廓
            //        bool inside = false;
            //        for (int j = 0; j < contains.Count; j++)
            //        {
            //            List<Vector2> contour = contains[j];
            //            if (origin.Equals(contour))
            //                continue;

            //            if (Polygon2.Inside(origin[0], contour))
            //            {
            //                inside = true;
            //                break;
            //            }
            //        }
            //        // 存在
            //        if (inside)
            //        {
            //            contains.RemoveAt(index);
            //        }
            //    }

            //    // 生成房间
            //    List<Wall> innerWalls = new List<Wall>();
            //    foreach (var contour in contains)
            //    {
            //        innerWalls.AddRange(m_ContourWallMap[contour]);
            //    }

            //    Room room = new Room(m_ContourWallMap[inner], innerWalls);
            //    room.Contour = inner;
            //    room.InnerContours.Clear();
            //    room.InnerContours.AddRange(contains);

            //    searchRooms.Add(room);
            //    // 释放
            //    ListPool<List<Vector2>>.Release(contains);
            //}

            // 查找房间
            SearchRoom(floor.Walls, out List<Room> searchRooms);
            // copy
            List<Room> originRooms = ListPool<Room>.Get();
            foreach (var item in floor.Rooms)
            {
                originRooms.Add(item);
            }
            // 合并房间
            MergeRoom(searchRooms, originRooms);
            // 释放
            ListPool<Room>.Release(searchRooms);
            ListPool<Room>.Release(originRooms);

            stopwatch.Stop();
            Debug.Log("房间数量 : " + searchRooms.Count);
            Debug.LogFormat("---寻找闭合空间【即房间】，耗时：{0}【毫秒】", stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// 查找房间
        /// </summary>
        /// <param name="wallList"></param>
        /// <param name="searchRooms"></param>
        public static void SearchRoom(List<Wall> wallList, out List<Room> searchRooms)
        {
            // 线段索引与墙的对应关系
            m_SegmentWallMap.Clear();
            m_ContourWallMap.Clear();

            // 线段列表
            List<Segment2> segments = ListPool<Segment2>.Get();
            foreach (var wall in wallList)
            {
                foreach (var segment in wall.Segments)
                {
                    try
                    {
                        segments.Add(segment);
                        m_SegmentWallMap.Add(segment, wall);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("wall id : " + wall.Id);
                    }
                }
            }
            // 查询轮廓
            searchRooms = ListPool<Room>.Get();
            List<List<Vector2>> innerContours = ListPool<List<Vector2>>.Get();
            List<List<Vector2>> outerContours = ListPool<List<Vector2>>.Get();
            int i = segments.Count - 1;
            while (i >= 0)
            {
                List<Segment2> loop = ListPool<Segment2>.Get();
                Segment2 origin = segments[i];
                segments.RemoveAt(i);
                loop.Add(origin);
                DeepSearchRoom(origin, segments, loop);

                int loop_count = loop.Count;
                i -= loop_count;

                if (loop_count < 3 || !loop[0].OverlapPoint(loop[loop_count - 1]))
                    continue;

                //HashSet<>
                List<Vector2> contour = new List<Vector2>();
                List<Wall> walls = new List<Wall>();
                for (int p = 0, q = loop_count - 1; p < loop_count; q = p++)
                {
                    Segment2 prevSegment = loop[q];
                    Segment2 segment = loop[p];

                    Vector2 point;
                    if (prevSegment.OverlapPoint(segment, out point))
                    {
                        contour.Add(point);
                    }

                    Wall wall = m_SegmentWallMap[segment];
                    if (!walls.Contains(wall))
                    {
                        walls.Add(wall);
                    }
                }

                // 判断墙角点，是否在轮廓之外。
                bool outside = false;
                foreach (var wall in walls)
                {
                    Vector2 corner = wall.Corner0.ToPoint2();
                    if (Polygon2.Outside(corner, contour))
                    {
                        outside = true;
                        break;
                    }
                }
                if (outside)
                {
                    innerContours.Add(contour);
                    m_ContourWallMap.Add(contour, walls);
                }
                else
                {
                    outerContours.Add(contour);
                    m_ContourWallMap.Add(contour, walls);
                }

                // 回收
                ListPool<Segment2>.Release(loop);
            }

            // 查找房间
            foreach (var inner in innerContours)
            {
                // 内轮廓包含的所有外轮廓列表
                List<List<Vector2>> contains = ListPool<List<Vector2>>.Get();
                foreach (var outerContour in outerContours)
                {
                    if (Polygon2.Inside(outerContour[0], inner))
                    {
                        contains.Add(outerContour);
                    }
                }

                // 内轮廓包含的有效外轮廓列表
                for (int index = contains.Count - 1; index >= 0; index--)
                {
                    // 原轮廓
                    List<Vector2> origin = contains[index];
                    // 是否存在包围原轮廓的轮廓
                    bool inside = false;
                    for (int j = 0; j < contains.Count; j++)
                    {
                        List<Vector2> contour = contains[j];
                        if (origin.Equals(contour))
                            continue;

                        if (Polygon2.Inside(origin[0], contour))
                        {
                            inside = true;
                            break;
                        }
                    }
                    // 存在
                    if (inside)
                    {
                        contains.RemoveAt(index);
                    }
                }

                // 生成房间
                List<Wall> innerWalls = new List<Wall>();
                foreach (var contour in contains)
                {
                    innerWalls.AddRange(m_ContourWallMap[contour]);
                }
                Debug.LogError("生成房间");
                Room room = new Room(m_ContourWallMap[inner], innerWalls);
                room.Contour = inner;
                room.InnerContours.Clear();
                room.InnerContours.AddRange(contains);

                searchRooms.Add(room);
                // 释放
                ListPool<List<Vector2>>.Release(contains);
            }

            m_SegmentWallMap.Clear();
            m_ContourWallMap.Clear();
            // 回收对象到对象池
            ListPool<Segment2>.Release(segments);
            ListPool<List<Vector2>>.Release(innerContours);
            ListPool<List<Vector2>>.Release(outerContours);
        }

        /// <summary>
        /// 递归查找房间
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="segments"></param>
        /// <param name="loop"></param>
        public static void DeepSearchRoom(Segment2 origin, List<Segment2> segments, List<Segment2> loop)
        {
            Segment2 last = loop[loop.Count - 1];
            int index = -1;
            for (int i = segments.Count - 1; i >= 0; i--)
            {
                if (segments[i].OverlapPoint(last))
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                // 轮廓末尾有重合点的线段
                Segment2 segment = segments[index];
                segments.RemoveAt(index);
                loop.Add(segment);

                if (loop.Count < 3 || !origin.OverlapPoint(segment))
                {
                    DeepSearchRoom(origin, segments, loop);
                }
            }
        }

        /// <summary>
        /// 合并房间
        /// </summary>
        /// <param name="searchRooms"></param>
        /// <param name="originRooms"></param>
        public static void MergeRoom(List<Room> searchRooms, List<Room> originRooms)
        {
            List<IndexFloat> indexFloats = ListPool<IndexFloat>.Get();

            int index = -1;
            // 拆分房间
            for (int i = originRooms.Count - 1; i >= 0; i--)
            {
                bool equal = false;
                indexFloats.Clear();
                Room originRoom = originRooms[i];

                for (int j = 0; j < searchRooms.Count; j++)
                {
                    indexFloats.Clear();
                    Room searchRoom = searchRooms[j];
                    if (searchRoom.Overlap(originRoom, out float ratio))
                    {
                        // 和原始房间一致
                        if (MathUtility.Appr(ratio, 1))
                        {
                            index = j;
                            equal = true;
                            break;
                        }
                        // 拆分原始房间
                        else if (ratio < 1)
                        {
                            IndexFloat item = new IndexFloat();
                            item.index = j;
                            item.value = ratio;
                            indexFloats.Add(item);
                        }
                    }
                }

                // 排序 ：从小到大
                if (equal)
                {
                    Room searchRoom = searchRooms[index];
                    // 更新到目标房间
                    originRoom.Merge(searchRoom);
                    // 移除
                    originRooms.RemoveAt(i);
                    searchRooms.RemoveAt(index);
                }
                else if (indexFloats.Count > 0)
                {
                    indexFloats.Sort();
                    index = indexFloats[indexFloats.Count - 1].index;
                    Room searchRoom = searchRooms[index];
                    // 更新到目标房间
                    originRoom.Merge(searchRoom);
                    // 移除
                    originRooms.RemoveAt(i);
                    searchRooms.RemoveAt(index);
                }
            }

            // 合并房间
            for (int i = searchRooms.Count - 1; i >= 0; i--)
            {
                indexFloats.Clear();
                Room searchRoom = searchRooms[i];

                for (int j = 0; j < originRooms.Count; j++)
                {
                    Room originRoom = originRooms[j];
                    if (originRoom.Overlap(searchRoom, out float ratio))
                    {
                        if (ratio < 1)
                        {
                            IndexFloat item = new IndexFloat();
                            item.index = j;
                            item.value = ratio;
                            indexFloats.Add(item);
                        }
                    }
                }

                if (indexFloats.Count > 0)
                {
                    indexFloats.Sort();
                    index = indexFloats[indexFloats.Count - 1].index;
                    Room originRoom = originRooms[index];
                    // 更新到目标房间
                    originRoom.Merge(searchRoom);
                    // 移除
                    searchRooms.RemoveAt(i);
                    originRooms.RemoveAt(index);
                }
            }

            // 添加房间
            foreach (var room in searchRooms)
            {
                Architect.AddEntity(room);
                GraphicManager.CreateGraphic(room);
            }

            // 删除房间
            foreach (var room in originRooms)
            {
                Architect.RemoveEntity(room);
                GraphicManager.DestoryGraphic(room);
            }

            // 释放
            ListPool<IndexFloat>.Release(indexFloats);
        }

        /// <summary>
        /// 查找轮廓
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="contours"></param>
        public static void SearchContour(List<Segment2> segments, List<List<Vector2>> contours)
        {
            int i = segments.Count - 1;
            while (i >= 0)
            {
                List<Segment2> loop = ListPool<Segment2>.Get();
                Segment2 origin = segments[i];
                segments.RemoveAt(i);
                loop.Add(origin);
                DeepSearchContour(origin, segments, loop);
                int count = loop.Count;
                i -= loop.Count;
                if (loop.Count < 3 || !loop[0].OverlapPoint(loop[count - 1]))
                    continue;

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
                // add
                contours.Add(contour);
                // 释放
                ListPool<Segment2>.Release(loop);
            }
        }

        public static void DeepSearchContour(Segment2 origin, List<Segment2> segments, List<Segment2> loop)
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

            // 轮廓末尾有重合点的线段
            Segment2 segment = segments[index];
            segments.RemoveAt(index);
            loop.Add(segment);

            if (loop.Count < 3 || !origin.OverlapPoint(segment))
            {
                DeepSearchContour(origin, segments, loop);
            }
        }

        /// <summary>
        /// Corner 相同
        /// </summary>
        /// <param name="corner0"></param>
        /// <param name="corner1"></param>
        /// <returns></returns>
        public static bool IsSame(Corner corner0, Corner corner1)
        {
            return MathUtility.Appr(corner0.Position, corner1.Position);
        }

        /// <summary>
        /// Wall 相同
        /// </summary>
        /// <param name="wall0"></param>
        /// <param name="wall1"></param>
        /// <returns></returns>
        public static bool IsSame(Wall wall0, Wall wall1)
        {
            return IsSame(wall0.Corner0, wall1.Corner0) && IsSame(wall0.Corner1, wall1.Corner1);
        }

        /// <summary>
        /// 创建 group 图元
        /// </summary>
        /// <param name="group"></param>
        public static void RegisterGroupForCreate(Group group, bool allMembers = true)
        {
            if (allMembers)
            {
                foreach (var member in group.Members)
                {
                    if (member.Entity.Type == EntityType.Corner)
                        continue;

                    GraphicManager.CreateGraphic(member.Entity);
                }
            }
            GraphicManager.CreateGraphic(group);
        }

        /// <summary>
        /// 删除 group 图元
        /// </summary>
        /// <param name="group"></param>
        public static void RegisterGroupForDestory(Group group, bool allMembers = true)
        {
            if (allMembers)
            {
                foreach (var member in group.Members)
                {
                    GraphicManager.DestoryGraphic(member.Entity);
                }
            }
            GraphicManager.DestoryGraphic(group);
        }

        /// <summary>
        /// 创建 group 图元
        /// </summary>
        /// <param name="group"></param>
        public static void AddGroup(Floor floor, Group group, bool allMembers = true)
        {
            if (allMembers)
            {
                foreach (var member in group.Members)
                {
                    floor.AddEntity(member.Entity);
                }
            }
            floor.AddEntity(group);
        }

        /// <summary>
        /// 删除 group 图元
        /// </summary>
        /// <param name="group"></param>
        public static void RemoveGroup(Floor floor, Group group, bool allMembers = true)
        {
            if (allMembers)
            {
                foreach (var member in group.Members)
                {
                    floor.RemoveEntity(member.Entity);
                }
            }
            floor.RemoveEntity(group);
        }

        private static readonly Dictionary<Wall, bool> m_RawWallOverlaps = new Dictionary<Wall, bool>();
        private static readonly Dictionary<Wall, List<Vector2>> m_RawWallPoints = new Dictionary<Wall, List<Vector2>>();

        public static void AddGroupHandler(Floor floor, Group group)
        {
            m_RawWallOverlaps.Clear();
            m_RawWallPoints.Clear();
            List<Wall> addWalls = ListPool<Wall>.Get();
            List<Wall> deleteWalls = ListPool<Wall>.Get();

            int rawWallCount = group.RawWalls.Count;

            // 重置 group
            for (int i = 0; i < rawWallCount; i++)
            {
                var rawWall = group.RawWalls[i];
                group.TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls);
                int linkedWallCount = linkedWalls.Count;
                if (linkedWallCount == 1)
                {
                    var linkedWall = linkedWalls[0];
                    if (linkedWall.Corner0.Equals(rawWall.Corner0) && linkedWall.Corner1.Equals(rawWall.Corner1))
                    {
                        continue;
                    }
                }

                List<int> indies = new List<int>();
                for (int j = 0; j < linkedWallCount; j++)
                {
                    var linkedWall = linkedWalls[j];
                    bool isLinked = false;
                    foreach (var groupInFloor in floor.Groups)
                    {
                        if (groupInFloor.Equals(group))
                            continue;

                        if (groupInFloor.ContainsLinkedWall(linkedWall))
                        {
                            isLinked = true;
                            break;
                        }
                    }

                    if (isLinked)
                    {
                        indies.Add(j);
                    }
                }

                for (int j = linkedWallCount - 1; j >= 0; j--)
                {
                    var linkedWall = linkedWalls[j];

                    group.Remove(linkedWall);
                    if (!indies.Contains(j))
                    {
                        floor.RemoveEntity(linkedWall);
                        GraphicManager.DestoryGraphic(linkedWall);
                    }
                }

                group.ClearLinkedWall(rawWall);

                rawWall.Corner0.Active = true;
                rawWall.Corner1.Active = true;

                Wall wall = new Wall(rawWall.Corner0, rawWall.Corner1);
                wall.Thickness = rawWall.Thickness;
                //floor.Walls.Add(wall);
                floor.AddEntity(wall);
                GraphicManager.CreateGraphic(wall);

                group.Add(wall);
            }

            // 和墙体进行相交
            for (int i = 0; i < rawWallCount; i++)
            {
                bool overlap = false;
                addWalls.Clear();
                deleteWalls.Clear();
                List<Vector2> points = new List<Vector2>();

                var rawWall = group.RawWalls[i];
                Segment2 origin = rawWall.ToSegment2(rawWall.Corner0);
                Segment2 segment = new Segment2();

                foreach (var wallInFloor in floor.Walls)
                {
                    if (!wallInFloor.Active)
                        continue;

                    if (group.ContainsLinkedWall(wallInFloor))
                        continue;

                    segment.p1 = wallInFloor.Corner0.ToPoint2();
                    segment.p2 = wallInFloor.Corner1.ToPoint2();
                    if (origin.Overlap(segment, out Segment2 seg))
                    {
                        overlap = true;
                        SplitWall(wallInFloor, new List<Vector2>() { seg.p1, seg.p2 }, out List<Wall> splitWalls);
                        if (splitWalls.Count > 0)
                        {
                            addWalls.AddRange(splitWalls);
                            deleteWalls.Add(wallInFloor);

                            for (int j = wallInFloor.RelatedRooms.Count - 1; j >= 0 ; j--)
                            {
                                var relatedRoom = wallInFloor.RelatedRooms[j];
                                Room room = relatedRoom.Room;

                                if (relatedRoom.Status == 0) //内墙墙
                                {
                                    room.InnerWalls.Remove(wallInFloor);
                                }
                                else if (relatedRoom.Status == 1) //外墙
                                {
                                    room.Walls.Remove(wallInFloor);
                                }

                                foreach (var splitWall in splitWalls)
                                {
                                    if (relatedRoom.Status == 0 && !room.InnerWalls.Contains(splitWall)) //内墙墙
                                    {
                                        room.InnerWalls.Add(splitWall);
                                    }
                                    else if (relatedRoom.Status == 1 && !room.Walls.Contains(splitWall)) //外墙
                                    {
                                        room.Walls.Add(splitWall);
                                    }
                                }
                            }

                            // 遍历
                            foreach (var groupInFloor in floor.Groups)
                            {
                                Wall wallInGroupOfFloor = groupInFloor.GetRawWallByLinkedWall(wallInFloor);
                                if (wallInGroupOfFloor != null)
                                {
                                    groupInFloor.Remove(wallInFloor);

                                    groupInFloor.RemoveLinkedWall(wallInGroupOfFloor, wallInFloor);
                                    groupInFloor.AddRangeLinkedWall(wallInGroupOfFloor, splitWalls);
                                }
                            }


                        }
                        points.Add(seg.p1);
                        points.Add(seg.p2);
                    }
                }

                // 移除墙体
                foreach (var wall in deleteWalls)
                {
                    //wall.Corner0.RemoveWall(wall);
                    //wall.Corner1.RemoveWall(wall);
                    //floor.Walls.Remove(wall);
                    floor.RemoveEntity(wall);
                    GraphicManager.DestoryGraphic(wall);
                }

                // 添加墙体
                foreach (var wall in addWalls)
                {
                    //floor.Walls.Add(wall);
                    floor.AddEntity(wall);
                    GraphicManager.CreateGraphic(wall);
                }

                m_RawWallOverlaps.Add(rawWall, overlap);
                m_RawWallPoints.Add(rawWall, points);
            }

            // 分割墙体
            for (int i = 0; i < rawWallCount; i++)
            {
                var rawWall = group.RawWalls[i];
                List<Vector2> points = m_RawWallPoints[rawWall];
                // 墙体 target 拆分
                if (points.Count > 0)
                {
                    SplitWall(rawWall, points, out List<Wall> splitWalls);
                    if (splitWalls.Count == 0)
                    {
                        Corner corner0 = null;
                        Corner corner1 = null;

                        Architect.TryGetCorner(rawWall.Corner0.Position, out corner0);
                        Architect.TryGetCorner(rawWall.Corner1.Position, out corner1);

                        Wall equalWall = null;
                        group.TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls);
                        foreach (var wall in floor.Walls)
                        {
                            if (corner0 == null)
                                break;

                            if (corner1 == null)
                                break;

                            if (!wall.Active)
                                continue;

                            if (linkedWalls.Contains(wall))
                                continue;

                            if ((corner0.Equals(wall.Corner0) && corner1.Equals(wall.Corner1))
                                || (corner0.Equals(wall.Corner1) && corner1.Equals(wall.Corner0)))
                            {
                                equalWall = wall;
                                break;
                            }
                        }

                        if (equalWall != null)
                        {
                            foreach (var linkedWall in linkedWalls)
                            {
                                //linkedWall.Corner0.RemoveWall(linkedWall);
                                //linkedWall.Corner1.RemoveWall(linkedWall);
                                //floor.Walls.Remove(linkedWall);
                                group.Remove(linkedWall);
                                floor.RemoveEntity(linkedWall);
                                GraphicManager.DestoryGraphic(linkedWall);
                            }
                            group.ClearLinkedWall(rawWall);
                            group.AddLinkedWall(rawWall, equalWall);
                        }
                    }
                    else
                    {
                        if (group.TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls))
                        {
                            foreach (var linkedWall in linkedWalls)
                            {
                                //linkedWall.Corner0.RemoveWall(linkedWall);
                                //linkedWall.Corner1.RemoveWall(linkedWall);
                                //floor.Walls.Remove(linkedWall);
                                group.Remove(linkedWall);
                                floor.RemoveEntity(linkedWall);
                                GraphicManager.DestoryGraphic(linkedWall);
                            }
                        }
                        group.ClearLinkedWall(rawWall);

                        foreach (var splitWall in splitWalls)
                        {
                            Wall equalWall = null;
                            foreach (var wall in floor.Walls)
                            {
                                if (!wall.Active)
                                    continue;

                                if ((splitWall.Corner0.Equals(wall.Corner0) && splitWall.Corner1.Equals(wall.Corner1))
                                    || (splitWall.Corner0.Equals(wall.Corner1) && splitWall.Corner1.Equals(wall.Corner0)))
                                {
                                    equalWall = wall;
                                    break;
                                }
                            }

                            if (equalWall != null)
                            {
                                splitWall.Corner0.RemoveWall(splitWall);
                                splitWall.Corner1.RemoveWall(splitWall);
                                group.AddLinkedWall(rawWall, equalWall);
                            }
                            else
                            {
                                //floor.Walls.Add(splitWall);
                                floor.AddEntity(splitWall);
                                group.AddLinkedWall(rawWall, splitWall);
                                GraphicManager.CreateGraphic(splitWall);
                            }
                        }
                    }
                }
            }

            // 合并墙体
            for (int i = floor.Corners.Count - 1; i >= 0; i--)
            {
                Corner corner = floor.Corners[i];

                if (!corner.Active)
                    continue;

                if (corner.Walls.Count != 2)
                    continue;

                Wall wall0 = corner.Walls[0];
                Wall wall1 = corner.Walls[1];
                Corner corner0 = wall0.Another(corner);
                Corner corner1 = wall1.Another(corner);

                if (MathUtility.Collinear(corner0.Position, corner.Position, corner1.Position))
                {
                    Wall wall = new Wall(corner0, corner1);
                    wall.Height = wall0.Height;
                    wall.Thickness = wall0.Thickness;

                    bool isLinked0 = false;
                    bool isLinked1 = false;
                    bool canMerge = true;

                    // 遍历
                    foreach (var groupInFloor in floor.Groups)
                    {
                        Wall rawWall0 = groupInFloor.GetRawWallByLinkedWall(wall0);
                        Wall rawWall1 = groupInFloor.GetRawWallByLinkedWall(wall1);

                        if (rawWall0 != null)
                        {
                            isLinked0 = true;
                        }

                        if (rawWall1 != null)
                        {
                            isLinked1 = true;
                        }

                        if (!object.Equals(rawWall0, rawWall1))
                        {
                            canMerge = false;
                        }
                    }

                    if (canMerge)
                    {
                        if (!isLinked0 && !isLinked1)
                        {
                            for (int j = wall0.RelatedRooms.Count - 1; j >= 0; j--)
                            {
                                var relatedRoom = wall0.RelatedRooms[j];
                                Room room = relatedRoom.Room;

                                if (relatedRoom.Status == 0 && !room.InnerWalls.Contains(wall)) //内墙墙
                                {
                                    room.InnerWalls.Add(wall);
                                }
                                else if (relatedRoom.Status == 1 && !room.Walls.Contains(wall)) //外墙
                                {
                                    room.Walls.Add(wall);
                                }
                            }
                        }
                        else
                        {
                            for (int j = wall0.RelatedRooms.Count - 1; j >= 0; j--)
                            {
                                var relatedRoom = wall0.RelatedRooms[j];
                                Room room = relatedRoom.Room;

                                if (relatedRoom.Status == 0 && !room.InnerWalls.Contains(wall)) //内墙墙
                                {
                                    room.InnerWalls.Add(wall);
                                }
                                else if (relatedRoom.Status == 1 && !room.Walls.Contains(wall)) //外墙
                                {
                                    room.Walls.Add(wall);
                                }
                            }

                            // 遍历
                            foreach (var groupInFloor in floor.Groups)
                            {
                                Wall rawWall0 = groupInFloor.GetRawWallByLinkedWall(wall0);
                                Wall rawWall1 = groupInFloor.GetRawWallByLinkedWall(wall1);

                                if (rawWall0 != null && rawWall0.Equals(rawWall1))
                                {
                                    canMerge = true;

                                    groupInFloor.Remove(wall0);
                                    groupInFloor.Remove(wall1);

                                    groupInFloor.RemoveLinkedWall(rawWall0, wall0);
                                    groupInFloor.RemoveLinkedWall(rawWall0, wall1);

                                    groupInFloor.AddLinkedWall(rawWall0, wall);

                                }
                            }
                        }

                        floor.RemoveEntity(wall0);
                        floor.RemoveEntity(wall1);
                        floor.RemoveEntity(corner);

                        GraphicManager.DestoryGraphic(wall0);
                        GraphicManager.DestoryGraphic(wall1);

                        floor.AddEntity(wall);
                        GraphicManager.CreateGraphic(wall);
                    }
                    else
                    {
                        wall.Unbind();
                    }
                }
            }

            // 释放
            m_RawWallOverlaps.Clear();
            m_RawWallPoints.Clear();
            ListPool<Wall>.Release(addWalls);
            ListPool<Wall>.Release(deleteWalls);
        }

        public static void RemoveGroupHandler(Floor floor, Group group)
        {
            foreach (var member in group.Members)
            {
                if (member.Entity.Type != EntityType.Wall)
                {
                    GraphicManager.DestoryGraphic(member.Entity);
                }
            }
            GraphicManager.DestoryGraphic(group);

            int rawWallCount = group.RawWalls.Count;
            // 删除和其他 Group 无关的墙体
            for (int i = 0; i < rawWallCount; i++)
            {
                var rawWall = group.RawWalls[i];
                group.TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls);
                int linkedWallCount = linkedWalls.Count;
                //if (linkedWallCount == 1)
                //{
                //    var linkedWall = linkedWalls[0];
                //    if (linkedWall.Corner0.Equals(rawWall.Corner0) && linkedWall.Corner1.Equals(rawWall.Corner1))
                //    {
                //        continue;
                //    }
                //}

                List<int> indies = new List<int>();
                for (int j = 0; j < linkedWallCount; j++)
                {
                    var linkedWall = linkedWalls[j];
                    bool isLinked = false;
                    foreach (var groupInFloor in floor.Groups)
                    {
                        if (groupInFloor.Equals(group))
                            continue;

                        if (groupInFloor.ContainsLinkedWall(linkedWall))
                        {
                            isLinked = true;
                            break;
                        }
                    }

                    if (!isLinked)
                    {
                        foreach (var room in floor.Rooms)
                        {
                            //if (room.Owner != null)
                            //    continue;

                            if (group.Contains(room))
                                continue;

                            if (room.Contains(linkedWall))
                            {
                                isLinked = true;
                                break;
                            }
                        }
                    }

                    if (isLinked)
                    {
                        indies.Add(j);
                    }
                }

                for (int j = linkedWallCount - 1; j >= 0; j--)
                {
                    var linkedWall = linkedWalls[j];


                    group.Remove(linkedWall);
                    if (!indies.Contains(j))
                    {
                        floor.RemoveEntity(linkedWall);
                        GraphicManager.DestoryGraphic(linkedWall);
                    }
                }
            }
        }

        /// <summary>
        /// 分割墙体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="points"></param>
        /// <param name="ignoreCorners"></param>
        /// <param name="results"></param>
        public static void SplitWall(Wall target, List<Vector2> points, out List<Wall> results)
        {
            if (points == null || points.Count == 0)
            {
                results = null;
                return;
            }

            Corner corner0 = target.Corner0;
            Corner corner1 = target.Corner1;

            if (target.Corner0.Owner is Group)
            {
                Group group = target.Corner0.Owner as Group;
                if (!Architect.TryGetCorner(target.Corner0.Position, out corner0))
                {
                    corner0 = new Corner();
                    corner0.Position = target.Corner0.Position;
                    Architect.AddEntity(corner0);
                }
                group.ReplaceCorner(target.Corner0, corner0, target.Corner0.Position);
            }

            if (target.Corner1.Owner is Group)
            {
                Group group = target.Corner1.Owner as Group;
                if (!Architect.TryGetCorner(target.Corner1.Position, out corner1))
                {
                    corner1 = new Corner();
                    corner1.Position = target.Corner1.Position;
                    Architect.AddEntity(corner1);
                }
                group.ReplaceCorner(target.Corner1, corner1, target.Corner1.Position);
            }

            float thickness = target.Thickness;
            float y = corner0.Position.y;
            // 去重
            MathUtility.Distinct(points);
            // indexFloats
            List<IndexFloat> indexFloats = ListPool<IndexFloat>.Get();
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 point = points[i];
                if (MathUtility.Appr(point, corner0.ToPoint2())
                    || MathUtility.Appr(point, corner1.ToPoint2()))
                    continue;

                IndexFloat indexFloat = new IndexFloat();
                indexFloat.index = i;
                indexFloat.value = Vector2.Distance(point, corner0.ToPoint2());
                indexFloats.Add(indexFloat);
            }
            // Corner0 - Corner1 排序
            indexFloats.Sort();
            // results
            results = new List<Wall>();
            Corner prevCorner = corner0;
            foreach (var item in indexFloats)
            {
                Vector3 position = points[item.index].XOZ(y);
                Corner corner = null;
                if (!Architect.TryGetCorner(position, out corner))
                {
                    corner = new Corner();
                    corner.Position = position;
                    Architect.AddEntity(corner);
                }
                // wall
                Wall wall = new Wall(prevCorner, corner);
                wall.Thickness = thickness;
                results.Add(wall);
                // prevCorner
                prevCorner = corner;
            }
            // 墙体拆分
            if (indexFloats.Count > 0)
            {
                // wall
                Wall wall = new Wall(prevCorner, corner1);
                wall.Thickness = thickness;
                results.Add(wall);
                // remove
                corner0.RemoveWall(target);
                corner1.RemoveWall(target);
            }

            // 释放
            ListPool<IndexFloat>.Release(indexFloats);
        }

        /// <summary>
        /// 将楼层种合并的墙体，进行合并
        /// </summary>
        /// <param name="floor"></param>
        public static void CombineWall(Floor floor)
        {
            // 合并墙体
            for (int i = floor.Corners.Count - 1; i >= 0; i--)
            {
                Corner corner = floor.Corners[i];

                if (!corner.Active)
                    continue;

                if (corner.Walls.Count != 2)
                    continue;

                Wall wall0 = corner.Walls[0];
                Wall wall1 = corner.Walls[1];
                Corner corner0 = wall0.Another(corner);
                Corner corner1 = wall1.Another(corner);

                if (MathUtility.Collinear(corner0.Position, corner.Position, corner1.Position))
                {
                    Wall wall = new Wall(corner0, corner1);
                    wall.Height = wall0.Height;
                    wall.Thickness = wall0.Thickness;

                    bool isLinked0 = false;
                    bool isLinked1 = false;
                    bool canMerge = true;

                    // 遍历
                    foreach (var groupInFloor in floor.Groups)
                    {
                        Wall rawWall0 = groupInFloor.GetRawWallByLinkedWall(wall0);
                        Wall rawWall1 = groupInFloor.GetRawWallByLinkedWall(wall1);

                        if (rawWall0 != null)
                        {
                            isLinked0 = true;
                        }

                        if (rawWall1 != null)
                        {
                            isLinked1 = true;
                        }

                        if (!object.Equals(rawWall0, rawWall1))
                        {
                            canMerge = false;
                        }
                    }

                    if (canMerge)
                    {
                        if (!isLinked0 && !isLinked1)
                        {
                            for (int j = wall0.RelatedRooms.Count - 1; j >= 0; j--)
                            {
                                var relatedRoom = wall0.RelatedRooms[j];
                                Room room = relatedRoom.Room;

                                if (relatedRoom.Status == 0 && !room.InnerWalls.Contains(wall)) //内墙墙
                                {
                                    room.InnerWalls.Add(wall);
                                }
                                else if (relatedRoom.Status == 1 && !room.Walls.Contains(wall)) //外墙
                                {
                                    room.Walls.Add(wall);
                                }
                            }
                        }
                        else
                        {
                            for (int j = wall0.RelatedRooms.Count - 1; j >= 0; j--)
                            {
                                var relatedRoom = wall0.RelatedRooms[j];
                                Room room = relatedRoom.Room;

                                if (relatedRoom.Status == 0 && !room.InnerWalls.Contains(wall)) //内墙墙
                                {
                                    room.InnerWalls.Add(wall);
                                }
                                else if (relatedRoom.Status == 1 && !room.Walls.Contains(wall)) //外墙
                                {
                                    room.Walls.Add(wall);
                                }
                            }

                            // 遍历
                            foreach (var groupInFloor in floor.Groups)
                            {
                                Wall rawWall0 = groupInFloor.GetRawWallByLinkedWall(wall0);
                                Wall rawWall1 = groupInFloor.GetRawWallByLinkedWall(wall1);

                                if (rawWall0 != null && rawWall0.Equals(rawWall1))
                                {
                                    canMerge = true;

                                    groupInFloor.Remove(wall0);
                                    groupInFloor.Remove(wall1);

                                    groupInFloor.RemoveLinkedWall(rawWall0, wall0);
                                    groupInFloor.RemoveLinkedWall(rawWall0, wall1);

                                    groupInFloor.AddLinkedWall(rawWall0, wall);

                                }
                            }
                        }

                        floor.RemoveEntity(wall0);
                        floor.RemoveEntity(wall1);
                        floor.RemoveEntity(corner);

                        GraphicManager.DestoryGraphic(wall0);
                        GraphicManager.DestoryGraphic(wall1);

                        floor.AddEntity(wall);
                        GraphicManager.CreateGraphic(wall);
                    }
                    else
                    {
                        wall.Unbind();
                    }
                }
            }
        }
    }


}
