using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 导航组件
    /// </summary>
    public class NavigationBar : Accordion
    {
        public class OnSelectedEvent : UnityEvent<NavgationMenuItem> { }

        private OnSelectedEvent m_MenuItemSelected = new OnSelectedEvent();
        /// <summary>
        /// 当选中时，触发
        /// </summary>
        public OnSelectedEvent MenuItemSelected
        {
            get { return m_MenuItemSelected; }
            set { m_MenuItemSelected = value; }
        }

        [SerializeField]
        public GameObject TogglePrefab;

        [SerializeField]
        public GameObject ContentPrefab;

        /// <summary>
        /// 图标列表
        /// </summary>
        public ImageList imageList;

        /// <summary>
        /// 
        /// </summary>
        public ToggleGroup toggleGroup;

        /// <summary>
        /// 动作试图列表
        /// </summary>
        private List<NavMenuItemListView> menuItemListViews;

        void Awake()
        {
            if (TogglePrefab == null)
            {
                throw new NullReferenceException("TogglePrefab is null.");
            }

            if (ContentPrefab == null)
            {
                throw new NullReferenceException("ContentPrefab is null.");
            }

            TogglePrefab.SetActive(false);
            ContentPrefab.SetActive(false);
            menuItemListViews = new List<NavMenuItemListView>();
            toggleGroup = transform.GetComponent<ToggleGroup>();
        }

        public void Initialize(List<NavMenu> menuDataList)
        {
            StartCoroutine(Loading(menuDataList));
        }

        IEnumerator Loading(List<NavMenu> menuList)
        {
            foreach (var menuData in menuList)
            {
                GameObject toggleObj = Instantiate(TogglePrefab);
                GameObject contentObj = Instantiate(ContentPrefab);
                toggleObj.SetActive(true);
                contentObj.SetActive(true);

                AccordionItem item = new AccordionItem();
                //伸缩组件标题
                NavigationMenu menuComponent = toggleObj.GetComponent<NavigationMenu>();
                if (menuComponent != null)
                {
                    Transform t = menuComponent.transform;
                    t.SetParent(transform, false);
                    toggleObj.layer = gameObject.layer;

                    Sprite sprite = imageList[menuData.Icon];
                    menuComponent.SetValue(sprite, menuData.Name);
                }

                //伸缩组件内容
                NavMenuItemListView menuItemView = contentObj.GetComponent<NavMenuItemListView>();
                if (menuItemView != null)
                {
                    Transform t = menuItemView.transform;
                    t.SetParent(transform, false);
                    contentObj.layer = gameObject.layer;

                    foreach (var menuItemData in menuData.Items)
                    {
                        Sprite sprite = imageList[menuItemData.Icon];
                        menuItemView.AddMenuItem(menuItemData, toggleGroup, sprite);
                    }
                }

                //设置组件内容尺寸
                menuItemListViews.Add(menuItemView);
                menuItemView.OnSelected.AddListener(menuItemView_OnSelected);

                yield return new WaitForEndOfFrame();
                //menuItemView.SetSizeDelta();
                //yield return new WaitForEndOfFrame();
                //组织伸缩部分
                item.ToggleObject = toggleObj;
                item.ContentObject = contentObj;
                //添加导航控件下拉表
                AddCallback(item);
                yield return new WaitForEndOfFrame();
                Open(item);
            }
        }

        /// <summary>
        /// 导航栏菜单项选中触发
        /// </summary>
        /// <param name="arg0"></param>
        private void menuItemView_OnSelected(NavgationMenuItem menuItem)
        {
            MenuItemSelected.Invoke(menuItem);
        }
    }
}
