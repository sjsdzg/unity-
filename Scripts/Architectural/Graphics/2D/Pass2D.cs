using UnityEngine;
using System.Collections;
using XFramework.Math;
using XFramework.Runtime;
using XFramework.Core;
using UnityEngine.Events;
using System;
using XFramework.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace XFramework.Architectural
{
    public class Pass2D : ProceduralGraphic, IGraphicRaycast
    {
        private Pass pass;
        /// <summary>
        /// 垭口
        /// </summary>
        public Pass Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                pass.TransformChanged += Window_TransformChanged;
                pass.VerticesChanged += Window_VerticesChanged;
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
            transform.position = pass.Position - new Vector3(0, pass.Bottom + pass.Height * 0.5f, 0);
            transform.rotation = pass.Rotation;
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Lines;
            vh.AddPass2D(Pass, Color);
        }

        public bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result)
        {
            Vector3 wp = Architect.ScreenToWorldPoint(screenPoint);
            Vector2 sp = wp.XZ();

            result = new RaycastInfo()
            {
                gameObject = this.gameObject,
                distance = 0,
                depth = ArchitectSettings.Depth2D.pass,
                worldPosition = wp,
                worldNormal = Vector3.up,
                screenPosition = screenPoint,
            };

            Vector2 center = pass.ToPoint2();
            Vector2 direction = pass.ToVector2();
            if (GeometryUtils.RectangleContainsPoint(center, pass.Length, pass.Thickness, direction, sp))
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
                    new ContextMenuParameter("删除", x=>
                    {
                        if (Selection.Instance.currentSelectedEntityObject.Equals(pass))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        Architect.RemoveEntity(pass);
                        GraphicManager.DestoryGraphic(pass);
                    }),
                    new ContextMenuParameter("关闭", x => { ContextMenuEx.Instance.Hide(); })
                };
                ContextMenuEx.Instance.Show(gameObject, parameters);
            }
        }

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(pass);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    Color = ArchitectSettings.Pass.color2d;
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

