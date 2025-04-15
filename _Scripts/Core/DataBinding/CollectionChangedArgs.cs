using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public enum CollectionChangedAction
    {
        /// <summary>
        /// 加入
        /// </summary>
        Add,
        /// <summary>
        /// 移除
        /// </summary>
        Remove,
        /// <summary>
        /// 替换
        /// </summary>
        Replace,
        /// <summary>
        /// 移动
        /// </summary>
        Move,
        /// <summary>
        /// 集合的内容发生显著更改
        /// </summary>
        Reset,
    }

    public class CollectionChangedArgs : EventArgs
    {
        //public CollectionChangedArgs(CollectionChangedAction action)
        //{
        //    Action = action;
        //    Element = null;
        //    Index = -1;
        //}

        //public CollectionChangedArgs(CollectionChangedAction action, object element, int index)
        //{
        //    Action = action;
        //    Element = element;
        //    Index = index;
        //    Count = 1;
        //}

        //public CollectionChangedArgs(CollectionChangedAction action, int index, int count)
        //{
        //    Action = action;
        //    Element = null;
        //    Index = index;
        //    Count = count;
        //}

        private CollectionChangedAction _action;
        /// <summary>
        /// Action
        /// </summary>
        public CollectionChangedAction Action 
        { 
            get { return _action; } 
        }

        public object Element { get; private set; }

        public int Index { get; private set; }

        public int Count { get; private set; }

        private IList _newItems;
        /// <summary>
        /// NewItems
        /// </summary>
        public IList NewItems
        {
            get { return _newItems; }
        }

        private int _newStartingIndex;
        /// <summary>
        /// NewStartingIndex
        /// </summary>
        public int NewStartingIndex
        {
            get { return _newStartingIndex; }
        }

        private IList _oldItems;
        /// <summary>
        /// OldItems
        /// </summary>
        public IList OldItems
        {
            get { return _oldItems; }
        }

        private int _oldStartingIndex;
        /// <summary>
        /// OldStartingIndex
        /// </summary>
        public int OldStartingIndex
        {
            get { return _oldStartingIndex; }
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a reset change. 
        ///  
        /// The action that caused the event (must be Reset).
        public CollectionChangedArgs(CollectionChangedAction action)
        {
            if (action != CollectionChangedAction.Reset)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Reset), action");

            InitializeAdd(action, null, -1);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a one-item change. 
        /// 
        /// The action that caused the event; can only be Reset, Add or Remove action.
        /// The item affected by the change.
        public CollectionChangedArgs(CollectionChangedAction action, object changedItem)
        {
            if ((action != CollectionChangedAction.Add) && (action != CollectionChangedAction.Remove)
                    && (action != CollectionChangedAction.Reset))
                throw new ArgumentException("SR.Get(SRID.MustBeResetAddOrRemoveActionForCtor), action");

            if (action == CollectionChangedAction.Reset)
            {
                if (changedItem != null)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresNullItem), action");

                InitializeAdd(action, null, -1);
            }
            else
            {
                InitializeAddOrRemove(action, new object[] { changedItem }, -1);
            }
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a one-item change. 
        ///  
        /// The action that caused the event.
        /// The item affected by the change. 
        /// The index where the change occurred.
        public CollectionChangedArgs(CollectionChangedAction action, object changedItem, int index)
        {
            if ((action != CollectionChangedAction.Add) && (action != CollectionChangedAction.Remove)
                    && (action != CollectionChangedAction.Reset))
                throw new ArgumentException("SR.Get(SRID.MustBeResetAddOrRemoveActionForCtor), action");

            if (action == CollectionChangedAction.Reset)
            {
                if (changedItem != null)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresNullItem), action");
                if (index != -1)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresIndexMinus1), action");

                InitializeAdd(action, null, -1);
            }
            else
            {
                InitializeAddOrRemove(action, new object[] { changedItem }, index);
            }
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a multi-item change. 
        ///  
        /// The action that caused the event.
        /// The items affected by the change. 
        public CollectionChangedArgs(CollectionChangedAction action, IList changedItems)
        {
            if ((action != CollectionChangedAction.Add) && (action != CollectionChangedAction.Remove)
                    && (action != CollectionChangedAction.Reset))
                throw new ArgumentException("SR.Get(SRID.MustBeResetAddOrRemoveActionForCtor), action");

            if (action == CollectionChangedAction.Reset)
            {
                if (changedItems != null)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresNullItem), action");

                InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                    throw new ArgumentNullException("changedItems");

                InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a multi-item change (or a reset). 
        ///  
        /// The action that caused the event.
        /// The items affected by the change. 
        /// The index where the change occurred.
        public CollectionChangedArgs(CollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if ((action != CollectionChangedAction.Add) && (action != CollectionChangedAction.Remove)
                    && (action != CollectionChangedAction.Reset))
                throw new ArgumentException("SR.Get(SRID.MustBeResetAddOrRemoveActionForCtor), action");

            if (action == CollectionChangedAction.Reset)
            {
                if (changedItems != null)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresNullItem), action");
                if (startingIndex != -1)
                    throw new ArgumentException("SR.Get(SRID.ResetActionRequiresIndexMinus1), action");

                InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                    throw new ArgumentNullException("changedItems");
                if (startingIndex < -1)
                    throw new ArgumentException("SR.Get(SRID.IndexCannotBeNegative), startingIndex");

                InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a one-item Replace event.
        /// 
        /// Can only be a Replace action. 
        /// The new item replacing the original item.
        /// The original item that is replaced. 
        public CollectionChangedArgs(CollectionChangedAction action, object newItem, object oldItem)
        {
            if (action != CollectionChangedAction.Replace)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Replace), action");

            InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a one-item Replace event. 
        /// 
        /// Can only be a Replace action. 
        /// The new item replacing the original item.
        /// The original item that is replaced.
        /// The index of the item being replaced.
        public CollectionChangedArgs(CollectionChangedAction action, object newItem, object oldItem, int index)
        {
            if (action != CollectionChangedAction.Replace)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Replace), action");

            InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a multi-item Replace event. 
        /// 
        /// Can only be a Replace action. 
        /// The new items replacing the original items. 
        /// The original items that are replaced.
        public CollectionChangedArgs(CollectionChangedAction action, IList newItems, IList oldItems)
        {
            if (action != CollectionChangedAction.Replace)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Replace), action");
            if (newItems == null)
                throw new ArgumentNullException("newItems");
            if (oldItems == null)
                throw new ArgumentNullException("oldItems");

            InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a multi-item Replace event. 
        /// 
        /// Can only be a Replace action. 
        /// The new items replacing the original items. 
        /// The original items that are replaced.
        /// The starting index of the items being replaced. 
        public CollectionChangedArgs(CollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
        {
            if (action != CollectionChangedAction.Replace)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Replace), action");
            if (newItems == null)
                throw new ArgumentNullException("newItems");
            if (oldItems == null)
                throw new ArgumentNullException("oldItems");

            InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a one-item Move event.
        ///  
        /// Can only be a Move action. 
        /// The item affected by the change.
        /// The new index for the changed item. 
        /// The old index for the changed item.
        public CollectionChangedArgs(CollectionChangedAction action, object changedItem, int index, int oldIndex)
        {
            if (action != CollectionChangedAction.Move)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Move), action");
            if (index < 0)
                throw new ArgumentException("SR.Get(SRID.IndexCannotBeNegative), index");

            object[] changedItems = new object[] { changedItem };
            InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }

        /// 

        /// Construct a CollectionChangedArgs that describes a multi-item Move event.
        ///  
        /// The action that caused the event. 
        /// The items affected by the change.
        /// The new index for the changed items. 
        /// The old index for the changed items.
        public CollectionChangedArgs(CollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            if (action != CollectionChangedAction.Move)
                throw new ArgumentException("SR.Get(SRID.WrongActionForCtor, CollectionChangedAction.Move), action");
            if (index < 0)
                throw new ArgumentException("SR.Get(SRID.IndexCannotBeNegative), index");

            InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }

        private void InitializeAddOrRemove(CollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == CollectionChangedAction.Add)
                InitializeAdd(action, changedItems, startingIndex);
            else if (action == CollectionChangedAction.Remove)
                InitializeRemove(action, changedItems, startingIndex);
            else
                throw new ArgumentException("Unsupported action: {0}", action.ToString());
        }

        private void InitializeAdd(CollectionChangedAction action, IList newItems, int newStartingIndex)
        {
            _action = action;
            _newItems = (newItems == null) ? null : ArrayList.ReadOnly(newItems);
            _newStartingIndex = newStartingIndex;
        }

        private void InitializeRemove(CollectionChangedAction action, IList oldItems, int oldStartingIndex)
        {
            _action = action;
            _oldItems = (oldItems == null) ? null : ArrayList.ReadOnly(oldItems);
            _oldStartingIndex = oldStartingIndex;
        }

        private void InitializeMoveOrReplace(CollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            InitializeAdd(action, newItems, startingIndex);
            InitializeRemove(action, oldItems, oldStartingIndex);
        }
    }
}
