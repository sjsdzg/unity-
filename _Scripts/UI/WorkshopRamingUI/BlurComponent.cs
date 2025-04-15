using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 背景模糊组件
    /// </summary>
	public class BlurComponent : MonoBehaviour {

        /// <summary>
        /// 遮挡面片 关闭功能
        /// </summary>
        private Button buttonMask;

        /// <summary>
        /// 相机模糊效果
        /// </summary>
        private DepthOfFieldDeprecated m_depthOfField;

        // Use this for initialization
        void Start () {
            m_depthOfField = Camera.main.GetComponent<DepthOfFieldDeprecated>();
            buttonMask = transform.GetComponent<Button>();
        }


        public void Open()
        {
            m_depthOfField.simpleTweakMode = true;
        }

        public void Close()
        {
            m_depthOfField.simpleTweakMode = false;

        }
    }
}