using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    [RequireComponent(typeof(Camera))]
    public class ArchitectRaycaster : BaseRaycaster
    {
        /// <summary>
        /// Const to use for clarity when no event mask is set
        /// </summary>
        protected const int kNoEventMaskSet = -1;

        protected Camera m_EventCamera;

        /// <summary>
        /// Layer mask used to filter events. Always combined with the camera's culling mask if a camera is used.
        /// </summary>
        [SerializeField]
        protected LayerMask m_EventMask = kNoEventMaskSet;

        RaycastHit[] m_Hits;

        public override Camera eventCamera
        {
            get
            {
                if (m_EventCamera == null)
                    m_EventCamera = GetComponent<Camera>();
                return m_EventCamera ?? Camera.main;
            }
        }

        /// <summary>
        /// Depth used to determine the order of event processing.
        /// </summary>
        public virtual int depth
        {
            get { return (eventCamera != null) ? (int)eventCamera.depth : 0xFFFFFF; }
        }

        /// <summary>
        /// Event mask used to determine which objects will receive events.
        /// </summary>
        public int finalEventMask
        {
            get { return (eventCamera != null) ? eventCamera.cullingMask & m_EventMask : kNoEventMaskSet; }
        }

        /// <summary>
        /// Layer mask used to filter events. Always combined with the camera's culling mask if a camera is used.
        /// </summary>
        public LayerMask eventMask
        {
            get { return m_EventMask; }
            set { m_EventMask = value; }
        }

        protected void ComputeRayAndDistance(PointerEventData eventData, out Ray ray, out float distanceToClipPlane)
        {
            ray = eventCamera.ScreenPointToRay(eventData.position);
            // compensate far plane distance - see MouseEvents.cs
            float projectionDirection = ray.direction.z;
            distanceToClipPlane = Mathf.Approximately(0.0f, projectionDirection)
                ? Mathf.Infinity
                : Mathf.Abs((eventCamera.farClipPlane - eventCamera.nearClipPlane) / projectionDirection);
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (HandleSystem.current != null && HandleSystem.current.IsPointerOverHandle())
                return;

            ToolBase tool = Architect.Instance.ActiveTool;
            if (tool != null && tool.TopMost)
                return;

            // Cull ray casts that are outside of the view rect. (case 636595)
            if (eventCamera == null || !eventCamera.pixelRect.Contains(eventData.position))
                return;

            List<IGraphicRaycast> raycasts = ListPool<IGraphicRaycast>.Get();
            GraphicManager.Instance.GetRaycasts(raycasts);

            Vector2 eventPosition = eventData.position;
            float hitDistance = float.MaxValue;

            Ray ray;
            float distanceToClipPlane;
            ComputeRayAndDistance(eventData, out ray, out distanceToClipPlane);

            int hitCount = 0;

            m_Hits = Physics.RaycastAll(ray, distanceToClipPlane, finalEventMask);
            hitCount = m_Hits.Length;

            if (hitCount > 1)
                Array.Sort(m_Hits, (r1, r2) => r1.distance.CompareTo(r2.distance));

            if (hitCount > 0)
                hitDistance = m_Hits[0].distance;

            List<RaycastInfo> raycastInfos = ListPool<RaycastInfo>.Get();
            foreach (var raycast in raycasts)
            {
                if (raycast.Raycaset(eventPosition, eventCamera, out RaycastInfo result))
                {
                    raycastInfos.Add(result);
                }
            }
            raycastInfos.Sort((r1, r2) => r2.depth.CompareTo(r1.depth));

            foreach (var raycast in raycastInfos)
            {
                float distance = Vector3.Distance(ray.origin, raycast.worldPosition);
                if (distance >= hitDistance)
                    continue;

                var castResult = new RaycastResult
                {
                    gameObject = GetOwner(raycast.gameObject),
                    module = this,
                    distance = distance,
                    screenPosition = eventPosition,
                    index = resultAppendList.Count,
                    depth = raycast.depth,
                    sortingLayer = 0,
                    sortingOrder = 0
                };
                resultAppendList.Add(castResult);
            }

            if (hitCount != 0)
            {
                for (int b = 0, bmax = hitCount; b < bmax; ++b)
                {
                    var result = new RaycastResult
                    {
                        gameObject = GetOwner(m_Hits[b].collider.gameObject),
                        module = this,
                        distance = m_Hits[b].distance,
                        worldPosition = m_Hits[b].point,
                        worldNormal = m_Hits[b].normal,
                        screenPosition = eventData.position,
                        index = resultAppendList.Count,
                        sortingLayer = 0,
                        sortingOrder = 0
                    };
                    resultAppendList.Add(result);
                }
            }
        }

        public GameObject GetOwner(GameObject go)
        {
            if (go == null)
                return null;

            GraphicObject graphic = go.GetComponent<GraphicObject>();
            while (graphic != null)
            {
                if (graphic.Owner == null)
                    return go;

                go = graphic.Owner.gameObject;
                graphic = go.GetComponent<GraphicObject>();
            }

            return go;
        }
    }
}
