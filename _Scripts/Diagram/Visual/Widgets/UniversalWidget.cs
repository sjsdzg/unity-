using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.Diagram
{
    public class UniversalWidget : VisualWidget, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum SelectionState
        {
            Normal,
            Highlighted,
            Selected,
        }

        /// <summary>
        /// inside
        /// </summary>
        public bool isPointerInside { get; set; }

        /// <summary>
        /// 是否选定
        /// </summary>
        public bool hasSelection { get; set; }

        /// <summary>
        /// 当前选择状态
        /// </summary>
        public SelectionState currentSelectionState
        {
            get
            {
                if (hasSelection)
                    return SelectionState.Selected;
                if (isPointerInside)
                    return SelectionState.Highlighted;
                return SelectionState.Normal;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ClearState();
        }

        protected void EvaluateAndTransition()
        {
            DoStateTransition(currentSelectionState);
        }

        public virtual void DoStateTransition(SelectionState state)
        {

        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;

            EvaluateAndTransition();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;

            EvaluateAndTransition();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Middle)
                return;

            Press();
        }

        /// <summary>
        /// 按下
        /// </summary>
        public virtual void Press()
        {

        }

        /// <summary>
        /// 选定
        /// </summary>
        public virtual void Select()
        {
            hasSelection = true;
            EvaluateAndTransition();
        }

        /// <summary>
        /// 取消选定
        /// </summary>
        public virtual void Deselect()
        {
            hasSelection = false;
            EvaluateAndTransition();
        }

        public void ClearState()
        {
            isPointerInside = false;
            hasSelection = false;
            EvaluateAndTransition();
        }
    }
}
