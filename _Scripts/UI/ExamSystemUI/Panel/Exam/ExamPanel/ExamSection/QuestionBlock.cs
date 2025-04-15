using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    public class QuestionBlock : MonoBehaviour
    {
        public class OnCompletedEvent : UnityEvent<bool> { }

        private OnCompletedEvent m_OnCompleted = new OnCompletedEvent();
        /// <summary>
        /// 题目完成时，触发
        /// </summary>
        public OnCompletedEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        private UnityEvent m_OnEnter = new UnityEvent();
        /// <summary>
        /// 进入操作场景事件
        /// </summary>
        public UnityEvent OnEnter
        {
            get { return m_OnEnter; }
            set { m_OnEnter = value; }
        }

        /// <summary>
        /// 题型
        /// </summary>
        public virtual int QType
        {
            get { return 0; }
        }

        void Awake()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetParams(object value)
        {

        }

        /// <summary>
        /// 获取答案
        /// </summary>
        /// <returns></returns>
        public virtual string GetKey()
        {
            return "";
        }

        /// <summary>
        /// 设置答案
        /// </summary>
        /// <returns></returns>
        public virtual void SetKey(string _key)
        {

        }
    }
}
