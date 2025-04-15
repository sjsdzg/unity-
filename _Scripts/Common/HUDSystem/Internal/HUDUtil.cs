using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class HUDUtil
    {
        /// <summary>
        /// 获取角度
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static float GetAngle(float x1, float y1, float x2, float y2)
        {
            float diferenceX = x2 - x1;
            float diferenceY = y2 - y1;
            float angle = Mathf.Atan(diferenceY / diferenceX) * Mathf.Rad2Deg;
            if (diferenceX < 0)
            {
                angle += 180;
            }
            return angle;
        }

        /// <summary>
        /// 获取当前相机
        /// </summary>
        public static Camera mCamera
        {
            get
            {
                if (Camera.main != null)
                {
                    return Camera.main;
                }
                else
                {
                    return Camera.current;
                }
            }
        }

        /// <summary>
        /// get the position of target in screen
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 ScreenPosition(Vector3 position)
        {
            return mCamera.WorldToScreenPoint(position);
        }

        /// <summary>
        /// 位置在相机后面
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool BehindCamera(Vector3 position)
        {
            float dot = Vector3.Dot(mCamera.transform.forward, position - mCamera.transform.position);
            if (dot < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// is or not in screen
        /// </summary>
        /// <param name="position"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static bool OnScreen(Vector3 position, float border)
        {
            //is or not behind the camera.
            float dot = Vector3.Dot(mCamera.transform.forward, position - mCamera.transform.position);
            if (dot < 0 )
            {
                return false;
            }
            //get the position in screen
            Vector3 screenPos = ScreenPosition(position);
            if (screenPos.x > border && screenPos.x < Screen.width - border && screenPos.y > border && screenPos.y < Screen.height - border)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
