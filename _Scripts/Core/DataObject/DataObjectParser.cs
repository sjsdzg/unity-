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
    public class DataObjectParser<T> : IDataObjectParser<T> where T : IDataObject<T>
    {
        /// <summary>
        /// 从Json文件解析
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public T ParseJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json");

            return JsonConvert.DeserializeObject<T>(json);
        }

        public T ParseJsonFromResources(string path)
        {
            path = path.Substring(0, path.LastIndexOf("."));
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
                throw new NullReferenceException("TextAsset is null");

            return ParseJson(asset.text);
        }

        /// <summary>
        /// 从Json文件解析
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<T> ParseJsonToList(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json");

            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        /// <summary>
        /// 从XML文件解析
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public T ParseXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                using (XmlReader xr = XmlReader.Create(ms, settings))
                {
                    return (T)serializer.Deserialize(xr);
                }
            }
        }

        public T ParseXmlFromResources(string path)
        {
            path = path.Substring(0, path.LastIndexOf("."));
            //Debug.Log(path);
            TextAsset asset = Resources.Load<TextAsset>(path);
            if (asset == null)
                throw new NullReferenceException("TextAsset is null");

            return ParseXml(asset.text);
        }
    }
}
