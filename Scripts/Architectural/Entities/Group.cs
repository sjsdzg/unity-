using System;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class GroupChangedArgs : EventArgs
    {
        private readonly EntityObject item;
        /// <summary>
        /// Gets the entity that is being added or removed.
        /// </summary>
        public EntityObject Item
        {
            get { return item; }
        }

        public GroupChangedArgs(EntityObject item)
        {
            this.item = item;
        }
    }

    public class Group : EntityObject
    {
        public delegate void GroupChangedHandler(Group sender, GroupChangedArgs e);

        public override EntityType Type
        {
            get { return EntityType.Group; }
        }

        /// <summary>
        /// 实体添加时，触发
        /// </summary>
        public event GroupChangedHandler EntityAdded;

        /// <summary>
        /// 实体移除时，触发
        /// </summary>
        public event GroupChangedHandler EntityRemoved;

        private Vector3 position;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { SetProperty(ref position, value, Constants.position); }
        }

        private Quaternion rotation;
        /// <summary>
        /// 旋转
        /// </summary>
        public Quaternion Rotation
        {
            get { return rotation; }
            set { SetProperty(ref rotation, value, Constants.rotation); }
        }

        private Vector3 scale;
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value, Constants.scale); }
        }

        /// <summary>
        /// localToWorldMatrix
        /// </summary>
        public Matrix4x4 localToWorldMatrix { get; set; }

        /// <summary>
        /// worldToLocalToMatrix
        /// </summary>
        public Matrix4x4 worldToLocalToMatrix { get; set; }

        private List<Member> members = new List<Member>();
        /// <summary>
        /// 组员列表
        /// </summary>
        public List<Member> Members
        {
            get { return members; }
        }

        /// <summary>
        /// 组内墙角点 对应的墙角点
        /// </summary>
        public readonly Dictionary<Corner, Corner> m_LinkedCorners = new Dictionary<Corner, Corner>();

        /// <summary>
        /// 组内墙体 对应的墙体列表
        /// </summary>
        public readonly Dictionary<Wall, List<Wall>> m_LinkedWalls = new Dictionary<Wall, List<Wall>>();

        private List<Wall> m_RawWalls = new List<Wall>();
        /// <summary>
        /// 原始墙列表
        /// </summary>
        public List<Wall> RawWalls
        {
            get { return m_RawWalls; }
        }

        public Group()
        {
            Name = "未命名";
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
            this.scale = Vector3.one;
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.position:
                    UpdateMembers();
                    OnTransformChanged();
                    
                    break;
                case Constants.rotation:
                    UpdateMembers();
                    OnTransformChanged();
                    break;
                case Constants.scale:
                    UpdateMembers();
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="item"></param>
        protected void AddRange(List<EntityObject> items)
        {
            foreach (var item in items)
            {
                if (Contains(item))
                    continue;

                Member member = new Member(item);
                item.Owner = this;

                AddMember(member);

                EntityAdded?.Invoke(this, new GroupChangedArgs(member.Entity));
            }
            // 更新轴心
            UpdatePovit();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="item"></param>
        public void Add(EntityObject item)
        {
            if (Contains(item))
                return;

            Member member = new Member(item);
            item.Owner = this;

            AddMember(member);

            EntityAdded?.Invoke(this, new GroupChangedArgs(member.Entity));
            // 更新轴心
            UpdatePovit();
        }

        public void AddMember(Member member)
        {
            EntityObject entity = member.Entity;
            if (entity.Type == EntityType.Corner)
            {
                this.members.Insert(0, member);
            }
            else if (member.Entity.Type == EntityType.Wall)
            {
                int index = this.members.FindLastIndex(x => x.Entity.Type == EntityType.Corner);
                if (index == -1)
                {
                    this.members.Insert(0, member);
                }
                else
                {
                    this.members.Insert(index + 1, member);
                }

                // rawWall
                Wall wall = (Wall)entity;
                Wall rawWall = RawWalls.Find(x => x.Corner0.Equals(wall.Corner0) && x.Corner1.Equals(wall.Corner1));
                if (rawWall == null)
                {
                    rawWall = (Wall)wall.Clone();
                    rawWall.Active = false;
                    rawWall.SetCorners(wall.Corner0, wall.Corner1);
                    m_RawWalls.Add(rawWall);

                    foreach (var linkedRoom in wall.RelatedRooms)
                    {
                        rawWall.AddRoom(linkedRoom.Room, linkedRoom.Status);
                    }
                }

                AddLinkedWall(rawWall, wall);
            }
            else
            {
                this.members.Add(member);
            }
        }

        public bool Contains(EntityObject item)
        {
            return this.members.Exists(x => x.Entity.Equals(item));
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="item"></param>
        public void Remove(EntityObject item)
        {
            if (Contains(item))
            {
                Member member = this.members.Find(x => x.Entity.Equals(item));
                this.members.Remove(member);
                item.Owner = null;
                EntityRemoved?.Invoke(this, new GroupChangedArgs(member.Entity));
                // 更新轴心
                UpdatePovit();
            }
        }



        /// <summary>
        /// 打组
        /// </summary>
        public static Group Combine(List<EntityObject> items)
        {
            List<EntityObject> entities = new List<EntityObject>();
            foreach (var item in items)
            {
                if (TryGetDependencies(item, out List<EntityObject> dependencies))
                {
                    foreach (var dependency in dependencies)
                    {
                        if (!entities.Contains(dependency))
                        {
                            entities.Add(dependency);
                        }
                    }
                }

                if (!entities.Contains(item))
                {
                    entities.Add(item);
                }
            }

            if (entities.Count < 2)
            {
                Debug.LogWarning("请至少选择两个对象进行合并。");
                return null;
            }

            // group
            Group group = new Group();
            group.AddRange(entities);
            return group;
        }

        /// <summary>
        /// 探究 entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static bool TryGetDependencies(EntityObject entity, out List<EntityObject> entities)
        {
            entities = new List<EntityObject>();
            if (entity.Type == EntityType.Wall)
            {
                Wall wall = entity as Wall;
                entities.Add(wall.Corner0);
                entities.Add(wall.Corner1);
                return true;
            }
            else if (entity.Type == EntityType.Room)
            {
                Room room = entity as Room;
                foreach (var wall in room.Walls)
                {
                    if (!entities.Contains(wall.Corner0))
                    {
                        entities.Add(wall.Corner0);
                    }

                    if (!entities.Contains(wall.Corner1))
                    {
                        entities.Add(wall.Corner1);
                    }

                    if (!entities.Contains(wall))
                    {
                        entities.Add(wall);
                    }
                }

                foreach (var wall in room.InnerWalls)
                {
                    if (!entities.Contains(wall.Corner0))
                    {
                        entities.Add(wall.Corner0);
                    }

                    if (!entities.Contains(wall.Corner1))
                    {
                        entities.Add(wall.Corner1);
                    }

                    if (!entities.Contains(wall))
                    {
                        entities.Add(wall);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 解组
        /// </summary>
        public void Ungroup()
        {
            for (int i = members.Count - 1; i >= 0; i--)
            {
                Member member = this.members[i];
                EntityRemoved?.Invoke(this, new GroupChangedArgs(member.Entity));
                member.Entity.Owner = null;
                this.members.RemoveAt(i);
            }
            // dispose
            this.Unbind();
        }

        /// <summary>
        /// 更新轴心
        /// </summary>
        public virtual void UpdatePovit()
        {
            List<Vector3> array = new List<Vector3>();
            foreach (var member in members)
            {
                EntityObject entity = member.Entity;
                switch (entity.Type)
                {
                    case EntityType.Corner:
                        Corner corner = entity as Corner;
                        array.Add(corner.Position);
                        break;
                    //case EntityType.Equipment:
                    //    Equipment equipment = entity as Equipment;
                    //    array.Add(equipment.Position);
                    //    break;
                    default:
                        break;
                }
            }

            // 计算中心位置
            this.position = MathUtility.Average(array);
            localToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);
            worldToLocalToMatrix = localToWorldMatrix.inverse;

            // 更改成员相对位置
            foreach (var member in members)
            {
                EntityObject entity = member.Entity;
                switch (entity.Type)
                {
                    case EntityType.Corner:
                        Corner corner = entity as Corner;
                        member.Position = worldToLocalToMatrix.MultiplyPoint(corner.Position);
                        break;
                    case EntityType.Equipment:
                        Equipment equipment = entity as Equipment;
                        member.Position = worldToLocalToMatrix.MultiplyPoint(equipment.Position);
                        break;
                    case EntityType.Group:
                        Group group = entity as Group;
                        member.Position = worldToLocalToMatrix.MultiplyPoint(group.Position);
                        break;
                    default:
                        break;
                }
            }

            OnTransformChanged();
        }

        /// <summary>
        /// 更新成员
        /// </summary>
        public virtual void UpdateMembers()
        {
            localToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);

            // 更改成员位置 (世界坐标系)
            foreach (var member in members)
            {
                EntityObject entity = member.Entity;
                switch (entity.Type)
                {
                    case EntityType.Corner:
                        Corner corner = entity as Corner;
                        corner.Position = localToWorldMatrix.MultiplyPoint(member.Position);
                        break;
                    case EntityType.Equipment:
                        Equipment equipment = entity as Equipment;
                        equipment.Position = localToWorldMatrix.MultiplyPoint(member.Position);
                        break;
                    case EntityType.Group:
                        Group group = entity as Group;
                        group.Position = localToWorldMatrix.MultiplyPoint(member.Position);
                        break;
                    default:
                        break;
                }
            }
        }

        public bool TryGetCorners(out List<Corner> corners)
        {
            corners = new List<Corner>();
            foreach (var member in members)
            {
                if (member.Entity.Type == EntityType.Corner)
                {
                    corners.Add((Corner)member.Entity);
                }
            }

            if (corners.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool TryGetWalls(out List<Wall> walls)
        {
            walls = new List<Wall>();
            foreach (var member in members)
            {
                if (member.Entity.Type == EntityType.Wall)
                {
                    walls.Add((Wall)member.Entity);
                }
            }

            if (walls.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool TryGetRooms(out List<Room> rooms)
        {
            rooms = new List<Room>();
            foreach (var member in members)
            {
                if (member.Entity.Type == EntityType.Room)
                {
                    rooms.Add((Room)member.Entity);
                }
            }

            if (rooms.Count > 0)
            {
                return true;
            }

            return false;
        }



        public void AddLinkedWall(Wall rawWall, Wall wall)
        {
            if (!m_RawWalls.Contains(rawWall))
                return;

            if (!m_LinkedWalls.TryGetValue(rawWall, out List<Wall> linkedWalls))
            {
                linkedWalls = new List<Wall>();
                m_LinkedWalls.Add(rawWall, linkedWalls);
            }

            if (!linkedWalls.Contains(wall))
            {
                foreach (var relatedRoom in rawWall.RelatedRooms)
                {
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

                linkedWalls.Add(wall);
            }

        }

        public void AddRangeLinkedWall(Wall rawWall, List<Wall> walls)
        {
            if (!m_RawWalls.Contains(rawWall))
                return;

            if (!m_LinkedWalls.TryGetValue(rawWall, out List<Wall> linkedWalls))
            {
                linkedWalls = new List<Wall>();
                m_LinkedWalls.Add(rawWall, linkedWalls);
            }

            foreach (var wall in walls)
            {
                if (!linkedWalls.Contains(wall))
                {
                    foreach (var linkedRoom in rawWall.RelatedRooms)
                    {
                        Room room = linkedRoom.Room;
                        if (linkedRoom.Status == 0 && !room.InnerWalls.Contains(wall)) //内墙墙
                        {
                            room.InnerWalls.Add(wall);
                        }
                        else if (linkedRoom.Status == 1 && !room.Walls.Contains(wall)) //外墙
                        {
                            room.Walls.Add(wall);
                        }
                    }

                    linkedWalls.Add(wall);
                }
            }
        }

        public void RemoveLinkedWall(Wall rawWall, Wall wall)
        {
            if (!m_RawWalls.Contains(rawWall))
                return;

            if (m_LinkedWalls.TryGetValue(rawWall, out List<Wall> linkedWalls))
            {
                if (linkedWalls.Contains(wall))
                {
                    foreach (var relatedRoom in rawWall.RelatedRooms)
                    {
                        Room room = relatedRoom.Room;
                        if (relatedRoom.Status == 0) //内墙墙
                        {
                            room.InnerWalls.Remove(wall);
                        }
                        else if (relatedRoom.Status == 1) //外墙
                        {
                            room.Walls.Remove(wall);
                        }
                    }

                    linkedWalls.Remove(wall);
                }
                
            }
        }

        public void ClearLinkedWall(Wall rawWall)
        {
            if (!m_RawWalls.Contains(rawWall))
                return;

            if (m_LinkedWalls.TryGetValue(rawWall, out List<Wall> linkedWalls))
            {
                foreach (var linkedWall in linkedWalls)
                {
                    foreach (var relatedRoom in rawWall.RelatedRooms)
                    {
                        Room room = relatedRoom.Room;
                        if (relatedRoom.Status == 0) //内墙墙
                        {
                            room.InnerWalls.Remove(linkedWall);
                        }
                        else if (relatedRoom.Status == 1) //外墙
                        {
                            room.Walls.Remove(linkedWall);
                        }
                    }
                }
                linkedWalls.Clear();
            }
        }

        public bool TryGetLinkedWalls(Wall rawWall, out List<Wall> linkedWalls)
        {
            return m_LinkedWalls.TryGetValue(rawWall, out linkedWalls);
        }

        public bool ContainsLinkedWall(Wall linkedWall)
        {
            foreach (var key in m_LinkedWalls.Keys)
            {
                if (m_LinkedWalls[key].Contains(linkedWall))
                {
                    return true;
                }
            }

            return false;
        }

        public Wall GetRawWallByLinkedWall(Wall linkedWall)
        {
            foreach (var key in m_LinkedWalls.Keys)
            {
                if (m_LinkedWalls[key].Contains(linkedWall))
                {
                    return key;
                }
            }

            return null;
        }

        /// <summary>
        /// 替换 corner
        /// </summary>
        /// <param name="rawCorner"></param>
        public void ReplaceCorner(Corner rawCorner, Corner replaceCorner, Vector3 position)
        {
            if (TryGetCorners(out List<Corner> corners))
            {
                bool isEqual = rawCorner.Equals(replaceCorner);
                rawCorner.Active = isEqual;

                if (corners.Contains(rawCorner))
                {
                    if (m_LinkedCorners.ContainsKey(rawCorner))
                    {
                        m_LinkedCorners[rawCorner] = replaceCorner;
                    }
                    else
                    {
                        m_LinkedCorners.Add(rawCorner, replaceCorner);
                    }
                }

                foreach (var rawWall in RawWalls)
                {
                    if (TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls))
                    {
                        foreach (var linkedWall in linkedWalls)
                        {
                            if (MathUtility.Appr(linkedWall.Corner0.Position, position))
                            {
                                linkedWall.Corner0 = replaceCorner;
                            }

                            if (MathUtility.Appr(linkedWall.Corner1.Position, position))
                            {
                                linkedWall.Corner1 = replaceCorner;
                            }
                        }
                    }
                }
            }
        }

        public override object Clone()
        {
            Group group = new Group()
            {
                Name = this.Name,
                position = this.position,
                rotation = this.rotation,
                scale = this.scale,
            };

            List<Corner> cloneCorners = new List<Corner>();
            List<Wall> cloneWalls = new List<Wall>();
            bool isCloneRawWall = false;

            foreach (var member in members)
            {
                EntityObject entity = member.Entity;

                if (entity.Type != EntityType.Corner && !isCloneRawWall)
                {
                    // RawWall
                    foreach (var rawWall in RawWalls)
                    {
                        Wall cloneWall = (Wall)rawWall.Clone();
                        Corner cloneCorner0 = cloneCorners.Find(x => ArchitectUtility.IsSame(x, rawWall.Corner0));
                        Corner cloneCorner1 = cloneCorners.Find(x => ArchitectUtility.IsSame(x, rawWall.Corner1));
                        cloneWall.SetCorners(cloneCorner0, cloneCorner1);
                        cloneWalls.Add(cloneWall);
                        // RawWall
                        Wall cloneRawWall = (Wall)cloneWall.Clone();
                        cloneRawWall.Active = false;
                        cloneRawWall.SetCorners(cloneWall.Corner0, cloneWall.Corner1);
                        group.RawWalls.Add(cloneRawWall);
                        group.AddLinkedWall(cloneRawWall, cloneWall);
                        // Member
                        Member _cloneMember = (Member)member.Clone();
                        _cloneMember.Entity = cloneWall;
                        _cloneMember.Entity.Owner = group;
                        group.members.Add(_cloneMember);
                    }
                    // 
                    isCloneRawWall = true;
                }

                // Member
                Member cloneMember = (Member)member.Clone();
                switch (entity.Type)
                {
                    case EntityType.Corner:
                        Corner corner = (Corner)entity;
                        Corner cloneCorner = (Corner)corner.Clone();
                        cloneCorners.Add(cloneCorner);
                        // Member
                        cloneMember.Entity = cloneCorner;
                        cloneMember.Entity.Owner = group;
                        group.members.Add(cloneMember);
                        break;
                    case EntityType.Wall:
                        //if (!isCloneRawWall)
                        //{
                        //    // RawWall
                        //    foreach (var rawWall in RawWalls)
                        //    {
                        //        Wall cloneWall = (Wall)rawWall.Clone();
                        //        Corner cloneCorner0 = cloneCorners.Find(x => ArchitectUtility.IsSame(x, rawWall.Corner0));
                        //        Corner cloneCorner1 = cloneCorners.Find(x => ArchitectUtility.IsSame(x, rawWall.Corner1));
                        //        cloneWall.SetCorners(cloneCorner0, cloneCorner1);
                        //        cloneWalls.Add(cloneWall);
                        //        // RawWall
                        //        Wall cloneRawWall = (Wall)cloneWall.Clone();
                        //        cloneRawWall.Active = false;
                        //        cloneRawWall.SetCorners(cloneWall.Corner0, cloneWall.Corner1);
                        //        group.RawWalls.Add(cloneRawWall);
                        //        group.AddLinkedWall(cloneRawWall, cloneWall);
                        //        // Member
                        //        Member _cloneMember = (Member)member.Clone();
                        //        _cloneMember.Entity = cloneWall;
                        //        _cloneMember.Entity.Owner = group;
                        //        group.members.Add(_cloneMember);
                        //    }
                        //    // 
                        //    isCloneRawWall = true;
                        //}
                        break;
                    case EntityType.Room:
                        Room room = (Room)entity;
                        Room cloneRoom = (Room)room.Clone();

                        List<Wall> cloneRoomWalls = new List<Wall>();
                        foreach (var _wall in room.Walls)
                        {
                            Wall _rawWall = this.GetRawWallByLinkedWall(_wall);
                            Wall _cloneWall = cloneWalls.Find(x => ArchitectUtility.IsSame(x, _rawWall));
                            cloneRoomWalls.Add(_cloneWall);

                            Wall _cloneRawWall = group.GetRawWallByLinkedWall(_cloneWall);
                            _cloneRawWall.AddRoom(cloneRoom);
                        }

                        List<Wall> cloneRoomInnerWalls = new List<Wall>();
                        foreach (var _wall in room.InnerWalls)
                        {
                            Wall _rawWall = this.GetRawWallByLinkedWall(_wall);
                            Wall _cloneWall = cloneWalls.Find(x => ArchitectUtility.IsSame(x, _rawWall));
                            cloneRoomWalls.Add(_cloneWall);

                            Wall _cloneRawWall = group.GetRawWallByLinkedWall(_cloneWall);
                            _cloneRawWall.AddRoom(cloneRoom, 0);
                        }

                        cloneRoom.SetWalls(cloneRoomWalls, cloneRoomInnerWalls);
                        // Member
                        cloneMember.Entity = cloneRoom;
                        cloneMember.Entity.Owner = group;
                        group.members.Add(cloneMember);
                        break;
                    case EntityType.Door:
                    case EntityType.Window:
                    case EntityType.Pass:
                        WallHole wallHole = (WallHole)entity;
                        WallHole cloneWallHole = (WallHole)wallHole.Clone();
                        Wall rawWall = this.GetRawWallByLinkedWall(wallHole.Wall);
                        Wall cloneWall = cloneWalls.Find(x => ArchitectUtility.IsSame(x, rawWall));
                        cloneWallHole.Wall = cloneWalls.Find(x => ArchitectUtility.IsSame(x, wallHole.Wall));
                        cloneMember.Entity = cloneWallHole;
                        cloneMember.Entity.Owner = group;
                        group.members.Add(cloneMember);
                        break;
                    default:
                        EntityObject cloneEntity = (EntityObject)entity.Clone();
                        cloneMember.Entity = cloneEntity;
                        cloneMember.Entity.Owner = group;
                        group.members.Add(cloneMember);
                        break;
                }
            }

            return group;
        }

        public override void Unbind()
        {
            base.Unbind();
            members.Clear();
        }

        /// <summary>
        /// 成员
        /// </summary>
        public class Member : ICloneable
        {
            private EntityObject entity;
            /// <summary>
            /// 组件数据
            /// </summary>
            public EntityObject Entity
            {
                get { return entity; }
                set { entity = value; }
            }

            private Vector3 position;
            /// <summary>
            /// 位置
            /// </summary>
            public Vector3 Position
            {
                get { return position; }
                set { position = value; }
            }

            private Quaternion rotation;
            /// <summary>
            /// 旋转
            /// </summary>
            public Quaternion Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            private Vector3 scale;
            /// <summary>
            /// 缩放
            /// </summary>
            public Vector3 Scale
            {
                get { return scale; }
                set { scale = value; }
            }

            public Member()
            {

            }

            public Member(EntityObject entity)
            {
                this.entity = entity;
            }

            public object Clone()
            {
                Member member = new Member()
                {
                    Position = this.position,
                    Rotation = this.rotation,
                    Scale = this.scale,
                };

                return member;
            }
        }
    }
}                                                  