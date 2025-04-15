using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 日志栏目
    /// </summary>
    public class LogBar : MonoBehaviour
    {
        /// <summary>
        /// 操作日志数据
        /// </summary>
        private List<LogItemData> m_LogDatas;

        /// <summary>
        /// 动作组件列表
        /// </summary>
        private List<LogItem> m_ItemComponets;

        /// <summary>
        /// 默认项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 内容
        /// </summary>
        private RectTransform Content;

        void Awake()
        {
            m_ItemComponets = new List<LogItem>();
            m_LogDatas = new List<LogItemData>();
            Content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
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
        public void AddLogItem(LogItemData data)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            LogItem component = obj.GetComponent<LogItem>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(data);
                m_ItemComponets.Add(component);
                m_LogDatas.Add(data);
            }
        }

        /// <summary>
        /// 增加日志Item
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public void AddLogItem(string log, LogType type)
        {
            LogItemData data = new LogItemData(log, "", type);
            AddLogItem(data);
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < m_ItemComponets.Count; i++)
            {
                LogItem item = m_ItemComponets[i];
                Destroy(item.gameObject);
            }

            m_ItemComponets.Clear();
            m_LogDatas.Clear();
        }


        public void Show()
        {
            if (!gameObject.activeSelf)
            {
                transform.DOScale(0, 0.3f).From();
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
