using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public class ObservableCollection<T> : IList<T>
    {
        private List<T> m_List;

        public event CollectionChangedHandler CollectionChanged;

        public event PropertyChangedHandler ItemChanged;

        public ObservableCollection()
        {
            m_List = new List<T>();
        }

        public ObservableCollection(IEnumerable<T> collection)
        {
            m_List = new List<T>(collection);
            m_List.ForEach(AddCallback);
        }


        public T this[int index]
        {
            get { return m_List[index]; }
            set
            {
                T originalItem = this[index];
                RemoveCallback(originalItem);
                m_List[index] = value;
                AddCallback(m_List[index]);

                OnCollectionChanged(CollectionChangedAction.Replace, originalItem, value, index);
            }
        }

        public int Count { get { return m_List.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Contains(T item)
        {
            return m_List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return m_List.IndexOf(item);
        }

        public void Add(T item)
        {
            int index = m_List.Count;
            m_List.Add(item);
            AddCallback(item);

            
            OnCollectionChanged(CollectionChangedAction.Add, item, index);
        }

        public void AddRange(IList<T> collection)
        {
            int index = m_List.Count;
            m_List.AddRange(collection);
            AddRangeCallback(collection);

            OnCollectionChanged(new CollectionChangedArgs(CollectionChangedAction.Add, collection.ToArray(), index));
        }


        public void Insert(int index, T item)
        {
            m_List.Insert(index, item);
            AddCallback(item);

            OnCollectionChanged(CollectionChangedAction.Add, item, index);
        }

        public bool Remove(T item)
        {
            var index = m_List.IndexOf(item);
            if (index == -1) return false;
            m_List.RemoveAt(index);
            RemoveCallback(item);

            OnCollectionChanged(CollectionChangedAction.Remove, item, index);
            return true;
        }

        public void RemoveAt(int index)
        {
            var item = m_List[index];
            m_List.RemoveAt(index);
            RemoveCallback(item);

            OnCollectionChanged(CollectionChangedAction.Remove, item, index);
        }

        public T Find(Predicate<T> match)
        {
            foreach (var item in m_List)
            {
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        public void Move(int oldIndex, int newIndex)
        {
            var removedItem = m_List[oldIndex];
            m_List.RemoveAt(oldIndex);
            m_List.Insert(newIndex, removedItem);

            OnCollectionChanged(CollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        public void Clear()
        {
            m_List.ForEach(RemoveCallback);
            m_List.Clear();

            OnCollectionReset();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        private void AddCallback(T item)
        {
            if (item is IPropertyChanged)
            {
                (item as IPropertyChanged).PropertyChanged += ItemChanged;
            }
        }

        private void AddRangeCallback(IList<T> collection)
        {
            foreach (var item in collection)
            {
                if (item is IPropertyChanged)
                {
                    (item as IPropertyChanged).PropertyChanged += ItemChanged;
                }
            }
        }

        private void RemoveCallback(T item)
        {
            if (item is IPropertyChanged)
            {
                (item as IPropertyChanged).PropertyChanged -= ItemChanged;
            }
        }

        public void Sort()
        {
            m_List.Sort();
            OnCollectionReset();
        }

        public void Sort(Comparison<T> comparison)
        {
            m_List.Sort(comparison);
            OnCollectionReset();
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(CollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new CollectionChangedArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(CollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new CollectionChangedArgs(action, item, index, oldIndex));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(CollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new CollectionChangedArgs(action, newItem, oldItem, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event with action == Reset to any listeners
        /// </summary>
        private void OnCollectionReset()
        {
            OnCollectionChanged(new CollectionChangedArgs(CollectionChangedAction.Reset));
        }

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// </remarks>
        protected virtual void OnCollectionChanged(CollectionChangedArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
    }
}
