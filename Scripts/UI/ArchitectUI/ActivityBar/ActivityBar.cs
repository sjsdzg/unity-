using UnityEngine;
using System.Collections;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class ActivityBar : MonoBehaviour
    {
        private UniEvent< ActivityItem> m_OnSelectd = new UniEvent<ActivityItem>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<ActivityItem> OnSelectd
        {
            get { return m_OnSelectd; }
            set { m_OnSelectd = value; }
        }

        /// <summary>
        /// Items
        /// </summary>
        private ActivityItem[] activityItems;


        private ActivityItem m_SelectedItem;
        /// <summary>
        /// 选中项
        /// </summary>
        public ActivityItem SelectedItem
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
            activityItems = GetComponentsInChildren<ActivityItem>();
            int length = activityItems.Length;
            for (int i = 0; i < length; i++)
            {
                var activityItem = activityItems[i];
                activityItem.OnSelectd.AddListener(activityItem_OnValueChanged);

            }

            if (length > 0)
            {
                SelectedItem = activityItems[0];
            }
        }

        private void activityItem_OnValueChanged(ActivityItem arg0)
        {
            SelectedItem = arg0;
        }
    }
}

