using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class ProcessDropPanel : MonoBehaviour, IDropHandler
    {
        private UniEvent<ProcessDropItem> m_OnItemSelected = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 选中触发事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnItemSelected
        {
            get { return m_OnItemSelected; }
            set { m_OnItemSelected = value; }
        }

        private UniEvent<ProcessDropItem> m_OnItemEnqueued = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 入列事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnItemEnqueued
        {
            get { return m_OnItemEnqueued; }
            set { m_OnItemEnqueued = value; }
        }

        private UniEvent<ProcessDropItem> m_OnItemDequeued = new UniEvent<ProcessDropItem>();
        /// <summary>
        /// 出列事件
        /// </summary>
        public UniEvent<ProcessDropItem> OnItemDequeued
        {
            get { return m_OnItemDequeued; }
            set { m_OnItemDequeued = value; }
        }


        /// <summary>
        /// Container
        /// </summary>
        public RectTransform m_Container;

        /// <summary>
        /// Item
        /// </summary>
        public ProcessDropItem m_DefaultItem;

        /// <summary>
        /// 箭头
        /// </summary>
        public GameObject m_Arrow;

        /// <summary>
        /// 标题头
        /// </summary>
        [SerializeField]
        private Text m_TextHeader;

        private List<ProcessDropItem> m_Items = new List<ProcessDropItem>();
        private List<GameObject> m_ArrowList = new List<GameObject>();
        /// <summary>
        /// 列表
        /// </summary>
        public List<ProcessDropItem> Items { get { return m_Items; } }

        /// <summary>
        /// 对象池池
        /// </summary>
        private GameObjectPool m_ObjectPool = null;

        private ProcessDropItem selectedItem;
        private int m_Count;
        /// <summary>
        /// 选中 Item
        /// </summary>
        public ProcessDropItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnItemSelected.Invoke(selectedItem);
                //if (selectedItem == value)
                //    return;

                //if (selectedItem != null)
                //{
                //    selectedItem.DoUnselect();
                //}

                //selectedItem = value;
                //OnItemSelected.Invoke(selectedItem);

                //if (selectedItem != null)
                //{
                //    selectedItem.DoSelect();
                //}
            }
        }


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
        }

        private void Start()
        {
            //for (int i = 1; i <= 13; i++)
            //{
            //    AddItem(i);
            //}
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="title"></param>
        public void SetState(string title)
        {
            m_TextHeader.text = title;
        }

        /// <summary>
        /// 添加 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void AddItem(int number, ProcessLibraryItemData date)
        {
          
            GameObject go = m_ObjectPool.Spawn();
            ProcessDropItem item = go.GetComponent<ProcessDropItem>();
        

            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
              
                item.UpdateDropItem.AddListener(item_OnUpdateDropItem);
                item.OnEnqueued.AddListener(item_OnEnqueued);
                item.OnDeleted.AddListener(item_OnDeleted);
                item.OnSelected.AddListener(item_OnSelected);
                item.SetData(number, date);
                m_Items.Add(item);            
            }
            // arrow
            GameObject arrow = Instantiate(m_Arrow);
            arrow.SetActive(true);
            arrow.transform.SetParent(m_Container, false);
            arrow.layer = m_Container.gameObject.layer;
            m_ArrowList.Add(arrow);
            item.SetData(arrow);
            for (int i = 0; i < m_ArrowList.Count; i++)
            {
                if (i== m_ArrowList.Count-1)
                {
                    m_ArrowList[i].gameObject.SetActive(false);
                }
                else
                {
                    m_ArrowList[i].gameObject.SetActive(true);
                }
            }         
        }

        private void item_OnDeleted(ProcessDropItem arg0)
        {
            m_ArrowList.Remove(m_ArrowList[int.Parse(arg0.TextNumber)-1]);

            item_OnUpdateDropItem();
        }

        public void item_OnUpdateDropItem()
        {
            m_Items.Clear();
            ProcessDropItem[] itemArr = m_Container.GetComponentsInChildren<ProcessDropItem>();
            m_Items.AddRange(itemArr);
            if (m_Items.Count == 0)
            {
                EventDispatcher.ExecuteEvent<List<ProcessDropItem>>(Events.Process.ProcessSortItem.UpdateSortItem, m_Items);
                return;
            }
            for (int i = 0; i < m_Items.Count; i++)
            {
                //设置序号
                m_Items[i].SetData(i+1);
            }
            m_Items[m_Items.Count - 1].NextArrow.SetActive(false);
            EventDispatcher.ExecuteEvent<List<ProcessDropItem>>(Events.Process.ProcessSortItem.UpdateSortItem, m_Items);
        }
        /// <summary>
        /// 移除 Item
        /// </summary>
        /// <param name="itemData"></param>
        public void RemoveItem(int number)
        {
            ProcessDropItem item = m_Items.Find(x => x.currentNumber.Equals(number));
            if (item != null)
            {
                for (int i = item.currentNumber; i < m_Items.Count; i++)
                {
                    m_Items[i - 1].SetData(i);
                }
                m_Items.Remove(item);
               
                m_ObjectPool.Despawn(item.gameObject);
            }
            m_ArrowList[number - 1].gameObject.SetActive(false);
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


        private void item_OnSelected(ProcessDropItem arg0)
        {
            SelectedItem = arg0;
        }

        private void item_OnEnqueued(ProcessDropItem arg0)
        {
            OnItemEnqueued.Invoke(arg0);
            SelectedItem = arg0;
        }

        private void item_OnDequeued(ProcessDropItem arg0)
        {
            // OnItemDequeued.Invoke(arg0);

            RemoveItem(arg0.currentNumber);
        }

        public void OnDrop(PointerEventData eventData)
        {
            ProcessSampleItem d = eventData.pointerDrag.GetComponent<ProcessSampleItem>();

            if (d != null)
            {
                m_Count = m_Items.Count+1;
                AddItem(m_Count,d.Data);
                EventDispatcher.ExecuteEvent<ProcessLibraryItemData,int>(Events.Process.ProcessSortItem.AddSortItem, d.Data,m_Count);
            }
        }
    }
}

