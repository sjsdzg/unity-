using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 工艺标签组件
    /// </summary>
    public class ProcessLabelComponent : MonoBehaviour
    {
        private Transform m_Cam = null;

        private Vector3 _scale = Vector3.zero;

        private bool isOn = false;

        void Awake()
        {
            m_Cam = Camera.main.transform;
            _scale = transform.localScale;
        }

        void Update()
        {
            if (isOn)
            {
                transform.eulerAngles = m_Cam.eulerAngles;
            }         
        }

        /// <summary>
        /// 出现
        /// </summary>
        public void Appear()
        {
            gameObject.SetActive(true);
            transform.DOScale(_scale, 0.5f);
            isOn = true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Disappear()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
            isOn = false;
        }
    }
}
