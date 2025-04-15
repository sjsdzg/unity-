using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Architectural
{
    public class Selection : Singleton<Selection>, IUpdate
    {
        public const string setSelectedEntityObjectEvent = "setSelectedEntityObjectEvent";

        private EntityObject m_currentSelectedEntityObject;
        /// <summary>
        /// 当前选定的实体对象
        /// </summary>
        public EntityObject currentSelectedEntityObject
        {
            get { return m_currentSelectedEntityObject; }
            set { m_currentSelectedEntityObject = value; }
        }

        protected override void Init()
        {
            base.Init();
            MonoDriver.Attach(this);
        }

        public void SetSelectedEntityObject(EntityObject selected)
        {
            if (selected == m_currentSelectedEntityObject)
            {
                return;
            }

            List<GameObject> gameObjects = GraphicManager.Instance.FindGameObjects(m_currentSelectedEntityObject);
            if (gameObjects != null)
            {
                foreach (var go in gameObjects)
                {
                    SelectableGraphic selectableGraphic = go.GetComponent<SelectableGraphic>();
                    if (selectableGraphic != null)
                    {
                        selectableGraphic.Deselect();
                    }
                }
            }

            m_currentSelectedEntityObject = selected;

            gameObjects = GraphicManager.Instance.FindGameObjects(m_currentSelectedEntityObject);
            if (gameObjects != null)
            {
                foreach (var go in gameObjects)
                {
                    SelectableGraphic selectableGraphic = go.GetComponent<SelectableGraphic>();
                    if (selectableGraphic != null)
                    {
                        selectableGraphic.Select();
                    }
                }
            }
            // 触发选中事件
            EventDispatcher.ExecuteEvent<EntityObject>(setSelectedEntityObjectEvent, m_currentSelectedEntityObject);
            
            /*
            MonoDriver.Instance.WaitForFrame(2, () =>
            {
                gameObjects = GraphicManager.Instance.FindGameObjects(m_currentSelectedEntityObject);
                if (gameObjects != null)
                {
                    foreach (var go in gameObjects)
                    {
                        SelectableGraphic selectableGraphic = go.GetComponent<SelectableGraphic>();
                        if (selectableGraphic != null)
                        {
                            selectableGraphic.Select();
                        }
                    }
                }

                // 触发选中事件
                EventDispatcher.ExecuteEvent<EntityObject>(setSelectedEntityObjectEvent, m_currentSelectedEntityObject);

                
                if (m_currentSelectedEntityObject is Group)
                {
                    group = (Group)m_currentSelectedEntityObject;
                    GroupInfo groupInfo = new GroupInfo();
                    groupInfo.Id = group.Id;
                    groupInfo.Name = group.Name;
                    groupInfo.Description = "";

                    if (manifest.GroupInfos == null)
                    {
                        manifest.GroupInfos = new List<GroupInfo>();
                    }

                    if (!manifest.GroupInfos.Exists(x => x.Id.Equals(groupInfo.Id)))
                    {
                        manifest.GroupInfos.Add(groupInfo);
                    }
                }
                
            }); */
        }

        private GroupInfoManifest manifest = new GroupInfoManifest();

        private Group group;

        Equipment equipment0;

        Equipment equipment1;

        Equipment equipment2;

        Equipment equipment3;

        public void Update()
        {

            /*
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (m_currentSelectedEntityObject.Type == EntityType.Room)
                {
                    List<EntityObject> entityObjects = new List<EntityObject>();
                    Room room = m_currentSelectedEntityObject as Room;
                    entityObjects.Add(room);

                    // 培养基配制间
                    if (room.Name.Equals("培养基配制间")) 
                    {
                        entityObjects.Add(equipment0);
                    }
                    // 细胞培养间
                    if (room.Name.Equals("细胞培养间"))
                    {
                        entityObjects.Add(equipment1);
                    }
                    // 细胞扩增
                    if (room.Name.Equals("细胞扩增"))
                    {
                        entityObjects.Add(equipment2);
                    }
                    // 纯化一
                    if (room.Name.Equals("纯化一"))
                    {
                        entityObjects.Add(equipment3);
                    }

                    group = Group.Combine(entityObjects);

                    Architect.AddEntity(group);
                    ArchitectUtility.RegisterGroupForCreate(group, false);

                    SetSelectedEntityObject(group);
                }
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                if (m_currentSelectedEntityObject.Type == EntityType.Room)
                {
                    Room room = m_currentSelectedEntityObject as Room;
                    room.Active = false;
                }
                else if (m_currentSelectedEntityObject.Type == EntityType.Room)
                {

                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (m_currentSelectedEntityObject.Type == EntityType.Wall)
                {
                    Wall wall = m_currentSelectedEntityObject as Wall;
                    wall.Dispose();
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                group.Position = new Vector3(group.Position.x - 1, group.Position.y, group.Position.z);
                ArchitectUtility.AddGroupHandler(Architect.Instance.CurrentFloor, group);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                group.Position = new Vector3(group.Position.x + 1, group.Position.y, group.Position.z);
                ArchitectUtility.AddGroupHandler(Architect.Instance.CurrentFloor, group);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                Architect.Instance.ActiveTool = Architect.Instance.GetTool<GroupCreateTool>();
                GroupCreateToolArgs t = new GroupCreateToolArgs();
                t.Group = (Group)group.Clone();
                Architect.Instance.ActiveTool.Init(t);
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                foreach (var room in Architect.instance.CurrentFloor.Rooms)
                {
                    List<EntityObject> entityObjects = new List<EntityObject>();

                    entityObjects.Add(room);

                    // 培养基配制间
                    if (room.Name.Equals("培养基配制间"))
                    {
                        entityObjects.Add(equipment0);
                    }
                    // 细胞培养间
                    if (room.Name.Equals("细胞培养间"))
                    {
                        entityObjects.Add(equipment1);
                    }
                    // 细胞扩增
                    if (room.Name.Equals("细胞扩增"))
                    {
                        entityObjects.Add(equipment2);
                    }
                    // 纯化一
                    if (room.Name.Equals("纯化一"))
                    {
                        entityObjects.Add(equipment3);
                    }

                    group = Group.Combine(entityObjects);
                    group.Name = room.Name;

                    Architect.AddEntity(group);
                    ArchitectUtility.RegisterGroupForCreate(group, false);
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                string path = "D:/GroupInfoManifest.json";
                string json = manifest.ToJson();
                File.WriteAllText(path, json);
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha1)");
                equipment0 = new Equipment("培养基配制间设备", "Equipments/培养基配制间设备");
                equipment0.Position = new Vector3(0, -7.5f, 0);
                equipment0.Rotation = Quaternion.Euler(0, 180, 0);
                Architect.AddEntity(equipment0);
                GraphicRegistry.RegisterEntityForCreate(equipment0);

                equipment1 = new Equipment("细胞培养间设备", "Equipments/细胞培养间设备");
                equipment1.Position = new Vector3(0, -7.5f, 0);
                equipment1.Rotation = Quaternion.Euler(0, 180, 0);
                Architect.AddEntity(equipment1);
                GraphicRegistry.RegisterEntityForCreate(equipment1);

                equipment2 = new Equipment("细胞扩增间设备", "Equipments/细胞扩增间设备");
                equipment2.Position = new Vector3(0, -7.5f, 0);
                equipment2.Rotation = Quaternion.Euler(0, 180, 0);
                Architect.AddEntity(equipment2);
                GraphicRegistry.RegisterEntityForCreate(equipment2);

                equipment3 = new Equipment("蛋白纯化间设备", "Equipments/蛋白纯化间设备");
                equipment3.Position = new Vector3(0, -7.5f, 0);
                equipment3.Rotation = Quaternion.Euler(0, 180, 0);
                Architect.AddEntity(equipment3);
                GraphicRegistry.RegisterEntityForCreate(equipment3);
            }

            */
        }

        public override void Release()
        {
            base.Release();
            MonoDriver.Detach(this);
        }
    }
}
