using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;
using DG.Tweening;

namespace XFramework.Simulation
{
    /// <summary>
    /// 镜头观察点Manager
    /// </summary>
    public class CameraLookPointManager : Singleton<CameraLookPointManager>
    {
        /// <summary>
        /// 镜头观察点列表
        /// </summary>
        private Dictionary<string, Transform> m_LookPoints = new Dictionary<string, Transform>();

        public void Init(Transform parent)
        {
            m_LookPoints.Clear();
            if (parent != null)
            {
                foreach (Transform item in parent)
                {
                    m_LookPoints.Add(item.name, item);
                }
            }
        }

        /// <summary>
        /// 获取镜头的position及rotation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void InvokeLookPointPositionAndRotation(string name)
        {
            //Vector3 pointPosition = new Vector3();
            foreach (string item in m_LookPoints.Keys)
            {
                if (item == name)
                {
                    Camera.main.transform.position = m_LookPoints[item].transform.position;
                    Camera.main.transform.rotation = m_LookPoints[item].transform.rotation;
                }
            }
        }

        /// <summary>
        /// 获取镜头的position
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void InvokeLookPointPosition(string name)
        {
            //Vector3 pointPosition = new Vector3();
            foreach (string item in m_LookPoints.Keys)
            {
                if (item == name)
                {
                    Camera.main.transform.position =  m_LookPoints[item].transform.position;
                }
            }
        }

        /// <summary>
        /// 获取镜头的rotation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void InvokeLookPointRotation(string name)
        {
            //Vector3 pointRotation = new Vector3();
            foreach (string item in m_LookPoints.Keys)
            {
                if (item == name)
                {
                    Camera.main.transform.rotation = m_LookPoints[item].transform.rotation;
                }
            }
        }
        /// <summary>
        /// 调用镜头
        /// </summary>
        public void InvokeCameraLookPoint(string name)
        {
            switch (Camera.main.GetComponent<CameraSwitcher>().CurrentStyle)
            {
                case CameraStyle.Walk:
                    Camera.main.GetComponent<CameraSwitcher>().Switch(CameraStyle.Look);
                    break;
                case CameraStyle.Look:
                    break;
                default:
                    break;
            }
            foreach (string item in m_LookPoints.Keys)
            {
                if (item == name)
                {
                    Camera.main.transform.position= m_LookPoints[item].transform.position;
                    Camera.main.transform.rotation = m_LookPoints[item].transform.rotation;
                }
            }
        }
        /// <summary>
        /// 调用镜头
        /// </summary>
        public void InvokeCameraLookPoint(string name,float time)
        {
            foreach (string item in m_LookPoints.Keys)
            {
                if (item == name)
                {
                    Camera.main.transform.DOMove(m_LookPoints[item].transform.position, time);
                    Camera.main.transform.DORotate(m_LookPoints[item].transform.eulerAngles, time).OnComplete(()=> {
                        Camera.main.GetComponent<CameraSwitcher>().enabled = true;
                        EventDispatcher.ExecuteEvent(Events.Prompt.Show);
                    });                  
                }
            }
        }
    }
}
