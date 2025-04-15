using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using XFramework.Module;

namespace XFramework.UI
{
    public class ValidationContentComponent : MonoBehaviour
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
        public List<ValidationItemComponent> m_Components { get; private set; }

        /// <summary>
        /// 工具点击事件类
        /// </summary>
        public class ItemClickEvent : UnityEvent<ValidationItemComponent> { }

        private ItemClickEvent m_ItemOnClicked = new ItemClickEvent();
        /// <summary>
        /// 工具点击触发
        /// </summary>
        public ItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        private void Awake()
        {
            m_Components = new List<ValidationItemComponent>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <param name="data"></param>
        public void AddItem(ValidationItem data)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ValidationItemComponent component = obj.GetComponent<ValidationItemComponent>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(this, data);
                m_Components.Add(component);

                component.OnClick.AddListener(Item_onClick);
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
                ValidationItemComponent component = m_Components[i];
                if (component.data.Name == name)
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
                ValidationItemComponent item = m_Components[i];
                Destroy(item.gameObject);
            }
            m_Components.Clear();
        }

        /// <summary>
        /// 工具点击时，触发事件
        /// </summary>
        private void Item_onClick(ValidationItemComponent arg0)
        {
            m_ItemOnClicked.Invoke(arg0);
        }
    }
}

