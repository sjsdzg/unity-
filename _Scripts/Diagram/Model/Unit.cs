using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;

namespace XFramework.Diagram
{
    public class Unit : IPropertyChanged, IEquatable<Unit>
    {
        public event PropertyChangedHandler PropertyChanged;

        public void RemoveAllEvent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        private bool active = true;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active
        {
            get { return active; }
            set
            {
                SetProperty(ref active, value, "active");
            }
        }

        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value, "name");
            }
        }

        //private float lossyScale = 1;
        ///// <summary>
        ///// 真实缩放值
        ///// </summary>
        //public float LossyScale
        //{
        //    get { return lossyScale; }
        //    set 
        //    {
        //        SetProperty(ref lossyScale, value, "lossyScale");
        //    }
        //}

        /// <summary>
        /// 是否激活 更改时触发
        /// </summary>
        public event Action ActiveChanged;

        /// <summary>
        /// 名称 更改时触发
        /// </summary>
        public event Action NameChanged;

        /// <summary>
        /// Transform 更改时触发
        /// </summary>
        public event Action TransformChanged;

        /// <summary>
        /// Vertices 更改时触发
        /// </summary>
        public event Action VerticesChanged;

        public Unit()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            if (object.Equals(storage, value))
                return;

            T oldValue = storage;
            storage = value;
            OnPropertyChanged(propertyName, oldValue, storage);
        }

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedArgs(propertyName, oldValue, newValue));
            switch (propertyName)
            {
                case "active":
                    OnActiveChanged();
                    break;
                case "name":
                    OnNameChanged();
                    break;
                //case "lossyScale":
                //    OnVerticesChanged();
                //    break;
                default:
                    break;
            }
        }

        public void OnActiveChanged()
        {
            ActiveChanged?.Invoke();
        }

        public void OnNameChanged()
        {
            NameChanged?.Invoke();
        }

        public void OnTransformChanged()
        {
            TransformChanged?.Invoke();
        }

        public void OnVerticesChanged()
        {
            VerticesChanged?.Invoke();
        }

        public bool Equals(Unit other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id);
        }
    }
}
