using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Module;

namespace XFramework.Diagram
{
    public class NodeAnchor
    {
        public Node Node { get; set; }

        public Vector2 Anchor { get; set; }
    }

    public class Node : Unit
    {
        private Vector2 position;
        /// <summary> 
        /// 位置
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { SetProperty(ref position, value, "Position"); }
        }

        private Vector2 sizeDelta;
        /// <summary>
        /// width height
        /// </summary>
        public Vector2 SizeDelta
        {
            get { return sizeDelta; }
            set { SetProperty(ref sizeDelta, value, "SizeDelta"); }
        }

        private List<Vector2> points;
        /// <summary>
        /// 沿边的端点
        /// </summary>
        public List<Vector2> Points
        {
            get { return points; }
            set { points = value; }
        }

        private List<Variable> variables;
        /// <summary>
        /// 变量列表
        /// </summary>
        public List<Variable> Variables
        {
            get { return variables; }
            set { variables = value; }
        }


        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case "Position":
                    OnTransformChanged();
                    break;
                case "SizeDelta":
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }
    }
}
