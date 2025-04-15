using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{

    public class PropertyWrapper
    {
        public Func<object> Getter { get; set; }
        public Action<object> Setter { get; set; }

        public PropertyWrapper(Func<object> getter, Action<object> setter)
        {
            Getter = getter;
            Setter = setter;
        }

        public PropertyWrapper(Func<object> getter)
        {
            Getter = getter;
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                if (Getter == null)
                    return null;
                //获取值
                return Getter();
            }
            set
            {
                if (Setter == null)
                    return;
                //赋值
                Setter(value);
            }
        }
    }

    public class PropertyWrapper<T>
    {
        public static PropertyWrapper<T> Default
        {
            get
            {
                var c = new PropertyWrapper<T>(() => default(T));
                return c;
            }
        }

        public Func<T> Getter { get; set; }
        public Action<T> Setter { get; set; }

        public PropertyWrapper(Func<T> getter, Action<T> setter)
        {
            Getter = getter;
            Setter = setter;
        }

        public PropertyWrapper(Func<T> getter)
        {
            Getter = getter;
        }

        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get
            {
                if (Getter == null)
                    return default(T);
                //获取值
                return Getter();
            }
            set
            {
                if (Setter == null)
                    return;
                //赋值
                Setter(value);
            }
        }

        public T GetValue()
        {
            if (Getter == null)
                return default(T);
            //获取值
            return Getter();
        }

        public void SetValue(T value)
        { 
            if (Setter == null)
                return;
            //赋值
            Setter(value);
        }
    }

}
