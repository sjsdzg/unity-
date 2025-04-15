using System;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Floor : EntityObject
    {
        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type
        {
            get
            {
                return EntityType.Floor;
            }
        }

        private int m_Number = 1;
        /// <summary>
        /// 楼层序号
        /// </summary>
        public int Number
        {
            get { return m_Number; }
            set { m_Number = value; }
        }

        private float height = 2.8f;
        /// <summary>
        /// 层高
        /// </summary>
        public float Height
        {
            get { return height; }
            set { SetProperty(ref height, value, Constants.height); }
        }

        private float thickness = 0.012f;
        /// <summary>
        /// 地板厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set
            {
                SetProperty(ref thickness, value, Constants.thickness);
            }
        }

        private float altitude = 0f;
        /// <summary>
        /// 离地高度（水平面）
        /// </summary>
        public float Altitude
        {
            get { return altitude; }
            set 
            {
                SetProperty(ref altitude, value, Constants.altitude);
            }
        }

        /// <summary>
        /// 墙角列表
        /// </summary>
        private List<Corner> m_Corners = new List<Corner>();

        /// <summary>
        /// 墙列表
        /// </summary>
        private List<Wall> m_Walls = new List<Wall>();

        /// <summary>
        /// 房间列表
        /// </summary>
        private List<Room> m_Rooms = new List<Room>();

        /// <summary>
        /// 门列表
        /// </summary>
        private List<Door> m_Doors = new List<Door>();

        /// <summary>
        /// 窗户列表
        /// </summary>
        private List<Window> m_Windows = new List<Window>();

        /// <summary>
        /// 垭口列表
        /// </summary>
        private List<Pass> m_Passes = new List<Pass>();

        /// <summary>
        /// 组列表
        /// </summary>
        private List<Group> m_Groups = new List<Group>();

        /// <summary>
        /// 区域列表
        /// </summary>
        private List<Area> m_Areas = new List<Area>();

        /// <summary>
        /// 设备列表
        /// </summary>
        private List<Equipment> m_Equipments = new List<Equipment>();

        public List<Corner> Corners { get { return m_Corners; } }

        public List<Wall> Walls { get { return m_Walls; } }

        public List<Wall> ActiveWalls
        {
            get
            {
                List<Wall> walls = new List<Wall>();
                foreach (var wall in m_Walls)
                {
                    if (wall.Active)
                    {
                        walls.Add(wall);
                    }
                }
                return walls;
            }
        }

        public List<Wall> NonLinkedWallsWithGroup
        { 
            get
            {
                List<Wall> walls = new List<Wall>();
                foreach (var wall in m_Walls)
                {
                    if (!IsLinkedWallWithGroup(wall))
                    {
                        walls.Add(wall);
                    }
                }

                return walls;
            }
        }



        public List<Room> Rooms { get { return m_Rooms; } }

        public List<Door> Doors { get { return m_Doors; } }

        public List<Window> Windows { get { return m_Windows; } }

        public List<Pass> Passes { get { return m_Passes; } }

        public List<Group> Groups { get { return m_Groups; } }

        public List<Area> Areas { get { return m_Areas; } }

        public List<Equipment> Equipments { get { return m_Equipments; } }

        public Floor(int number)
        {
            m_Number = number;
        }

        /// <summary>
        /// 捕捉点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="point"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool SnapPoint(Vector3 target, out Vector3 result, float epsilon = 0.01f)
        {
            bool flag = false;
            result = Vector3.zero;

            float k = epsilon;
            foreach (var wall in m_Walls)
            {
                if (!wall.Active)
                    continue;

                foreach (var point in wall.Points)
                {
                    float temp = Vector3.Distance(target, point);
                    if (temp <= k)
                    {
                        result = point;
                        flag = true;
                        k = temp;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 捕捉墙角
        /// </summary>
        /// <param name="target"></param>
        /// <param name="corner"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool SnapCorner(Vector3 target, out Corner corner, float epsilon = 0.01f)
        {
            List<Corner> corners = m_Corners.FindAll(x => Mathf.Abs(target.x - x.Position.x) <= epsilon && Mathf.Abs(target.z - x.Position.z) <= epsilon);
            if (corners.Count == 0)
            {
                corner = null;
                return false;
            }

            float k = float.MaxValue;
            int index = -1;
            for (int i = 0; i < corners.Count; i++)
            {
                float temp = Mathf.Abs(Vector3.Distance(target, corners[i].Position));
                if (k >= temp)
                {
                    index = i;
                    k = temp;
                }
            }
            corner = corners[index];
            return true;
        }

        /// <summary>
        /// 根据轴，捕捉墙角点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="corner"></param>
        /// <param name="axisMode"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool SnapAxis(Vector3 target, out Vector3 axis, float epsilon = 0.01f)
        {
            List<Corner> x_corners = m_Corners.FindAll(x => Mathf.Abs(target.x - x.Position.x) <= epsilon);
            //List<Corner> cornersY = m_Corners.FindAll(x => Mathf.Abs(target.y - x.Position.y) <= epsilon);
            List<Corner> z_corners = m_Corners.FindAll(x => Mathf.Abs(target.z - x.Position.z) <= epsilon);

            axis = Vector3.positiveInfinity;

            if (x_corners.Count == 0 && z_corners.Count == 0)
            {
                return false;
            }

            float k = epsilon;
            int index = -1;
            if (x_corners.Count > 0)
            {
                for (int i = 0; i < x_corners.Count; i++)
                {
                    float temp = Mathf.Abs(target.x - x_corners[i].Position.x);
                    if (k >= temp)
                    {
                        index = i;
                        k = temp;
                    }
                }
                axis.x = x_corners[index].Position.x;
            }

            if (z_corners.Count > 0)
            {
                k = epsilon;
                index = -1;
                for (int i = 0; i < z_corners.Count; i++)
                {
                    float temp = Mathf.Abs(target.z - z_corners[i].Position.z);
                    if (k >= temp)
                    {
                        index = i;
                        k = temp;
                    }
                }
                axis.z = z_corners[index].Position.z;
            }
            return true;
        }

        /// <summary>
        /// 捕捉墙
        /// </summary>
        /// <param name="target"></param>
        /// <param name="wall"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool SnapWall(Vector3 target, out Wall wall, float offset = 0f, float epsilon = 0.01f)
        {
            Wall nearestWall = m_Walls.Min(x => x.GetSanpSegment2().Trim(offset + epsilon).GetDistanceToPoint(target.XZ()));

            if (nearestWall == null)
            {
                wall = null;
                return false;
            }

            float distance = nearestWall.GetSanpSegment2().Trim(offset + epsilon).GetDistanceToPoint(target.XZ());

            if (distance <= epsilon)
            {
                wall = nearestWall;
                return true;
            }
            else
            {
                wall = null;
                return false;
            }
        }

        /// <summary>
        /// 捕捉墙（和线段平行且最近的墙体）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="wall"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool SnapWall(Segment2 target, out Wall wall, float epsilon = 0.01f)
        {
            bool flag = false;
            wall = null;
            // 和线段平行的墙体
            List<Wall> parallelWalls = m_Walls.FindAll(x => MathUtility.IsParallel(target.direction, x.ToVector2(x.Corner0)));

            float k = epsilon;
            foreach (var parallelWall in parallelWalls)
            {
                Segment2 segment = parallelWall.ToSegment2(parallelWall.Corner0);
                float distance = ((Line2)segment).GetDistanceToPoint(target.p1);
                //float dis = parallelWall.ToSegment2(parallelWall.Corner0).GetDistanceToPoint(target.p1);
                //float dis2 = parallelWall.ToSegment2(parallelWall.Corner0).GetDistanceToPoint(target.p2);
                //dis = dis <= dis2 ? dis : dis2;
                if (distance < k)
                {
                    float cross = Vector2.Dot(segment.normal, target.p1 - parallelWall.Corner0.ToPoint2());
                    if (cross > 0)
                        segment = segment.Move(distance);
                    else
                        segment = segment.Move(-distance);

                    if (target.Overlap(segment, out Segment2 result))
                    {
                        flag = true;
                        wall = parallelWall;
                        k = distance;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 添加元件数据
        /// </summary>
        /// <param name="entity"></param>
        public void AddEntity(EntityObject entity)
        {
            switch (entity.Type)
            {
                case EntityType.None:
                    break;
                case EntityType.Corner:
                    Corner corner = entity as Corner;
                    m_Corners.Add(corner);
                    break;
                case EntityType.Wall:
                    Wall wall = entity as Wall;
                    m_Walls.Add(wall);
                    break;
                case EntityType.Room:
                    Room room = entity as Room;
                    m_Rooms.Add(room);
                    break;
                case EntityType.Door:
                    Door door = entity as Door;
                    m_Doors.Add(door);
                    break;
                case EntityType.Window:
                    Window window = entity as Window;
                    m_Windows.Add(window);
                    break;
                case EntityType.Pass:
                    Pass pass = entity as Pass;
                    m_Passes.Add(pass);
                    break;
                case EntityType.Group:
                    Group group = entity as Group;
                    m_Groups.Add(group);
                    break;
                case EntityType.Area:
                    Area area = entity as Area;
                    m_Areas.Add(area);
                    break;
                case EntityType.Equipment:
                    Equipment equipment = entity as Equipment;
                    m_Equipments.Add(equipment);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 移除元件数据
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveEntity(EntityObject entity)
        {
            switch (entity.Type)
            {
                case EntityType.None:
                    break;
                case EntityType.Corner:
                    Corner corner = entity as Corner;
                    m_Corners.Remove(corner);
                    break;
                case EntityType.Wall:
                    Wall wall = entity as Wall;
                    wall.Unbind();
                    m_Walls.Remove(wall);
                    break;
                case EntityType.Room:
                    Room room = entity as Room;
                    m_Rooms.Remove(room);
                    break;
                case EntityType.Door:
                    Door door = entity as Door;
                    door.Unbind();
                    m_Doors.Remove(door);
                    break;
                case EntityType.Window:
                    Window window = entity as Window;
                    window.Unbind();
                    m_Windows.Remove(window);
                    break;
                case EntityType.Pass:
                    Pass pass = entity as Pass;
                    m_Passes.Remove(pass);
                    break;
                case EntityType.Group:
                    Group group = entity as Group;
                    m_Groups.Remove(group);
                    break;
                case EntityType.Area:
                    Area area = entity as Area;
                    m_Areas.Remove(area);
                    break;
                case EntityType.Equipment:
                    Equipment equipment = entity as Equipment;
                    m_Equipments.Remove(equipment);
                    break;
                default:
                    break;
            }
        }

        public EntityObject GetEntity(string id)
        {
            EntityObject entity = null;

            entity = m_Corners.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Walls.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Rooms.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Doors.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Windows.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Passes.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Groups.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Areas.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            entity = m_Equipments.Find(x => x.Id.Equals(id));
            if (entity != null)
            {
                return entity;
            }

            return entity;
        }

        /// <summary>
        /// 线段投射墙体，返回击中墙体的信息
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public List<SegmentCastWallHit> SegmentCastWalls(Segment2 origin)
        {
            return ArchitectUtility.SegmentCastAllWalls(origin, m_Walls);
        }

        /// <summary>
        /// 是否能添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public bool CanAddRoom(Room target)
        {
            bool flag = true;

            ComplexPolygon2 originalPolygon = new ComplexPolygon2();
            originalPolygon.contours = new List<List<Vector2>>();
            originalPolygon.contours.Add(target.Contour);
            originalPolygon.contours.AddRange(target.InnerContours);

            ComplexPolygon2 otherPolygon = new ComplexPolygon2();
            otherPolygon.contours = new List<List<Vector2>>();

            List<Vector2> contour = ListPool<Vector2>.Get();
            foreach (var area in m_Areas)
            {
                contour.Clear();
                foreach (var point in area.Contour)
                {
                    contour.Add(point.XZ());
                }

                otherPolygon.contours.Clear();
                otherPolygon.contours.Add(contour);

                if (!otherPolygon.Contains(originalPolygon))
                {
                    flag = false;
                    break;
                }
            }

            foreach (var room in m_Rooms)
            {
                if (!room.Active)
                    continue;

                otherPolygon.contours.Clear();
                otherPolygon.contours.Add(room.Contour);
                otherPolygon.contours.AddRange(room.InnerContours);

                if (originalPolygon.Overlaps(otherPolygon))
                {
                    flag = false;
                    break;
                }
            }

            ListPool<Vector2>.Release(contour);
            //if (!flag)
            //{
            //    foreach (var wall in m_Walls)
            //    {
            //        Segment2 segment = wall.ToSegment2(wall.Corner0).Trim(Architect.Instance.SnapEpsilon);
            //        foreach (var _wall in target.Walls)
            //        {
            //            Segment2 _segment = _wall.ToSegment2(_wall.Corner0).Trim(Architect.Instance.SnapEpsilon);
            //            if (segment.Intersect(_segment, out Vector2 point))
            //            {
            //                flag = false;
            //                break;
            //            }
            //        }

            //        if (!flag)
            //        {
            //            break;
            //        }
            //    }
            //}

            return flag;
        }

        public bool TryGetCorner(Vector3 position, out Corner result)
        {
            foreach (var corner in m_Corners)
            {
                if (!corner.Active)
                    continue;

                if (corner.Owner is Group)
                    continue;

                if (MathUtility.Appr(position, corner.Position))
                {
                    result = corner;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public bool TryGetWall(Corner corner0, Corner corner1, out Wall result)
        {
            foreach (var wall in m_Walls)
            {
                if (!wall.Active)
                    continue;

                if ((corner0.Equals(wall.Corner0) && corner1.Equals(wall.Corner1))
                    || (corner0.Equals(wall.Corner1) && corner1.Equals(wall.Corner0)))
                {
                    result = wall;
                    return true;
                }
            }
            // 未获取到
            result = null;
            return false;
        }

        public bool TryGetWall(Vector3 position0, Vector3 position1, out Wall result)
        {
            foreach (var wall in m_Walls)
            {
                if ((MathUtility.Appr(position0, wall.Corner0.Position) && MathUtility.Appr(position1, wall.Corner1.Position))
                    || (MathUtility.Appr(position0, wall.Corner1.Position) && MathUtility.Appr(position1, wall.Corner0.Position)))
                {
                    result = wall;
                    return true;
                }
            }
            // 未获取到
            result = null;
            return false;
        }

        private void Group_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            switch (e.PropertyName)
            {
                case Constants.position:

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 是否跟（Group）中墙体有连接
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public bool IsLinkedWallWithGroup(Wall wall)
        {
            return m_Groups.Exists(x => x.ContainsLinkedWall(wall));
        }


        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}                                          