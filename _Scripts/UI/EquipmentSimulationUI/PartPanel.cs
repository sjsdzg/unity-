using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 部件面板
    /// </summary>
    public class PartPanel : MonoBehaviour
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
        public List<PartItemComponent> m_Components { get; private set; }

        /// <summary>
        /// 工具点击事件类
        /// </summary>
        public class PartItemClickEvent : UnityEvent<PartItemComponent> { }

        private PartItemClickEvent m_ItemOnClicked = new PartItemClickEvent();
        /// <summary>
        /// 部件Item点击触发
        /// </summary>
        public PartItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        /// <summary>
        /// 选中的部件组件
        /// </summary>
        public PartItemComponent SelectedItem { get; private set; }

        void Awake()
        {
            m_Components = new List<PartItemComponent>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddPartItem(EquipmentPart info)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            PartItemComponent component = obj.GetComponent<PartItemComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(info);
                m_Components.Add(component);

                component.OnClick.AddListener(Component_OnClick);
            }
        }

        /// <summary>
        /// 移除部件Item
        /// </summary>
        public void RemovePartItem(string name)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                PartItemComponent component = m_Components[i];

                if (component.data.Name.Equals(name))
                {
                    m_Components.Remove(component);
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
            for (int i = 0; i < m_Components.Count; i++)
            {
                PartItemComponent item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 部件Item点击时，触发。
        /// </summary>
        /// <param name="item"></param>
        private void Component_OnClick(PartItemComponent item)
        {
            m_ItemOnClicked.Invoke(item);
        }
    }
}
