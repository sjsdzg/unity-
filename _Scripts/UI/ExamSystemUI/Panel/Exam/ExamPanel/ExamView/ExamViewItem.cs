using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class ExamViewItem : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<int> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 题目序号点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 图片Image
        /// </summary>
        private Image image;

        /// <summary>
        /// 序号Text
        /// </summary>
        private Text textNumber;

        /// <summary>
        /// 按钮
        /// </summary>
        private Button button;

        private int number;
        /// <summary>
        /// 序号
        /// </summary>
        public int Number
        {
            get
            {
                number = int.Parse(textNumber.text);
                return number;
            }
            set
            {
                number = value;
                textNumber.text = number.ToString();
            }
        }

        private bool isOn;
        /// <summary>
        /// 开启/关闭
        /// </summary>
        public bool IsOn
        {
            get{ return isOn; }
            set
            {
                isOn = value;
                if (isOn)
                {
                    image.color = _onColor;
                }
                else
                {
                    image.color = _offColor;
                }
            }
        }

        /// <summary>
        /// 亮
        /// </summary>
        public Color _onColor;
        /// <summary>
        /// 暗
        /// </summary>
        public Color _offColor;

        void Awake()
        {
            textNumber = transform.Find("Text").GetComponent<Text>();
            image = transform.GetComponent<Image>();
            button = transform.GetComponent<Button>();
            button.onClick.AddListener(button_OnClicked);

            IsOn = false;
        }

        /// <summary>
        /// 题目序号点击时，触发
        /// </summary>
        /// <param name="value"></param>
        private void button_OnClicked()
        {
            OnClicked.Invoke(Number);
        }

    }
}
