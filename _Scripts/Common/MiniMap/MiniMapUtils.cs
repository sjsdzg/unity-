using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 小地图工具集
    /// </summary>
    public static class MiniMapUtils
    {
        /// <summary>
        /// 计算ViewPoint在小地图上的位置
        /// </summary>
        /// <param name="viewPoint"></param>
        /// <param name="maxAnchor"></param>
        /// <returns></returns>
        public static Vector3 CalculateMiniMapPosition(Vector3 viewPoint, RectTransform maxAnchor)
        {
            viewPoint = new Vector2((viewPoint.x * maxAnchor.sizeDelta.x) - (maxAnchor.sizeDelta.x * 0.5f),
                (viewPoint.y * maxAnchor.sizeDelta.y) - (maxAnchor.sizeDelta.y * 0.5f));

            return viewPoint;
        }
        /// <summary>
        /// 计算ViewPoint在小地图上的位置  相反位置
        /// </summary>
        /// <param name="viewPoint"></param>
        /// <param name="maxAnchor"></param>
        /// <returns></returns>
        public static Vector3 CalculateMiniMapPositionEx(Vector3 viewPoint, RectTransform maxAnchor)
        {
            viewPoint = new Vector2(-((viewPoint.x * maxAnchor.sizeDelta.x) - (maxAnchor.sizeDelta.x * 0.5f)),
               -((viewPoint.y * maxAnchor.sizeDelta.y) - (maxAnchor.sizeDelta.y * 0.5f)));

            return viewPoint;
        }
        /// <summary>
        /// 计算小地图点在世界上的坐标
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="anchor"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 CalculateDragOffset(MiniMap miniMap, Vector3 offsetToOriginal)
        {
            Vector3 offset = new Vector3(offsetToOriginal.x * miniMap.DefaultSize * 2 / miniMap.MiniMapRect.sizeDelta.x, 0, offsetToOriginal.y * miniMap.DefaultSize * 2 / miniMap.MiniMapRect.sizeDelta.y);
            return offset;
        }

        /// <summary>
        /// 获取大地图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MiniMap GetWorldMap(string name)
        {
            MiniMap miniMap = null;
            MiniMap[] miniMaps = GameObject.FindObjectsOfType<MiniMap>();
            for (int i = 0; i < miniMaps.Length; i++)
            {
                if (miniMaps[i].m_MapType == MapType.World && miniMaps[i].MiniMapName == name)
                {
                    miniMap = miniMaps[i];
                    break;
                }
            }

            return miniMap;
        }

        /// <summary>
        /// 获取相机
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Camera GetCameraWithTag(string tag)
        {
            foreach (var camera in Camera.allCameras)
            {
                //if (camera.tag.Equals(tag))
                //{
                //    return camera;
                //}
                if (camera.name.Equals(tag))
                {
                    return camera;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取场景中所有小地图
        /// </summary>
        /// <returns></returns>
        public static MiniMap[] GetAllMiniMap()
        {
            MiniMap[] miniMaps = GameObject.FindObjectsOfType<MiniMap>();
            return miniMaps;
        }
    }
}
