using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Runtime
{
    public class HandleSystem : MonoBehaviour
    {
        private HandleInputModule m_InputModule;

        /// <summary>
        /// 操作手柄系统
        /// </summary>
        public static HandleSystem current { get; set; }

        [SerializeField]
        private int m_DragThreshold = 10;
        public int pixelDragThreshold
        {
            get { return m_DragThreshold; }
            set { m_DragThreshold = value; }
        }

        public BaseHandle m_CurrentSelected;

        public BaseHandle currentSelectedHandle
        {
            get { return m_CurrentSelected; }
        }

        private bool m_HasFocus = true;
        /// <summary>
        /// isFocused
        /// </summary>
        public bool isFocused
        {
            get { return m_HasFocus; }
        }

        private void OnEnable()
        {
            current = this;
            m_InputModule = GetComponent<HandleInputModule>();
        }

        private void OnDisable()
        {
            m_InputModule.ClearSelection();
            m_InputModule = null;
        }

        protected virtual void Update()
        {
            if (m_InputModule != null)
            {
                m_InputModule.Process();
                UpdateHandlesState();
            }
        }

        private bool m_SelectionGuard;
        public bool alreadySelecting
        {
            get { return m_SelectionGuard; }
        }

        public void SetSelectedHandle(BaseHandle selected, HandlePointerData pointer)
        {
            if (m_SelectionGuard)
            {
                Debug.LogError("Attempting to select " + selected + "while already selecting an object.");
                return;
            }

            m_SelectionGuard = true;
            if (selected == m_CurrentSelected)
            {
                m_SelectionGuard = false;
                return;
            }

            // Debug.Log("Selection: new (" + selected + ") old (" + m_CurrentSelected + ")");
            HandleExecuteEvents.OnDeselect(m_CurrentSelected, pointer);
            m_CurrentSelected = selected;
            HandleExecuteEvents.OnSelect(m_CurrentSelected, pointer);
            m_SelectionGuard = false;
        }

        public void SetSelectedHandle(BaseHandle selected)
        {
            SetSelectedHandle(selected, new HandlePointerData());
        }

        private void OnApplicationFocus(bool focus)
        {
            m_HasFocus = focus;
        }

        public bool IsPointerOverHandle()
        {
            return IsPointerOverHandle(HandleInputModule.kMouseLeftId);
        }

        public bool IsPointerOverHandle(int pointerId)
        {
            if (m_InputModule == null)
                return false;

            return m_InputModule.IsPointerOverHandle(pointerId);
        }

        public int FindNearestHandleControl()
        {
            HandleUtility.ProcessHandleControl();
            return HandleUtility.nearestControl;
        }


        public void UpdateHandlesState()
        {
            HandleUtility.UpdateHandlesState();
        }


        public BaseHandle GetHandle(int id)
        {
            return HandleUtility.GetHandle(id);
        }


    }
}
