using UnityEngine;
using System.Collections;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class Selector : MonoBehaviour
    {
        private UniEvent<SelectorItem> m_OnSelectd = new UniEvent<SelectorItem>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<SelectorItem> OnSelectd
        {
            get { return m_OnSelectd; }
            set { m_OnSelectd = value; }
        }

        /// <summary>
        /// Items
        /// </summary>
        private SelectorItem[] m_Items;


        private SelectorItem m_SelectedItem;
        /// <summary>
        /// 选中项
        /// </summary>
        public SelectorItem SelectedItem
        {
            get { return m_SelectedItem; }
            set 
            {
                if (m_SelectedItem == value)
                    return;

                if (m_SelectedItem != null)
                {
                    m_SelectedItem.DoDeselect();
                }

                m_SelectedItem = value;

                if (m_SelectedItem != null)
                {
                    m_SelectedItem.DoSelect();
                }

                // 选中事件
                OnSelectd.Invoke(m_SelectedItem);
            }
        }

        private void Start()
        {
            m_Items = GetComponentsInChildren<SelectorItem>();
            int length = m_Items.Length;
            for (int i = 0; i < length; i++)
            {
                var activityItem = m_Items[i];
                activityItem.OnSelectd.AddListener(activityItem_OnValueChanged);
            }

            if (m_Items.Length > 0)
            {
                SelectedItem = m_Items[0];
            }
        }

        private void activityItem_OnValueChanged(SelectorItem arg0)
        {
            SelectedItem = arg0;
        }
    }
}

