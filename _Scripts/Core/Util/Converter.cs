using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class Converter
    {
        /// <summary>
        /// 字节数组转化为结构体类型
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static T ToSturct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            if (bytes.Length < size)
            {
                return default(T);
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            T t = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return t;
        }

        /// <summary>
        /// 结构体转换成字节数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBytes(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] buffer = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            return buffer;
        }

        /// <summary>
        /// 将字符串转换成Vector3
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(string value)
        {
            Vector3 vector = Vector3.zero;
            string[] temps = value.Split(',');
            if (temps.Length == 3)
            {
                vector.x = Convert.ToSingle(temps[0]);
                vector.y = Convert.ToSingle(temps[1]);
                vector.z = Convert.ToSingle(temps[2]);
            }
            return vector;
        }

        /// <summary>
        /// 将字符串转换成Vector2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 ToVector2(string value)
        {
            Vector3 vector = Vector3.zero;
            string[] temps = value.Split(',');
            if (temps.Length == 2)
            {
                vector.x = Convert.ToSingle(temps[0]);
                vector.y = Convert.ToSingle(temps[1]);
            }
            return vector;
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp(DateTime dateTime)
        {
            DateTime dt1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime NewDateTime(long timestamp)
        {
            DateTime dt1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = dt1970.Ticks + timestamp * 10000;
            return new DateTime(t);
        }
    }
}
