using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    public class ProcessLibraryPanel : MultiListViewCustom<ProcessLibraryData, ProcessLibraryItem, ProcessLibraryItemData>
    {
        [SerializeField]
        private Text m_Text;

        private UniEvent<ProcessLibraryItem> m_OnItemBeginDrag = new UniEvent<ProcessLibraryItem>();
        /// <summary>
        /// 开始拖拽事件
        /// </summary>
        public UniEvent<ProcessLibraryItem> OnItemBeginDrag
        {
            get { return m_OnItemBeginDrag; }
            set { m_OnItemBeginDrag = value; }
        }

        public override void SetData(ProcessLibraryData data)
        {
            base.SetData(data);
            m_Text.text = data.Name;
            DataSource.Clear();
            if (data.ItemDataList != null)
            {
                DataSource.AddRange(data.ItemDataList);
            }
        }

        protected override void SetData(ProcessLibraryItem item, ProcessLibraryItemData data)
        {
            base.SetData(item, data);
            item.OnBeginDragEvent.AddListener(item_OnBeginDragEvent);
        }

        private void item_OnBeginDragEvent(ProcessLibraryItem arg0)
        {
            OnItemBeginDrag.Invoke(arg0);
        }
    }

}