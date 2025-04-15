using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.Architectural
{
    /// <summary>
    /// 相机模式
    /// </summary>
    public enum CameraMode
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 2D
        /// </summary>
        Visual2D,
        /// <summary>
        /// 3D
        /// </summary>
        Visual3D,
    }

    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera m_MainCamera;

        [SerializeField]
        private CameraMode m_CameraMode;

        [SerializeField]
        private Transform m_Pivot = null;

        private VisualCamera2D m_CameraView2D;
        private VisualCamera3D m_CameraView3D;

        public Camera MainCamera
        {
            get
            {
                if (m_MainCamera == null)
                {
                    m_MainCamera = Camera.main;
                }

                return m_MainCamera;
            }
        }

        /// <summary>
        /// 相机模式
        /// </summary>
        public CameraMode CameraMode
        {
            get
            {
                return m_CameraMode;
            }
            set
            {
                if (m_CameraMode == value)
                    return;

                var prevMode = m_CameraMode;
                var nextMode = value;
                //退出
                if (CurrentCameraView != null)
                {
                    CurrentCameraView.Exit(nextMode);
                }
                m_CameraMode = value;
                //进入
                if (CurrentCameraView != null)
                {
                    CurrentCameraView.Enter(prevMode);
                }
               
            }
        }

        /// <summary>
        /// 轴心点
        /// </summary>
        public Transform Pivot
        {
            get
            {
                if (m_Pivot == null)
                {
                    GameObject pivot = new GameObject("Pivot");
                    if (transform.parent != null)
                    {
                        pivot.transform.SetParent(transform.parent);
                    }
                    pivot.transform.position = VisualCameraSettings.Visual3D.pivot;
                    m_Pivot = pivot.transform;
                }
                return m_Pivot;
            }
        }

        public VisualCameraBase CurrentCameraView
        {
            get
            {
                switch (m_CameraMode)
                {
                    case CameraMode.None:
                        return null;
                    case CameraMode.Visual2D:
                        return m_CameraView2D;
                    case CameraMode.Visual3D:
                        return m_CameraView3D;
                    default:
                        return null;
                }
            }
        }

        void Awake()
        {
            m_CameraView2D = new VisualCamera2D(this);
            m_CameraView3D = new VisualCamera3D(this);
        }

        void Start()
        {
            CameraMode = CameraMode.Visual2D;
        }

        void Update()
        {
            if (MainCamera == null)
                return;

            if (CurrentCameraView == null)
                return;

            if (IsPointerOverUI())
                return;

            CurrentCameraView.Update();
            
        }


        /// <summary>
        /// 判断是否在UI上
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            if (EventSystem.current.gameObject == null)
                return true;
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
        private void OnDestroy()
        {
            GraphicManager.Instance.Release();
        }
    }
}