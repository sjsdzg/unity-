using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Architectural
{
    public partial class GraphicManager
    {
        /// <summary>
        /// 删除 Graphic
        /// </summary>
        /// <param name="entity"></param>
        public static void DestoryGraphic(EntityObject entity)
        {
            Instance.InternalDestoryGraphic(entity);
        }

        /// <summary>
        /// Internal 删除 Graphic
        /// </summary>
        /// <param name="entity"></param>
        private void InternalDestoryGraphic(EntityObject entity)
        {
            List<GameObject> gameObjects = FindGameObjects(entity);
            foreach (var go in gameObjects)
            {
                GraphicObject graphic = go.GetComponent<GraphicObject>();
                graphic.Reset();
                m_GraphicRebuildQueue.Remove(graphic);
            }

            switch (entity.Type)
            {
                case EntityType.None:
                    break;
                case EntityType.Corner:
                    Corner corner = (Corner)entity;
                    DestoryCorner2D(corner);
                    break;
                case EntityType.Wall:
                    Wall wall = (Wall)entity;
                    DestoryWall2D(wall);
                    DestoryWall3D(wall);
                    break;
                case EntityType.Room:
                    Room room = (Room)entity;
                    DestoryRoom2D(room);
                    DestoryRoom3D(room);
                    break;
                case EntityType.Door:
                    Door door = (Door)entity;
                    DestoryDoor2D(door);
                    DestoryDoor3D(door);
                    break;
                case EntityType.Window:
                    Window window = (Window)entity;
                    DestoryWindow2D(window);
                    DestoryWindow3D(window);
                    break;
                case EntityType.Group:
                    Group group = (Group)entity;
                    DestoryGroup2D(group);
                    DestoryGroup3D(group);
                    break;
                case EntityType.Area:
                    Area area = (Area)entity;
                    DestoryArea2D(area);
                    break;
                case EntityType.Equipment:
                    Equipment equipment = (Equipment)entity;
                    DestoryEquipment(equipment);
                    break;
                case EntityType.AText:
                    break;
                default:
                    break;
            }
        }

        private void DestoryCorner2D(Corner corner)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Corner2D>();
            // destory
            string key = GetName(corner.Type) + "2D#" + corner.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryWall2D(Wall wall)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Wall2D>();
            // destory
            string key = GetName(wall.Type) + "2D#" + wall.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryWall3D(Wall wall)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Wall3D>();
            // destory
            string key = GetName(wall.Type) + "3D#" + wall.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryDoor2D(Door door)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Door2D>();
            // destory
            string key = GetName(door.Type) + "2D#" + door.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryDoor3D(Door door)
        {
            //string path = string.Empty;
            //if (door.DoorType == DoorType.Single)
            //{
            //    path = "Architect/Prefabs/单开门（900）";
            //}
            //else if (door.DoorType == DoorType.Double)
            //{
            //    path = "Architect/Prefabs/双开门（1500）";
            //}
            //// pool
            //GameObjectPool pool = GetOrAddPoolFromResources<Door3D>(path);
            string name = string.Empty;
            if (door.DoorType == DoorType.Single)
            {
                name = "单开门";
            }
            else if (door.DoorType == DoorType.Double)
            {
                name = "双开门";
            }
            GameObjectPool pool = GetPool<Door3D>(name);
            // destory
            string key = GetName(door.Type) + "3D#" + door.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryWindow2D(Window window)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Window2D>();
            // destory
            string key = GetName(window.Type) + "2D#" + window.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }


        public void DestoryWindow3D(Window window)
        {
            //string path = "Architect/Prefabs/窗户";
            // pool
            //GameObjectPool pool = GetOrAddPoolFromResources<Window3D>(path);
            string name = "窗户";
            GameObjectPool pool = GetPool<Window3D>(name);
            // destory
            string key = GetName(window.Type) + "3D#" + window.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryRoom2D(Room room)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Room2D>();
            // destory
            string key = GetName(room.Type) + "2D#" + room.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryRoom3D(Room room)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Room3D>();
            // destory
            string key = GetName(room.Type) + "3D#" + room.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryGroup2D(Group group)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Group2D>();
            // destory
            string key = GetName(group.Type) + "2D#" + group.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryGroup3D(Group group)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Group3D>();
            // destory
            string key = GetName(group.Type) + "3D#" + group.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryArea2D(Area area)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Area2D>();
            // destory
            string key = GetName(area.Type) + "2D#" + area.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }

        public void DestoryEquipment(Equipment equipment)
        {
            //string path = equipment.Address;
            // pool
            //GameObjectPool pool = GetOrAddPoolFromResources<EquipmentGraphic>(path);
            string name = equipment.Address;
            GameObjectPool pool = GetPool<EquipmentGraphic>(name);
            // destory
            string key = GetName(equipment.Type) + "#" + equipment.Id;
            if (TryGetGameobject(key, out GameObject go))
            {
                pool.Despawn(go);
            }
        }
    }
}
