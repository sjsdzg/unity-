using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 相机模式
    /// </summary>
    public enum CameraMode
    {
        /// <summary>
        /// 俯瞰
        /// </summary>
        Overlook,
        /// <summary>
        /// 漫游
        /// </summary>
        Roaming
    }

    /// <summary>
    /// 相机切换
    /// </summary>
    public class SwitchCameraMode : MonoBehaviour
    {
        /// <summary>
        /// 主相机
        /// </summary>
        private Transform m_Camera;

        /// <summary>
        /// 俯瞰最后视角
        /// </summary>
        private Vector3 overlookPos;

        /// <summary>
        /// 俯瞰最后角度
        /// </summary>
        private Vector3 overlookRot;

        /// <summary>
        /// 是否执行切换过程
        /// </summary>
        private bool isOn = false;

        /// <summary>
        /// 相机模式
        /// </summary>
        public CameraMode CurrentMode { get; private set; }

        void Awake()
        {
            m_Camera = Camera.main.transform;
        }

        public void Switch(CameraMode mode)
        {
            if (CurrentMode == mode)
                return;

            if (mode == CameraMode.Overlook)
            {
                if (isOn)
                {
                    m_Camera.position = overlookPos;
                    m_Camera.eulerAngles = overlookRot;
                }

                m_Camera.GetComponent<CameraController>().enabled = false;
                m_Camera.GetComponent<FreeForm>().enabled = false;
                if (m_Camera.GetComponent<MouseOrbit>() != null)
                    m_Camera.GetComponent<MouseOrbit>().enabled = true;
            }
            else if (mode == CameraMode.Roaming)
            {
                overlookPos = m_Camera.position;
                overlookRot = m_Camera.eulerAngles;

                m_Camera.GetComponent<CameraController>().enabled = true;
                m_Camera.GetComponent<FreeForm>().enabled = true;
                if(m_Camera.GetComponent<MouseOrbit>()!=null)
                    m_Camera.GetComponent<MouseOrbit>().enabled = false;
                m_Camera.eulerAngles = new Vector3(0, 180, 0);
                isOn = true;
            }

            CurrentMode = mode;
        }
    }
}
