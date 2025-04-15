using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XFramework.Core;

namespace XFramework.UIWidgets
{
    public class MenuItem : Selectable, IPointerClickHandler
    {
        private UniEvent<MenuItem> m_OnClicked = new UniEvent<MenuItem>();
        /// <summary>
        /// 点击时，触发
        /// </summary>
        public UniEvent<MenuItem> OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnClicked.Invoke(this);
        }
    }
}

