using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace XFramework.Core
{
    [Serializable]
    public class DataObject<T> : IDataObject<T> where T : DataObject<T>
    {
        private static DataObjectParser<T> parser = new DataObjectParser<T>();
        /// <summary>
        /// 转换器
        /// </summary>
        public static DataObjectParser<T> Parser { get { return parser; } }
    }
}
