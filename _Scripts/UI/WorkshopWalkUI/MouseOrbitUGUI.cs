using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    public class MouseOrbitUGUI : MonoBehaviour
    {
        /// <summary>
        /// 观察目标
        /// </summary>
        public Transform target;

        /// <summary>
        /// 鼠标中间滚动控制相机前后移动速度
        /// </summary>
        public int scrollSensitivity = 20;

        /// <summary>
        /// 相机离目标最小距离
        /// </summary>
        public float minDistance = 3f;

        /// <summary>
        /// 相机离目标最大距离
        /// </summary>
        public float maxDistance = 10;

        private float distance;
        /// <summary>
        /// 相机到目标距离
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// 插值速度
        /// </summary>
        public float lerpSpeed = 10f;

        /// <summary>
        /// 按下鼠标右键旋转视角时X轴旋转速度
        /// </summary>
        private float xSpeed = 250.0f;

        /// <summary>
        /// 按下鼠标右键旋转视角时Y轴旋转速度
        /// </summary>
        private float ySpeed = 150.0f;

        /// <summary>
        /// 相机旋转角度在X轴上限制
        /// </summary>
        public int yMaxLimit = 90;

        /// <summary>
        /// 相机旋转角度在Y轴上限制
        /// </summary>
        public int yMinLimit = -90;

        /// <summary>
        /// 相机在X轴上的角度
        /// </summary>
        private float x = 0.0f;

        /// <summary>
        /// 相机在Y轴上的角度
        /// </summary>
        private float y = 0.0f;

        /// <summary>
        /// 相机旋转角度
        /// </summary>
        private Quaternion rotation;

        /// <summary>
        /// 相机旋转目标角度
        /// </summary>
        private Quaternion desiredRotation;

        /// <summary>
        /// 相机位置
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// 相机目标位置
        /// </summary>
        private Vector3 desiredPosition;

        /// <summary>
        /// 相机原始位置
        /// </summary>
        private Vector3 rawCameraPos;

        /// <summary>
        /// 目标原始位置
        /// </summary>
        private Vector3 rawTargetPos;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled = true;

        void Awake()
        {
            //目标与摄像机之间的距离
            Distance = Vector3.Distance(target.position, transform.position);
            //相机欧拉角
            Vector3 angle = transform.eulerAngles;
            y = angle.x;
            x = angle.y;
            //相机位置和角度
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
            transform.rotation = rotation;
            transform.position = position;
            transform.LookAt(target);//摄像机对着目标

            //相机初始位置
            rawCameraPos = transform.position;
            rawTargetPos = target.position;
        }

        void LateUpdate()
        {
            if (!Enabled)
                return;

            #region 通过鼠标操作，控制相机视角，观察场景

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * (xSpeed) * 0.04f;
                y -= Input.GetAxis("Mouse Y") * (ySpeed) * 0.02f;
                y = ClampAngle(this.y, (float)yMinLimit, (float)yMaxLimit);//从最小大最大变化

                rotation = Quaternion.Euler(y, x, 0);
                position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
                transform.position = position;
                transform.rotation = rotation;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)//滚动中键，视角缩放
            {
                rotation = Quaternion.Euler(y, x, 0);
                Distance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity, minDistance, maxDistance);
                transform.position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
                position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
            }
            #endregion

        }

        /// <summary>
        /// 相机角度限制
        /// </summary>
        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
            {
                angle += 360f;
            }
            if (angle > 360f)
            {
                angle -= 360f;
            }
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            transform.position = rawCameraPos;
            target.position = rawTargetPos;
            transform.LookAt(target);//摄像机对着目标
            //相机欧拉角
            Vector3 angle = transform.eulerAngles;
            y = angle.x;
            x = angle.y;

            //目标与摄像机之间的距离
            Distance = Vector3.Distance(target.position, transform.position);
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="dis"></param>
        public void Zoom(float dis)
        {
            rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.Clamp(dis, minDistance, maxDistance);
            transform.position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        }

        /// <summary>
        /// 绕着轴心X轴旋转
        /// </summary>
        /// <param name="angle"></param>
        public void RotateAroundX(float angle)
        {
            x += angle;
            rotation = Quaternion.Euler(y, x, 0);
            position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
