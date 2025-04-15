using XFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.UI;

namespace XFramework.Component
{
    /// <summary>
    /// 右键菜单组件
    /// </summary>
    public class ContextMenuComponent : ToolTipComponent, IPointerClickHandler
    {
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

        public override void OnAwake()
        {
            base.OnAwake();
            InitContextMenu();
            if (m_Parameters.Count<=0)
            {
                ContextMenuEnabled = false;
            }
        }

        /// <summary>
        /// 初始化右键菜单
        /// </summary>
        public virtual void InitContextMenu()
        {
           
        }
        /// <summary>
        /// 是否激活鼠标功能
        /// </summary>
        /// <param name="bo"></param>
        public virtual void ActiveMouseAbility(bool bo)
        {
            if(m_Parameters.Count>0)
            {
                ContextMenuEnabled = bo;
            }
            IsToolTip = bo;
            IsHighlighted = bo;
            Quit();
        }
        /// <summary>
        /// 指针点击
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (ContextMenuEnabled && eventData.button == PointerEventData.InputButton.Left)
            {
                ContextMenuEx.Instance.Show(gameObject, Input.mousePosition, m_Parameters);
            }
        }

        /// <summary>
        /// 设置某一项参数是否启用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="able"></param>
        public void SetEnabled(int id, bool able)
        {

            if(m_Parameters.Count>0)
              m_Parameters.Find(x => x.ID == id).Enabled = able;

        }
        /// <summary>
        /// 指针退出
        /// </summary>
        private void Quit()
        {
            try
            {
                if (ToolTip.Instance != null)
                    ToolTip.Instance.Show(false);
                h.ConstantOff();
            }
            catch {
                Debug.LogError(" name:"+name+"  Error");
            }
            
        }
    }
}
