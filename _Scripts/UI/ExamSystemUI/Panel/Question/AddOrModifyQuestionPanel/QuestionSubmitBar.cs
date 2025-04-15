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
    /// 试题提交栏
    /// </summary>
    public class QuestionSubmitBar : MonoBehaviour
    {
        private UnityEvent m_OnSubmit = new UnityEvent();
        /// <summary>
        /// 提交事件
        /// </summary>
        public UnityEvent OnSubmit
        {
            get { return m_OnSubmit; }
            set { m_OnSubmit = value; }
        }

        private UnityEvent m_OnCancel = new UnityEvent();
        /// <summary>
        /// 取消事件
        /// </summary>
        public UnityEvent OnCancel
        {
            get { return m_OnCancel; }
            set { m_OnCancel = value; }
        }


        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        /// <summary>
        /// 取消按钮
        /// </summary>
        private Button buttonCancel;

        void Awake()
        {
            buttonSubmit = transform.Find("ButtonSubmit").GetComponent<Button>();
            buttonCancel = transform.Find("ButtonCancel").GetComponent<Button>();
            //事件
            buttonSubmit.onClick.AddListener(() => OnSubmit.Invoke());
            buttonCancel.onClick.AddListener(() => OnCancel.Invoke());
        }
    }
}
