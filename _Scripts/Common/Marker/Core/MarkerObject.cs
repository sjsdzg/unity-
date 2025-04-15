using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class MarkerObject : MonoBehaviour
    {
        /// <summary>
        /// 标签Map
        /// </summary>
        private MarkerMap markerMap;

        /// <summary>
        /// 标签Prefab
        /// </summary>
        public GameObject markerPrefab = null;

        /// <summary>
        /// 目标物体
        /// </summary>
        public Transform m_Target = null;

        /// <summary>
        /// 偏移
        /// </summary>
        public Vector3 offset = Vector3.zero;

        /// <summary>
        /// 渲染延迟时间
        /// </summary>
        [Range(0, 3)]
        public float renderDelay = 0.3f;

        /// <summary>
        /// 位置
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// 实例化的物体
        /// </summary>
        private GameObject cacheMarker = null;

        /// <summary>
        /// 初始化时，创建标签
        /// </summary>
        public bool OnAwakeCreated = true;

        /// <summary>
        /// 是否能交互
        /// </summary>
        public bool interactable = true;

        /// <summary>
        /// 文本内容
        /// </summary>
        public string text = null;

        /// <summary>
        /// 人物设置显示
        /// </summary>
        private bool Visible = true;

        void Awake()
        {
            markerMap = MarkerUtils.GetMarkerMap;

            if (markerMap.CanvasParent != null && OnAwakeCreated)
            {
                CreateMark();
            }
        }

        /// <summary>
        /// 在地图上创建标签
        /// </summary>
        public void CreateMark()
        {
            cacheMarker = Instantiate(markerPrefab) as GameObject;
            cacheMarker.transform.SetParent(markerMap.CanvasParent.transform, false);
            Marker marker = cacheMarker.GetComponent<Marker>();
            marker.DelayRender(renderDelay);
            marker.Item = this;//标签绑定Item
            marker.m_Text.text = text.Replace("\\n", "\n"); ;
            markerMap.AddMarker(marker);//添加标签

            cacheMarker.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            cacheMarker.GetComponent<CanvasGroup>().interactable = interactable;

            if (m_Target == null)
            {
                m_Target = transform;
            }
        }

        void LateUpdate()
        {
            if (Visible == false)
                return;

            if (m_Target == null)
                return;

            if (cacheMarker == null)
                return;

            position = m_Target.GetComponent<Collider>().bounds.center + ((Vector3.up * m_Target.GetComponent<Collider>().bounds.size.y) * 0.5f);
            position += offset;

            Vector3 front = position - markerMap.MCamera.transform.position;
            if ((front.magnitude < markerMap.HideDistance) && (Vector3.Angle(markerMap.MCamera.transform.forward, position - markerMap.MCamera.transform.position) < markerMap.MaxViewAngle))
            {
                cacheMarker.SetActive(true);
                Vector3 v = markerMap.MCamera.WorldToScreenPoint(position);
                cacheMarker.GetComponent<RectTransform>().anchorMax = Vector2.zero;
                cacheMarker.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                cacheMarker.GetComponent<RectTransform>().anchoredPosition3D = v;
            }
            else
            {
                cacheMarker.SetActive(false);
            }
        }

        /// <summary>
        /// 显示标签
        /// </summary>
        public void Show()
        {
            Visible = true;
            if (cacheMarker != null)
            {
                cacheMarker.SetActive(true);
            }
            else
            {
                CreateMark();
            }
        }

        /// <summary>
        /// 隐藏标签
        /// </summary>
        public void Hide()
        {
            Visible = false;
            if (cacheMarker != null)
            {
                cacheMarker.SetActive(false);
            }
        }
    }
}
