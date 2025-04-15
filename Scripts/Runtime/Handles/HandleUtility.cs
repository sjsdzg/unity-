using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{
    public class HandleControl
    {
        public int Id { get; set; }

        /// <summary>
        /// Pixel distance from mouse pointer to ...
        /// </summary>
        public Func<float> DistanceToFunc { get; set; }

        public HandleControl(int id, Func<float> distanceToFunc)
        {
            Id = id;
            DistanceToFunc = distanceToFunc;
        }
    }

    public class HandleUtility
    {
        private static Camera m_Camera = null;
        /// <summary>
        /// 相机
        /// </summary>
        public static Camera Camera
        {
            get
            {
                if (m_Camera == null)
                {
                    m_Camera = Camera.main;
                }
                return m_Camera;
            }
            set { m_Camera = value; }
        }

        /// <summary>
        /// 获取鼠标位置，在平面上的位置
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <param name="plane"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool GetPointOnPlane(Vector3 mousePosition, Plane plane, out Vector3 point)
        {
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            float center;
            if (plane.Raycast(ray, out center))
            {
                point = ray.GetPoint(center);
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        const float k_KHandleSize = 80.0f;

        /// <summary>
        /// Get world space size of a manipulator at given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static float GetHandleSize(Vector3 position)
        {
            if (Camera)
            {
                Transform tr = Camera.transform;
                Vector3 camPos = tr.position;
                float distance = Vector3.Dot(position - camPos, tr.TransformDirection(new Vector3(0, 0, 1)));
                Vector3 screenPos = Camera.WorldToScreenPoint(camPos + tr.TransformDirection(new Vector3(0, 0, distance)));
                Vector3 screenPos2 = Camera.WorldToScreenPoint(camPos + tr.TransformDirection(new Vector3(1, 0, distance)));
                float screenDist = (screenPos - screenPos2).magnitude;
                return k_KHandleSize / Mathf.Max(screenDist, 0.0001f);
            }
            return 20.0f;
        }

        static Material s_HandleMaterial;
        /// <summary>
        /// 
        /// </summary>
        public static Material handleMaterial
        {
            get
            {
                if (!s_HandleMaterial)
                {
                    s_HandleMaterial = LoadRequired<Material>("Handles/Handles");
                }
                return s_HandleMaterial;
            }
        }

        static Material s_HandleWireMaterial;
        /// <summary>
        /// 
        /// </summary>
        public static Material handleWireMaterial
        {
            get
            {
                if (!s_HandleWireMaterial)
                {
                    s_HandleWireMaterial = LoadRequired<Material>("Handles/HandleLines");
                }
                s_HandleWireMaterial.SetColor("_Color", Color.white);
                return s_HandleWireMaterial;
            }
        }

        /// <summary>
        /// 加载所需对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private static T LoadRequired<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        ///  Handle Controls
        /// </summary>
        public static readonly List<HandleControl> m_HandleControls = new List<HandleControl>();

        /// <summary>
        /// Handle Dict
        /// </summary>
        private static Dictionary<int, BaseHandle> m_HandleDict = new Dictionary<int, BaseHandle>();

        /// <summary>
        /// Pixel distance from mouse pointer to line segment.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float DistanceToLineSegment(Vector3 p1, Vector3 p2)
        {
            p1 = Camera.WorldToScreenPoint(p1);
            p2 = Camera.WorldToScreenPoint(p2);

            Vector2 point = InputUtility.mousePosition;

            float retval = DistancePointToLineSegment(point, p1, p2);
            if (retval < 0)
                retval = 0.0f;

            return retval;
        }

        /// <summary>
        /// Pixel distance from mouse pointer to camera facing circle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static float DistanceToCircle(Vector3 position, float radius)
        {
            Vector2 screenCenter = WorldToScreenPoint(position);
            Vector2 screenEdge = WorldToScreenPoint(position + Camera.transform.right * radius);
            radius = (screenCenter - screenEdge).magnitude;

            float dist = (screenCenter - InputUtility.mousePosition).magnitude;
            if (dist < radius)
                return 0;

            return dist - radius;
        }

        static Vector3[] s_Points = { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static float DistanceToRectangle(Vector3 position, Quaternion rotation, Vector2 size)
        {
            Vector3 sideways = rotation * new Vector3(size.x / 2, 0, 0);
            Vector3 up = rotation * new Vector3(0, 0, size.y / 2);
            s_Points[0] = WorldToScreenPoint(position + sideways + up);
            s_Points[1] = WorldToScreenPoint(position + sideways - up);
            s_Points[2] = WorldToScreenPoint(position - sideways - up);
            s_Points[3] = WorldToScreenPoint(position - sideways + up);
            s_Points[4] = s_Points[0];

            Vector2 pos = InputUtility.mousePosition;
            bool oddNodes = false;
            int j = 4;
            for (int i = 0; i < 5; i++)
            {
                if ((s_Points[i].y > pos.y) != (s_Points[j].y > pos.y))
                {
                    if (pos.x < (s_Points[j].x - s_Points[i].x) * (pos.y - s_Points[i].y) / (s_Points[j].y - s_Points[i].y) + s_Points[i].x)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }

            if (!oddNodes)
            {
                // Distance to closest edge (not so fast)
                float closestDist = -1f;
                j = 1;
                for (int i = 0; i < 4; i++)
                {
                    var dist = DistancePointToLineSegment(pos, s_Points[i], s_Points[j++]);
                    if (dist < closestDist || closestDist < 0)
                        closestDist = dist;
                }
                return closestDist;
            }
            return 0;
        }

        /// <summary>
        /// Distance from a point in 2d to a line segment defined by two points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistancePointToLineSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            float l2 = (b - a).sqrMagnitude;    // i.e. |b-a|^2 -  avoid a sqrt
            if (l2 == 0.0)
                return (p - a).magnitude;       // a == b case
            float t = Vector2.Dot(p - a, b - a) / l2;
            if (t < 0.0)
                return (p - a).magnitude;       // Beyond the 'a' end of the segment
            if (t > 1.0)
                return (p - b).magnitude;         // Beyond the 'b' end of the segment
            Vector2 projection = a + t * (b - a); // Projection falls on the segment
            return (p - projection).magnitude;
        }

        public static Vector3 WorldToScreenPoint(Vector3 position)
        {
            return Camera.WorldToScreenPoint(position);
        }

        private static int s_NearestControl;
        /// <summary>
        /// nearest
        /// </summary>
        public static int nearestControl
        {
            get { return s_NearestDistance <= kPickDistance ? s_NearestControl : 0; }
            set { s_NearestControl = value; }
        }

        private static int s_HotControl;
        /// <summary>
        /// hot
        /// </summary>
        public static int hotControl
        {
            get { return s_HotControl; }
            set { s_HotControl = value; }
        }

        /// <summary>
        /// 最近距离
        /// </summary>
        private static float s_NearestDistance;

        /// <summary>
        /// 选取距离
        /// </summary>
        private const float kPickDistance = 5.0f;

        /// <summary>
        /// 添加 HandleControl
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="distanceToFunc"></param>
        public static void AddHandleControl(int controlId, Func<float> distanceToFunc)
        {
            m_HandleControls.Add(new HandleControl(controlId, distanceToFunc));
        }

        /// <summary>
        /// 移除 HandleControl
        /// </summary>
        /// <param name="controlId"></param>
        public static void RemoveHandleControl(int controlId)
        {
            m_HandleControls.RemoveAll(x => x.Id == controlId);
        }

        /// <summary>
        /// 添加 Handle
        /// </summary>
        /// <param name="handle"></param>
        public static void AddHandle(BaseHandle handle)
        {
            m_HandleDict.Add(handle.id, handle);
        }

        /// <summary>
        /// 移除 Handle
        /// </summary>
        /// <param name="handle"></param>
        public static void RemoveHandle(BaseHandle handle)
        {
            m_HandleDict.Remove(handle.id);
            RemoveHandleControl(handle.id);
        }

        /// <summary>
        /// 获取 Handle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BaseHandle GetHandle(int id)
        {
            BaseHandle handle;
            if (m_HandleDict.TryGetValue(id, out handle))
            {
                return handle;
            }
            return null;
        }

        /// <summary>
        /// 获取一个 Handle ID，不为0。
        /// </summary>
        /// <returns></returns>
        public static int GetHandleId()
        {
            return GUIUtility.GetControlID(FocusType.Passive);
        }

        /// <summary>
        /// 遍历 HandleControl 列表，获取鼠标距离最近的 Handle。
        /// </summary>
        public static void ProcessHandleControl()
        {
            s_NearestControl = 0;
            s_NearestDistance = kPickDistance;
            float distance = 0;
            foreach (var control in m_HandleControls)
            {
                if (control.Id == 0)
                    continue;

                if (control.DistanceToFunc == null)
                    continue;

                distance = control.DistanceToFunc();
                if (distance <= s_NearestDistance)
                {
                    s_NearestDistance = distance;
                    s_NearestControl = control.Id;
                }
            }
        }

        /// <summary>
        /// 遍历 m_HandleDict 列表，更新Handle状态
        /// </summary>
        public static void UpdateHandlesState()
        {
            foreach (var key in m_HandleDict.Keys)
            {
                var handle = m_HandleDict[key];
                if (!handle.active)
                    continue;

                handle.OnDraw();
            }
        }

        /// <summary>
        /// 捕捉
        /// </summary>
        /// <param name="val"></param>
        /// <param name="snap"></param>
        /// <returns></returns>
        public static float SnapValue(float val, float snap)
        {
            if (snap > 0)
            {
                return Mathf.Round(val / snap) * snap;
            }
            return val;
        }
    }
}
