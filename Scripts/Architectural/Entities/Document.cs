using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.IO;

namespace XFramework.Architectural
{
    public class Document
    {
        private readonly HeaderVariables headerVariables;
        /// <summary>
        /// Get the HeaderVariables.
        /// </summary>
        public HeaderVariables HeaderVariables
        {
            get { return headerVariables; }
        }

        private List<Floor> floors = new List<Floor>();
        /// <summary>
        /// 楼层列表
        /// </summary>
        public List<Floor> Floors
        {
            get { return floors; }
        }

        /// <summary>
        /// 当前楼层总数
        /// </summary>
        public int FloorCount
        {
            get { return this.floors.Count; }
        }

        private int currentFloorNumber = 0;
        /// <summary>
        /// 当前楼层序号
        /// </summary>
        public int CurrentFloorNumber
        {
            get { return currentFloorNumber; }
            set 
            {
                int count = this.floors.Count;
                if (value <= 0)
                    throw new ArgumentException("楼层序号不能小于0");
                else if (value > count)
                    throw new ArgumentException("楼层序号不能大于当前楼层数量：" + count);

                currentFloorNumber = value;
                currentFloor = this.floors[currentFloorNumber - 1];
            }
        }

        private Floor currentFloor;
        /// <summary>
        /// 当前楼层
        /// </summary>
        public Floor CurrentFloor
        {
            get { return currentFloor; }
        }

        public Document()
        {
            headerVariables = new HeaderVariables();
        }

        /// <summary>
        /// 添加楼层
        /// </summary>
        /// <param name="number"></param>
        public void AddFloor()
        {
            int count = this.floors.Count;
            int number = count + 1;

            float altitude = 0;
            for (int i = 0; i < count; i++)
            {
                altitude += this.floors[i].Height;
            }

            Floor floor = new Floor(number);
            floor.Altitude = altitude;
            this.floors.Add(floor);

            CurrentFloorNumber = number;
        }

        /// <summary>
        /// 移除楼层
        /// </summary>
        /// <param name="number"></param>
        public void RemoveFloor(int number)
        {
            int count = this.floors.Count;
            if (number <= 0)
                throw new ArgumentException("楼层序号不能小于0");
            else if (number > count)
                throw new ArgumentException("楼层序号不能大于当前楼层数量：" + count);
            else if (count == 1)
                throw new ArgumentException("当前只有一层，不能删除。");

            this.floors.RemoveAt(number - 1);
            count = this.floors.Count;
            for (int i = number - 1; i < count; i++)
            {
                Floor floor = this.floors[i];
                floor.Number = i + 1;

                float altitude = 0;
                for (int j = 0; j < i; j++)
                {
                    altitude += this.floors[j].Height;
                }

                floor.Altitude = altitude;
            }
            // 设置楼层
            if (CurrentFloorNumber > count)
                CurrentFloorNumber = count;
        }

        public void RegisterForCreate()
        {
            //foreach (var floor in floors)
            //{
            //    // Corners
            //    //foreach (var corner in floor.Corners)
            //    //{
            //    //    ComponentRegistry.RegisterEntityForCreate(corner);
            //    //}
            //    // Walls
            //    foreach (var wall in floor.Walls)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(wall);
            //    }
            //    // Doors
            //    foreach (var door in floor.Doors)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(door);
            //    }
            //    // Windows
            //    foreach (var window in floor.Windows)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(window);
            //    }
            //    // Passes
            //    foreach (var pass in floor.Passes)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(pass);
            //    }
            //    // Rooms
            //    foreach (var room in floor.Rooms)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(room);
            //    }
            //    // Groups
            //    foreach (var group in floor.Groups)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(group);
            //    }
            //    // Areas
            //    foreach (var area in floor.Areas)
            //    {
            //        GraphicRegistry.RegisterEntityForCreate(area);
            //    }
            //}
            MonoDriver.Instance.StartCoroutine(RegisterForCreateIEnumerator());
        }

        IEnumerator RegisterForCreateIEnumerator()
        {
            foreach (var floor in floors)
            {
                // Corners
                //foreach (var corner in floor.Corners)
                //{
                //    ComponentRegistry.RegisterEntityForCreate(corner);
                //}
                // Walls
                foreach (var wall in floor.Walls)
                {
                    GraphicManager.CreateGraphic(wall);
                    yield return null;
                }
                // Doors
                foreach (var door in floor.Doors)
                {
                    GraphicManager.CreateGraphic(door);
                    yield return null;
                }
                // Windows
                foreach (var window in floor.Windows)
                {
                    GraphicManager.CreateGraphic(window);
                    yield return null;
                }
                // Passes
                foreach (var pass in floor.Passes)
                {
                    GraphicManager.CreateGraphic(pass);
                    yield return null;
                }
                // Rooms
                foreach (var room in floor.Rooms)
                {
                    GraphicManager.CreateGraphic(room);
                    yield return null;
                }
                // Groups
                foreach (var group in floor.Groups)
                {
                    GraphicManager.CreateGraphic(group);
                    yield return null;
                }
                // Areas
                foreach (var area in floor.Areas)
                {
                    GraphicManager.CreateGraphic(area);
                    yield return null;
                }
                // Equipments
                foreach (var equipment in floor.Equipments)
                {
                    GraphicManager.CreateGraphic(equipment);
                    yield return null;
                }
            }
        }

        public void RegisterForDestory()
        {
            foreach (var floor in floors)
            {
                // Corners
                foreach (var corner in floor.Corners)
                {
                    GraphicManager.DestoryGraphic(corner);
                }
                // Walls
                foreach (var wall in floor.Walls)
                {
                    GraphicManager.DestoryGraphic(wall);
                }
                // Windows
                foreach (var window in floor.Windows)
                {
                    GraphicManager.DestoryGraphic(window);
                }
                // Doors
                foreach (var door in floor.Doors)
                {
                    GraphicManager.DestoryGraphic(door);

                }
                // Passes
                foreach (var pass in floor.Passes)
                {
                    GraphicManager.DestoryGraphic(pass);
                }
                // Rooms
                foreach (var room in floor.Rooms)
                {
                    GraphicManager.DestoryGraphic(room);
                }
                // Groups
                foreach (var group in floor.Groups)
                {
                    GraphicManager.DestoryGraphic(group);
                }
                // Areas
                foreach (var area in floor.Areas)
                {
                    GraphicManager.DestoryGraphic(area);
                }
                // Equipments
                foreach (var equipment in floor.Equipments)
                {
                    GraphicManager.DestoryGraphic(equipment);
                }
            }
        }

        public static Document Load(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                ArchReader reader = new ArchReader();
                Document doc = reader.Read(fs);
                return doc;
            }
        }

        public static Document Load(Stream stream)
        {
            ArchReader reader = new ArchReader();
            Document doc = reader.Read(stream);
            return doc;
        }

        public static Document LoadFromResources(string path)
        {
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
            {
                throw new NullReferenceException("TextAsset is null");
            }

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(asset.text)))
            {
                ArchReader reader = new ArchReader();
                Document doc = reader.Read(ms);
                return doc;
            }
        }

        public void Save(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                ArchWriter writer = new ArchWriter();
                writer.Write(fs, this);
            }
        }

        public void Save(Stream stream)
        {
            ArchWriter writer = new ArchWriter();
            writer.Write(stream, this);
        }
    }
}
