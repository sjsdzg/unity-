using UnityEngine;
using System.Collections;
using XFramework.UIWidgets;
using UnityEngine.UI;

namespace XFramework.Diagram
{
    public class LineWidget : VisualWidget
    {
        [SerializeField]
        private Vector2 p1;
        /// <summary>
        /// p1
        /// </summary>
        public Vector2 P1
        {
            get { return p1; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref p1, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private Vector2 p2;
        /// <summary>
        /// p2
        /// </summary>
        public Vector2 P2
        {
            get { return p2; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref p2, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private float thickness = 1;
        /// <summary>
        /// 厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref thickness, value))
                {
                    SetVerticesDirty();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);

            float width = thickness / Scale;

            vh.Clear();
            vh.AddLine(p1, p2, width, color);
        }
    }
}
