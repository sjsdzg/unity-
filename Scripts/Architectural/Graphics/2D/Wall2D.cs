using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XFramework.Architectural;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Wall2D : ProceduralGraphic, IGraphicRaycast
    {
        private Wall wall;

        public Wall Wall
        {
            get { return wall; }
            set
            {
                wall = value;
                wall.VerticesChanged += Wall_VerticesChanged;
            }
        }

        private void Wall_VerticesChanged()
        {
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Lines;
            if (wall.Corner0 == null)
            {
                Debug.Log("wall : " + wall.Id);
                Debug.Log("wall.Corner0 == null");
            }
            vh.AddWall2D(Wall, Color);
        }

        public bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result)
        {
            Vector3 wp = Architect.ScreenToWorldPoint(screenPoint);
            Vector2 sp = wp.XZ();

            result = new RaycastInfo()
            {
                gameObject = this.gameObject,
                distance = 0,
                depth = ArchitectSettings.Depth2D.wall,
                worldPosition = wp,
                worldNormal = Vector3.up,
                screenPosition = screenPoint,
            };

            List<Vector2> polygon = new List<Vector2>();
            foreach (var point in wall.Points)
            {
                polygon.Add(point.XZ());
            }
            // 
            return GeometryUtils.PolygonContainsPoint(polygon, sp);
        }

        public override void Press()
        {
            base.Press();
            Selection.Instance.SetSelectedEntityObject(wall);
        }

        public override void DoStateTransition(SelectionState state)
        {
            base.DoStateTransition(state);
            switch (state)
            {
                case SelectionState.Normal:
                    Color = ArchitectSettings.Wall.color2d;
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
