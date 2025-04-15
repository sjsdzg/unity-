using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Architectural;
using XFramework.Common;

namespace XFramework.IO
{
    public class ArchReader
    {
        private bool isBinary;
        private IValueReader chunk;
        private Document doc;

        public Document Read(Stream stream)
        {
            long startPosition = stream.Position;
            if (stream == null)
                throw new ArgumentException("stream");

            string version = CheckArchFileVersion(stream, out this.isBinary);
            stream.Position = startPosition;

            //if (this.isBinary)
            //{
                
            //}
            //else
                this.chunk = new TextValueReader(new StreamReader(stream, Encoding.UTF8));

            this.doc = new Document();
            while (this.chunk.ReadString() != ArchValueCode.EndOfFile)
            {
                if (this.chunk.ReadString() == ArchValueCode.BeginSection)
                {
                    this.chunk.Next();
                    switch (this.chunk.ReadString())
                    {
                        case ArchValueCode.HeaderSection:
                            ReadHeader();
                            break;
                        case ArchValueCode.FloorsSection:
                            ReadFloors();
                            break;
                        default:
                            break;
                    }
                }
                this.chunk.Next();
            }

            return doc;
        }

        private void ReadHeader()
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                string name = this.chunk.ReadString();
                this.chunk.Next();

                switch (name)
                {
                    case HeaderVariableCode.Version:
                        string version = this.chunk.ReadString();
                        this.doc.HeaderVariables.Version = version;
                        this.chunk.Next();
                        break;
                    case HeaderVariableCode.TDCreate:
                        long tdCreate = this.chunk.ReadLong();
                        this.doc.HeaderVariables.TDCreate = DateTimeUtil.OfEpochMilli(tdCreate);
                        this.chunk.Next();
                        break;
                    case HeaderVariableCode.TDUpdate:
                        long tdUpdate = this.chunk.ReadLong();
                        this.doc.HeaderVariables.TDUpdate = DateTimeUtil.OfEpochMilli(tdUpdate);
                        this.chunk.Next();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReadFloors()
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginFloor:
                        ReadFloor();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadFloor()
        {
            this.doc.AddFloor();
            string id;
            int number;
            float height;
            float thickness;
            float altitude;

            this.chunk.Next();
            while (this.chunk.Code != 0)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.number:
                        number = this.chunk.ReadInt();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.altitude:
                        altitude = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            while (this.chunk.ReadString() != ArchValueCode.EndFloor)
            {
                if (this.chunk.ReadString() == ArchValueCode.BeginSection)
                {
                    this.chunk.Next();
                    switch (this.chunk.ReadString())
                    {
                        case ArchValueCode.CornersSection:
                            ReadCorners();
                            break;
                        case ArchValueCode.WallsSection:
                            ReadWalls();
                            break;
                        case ArchValueCode.DoorsSection:
                            ReadDoors();
                            break;
                        case ArchValueCode.WindowsSection:
                            ReadWindows();
                            break;
                        case ArchValueCode.PassesSection:
                            ReadPasses();
                            break;
                        case ArchValueCode.RoomsSection:
                            ReadRooms();
                            break;
                        case ArchValueCode.EquipmentsSection:
                            ReadEquipments();
                            break;
                        case ArchValueCode.AreasSection:
                            ReadAreas();
                            break;
                        case ArchValueCode.GroupsSection:
                            ReadGroups();
                            break;
                        default:
                            break;
                    }
                }
                this.chunk.Next();
            }
        }

        private void ReadCorners()
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginCorner:
                        ReadCorner();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadCorner()
        {
            string id = string.Empty;
            string special = string.Empty;
            Vector3 position = Vector3.zero;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndCorner)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // 添加corner
            Corner corner = new Corner();
            corner.Id = id;
            corner.Special = special;
            corner.Position = position;
            this.doc.CurrentFloor.Corners.Add(corner);
        }

