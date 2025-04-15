using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class EngineeringHandlerBar : MonoBehaviour
    {
        /// <summary>
        /// 模式Toggle
        /// </summary>
        private Toggle toggleMode;

        /// <summary>
        /// 模式Text
        /// </summary>
        private Text textMode;

        /// <summary>
        /// 
        /// </summary>
        private Button buttonFocus;

        /// <summary>
        /// 放大按钮
        /// </summary>
        private Button buttonPlus;

        /// <summary>
        /// 缩小按钮
        /// </summary>
        private Button buttonMinus;

        /// <summary>
        /// 指南针栏
        /// </summary>
        private EngineeringCompassBar compassBar;

        /// <summary>
        /// 轴心点观察组件
        /// </summary>
        private MouseOrbit mouseOrbit;

        /// <summary>
        /// 是否旋转中
        /// </summary>
        private bool isRotating = false;

        /// <summary>
        /// 距离增量
        /// </summary>
        public float distanceIncrement = 10;

        /// <summary>
        /// 角度增量
        /// </summary>
        public float angleIncrement = 90;

        private void Awake()
        {
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();
            toggleMode = transform.Find("ToggleMode").GetComponent<Toggle>();
            textMode = transform.Find("ToggleMode/Text").GetComponent<Text>();
            buttonFocus = transform.Find("ButtonFocus").GetComponent<Button>();
            buttonPlus = transform.Find("ButtonPlus").GetComponent<Button>();
            buttonMinus = transform.Find("ButtonMinus").GetComponent<Button>();
            compassBar = transform.Find("CompassBar").GetComponent<EngineeringCompassBar>();
            //Event
            toggleMode.onValueChanged.AddListener(toggleMode_onValueChanged);
            buttonFocus.onClick.AddListener(buttonFocus_onClick);
            buttonPlus.onClick.AddListener(buttonPlus_onClick);
            buttonMinus.onClick.AddListener(buttonMinus_onClick);
            compassBar.OnRotated.AddListener(compassBar_OnRotated);
            mouseOrbit.OnAngleChanged.AddListener(mouseOrbit_OnAngleChanged);
        }

        /// <summary>
        /// 模式Toggle更改时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleMode_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                textMode.text = "3D";
                mouseOrbit.SetY(45);
            }
            else
            {
                textMode.text = "2D";
                mouseOrbit.SetY(90);
            }
        }

        /// <summary>
        /// 点击时，触发
        /// </summary>
        private void buttonPlus_onClick()
        {
            mouseOrbit.Zoom(-distanceIncrement);
        }

        /// <summary>
        /// 缩小按钮点击时，触发
        /// </summary>
        private void buttonMinus_onClick()
        {
            mouseOrbit.Zoom(distanceIncrement);
        }

        /// <summary>
        /// 放大按钮点击时，触发
        /// </summary>
        private void buttonFocus_onClick()
        {
            mouseOrbit.Reset();
        }

        private void mouseOrbit_OnAngleChanged(Quaternion value)
        {
            compassBar.SetCompassAngle(value.eulerAngles.y);
        }

        private void compassBar_OnRotated(float angle)
        {
            //mouseOrbit.Rotate(-angle);
            if (!isRotating)
            {
                StartCoroutine(Rotating(-angle));
            }
        }

        /// <summary>
        /// 防止旋转幅度过大
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        IEnumerator Rotating(float angle)
        {
            isRotating = true;
            float sign = angle / Mathf.Abs(angle);//正负号
            float speed = 3f * sign;//一帧增加3°
            while (Mathf.Abs(angle) > 0)
            {
                mouseOrbit.Rotate(speed);
                angle -= speed;
                yield return new WaitForEndOfFrame();
            }
            isRotating = false;
        }
    }
}
