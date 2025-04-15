using UnityEngine;
using System.Collections;
using XFramework.Core;

namespace XFramework.Common
{
    /// <summary>
    /// Custom ListView
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class ListViewCustom<TItem, TData> : ListViewBase<object, TItem, TData>
        where TItem : ListViewItemBase<TData>  
    {
        public sealed override void SetData(object data)
        {
            base.SetData(data);
        }
    }
}

