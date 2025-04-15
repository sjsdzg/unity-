using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 镜头观察点Manager
    /// </summary>
    public class LookPointManager : Singleton<LookPointManager>
    {
        private string m_CurrentName;
         /// <summary>
        /// 当前观察点名称
        /// </summary>
        public string CurrentName
        {
            get { return m_CurrentName; }

            set
            {
                if (m_CurrentName == value)
                    return;

                m_CurrentName = value;                
                if (!isEnterAngle)
                    return;

                EnterCurrent();
            }
        }
        public bool isEnterAngle = false;
        private CameraSwitcher m_CameraSwitcher;
        /// <summary>
        /// CameraSwitcher
        /// </summary>
        public CameraSwitcher CameraSwitcher
        {
            get 
            {
                if (m_CameraSwitcher == null)
                {
                    m_CameraSwitcher = Camera.main.GetComponent<CameraSwitcher>();
                }
                return m_CameraSwitcher;
            }
        }
        private Common.MouseLook m_MouseLook;
        /// <summary>
        /// 
        /// </summary>
        public Common.MouseLook MouseLook
        {
            get 
            {
                if (m_MouseLook == null)
                {
                    m_MouseLook = Camera.main.GetComponent<Common.MouseLook>();
                }
                
                return m_MouseLook;
            }
        }     
        /// <summary>
        /// 镜头观察点列表
        /// </summary>
        private Dictionary<string, Transform> m_LookPoints = new Dictionary<string, Transform>();

        public void Init(Transform parent)
        {
            if (parent == null)
                return;

            m_LookPoints.Clear();
            foreach (Transform item in parent)
            {
                m_LookPoints.Add(item.name, item);
            }
        }
        
        public void EnterCurrent()
        {
            Enter(CurrentName);
        }

        public void Enter(string name, UnityAction callback = null, bool instant = true, float duration = 1f)
        {
            foreach (string key in m_LookPoints.Keys)
            {
                if (key.Equals(name))
                {
                    CameraSwitcher.Switch(CameraStyle.Look);
                    Transform trans = m_LookPoints[key];
                    MouseLook.Teleport(trans, callback, instant, duration);
                    break;
                }
            }
        }
    }
}
