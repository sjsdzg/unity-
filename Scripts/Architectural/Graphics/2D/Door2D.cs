using UnityEngine;
using System.Collections;
using XFramework.Math;
using XFramework.Runtime;
using XFramework.Core;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using XFramework.UI;

namespace XFramework.Architectural
{
    public class Door2D : ProceduralGraphic, IGraphicRaycast
    {
        private Door door;
        /// <summary>
        /// 墙角
        /// </summary>
        public Door Door
        {
            get { return door; }
            set
            {
                door = value;

                door.TransformChanged += Door_TransformChanged;
                door.VerticesChanged += Door_VerticesChanged;
            }
        }

        private bool requireSyncArcs = false;


        private readonly List<Arc2> arcs = new List<Arc2>();
        /// <summary>
        /// 包含的圆弧列表
        /// </summary>
        public List<Arc2> Arcs
        {
            get
            {
                if (requireSyncArcs)
                {
                    SyncArcs();
                }

                return arcs;
            }
        }

        private void Door_TransformChanged()
        {
            SetTranformDirty();
        }

        private void Door_VerticesChanged()
        {
            SetVerticesDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = door.Position - new Vector3(0, door.Bottom + door.Height * 0.5f, 0);
            transform.rotation = door.Rotation;
            switch (door.DoorType)
            {
                case DoorType.Single:
                    if (door.Flip == 0)
                        transform.localScale = new Vector3(1, 1, 1);
                    else if (door.Flip == 1)
                        transform.localScale = new Vector3(1, 1, -1);
                    else if (door.Flip == 2)
                        transform.localScale = new Vector3(-1, 1, -1);
                    else if (door.Flip == 3)
                        transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case DoorType.Double:
                    if (door.Flip == 0)
                        transform.localScale = new Vector3(1, 1, 1);
                    else if (door.Flip == 1)
                        transform.localScale = new Vector3(1, 1, -1);
                    break;
                default:
                    break;
            }
            // 同步
            requireSyncArcs = true;
        }

        public void FlipDoor()
        {
            int div = 4;
            if (door.DoorType == DoorType.Single)
                div = 4;
            else if (door.DoorType == DoorType.Double)
                div = 2;

            door.Flip = (door.Flip + 1) % div;
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Lines;
            vh.AddDoor2D(Door, Color);
            // 同步
            requireSyncArcs = true;
        }

        public bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result)
        {
            Vector3 wp = Architect.ScreenToWorldPoint(screenPoint);
            Vector2 sp = wp.XZ();

            result = new RaycastInfo()
            {
                gameObject = this.gameObject,
                distance = 0,
                depth = ArchitectSettings.Depth2D.door,
                worldPosition = wp,
                worldNormal = Vector3.up,
                screenPosition = screenPoint,
            };

            Vector2 center = door.ToPoint2();
            if (door.Wall == null)
            {
                return false;
            }
            Vector2 direction = door.ToVector2();
            if (GeometryUtils.RectangleContainsPoint(center, door.Length, door.Thickness, direction, sp))
            {
                return true;
            }

            bool inside = false;
            foreach (var arc in Arcs)
            {
                if (GeometryUtils.CircularSectorContainsPoint(arc, sp))
                {
                    inside = true;
                    break;
                }
            }

            return inside;
        }

        /// <summary>
        /// 同步圆弧列表
        /// </summary>
        public void SyncArcs()
        {
            arcs.Clear();

            Vector2 direction = door.ToVector2();
            float rad = Vector2.SignedAngle(Vector2.right, direction) * Mathf.Deg2Rad;
            Vector2 p1 = door.ToPoint2() - direction * door.Length * 0.5f;
            Vector2 p2 = door.ToPoint2() + direction * door.Length * 0.5f;

            switch (door.DoorType)
            {
                case DoorType.Single:
                    Arc2 arc = new Arc2();
                    arc.radius = door.Length;
                    arc.sweepAngle = Mathf.PI * 0.5f;
                    if (door.Flip == 0)
                    {
                        arc.center = p1;
                        arc.startAngle = rad;
                    }
                    else if (door.Flip == 1)
                    {
                        arc.center = p1;
                        arc.startAngle = rad + Mathf.PI * 1.5f;
                    }
                    else if (door.Flip == 2)
                    {
                        arc.center = p2;
                        arc.startAngle = rad + Mathf.PI;
                    }
                    else if (door.Flip == 3)
                    {
                        arc.center = p2;
                        arc.startAngle = rad + Mathf.PI * 0.5f;
                    }
                    // add
                    arcs.Add(arc);
                    break;
                case DoorType.Double:
                    Arc2 arc1 = new Arc2();
                    Arc2 arc2 = new Arc2();
                    if (door.Flip == 0)
                    {
                        arc1.center = p1;
                        arc1.radius = door.Length * 0.5f;
                        arc1.startAngle = rad;
                        arc1.sweepAngle = Mathf.PI * 0.5f;

                        arc2.center = p2;
                        arc2.radius = door.Length * 0.5f;
                        arc2.startAngle = rad + Mathf.PI * 0.5f;
                        arc2.sweepAngle = Mathf.PI * 0.5f;
                    }
                    else if (door.Flip == 1)
                    {
                        arc1.center = p1;
                        arc1.radius = door.Length * 0.5f;
                        arc1.startAngle = rad + Mathf.PI * 1.5f;
                        arc1.sweepAngle = Mathf.PI * 0.5f;

                        arc2.center = p2;
                        arc2.radius = door.Length * 0.5f;
                        arc2.startAngle = rad + Mathf.PI;
                        arc2.sweepAngle = Mathf.PI * 0.5f;
                    }
                    // add
                    arcs.Add(arc1);
                    arcs.Add(arc2);
                    break;
                case DoorType.Revolving:
                    break;
                default:
                    break;
            }
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
                        if (Selection.Instance.currentSelectedEntityObject.Equals(door))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        Architect.RemoveEntity(door);
                        GraphicManager.DestoryGraphic(door);
                    }),
                    new ContextMenuParameter("翻转", x=>
                    {
                        FlipDoor();
                    }),
                    new ContextMenuParameter("编辑位置", x=>
                    {
                        //先删除
                        if (Selection.Instance.currentSelectedEntityObject.Equals(door))
                        {
                            Selection.Instance.currentSelectedEntityObject = null;
                        }
                        Architect.RemoveEntity(door);
                        GraphicManager.DestoryGraphic(door);
                        //重新添加
                        //判断门的类型
                        Architect.Instance.ActiveTool = Architect.Instance.GetTool<DoorCreateTool>();
                        Architect.Instance.ActiveTool.Init(new DoorCreateToolArgs(door.DoorType));
                    }),
                    new ContextMenuParameter("关闭", x => { ContextMenuEx.Instance.Hide(); })
                };
                ContextMenuEx.Instance.Show(gameObject, parameters);
            }
        }

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(door);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    Color = ArchitectSettings.Door.color2d;
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

