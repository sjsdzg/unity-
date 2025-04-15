using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Runtime
{
    public static class HandleExecuteEvents
    {
        public static void OnPointerEnter(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnPointerEnter(pointerData);
        }

        public static void OnPointerExit(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnPointerExit(pointerData);
        }

        public static void OnPointerDown(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnPointerDown(pointerData);
        }

        public static void OnPointerUp(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnPointerUp(pointerData);
        }

        public static void OnPointerClick(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnPointerClick(pointerData);
        }

        public static void OnSelect(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnSelect(pointerData);
        }

        public static void OnDeselect(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnDeselect(pointerData);
        }

        public static void OnBeginDrag(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnBeginDrag(pointerData);
        }

        public static void OnDrag(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnDrag(pointerData);
        }

        public static void OnEndDrag(BaseHandle handle, HandlePointerData pointerData)
        {
            if (handle == null)
                return;

            if (!handle.interactable)
                return;

            handle.OnEndDrag(pointerData);
        }

    }
}
