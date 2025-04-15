using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using System;
using System.Linq;

namespace XFramework.Common
{
    /// <summary>
    /// ListView Base
    /// </summary>
    /// <typeparam name="TMultiData"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public abstract class ListViewBase<TMultiData, TItem, TData> : ListViewItemBase<TMultiData>
        where TItem : ListViewItemBase<TData>
    {
        protected ObservableCollection<TData> dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public virtual ObservableCollection<TData> DataSource
        {
            get
            {
                if (dataSource == null)
                {
                    dataSource = new ObservableCollection<TData>();
                    Initialize();
                }
                return dataSource;
            }
            set
            {
                if (dataSource != null)
                    dataSource.Clear();

                dataSource = value;
                Initialize();
                AddRange(dataSource.ToArray());
            }
        }

        [SerializeField]
        private bool multiSelect = false;
        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool MultiSelect
        {
            get { return multiSelect; }
            set { multiSelect = value; }
        }

        [SerializeField]
        private int selectedIndex = -1;
        /// <summary>
        /// 选中索引
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        [SerializeField]
        private List<int> selectedIndicies = new List<int>();
        /// <summary>
        /// 选中索引列表
        /// </summary>
        public List<int> SelectedIndicies
        {
            get { return selectedIndicies; }
            set { selectedIndicies = value; }
        }

        private UniEvent<TItem> m_OnItemSelected = new UniEvent<TItem>();
        /// <summary>
        /// 选中Item事件
        /// </summary>
        public UniEvent<TItem> OnItemSelected
        {
            get { return m_OnItemSelected; }
            set { m_OnItemSelected = value; }
        }

        private UniEvent<TItem> m_OnItemDeselected = new UniEvent<TItem>();
        /// <summary>
        /// 取消选中Item事件
        /// </summary>
        public UniEvent<TItem> OnItemDeselected
        {
            get { return m_OnItemDeselected; }
            set { m_OnItemDeselected = value; }
        }

        /// <summary>
        /// The container for items objects.
        /// </summary>
        [SerializeField]
        public Transform m_Container;

        /// <summary>
        /// The default item.
        /// </summary>
        [SerializeField]
        public TItem m_ItemTemplate;

        protected List<TItem> m_Items = new List<TItem>();
        /// <summary>
        /// The items list.
        /// </summary>
        public List<TItem> Items
        {
            get { return m_Items; }
        }

        /// <summary>
        /// 选中的Items
        /// </summary>
        public List<TItem> SelectedItems {
            get
            {
                List<TItem> selectItems = new List<TItem>();
                foreach (var index in SelectedIndicies)
                {
                    TItem item = GetItem(index);
                    selectItems.Add(item);
                }
                return selectItems;
            }
        }

        /// <summary>
        /// 组建池
        /// </summary>
        protected GameObjectPool m_ObjectPool = null;

        protected override void Start()
        {
            base.Start();

            if (m_Container == null)
                throw new NullReferenceException("m_Container is null.");

            if (m_ItemTemplate == null)
                throw new NullReferenceException("m_Template is null.");

            m_ItemTemplate.gameObject.SetActive(false);
        }

        protected virtual void Initialize()
        {
            if (m_ObjectPool == null)
            {
                //unused
                GameObject unused;
                Transform temp = transform.Find("unused");
                if (temp == null)
                    unused = new GameObject("unused");
                else
                    unused = temp.gameObject;

                unused.SetActive(false);
                unused.transform.SetParent(transform, false);

                m_ObjectPool = new GameObjectPool(unused.transform, m_ItemTemplate.gameObject);
            }

            //DataSource.OnAdd += Add;
            //DataSource.OnRemove += Remove;
            //DataSource.OnClear += Clear;
            //DataSource.OnInsert += Insert;
            //DataSource.OnRemoveAt += RemoveAt;
            DataSource.CollectionChanged += DataSource_CollectionChanged;
            // 清除

        }

        private void DataSource_CollectionChanged(object sender, CollectionChangedArgs e)
        {
            switch (e.Action)
            {
                case CollectionChangedAction.Add:
                    int newStartingIndex = e.NewStartingIndex;
                    foreach (var item in e.NewItems)
                    {
                        TData data = (TData)item;
                        if (newStartingIndex == -1)
                        {
                            Add(data);
                        }
                        else
                        {
                            Insert(newStartingIndex, data);
                            newStartingIndex++;
                        }
                    }
                    break;
                case CollectionChangedAction.Remove:
                    int oldStartingIndex = e.OldStartingIndex;
                    foreach (var item in e.OldItems)
                    {
                        TData data = (TData)item;
                        if (oldStartingIndex == -1)
                        {
                            Remove(data);
                        }
                        else
                        {
                            RemoveAt(oldStartingIndex);
                            oldStartingIndex++;
                        }
                    }
                    break;
                case CollectionChangedAction.Replace:
                    RemoveAt(e.OldStartingIndex);
                    Insert(e.NewStartingIndex, (TData)e.NewItems[0]);
                    break;
                case CollectionChangedAction.Move:
                    RemoveAt(e.OldStartingIndex);
                    Insert(e.NewStartingIndex, (TData)e.NewItems[0]);
                    break;
                case CollectionChangedAction.Reset:
                    Clear();
                    foreach (var item in DataSource)
                    {
                        TData data = (TData)item;
                        Add(data);
                    }
                    break;
                default:
                    break;
            }
        }

        protected virtual void SetData(TItem item, TData data)
        {
            item.SetData(data);
            item.OnClick.AddListener(Item_OnClick);
        }

        protected TItem GetItem(int index)
        {
            return m_Items[index];
        }

        private void Item_OnClick(ListViewItemBase<TData> item)
        {
            var shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var haveSelected = selectedIndicies.Count > 0;
            if (MultiSelect && shiftPressed && haveSelected && selectedIndicies[0] != item.Index)
            {
                selectedIndicies.GetRange(1, selectedIndicies.Count - 1).ForEach(DeselectItem);

                var min = Mathf.Min(selectedIndicies[0], item.Index);
                var max = Mathf.Max(selectedIndicies[0], item.Index);

                Enumerable.Range(min, max - min + 1).ForEach(SelectItem);
                return;
            }
            if (MultiSelect && item.IsSelected)
            {
                DeselectItem(item.Index);
            }
            else
            {
                SelectItem(item.Index);
            }
        }

        /// <summary>
        /// 选择Item
        /// </summary>
        /// <param name="index"></param>
        public virtual void SelectItem(int index)
        {
            if (index == -1)
                return;


            TItem item = GetItem(index);

            if (MultiSelect && item.IsSelected)
                return;

            if (!MultiSelect)
            {
                if ((selectedIndex != -1) && (selectedIndex != index))
                {
                    DeselectItem(selectedIndex);
                }
                // 取消多选项
                for (int i = 0; i < selectedIndicies.Count; i++)
                {
                    var multi = selectedIndicies[i];
                    DeselectItem(multi);
                }
                selectedIndicies.Clear();
            }

            selectedIndicies.Add(index);
            selectedIndex = index;

            item.DoSelect();

            OnItemSelected.Invoke(item);
        }

        /// <summary>
        /// 取消选择Item
        /// </summary>
        /// <param name="index"></param>
        public virtual void DeselectItem(int index)
        {
            if (index == -1)
                return;

            TItem item = GetItem(index);

            if (!item.IsSelected)
                return;

            selectedIndicies.Remove(index);
            selectedIndex = (SelectedIndicies.Count > 0) ? SelectedIndicies.Last() : -1;

            item.DoDeselect();

            OnItemDeselected.Invoke(item);
        }

        public void DeselectAll()
        {
            int length = SelectedIndicies.Count;
            for (int i = length - 1; i >= 0; i--)
            {
                var index = SelectedIndicies[i];
                DeselectItem(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        protected virtual void AddRange(TData[] array)
        {
            foreach (var item in array)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        protected virtual void Add(TData data)
        {
            GameObject go = m_ObjectPool.Spawn();
            TItem item = go.GetComponent<TItem>();
            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
                item.Index = m_Items.Count;
                m_Items.Add(item);
                SetData(item, data);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="data"></param>
        protected virtual void Remove(TData data)
        {
            TItem item = m_Items.Find(x => object.Equals(x.Data, data));
            m_Items.ForEach(y =>
            {
                if (y.Index > item.Index)
                    y.Index--;
            });
            m_Items.Remove(item);
            // Index
            selectedIndicies.Remove(item.Index);
            selectedIndex = -1;

            item.OnReset();
            m_ObjectPool.Despawn(item.gameObject);
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void Clear()
        {
            m_Items.ForEach(item => {
                item.OnReset();
                m_ObjectPool.Despawn(item.gameObject);
            });
            m_Items.Clear();
            OnReset();
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        protected virtual void Insert(int index, TData data)
        {
            if (index < 0 || index > m_Items.Count)
                throw new ArgumentOutOfRangeException("ListView");

            GameObject go = m_ObjectPool.Spawn();
            TItem item = go.GetComponent<TItem>();
            if (item != null && m_Container != null)
            {
                item.transform.SetParent(m_Container, false);
                item.gameObject.layer = m_Container.gameObject.layer;
                item.transform.SetSiblingIndex(index);
                item.Index = index;
                m_Items.Insert(index, item);
                for (int i = index; i < m_Items.Count; i++)
                {
                    m_Items[i].Index = i;
                }
                SetData(item, data);
            }
        }

        /// <summary>
        /// 移除某一个
        /// </summary>
        /// <param name="index"></param>
        protected virtual void RemoveAt(int index)
        {
            TItem item = m_Items[index];
            m_Items.RemoveAt(index);

            for (int i = index; i < m_Items.Count; i++)
            {
                m_Items[i].Index = i;
            }
            // Index
            selectedIndicies.Remove(item.Index);
            selectedIndex = -1;

            m_ObjectPool.Despawn(item.gameObject);
        }

        public override void OnReset()
        {
            base.OnReset();
            selectedIndex = -1;
            selectedIndicies.Clear();
        }
    }
}

