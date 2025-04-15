using System.Collections.Generic;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using DG.Tweening;
using XFramework.Core;
using UnityEngine.Events;

namespace XFramework.Simulation
{
    /// <summary>
    /// 镜头观察点Manager
    /// </summary>
    public class FocusManager : Singleton<FocusManager>
    {
        /// <summary>
        /// 镜头观察点列表
        /// </summary>
        private Dictionary<string, FocusComponent> m_FocusComponents = new Dictionary<string, FocusComponent>();

        public void Init(Transform parent)
        {
            m_FocusComponents.Clear();
            if (parent != null)
            {
                foreach (Transform item in parent)
                {
                    FocusComponent component = item.GetOrAddComponent<FocusComponent>();
                    m_FocusComponents.Add(item.name, component);
                }
            }
        }
        
        public void InvokeFocusAction(string name, UnityAction OnFocusCompolete=null)
        {
            //Vector3 pointPosition = new Vector3();
            foreach (string item in m_FocusComponents.Keys)
            {
                if (item == name)
                {
                    m_FocusComponents[item].Focus(OnFocusCompolete);
                    Camera.main.GetComponent<CameraSwitcher>().Switch(CameraStyle.Look);
                }
            }
        }

        public void ExitFocus()
        {
            CameraSwitcher cameraSwitcher = Camera.main.GetComponent<CameraSwitcher>();
            if (cameraSwitcher.CurrentStyle==CameraStyle.Walk)
            {
                return;
            }
            //switch (cameraSwitcher.LastMode)
            //{
            //    case CameraStyle.Walk:
            //        cameraSwitcher.Switch(CameraStyle.Walk);
            //        break;
            //    case CameraStyle.Look:
            //        cameraSwitcher.Switch(CameraStyle.Look);
            //        break;
            //    default:
            //        break;
            //}
            
        }
    }
}
