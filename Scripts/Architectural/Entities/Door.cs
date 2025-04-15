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
    /// <summary>
    /// 门类型
    /// </summary>
    [SerializeField]
    public enum DoorType
    {
        /// <summary>
        /// 门
        /// </summary>
        Single,
        /// <summary>
        /// 双开门
        /// </summary>
        Double,
        /// <summary>
        /// 旋转门
        /// </summary>
        Revolving,
    }

    public class Door : WallHole
    {
        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type
        {
            get
            {
                return EntityType.Door;
            }
        }

        /// <summary>
        /// 门类型
        /// </summary>
        public DoorType DoorType { get; set; }

        private int flip;
        /// <summary>
        /// 翻转
        /// </summary>
        public int Flip
        {
            get { return flip; }
            set
            {
                SetProperty(ref flip, value, Constants.flip);
            }
        }

        public Door()
        {
            Special = "建筑";
        }

        public Door(Wall wall) : base(wall)
        {
            Special = "建筑";
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.flip:
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }

        public override object Clone()
        {
            Door entity = new Door()
            {
                position = this.Position,
                rotation = this.Rotation,
                length = this.Length,
                thickness = this.Thickness,
                bottom = this.Bottom,
                DoorType = this.DoorType,
                flip = this.flip,
            };

            return entity;
        }
    }
}