        private void ReadWalls()
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginWall:
                        ReadWall();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadWall()
        {
            string id = string.Empty;
            string special = string.Empty;
            float height = 0;
            float thickness = 0;
            string corner0_id = string.Empty;
            string corner1_id = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndWall)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.corner0_id:
                        corner0_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.corner1_id:
                        corner1_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // 添加Wall
            Corner corner0 = this.doc.CurrentFloor.Corners.Find(x => x.Id.Equals(corner0_id));
            Corner corner1 = this.doc.CurrentFloor.Corners.Find(x => x.Id.Equals(corner1_id));
            Wall wall = new Wall(corner0, corner1);
            wall.Id = id;
            wall.Special = special;
            wall.Height = height;
            wall.Thickness = thickness;
            this.doc.CurrentFloor.Walls.Add(wall);
        }

        private void ReadDoors()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginDoor:
                        ReadDoor();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadDoor()
        {
            string id = string.Empty;
            string special = string.Empty;
            DoorType doorType = DoorType.Single;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float length = 0;
            float height = 0;
            float thickness = 0;
            float bottom = 0;
            string wall_id = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndDoor)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.doorType:
                        doorType = EnumUtil.Parse<DoorType>(this.chunk.ReadString());
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.length:
                        length = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.wall_id:
                        wall_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // 添加门
            Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wall_id));
            Door door = new Door();
            door.Id = id;
            door.Special = special;
            door.DoorType = doorType;
            door.Position = position;
            door.Rotation = rotation;
            door.Length = length;
            door.Height = height;
            door.Thickness = thickness;
            door.Bottom = bottom;
            door.Wall = wall;
            this.doc.CurrentFloor.Doors.Add(door);
        }

        private void ReadWindows()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginWindow:
                        ReadWindow();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadWindow()
        {
            string id = string.Empty;
            string special = string.Empty;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float length = 0;
            float height = 0;
            float thickness = 0;
            float bottom = 0;
            string wall_id = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndWindow)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.length:
                        length = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.wall_id:
                        wall_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // 添加窗子
            Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wall_id));
            Window window = new Window();
            window.Id = id;
            window.Special = special;
            window.Position = position;
            window.Rotation = rotation;
            window.Length = length;
            window.Height = height;
            window.Thickness = thickness;
            window.Bottom = bottom;
            window.Wall = wall;
            this.doc.CurrentFloor.Windows.Add(window);
        }

        private void ReadPasses()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginPass:
                        ReadPass();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadPass()
        {
            string id = string.Empty;
            string special = string.Empty;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float length = 0;
            float height = 0;
            float thickness = 0;
            float bottom = 0;
            string wall_id = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndPass)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.length:
                        length = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.wall_id:
                        wall_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // 添加垭口
            Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wall_id));
            Pass pass = new Pass();
            pass.Id = id;
            pass.Special = special;
            pass.Position = position;
            pass.Rotation = rotation;
            pass.Length = length;
            pass.Height = height;
            pass.Thickness = thickness;
            pass.Bottom = bottom;
            pass.Wall = wall;
            this.doc.CurrentFloor.Passes.Add(pass);
        }

        private void ReadRooms()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginRoom:
                        ReadRoom();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadRoom()
        {
            string id = string.Empty;
            string special = string.Empty;
            bool active = true;
            string name = string.Empty;
            float area = 0;
            List<string> wallIds = new List<string>();
            List<string> innerWallIds = new List<string>();

            this.chunk.Next();
            while (this.chunk.Code != 0)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.active:
                        active = this.chunk.ReadBool();
                        this.chunk.Next();
                        break;
                    case ArchCode.name:
                        name = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.area:
                        area = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            while (this.chunk.ReadString() != ArchValueCode.EndRoom)
            {
                if (this.chunk.ReadString() == ArchValueCode.BeginSection)
                {
                    this.chunk.Next();
                    switch (this.chunk.ReadString())
                    {
                        case ArchValueCode.WallsSection:
                            ReadWallIds(ref wallIds);
                            break;
                        case ArchValueCode.InnerWallsSection:
                            ReadWallIds(ref innerWallIds);
                            break;
                        default:
                            break;
                    }
                }
                this.chunk.Next();
            }

            List<Wall> walls = new List<Wall>();
            foreach (var wallId in wallIds)
            {
                Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wallId));
                walls.Add(wall);
            }

            List<Wall> innerWalls = new List<Wall>();
            foreach (var wallId in innerWallIds)
            {
                Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wallId));
                innerWalls.Add(wall);
            }

            // 添加房间
            Room room = new Room(walls, innerWalls);
            room.Id = id;
            room.Special = special;
            room.Active = active;
            room.Name = name;
            room.SyncContour();
            room.SyncInnerContour();
            this.doc.CurrentFloor.Rooms.Add(room);
        }

        private void ReadWallIds(ref List<string> wallIds)
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.wall_id:
                        wallIds.Add(this.chunk.ReadString());
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadGroups()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginGroup:
                        ReadGroup();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadGroup()
        {
            string id = string.Empty;
            string special = string.Empty;
            string name = string.Empty;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one;

            List<string> ids = new List<string>();
            List<string> innerWallIds = new List<string>();

            this.chunk.Next();
            while (this.chunk.Code != 0)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.name:
                        name = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.scale:
                        scale = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            // 添加房间
            Group group = new Group();
            group.Id = id;
            group.Special = special;
            group.Name = name;
            group.Position = position;
            group.Rotation = rotation;
            group.Scale = scale;
            
            while (this.chunk.ReadString() != ArchValueCode.EndGroup)
            {
                if (this.chunk.ReadString() == ArchValueCode.BeginSection)
                {
                    this.chunk.Next();
                    switch (this.chunk.ReadString())
                    {
                        case ArchValueCode.MembersSection:
                            ReadMembers(group);
                            break;
                        case ArchValueCode.RawWallsSection:
                            ReadRawWalls(group);
                            break;
                        default:
                            break;
                    }
                }
                this.chunk.Next();
            }

            this.doc.CurrentFloor.Groups.Add(group);
        }

        public void ReadRawWalls(Group group)
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginRawWall:
                        ReadRawWall(group);
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        public void ReadRawWall(Group group)
        {
            string id = string.Empty;
            float height = 0;
            float thickness = 0;
            string corner0_id = string.Empty;
            string corner1_id = string.Empty;
            string linkedWalls = string.Empty;
            string relatedRooms = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndRawWall)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.corner0_id:
                        corner0_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.corner1_id:
                        corner1_id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.height:
                        height = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.thickness:
                        thickness = this.chunk.ReadFloat();
                        this.chunk.Next();
                        break;
                    case ArchCode.linkedWalls:
                        linkedWalls = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.relatedRooms:
                        relatedRooms = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
            // RawWall
            Corner corner0 = this.doc.CurrentFloor.Corners.Find(x => x.Id.Equals(corner0_id));
            Corner corner1 = this.doc.CurrentFloor.Corners.Find(x => x.Id.Equals(corner1_id));
            Wall rawWall = new Wall();
            rawWall.Active = false;
            rawWall.Id = id;
            rawWall.SetCorners(corner0, corner1);
            rawWall.Height = height;
            rawWall.Thickness = thickness;
            group.RawWalls.Add(rawWall);

            // linkedWalls
            string[] splits = linkedWalls.Split(',');
            foreach (var split in splits)
            {
                string wallId = split;
                Wall wall = this.doc.CurrentFloor.Walls.Find(x => x.Id.Equals(wallId));
                group.AddLinkedWall(rawWall, wall);
            }

            // linkedRooms
            splits = relatedRooms.Split(',');
            foreach (var split in splits)
            {
                string[] temp = split.Split('&');
                string roomId = temp[0];
                int flags = int.Parse(temp[1]);
                Room room = this.doc.CurrentFloor.Rooms.Find(x => x.Id.Equals(roomId));
                rawWall.AddRoom(room, flags);
            }
        }

        public void ReadMembers(Group group)
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginMember:
                        ReadMember(group);
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        public void ReadMember(Group group)
        {
            string id = string.Empty;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Vector3 scale = Vector3.one;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndMember)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.scale:
                        scale = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            EntityObject entity = this.doc.CurrentFloor.GetEntity(id);
            if (entity != null)
            {
                entity.Owner = group;
                Group.Member member = new Group.Member(entity);
                member.Position = position;
                member.Rotation = rotation;
                member.Scale = scale;

                group.Members.Add(member);
            }

        }

        private void ReadAreas()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginArea:
                        ReadArea();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadArea()
        {
            string id = string.Empty;
            string special = string.Empty;
            bool active = true;
            string name = string.Empty;
            List<Vector3> points = new List<Vector3>();

            this.chunk.Next();
            while (this.chunk.Code != 0)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.active:
                        active = this.chunk.ReadBool();
                        this.chunk.Next();
                        break;
                    case ArchCode.name:
                        name = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            while (this.chunk.ReadString() != ArchValueCode.EndArea)
            {
                if (this.chunk.ReadString() == ArchValueCode.BeginSection)
                {
                    this.chunk.Next();
                    switch (this.chunk.ReadString())
                    {
                        case ArchValueCode.PointsSection:
                            ReadPoints(ref points);
                            break;
                        default:
                            break;
                    }
                }
                this.chunk.Next();
            }


            // 添加区域
            Area area = new Area();
            area.Id = id;
            area.Special = special;
            area.Active = active;
            area.Name = name;
            area.SetContour(points);

            this.doc.CurrentFloor.Areas.Add(area);
        }

        public void ReadPoints(ref List<Vector3> points)
        {
            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.@Vector3:
                        points.Add(this.chunk.ReadVector3());
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }


        private void ReadEquipments()
        {
            while (this.chunk.ReadString() != ArchValueCode.EndSection)
            {
                switch (this.chunk.ReadString())
                {
                    case ArchValueCode.BeginEquipment:
                        ReadEquipment();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }
        }

        private void ReadEquipment()
        {
            string id = string.Empty;
            string special = string.Empty;
            bool active = true;
            string name = string.Empty;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            string address = string.Empty;

            this.chunk.Next();
            while (this.chunk.ReadString() != ArchValueCode.EndEquipment)
            {
                switch (this.chunk.Code)
                {
                    case ArchCode.id:
                        id = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.special:
                        special = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.active:
                        active = this.chunk.ReadBool();
                        this.chunk.Next();
                        break;
                    case ArchCode.name:
                        name = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    case ArchCode.position:
                        position = this.chunk.ReadVector3();
                        this.chunk.Next();
                        break;
                    case ArchCode.rotation:
                        rotation = this.chunk.ReadQuaternion();
                        this.chunk.Next();
                        break;
                    case ArchCode.address:
                        address = this.chunk.ReadString();
                        this.chunk.Next();
                        break;
                    default:
                        this.chunk.Next();
                        break;
                }
            }

            // 添加区域
            Equipment equipment = new Equipment();
            equipment.Id = id;
            equipment.Special = special;
            equipment.Active = active;
            equipment.Name = name;
            equipment.Position = position;
            equipment.Rotation = rotation;
            equipment.Address = address;

            this.doc.CurrentFloor.Equipments.Add(equipment);
        }

        public static string CheckArchFileVersion(Stream stream, out bool isBinary)
        {
            string value = CheckHeaderVariable(stream, HeaderVariableCode.Version, out isBinary);
            return value;
        }

        public static string CheckHeaderVariable(Stream stream, string headerVariableCode, out bool isBinary)
        {
            long startPosition = stream.Position;
            IValueReader chunk;
            isBinary = IsBinary(stream);
            stream.Position = startPosition;

            //if (isBinary)
            //{
            //    // TODO
            //}
            //else
            chunk = new TextValueReader(new StreamReader(stream));

            chunk.Next();
            while (chunk.ReadString() != ArchValueCode.EndOfFile)
            {
                chunk.Next();
                if (chunk.ReadString() == ArchValueCode.HeaderSection)
                {
                    chunk.Next();
                    while (chunk.ReadString() != ArchValueCode.EndSection)
                    {
                        string name = chunk.ReadString();
                        chunk.Next();

                        if (name == headerVariableCode)
                        {
                            return chunk.ReadString();
                        }
                    }

                    return null;
                }
            }

            return null;
        }

        public static bool IsBinary(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte[] sentinel = reader.ReadBytes(15);
            StringBuilder sb = new StringBuilder(11);
            for (int i = 0; i < 11; i++)
                sb.Append((char)sentinel[i]);

            return sb.ToString() == "Binary ARCH";
        }
    }
}
