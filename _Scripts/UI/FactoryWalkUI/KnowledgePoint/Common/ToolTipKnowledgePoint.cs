using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class ToolTipKnowledgePoint : HighlighterKnowledgePoint
    {
        [SerializeField]
        private bool isToolTip = true;
        /// <summary>
        /// 是否显示提示信息
        /// </summary>
        public bool IsToolTip
        {
            get { return isToolTip; }
            set { isToolTip = value; }
        }

        [SerializeField]
        private string m_CatchToolTip;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string CatchToolTip
        {
            get { return m_CatchToolTip; }
            set { m_CatchToolTip = value; }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (IsToolTip)
            {
                ToolTip.Instance.Show(true, 0.4f, CatchToolTip);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (IsToolTip)
            {
                ToolTip.Instance.Show(false);
            }
        }
    }
}
