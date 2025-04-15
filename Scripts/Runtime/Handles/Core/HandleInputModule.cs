using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XFramework.Runtime
{
    public class HandleInputModule : MonoBehaviour
    {
        public const int kMouseLeftId = -1;

        private HandleSystem m_HandleSystem;
        /// <summary>
        /// HandleSystem
        /// </summary>
        protected HandleSystem handleSystem
        {
            get { return m_HandleSystem; }
        }

        protected Dictionary<int, HandlePointerData> m_PointerDataDict = new Dictionary<int, HandlePointerData>();

        protected bool GetPointerData(int id, out HandlePointerData data, bool create)
        {
            if (!m_PointerDataDict.TryGetValue(id, out data) && create)
            {
                data = new HandlePointerData()
                {
                    pointerId = id,
                };
                m_PointerDataDict.Add(id, data);
                return true;
            }

            return false;
        }

        protected void OnEnable()
        {
            m_HandleSystem = GetComponent<HandleSystem>();
        }

        protected virtual HandlePointerData GetMousePointerData(int id, out bool pressed, out bool released)
        {
            // Populate the left button...
            HandlePointerData data;
            var created = GetPointerData(kMouseLeftId, out data, true);

            pressed = InputUtility.GetMouseButtonDown(id);
            released = InputUtility.GetMouseButtonUp(id);

            if (created)
                data.position = InputUtility.mousePosition;

            Vector2 pos = InputUtility.mousePosition;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                data.position = new Vector2(-1.0f, -1.0f);
                data.delta = Vector2.zero;
            }
            else
            {
                data.delta = pos - data.position;
                data.position = pos;
            }
            data.scrollDelta = InputUtility.mouseScrollDelta;
            int nearest = handleSystem.FindNearestHandleControl();
            data.pointerCurrentControl = nearest;

            return data;
        }

        public void Process()
        {
            if (!m_HandleSystem.isFocused)
                return;

            ProcessMouseEvent();
        }

        protected void ProcessMouseEvent()
        {
            bool pressed;
            bool released;
            var pointerData = GetMousePointerData(0, out pressed, out released);

            ProcessMousePress(pointerData, pressed, released);
            ProcessMove(pointerData);
            ProcessDrag(pointerData);
        }

        private void ProcessMousePress(HandlePointerData pointerData, bool pressed, bool released)
        {
            int handleId = pointerData.pointerCurrentControl;
            BaseHandle handle = handleSystem.GetHandle(handleId);

            if (pressed)
            {
                pointerData.delta = Vector2.zero;
                pointerData.dragging = false;
                pointerData.useDragThreshold = true;
                pointerData.pressPosition = pointerData.position;
                pointerData.pointerPressControl = pointerData.pointerCurrentControl;

                DeselectIfSelectionChanged(handle, pointerData);

                HandleExecuteEvents.OnPointerDown(handle, pointerData);

                pointerData.pointerPress = handle;

                pointerData.pointerDrag = handle;
            }

            if (released)
            {
                HandleExecuteEvents.OnPointerUp(pointerData.pointerPress, pointerData);

                if (pointerData.pointerPress == handle)
                {
                    HandleExecuteEvents.OnPointerClick(pointerData.pointerPress, pointerData);
                }
                else if (pointerData.pointerDrag != null && pointerData.dragging)
                {
                    // OnDrop TODO
                }

                pointerData.pointerPress = null;

                if (pointerData.pointerDrag != null && pointerData.dragging)
                {
                    HandleExecuteEvents.OnEndDrag(pointerData.pointerDrag, pointerData);
                }

                pointerData.dragging = false;
                pointerData.pointerDrag = null;

                if (handle != pointerData.pointerEnter)
                {
                    HandlePointerExitAndEnter(pointerData, null);
                    HandlePointerExitAndEnter(pointerData, handle);
                }
            }
        }

        private void ProcessMove(HandlePointerData pointerData)
        {
            BaseHandle handle = (Cursor.lockState == CursorLockMode.Locked) ? null : handleSystem.GetHandle(pointerData.pointerCurrentControl);
            HandlePointerExitAndEnter(pointerData, handle);
        }

        private void ProcessDrag(HandlePointerData pointerData)
        {
            if (!pointerData.IsPointerMoving() ||
                Cursor.lockState == CursorLockMode.Locked
                || pointerData.pointerDrag == null)
            {
                return;
            }

            if (!pointerData.dragging &&
                ShouldStartDrag(pointerData.pressPosition, pointerData.position, handleSystem.pixelDragThreshold, pointerData.useDragThreshold))
            {
                HandleExecuteEvents.OnBeginDrag(pointerData.pointerDrag, pointerData);
                pointerData.dragging = true;
            }

            if (pointerData.dragging)
            {
                if (pointerData.pointerPress != pointerData.pointerDrag)
                {
                    HandleExecuteEvents.OnPointerUp(pointerData.pointerPress, pointerData);

                    pointerData.pointerPress = null;
                }

                HandleExecuteEvents.OnDrag(pointerData.pointerDrag, pointerData);
            }
        }

        private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            if (!useDragThreshold)
                return true;

            return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
        }

        protected void HandlePointerExitAndEnter(HandlePointerData currentPointerData, BaseHandle newEnterTarget)
        {
            if (currentPointerData.pointerEnter == newEnterTarget)
                return;

            if (currentPointerData.pointerEnter != null)
            {
                HandleExecuteEvents.OnPointerExit(currentPointerData.pointerEnter, currentPointerData);
            }

            currentPointerData.pointerEnter = newEnterTarget;
            if (newEnterTarget != null)
            {
                HandleExecuteEvents.OnPointerEnter(newEnterTarget, currentPointerData);
            }
        }

        protected HandlePointerData GetLastPointerData(int id)
        {
            HandlePointerData data;
            GetPointerData(id, out data, false);
            return data;
        }

        public bool IsPointerOverHandle(int pointerId)
        {
            var lastPointer = GetLastPointerData(pointerId);
            if (lastPointer != null)
                return lastPointer.pointerEnter != null;

            return false;
        }

        public void ClearSelection()
        {
            var baseEventData = new HandlePointerData();

            foreach (var pointer in m_PointerDataDict.Values)
            {
                // clear all selection
                HandlePointerExitAndEnter(pointer, null);
            }

            m_PointerDataDict.Clear();
            handleSystem.SetSelectedHandle(null, baseEventData);
        }

        protected void DeselectIfSelectionChanged(BaseHandle currentOverHandle, HandlePointerData pointerEvent)
        {
            if (currentOverHandle != handleSystem.currentSelectedHandle)
                handleSystem.SetSelectedHandle(null, pointerEvent);
        }
    }
}

