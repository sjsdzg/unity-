using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Core
{
    public interface IDataObjectParser<T> where T : IDataObject<T>
    {
        /// <summary>
        /// 从Json字符串解析
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        T ParseJson(string json);

        /// <summary>
        /// 从XML字符串解析
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        T ParseXml(string xml);
    }
}
