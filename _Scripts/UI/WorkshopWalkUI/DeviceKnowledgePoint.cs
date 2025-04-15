using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using HighlightingSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 设备知识点
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class DeviceKnowledgePoint : BaseKnowledgePoint, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

        [SerializeField]
        private bool contextMenuEnabled = true;
        /// <summary>
        /// 右键菜单是否启用
        /// </summary>
        public bool ContextMenuEnabled
        {
            get { return contextMenuEnabled; }
            set { contextMenuEnabled = value; }
        }

        private List<ContextMenuParameter> m_Parameters = new List<ContextMenuParameter>();
        /// <summary>
        /// 右键菜单参数列表
        /// </summary>
        public List<ContextMenuParameter> Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        /// <summary>
        /// 高亮提示组件
        /// </summary>
        protected Highlighter h;

        void Awake()
        {
            h = transform.GetComponent<Highlighter>();
        }


        void Start()
        {
            if (h == null)
            {
                h = transform.GetComponent<Highlighter>();
            }
            InitContextMenu();
        }

        /// <summary>
        /// 初始化右键菜单
        /// </summary>
        public virtual void InitContextMenu()
        {
            Parameters.Add(new ContextMenuParameter(1, "查看", "", true, ViewCallBack));
        }

        /// <summary>
        /// 指针点击
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (ContextMenuEnabled && eventData.button == PointerEventData.InputButton.Right)
            {
                OnShowContextMenu();
            }
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        public virtual void OnShowContextMenu()
        {
            ContextMenuEx.Instance.Show(gameObject, Input.mousePosition, m_Parameters);
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

            if (IsToolTip)
            {
                ToolTip.Instance.Show(true, 0.4f, CatchToolTip);
            }
        }

        /// <summary>
        /// 指针退出
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            h.ConstantOff();
            ToolTip.Instance.Show(false);
        }

        /// <summary>
        /// 查看回调
        /// </summary>
        /// <param name="arg0"></param>
        private void ViewCallBack(ContextMenuParameter arg0)
        {
            Display();
        }

        public override void Display()
        {
            Messager.Instance.SendMessage("DeviceKnowledgePoint", this, m_CatchToolTip);
        }

        public override void Close()
        {
            
        }

    }
}
