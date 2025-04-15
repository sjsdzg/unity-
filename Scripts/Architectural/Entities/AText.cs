using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    /// <summary>
    /// 文本对象
    /// </summary>
    public class AText : EntityObject
    {
        public override EntityType Type
        {
            get { return EntityType.AText; }
        }

        private Vector3 position;
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

        private string text;
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text
        {
            get { return text; }
            set 
            {
                SetProperty(ref text, value, Constants.text);
            }
        }

        /// <summary>
        /// 文本内容更改时触发
        /// </summary>
        public event Action TextChanged;

        public void OnTextChanged()
        {
            TextChanged?.Invoke();
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.position:
                    OnTransformChanged();
                    break;
                case Constants.text:
                    OnTextChanged();
                    break;
                default:
                    break;
            }
        }

        public override object Clone()
        {
            AText entity = new AText()
            {
                Position = this.position,
            };

            return entity;
        }
    }
}
