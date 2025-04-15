using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    [System.Serializable]
    public enum MapType
    {
        Target,
        World,
    }

    public class MiniMap : MonoBehaviour
    {
        [SerializeField]
        private string miniMapName;
        /// <summary>
        /// 地图名称
        /// </summary>
        public string MiniMapName
        {
            get { return miniMapName; }
            set { miniMapName = value; }
        }

        [SerializeField]
        private Transform m_Target;
        /// <summary>
        /// 目标对象
        /// </summary>
        public Transform Target
        {
            get
            {
                if (m_Target != null)
                {
                    return m_Target;
                }
                return m_Target;
            }
            set
            {
                m_Target = value;
            }
        }

        private Transform t;
        /// <summary>
        /// 自身
        /// </summary>
        private Transform m_Transform
        {
            get
            {
                if (t == null)
                {
                    t = this.GetComponent<Transform>();
                }
                return t;
            }
        }

        /// <summary>
        /// 小地图相机
        /// </summary>
        public Camera MiniMapCamera = null;

        /// <summary>
        /// 标签的放大倍数
        /// </summary>
        [Range(0.05f, 2)]
        public float MarkMultiplier = 1;

        /// <summary>
        /// 小地图Rect
        /// </summary>
        public RectTransform MiniMapRect;

        /// <summary>
        /// 默认高度
        /// </summary>
        public float DefaultSize = 2.5f;
        /// <summary>
        /// 最大高度
        /// </summary>
        public float maxSize = 10;
        /// <summary>
        /// 最小高度
        /// </summary>
        public float minSize = 1;
        /// <summary>
        /// 插值高度
        /// </summary>
        public float LerpSize = 8;
        /// <summary>
        /// 插值旋转速度
        /// </summary>
        public float LerpRotation = 8;
        /// <summary>
        /// 是否平滑旋转
        /// </summary>
        public bool SmoothRotation = true;
        /// <summary>
        /// 角色图标
        /// </summary>
        public Image PlayerIcon;
        /// <summary>
        /// 地图模式
        /// </summary>
        public MapType m_MapType = MapType.Target;

        /// <summary>
        /// 小地图尺寸改变速率
        /// </summary>
        [Range(1, 10)]
        public int scrollSensitivity = 3;

        /// <summary>
        /// 相机Tag
        /// </summary>
        public string cameraTag = "MiniMapCamera";

        /// <summary>
        /// 小地图标签列表
        /// </summary>
        List<MiniMapMarker> markerList = new List<MiniMapMarker>();

        public class OnClickedEvent : UnityEvent<MiniMapMarker> { }

        private OnClickedEvent m_ItemClicked = new OnClickedEvent();
        /// <summary>
        /// 标签项点击事件
        /// </summary>
        public OnClickedEvent ItemClicked
        {
            get { return m_ItemClicked; }
            set { m_ItemClicked = value; }
        }

        private void Start()
        {
            if (MiniMapCamera == null)
            {
                MiniMapCamera = MiniMapUtils.GetCameraWithTag(cameraTag);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        void LateUpdate()
        {
            if (m_Target == null)
            {
                GameObject obj = GameObject.FindWithTag("Player");
                if (obj != null)
                {
                    m_Target = obj.transform;
                }
            }

            if (m_Target == null)
                return;
            if (MiniMapCamera == null)
                return;
            if (PlayerIcon == null)
                return;

            //Controlles inputs key for minimap
            //Inputs();
            //controled that minimap follow the target
            PositionControll();
            //Apply rotation settings
            RotationControll();
            //for minimap and world map control
            MapSize();
        }

        /// <summary>
        /// 控制相机位置
        /// </summary>
        void PositionControll()
        {
            if (m_MapType == MapType.Target)
            {
                Vector3 p = Vector3.zero;
                // Update the transformation of the camera as per the target's position.
                p.x = Target.position.x;
                p.z = Target.position.z;
                p.y = transform.position.y;

                MiniMapCamera.transform.position = p;
            }
            else if (m_MapType == MapType.World)
            {
                Vector3 vp = MiniMapCamera.WorldToViewportPoint(m_Target.position);
                PlayerIcon.rectTransform.anchoredPosition = MiniMapUtils.CalculateMiniMapPosition(vp, MiniMapRect);
            }
        }

        /// <summary>
        /// 控制Icon旋转
        /// </summary>
        void RotationControll()
        {
            RectTransform rt = PlayerIcon.GetComponent<RectTransform>();
            Vector3 tr = Target.localEulerAngles;
            Vector3 r = Vector3.zero;
            r.z = 180 - tr.y;
            rt.localEulerAngles = r;
        }

        /// <summary>
        /// 小地图尺寸
        /// </summary>
        void MapSize()
        {
            MiniMapCamera.orthographicSize = Mathf.Lerp(MiniMapCamera.orthographicSize, DefaultSize, Time.deltaTime * LerpSize);
        }

        /// <summary>
        /// 控制
        /// </summary>
        /// <param name="b"></param>
        public void ChangeSize(bool b)
        {
            if (b)
            {
                if (DefaultSize + scrollSensitivity <= maxSize)
                {
                    DefaultSize += scrollSensitivity;
                }
                else
                {
                    DefaultSize = maxSize;
                }
            }
            else
            {
                if (DefaultSize - scrollSensitivity >= minSize)
                {
                    DefaultSize -= scrollSensitivity;
                }
                else
                {
                    DefaultSize = minSize;
                }
            }
        }

        /// <summary>
        /// 添加小地图标签
        /// </summary>
        /// <param name="marker"></param>
        public void AddMarker(MiniMapMarker marker)
        {
            marker.OnClicked.AddListener(marker_OnClicked);
            markerList.Add(marker);
        }

        /// <summary>
        /// 移除小地图标签
        /// </summary>
        /// <param name="marker"></param>
        protected void RemoveMarker(MiniMapMarker marker)
        {
            marker.OnClicked.RemoveListener(marker_OnClicked);
            markerList.Remove(marker);
            marker.gameObject.SetActive(false);
        }

        /// <summary>
        /// 清空小地图标签
        /// </summary>
        protected void ClearMarker()
        {
            foreach (var marker in markerList)
            {
                RemoveMarker(marker);
            }
        }

        /// <summary>
        /// 标签点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void marker_OnClicked(MiniMapMarker item)
        {
            ItemClicked.Invoke(item);
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public void FocusPlayer()
        {
            Vector3 p = Vector3.zero;
            // Update the transformation of the camera as per the target's position.
            p.x = Target.position.x;
            p.y = MiniMapCamera.transform.position.y;
            p.z = Target.position.z;
            
            MiniMapCamera.transform.position = p;
        }
    }
}
