using UnityEngine;

namespace XFramework.Architectural
{ 
    public class InputUtils
    {
        /// <summary>
        /// 鼠标左键
        /// </summary>
        public const int left_button = 0;
        /// <summary>
        /// 鼠标右键
        /// </summary>
        public const int right_button = 1;
        /// <summary>
        /// 鼠标中建
        /// </summary>
        public const int middle_button = 2;
        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        public const string mouse_scroll_wheel = "Mouse ScrollWheel";
        /// <summary>
        /// 鼠标横向移动
        /// </summary>
        public const string mouse_x = "Mouse X";
        /// <summary>
        /// 鼠标纵向移动
        /// </summary>
        public const string mouse_y = "Mouse Y";

        public static Vector2 GetAxisMouseXY()
        {
            Vector2 vector = new Vector2();
            vector.x = Input.GetAxis(InputUtils.mouse_x);
            vector.y = Input.GetAxis(InputUtils.mouse_y);
            return vector;
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

        public static float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }
    }
}