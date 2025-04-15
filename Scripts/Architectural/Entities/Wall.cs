using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{

    public class Wall : EntityObject
    {
        public class RelatedRoom
        {
            public Room Room { get; set; }

            /// <summary>
            /// 0 ： 墙体是房间的内轮廓墙体
            /// 1 ： 墙体是房间的外轮廓墙体
            /// </summary>
            public int Status { get; set; }
        }

        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type
        {
            get
            {
                return EntityType.Wall;
            }
        }

        private Corner corner0;
        /// <summary>
        /// 墙角0
        /// </summary>
        public Corner Corner0
        {
            get { return corner0; }
            set
            {
                SetProperty(ref corner0, value, Constants.corner0);
            }
        }

        private Corner corner1;
        /// <summary>
        /// 墙角1
        /// </summary>
        public Corner Corner1
        {
            get { return corner1; }
            set
            {
                SetProperty(ref corner1, value, Constants.corner1);
            }
        }

        private ObservableCollection<WallHole> holes = new ObservableCollection<WallHole>();
        /// <summary>
        /// 包含墙洞的列表
        /// </summary>
        public ObservableCollection<WallHole> Holes
        {
            get { return holes; }
        }

        private ObservableCollection<RelatedRoom> m_RelatedRooms = new ObservableCollection<RelatedRoom>();
        /// <summary>
        /// 连接的房间列表
        /// </summary>
        public ObservableCollection<RelatedRoom> RelatedRooms
        {
            get { return m_RelatedRooms; }
        }

        private float height = 2.8f;
        /// <summary>
        /// 高度
        /// </summary>
        public float Height
        {
            get { return height; }
            set { SetProperty(ref height, value, Constants.height); }
        }

        private float thickness = 0.2f;
        /// <summary>
        /// 厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set
            {
                SetProperty(ref thickness, value, Constants.thickness);
            }
        }

        private List<Segment2> segments = new List<Segment2>();
        /// <summary>
        /// 轮廓线段列表
        /// </summary>
        public List<Segment2> Segments
        {
            get
            {
                return segments;
            }
        }

        private List<Vector3> points = new List<Vector3>();
        /// <summary>
        /// 顶点列表
        /// </summary>
        public List<Vector3> Points
        {
            get { return points; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public float Length
        {
            get
            {
                return Vector3.Distance(corner0.Position, corner1.Position);
            }
        }

        /// <summary>
        /// 获取捕捉墙体中心线段
        /// </summary>
        /// <returns></returns>
        public Segment2 GetSanpSegment2()
        {
            int count = Segments.Count;
            Segment2 segment0 = Segments[0].Move(-thickness * 0.5f);
            Segment2 segment1 = Segments[1].Move(thickness * 0.5f);
            segment0.Overlap(segment1, out Segment2 result);
            return result;
        }

        public Wall()
        {
            Special = "建筑";
        }

        public Wall(Corner corner0, Corner corner1)
        {
            Special = "建筑";

            SetCorners(corner0, corner1);
        }

        public void SetCorners(Corner corner0, Corner corner1)
        {
            this.corner0 = corner0;
            this.corner1 = corner1;

            if (!Active) return;

            Bind();
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);

            switch (propertyName)
            {
                case Constants.active:
                    if (corner0 == null || corner1 == null)
                        return;

                    if (Active)
                    {
                        // 建立联系
                        corner0.AddWall(this);
                        corner1.AddWall(this);

                        // 初始化事件
                        corner0.PropertyChanged += Corner0_PropertyChanged;
                        corner1.PropertyChanged += Corner1_PropertyChanged;

                        Corner0.Walls.ItemChanged += Corner0_Walls_ItemChanged;
                        Corner1.Walls.ItemChanged += Corner1_Walls_ItemChanged;

                        Corner0.Walls.CollectionChanged += Corner0_Walls_CollectionChanged;
                        Corner1.Walls.CollectionChanged += Corner1_Walls_CollectionChanged;

                        holes.ItemChanged += Holes_ItemChanged;
                        holes.CollectionChanged += Holes_CollectionChanged;
                        // 同步
                        corner0.Sync();
                        corner1.Sync();
                        Sync();
                    }
                    else
                    {
                        // 初始化事件
                        corner0.PropertyChanged -= Corner0_PropertyChanged;
                        corner1.PropertyChanged -= Corner1_PropertyChanged;

                        Corner0.Walls.ItemChanged -= Corner0_Walls_ItemChanged;
                        Corner1.Walls.ItemChanged -= Corner1_Walls_ItemChanged;

                        Corner0.Walls.CollectionChanged -= Corner0_Walls_CollectionChanged;
                        Corner1.Walls.CollectionChanged -= Corner1_Walls_CollectionChanged;

                        holes.ItemChanged -= Holes_ItemChanged;
                        holes.CollectionChanged -= Holes_CollectionChanged;

                        corner0.RemoveWall(this);
                        corner1.RemoveWall(this);
                    }
                    break;
                case Constants.corner0:
                    PropertyChangedUtils.RemoveEvent(oldValue, Corner0_PropertyChanged);
                    PropertyChangedUtils.AddEvent(newValue, Corner0_PropertyChanged);

                    Corner oldCorner0 = (Corner)oldValue;
                    Corner newCorner0 = (Corner)newValue;

                    if (oldCorner0 != null)
                    {
                        oldCorner0.Walls.ItemChanged -= Corner0_Walls_ItemChanged;
                        oldCorner0.Walls.CollectionChanged -= Corner0_Walls_CollectionChanged;
                        oldCorner0.RemoveWall(this);
                    }

                    if (newCorner0 != null)
                    {
                        newCorner0.Walls.ItemChanged += Corner0_Walls_ItemChanged;
                        newCorner0.Walls.CollectionChanged += Corner0_Walls_CollectionChanged;
                        newCorner0.AddWall(this);
                    }
                    break;
                case Constants.corner1:
                    PropertyChangedUtils.RemoveEvent(oldValue, Corner1_PropertyChanged);
                    PropertyChangedUtils.AddEvent(newValue, Corner1_PropertyChanged);

                    Corner oldCorner1 = (Corner)oldValue;
                    Corner newCorner1 = (Corner)newValue;

                    if (oldCorner1 != null)
                    {
                        oldCorner1.Walls.ItemChanged -= Corner1_Walls_ItemChanged;
                        oldCorner1.Walls.CollectionChanged -= Corner1_Walls_CollectionChanged;
                        oldCorner1.RemoveWall(this);
                    }

                    if (newCorner1 != null)
                    {
                        newCorner1.Walls.ItemChanged += Corner1_Walls_ItemChanged;
                        newCorner1.Walls.CollectionChanged += Corner1_Walls_CollectionChanged;
                        newCorner1.AddWall(this);
                    }

                    break;
                case Constants.height:
                    Sync();
                    break;
                case Constants.thickness:
                    Sync();
                    break;
                default:
                    break;
            }
        }

        private void Corner0_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            switch (e.PropertyName)
            {
                case Constants.position:
                    List<Corner> connectedCorners = corner0.GetConnectedCorners();
                    foreach (var corner in connectedCorners)
                    {
                        corner.Sync();
                        foreach (var wall in corner.Walls)
                        {
                            wall.Sync();
                        }
                    }
                    corner0.Sync();
                    Sync();
                    // 同步房间
                    foreach (var linkedRoom in RelatedRooms)
                    {
                        Room room = linkedRoom.Room;
                        if (linkedRoom.Status == 1)
                        {
                            room.SyncContour();
                        }
                        else if (linkedRoom.Status == 0)
                        {
                            room.SyncInnerContour();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Corner1_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            switch (e.PropertyName)
            {
                case Constants.position:
                    List<Corner> connectedCorners = corner1.GetConnectedCorners();
                    foreach (var corner in connectedCorners)
                    {
                        corner.Sync();
                        foreach (var wall in corner.Walls)
                        {
                            wall.Sync();
                        }
                    }
                    corner1.Sync();
                    Sync();
                    // 同步房间
                    foreach (var linkedRoom in RelatedRooms)
                    {
                        Room room = linkedRoom.Room;
                        if (linkedRoom.Status == 1)
                        {
                            room.SyncContour();
                        }
                        else if (linkedRoom.Status == 0)
                        {
                            room.SyncInnerContour();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Corner0_Walls_ItemChanged(object sender, PropertyChangedArgs e)
        {
            if (this.Equals((Wall)sender))
                return;

            switch (e.PropertyName)
            {
                case Constants.thickness:
                    Sync();
                    break;
                default:
                    break;
            }
        }

        private void Corner1_Walls_ItemChanged(object sender, PropertyChangedArgs e)
        {
            if (this.Equals((Wall)sender))
                return;

            switch (e.PropertyName)
            {
                case Constants.thickness:
                    Sync();
                    break;
                default:
                    break;
            }
        }

        private void Corner0_Walls_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            corner0.Sync();
            Sync();
        }

        private void Corner1_Walls_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            corner1.Sync();
            Sync();
        }

        private void Holes_ItemChanged(object sender, PropertyChangedArgs e)
        {
            OnVerticesChanged();
        }

        private void Holes_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            OnVerticesChanged();
        }

        /// <summary>
        /// 同步更新片段信息
        /// </summary>
        public void Sync()
        {
            segments.Clear();
            points.Clear();

            Vector2[] intCorner0Points, intCorner1Points;

            /* 点排布
             * Corner0_0  Corner1_0
             * Corner0_2  Corner1_2
             * Corner0_2  Corner1_3
             * Corner0_1  Corner1_1
             */
            int[] intCorner0States = IntersectInCorner0(out intCorner0Points);
            int[] intCorner1States = IntersectInCorner1(out intCorner1Points);
            /*
             * Corner0_0  Corner1_0
             */
            segments.Add(new Segment2(intCorner0Points[0], intCorner1Points[0]));

            /*
            * Corner0_1  Corner1_1
            */
            segments.Add(new Segment2(intCorner0Points[1], intCorner1Points[1]));

            if (corner0.Walls.Count == 1)
            {
                /*
                * Corner0_0
                * Corner0_1
                */
                segments.Add(new Segment2(intCorner0Points[0], intCorner0Points[1]));
            }

            if (corner1.Walls.Count == 1)
            {
                /*
                * Corner1_0
                * Corner1_1
                */
                segments.Add(new Segment2(intCorner1Points[0], intCorner1Points[1]));
            }

            if (intCorner0States[2] == 1)
            {
                segments.Add(new Segment2(intCorner0Points[0], intCorner0Points[2]));

            }

            if (intCorner0States[3] == 1)
            {
                segments.Add(new Segment2(intCorner0Points[1], intCorner0Points[3]));
            }

            if (intCorner1States[2] == 1)
            {
                segments.Add(new Segment2(intCorner1Points[0], intCorner1Points[2]));

            }

            if (intCorner1States[3] == 1)
            {
                segments.Add(new Segment2(intCorner1Points[1], intCorner1Points[3]));
            }

            points.Add(corner0.Position);
            points.Add(new Vector3(intCorner0Points[0].x, corner0.Position.y, intCorner0Points[0].y));
            points.Add(new Vector3(intCorner1Points[0].x, corner0.Position.y, intCorner1Points[0].y));
            points.Add(corner1.Position);
            points.Add(new Vector3(intCorner1Points[1].x, corner0.Position.y, intCorner1Points[1].y));
            points.Add(new Vector3(intCorner0Points[1].x, corner0.Position.y, intCorner0Points[1].y));

            // OnVerticesChanged
            OnVerticesChanged();
        }


        /// <summary>
        /// 左墙角相交的情况
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public int[] IntersectInCorner0(out Vector2[] points)
        {
            points = new Vector2[4];
            int[] states = new int[4] { 1, 1, 0, 0 };
            
            Line2 leftLine = this.GetLine2(corner0, ClockDirection.CounterClockwise);
            Line2 rightLine = this.GetLine2(corner0, ClockDirection.Clockwise);

            Vector2 point;
            Wall leftWall = corner0.GetWall(this, ClockDirection.CounterClockwise);
            if (leftWall == null)
            {
                points[0] = leftLine.p1;
                points[1] = rightLine.p1;
                return states;
            }

            Line2 leftWallRightLine = leftWall.GetLine2(corner0, ClockDirection.Clockwise);
            if (leftWallRightLine.Intersect(leftLine, out point))
            {
                points[0] = point;
            }
            else // 直线平行
            {
                points[0] = leftLine.p1;
                if (this.thickness - leftWall.thickness > MathUtility.Epsilon)
                {
                    points[2] = leftWallRightLine.p1;
                    states[2] = 1;
                }
            }

            Wall rightWall = corner0.GetWall(this, ClockDirection.Clockwise);
            Line2 rightWallLeftLine = rightWall.GetLine2(corner0, ClockDirection.CounterClockwise);
            if (rightWallLeftLine.Intersect(rightLine, out point))
            {
                points[1] = point;
            }
            else // 直线平行
            {
                points[1] = rightLine.p1;
                if (this.thickness - rightWall.thickness > MathUtility.Epsilon)
                {
                    points[3] = rightWallLeftLine.p1;
                    states[3] = 1;
                }
            }

            return states;
        }

        /// <summary>
        /// 右墙角相交的情况
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public int[] IntersectInCorner1(out Vector2[] points)
        {
            points = new Vector2[4];
            int[] states = new int[4] { 1, 1, 0, 0 };

            Line2 leftLine = this.GetLine2(corner0, ClockDirection.CounterClockwise);
            Line2 rightLine = this.GetLine2(corner0, ClockDirection.Clockwise);

            Vector2 point;
            Wall leftWall = corner1.GetWall(this, ClockDirection.CounterClockwise);
            if (leftWall == null)
            {
                points[0] = leftLine.p2;
                points[1] = rightLine.p2;
                return states;
            }

            Line2 leftWallRightLine = leftWall.GetLine2(corner1, ClockDirection.Clockwise);
            if (leftWallRightLine.Intersect(rightLine, out point))
            {
                points[1] = point;
            }
            else // 直线平行
            {
                points[1] = rightLine.p2;
                if (this.thickness - leftWall.thickness > MathUtility.Epsilon)
                {
                    points[3] = leftWallRightLine.p2;
                    states[3] = 1;
                }
            }

            Wall rightWall = corner1.GetWall(this, ClockDirection.Clockwise);
            Line2 rightWallLeftLine = rightWall.GetLine2(corner1, ClockDirection.CounterClockwise);
            if (rightWallLeftLine.Intersect(leftLine, out point))
            {
                points[0] = point;
            }
            else // 直线平行
            {
                points[0] = leftLine.p2;
                if (this.thickness - rightWall.thickness > MathUtility.Epsilon)
                {
                    points[2] = rightWallLeftLine.p1;
                    states[2] = 1;
                }
            }

            return states;
        }

        /// <summary>
        /// 添加墙洞
        /// </summary>
        /// <param name="hole"></param>
        public void AddHole(WallHole hole)
        {
            if (holes.Contains(hole))
                return;

            holes.Add(hole);
        }

        /// <summary>
        /// 移除墙洞
        /// </summary>
        /// <param name="hole"></param>
        public void RemoveHole(WallHole hole)
        {
            if (!holes.Contains(hole))
                return;

            holes.Remove(hole);
        }

        /// <summary>
        /// 能否添加墙洞
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanAddHole(WallHole target)
        {
            Vector2 direction = this.ToVector2(Corner0).normalized;

            Segment2 holeSegment = new Segment2();
            holeSegment.p1 = target.ToPoint2() - direction * target.Length * 0.5f;
            holeSegment.p2 = target.ToPoint2() + direction * target.Length * 0.5f;

            bool flag = true;
            Segment2 segment = new Segment2();
            foreach (WallHole hole in Holes)
            {
                if (hole.Equals(target))
                    continue;

                segment.p1 = hole.ToPoint2() - direction * hole.Length * 0.5f;
                segment.p2 = hole.ToPoint2() + direction * hole.Length * 0.5f;

                if (segment.Overlap(holeSegment, out Segment2 result))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        public void AddRoom(Room room, int status = 1)
        {
            for (int i = m_RelatedRooms.Count - 1; i >= 0; i--)
            {
                if (m_RelatedRooms[i].Room.Equals(room))
                {
                    return;
                }
            }

            RelatedRoom linkedRoom = new RelatedRoom();
            linkedRoom.Room = room;
            linkedRoom.Status = status;            
            m_RelatedRooms.Add(linkedRoom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        public void RemoveRoom(Room room)
        {
            int index = -1;
            for (int i = m_RelatedRooms.Count - 1; i >= 0; i--)
            {
                if (m_RelatedRooms[i].Room.Equals(room))
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                m_RelatedRooms.RemoveAt(index);
            }
            
        }

        /// <summary>
        /// 是否能分割墙体
        /// 如果此处已经放置门、窗、垭口。即不可分割
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool CanSplit(Vector2 point)
        {
            Segment2 segment = this.ToSegment2(Corner0);
            float distance = segment.GetDistanceToPoint(point);
            // 距离
            if (!MathUtility.Appr(distance, 0))
                return false;

            bool flag = true;
            Vector2 direction = this.ToVector2(Corner0).normalized;
            foreach (WallHole hole in Holes)
            {
                segment.p1 = hole.ToPoint2() - direction * hole.Length * 0.5f;
                segment.p2 = hole.ToPoint2() + direction * hole.Length * 0.5f;

                distance = segment.GetDistanceToPoint(point);
                if (MathUtility.Appr(distance, 0))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        public void Split(List<Vector2> points, out List<Wall> results)
        {
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
                indexFloat.value = Vector2.Distance(point, Corner0.ToPoint2());
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
                if (!Architect.TryGetCorner(position, out Corner corner))
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
                corner0.RemoveWall(this);
                corner1.RemoveWall(this);
            }

            // 释放
            ListPool<IndexFloat>.Release(indexFloats);
        }

        public override void Bind()
        {
            base.Bind();
            // 建立联系
            corner0.AddWall(this);
            corner1.AddWall(this);

            // 初始化事件
            corner0.PropertyChanged += Corner0_PropertyChanged;
            corner1.PropertyChanged += Corner1_PropertyChanged;

            corner0.Walls.ItemChanged += Corner0_Walls_ItemChanged;
            corner1.Walls.ItemChanged += Corner1_Walls_ItemChanged;

            corner0.Walls.CollectionChanged += Corner0_Walls_CollectionChanged;
            corner1.Walls.CollectionChanged += Corner1_Walls_CollectionChanged;

            holes.ItemChanged += Holes_ItemChanged;
            holes.CollectionChanged += Holes_CollectionChanged;
            // 同步
            corner0.Sync();
            corner1.Sync();
            Sync();
        }

        public override void Unbind()
        {
            base.Unbind();
            corner0.PropertyChanged -= Corner0_PropertyChanged;
            corner1.PropertyChanged -= Corner1_PropertyChanged;

            corner0.Walls.ItemChanged -= Corner0_Walls_ItemChanged;
            corner1.Walls.ItemChanged -= Corner1_Walls_ItemChanged;

            corner0.Walls.CollectionChanged -= Corner0_Walls_CollectionChanged;
            corner1.Walls.CollectionChanged -= Corner1_Walls_CollectionChanged;

            corner0.RemoveWall(this);
            corner1.RemoveWall(this);
            //this.corner0 = null;
            //this.corner1 = null;

            int count = RelatedRooms.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                Room room = m_RelatedRooms[i].Room;
                int status = m_RelatedRooms[i].Status;
                if (status == 0)
                {
                    room.InnerWalls.Remove(this);
                }
                else if (status == 1)
                {
                    room.Walls.Remove(this);
                }
            }
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            Wall entity = new Wall()
            {
                height = this.height,
                thickness = this.thickness,
            };

            return entity;
        }

    }
}
