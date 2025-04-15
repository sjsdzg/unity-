using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.Common
{
    public class Utils
    {
        /// <summary>
        /// 判断是否在UI上
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == 5)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// PointerEnter GameObject
        /// </summary>
        /// <returns></returns>
        public static GameObject CurrentPointerEnterGameObject()
        {
            if (EventSystem.current == null)
                return null;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                return results[0].gameObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 返回一个临时物体(在鼠标位置)
        /// </summary>
        /// <returns></returns>
        public static GameObject NewGameObject(PositionType type = PositionType.ScreenCenter, float destroyTime = 10)
        {
            Vector3 position = Vector3.zero;
            //鼠标位置
            switch (type)
            {
                case PositionType.MousePosition:
                    position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 50, 0);
                    break;
                case PositionType.ScreenCenter:
                    position = new Vector3(Screen.width / 2, Screen.height / 2);
                    break;
                default:
                    break;
            }
            //产生物体
            Ray ray = Camera.main.ScreenPointToRay(position);
            GameObject go = new GameObject();
            go.transform.position = ray.GetPoint(5);
            go.AddComponent<MeshCollider>();
            GameObject.Destroy(go, destroyTime);
            return go;
        }
    }

    public enum PositionType
    {
        /// <summary>
        /// 鼠标位置
        /// </summary>
        MousePosition,
        /// <summary>
        /// 屏幕中心
        /// </summary>
        ScreenCenter,
    }
}
