using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Architectural
{
    /// <summary>
    /// 垭口
    /// </summary>
    public class Pass : WallHole
    {
        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type 
        { 
            get 
            {
                return EntityType.Pass; 
            } 
        }

        public Pass()
        {
            Special = "建筑";
        }

        public Pass(Wall wall) : base(wall)
        {
            Special = "建筑";
        }

        protected override void Wall_PropertyChanged(object sender, PropertyChangedArgs e)
        {
            base.Wall_PropertyChanged(sender, e);
            switch (e.PropertyName)
            {
                case Constants.thickness:
                    OnVerticesChanged();
                    break;
                default:
                    break;
            }
        }

        public override object Clone()
        {
            Pass entity = new Pass()
            {
                position = this.Position,
                rotation = this.Rotation,
                length = this.Length,
                thickness = this.Thickness,
                bottom = this.Bottom,
            };

            return entity;
        }
    }
}
