using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{
    /// <summary>
    /// 二维滑动柄
    /// </summary>
    public class Slider2DHandle : BaseHandle
    {
        /// <summary>
        /// 滑动柄位置
        /// </summary>
        private PropertyWrapper<Vector3> hanldePosWrapper;

        /// <summary>
        /// 控制柄的方向，仅用于控制柄渲染。
        /// </summary>
        private Vector3 handleDir;

        /// <summary>
        /// 控制柄的偏移
        /// </summary>
        private PropertyWrapper<Vector3> offsetWrapper;

        /// <summary>
        /// 滑动第一方向
        /// </summary>
        private Vector3 slideDir1;

        /// <summary>
        /// 滑动第二方向
        /// </summary>
        private Vector3 slideDir2;

        /// <summary>
        /// 控制柄的大小
        /// </summary>
        private PropertyWrapper<float> hanldeSizeWrapper;

        /// <summary>
        /// 调用函数来用于实际的绘制
        /// </summary>
        private RuntimeHandles.CapFunction drawFunc;

        /// <summary>
        /// 使用捕捉
        /// </summary>
        private float snap;

        /// <summary>
        /// 颜色块
        /// </summary>
        private ColorBlockRef colorBlock;

        /// <summary>
        /// 当前
        /// </summary>
        private Color m_Color;

        /// <summary>
        /// 起始位置
        /// </summary>
        private Vector3 startPosition;

        /// <summary>
        /// 拖拽平面
        /// </summary>
        private Plane dragPlane = new Plane(Vector3.up, Vector3.zero);

        /// <summary>
        /// 拖拽开始，鼠标在平面上的位置
        /// </summary>
        private Vector3 startPointOnPlane;

        /// <summary>
        /// 当前，鼠标在平面上的位置
        /// </summary>
        private Vector3 currentPointOnPlane;

        public Slider2DHandle(int id, PropertyWrapper<Vector3> hanldePosWrapper, PropertyWrapper<Vector3> offsetWrapper, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, PropertyWrapper<float> handleSizeWrapper, RuntimeHandles.CapFunction func, ColorBlockRef colorBlock, float snap) 
            : base(id)
        {
            this.hanldePosWrapper = hanldePosWrapper;
            this.handleDir = handleDir;
            this.offsetWrapper = offsetWrapper;
            this.slideDir1 = slideDir1;
            this.slideDir2 = slideDir2;
            this.hanldeSizeWrapper = handleSizeWrapper;
            this.drawFunc = func;
            this.colorBlock = colorBlock;
            this.snap = snap;
        }

        protected override void DoStateTransition(SelectionStateRef state, bool instant)
        {
            base.DoStateTransition(state, instant);
            switch (state)
            {
                case SelectionStateRef.Normal:
                    m_Color = colorBlock.normalColor;
                    break;
                case SelectionStateRef.Highlighted:
                    m_Color = colorBlock.highlightedColor;
                    break;
                case SelectionStateRef.Pressed:
                    m_Color = colorBlock.pressedColor;
                    break;
                case SelectionStateRef.Disabled:
                    m_Color = colorBlock.disabledColor;
                    break;
                default:
                    break;
            }
        }

        public override void OnBeginDrag(HandlePointerData pointerData)
        {
            base.OnBeginDrag(pointerData);
            startPosition = hanldePosWrapper.Value;
            // dragPlane
            Vector3 normal = Vector3.Cross(slideDir1, slideDir2);
            dragPlane.SetNormalAndPosition(normal, startPosition); 
            HandleUtility.GetPointOnPlane(pointerData.position, dragPlane, out startPointOnPlane);
        }

        public override void OnDrag(HandlePointerData pointerData)
        {
            base.OnDrag(pointerData);
            HandleUtility.GetPointOnPlane(pointerData.position, dragPlane, out currentPointOnPlane);
            Vector3 delta = currentPointOnPlane - startPointOnPlane;
            float dist1 = Vector3.Dot(delta, slideDir1);
            float dist2 = Vector3.Dot(delta, slideDir2);

            dist1 = HandleUtility.SnapValue(dist1, snap);
            dist2 = HandleUtility.SnapValue(dist2, snap);

            hanldePosWrapper.Value = startPosition + dist1 * slideDir1 + dist2 * slideDir2;
        }

        public override void OnDraw()
        {
            base.OnDraw();
            if (drawFunc != null)
            {
                drawFunc(hanldePosWrapper.Value + offsetWrapper.Value, Quaternion.FromToRotation(Vector3.up, handleDir), hanldeSizeWrapper.Value, m_Color);
            }
        }
    }
}
