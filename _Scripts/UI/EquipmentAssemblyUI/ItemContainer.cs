using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XFramework.UI
{
    /// <summary>
    /// Item容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemContainer<TElement, TItem> : Element where TElement : Element
    {
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
        public List<TElement> m_Elements { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            m_Elements = new List<TElement>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddItem(TItem item)
        {
            try
            {
                GameObject obj = Instantiate(DefaultItem);
                obj.SetActive(true);

                TElement component = obj.GetComponent<TElement>();

                if (Content != null && component != null)
                {
                    component.transform.SetParent(Content, false);
                    obj.layer = Content.gameObject.layer;
                    m_Elements.Add(component);
                    component.Data = item;
                    SetData(component, item);
                }
            }
            catch(Exception ex)
            {
                print(ex.ToString());
                print("实例化item 失败："+ item+"  "+gameObject.name);
            }
        }

        /// <summary>
        /// Sets component data with specified item.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="item">Item.</param>
        protected virtual void SetData(TElement element, TItem item)
        {

        }

        /// <summary>
        /// 移除部件Item
        /// </summary>
        public void RemoveItem(TItem item)
        {
            for (int i = 0; i < m_Elements.Count; i++)
            {
                TElement component = m_Elements[i];

                if (component.Data.Equals(item))
                {
                    m_Elements.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// 清空部件Item
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Elements.Count; i++)
            {
                TElement item = m_Elements[i];
                Destroy(item.gameObject);
            }

            m_Elements.Clear();
        }
    }
}

