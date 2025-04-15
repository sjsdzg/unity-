using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Common
{
    public class EnumUtil
    {
        /// <summary>
        /// 转成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(Enum value)
        {
            return value.ToString();
        }

        /// <summary>
        /// 从字符串解析
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, out TEnum result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at type \"{1}\"", value, typeof(TEnum)));
        }
    }
}
