using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XFramework.Module;

namespace XFramework.UI
{
    public class FaultPhenomenaPanel : MonoBehaviour
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
        /// 正确故障信息
        /// </summary>
        public FaultInfo DefaultFaultInfo { get; set; }

        /// <summary>
        /// 故障现象项组件列表
        /// </summary>
        public List<FaultPhenomenaComponent> m_Components { get; private set; }

        void Awake()
        {
            m_Components = new List<FaultPhenomenaComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加故障现象
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(FaultPhenomena item)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            FaultPhenomenaComponent component = obj.GetComponent<FaultPhenomenaComponent>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                component.SetNumber(m_Components.Count + 1);
                component.SetValue(item);
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
                FaultPhenomenaComponent component = m_Components[i];
                if (component.Data.ID == id)
                {
                    m_Components.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }

            for (int i = 0; i < m_Components.Count; i++)
            {
                FaultPhenomenaComponent component = m_Components[i];
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
                FaultPhenomenaComponent item = m_Components[i];
                Destroy(item.gameObject);
            }

            m_Components.Clear();
        }

        /// <summary>
        /// 故障现象是否匹配
        /// </summary>
        /// <returns></returns>
        public bool IsMatchPhenomena()
        {
            bool isMatch = false;
            if (m_Components.Count == DefaultFaultInfo.FaultPhenomenas.Count)
            {
                int matchCount = 0;
                foreach (var component in m_Components)
                {
                    FaultPhenomena item = DefaultFaultInfo.FaultPhenomenas.Find(x => x.ID == component.Data.ID);
                    if (item != null)
                    {
                        matchCount++;
                    }
                }
                if (matchCount == DefaultFaultInfo.FaultPhenomenas.Count)
                {
                    isMatch = true;
                }
            }

            return isMatch;
        }
    }
}

