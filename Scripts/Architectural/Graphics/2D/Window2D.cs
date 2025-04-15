using UnityEngine;
using System.Collections;
using XFramework.Math;
using XFramework.Runtime;
using XFramework.Core;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using XFramework.UI;
using System.Collections.Generic;

namespace XFramework.Architectural
{
    public class Window2D : ProceduralGraphic, IGraphicRaycast
    {
        private Window window;
        /// <summary>
        /// 墙角
        /// </summary>
        public Window Window
        {
            get { return window; }
            set
            {
                window = value;
                window.TransformChanged += Window_TransformChanged;
                window.VerticesChanged += Window_VerticesChanged;
            }
        }

        private void Window_TransformChanged()
        {
            SetTranformDirty();
        }

        private void Window_VerticesChanged()
        {
            SetVerticesDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = window.Position - new Vector3(0, window.Bottom + window.Height * 0.5f, 0);
            transform.rotation = window.Rotation;
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Lines;
            vh.AddWindow2D(Window, Color);
        }

        public bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result)
        {
            Vector3 wp = Architect.ScreenToWorldPoint(screenPoint);
            Vector2 sp = wp.XZ();

            result = new RaycastInfo()
            {
                gameObject = this.gameObject,
                distance = 0,
                depth = ArchitectSettings.Depth2D.window,
                worldPosition = wp,
                worldNormal = Vector3.up,
                screenPosition = screenPoint,
            };

            Vector2 center = window.ToPoint2();
            Vector2 direction = window.ToVector2();
            if (GeometryUtils.RectangleContainsPoint(center, window.Length, window.Thickness, direction, sp))
            {
                return true;
            }

            return false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                List<ContextMenuParameter> parameters = new List<ContextMenuParameter>
                {
                    new ContextMenuParameter("编辑位置", x =>
                    {
                        //先删除窗户
                        if (Selection.Instance.currentSelectedEntityObject.Equals(window))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        Architect.RemoveEntity(window);
                        GraphicManager.DestoryGraphic(window);
                        //再添加窗户
                         Architect.Instance.ActiveTool = Architect.Instance.GetTool<WindowCreateTool>();
                        Architect.Instance.ActiveTool.Init(ToolArgs.Empty);
                    }),
                    new ContextMenuParameter("删除", x=>
                    {
                        if (Selection.Instance.currentSelectedEntityObject.Equals(window))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        Architect.RemoveEntity(window);
                        GraphicManager.DestoryGraphic(window);
                    }),
                    new ContextMenuParameter("关闭", x => { ContextMenuEx.Instance.Hide(); })
                };
                ContextMenuEx.Instance.Show(gameObject, parameters);
            }
        }

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(window);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    Color = ArchitectSettings.Window.color2d;
                    break;
                case SelectionState.Highlighted:
                    Color = ArchitectSettings.ColorBlock.highlightedColor;
                    break;
                case SelectionState.Selected:
                    Color = ArchitectSettings.ColorBlock.selectedColor;
                    break;
                default:
                    break;
            }
        }
    }
}

