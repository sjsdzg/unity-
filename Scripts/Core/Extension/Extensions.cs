using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public static class Extensions
    {
        public static T Min<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            if (source == null || selector == null)
            {
                return default;
            }

            T result = default;
            float k = float.MaxValue;
            foreach (var item in source)
            {
                float temp = selector(item);
                if (temp <= k)
                {
                    k = temp;
                    result = item;
                }
            }

            return result;
        }

        public static T Max<T>(this IEnumerable<T> source, Func<T, float> selector)
        {
            if (source == null || selector == null)
            {
                return default;
            }

            T result = default;
            float k = float.MinValue;
            foreach (var item in source)
            {
                float temp = selector(item);
                if (temp >= k)
                {
                    k = temp;
                    result = item;
                }
            }

            return result;
        }

        /// <summary>
        /// 交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origin"></param>
        /// <param name="other"></param>
        /// <param name="results"></param>
        public static void Intersect<T>(this IList<T> origin, IList<T> other, ref List<T> results)
        {
            foreach (var item in origin)
            {
                if (other.Contains(item))
                {
                    results.Add(item);
                }
            }
        }

        /// <summary>
        /// 差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origin"></param>
        /// <param name="other"></param>
        /// <param name="results"></param>
        public static void Except<T>(this IList<T> origin, IList<T> other, ref List<T> results)
        {
            List<T> intersectResult = ListPool<T>.Get();
            Intersect(origin, other, ref intersectResult);
            foreach (var item in intersectResult)
            {
                if (!origin.Contains(item))
                {
                    results.Add(item);
                }
            }
            ListPool<T>.Release(intersectResult);
        }


        ///// <summary>
        ///// 获取及添加组件
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="trans"></param>
        ///// <returns></returns>
        //public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        //{
        //    T t = go.GetComponent<T>();
        //    if (t == null)
        //    {
        //        t = go.AddComponent<T>();
        //    }
        //    return t;
        //}

        ///// <summary>
        ///// 获取及添加组件
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="trans"></param>
        ///// <returns></returns>
        //public static T GetOrAddComponent<T>(this Transform trans) where T : Component
        //{
        //    T t = trans.GetComponent<T>();
        //    if (t == null)
        //    {
        //        t = trans.gameObject.AddComponent<T>();
        //    }
        //    return t;
        //}
    }
}
