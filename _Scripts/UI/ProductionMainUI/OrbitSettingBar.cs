using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 轨道设置栏
    /// </summary>
    public class OrbitSettingBar : MonoBehaviour
    {
        /// <summary>
        /// MouseOrbit.
        /// </summary>
        private MouseOrbit mouseOrbit;

        /// <summary>
        /// 缩放滑动条
        /// </summary>
        private Slider sliderZoom;

        /// <summary>
        /// 正转
        /// </summary>
        private RepeatButton buttonPositive;

        /// <summary>
        /// 反转
        /// </summary>
        private RepeatButton buttonReverse;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public float rotateAngle = 1f;

        void Awake()
        {
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();
            sliderZoom = transform.Find("Zoom/Slider").GetComponent<Slider>();
            buttonPositive = transform.Find("Rotate/ButtonPositive").GetComponent<RepeatButton>();
            buttonReverse = transform.Find("Rotate/ButtonReverse").GetComponent<RepeatButton>();

            //事件
           // mouseOrbit.OnDistanceChange.AddListener(mouseOrbit_OnDistanceChange);
            sliderZoom.onValueChanged.AddListener(sliderZoom_onValueChanged);
            buttonPositive.OnClick.AddListener(buttonPositive_onClick);
            buttonReverse.OnClick.AddListener(buttonReverse_onClick);
        }

        /// <summary>
        /// 滑动条
        /// </summary>
        /// <param name="f"></param>
        private void sliderZoom_onValueChanged(float f)
        {
            float dis = ConvertToDis(f);
            mouseOrbit.Zoom(dis);
        }

        /// <summary>
        /// MouseOrbit距离更改时，触发
        /// </summary>
        /// <param name="f"></param>
        private void mouseOrbit_OnDistanceChange(float f)
        {
            float value = ConvertToSlider(f);
            sliderZoom.value = value;
        }

        /// <summary>
        /// 正转按钮点击时，触发
        /// </summary>
        private void buttonPositive_onClick()
        {
            mouseOrbit.Rotate(rotateAngle);
        }

        /// <summary>
        /// 反转按钮点击时，触发
        /// </summary>
        private void buttonReverse_onClick()
        {
            mouseOrbit.Rotate(-rotateAngle);
        }

        /// <summary>
        /// 从slider值转化distance
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private float ConvertToDis(float f)
        {
            float distance = (mouseOrbit.maxDistance - mouseOrbit.minDistance) * (1 - f) + mouseOrbit.minDistance;
            return distance;
        }

        /// <summary>
        /// 从distance转化成Slider的Value
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private float ConvertToSlider(float f)
        {
            float value = (mouseOrbit.maxDistance - f) / (mouseOrbit.maxDistance - mouseOrbit.minDistance);
            return value;
        }
    }
}
