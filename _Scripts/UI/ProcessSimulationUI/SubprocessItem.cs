using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 子工艺Item
    /// </summary>
    public class SubprocessItem : MonoBehaviour
    {
        /// <summary>
        /// 子工艺序号
        /// </summary>
        private Text Number;

        /// <summary>
        /// 子工艺内容
        /// </summary>
        private Text Content;

        /// <summary>
        /// 箭头
        /// </summary>
        private Transform Arrow;

        /// <summary>
        /// 背景Rect
        /// </summary>
        private Transform m_Rect;

        /// <summary>
        /// 子工艺信息
        /// </summary>
        public SubprocessInfo Data { get; private set; }

        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<SubprocessItem> { }

        private ClickEvent m_OnClick = new ClickEvent();
        /// <summary>
        /// 子工艺点击事件
        /// </summary>
        public ClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private UnityEvent m_OnNext = new UnityEvent();
        /// <summary>
        /// 下一个展现事件
        /// </summary>
        public UnityEvent OnNext
        {
            get { return m_OnNext; }
            set { m_OnNext = value; }
        }

        void Awake()
        {
            m_Rect = transform.Find("Background");
            Number = transform.Find("Background/Number").GetComponent<Text>();
            Content = transform.Find("Background/Content").GetComponent<Text>();
            Arrow = transform.Find("Arrow");

            //注册点击事件
            transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick.Invoke(this);
            });
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="number"></param>
        /// <param name="content"></param>
        public void SetValue(SubprocessInfo info, bool arrowActive = true)
        {
            Data = info;
            SetValue(info.Number.ToString(), info.Name, arrowActive);
        }

        /// <summary>
        /// 设置内容(重载)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="content"></param>
        /// <param name="arrowActive"></param>
        private void SetValue(string number, string content, bool arrowActive = true)
        {
            Number.text = number;
            Content.text = content;
            Arrow.gameObject.SetActive(arrowActive);
            PlayEffect(content, arrowActive);
        }

        /// <summary>
        /// 动态效果
        /// </summary>
        private void PlayEffect(string content, bool arrowActive)
        {
            m_Rect.DORotate(new Vector3(90, 0, 0), 0.2f).From();
            if (arrowActive)
            {
                Arrow.DOScaleY(0, 0.5f).From().OnComplete(() =>
                {
                    OnNext.Invoke();
                });
            }
        }
    }
}
