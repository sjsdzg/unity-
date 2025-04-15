using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.Common
{
    public class MarkerMap : MonoBehaviour
    {
        /// <summary>
        /// The Canvas Root of scene.
        /// </summary>
        public Transform CanvasParent;

        /// <summary>
        /// 小地图标签列表
        /// </summary>
        private List<Marker> markerList = new List<Marker>();

        /// <summary>
        /// 隐藏距离
        /// </summary>
        public float HideDistance = 20;

        /// <summary>
        /// 最大视角
        /// </summary>
        [Range(0, 180)]
        public float MaxViewAngle = 90;

        private Camera m_Cam = null;
        /// <summary>
        /// Get the uiCamera / PlayerCamera
        /// </summary>
        public Camera MCamera
        {
            get
            {
                if (m_Cam == null)
                {
                    m_Cam = (Camera.main != null) ? Camera.main : Camera.current;
                }

                return m_Cam;
            }
        }

        public class OnClickedEvent : UnityEvent<Marker> { }

        private OnClickedEvent markerClicked = new OnClickedEvent();
        /// <summary>
        /// 标签项点击事件
        /// </summary>
        public OnClickedEvent MarkerClicked
        {
            get { return markerClicked; }
            set { markerClicked = value; }
        }

        /// <summary>
        /// 添加图标签
        /// </summary>
        /// <param name="marker"></param>
        public void AddMarker(Marker marker)
        {
            marker.OnClicked.AddListener(marker_OnClicked);
            markerList.Add(marker);
        }

        /// <summary>
        /// 移除标签
        /// </summary>
        /// <param name="marker"></param>
        protected void RemoveMarker(Marker marker)
        {
            marker.OnClicked.RemoveListener(marker_OnClicked);
            markerList.Remove(marker);
            marker.gameObject.SetActive(false);
        }

        /// <summary>
        /// 清空标签
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
        private void marker_OnClicked(Marker item)
        {
            MarkerClicked.Invoke(item);
        }
    }
}
