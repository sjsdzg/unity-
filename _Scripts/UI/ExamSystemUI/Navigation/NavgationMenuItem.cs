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
    /// 导航栏菜单项目
    /// </summary>
    public class NavgationMenuItem : MonoBehaviour
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
        /// 选中状态
        /// </summary>
        public Toggle toggle;
        /// <summary>
        /// 图标
        /// </summary>
        public Image image;
        /// <summary>
        /// 文本
        /// </summary>
        public Text text;
        /// <summary>
        /// 动作视图
        /// </summary>
        public NavMenuItemListView Parent { get; private set; }
        /// <summary>
        /// 动作数据
        /// </summary>
        public NavMenuItem Data { get; private set; }

        void Awake()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(toggle_onValueChanged);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(NavMenuItemListView listView, NavMenuItem _data, Sprite sprite)
        {
            Data = _data;
            Parent = listView;

            if (image != null)
            {
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }

            if (text != null)
            {
                text.text = _data.Name;
            }
        }

        /// <summary>
        /// toggle改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void toggle_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(this);
            }
        }

    }
}
