using UnityEngine;
using System.Collections;

namespace XFramework.Common
{
    /// <summary>
    /// Sample ListView
    /// </summary>
    public class ListView : ListViewCustom<ListViewItem, ListViewItemData>
    {
        protected override void SetData(ListViewItem component, ListViewItemData data)
        {
            base.SetData(component, data);
        }
    }
}

