using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public class BindableProperty<T>
    {
        public class PropertyChangedArgs
        {
            /// <summary>
            /// 原始值
            /// </summary>
            public T OldValue { get; set; }

            /// <summary>
            /// 新值
            /// </summary>
            public T NewValue { get; set; }

            public PropertyChangedArgs()
            {

            }

            public PropertyChangedArgs(T oldValue, T newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        public delegate void PropertyChangedHandler(object sender, PropertyChangedArgs e);

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        public PropertyChangedHandler OnPropertyChanged;

        private T _value;
        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get { return _value; }
            set 
            {
                if (!Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    NotifyPropertyChanged(old, _value);
                }
            }
        }

        /// <summary>
        /// 通知属性值更改
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void NotifyPropertyChanged(T oldValue, T newValue)
        {
            OnPropertyChanged?.Invoke(this, new PropertyChangedArgs(oldValue, newValue));
        }



        public override string ToString()
        {
            return Value != null ? Value.ToString() : "null";
        }

        //public 
    }
}
