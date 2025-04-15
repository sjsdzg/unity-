using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 小地图控制器
    /// </summary>
    public class DragMiniMap : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        /// <summary>
        /// 自身Rect
        /// </summary>
        private RectTransform m_Rect;

        /// <summary>
        /// 按下鼠标的位置
        /// </summary>
        private Vector2 originalLocalPointerPosition;

        /// <summary>
        /// 拖动鼠标的位置
        /// </summary>
        private Vector2 localPointerPosition;

        /// <summary>
        /// 小地图相机原始位置
        /// </summary>
        private Vector3 originalMiniMapCameraPosition;

        /// <summary>
        /// 小地图
        /// </summary>
        public MiniMap m_MiniMap;

        /// <summary>
        /// 原始相机中心位置
        /// </summary>
        private Vector3 originalMiniMapCameraCenter;

        /// <summary>
        /// 尺寸限制
        /// </summary>
        public Vector2 ClampSize = Vector2.zero;

        void Awake()
        {
            m_Rect = transform as RectTransform;
            originalMiniMapCameraCenter = m_MiniMap.MiniMapCamera.transform.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            originalMiniMapCameraPosition = m_MiniMap.MiniMapCamera.transform.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Rect, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (m_Rect == null)
                return;
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Rect, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                Vector3 offset = MiniMapUtils.CalculateDragOffset(m_MiniMap, offsetToOriginal);
                Vector3 pos = originalMiniMapCameraPosition + offset;
                ClampToPosition(pos);
            }
        }

        /// <summary>
        /// 限制小地图相机位置
        /// </summary>
        /// <param name="pos"></param>
        public void ClampToPosition(Vector3 pos)
        {
            //if (pos.x + m_MiniMap.DefaultSize < originalMiniMapCameraCenter.x + ClampSize.x
            //&& pos.x - m_MiniMap.DefaultSize > originalMiniMapCameraCenter.x - ClampSize.x
            //&& pos.z + m_MiniMap.DefaultSize * ClampSize.y / ClampSize.x < originalMiniMapCameraCenter.z + ClampSize.y
            //&& pos.z - m_MiniMap.DefaultSize * ClampSize.y / ClampSize.x > originalMiniMapCameraCenter.z - ClampSize.y)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            pos.x = Mathf.Clamp(pos.x, originalMiniMapCameraCenter.x - ClampSize.x + m_MiniMap.DefaultSize, originalMiniMapCameraCenter.x + ClampSize.x - m_MiniMap.DefaultSize);
            pos.z = Mathf.Clamp(pos.z, originalMiniMapCameraCenter.z - ClampSize.y + m_MiniMap.DefaultSize * ClampSize.y / ClampSize.x, originalMiniMapCameraCenter.z + ClampSize.y - m_MiniMap.DefaultSize * ClampSize.y / ClampSize.x);
            m_MiniMap.MiniMapCamera.transform.position = pos;
        }

        void Update()
        {
            ClampToPosition(m_MiniMap.MiniMapCamera.transform.position);

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                m_MiniMap.ChangeSize(false);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                m_MiniMap.ChangeSize(true);
            }
        }
    }
}
