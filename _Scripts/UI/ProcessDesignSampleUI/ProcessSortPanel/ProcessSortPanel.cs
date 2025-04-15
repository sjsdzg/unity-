using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
   public class ProcessSortPanel:MonoBehaviour
    {
        /// <summary>
        /// Container
        /// </summary>
        public RectTransform m_Container;

        /// <summary>
        /// Item
        /// </summary>
        public ProcessSortItem m_DefaultItem;
        public GameObject m_Arrow;
        private List<GameObject> m_ArrowList = new List<GameObject>();
        private List<ProcessSortItem> m_Items = new List<ProcessSortItem>();


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
            m_Arrow.SetActive(false);
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
            EventDispatcher.RegisterEvent<List<ProcessDropItem>>(Events.Process.ProcessSortItem.UpdateSortItem, UpdateItem);
            EventDispatcher.RegisterEvent<ProcessLibraryItemData,int>(Events.Process.ProcessSortItem.AddSortItem, AddItem);
        }
        public void AddItem(ProcessLibraryItemData data,int num)
        {
            GameObject go = m_ObjectPool.Spawn();
            ProcessSortItem item = go.GetComponent<ProcessSortItem>();
            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
                item.SetData( "("+ num+")." + data.Name);
                m_Items.Add(item);
                // arrow
                GameObject arrow = Instantiate(m_Arrow);
                arrow.SetActive(true);
                arrow.transform.SetParent(m_Container, false);
                arrow.layer = m_Container.gameObject.layer;
                m_ArrowList.Add(arrow);
            }
            for (int i = 0; i < m_ArrowList.Count; i++)
            {
                if (i == m_ArrowList.Count - 1)
                {
                    m_ArrowList[i].gameObject.SetActive(false);
                }
                else
                {
                    m_ArrowList[i].gameObject.SetActive(true);
                }
            }
        }
        /// <summary>
        /// 更新 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void UpdateItem(List<ProcessDropItem> items)
        {
            Clear();
            if (items.Count==0)
            {
                ProcessSortItem sortItem= m_Container.GetComponent<ProcessSortItem>();
                if (sortItem!=null)
                {
                    Destroy(sortItem);
                }              
                return;
            }
            for (int i = 0; i < items.Count; i++)
            {
                AddItem(items[i].Data,i+1);             
            }                                                         
        }
        private void Clear()
        {
            m_ArrowList.Clear();
            Transform container = m_Container as Transform;
            foreach (Transform item in container)
            {
                if (item.gameObject.activeSelf)
                {
                    Destroy(item.gameObject);
                }              
            }
        }
        private void OnDestroy()
        {
            EventDispatcher.UnregisterEvent<List<ProcessDropItem>>(Events.Process.ProcessSortItem.UpdateSortItem, UpdateItem);
            EventDispatcher.UnregisterEvent<ProcessLibraryItemData,int>(Events.Process.ProcessSortItem.AddSortItem, AddItem);
        }
    }
}
