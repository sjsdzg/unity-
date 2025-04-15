using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.UI
{ 
    public class OnlineUserListView : ListViewCustom<OnlineUserItem, OnlineUserItemData>
    {
        private UniEvent<OnlineUserItem> m_OnItemClosed = new UniEvent<OnlineUserItem>();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public UniEvent<OnlineUserItem> OnItemClosed
        {
            get { return m_OnItemClosed; }
            set { m_OnItemClosed = value; }
        }

        /// <summary>
        /// 在线人数
        /// </summary>
        private Text m_TextOnlineNumber;

        protected override void Awake()
        {
            base.Awake();
            m_TextOnlineNumber = transform.Find("Title/Text").GetComponent<Text>();
        }

        protected override void SetData(OnlineUserItem item, OnlineUserItemData data)
        {
            base.SetData(item, data);
            item.OnClosed.AddListener(OnlineUserItem_OnClosed);
        }

        private void OnlineUserItem_OnClosed(OnlineUserItem arg0)
        {
            Debug.Log("OnlineUserItem_OnClosed");
            OnItemClosed.Invoke(arg0);
        }

        public void SetOnlineNumber(int number)
        {
            m_TextOnlineNumber.text = "在线人数：" + number.ToString();
        }
    }
}
