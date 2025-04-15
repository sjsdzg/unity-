using UnityEngine;
using System.Collections;

namespace XFramework.Architectural
{
    public class Door3D : GraphicObject
    {
        private Door door;
        /// <summary>
        /// 门
        /// </summary>
        public Door Door
        {
            get { return door; }
            set 
            {
                door = value;
                door.TransformChanged += Door_TransformChanged;
            }
        }

        private void Door_TransformChanged()
        {
            SetTranformDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = door.Position;
            transform.rotation = door.Rotation * Quaternion.Euler(-90, 0, 0);
        }
    }
}

