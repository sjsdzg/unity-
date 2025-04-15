using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 在线用户Item
    /// </summary>
    public class NetworkFileItem : ListViewItemBase<NetworkFileItemData>
    {
        private UniEvent<NetworkFileItem> m_OnSubmitEvent = new UniEvent<NetworkFileItem>();
        /// <summary>
        /// 提交事件
        /// </summary>
        public UniEvent<NetworkFileItem> OnSubmitEvent
        {
            get { return m_OnSubmitEvent ; }
            set { m_OnSubmitEvent = value; }
        }

        /// <summary>
        /// 用户图标
        /// </summary>
        public Image m_Icon;

        /// <summary>
        /// 锁定图标
        /// </summary>
        public GameObject m_Lock;

        /// <summary>
        /// 输入框
        /// </summary>
        public UIInputField m_InputField;

        /// <summary>
        /// 列表
        /// </summary>
        public NetworkFileListView ListView { get; set; }

        private bool isEdit;
        /// <summary>
        /// 是否编辑
        /// </summary>
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                if (isEdit)
                {
                    m_InputField.interactable = true;
                    m_InputField.GetComponent<Image>().enabled = true;
                    m_InputField.ActivateInputField();
                }
                else
                {
                    m_InputField.interactable = false;
                    m_InputField.GetComponent<Image>().enabled = false;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_Icon = transform.Find("Icon").GetComponent<Image>();
            m_InputField = transform.Find("InputField").GetComponent<UIInputField>();
            m_Lock = transform.Find("Lock").gameObject;
            m_Lock.SetActive(false);

            m_InputField.onEndEdit.AddListener(m_InputField_onEndEdit);
            m_InputField.OnClicked.AddListener(m_InputField_OnClicked);
        }

        public override void SetData(NetworkFileItemData data)
        {
            base.SetData(data);
            m_Icon.sprite = data.FileIcon;
            m_InputField.text = data.NetworkFile.Name;
            IsEdit = data.IsEdit;
            if (data.NetworkFile.Status == 1)
            {
                m_Lock.SetActive(true);
            }
            else
            {
                m_Lock.SetActive(false);
            }
        }

        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="name"></param>
        private void m_InputField_onEndEdit(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                m_InputField.text = this.Data.NetworkFile.Name;
            }
            else
            {
                if (!this.Data.NetworkFile.Name.Equals(name))
                {
                    string suffix = Path.GetExtension(name);
                    if (this.Data.NetworkFile.Type.Equals("dir"))
                    {
                        //
                    }
                    else if (string.IsNullOrEmpty(suffix) || suffix.Equals(".dir"))
                    {
                        this.Data.NetworkFile.Type = "unknown";
                    }
                    else
                    {
                        this.Data.NetworkFile.Type = suffix.Trim('.').ToLower();
                    }

                    this.Data.NetworkFile.Name = name;
                    OnSubmitEvent.Invoke(this);
                }
            }
            // 等待0.1f
            this.Invoke(0.1f, () => IsEdit = false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData"></param>
        private void m_InputField_OnClicked(PointerEventData eventData)
        {
            if (!ListView.MultiSelect && IsSelected && !m_InputField.isFocused 
                && eventData.button == PointerEventData.InputButton.Left
                && eventData.clickCount == 1 && Data.NetworkFile.Status != 1)
            {
                IsEdit = true;
            }

            OnPointerClick(eventData);
        }

        public override void OnReset()
        {
            base.OnReset();
            m_OnSubmitEvent.RemoveAllListeners();
        }
    }
}
