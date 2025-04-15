using UnityEngine;
using System.Collections;
using XFramework.Math;
using XFramework.Runtime;
using XFramework.Core;

namespace XFramework.Architectural
{
    public class Corner2D : ProceduralGraphic
    {
        private Corner corner;
        /// <summary>
        /// 墙角
        /// </summary>
        public Corner Corner
        {
            get { return corner; }
            set
            {
                corner = value;
                corner.TransformChanged += Corner_TransformChanged;
            }
        }

        private void Corner_TransformChanged()
        {
            SetTranformDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = corner.Position;
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Triangles;

            vh.AddRectWithOutline(Vector3.zero, Quaternion.identity, Vector2.one * 2.5f, 0.25f, Color.blue, Color.grey);
        }

        

        private void LateUpdate()
        {
            float size = RuntimeHandles.GetHandleSize(corner.Position);
            transform.localScale = new Vector3(size, size, size);
        }
    }
}

