using UnityEngine;

namespace XFramework.Architectural
{
    public class Equipment : EntityObject
    {
        /// <summary>
        /// 元件类型
        /// </summary>
        public override EntityType Type
        {
            get
            {
                return EntityType.Equipment;
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

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        public Equipment()
        {
            Special = "设备";
        }

        public Equipment(string name, string address)
        {
            Special = "设备";
            Name = name;
            Address = address;
        }

        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            switch (propertyName)
            {
                case Constants.position:
                    OnTransformChanged();
                    break;
                case Constants.rotation:
                    OnTransformChanged();
                    break;
                default:
                    break;
            }
        }

        public override object Clone()
        {
            Equipment entity = new Equipment()
            {
                Name = this.Name,
                position = this.position,
                rotation = this.rotation,
                Address = this.Address,
            };

            return entity;
        }
    }
}                                                  