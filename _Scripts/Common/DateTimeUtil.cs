using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class DateTimeUtil
    {
        /// <summary>
        /// 零点（格林时间）
        /// </summary>
        public static DateTime Zero = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToEpochMilli(DateTime dateTime)
        {
            DateTime dt1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        /// <summary>
        /// 根据long（单位毫秒）计算日期
        /// </summary>
        /// <param name="epochMilli"></param>
        /// <returns></returns>
        public static DateTime OfEpochMilli(long epochMilli)
        {
            DateTime dt1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = dt1970.Ticks + epochMilli * 10000;
            return new DateTime(t);
        }

        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToString(DateTime dateTime)
        {
            string str = string.Empty;
            if (dateTime != Zero)
            {
                str = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return str;
        }
    }
}
