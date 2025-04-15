using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using XFramework.Module;
using UnityEngine.UI;
using System;

namespace XFramework.UI
{
    public class ProcessParamSettingPanel : MonoBehaviour
    {
        /// <summary>
        /// Container
        /// </summary>
        public RectTransform m_Container;

        /// <summary>
        /// Item
        /// </summary>
        public ProcessParamSettingItem m_DefaultItem;

        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public Button buttonClose;

        /// <summary>
        /// 列表
        /// </summary>
        private List<ProcessParamSettingItem> m_Items = new List<ProcessParamSettingItem>();

        /// <summary>
        /// 对象池池
        /// </summary>
        protected GameObjectPool m_ObjectPool = null;

        private void Awake()
        {
            if (m_DefaultItem == null)
            {
                throw new System.Exception("default item is null!");
            }

            m_DefaultItem.gameObject.SetActive(false);

            //unused
            GameObject unused;
            Transform temp = transform.Find("unused");
            if (temp == null)
                unused = new GameObject("unused");
            else
                unused = temp.gameObject;

            unused.SetActive(false);
            unused.transform.SetParent(transform, false);

            m_ObjectPool = new GameObjectPool(unused.transform, m_DefaultItem.gameObject);

            buttonClose.onClick.AddListener(buttonClose_onClick);

        }

        private void buttonClose_onClick()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
        public void SetData(ProcessLibraryItemData data)
        {
            gameObject.SetActive(true);
            Clear();
            if (data == null)
            {
                m_Text.text = "";
                return;
            }

            m_Text.text = data.Name;

            if (data.Variables != null)
            {
                foreach (var itemData in data.Variables)
                {
                    AddItem(itemData);
                }
            }
        }

        /// <summary>
        /// 添加 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void AddItem(Variable itemData)
        {
            GameObject go = m_ObjectPool.Spawn();
            ProcessParamSettingItem item = go.GetComponent<ProcessParamSettingItem>();
            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
                item.SetData(itemData);
                m_Items.Add(item);
            }
        }

        /// <summary>
        /// 移除 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void RemoveItem(Variable itemData)
        {
            ProcessParamSettingItem item = m_Items.Find(x => x.Data.Equals(itemData));
            if (item != null)
            {
                m_Items.Remove(item);
                m_ObjectPool.Despawn(item.gameObject);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            m_Items.ForEach(item =>
            {
                m_ObjectPool.Despawn(item.gameObject);
            });
            m_Items.Clear();
        }
    }
}

