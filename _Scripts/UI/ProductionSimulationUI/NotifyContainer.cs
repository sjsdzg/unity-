using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Module;
using System;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 通知管理器
    /// </summary>
    public class NotifyContainer : MonoSingleton<NotifyContainer>
    {
        public class OnClickedEvent : UnityEvent<KnowledgePoint> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 动作组件列表
        /// </summary>
        private List<KnowledgePointNotify> m_Componets;

        protected override void Init()
        {
            base.Init();
            m_Componets = new List<KnowledgePointNotify>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type LogItem to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 增加日志Item
        /// </summary>
        /// <param name="data"></param>
        public void AddItem(KnowledgePoint knowledgePoint)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            KnowledgePointNotify component = obj.GetComponent<KnowledgePointNotify>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.Show(knowledgePoint);
                m_Componets.Add(component);
                component.OnClicked.AddListener(component_OnClicked);
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Componets.Count; i++)
            {
                KnowledgePointNotify item = m_Componets[i];
                Destroy(item.gameObject);
            }
            //清空
            m_Componets.Clear();
        }

        /// <summary>
        /// 组件点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClicked(KnowledgePoint arg0)
        {
            OnClicked.Invoke(arg0);
        }
    }
}

