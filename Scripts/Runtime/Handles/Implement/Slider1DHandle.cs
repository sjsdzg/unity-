using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Runtime
{
    public class Slider1DHandle : BaseHandle
    {
        /// <summary>
        /// 滑动柄位置
        /// </summary>
        private PropertyWrapper<Vector3> handlePosWrapper;

        /// <summary>
        /// 控制柄的方向，仅用于控制柄渲染。
        /// </summary>
        private Vector3 handleDir;

        /// <summary>
        /// 滑向的方向
        /// </summary>
        private Vector3 slideDir;

        /// <summary>
        /// 控制柄的大小
        /// </summary>
        private PropertyWrapper<float> handleSizeWrapper;

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

        public Slider1DHandle(int id, PropertyWrapper<Vector3> handlePosWrapper, Vector3 handleDir, Vector3 slideDir, PropertyWrapper<float> hanldeSizeWrapper, RuntimeHandles.CapFunction func, ColorBlockRef colorBlock, float snap) 
            : base(id)
        {
            this.handlePosWrapper = handlePosWrapper;
            this.handleDir = handleDir;
            this.slideDir = slideDir;
            this.handleSizeWrapper = hanldeSizeWrapper;
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
                case SelectionStateRef.Selected:
                    m_Color = colorBlock.selectedColor;
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
            startPosition = handlePosWrapper.Value;
            // dragPlane
            dragPlane.SetNormalAndPosition(HandleUtility.Camera.transform.forward, startPosition); 
            HandleUtility.GetPointOnPlane(pointerData.position, dragPlane, out startPointOnPlane);
        }

        public override void OnDrag(HandlePointerData pointerData)
        {
            base.OnDrag(pointerData);
            HandleUtility.GetPointOnPlane(pointerData.position, dragPlane, out currentPointOnPlane);
            Vector3 delta = currentPointOnPlane - startPointOnPlane;
            float dist = Vector3.Dot(delta, slideDir);
            // snap
            dist = HandleUtility.SnapValue(dist, snap);
            handlePosWrapper.Value = startPosition + dist * slideDir;
        }

        public override void OnDraw()
        {
            base.OnDraw();
            if (drawFunc != null)
            {
                drawFunc(handlePosWrapper.Value, Quaternion.FromToRotation(Vector3.up, handleDir), handleSizeWrapper.Value, m_Color);
            }
        }
    }
}
