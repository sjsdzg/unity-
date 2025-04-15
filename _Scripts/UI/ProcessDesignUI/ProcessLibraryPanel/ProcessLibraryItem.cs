using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.UI
{
    public class ProcessLibraryItem : ListViewItemBase<ProcessLibraryItemData>, IBeginDragHandler, IDragHandler
    {
        /// <summary>
        /// 文本
        /// </summary>
        [SerializeField]
        private Text m_Text;

        private UniEvent<ProcessLibraryItem> m_OnBeginDragEvent = new UniEvent<ProcessLibraryItem>();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UniEvent<ProcessLibraryItem> OnBeginDragEvent
        {
            get { return m_OnBeginDragEvent; }
            set { m_OnBeginDragEvent = value; }
        }

        public override void SetData(ProcessLibraryItemData data)
        {
            base.SetData(data);
            m_Text.text = data.Name;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            OnBeginDragEvent.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public override void OnReset()
        {
            base.OnReset();
            OnBeginDragEvent.RemoveAllListeners();
        }


    }
}

