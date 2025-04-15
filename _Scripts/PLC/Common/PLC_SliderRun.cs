using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.PLC;

namespace XFramework.PLC
{
    class PLC_SliderRun : MonoBehaviour
    {
        public class ControlEvent : UnityEvent<string> { }

        private ControlEvent SliderValueOver = new ControlEvent();
        /// <summary>
        /// slider的值是否达到100
        /// </summary>
        public ControlEvent m_SliderValueOver
        {
            get
            {
                return SliderValueOver;
            }
        }

        public static PLC_SliderRun m_SliderRun = null;

        /// <summary>
        /// 获取当前Slider
        /// </summary>
        private Slider sliderCheck;

        /// <summary>
        /// 滚动条增加速度
        /// </summary>
        public float addSpeed = 2.0f;

        /// <summary>
        /// 文本框提示
        /// </summary>
        [HideInInspector] public Text text_SliderHint;

        /// <summary>
        /// 滚动条的值
        /// </summary>
        private Text text_ProgressBarNum;

        /// <summary>
        /// 判断滚动条是否已经走完, true：正在运行，false：运行完毕
        /// </summary>
        public bool isOver = true;

        /// <summary>
        /// 是否是开机启动
        /// </summary>
        public bool isStartUp = false;

        void Awake()
        {
            m_SliderRun = null;

            if (m_SliderRun == null)
            {
                m_SliderRun = this;
            }
        }

        void Start()
        {
            isOver = true;

            //滚动条的位置
            sliderCheck = transform.Find("Slider").GetComponent<Slider>();
            //文本框
            text_SliderHint = transform.Find("Text_Hint").GetComponent<Text>();

            //滚动条的值
            text_ProgressBarNum = transform.Find("Slider/FirstBg/Text_ProgressBarNum").GetComponent<Text>();

            StartCoroutine(SliderAdd());
        }

        IEnumerator SliderAdd()
        {
            //从0开始
            sliderCheck.value = 0;

            while (true)
            {
                yield return new WaitForSeconds(0.02f);

                sliderCheck.value += addSpeed;
                //滚动条的值
                text_ProgressBarNum.text = (sliderCheck.value).ToString();                

                if (sliderCheck.value >= 100)
                {
                    sliderCheck.value = 100;

                    isOver = false;

                    if (isStartUp)
                    {
                        text_SliderHint.text = "加载成功，正在进入登陆界面...";
                    }
                    //else
                    //{
                    //    //text_SliderHint.text = "";
                    //}

                    m_SliderValueOver.Invoke("");

                    yield return new WaitForSeconds(0.7f);

                    Destroy(gameObject);

                    break;
                }
            }
        }
    }
}
