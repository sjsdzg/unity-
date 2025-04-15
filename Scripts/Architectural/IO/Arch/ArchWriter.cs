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
    public class ArchWriter
    {
        private string activeSection = ArchValueCode.Unknown;
        private IValueWriter chunk;
        private Document doc;

        public void Write(Stream stream, Document document)
        {
            this.doc = document;
            // open
            this.Open(stream, Encoding.UTF8);

            // HEADER SECTION
            this.BeginSection(ArchValueCode.HeaderSection);
            foreach (var variable in this.doc.HeaderVariables.Values)
            {
                WriteHeaderVariable(variable);
            }
            this.EndSection();

            // FLOORS SECTION
            this.BeginSection(ArchValueCode.FloorsSection);
            foreach (Floor floor in this.doc.Floors)
            {
                WriteFloor(floor);
            }
            this.EndSection();

            this.Close();
        }
        public void Open(Stream stream, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            this.chunk = new TextValueWriter(new StreamWriter(stream, encoding));
        }

        private void WriteHeaderVariable(HeaderVariable variable)
        {
            string name = variable.Name;
            object value = variable.Value;

            switch (name)
            {
                case HeaderVariableCode.Version:
                    this.chunk.Write(ArchCode.name, name);
                    this.chunk.Write(ArchCode.@string, value);
                    break;
                case HeaderVariableCode.TDCreate:
                    this.chunk.Write(ArchCode.name, name);
                    this.chunk.Write(ArchCode.@long, DateTimeUtil.ToEpochMilli((DateTime)value));
                    break;
                case HeaderVariableCode.TDUpdate:
                    this.chunk.Write(ArchCode.name, name);
                    this.chunk.Write(ArchCode.@long, DateTimeUtil.ToEpochMilli((DateTime)value));
                    break;
                default:
                    break;
            }
        }

        private void WriteFloor(Floor floor)
        {
            // begin floor
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginFloor);
            // id
            this.chunk.Write(ArchCode.id, floor.Id);
            // 序号
            this.chunk.Write(ArchCode.number, floor.Number);
            // 层高
            this.chunk.Write(ArchCode.height, floor.Height);
            // 地板厚度
            this.chunk.Write(ArchCode.thickness, floor.Thickness);
            // 离地高度
            this.chunk.Write(ArchCode.altitude, floor.Altitude);

            // CORNERS SECTION
            this.BeginSection(ArchValueCode.CornersSection);
            foreach (Corner corner in floor.Corners)
            {
                WriteCorner(corner);
            }
            this.EndSection();

            // WALLS SECTION
            this.BeginSection(ArchValueCode.WallsSection);
            foreach (Wall wall in floor.Walls)
            {
                WriteWall(wall);
            }
            this.EndSection();

            // DOORS SECTION
            this.BeginSection(ArchValueCode.DoorsSection);
            foreach (Door door in floor.Doors)
            {
                WriteDoor(door);
            }
            this.EndSection();

            // WINDOWS SECTION
            this.BeginSection(ArchValueCode.WindowsSection);
            foreach (Window window in floor.Windows)
            {
                WriteWindow(window);
            }
            this.EndSection();

            // PASSES SECTION
            this.BeginSection(ArchValueCode.PassesSection);
            foreach (Pass pass in floor.Passes)
            {
                WritePass(pass);
            }
            this.EndSection();

            // ROOMS SECTION
            this.BeginSection(ArchValueCode.RoomsSection);
            foreach (var room in floor.Rooms)
            {
                WriteRoom(room);
            }
            this.EndSection();

            // AREAS SECTION
            this.BeginSection(ArchValueCode.EquipmentsSection);
            foreach (var equipment in floor.Equipments)
            {
                WriteEquipment(equipment);
            }
            this.EndSection();

            // AREAS SECTION
            this.BeginSection(ArchValueCode.AreasSection);
            foreach (var area in floor.Areas)
            {
                WriteArea(area);
            }
            this.EndSection();

            // GROUPS SECTION
            this.BeginSection(ArchValueCode.GroupsSection);
            foreach (var group in floor.Groups)
            {
                WriteGroup(group);
            }
            this.EndSection();

            // end floor
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndFloor);
        }

        private void WriteCorner(Corner corner)
        {
            // begin corner
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginCorner);
            // id
            this.chunk.Write(ArchCode.id, corner.Id);
            // 专业
            this.chunk.Write(ArchCode.special, corner.Special);
            // 位置
            this.chunk.Write(ArchCode.position, corner.Position);
            // end corner
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndCorner);
        }

        private void WriteWall(Wall wall)
        {
            // begin wall
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginWall);
            // id
            this.chunk.Write(ArchCode.id, wall.Id);
            // 专业
            this.chunk.Write(ArchCode.special, wall.Special);
            // 高度
            this.chunk.Write(ArchCode.height, wall.Height);
            // 厚度
            this.chunk.Write(ArchCode.thickness, wall.Thickness);
            // corner0 id
            this.chunk.Write(ArchCode.corner0_id, wall.Corner0.Id);
            // corner1 id
            this.chunk.Write(ArchCode.corner1_id, wall.Corner1.Id);
            // end wall
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndWall);
        }

        private void WriteDoor(Door door)
        {
            // begin door
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginDoor);
            // id
            this.chunk.Write(ArchCode.id, door.Id);
            // 专业
            this.chunk.Write(ArchCode.special, door.Special);
            // 门类型
            this.chunk.Write(ArchCode.doorType, EnumUtil.ToString(door.DoorType));
            // 位置
            this.chunk.Write(ArchCode.position, door.Position);
            // 旋转
            this.chunk.Write(ArchCode.rotation, door.Rotation);
            // 长度
            this.chunk.Write(ArchCode.length, door.Length);
            // 高度
            this.chunk.Write(ArchCode.height, door.Height);
            // 厚度
            this.chunk.Write(ArchCode.thickness, door.Thickness);
            // 离地距离
            this.chunk.Write(ArchCode.bottom, door.Bottom);
            // 依赖墙体 id
            this.chunk.Write(ArchCode.wall_id, door.Wall.Id);
            // end door
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndDoor);
        }

        private void WriteWindow(Window window)
        {
            // begin window
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginWindow);
            // id
            this.chunk.Write(ArchCode.id, window.Id);
            // 专业
            this.chunk.Write(ArchCode.special, window.Special);
            // 位置
            this.chunk.Write(ArchCode.position, window.Position);
            // 旋转
            this.chunk.Write(ArchCode.rotation, window.Rotation);
            // 长度
            this.chunk.Write(ArchCode.length, window.Length);
            // 高度
            this.chunk.Write(ArchCode.height, window.Height);
            // 厚度
            this.chunk.Write(ArchCode.thickness, window.Thickness);
            // 离地距离
            this.chunk.Write(ArchCode.bottom, window.Bottom);
            // 依赖墙体 id
            this.chunk.Write(ArchCode.wall_id, window.Wall.Id);
            // end window
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndWindow);
        }

        private void WritePass(Pass pass)
        {
            // begin window
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginPass);
            // id
            this.chunk.Write(ArchCode.id, pass.Id);
            // 专业
            this.chunk.Write(ArchCode.special, pass.Special);
            // 位置
            this.chunk.Write(ArchCode.position, pass.Position);
            // 旋转
            this.chunk.Write(ArchCode.rotation, pass.Rotation);
            // 长度
            this.chunk.Write(ArchCode.length, pass.Length);
            // 高度
            this.chunk.Write(ArchCode.height, pass.Height);
            // 厚度
            this.chunk.Write(ArchCode.thickness, pass.Thickness);
            // 离地距离
            this.chunk.Write(ArchCode.bottom, pass.Bottom);
            // 依赖墙体 id
            this.chunk.Write(ArchCode.wall_id, pass.Wall.Id);
            // end window
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndPass);
        }

        private void WriteRoom(Room room)
        {
            // begin room
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginRoom);
            // id
            this.chunk.Write(ArchCode.id, room.Id);
            // 专业
            this.chunk.Write(ArchCode.special, room.Special);
            // 标志位
            this.chunk.Write(ArchCode.active, room.Active);
            // 房间名称
            this.chunk.Write(ArchCode.name, room.Name);
            // 房间面积
            this.chunk.Write(ArchCode.area, room.Area);

            // WALLS SECTION
            this.BeginSection(ArchValueCode.WallsSection);
            foreach (Wall wall in room.Walls)
            {
                // wall id
                this.chunk.Write(ArchCode.wall_id, wall.Id);
            }
            this.EndSection();

            // INNER WALLS SECTION
            this.BeginSection(ArchValueCode.InnerWallsSection);
            foreach (Wall innerWall in room.InnerWalls)
            {
                // inner wall id
                this.chunk.Write(ArchCode.wall_id, innerWall.Id);
            }
            this.EndSection();

            // end room
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndRoom);
        }

        private void WriteGroup(Group group)
        {
            // begin room
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginGroup);
            // id
            this.chunk.Write(ArchCode.id, group.Id);
            // 名称
            this.chunk.Write(ArchCode.name, group.Name);
            // 位置
            this.chunk.Write(ArchCode.position, group.Position);
            // 旋转
            this.chunk.Write(ArchCode.rotation, group.Rotation);
            // 旋转
            this.chunk.Write(ArchCode.scale, group.Scale);


            // WALLS SECTION
            this.BeginSection(ArchValueCode.MembersSection);
            foreach (var member in group.Members)
            {
                // begin member
                this.chunk.Write(ArchCode.tag, ArchValueCode.BeginMember);
                // id
                this.chunk.Write(ArchCode.id, member.Entity.Id);
                // 位置
                this.chunk.Write(ArchCode.position, member.Position);
                // 旋转
                this.chunk.Write(ArchCode.rotation, member.Rotation);
                // 旋转
                this.chunk.Write(ArchCode.scale, member.Scale);
                // end member
                this.chunk.Write(ArchCode.tag, ArchValueCode.EndMember);
            }
            this.EndSection();

            StringBuilder sb = new StringBuilder();
            // RAWWALLS SECTION
            this.BeginSection(ArchValueCode.RawWallsSection);
            foreach (var rawWall in group.RawWalls)
            {
                // begin member
                this.chunk.Write(ArchCode.tag, ArchValueCode.BeginRawWall);
                // id
                this.chunk.Write(ArchCode.id, rawWall.Id);
                // corner0
                this.chunk.Write(ArchCode.corner0_id, rawWall.Corner0.Id);
                // corner1
                this.chunk.Write(ArchCode.corner1_id, rawWall.Corner1.Id);
                // 高度
                this.chunk.Write(ArchCode.height, rawWall.Height);
                // 厚度
                this.chunk.Write(ArchCode.thickness, rawWall.Thickness);

                sb.Clear();
                group.TryGetLinkedWalls(rawWall, out List<Wall> linkedWalls);
                int count = linkedWalls.Count;
                for (int i = 0; i < count; i++)
                {
                    var linkedWall = linkedWalls[i];
                    sb.Append(linkedWall.Id);
                    if (i < count - 1)
                    {
                        sb.Append(",");
                    }
                }
                // 链接墙体
                this.chunk.Write(ArchCode.linkedWalls, sb.ToString());

                sb.Clear();
                count = rawWall.RelatedRooms.Count;
                for (int i = 0; i < count; i++)
                {
                    var linkedRoom = rawWall.RelatedRooms[i];
                    sb.Append(linkedRoom.Room.Id);
                    sb.Append("&");
                    sb.Append(linkedRoom.Status);
                    if (i < count - 1)
                    {
                        sb.Append(",");
                    }
                }

                // 链接房间
                this.chunk.Write(ArchCode.relatedRooms, sb.ToString());

                // end member
                this.chunk.Write(ArchCode.tag, ArchValueCode.EndRawWall);
            }
            this.EndSection();

            // end room
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndGroup);
        }

        private void WriteArea(Area area)
        {
            // begin area
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginArea);
            // id
            this.chunk.Write(ArchCode.id, area.Id);
            // 专业
            this.chunk.Write(ArchCode.special, area.Special);
            // 标志位
            this.chunk.Write(ArchCode.active, area.Active);
            // 名称
            this.chunk.Write(ArchCode.name, area.Name);

            // POINTS SECTION
            this.BeginSection(ArchValueCode.PointsSection);
            foreach (Vector3 point in area.Contour)
            {
                // wall id
                this.chunk.Write(ArchCode.Vector3, point);
            }
            this.EndSection();

            // end room
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndArea);
        }

        private void WriteEquipment(Equipment equipment)
        {
            // begin equipment
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginEquipment);
            // id
            this.chunk.Write(ArchCode.id, equipment.Id);
            // 专业
            this.chunk.Write(ArchCode.special, equipment.Special);
            // 标志位
            this.chunk.Write(ArchCode.active, equipment.Active);
            // 名称
            this.chunk.Write(ArchCode.name, equipment.Name);
            // 位置
            this.chunk.Write(ArchCode.position, equipment.Position);
            // 旋转
            this.chunk.Write(ArchCode.rotation, equipment.Rotation);
            // 地址
            this.chunk.Write(ArchCode.address, equipment.Address);

            // end equipment
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndEquipment);
        }

        /// <summary>
        /// Opens a new section
        /// </summary>
        /// <param name="section"></param>
        private void BeginSection(string section)
        {
            this.chunk.Write(ArchCode.tag, ArchValueCode.BeginSection);
            this.chunk.Write(ArchCode.head, section);
            activeSection = section;
        }

        /// <summary>
        /// Closes the active section
        /// </summary>
        private void EndSection()
        {
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndSection);
            activeSection = ArchValueCode.Unknown;
        }

        private void Close()
        {
            this.chunk.Write(ArchCode.tag, ArchValueCode.EndOfFile);
            this.chunk.Flush();
        }
    }
}
