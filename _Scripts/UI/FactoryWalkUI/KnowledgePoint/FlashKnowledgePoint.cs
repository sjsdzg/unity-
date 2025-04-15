using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 高亮闪烁知识点
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class FlashKnowledgePoint : BestViewKnowledgePoint
    {
        /// <summary>
        /// 高亮提示组件
        /// </summary>
        private Highlighter h;

        protected override void OnAwake()
        {
            base.OnAwake();
            h = transform.GetComponent<Highlighter>();
        }

        public override void Display()
        {
            base.Display();
            gameObject.SetActive(true);
            h.FlashingOn();
        }

        public override void Close()
        {
            h.FlashingOff();
            gameObject.SetActive(false);
        }

    }
}
