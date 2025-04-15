using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Wall3D : ProceduralGraphic
    {
        private Wall wall;
        /// <summary>
        /// 墙体
        /// </summary>
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
            vh.MeshTopology = MeshTopology.Triangles;
            vh.AddWall3D(wall, Color);
        }
    }
}                                                  