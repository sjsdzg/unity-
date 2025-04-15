using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 简要信息Item
    /// </summary>
    public class BrieflyInfoItem : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<BrieflyInfoItem> { }

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
        /// 数量
        /// </summary>
        private Text textCount;

        /// <summary>
        /// 描述
        /// </summary>
        private Text textDesc;

        /// <summary>
        /// 按钮
        /// </summary>
        private Button button;

        void Awake()
        {
            textCount = transform.Find("Count").GetComponent<Text>();
            textDesc = transform.Find("Desc").GetComponent<Text>();
            button = transform.Find("Count").GetComponent<Button>();
            //事件
            button.onClick.AddListener(buttonExam_onClick);
        }

        /// <summary>
        /// 按钮点击时，触发
        /// </summary>
        private void buttonExam_onClick()
        {
            OnClicked.Invoke(this);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="examName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="score"></param>
        public void SetParams(int number, string desc)
        {
            textCount.text = number.ToString();
            textDesc.text = desc;
        }
    }

}
