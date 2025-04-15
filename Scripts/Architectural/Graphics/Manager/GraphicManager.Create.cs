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
        /// 对象池字典
        /// </summary>
        private Dictionary<string, GameObjectPool> m_Pools = new Dictionary<string, GameObjectPool>();

        /// <summary>
        /// 是否包含对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool HasPool<T>(string token = "")
        {
            Type type = typeof(T);
            string key = type.ToString();
            return m_Pools.ContainsKey(key);
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public GameObjectPool GetOrAddPool<T>()
        {
            Type type = typeof(T);
            string key = type.ToString();

            if (!m_Pools.TryGetValue(key, out GameObjectPool pool))
            {
                pool = new GameObjectPool(m_Unused.transform);
                m_Pools.Add(key, pool);
            }

            return pool;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public GameObjectPool GetOrAddPoolFromResources<T>(string path)
        {
            Type type = typeof(T);
            string key = type.ToString() + path;

            if (!m_Pools.TryGetValue(key, out GameObjectPool pool))
            {
                pool = new GameObjectPool(m_Unused.transform, path);
                m_Pools.Add(key, pool);
            }

            return pool;
        }

        public GameObjectPool AddPool<T>(string token, GameObject template)
        {
            Type type = typeof(T);
            string key = type.ToString() + token;
            if (m_Pools.ContainsKey(key))
            {
                GameObjectPool pool = m_Pools[key];
                return pool;
                //throw new ArgumentException("error, this object pool : " + key + " already exists.");
            }
            else
            {
                GameObjectPool pool = new GameObjectPool(m_Unused.transform, template);
                m_Pools.Add(key, pool);

                return pool;
            }


        }

        public GameObjectPool GetPool<T>(string token)
        {
            Type type = typeof(T);
            string key = type.ToString() + token;

            if (!m_Pools.ContainsKey(key))
                throw new ArgumentException("error, this object pool : " + key + " is not exists.");

            return m_Pools[key];
        }

        /// <summary>
        /// 创建 Graphic
        /// </summary>
        /// <param name="entity"></param>
        public static void CreateGraphic(EntityObject entity)
        {
            Instance.InternalCreateGraphic(entity);
        }

        /// <summary>
        /// 创建 Graphic
        /// </summary>
        /// <param name="data"></param>
        public void InternalCreateGraphic(EntityObject data)
        {
            switch (data.Type)
            {
                case EntityType.None:
                    break;
                case EntityType.Corner:
                    Corner corner = (Corner)data;
                    GameObject corner2D = CreateCorner2D(corner);
                    PlaceGraphic2D(corner2D, corner.Special);
                    break;
                case EntityType.Wall:
                    Wall wall = (Wall)data;
                    GameObject wall2D = CreateWall2D(wall);
                    PlaceGraphic2D(wall2D, wall.Special);
                    GameObject wall3D = CreateWall3D(wall);
                    PlaceGraphic3D(wall3D, wall.Special);
                    break;
                case EntityType.Room:
                    Room room = (Room)data;
                    GameObject room2D = CreateRoom2D(room);
                    PlaceGraphic2D(room2D, room.Special);
                    GameObject room3D = CreateRoom3D(room);
                    PlaceGraphic3D(room3D, room.Special);
                    break;
                case EntityType.Door:
                    Door door = (Door)data;
                    GameObject door2D = CreateDoor2D(door);
                    PlaceGraphic2D(door2D, door.Special);
                    GameObject door3D = CreateDoor3D(door);
                    PlaceGraphic3D(door3D, door.Special);
                    break;
                case EntityType.Window:
                    Window window = (Window)data;
                    GameObject window2D = CreateWindow2D(window);
                    PlaceGraphic2D(window2D, window.Special);
                    GameObject window3D = CreateWindow3D(window);
                    PlaceGraphic3D(window3D, window.Special);
                    break;
                case EntityType.Pass:
                    Pass pass = (Pass)data;
                    GameObject pass2D = CreatePass2D(pass);
                    PlaceGraphic2D(pass2D, pass.Special);
                    break;
                case EntityType.AText:
                    break;
                case EntityType.Group:
                    Group group = (Group)data;
                    GameObject group2D = CreateGroup2D(group);
                    PlaceGraphic2D(group2D, group.Special);
                    GameObject group3D = CreateGroup3D(group);
                    PlaceGraphic3D(group3D, group.Special);
                    break;
                case EntityType.Area:
                    Debug.LogError("Create Area Graphic");
                    Area area = (Area)data;
                    GameObject area2D = CreateArea2D(area);
                    PlaceGraphic2D(area2D, area.Special);
                    break;
                case EntityType.Equipment:
                    Equipment equipment = (Equipment)data;
                    GameObject equipmentGraphic = CreateEquipment(equipment);
                    PlaceGraphicBoth(equipmentGraphic, equipment.Special);
                    break;
                default:
                    break;
            }
        }

        public GameObject CreateCorner2D(Corner corner)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Corner2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(corner.Type) + "2D#" + corner.Id;
            Corner2D corner2D = go.AddComponent<Corner2D>();
            corner2D.Material = ArchitectSettings.Corner.material2d;
            //corner2D.MainTexture = ArchitectSettings.Corner.texture;
            corner2D.Corner = corner;

            return go;
        }

        public GameObject CreateWall2D(Wall wall)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Wall2D>();
            // create
            GameObject go = pool.Spawn();

            go.name = GetName(wall.Type) + "2D#" + wall.Id;
            Wall2D wall2D = go.GetOrAddComponent<Wall2D>();
            wall2D.Material = ArchitectSettings.Wall.material2d;
            wall2D.Color = ArchitectSettings.Wall.color2d;
            wall2D.Wall = wall;
            return go;
        }

        public GameObject CreateWall3D(Wall wall)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Wall3D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(wall.Type) + "3D#" + wall.Id;
            Wall3D wall3D = go.GetOrAddComponent<Wall3D>();
            wall3D.Material = ArchitectSettings.Wall.material3d;
            wall3D.Color = ArchitectSettings.Wall.color3d;
            wall3D.Wall = wall;
            return go;
        }

        public GameObject CreateDoor2D(Door door)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Door2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(door.Type) + "2D#" + door.Id;
            Door2D door2D = go.GetOrAddComponent<Door2D>();
            door2D.Material = ArchitectSettings.Door.material2d;
            door2D.Color = ArchitectSettings.Door.color2d;
            door2D.Door = door;
            return go;
        }

        public GameObject CreateDoor3D(Door door)
        {
            //string name = string.Empty;
            //if (door.DoorType == DoorType.Single)
            //{
            //    path = "Architect/Prefabs/单开门（900）";
            //}
            //else if (door.DoorType == DoorType.Double)
            //{
            //    path = "Architect/Prefabs/双开门（1500）";
            //}
            // pool
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
            GameObjectPool pool = AddPool<Door3D>(name, PrefabList.GetPrefab(name));
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(door.Type) + "3D#" + door.Id;
            Door3D door3D = go.GetOrAddComponent<Door3D>();
            door3D.Door = door;

            return go;
        }

        public GameObject CreateWindow2D(Window window)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Window2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(window.Type) + "2D#" + window.Id;
            Window2D window2D = go.GetOrAddComponent<Window2D>();
            window2D.Material = ArchitectSettings.Window.material2d;
            window2D.Color = ArchitectSettings.Window.color2d;
            window2D.Window = window;

            return go;
        }


        public GameObject CreateWindow3D(Window window)
        {
            //string path = "Architect/Prefabs/窗户";
            // pool
            //GameObjectPool pool = GetOrAddPoolFromResources<Window3D>(path);
            string name = "窗户";
            GameObjectPool pool = AddPool<Window3D>(name, PrefabList.GetPrefab(name));
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(window.Type) + "3D#" + window.Id;
            Window3D window3D = go.GetOrAddComponent<Window3D>();
            window3D.Window = window;

            return go;
        }

        public GameObject CreatePass2D(Pass pass)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Pass2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(pass.Type) + "2D#" + pass.Id;
            Pass2D pass2D = go.GetOrAddComponent<Pass2D>();
            pass2D.Material = ArchitectSettings.Pass.material2d;
            pass2D.Color = ArchitectSettings.Pass.color2d;
            pass2D.Pass = pass;

            return go;
        }

        public GameObject CreateRoom2D(Room room)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Room2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(room.Type) + "2D#" + room.Id;
            Room2D room2D = go.GetOrAddComponent<Room2D>();
            room2D.Color = ArchitectSettings.Room.color2d;
            room2D.Material = ArchitectSettings.Room.material2d;
            room2D.Room = room;

            return go;
        }

        public GameObject CreateRoom3D(Room room)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Room3D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(room.Type) + "3D#" + room.Id;
            Room3D room3D = go.GetOrAddComponent<Room3D>();
            room3D.Color = ArchitectSettings.Room.color3d;
            room3D.Material = ArchitectSettings.Room.material3d;
            room3D.Room = room;

            return go;
        }

        public GameObject CreateGroup2D(Group group)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Group2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(group.Type) + "2D#" + group.Id;
            Group2D group2D = go.GetOrAddComponent<Group2D>();
            group2D.Group = group;

            foreach (var member in group.Members)
            {
                if (TryGetGraphic2D(member.Entity, out GraphicObject graphic))
                {
                    group2D.AddGraphic(member.Entity.Id, graphic);
                }
            }

            return go;
        }

        public GameObject CreateGroup3D(Group group)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Group3D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(group.Type) + "3D#" + group.Id;
            Group3D group3D = go.GetOrAddComponent<Group3D>();
            group3D.Group = group;

            foreach (var member in group.Members)
            {
                if (TryGetGraphic3D(member.Entity, out GraphicObject graphic))
                {
                    group3D.AddGraphic(member.Entity.Id, graphic);
                }
            }

            return go;
        }

        public GameObject CreateArea2D(Area area)
        {
            // pool
            GameObjectPool pool = GetOrAddPool<Area2D>();
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(area.Type) + "2D#" + area.Id;
            Area2D area2D = go.GetOrAddComponent<Area2D>();
            area2D.Color = Color.white;
            area2D.Material = ArchitectSettings.Area.material2d;
            area2D.Area = area;

            return go;
        }


        public GameObject CreateEquipment(Equipment equipment)
        {
            //string path = equipment.Address;
            // pool
            //GameObjectPool pool = GetOrAddPoolFromResources<EquipmentGraphic>(path);
            string name = equipment.Address;
            GameObjectPool pool = AddPool<EquipmentGraphic>(name, PrefabList.GetPrefab(name));
            // create
            GameObject go = pool.Spawn();
            go.name = GetName(equipment.Type) + "#" + equipment.Id;
            EquipmentGraphic equipmentGraphic = go.AddComponent<EquipmentGraphic>();
            equipmentGraphic.Equipment = equipment;

            return go;
        }
    }
}
