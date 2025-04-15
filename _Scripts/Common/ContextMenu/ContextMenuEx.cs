using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Core;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 右键菜单
    /// </summary>
    public class ContextMenuEx : MonoSingleton<ContextMenuEx>
    {
        public class OnCloseEvent : UnityEvent<ContextMenuHideReason> { }//关闭事件类

        private OnCloseEvent m_OnClose = new OnCloseEvent();
        /// <summary>
        /// 关闭右键菜单事件
        /// </summary>
        public OnCloseEvent OnClose
        {
            get { return m_OnClose; }
            set { m_OnClose = value; }
        }

        /// <summary>
        /// 右键菜单项列表
        /// </summary>
        protected List<ContextMenuItem> MenuItems { get; private set; }

        /// <summary>
        /// 右键菜单源
        /// </summary>
        public GameObject Source { get; private set; }

        /// <summary>
        /// 指针是否进入
        /// </summary>
        private bool isPointerEnter = false;

        /// <summary>
        /// 视图
        /// </summary>
        private RectTransform m_RectTransform;

        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 偏移
        /// </summary>
        [SerializeField]
        public Vector3 offset = new Vector3(0, 0, 0);

        protected override void Init()
        {
            m_RectTransform = transform as RectTransform;
            m_RectTransform.anchorMin = Vector2.zero;
            m_RectTransform.anchorMax = Vector2.zero;
            //DefaultItem
            MenuItems = new List<ContextMenuItem>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="position"></param>
        /// <param name="parameters"></param>
        public void Show(GameObject element, Vector2 position, List<ContextMenuParameter> parameters)
        {
            Clear();
            //动态显示
            m_RectTransform.localScale = new Vector3(1, 0);
            m_RectTransform.DOScaleY(1, 0.2f);

            gameObject.SetActive(true);
            Source = element;
            SetPosition(position);

            foreach (var parameter in parameters)
            {
                AddMenuItem(parameter);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="position"></param>
        /// <param name="parameters"></param>
        public void Show(GameObject element, List<ContextMenuParameter> parameters)
        {
            Clear();
            //动态显示
            m_RectTransform.localScale = new Vector3(1, 0);
            m_RectTransform.DOScaleY(1, 0.2f);

            gameObject.SetActive(true);
            Source = element;
            SetPosition(Input.mousePosition);

            foreach (var parameter in parameters)
            {
                AddMenuItem(parameter);
            }
        }

        public void Show(GameObject element, string[] texts, UnityAction[] actions)
        {
            if (texts.Length == actions.Length)
            {
                List<ContextMenuParameter> parameters = new List<ContextMenuParameter>();
                for (int i = 0; i < texts.Length; i++)
                {
                    UnityAction action = actions[i];
                    ContextMenuParameter parameter = new ContextMenuParameter(texts[i], x => action.Invoke());
                    parameters.Add(parameter);
                }
                Show(element, parameters);
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            Clear();
            gameObject.SetActive(false);
            OnClose.Invoke(ContextMenuHideReason.CloseCalled);
        }

        /// <summary>
        /// 添加右键菜单项
        /// </summary>
        /// <param name="parameter"></param>
        private void AddMenuItem(ContextMenuParameter parameter)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ContextMenuItem menuItem = obj.GetComponent<ContextMenuItem>();

            if (menuItem != null && m_RectTransform != null)
            {
                Transform t = menuItem.transform;
                t.SetParent(m_RectTransform, false);
                obj.layer = m_RectTransform.gameObject.layer;

                menuItem.SetValue(this, parameter);
                MenuItems.Add(menuItem);
                //设置参数属性
                parameter.ID = MenuItems.Count;

                menuItem.onClick.AddListener(menuItem_onClick);
            }
        }

        /// <summary>
        /// 移除所有项
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                ContextMenuItem item = MenuItems[i];
                Destroy(item.gameObject);
            }

            MenuItems.Clear();
        }

        /// <summary>
        /// 右键菜单项点击触发
        /// </summary>
        private void menuItem_onClick()
        {
            Hide();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject pointerEnterGameObject = Utils.CurrentPointerEnterGameObject();
                if (pointerEnterGameObject!=null)
                {
                    if ((pointerEnterGameObject.name != name) && !pointerEnterGameObject.transform.IsChildOf(transform))
                    {
                        Hide();
                    }
                }
                
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="mousePosition"></param>
        protected virtual void SetPosition(Vector3 mousePosition)
        {
            Vector3 pos = mousePosition + offset;
            var width = m_RectTransform.rect.width;
            var height = m_RectTransform.rect.height;

            float x = 0;
            float y = 0;
            //x轴位置
            if (pos.x > Screen.width - width * (1 - m_RectTransform.pivot.x))
            {                                                                                                                  
                x = Screen.width - width * (1 - m_RectTransform.pivot.x);
            }
            else if (pos.x < width * m_RectTransform.pivot.x)
            {
                x = width * m_RectTransform.pivot.x;
            }
            else
            {
                x = pos.x;
            }
            //y轴位置
            if (pos.y > Screen.height - height * (1 - m_RectTransform.pivot.x))
            {
                y = Screen.height - height * (1 - m_RectTransform.pivot.x);
            }
            else if (pos.y < height * m_RectTransform.pivot.y)
            {
                y = height * m_RectTransform.pivot.y;
            }
            else
            {
                y = pos.y;
            }
            //设置
            m_RectTransform.anchoredPosition = new Vector2(x, y);
        }
    }
}
