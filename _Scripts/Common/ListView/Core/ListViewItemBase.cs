using UnityEngine;
using System.Collections;
using XFramework.Core;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.UI.Extensions;
using UnityEngine.Serialization;

namespace XFramework.Common
{
    public abstract class ListViewItemBase<TData> : Selectable, IPointerClickHandler, ISubmitHandler
    {
        private int index = -1;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private bool isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        /// <summary>
        /// 默认颜色
        /// </summary>
        [SerializeField]
        private Color normalColor = Color.white;

        /// <summary>
        /// 选中颜色
        /// </summary>
        [SerializeField]
        private Color selectedColor = Color.white;

        private UniEvent<ListViewItemBase<TData>> m_OnClick = new UniEvent<ListViewItemBase<TData>>();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UniEvent<ListViewItemBase<TData>> OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private UniEvent<ListViewItemBase<TData>> m_DoubleClick = new UniEvent<ListViewItemBase<TData>>();
        /// <summary>
        /// 双击事件
        /// </summary>
        public UniEvent<ListViewItemBase<TData>> DoubleClick
        {
            get { return m_DoubleClick; }
            set { m_DoubleClick = value; }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public TData Data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (targetGraphic != null)
            {
                normalColor = targetGraphic.color;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        public virtual void SetData(TData data)
        {
            Data = data;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();

            if (eventData.clickCount == 2)
                m_DoubleClick.Invoke(this);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            m_OnClick.Invoke(this);
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public virtual void DoSelect()
        {
            isSelected = true;
            if (targetGraphic != null)
            {
                targetGraphic.color = selectedColor;
            }
        }

        /// <summary>
        /// 设置未选中状态
        /// </summary>
        public virtual void DoDeselect()
        {
            isSelected = false;
            if (targetGraphic != null)
            {
                targetGraphic.color = normalColor;
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        public virtual void OnReset()
        {
            DoDeselect();
            m_OnClick.RemoveAllListeners();
            m_DoubleClick.RemoveAllListeners();
        }
    }
}

