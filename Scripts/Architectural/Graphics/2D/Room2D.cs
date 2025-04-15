using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using XFramework.Math;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    public class Room2D : ProceduralGraphic, IGraphicRaycast
    {
        private Room room;
        /// <summary>
        /// 房间
        /// </summary>
        public Room Room
        {
            get { return room; }
            set 
            { 
                room = value;
                room.ActiveChanged += Room_ActiveChanged;
                room.NameChanged += Room_NameChanged;
                room.TransformChanged += Room_TransformChanged;
                room.VerticesChanged += Room_VerticesChanged;
            }
        }


        private void Room_ActiveChanged()
        {
            SetActiveDirty();
        }

        private void Room_NameChanged()
        {
            SetNameDirty();
        }

        private void Room_TransformChanged()
        {
            SetTranformDirty();
        }

        private void Room_VerticesChanged()
        {
            SetVerticesDirty();
        }

        private TextMeshPro m_Text;
        /// <summary>
        /// 显示文本
        /// </summary>
        public TextMeshPro Text
        {
            get
            {
                if (m_Text == null)
                {
                    GameObject textPrefab = Resources.Load<GameObject>("Prefabs/Text (TMP)");
                    GameObject textGameObject = Instantiate<GameObject>(textPrefab, transform);
                    m_Text = textGameObject.GetComponent<TextMeshPro>();
                }
                return m_Text;
            }
        }

        protected override void UpdateActive()
        {
            base.UpdateActive();
            gameObject.SetActive(room.Active);
        }

        protected override void UpdateName()
        {
            base.UpdateName();
            Text.text = string.Format("{0}\n<color=yellow>{1}m<sup>2</sup></color>", room.Name, room.Area.ToString("F0"));
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            Text.transform.position = room.GetVisualPoint();
            Text.text = string.Format("{0}\n<color=yellow>{1}m<sup>2</sup></color>", room.Name, room.Area.ToString("F0"));
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Triangles;
            vh.AddRoom2D(room, Color);
        }

        public bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result)
        {
            Vector3 wp = Architect.ScreenToWorldPoint(screenPoint);
            Vector2 sp = wp.XZ();

            result = new RaycastInfo()
            {
                gameObject = this.gameObject,
                distance = 0,
                depth = ArchitectSettings.Depth2D.room,
                worldPosition = wp,
                worldNormal = Vector3.up,
                screenPosition = screenPoint,
            };

            List<List<Vector2>> complexPolygon = new List<List<Vector2>>();
            complexPolygon.Add(room.Contour);
            complexPolygon.AddRange(room.InnerContours);

            return GeometryUtils.ComplexPolygonContainsPoint(complexPolygon, sp);
        }

        //private void LateUpdate()
        //{
        //    float size = RuntimeHandles.GetHandleSize(transform.position);
        //    size *= 2f;
        //    size = Mathf.Clamp(size, 0, room.Area * 0.03f);
        //    TextMesh.transform.localScale = new Vector3(size, size, size);
        //}

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(room);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    Color = ArchitectSettings.Room.color2d;
                    break;
                case SelectionState.Highlighted:
                    Color32 highlightedColor = ArchitectSettings.ColorBlock.highlightedColor;
                    highlightedColor.a = 25;
                    Color = highlightedColor;
                    break;
                case SelectionState.Selected:
                    Color32 selectedColor = ArchitectSettings.ColorBlock.selectedColor;
                    selectedColor.a = 25;
                    Color = selectedColor;
                    break;
                default:
                    break;
            }
        }
    }
}
