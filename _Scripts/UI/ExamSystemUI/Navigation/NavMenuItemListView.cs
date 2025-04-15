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
    /// 导航栏菜单项列表视图
    /// </summary>
    public class NavMenuItemListView : MonoBehaviour
    {
        public class OnSelectedEvent : UnityEvent<NavgationMenuItem> { }

        private OnSelectedEvent m_OnSelected = new OnSelectedEvent();
        /// <summary>
        /// 当选中时，触发
        /// </summary>
        public OnSelectedEvent OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 动作组件列表
        /// </summary>
        public List<NavgationMenuItem> componets;

        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        void Awake()
        {
            componets = new List<NavgationMenuItem>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加动作Item
        /// </summary>
        /// <param name="menuItemData"></param>
        public void AddMenuItem(NavMenuItem menuItemData,ToggleGroup toggleGroup, Sprite sprite)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            NavgationMenuItem component = obj.GetComponent<NavgationMenuItem>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                obj.GetComponent<Toggle>().group = toggleGroup;
                component.SetValue(this, menuItemData, sprite);
                componets.Add(component);

                component.OnSelected.AddListener(component_OnSelected);

            }
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        public void SetSizeDelta()
        {
            float height = 0;
            for (int i = 0; i < componets.Count; i++)
            {
                NavgationMenuItem conponent = componets[i];
                height += conponent.GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < componets.Count; i++)
            {
                NavgationMenuItem item = componets[i];
                Destroy(item.gameObject);
            }
            componets.Clear();
        }

        /// <summary>
        /// MenuItem点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnSelected(NavgationMenuItem menuItem)
        {
            OnSelected.Invoke(menuItem);
        }
    }
}
