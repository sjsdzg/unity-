using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Runtime
{
    public enum InputButton : int
    {
        Left = 0,
        Right = 1,
        Middle = 2,
    }

    public class InputUtility
    {
        public static string compositionString
        {
            get { return Input.compositionString; }
        }

        public static IMECompositionMode imeCompositionMode
        {
            get { return Input.imeCompositionMode; }
            set { Input.imeCompositionMode = value; }
        }

        public static Vector2 compositionCursorPos
        {
            get { return Input.compositionCursorPos; }
            set { Input.compositionCursorPos = value; }
        }

        public static bool mousePresent
        {
            get { return Input.mousePresent; }
        }

        public static bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public static bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public static bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public static Vector2 mousePosition
        {
            get { return Input.mousePosition; }
        }

        public static Vector2 mouseScrollDelta
        {
            get { return Input.mouseScrollDelta; }
        }

        public static bool touchSupported
        {
            get { return Input.touchSupported; }
        }

        public static int touchCount
        {
            get { return Input.touchCount; }
        }

        public static Touch GetTouch(int index)
        {
            return Input.GetTouch(index);
        }

        public static float GetAxisRaw(string axisName)
        {
            return Input.GetAxisRaw(axisName);
        }

        public static bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }
    }
}
