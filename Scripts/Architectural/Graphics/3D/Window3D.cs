using UnityEngine;
using System.Collections;

namespace XFramework.Architectural
{
    public class Window3D : GraphicObject
    {
        private Window window;
        /// <summary>
        /// 门
        /// </summary>
        public Window Window
        {
            get { return window; }
            set 
            {
                window = value;
                window.TransformChanged += Window_TransformChanged;
            }
        }

        private void Window_TransformChanged()
        {
            SetTranformDirty();
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            transform.position = window.Position;
            transform.rotation = window.Rotation;
        }
    }
}

