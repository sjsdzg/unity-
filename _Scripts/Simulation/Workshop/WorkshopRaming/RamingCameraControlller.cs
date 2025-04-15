using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;
using XFramework.UI;
using XFramework.Common;
namespace XFramework.UI
{
    /// <summary>
    /// 漫游中相机控制
    /// </summary>
	public class RamingCameraControlller : MonoBehaviour {


        private CameraController m_CameraCtrl;

        private MouseOrbit m_MouseOrbit;

        
		void Start () {
			
		}
		
		public void FollowTarget(Transform target)
        {
            
        }
	}
}