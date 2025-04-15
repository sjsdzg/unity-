using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using XFramework.Simulation;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class NameGroup : MonoBehaviour
    {
        public class OnSelectedEvent : UnityEvent<string> { }

        private OnSelectedEvent m_OnSelected = new OnSelectedEvent();
        /// <summary>
        /// 选中事件
        /// </summary>
        public OnSelectedEvent OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 组件列表
        /// </summary>
        private List<ProcItemComponent> m_Components { get; set; }

        private void Awake()
        {
            m_Components = new List<ProcItemComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(string name, bool flag = false)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ProcItemComponent component = obj.GetComponent<ProcItemComponent>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                m_Components.Add(component);
                component.OnValueChanged.AddListener(component_OnValueChanged);
                component.SetValue(name, flag);
            }
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="type">ToolType</param>
        public void RemoveItem(string name)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                ProcItemComponent component = m_Components[i];
                if (component.Name == name)
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
                ProcItemComponent item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void component_OnValueChanged(string arg0, bool arg1)
        {
            if (arg1)
            {
                OnSelected.Invoke(arg0);
            }
        }

    }
}

