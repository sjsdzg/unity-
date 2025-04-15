using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    public class NetworkFileListView : ListViewCustom<NetworkFileItem, NetworkFileItemData>
    {
        private UniEvent<NetworkFileItemData> m_DoubleItemClick = new UniEvent<NetworkFileItemData>();
        /// <summary>
        /// 双击事件
        /// </summary>
        public UniEvent<NetworkFileItemData> DoubleItemClick
        {
            get { return m_DoubleItemClick; }
            set { m_DoubleItemClick = value; }
        }

        private UniEvent<NetworkFileItem> m_OnItemSubmit = new UniEvent<NetworkFileItem>();
        /// <summary>
        /// 提交事件
        /// </summary>
        public UniEvent<NetworkFileItem> OnItemSubmit
        {
            get { return m_OnItemSubmit; }
            set { m_OnItemSubmit = value; }
        }

        private UniEvent<NetworkFile> m_OnAddressClicked = new UniEvent<NetworkFile>();
        /// <summary>
        /// 地址栏点击事件
        /// </summary>
        public UniEvent<NetworkFile> OnAddressClicked
        {
            get { return m_OnAddressClicked; }
            set { m_OnAddressClicked = value; }
        }

        /// <summary>
        /// 文件地址栏
        /// </summary>
        private AddressPanel m_AddressPanel;

        protected override void Awake()
        {
            base.Awake();
            m_AddressPanel = transform.Find("AddressBar").GetComponent<AddressPanel>();
            m_AddressPanel.OnClicked.AddListener(m_AddressPanel_OnClicked);
        }

        public void AddAddressButton(NetworkFile file)
        {
            m_AddressPanel.AddAddressButton(file, file.Name);
        }

        protected override void SetData(NetworkFileItem item, NetworkFileItemData data)
        {
            base.SetData(item, data);
            item.ListView = this;
            item.DoubleClick.AddListener(item_DoubleClick);
            item.OnSubmitEvent.AddListener(item_OnSubmitEvent);
        }

        private void item_DoubleClick(ListViewItemBase<NetworkFileItemData> arg0)
        {
            DoubleItemClick.Invoke(arg0.Data);
        }

        private void item_OnSubmitEvent(NetworkFileItem arg0)
        {
            OnItemSubmit.Invoke(arg0);
        }

        private void m_AddressPanel_OnClicked(object arg0)
        {
            NetworkFile file = arg0 as NetworkFile;
            OnAddressClicked.Invoke(file);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                MultiSelect = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftShift))
            {
                MultiSelect = false;
            }
        }
    }
}
