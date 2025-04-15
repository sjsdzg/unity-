using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using XFramework.Module;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ProductionItemContent : MonoBehaviour
    {
        public class OnClickEvent : UnityEvent<ProductionItemElement> { }

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;
        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 工具项组件列表
        /// </summary>
        public List<ProductionItemElement> m_Components { get; private set; }

        private OnClickEvent m_ItemOnClicked = new OnClickEvent();
        /// <summary>
        /// Item点击时，触发
        /// </summary>
        public OnClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        private ItemType m_CurrentItemType = ItemType.Clean;
        /// <summary>
        /// 工具分类
        /// </summary>
        public ItemType CurrentItemType
        {
            get { return m_CurrentItemType ; }
            set
            {
                m_CurrentItemType  = value;
                ChangeItemType(m_CurrentItemType);
            }
        }

        void Awake()
        {
            m_Components = new List<ProductionItemElement>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ProductionItemElement component = obj.GetComponent<ProductionItemElement>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(item);
                m_Components.Add(component);

                if (component.Item.Type == CurrentItemType)
                {
                    component.gameObject.SetActive(true);
                }
                else
                {
                    component.gameObject.SetActive(false);
                }

                component.OnClick.AddListener(component_OnClick);
            }
        }

        /// <summary>
        /// 移除工具
        /// </summary>
        /// <param name="type">ToolType</param>
        public void RemoveItem(string name)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                ProductionItemElement component = m_Components[i];
                if (component.Item.Name == name)
                {
                    m_Components.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                ProductionItemElement item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 组件点击时，触发。
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClick(ProductionItemElement element)
        {
            ItemOnClicked.Invoke(element);
        }

        private void ChangeItemType(ItemType itemType)
        {
            foreach (var component in m_Components)
            {
                if (component.Item.Type == CurrentItemType)
                {
                    component.gameObject.SetActive(true);
                }
                else
                {
                    component.gameObject.SetActive(false);
                }
            }
        }
    }
}

