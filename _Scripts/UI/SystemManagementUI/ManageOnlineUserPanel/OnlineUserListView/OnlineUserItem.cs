using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 在线用户Item
    /// </summary>
    public class OnlineUserItem : ListViewItemBase<OnlineUserItemData>
    {
        private UniEvent<OnlineUserItem> m_OnClosed = new UniEvent<OnlineUserItem>();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public UniEvent<OnlineUserItem> OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        /// <summary>
        /// 用户图标
        /// </summary>
        public Image m_UserIcon;

        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        protected override void Awake()
        {
            base.Awake();
            buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        public override void SetData(OnlineUserItemData data)
        {
            base.SetData(data);
            base.SetData(data);
            m_UserIcon.sprite = data.UserIcon;
            m_Text.text = data.UserProfile.RealName;
        }

        private void buttonClose_onClick()
        {
            OnClosed.Invoke(this);
        }
    }
}
