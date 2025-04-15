using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{
    public class BaseHandle
    {
        public int id { get; set; }

        public BaseHandle(int id)
        {
            this.id = id;
        }

        private bool m_Active = true;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool active
        {
            get { return m_Active; }
            set 
            { 
                m_Active = value;
                if (!m_Active && HandleSystem.current != null && HandleSystem.current.currentSelectedHandle == this)
                    HandleSystem.current.SetSelectedHandle(null);

                OnSetProperty();
            }
        }

        private bool m_Interactable = true;
        /// <summary>
        /// 是否交互
        /// </summary>
        public bool interactable
        {
            get { return m_Interactable; }
            set 
            { 
                m_Interactable = value;
                if (!m_Interactable && HandleSystem.current != null && HandleSystem.current.currentSelectedHandle == this)
                    HandleSystem.current.SetSelectedHandle(null);

                OnSetProperty();
            }
        }

        protected SelectionStateRef currentSelectionState
        {
            get
            {
                if (!interactable)
                    return SelectionStateRef.Disabled;
                if (isPointerDown)
                    return SelectionStateRef.Pressed;
                //if (hasSelection)
                //    return SelectionStateRef.Selected;
                if (isPointerInside)
                    return SelectionStateRef.Highlighted;
                return SelectionStateRef.Normal;
            }
        }

        private bool isPointerInside { get; set; }
        private bool isPointerDown { get; set; }
        private bool hasSelection { get; set; }

        protected virtual void OnSetProperty()
        {
            DoStateTransition(currentSelectionState, false);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            HandleUtility.AddHandle(this);
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void Release()
        {
            HandleUtility.RemoveHandle(this);
        }

        /// <summary>
        /// 绘制Handle
        /// </summary>
        public virtual void OnDraw()
        {

        }

        public virtual void OnPointerEnter(HandlePointerData pointerData)
        {
            isPointerInside = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerExit(HandlePointerData pointerData)
        {
            isPointerInside = false;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerDown(HandlePointerData pointerData)
        {
            if (interactable && HandleSystem.current != null)
                HandleSystem.current.SetSelectedHandle(this, pointerData);

            isPointerDown = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerUp(HandlePointerData pointerData)
        {
            isPointerDown = false;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnSelect(HandlePointerData pointerData)
        {
            hasSelection = true;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnDeselect(HandlePointerData pointerData)
        {
            hasSelection = false;
            EvaluateAndTransitionToSelectionState();
        }

        public virtual void OnPointerClick(HandlePointerData pointerData)
        { 

        }

        public virtual void OnBeginDrag(HandlePointerData pointerData)
        {

        }

        public virtual void OnDrag(HandlePointerData pointerData)
        {

        }

        public virtual void OnEndDrag(HandlePointerData pointerData)
        {

        }

        // Change the button to the correct state
        private void EvaluateAndTransitionToSelectionState()
        {
            if (!interactable)
                return;

            DoStateTransition(currentSelectionState, false);
        }

        /// <summary>
        /// Transition the Selectable to the entered state.
        /// </summary>
        /// <param name="state">State to transition to</param>
        /// <param name="instant">Should the transition occur instantly.</param>
        protected virtual void DoStateTransition(SelectionStateRef state, bool instant)
        {
            
        }

        public virtual void Select()
        {
            if (HandleSystem.current == null || HandleSystem.current.alreadySelecting)
                return;

            HandleSystem.current.SetSelectedHandle(this);
        }
    }
}
