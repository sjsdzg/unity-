using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XFramework.Module;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Common;

namespace XFramework.UI
{
    public class FaultCausePanel : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<FaultCausePanel> { }

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
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 故障现象项组件列表
        /// </summary>
        public List<FaultCauseComponent> m_Components { get; private set; }

        /// <summary>
        /// 正确故障信息
        /// </summary>
        public FaultInfo DefaultFaultInfo { get; set; }

        /// <summary>
        /// 选中按钮
        /// </summary>
        private Button selectButton;

        void Awake()
        {
            m_Components = new List<FaultCauseComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
            selectButton = transform.Find("TitleBar/SelectButton").GetComponent<Button>();
            selectButton.onClick.AddListener(selectButton_onClick);
        }

        /// <summary>
        /// 添加故障现象
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(FaultCause item)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            FaultCauseComponent component = obj.GetComponent<FaultCauseComponent>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(item);
                component.SetNumber(m_Components.Count + 1);
                m_Components.Add(component);
            }
        }

        /// <summary>
        /// 移除故障现象
        /// </summary>
        /// <param name="type">ToolType</param>
        public void RemoveItem(string id)
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                FaultCauseComponent component = m_Components[i];
                if (component.Data.ID == id)
                {
                    m_Components.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }

            for (int i = 0; i < m_Components.Count; i++)
            {
                FaultCauseComponent component = m_Components[i];
                component.SetNumber(i + 1);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Components.Count; i++)
            {
                FaultCauseComponent item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 选中按钮点击时，触发
        /// </summary>
        private void selectButton_onClick()
        {
            OnClicked.Invoke(this);
        }

        /// <summary>
        /// 故障原因是否匹配
        /// </summary>
        /// <returns></returns>
        public bool IsMatchCauses()
        {
            bool isMatch = false;
            if (m_Components.Count == DefaultFaultInfo.FaultCauses.Count)
            {
                int matchCount = 0;
                foreach (var component in m_Components)
                {
                    FaultCause item = DefaultFaultInfo.FaultCauses.Find(x => x.ID == component.Data.ID);
                    if (item != null)
                    {
                        matchCount++;
                    }
                }
                if (matchCount == DefaultFaultInfo.FaultCauses.Count)
                {
                    isMatch = true;
                }
            }
            return isMatch;
        }

    }
}

