using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class EngineeringCompassBar : MonoBehaviour
    {
        public class RotatedEvent : UnityEvent<float> { }

        private RotatedEvent m_OnRotated = new RotatedEvent();
        /// <summary>
        /// 旋转事件
        /// </summary>
        public RotatedEvent OnRotated
        {
            get { return m_OnRotated; }
            set { m_OnRotated = value; }
        }

        /// <summary>
        /// 指南针
        /// </summary>
        private RectTransform compassRect;

        /// <summary>
        /// 顺时针按钮
        /// </summary>
        private Button buttonCW;

        /// <summary>
        /// 逆时针按钮
        /// </summary>
        private Button buttonCCW;

        /// <summary>
        /// 旋转速度
        /// </summary>
        public float rotateAngle = 90;

        private void Awake()
        {
            compassRect = transform.Find("Compass").GetComponent<RectTransform>();
            buttonCW = transform.Find("ButtonCW").GetComponent<Button>();
            buttonCCW = transform.Find("ButtonCCW").GetComponent<Button>();
            //Event
            buttonCW.onClick.AddListener(buttonCW_onClick);
            buttonCCW.onClick.AddListener(buttonCCW_onClick);
        }

        /// <summary>
        /// 顺时针按钮点击时，触发
        /// </summary>
        private void buttonCW_onClick()
        {
            OnRotated.Invoke(rotateAngle);
        }

        /// <summary>
        /// 逆时针按钮点击时，触发
        /// </summary>
        private void buttonCCW_onClick()
        {
            OnRotated.Invoke(-rotateAngle);
        }

        /// <summary>
        /// 设置指南针角度
        /// </summary>
        /// <param name="value"></param>
        public void SetCompassAngle(float value)
        {
            compassRect.eulerAngles = new Vector3(compassRect.anchoredPosition3D.x, compassRect.anchoredPosition3D.y, value);
        }
    }
}
