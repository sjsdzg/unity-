using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class ActivityItem : Selectable, IPointerClickHandler
    {
        private UniEvent<ActivityItem> m_OnSelectd = new UniEvent<ActivityItem>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<ActivityItem> OnSelectd
        {
            get { return m_OnSelectd; }
            set { m_OnSelectd = value; }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        [SerializeField]
        private Image m_Icon;
        [SerializeField]
        private Text m_Text;
        [SerializeField]
        private Color normalColor;
        [SerializeField]
        private Color selectColor;
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

        protected override void OnEnable()
        {
            base.OnEnable();
            PlayEffect();
            Data = m_Text.text;
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

        private void PlayEffect()
        {
            if (isSelected)
            {
                targetGraphic.color = normalColor;
                m_Icon.color = selectColor;
                m_Text.color = selectColor;
            }
            else
            {
                targetGraphic.color = selectColor;
                m_Icon.color = normalColor;
                m_Text.color = normalColor;
            }
        }

    }
}
