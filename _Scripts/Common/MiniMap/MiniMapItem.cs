using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 要在小地图上显示的项
    /// </summary>
    public class MiniMapItem : MonoBehaviour
    {
        /// <summary>
        /// 小地图标签Prefab
        /// </summary>
        public GameObject m_MarkPrefab = null;

        /// <summary>
        /// 目标物体
        /// </summary>
        public Transform m_Target = null;

        /// <summary>
        /// 偏移
        /// </summary>
        public Vector3 offset = Vector3.zero;

        /// <summary>
        /// 是否能交互
        /// </summary>
        public bool interactable = true;

        /// <summary>
        /// 信息
        /// </summary>
        public string itemInfo = "Info Icon here";

        /// <summary>
        /// 渲染延迟时间
        /// </summary>
        [Range(0,3)]
        //public float renderDelay = 0.3f;

        /// <summary>
        /// 小地图
        /// </summary>
        public MiniMap[] m_MiniMaps;

        /// <summary>
        /// 地图标签类型
        /// </summary>
        public MapMarkerType Type = MapMarkerType.None;

        /// <summary>
        /// 位置
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// 实例化的物体
        /// </summary>
        private List<GameObject> cacheMarks = null;

        /// <summary>
        /// 图标
        /// </summary>
        private List<Image> m_Graphics = null;

        /// <summary>
        /// 小地图显示面板
        /// </summary>
        private List<RectTransform> miniMapRects;

        /// <summary>
        /// 初始化时，创建标签
        /// </summary>
        public bool OnAwakeCreated = true;

        /// <summary>
        /// 小地图相机名称
        /// </summary>
        public string miniMapName;

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 TargetPos
        {
            get
            {
                if (m_Target == null)
                {
                    return Vector3.zero;
                }

                return new Vector3(m_Target.position.x, 0, m_Target.position.z);
            }
        }

        void Start()
        {
            //m_MiniMap = MiniMapUtils.GetWorldMap(miniMapName);
            //if (m_MiniMap.MiniMapRect != null && OnAwakeCreated)
            //{
            //    CreateMark();
            //}
            m_MiniMaps = MiniMapUtils.GetAllMiniMap();
            cacheMarks = new List<GameObject>();
            m_Graphics = new List<Image>();
            miniMapRects = new List<RectTransform>();
            if (OnAwakeCreated)
            {
                CreateMark();
            }
        }

        /// <summary>
        /// 在地图上创建标签
        /// </summary>
        public void CreateMark()
        {
            foreach (var m_MiniMap in m_MiniMaps)
            {
                GameObject cacheMark = Instantiate(m_MarkPrefab) as GameObject;
                RectTransform miniMapRect = m_MiniMap.MiniMapRect;

                Image m_Graphic = cacheMark.GetComponent<Image>();
                cacheMark.transform.SetParent(miniMapRect.transform, false);
                m_Graphic.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                cacheMark.GetComponent<CanvasGroup>().interactable = interactable;

                MiniMapMarker mark = cacheMark.GetComponent<MiniMapMarker>();
                //mark.DelayRender(renderDelay);
                mark.Item = this;//标签绑定Item
                m_MiniMap.AddMarker(mark);//添加标签

                cacheMarks.Add(cacheMark);
                m_Graphics.Add(m_Graphic);
                miniMapRects.Add(miniMapRect);
            }

            if (m_Target == null)
            {
                m_Target = transform;
            }
        }

        void LateUpdate()
        {
            if (m_Target == null)
                return;

            if (cacheMarks.Count == 0)
                return;

            for (int i = 0; i < m_MiniMaps.Length; i++)
            {
                MiniMap m_MiniMap = m_MiniMaps[i];
                Image m_Graphic = m_Graphics[i];
                RectTransform miniMapRect = miniMapRects[i];

                if (m_Graphic == null)
                    return;

                RectTransform rt = m_Graphic.GetComponent<RectTransform>();
                Vector3 correctPos = TargetPos + offset;

                Vector2 vp2 = m_MiniMap.MiniMapCamera.WorldToViewportPoint(correctPos);

                //Debug.Log(correctPos);

                position = new Vector2((vp2.x * miniMapRect.sizeDelta.x) - (miniMapRect.sizeDelta.x * 0.5f),
                    (vp2.y * miniMapRect.sizeDelta.y) - (miniMapRect.sizeDelta.y * 0.5f));

                //Apply position to the UI (for follow)
                rt.anchoredPosition = position;

                //Change size with smooth transition
                float correctSize = m_MiniMap.MarkMultiplier;
                rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(rt.sizeDelta.x * correctSize, rt.sizeDelta.y * correctSize), Time.deltaTime * 8);

                Vector3 eulerAngle = m_MiniMap.transform.eulerAngles;
                Vector3 re = Vector3.zero;
                //re.z = (180 - m_Target.rotation.eulerAngles.y) + eulerAngle.y;
                re.z = m_Target.rotation.eulerAngles.y + eulerAngle.y;
                Quaternion q = Quaternion.Euler(re);
                rt.rotation = q;
            }

        }


        /// <summary>
        /// 显示标签
        /// </summary>
        public void ShowMark()
        {
            if (cacheMarks.Count > 0)
            {
                foreach (var cacheMark in cacheMarks)
                {
                    cacheMark.SetActive(true);
                }
            }
            else
            {
                CreateMark();
            }
        }

        /// <summary>
        /// 隐藏标签
        /// </summary>
        public void HideMark()
        {
            if (cacheMarks.Count > 0)
            {
                foreach (var cacheMark in cacheMarks)
                {
                    cacheMark.SetActive(false);
                }
            }
        }
    }
}
