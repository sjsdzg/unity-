using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 土建透明度 管理
    /// </summary>
	public class BuildingTransparency : MonoBehaviour {
	
		// Use this for initialization
		void Start () {
			
		}
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetTransparency();
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                ResetInfo();
            }
        }
        /// <summary>
        /// 设置透明
        /// </summary>
		public void SetTransparency()
        {
            TransparentHelper.SetObjectAlpha(gameObject,0.2f);
        }
        public void ResetInfo()
        {
            TransparentHelper.RestoreBack(gameObject);

        }
    }
}