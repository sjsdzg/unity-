using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class SelectorItem : Selectable, IPointerClickHandler
    {
        private UniEvent<SelectorItem> m_OnSelectd = new UniEvent<SelectorItem>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<SelectorItem> OnSelectd
        {
            get { return m_OnSelectd; }
            set { m_OnSelectd = value; }
        }

        [SerializeField]
        private bool isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public object Data { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayEffect();
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnSelectd.Invoke(this);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public virtual void DoSelect()
        {
            isSelected = true;
            PlayEffect();
        }

        /// <summary>
        /// 设置未选中状态
        /// </summary>
        public virtual void DoDeselect()
        {
            isSelected = false;
            PlayEffect();
        }

        protected virtual void PlayEffect()
        {

        }

    }
}
