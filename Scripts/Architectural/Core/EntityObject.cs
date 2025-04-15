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
    /// 元件数据
    /// </summary>
    public abstract class EntityObject : IEntityObject, IPropertyChanged, IEquatable<EntityObject>, ICloneable
    {
        /// <summary>
        /// 属性更改时，触发
        /// </summary>
        public event PropertyChangedHandler PropertyChanged;

        /// <summary>
        /// 元件类型
        /// </summary>
        public abstract EntityType Type { get; }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Special { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public EntityObject Owner { get; set; }

        private bool active = true;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active
        {
            get { return active; }
            set 
            { 
                SetProperty(ref active, value, Constants.active);
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
                SetProperty(ref name, value, Constants.name);
            }
        }

        /// <summary>
        /// 是否释放
        /// </summary>
        public bool Disposed { get; set; }

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

        /// <summary>
        /// Material  更改时触发
        /// </summary>
        public event Action MaterialChanged;

        public EntityObject()
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
                case Constants.active:
                    OnActiveChanged();
                    break;
                case Constants.name:
                    OnNameChanged();
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void RemoveAllEvent()
        {
            Delegate[] delegates = PropertyChanged.GetInvocationList();
            foreach (var del in delegates)
            {
                PropertyChanged -= (PropertyChangedHandler)del;
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

        public void OnMaterialChanged()
        {
            MaterialChanged?.Invoke();
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(EntityObject other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id);
        }


        /// <summary>
        /// 绑定关系
        /// </summary>
        public virtual void Bind()
        {

        }

        /// <summary>
        /// 解除绑定关系
        /// </summary>
        public virtual void Unbind()
        {
            
        }

        /// <summary>
        /// 克隆，创建一个新的实例。
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
    }
}
