using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace XFramework.UI
{
    public class ProcessSamplePanel : MonoBehaviour
    {
        /// <summary>
        /// Container
        /// </summary>
        public RectTransform m_Container;

        /// <summary>
        /// Item
        /// </summary>
        public ProcessSampleItem m_DefaultItem;
        public InputField inputFieldQuery;
        public Button buttonClose;

        /// <summary>
        /// 列表
        /// </summary>
        private List<ProcessSampleItem> m_Items = new List<ProcessSampleItem>();
        private List<string> nameList = new List<string>();
        /// <summary>
        /// 对象池池
        /// </summary>
        protected GameObjectPool m_ObjectPool = null;

        private Canvas m_Canvas;
        /// <summary>
        /// 缓存 Canvas
        /// </summary>
        public Canvas Canvas
        {
            get
            {
                if (m_Canvas == null)
                    CacheCanvas();

                return m_Canvas;
            }
        }

        private void CacheCanvas()
        {
            var list = ListPool<Canvas>.Get();
            gameObject.GetComponentsInParent<Canvas>(false, list);
            if (list.Count > 0)
            {
                // Find the first active and enabled canvas.
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].isActiveAndEnabled)
                    {
                        m_Canvas = list[i];
                        break;
                    }
                }
            }
            else
                m_Canvas = null;
        }

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
            inputFieldQuery.onEndEdit.AddListener(inputFieldQuery_onEndEdit);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            buttonClose.gameObject.SetActive(false);
            inputFieldQuery.text = "";
            for (int i = 0; i < m_Items.Count; i++)
            {
                ShowItem(m_Items[i].Data);
            }
        }

        private void inputFieldQuery_onEndEdit(string arg0)
        {
            if (!string.IsNullOrEmpty(arg0))
            {
                buttonClose.gameObject.SetActive(true);
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].Data.Name.Contains(arg0))
                {
                    ShowItem(m_Items[i].Data);
                }
                else
                {
                    HideItem(m_Items[i].Data);
                }
            }
        }

        public void SetData(ProcessLibraryData data)
        {
            foreach (var itemData in data.ItemDataList)
            {
                AddItem(itemData);
            }
        }

        /// <summary>
        /// 添加 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void AddItem(ProcessLibraryItemData itemData)
        {
            GameObject go = m_ObjectPool.Spawn();
            ProcessSampleItem item = go.GetComponent<ProcessSampleItem>();
            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
                item.Canvas = Canvas;
                item.SetData(itemData);
                nameList.Add(itemData.Name);
                m_Items.Add(item);
            }
        }

        /// <summary>
        /// 移除 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void RemoveItem(ProcessLibraryItemData itemData)
        {
            ProcessSampleItem item = m_Items.Find(x => x.Data.Equals(itemData));
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

        public void ShowItem(ProcessLibraryItemData itemData)
        {
            if (itemData == null)
                return;

            ProcessSampleItem item = m_Items.Find(x => x.Data.Equals(itemData));
            item.gameObject.SetActive(true);
        }

        public void HideItem(ProcessLibraryItemData itemData)
        {
            if (itemData == null)
                return;
                    
            ProcessSampleItem item = m_Items.Find(x => x.Data.Equals(itemData));
          //  item.OnEndDrag(null);
            item.gameObject.SetActive(false);
        }
    }
}

