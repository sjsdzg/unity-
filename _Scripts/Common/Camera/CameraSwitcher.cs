using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Common
{
    public class CameraSwitchEvent : UnityEvent<CameraStyle> { }

    /// <summary>
    /// Walk>Fixed>Focus
    /// </summary>
    public enum CameraStyle
    {
        /// <summary>
        /// 漫游模式
        /// </summary>
        Walk,
        /// <summary>
        /// Look模式
        /// </summary>
        Look,
        /// <summary>
        /// Free模式
        /// </summary>
        Free,
    }

    /// <summary>
    /// 相机切换
    /// </summary>
    public class CameraSwitcher : MonoBehaviour
    {
        private CameraSwitchEvent m_OnCameraSwitch = new CameraSwitchEvent();
        /// <summary>
        /// 相机切换事件
        /// </summary>
        public CameraSwitchEvent OnCameraSwitch
        {
            get { return m_OnCameraSwitch; }
            set { m_OnCameraSwitch = value; }
        }

        /// <summary>
        /// 主相机
        /// </summary>
        private Transform m_Camera;

        /// <summary>
        /// 最后漫游视角
        /// </summary>
        private Vector3 m_CameraPosition;

        /// <summary>
        /// 最后漫游角度
        /// </summary>
        private Quaternion m_CameraRotation;

        /// <summary>
        /// CameraController
        /// </summary>
        private CameraController m_CameraController;

        /// <summary>
        /// FreeForm
        /// </summary>
        private FreeForm m_FreeForm;

        /// <summary>
        /// MouseLook
        /// </summary>
        private MouseLook m_MouseLook;

        /// <summary>
        /// 相机模式
        /// </summary>
        public CameraStyle CurrentStyle { get; private set; }

        private GameObject player;
        /// <summary>
        /// 游戏对象
        /// </summary>
        public GameObject Player 
        { 
            get
            {
                if (player == null)
                {
                    player = GameObject.FindWithTag("Player");
                }
                return player;
            }
        }

        private void Awake()
        {
            m_Camera = Camera.main.transform;
            CurrentStyle = CameraStyle.Walk;
            m_CameraController = m_Camera.GetComponent<CameraController>();
            m_FreeForm = m_Camera.GetComponent<FreeForm>();
            m_MouseLook = m_Camera.GetComponent<MouseLook>();
            m_MouseLook.enabled = false;
        }

        private void Start()
        {
            //Switch(CurrentStyle);
        }
        
        public void Switch(CameraStyle cameraStyle/*,Transform cameraPoint=null*/)
        {
            if (cameraStyle == CameraStyle.Look)
            {
                Player.SetActive(false);
                EventDispatcher.ExecuteEvent(Events.Prompt.Show, "按<color=red> Esc </color>退出观察模式");

                if (CurrentStyle==CameraStyle.Walk)
                {
                    m_CameraPosition = m_Camera.position;
                    m_CameraRotation = m_Camera.rotation;
                }

                m_CameraController.enabled = false;
                m_FreeForm.enabled = false;
                m_MouseLook.enabled = true;
            }
            else if (cameraStyle == CameraStyle.Walk) ///默认overlook    
            {
                Player.SetActive(true);
                EventDispatcher.ExecuteEvent(Events.Prompt.Hide);

                m_CameraController.enabled = true;
                m_FreeForm.enabled = true;
                m_MouseLook.enabled = false;
                m_Camera.position = m_CameraPosition;
                m_Camera.rotation = m_CameraRotation;
                LookPointManager.Instance.isEnterAngle = false;
            }
            else if (cameraStyle == CameraStyle.Free) ///默认overlook    
            {
                Player.SetActive(false);
                EventDispatcher.ExecuteEvent(Events.Prompt.Hide);

                m_CameraController.enabled = false;
                m_FreeForm.enabled = false;
                m_MouseLook.enabled = false;
            }
            CurrentStyle = cameraStyle;
            OnCameraSwitch.Invoke(CurrentStyle);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (CurrentStyle)
                {
                    case CameraStyle.Walk:
                        Switch(CameraStyle.Walk);
                        break;
                    case CameraStyle.Look:
                        Switch(CameraStyle.Walk);
                        break;
                    case CameraStyle.Free:
                        Switch(CameraStyle.Walk);
                        break;
                    default:
                        Switch(CameraStyle.Walk);
                        break;
                }
            }
        }
    }
}
