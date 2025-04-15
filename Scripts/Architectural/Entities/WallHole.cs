using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;
using XFramework.Math;

namespace XFramework.Architectural
{
    public abstract class WallHole : EntityObject
    {
        protected Wall wall;
        /// <summary>
        /// 
        /// </summary>
        public Wall Wall
        {
            get { return wall; }
            set 
            {
                SetProperty(ref wall, value, Constants.wall);
            }
        }

        protected Vector3 position;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                SetProperty(ref position, value, Constants.position);
            }
        }

        protected Quaternion rotation;
        /// <summary>
        /// 旋转
        /// </summary>
        public Quaternion Rotation
        {
            get { return rotation; }
            set 
            {
                SetProperty(ref rotation, value, Constants.rotation);
            }
        }

        protected float length = 1f;
        /// <summary>
        /// 长度
        /// </summary>
        public float Length
        {
            get { return length; }
            set { SetProperty(ref length, value, Constants.length); }
        }

        protected float height = 1f;
        /// <summary>
        /// 高度
        /// </summary>
        public float Height
        {
            get { return height; }
            set { SetProperty(ref height, value, Constants.height); }
        }

        protected float thickness = 0.12f;
        /// <summary>
        /// 厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set
            {
                SetProperty(ref thickness, value, Constants.thickness);
            }
        }

        protected float bottom = 0f;
        /// <summary>
        /// 离地距离
        /// </summary>
        public float Bottom
        {
            get { return bottom; }
            set 
            {
                SetProperty(ref bottom, value, Constants.bottom);
            }
        }

        public WallHole()
        {

        }

        public WallHole(Wall wall)
        {
            this.wall = wall;
            wall.AddHole(this);

            wall.PropertyChanged += Wall_PropertyChanged;
            wall.Corner0.PropertyChanged += Corner0_PropertyChanged;
            wall.Corner1.PropertyChanged += Corner1_PropertyChanged;
        }


        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.wall:
                    PropertyChangedUtils.RemoveEvent(oldValue, Wall_PropertyChanged);
                    PropertyChangedUtils.AddEvent(newValue, Wall_PropertyChanged);

                    Wall oldWall = (Wall)oldValue;
                    if (oldWall != null)
                    {
                        PropertyChangedUtils.RemoveEvent(oldWall.Corner0, Corner0_PropertyChanged);
                        PropertyChangedUtils.RemoveEvent(oldWall.Corner1, Corner1_PropertyChanged);
                        oldWall.RemoveHole(this);
                    }

                    Wall newWall = (Wall)newValue;
                    if (newWall != null)
                    {
                        PropertyChangedUtils.AddEvent(newWall.Corner0, Corner0_PropertyChanged);
                        PropertyChangedUtils.AddEvent(newWall.Corner1, Corner1_PropertyChanged);
                        newWall.AddHole(this);
                    }
                    break;
                case Constants.position:
                    OnTransformChanged();
                    break;
                case Constants.rotation:
                    OnTransformChanged();
                    break;
                case Constants.length:
                    OnVerticesChanged();
                    break;
                case Constants.height:
                    OnVerticesChanged();
                    break;
                case Constants.thickness:
                    OnVerticesChanged();
                    break;
                case Constants.bottom:
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }

        private void Corner0_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            switch (e.PropertyName)
            {
                case Constants.position:
                    UpdatePositionAndRotation(wall.Corner1);
                    break;
                default:
                    break;
            }
        }

        private void Corner1_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            switch (e.PropertyName)
            {
                case Constants.position:
                    UpdatePositionAndRotation(wall.Corner0);
                    break;
                default:
                    break;
            }
        }

        protected virtual void Wall_PropertyChanged(object sender, PropertyChangedArgs e)
        {

        }

        /// <summary>
        /// 更新位置和旋转
        /// </summary>
        public virtual void UpdatePositionAndRotation(Corner corner)
        {
            float distance = Vector3.Distance(corner.Position.XOZ(), Position.XOZ());
            Vector3 normal = (wall.Another(corner).Position - corner.Position).XOZ().normalized;
            Position = corner.Position + normal * distance + new Vector3(0, Bottom + Height * 0.5f, 0);
            Rotation = Quaternion.FromToRotation(Vector3.right, wall.ToVector2(corner).XOZ());
        }

        public override void Unbind()
        {
            base.Unbind();
            wall.PropertyChanged -= Wall_PropertyChanged;
            wall.Corner0.PropertyChanged -= Corner0_PropertyChanged;
            wall.Corner1.PropertyChanged -= Corner1_PropertyChanged;
            wall.RemoveHole(this);
        }
    }
}