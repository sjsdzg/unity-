using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.UI
{
    public enum ViewMode
    {
        /// <summary>
        /// 俯瞰
        /// </summary>
        Overlook,
        /// <summary>
        /// 漫游
        /// </summary>
        Roaming,

        /// <summary>
        /// 禁用模式
        /// </summary>
        Diable,
    }
    /// <summary>
    /// 相机选择
    /// </summary>
	public class RamingCameraSwich : MonoBehaviour {

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
        public ViewMode CurrentMode { get; private set; }


        //public  ViewMode lastMode;
        void Awake()
        {
            CurrentMode = ViewMode.Roaming;
            m_Camera = Camera.main.transform;
        }

        public ViewMode Switch(ViewMode mode)
        {
            if (CurrentMode == mode)
                return mode;

            if (mode == ViewMode.Overlook)
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
            else if (mode == ViewMode.Roaming) ///默认overlook    
            {
                overlookPos = m_Camera.position;
                overlookRot = m_Camera.eulerAngles;

                m_Camera.GetComponent<CameraController>().enabled = true;
                m_Camera.GetComponent<FreeForm>().enabled = true;
                if (m_Camera.GetComponent<MouseOrbit>() != null)
                {
                    m_Camera.GetComponent<MouseOrbit>().enabled = false;
                }

                m_Camera.eulerAngles = new Vector3(0, 180, 0);
                isOn = true;
            }
            else if (mode == ViewMode.Diable)
            {
                m_Camera.GetComponent<CameraController>().enabled = false;
                m_Camera.GetComponent<FreeForm>().enabled = false;
                if (m_Camera.GetComponent<MouseOrbit>() != null)
                {
                    m_Camera.GetComponent<MouseOrbit>().enabled = false;
                }
            }

            CurrentMode = mode;
            return mode;
        }
    }
}