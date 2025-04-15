using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 高亮知识点
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class HighlighterKnowledgePoint : BaseKnowledgePoint, IPointerEnterHandler, IPointerExitHandler
    {
        public override KnowledgePointType GetKnowledgePointType()
        {
            throw new NotImplementedException();
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

        public override void Display()
        {
            
        }

        public override void Close()
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
                h.ConstantOn();
            }
        }

        /// <summary>
        /// 指针退出
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (isHighlighted)
            {
                h.ConstantOff();
            }
        }
    }
}
