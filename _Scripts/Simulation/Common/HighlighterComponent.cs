using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using XFramework.Simulation.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

namespace XFramework.Simulation.Component
{
    /// <summary>
    /// 高亮组件（通用）
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class HighlighterComponent : BaseComponent, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 组件类型
        /// </summary>
        /// <returns></returns>
        public override int GetComponentType()
        {
            return ElementType.HighlighterComponent;
        }

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

       void OnMouseEnter()
        {
            if (isHighlighted)
            {
                h.ConstantOn(Color.cyan);
            }
        }

        void OnMouseExit()
        {
            h.ConstantOff();
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
        /// 短暂的显示高亮
        /// </summary>
        public void ShowHighlighterShort(int count)
        {
            StartCoroutine(Highter(count));
        }
        IEnumerator Highter(int count)
        {
            for (int i = 0; i < count; i++)
            {
                h.ConstantOn(Color.cyan);
                yield return new WaitForSeconds(0.3f);
                h.ConstantOff();
                yield return new WaitForSeconds(0.5f);

            }

        }

        /// <summary>
        /// 开始闪亮
        /// </summary>
        public void FlashingOn()
        {
            Debug.Log("flashing : " + name);
            h.FlashingOn(Color.yellow, Color.white, 3);
        }

        /// <summary>
        /// 关闭闪亮
        /// </summary>
        public void FlashingOff()
        {
            h.FlashingOff();
        }
    }
}
