using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Simulation.Component;

namespace XFramework.Component
{
    /// <summary>
    /// 提示信息组件
    /// </summary>
    public class ToolTipComponent : HighlighterComponent
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
        public override void OnAwake()
        {
            base.OnAwake();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (IsToolTip && !string.IsNullOrEmpty(CatchToolTip) && ToolTip.Instance != null)
            {
                ToolTip.Instance.Show(true, 0.4f, CatchToolTip);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (IsToolTip && ToolTip.Instance != null)
            {
                ToolTip.Instance.Show(false);
            }
        }
    }
}
