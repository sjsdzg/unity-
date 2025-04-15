using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public class PropertyChangedArgs : EventArgs
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 原始值
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; set; }

        public PropertyChangedArgs(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public delegate void PropertyChangedHandler(object sender, PropertyChangedArgs e);

    public interface IPropertyChanged
    {
        event PropertyChangedHandler PropertyChanged;

        void RemoveAllEvent();
    }

    public static class PropertyChangedUtils
    {
        /// <summary>
        /// 添加属性更改事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="handler"></param>
        public static void AddEvent(object obj, PropertyChangedHandler handler)
        {
            if (obj == null)
                return;

            IPropertyChanged notify = obj as IPropertyChanged;

            if (notify == null)
                return;

            notify.PropertyChanged += handler;
        }

        /// <summary>
        /// 移除属性更改事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="handler"></param>
        public static void RemoveEvent(object obj, PropertyChangedHandler handler)
        {
            if (obj == null)
                return;

            IPropertyChanged notify = obj as IPropertyChanged;

            if (notify == null)
                return;

            notify.PropertyChanged -= handler;
        }

        public static void RemoveAllEvents(object obj)
        {
            if (obj == null)
                return;

            IPropertyChanged notify = obj as IPropertyChanged;

            if (notify == null) 
                return;

            notify.RemoveAllEvent();
        }

    }

}
