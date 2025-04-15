using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using XFramework.Math;

namespace XFramework.Architectural
{
    public class Room3D : ProceduralGraphic
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
                room.VerticesChanged += Room_VerticesChanged;
            }
        }

        private void Room_ActiveChanged()
        {
            //SetActiveDirty();
        }

        private void Room_VerticesChanged()
        {
            SetVerticesDirty();
        }

        protected override void UpdateActive()
        {
            //base.UpdateActive();
            //gameObject.SetActive(room.Active);
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Triangles;
            vh.AddRoom3D(room, Color);
        }
    }
}
