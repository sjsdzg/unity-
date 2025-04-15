using UnityEngine;
using System.Collections;
using XFramework.Core;

namespace XFramework.Common
{
    /// <summary>
    /// Custom Multi ListView
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class MultiListViewCustom<TMultiData, TItem, TData> : ListViewBase<TMultiData, TItem, TData> 
        where TItem : ListViewItemBase<TData>  
    {

    }
}

