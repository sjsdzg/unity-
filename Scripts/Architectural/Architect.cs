using System;
using UnityEngine;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Architect : Singleton<Architect>, IUpdate
    {
        /// <summary>
        /// 版本
        /// </summary>
        public const string version = "1.0";

        /// <summary>
        /// 添加 Group 事件
        /// </summary>
        public const string AddGroupEvent = "AddGroupEvent";

        /// <summary>
        /// 移除 Group 事件
        /// </summary>
        public const string RemoveGroupEvent = "RemoveGroupEvent";

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enable { get; set; }

        private ViewMode viewMode;
        /// <summary>
        /// 视图模式
        /// </summary>
        public ViewMode ViewMode
        {
            get { return viewMode; }
            set 
            {
                if (viewMode == value)
                    return;

                viewMode = value;
                OnViewModeChanged();
            }
        }

        /// <summary>
        /// 相机控制
        /// </summary>
        private CameraController m_CameraController;

        /// <summary>
        /// 当前平面
        /// </summary>
        private Plane m_Plane;

        /// <summary>
        /// 工具集
        /// </summary>
        private Dictionary<Type, ToolBase> m_Tools = new Dictionary<Type, ToolBase>();

        private ToolBase activeTool;
        /// <summary>
        /// 激活工具
        /// </summary>
        public ToolBase ActiveTool
        {
            get { return activeTool; }
            set
            {
                if (activeTool != null)
                    activeTool.Release();

                activeTool = value;
            }
        }

        private Document currentDocument;
        /// <summary>
        /// 当前文档
        /// </summary>
        public Document CurrentDocument
        {
            get { return currentDocument; }
            set 
            { 
                currentDocument = value;
                // 错误处理
                if (currentDocument == null)
                {
                    currentDocument = new Document();
                    currentDocument.AddFloor();
                    Debug.Log("currentDocument is null");
                }
                else if (currentDocument.FloorCount == 0)
                {
                    currentDocument.AddFloor();
                    Debug.Log("currentDocument FloorCount is 0");
                }
                else
                {
                    currentDocument.RegisterForCreate();
                }
            }
        }

        /// <summary>
        /// 当前楼层
        /// </summary>
        public Floor CurrentFloor 
        {
            get { return this.currentDocument.CurrentFloor; }
        }

        private float m_SnapEpsilon = 0.3f;
        /// <summary>
        /// 捕捉精度
        /// </summary>
        public float SnapEpsilon
        {
            get { return m_SnapEpsilon; }
            set { m_SnapEpsilon = value; }
        }

        protected override void Init()
        {
            base.Init();
            MonoDriver.Attach(this);

            currentDocument = new Document();
            currentDocument.AddFloor();

            m_CameraController = Camera.main.GetComponent<CameraController>();

            ViewMode = ViewMode.Drawing;
        }


        public override void Release()
        {
            base.Release();
            MonoDriver.Detach(this);
        }

        public void Update()
        {
            if (!Enable)
                return;

            if (ActiveTool != null)
            {
                ActiveTool.Update();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ActiveTool = null;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (ActiveTool != null)
                    ActiveTool.Cancel();
            }

            //if (Input.GetKeyDown(KeyCode.Delete))
            //{
            //    if (Selection.Instance.currentSelectedEntityObject == null)
            //        return;

            //    var selection = Selection.Instance.currentSelectedEntityObject;
            //    Selection.Instance.currentSelectedEntityObject = null;

            //    if (selection is Group)
            //    {
            //        var group = selection as Group;
            //        //ArchitectUtility.RegisterGroupForDestory(group);
            //        ArchitectUtility.RemoveGroupHandler(Architect.Instance.CurrentFloor, group);
            //        ArchitectUtility.RemoveGroup(Architect.Instance.CurrentFloor, group);
            //        ArchitectUtility.CombineWall(Architect.Instance.CurrentFloor);
            //    }


            //}

            //if (Input.GetKeyDown(KeyCode.F1))
            //{
            //    ActiveTool = GetTool<WallCreateTool>();
            //    ActiveTool.Init(ToolArgs.Empty);
            //}

            //if (Input.GetKeyDown(KeyCode.F2))
            //{
            //    ActiveTool = GetTool<DoorCreateTool>();
            //    DoorCreateToolArgs t = new DoorCreateToolArgs();
            //    t.DoorType = DoorType.Single;
            //    ActiveTool.Init(t);
            //}

            //if (Input.GetKeyDown(KeyCode.F3))
            //{
            //    ActiveTool = GetTool<DoorCreateTool>();
            //    DoorCreateToolArgs t = new DoorCreateToolArgs();
            //    t.DoorType = DoorType.Double;
            //    ActiveTool.Init(t);
            //}

            //if (Input.GetKeyDown(KeyCode.F4))
            //{
            //    ActiveTool = GetTool<DoorCreateTool>();
            //    DoorCreateToolArgs t = new DoorCreateToolArgs();
            //    t.DoorType = DoorType.Revolving;
            //    ActiveTool.Init(t);
            //}

            //if (Input.GetKeyDown(KeyCode.F5))
            //{
            //    ActiveTool = GetTool<WindowCreateTool>();
            //    ActiveTool.Init(ToolArgs.Empty);
            //}

            //if (Input.GetKeyDown(KeyCode.F6))
            //{
            //    ActiveTool = GetTool<PassCreateTool>();
            //    ActiveTool.Init(ToolArgs.Empty);
            //}

            //if (Input.GetKeyDown(KeyCode.F7))
            //{
            //    ActiveTool = GetTool<AreaCreateTool>();
            //    ActiveTool.Init(ToolArgs.Empty);
            //}

            //if (Input.GetKeyDown(KeyCode.O))
            //{
            //    bool flag = ArchitectSettings.Wall.ortho;
            //    ArchitectSettings.Wall.ortho.Value = !flag;
            //}

            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    switch (ViewMode)
            //    {
            //        case ViewMode.None:
            //            break;
            //        case ViewMode.Drawing:
            //            ViewMode = ViewMode.Facade;
            //            break;
            //        case ViewMode.Facade:
            //            ViewMode = ViewMode.Drawing;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// 视图模式更改时触发
        /// </summary>
        private void OnViewModeChanged()
        {
            GraphicManager.Instance.Show(ViewMode);
            switch (ViewMode)
            {
                case ViewMode.None:
                    break;
                case ViewMode.Drawing:
                    m_CameraController.CameraMode = CameraMode.Visual2D;
                    break;
                case ViewMode.Facade:
                    m_CameraController.CameraMode = CameraMode.Visual3D;
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 获取工具
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTool<T>() where T : ToolBase, new()
        {
            ToolBase tool;
            if (m_Tools.TryGetValue(typeof(T), out tool))
                return (T)tool;

            tool = new T();
            m_Tools.Add(typeof(T), tool);
            return (T)tool;
        }

        /// <summary>
        /// 捕捉点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool SnapPoint(Vector3 target, out Vector3 point)
        {
            return Instance.CurrentFloor.SnapPoint(target, out point, Instance.SnapEpsilon);
        }

        /// <summary>
        /// 捕捉墙角点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static bool SnapCorner(Vector3 target, out Corner corner)
        {
            return Instance.CurrentFloor.SnapCorner(target, out corner, Instance.SnapEpsilon);
        }

        /// <summary>
        /// 捕捉墙
        /// </summary>
        /// <param name="target"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static bool SnapWall(Vector3 target, out Wall wall, float offset = 0f)
        {
            return Instance.CurrentFloor.SnapWall(target, out wall, offset, Instance.SnapEpsilon);
        }

        /// <summary>
        /// 捕捉墙（和线段平行且最近的墙体）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="wall"></param>
        /// <returns></returns>
        public static bool SnapWall(Segment2 target, out Wall wall)
        {
            return Instance.CurrentFloor.SnapWall(target, out wall, Instance.SnapEpsilon);
        }

        /// <summary>
        /// 添加元件数据
        /// </summary>
        /// <param name="entity"></param>
        public static void AddEntity(EntityObject entity)
        {
            Instance.CurrentFloor.AddEntity(entity);
        }

        /// <summary>
        /// 移除元件数据
        /// </summary>
        /// <param name="entity"></param>
        public static void RemoveEntity(EntityObject entity)
        {
            Instance.CurrentFloor.RemoveEntity(entity);
        }

        /// <summary>
        /// 根据轴，捕捉墙角点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static bool SnapAxis(Vector3 target, out Vector3 axis)
        {
            return Instance.CurrentFloor.SnapAxis(target, out axis, Instance.SnapEpsilon);
        }

        public Vector3 InternalPointerToWorldPoint
        {
            get
            {
                Vector3 result = Vector3.zero;
                Camera camera = m_CameraController.MainCamera;
                switch (m_CameraController.CameraMode)
                {
                    case CameraMode.None:
                        break;
                    case CameraMode.Visual2D:
                        m_Plane = new Plane(Vector3.up, Vector3.zero);
                        break;
                    case CameraMode.Visual3D:
                        break;
                    default:
                        break;
                }

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                float enter;
                if (m_Plane.Raycast(ray, out enter))
                {
                    result = ray.GetPoint(enter);
                }

                return result;
            }
        }

        /// <summary>
        /// 将鼠标的位置从屏幕坐标转换为世界坐标
        /// </summary>
        public static Vector3 PointerToWorldPoint
        {
            get
            {
                return Instance.InternalPointerToWorldPoint;
            }
        }

        public Vector3 InternalScreenToWorldPoint(Vector3 position)
        {
            Vector3 result = Vector3.zero;
            Camera camera = m_CameraController.MainCamera;
            switch (m_CameraController.CameraMode)
            {
                case CameraMode.None:
                    break;
                case CameraMode.Visual2D:
                    m_Plane = new Plane(Vector3.up, Vector3.zero);
                    break;
                case CameraMode.Visual3D:
                    break;
                default:
                    break;
            }

            Ray ray = camera.ScreenPointToRay(position);
            float center;
            if (m_Plane.Raycast(ray, out center))
            {
                result = ray.GetPoint(center);
            }

            return result;
        }

        /// <summary>
        /// 将屏幕的位置从屏幕坐标转换为世界坐标
        /// </summary>
        public static Vector3 ScreenToWorldPoint(Vector3 position)
        {
            return Instance.InternalScreenToWorldPoint(position);
        }

        public Vector3 InternalWorldToScreenPoint(Vector3 position)
        {
            Camera camera = m_CameraController.MainCamera;
            return camera.WorldToScreenPoint(position);
        }

        /// <summary>
        /// 将点从世界坐标转换为屏幕坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 WorldToScreenPoint(Vector3 position)
        {
            return Instance.InternalWorldToScreenPoint(position);
        }

        public Vector3 InternalWorldToCameraPoint(Vector3 position)
        {
            Camera camera = m_CameraController.MainCamera;
            return camera.worldToCameraMatrix.MultiplyPoint(position);
        }

        /// <summary>
        /// 将点从世界坐标转换为相机坐标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 WorldToCameraPoint(Vector3 position)
        {
            return Instance.InternalWorldToCameraPoint(position);
        }

        public Vector3 InternalWorldToCameraVector(Vector3 vector)
        {
            Camera camera = m_CameraController.MainCamera;
            return camera.worldToCameraMatrix.MultiplyVector(vector);
        }

        /// <summary>
        /// 将向量从世界坐标转换为相机坐标
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 WorldToCameraVector(Vector3 vector)
        {
            return Instance.InternalWorldToCameraVector(vector);
        }

        /// <summary>
        /// 线段投射墙体，返回击中墙体的信息
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static List<SegmentCastWallHit> SegmentCastWalls(Segment2 origin)
        {
            return Instance.CurrentFloor.SegmentCastWalls(origin);
        }

        /// <summary>
        /// 根据点，获取 Corner
        /// </summary>
        /// <param name="position"></param>
        /// <param name="corner"></param>
        /// <returns></returns>
        public static bool TryGetCorner(Vector3 position, out Corner corner)
        {
            return Instance.CurrentFloor.TryGetCorner(position, out corner);
        }

        /// <summary>
        /// 根据 corner0，corner1，获取 Wall
        /// </summary>
        /// <param name="corner0"></param>
        /// <param name="corner1"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetWall(Corner corner0, Corner corner1, out Wall result)
        {
            return Instance.CurrentFloor.TryGetWall(corner0, corner1, out result);
        }
    }
}