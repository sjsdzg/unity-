using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(string propertyName = null)
        {
            return typeof(T).GetProperty(propertyName);
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName = null)
        {
            return type.GetProperty(propertyName);
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(object target, string propertyName = null)
        {
            return target.GetType().GetProperty(propertyName);
        }
    }
}
