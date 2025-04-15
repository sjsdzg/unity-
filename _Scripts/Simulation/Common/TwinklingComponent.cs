using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 高亮组件（通用）
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class TwinklingComponent : ComponentBase, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool isHighlighted = true;
        /// <summary>
        /// 是否高亮
        /// </summary>
        public bool IsHighlighted
        {
            get { return IsHighlighted; }
            set { isHighlighted = value; }
        }
        [SerializeField]
        private Color defaultColor = Color.cyan;
        [SerializeField]
        public bool isTwinkling = false;
        /// <summary>
        /// 是否闪亮
        /// </summary>
        //public bool IsTwinkling
        //{
        //    get { return isTwinkling; }
        //    set { 
        //        ShowTwinkling(isTwinkling);
        //    }
        //}

        /// <summary>
        /// 高亮提示组件
        /// </summary>
        protected Highlighter h;

        void Awake()
        {
            h = transform.GetComponent<Highlighter>();
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        void Start()
        {
            h = transform.GetComponent<Highlighter>();
            OnStart();
        }

        public virtual void OnStart()
        {

        }

        /// <summary>
        /// 指针进入
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (isHighlighted)
            {
                h.ConstantOn(Color.cyan);
            }
        }

        /// <summary>
        /// 指针退出
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            h.ConstantOff();
        }

        /// <summary>
        /// 显示闪亮
        /// </summary>
        public void ShowTwinkling(bool isOn)
        {
            ///2017年7月8日11:29:35 张玉良  添加
            StopCoroutine(Highter(isOn));
            StartCoroutine(Highter(isOn));
        }
        IEnumerator Highter(bool isOn)
        {
            while (isTwinkling)
            {
                h.ConstantOn(defaultColor);
                yield return new WaitForSeconds(0.3f);
                h.ConstantOff();
                yield return new WaitForSeconds(0.5f);                
            }
        }
    }
}
