using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NPOI.SS.UserModel;
using UnityEngine;

namespace XFramework.Common
{
    public class XMLHelper
    {
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点XmlNode.
        /// </summary>
        /// <param name="xmlFile">Xml文件路径</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNode</returns>
        public static XmlNode GetXmlNodeByXpath(string xmlFile,string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //忽略注释
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(xmlFile, settings);
                //加载XML文档
                xmlDoc.Load(reader);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        /// <summary>
        /// xml文件中是否存在 匹配XPath表达式的第一个节点XmlNode
        /// </summary>
        /// <param name="xmlFile">Xml文件路径</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回true为存在此节点，否则失败</returns>
        public static bool IsExistsNode(string xmlFile, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFile);//加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);

                if (xmlNode != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="xmlFile">xml文件路径</param>
        /// <param name="xpath">匹配的XPath表达式</param>
        /// <param name="htAtt">需要更新的属性表，Key代表需要更新的属性，Value代表要更新的值</param>
        /// <returns>返回true为更新成功，否则失败</returns>
        public static bool UpdateNode(string xmlFile,string xpath,Hashtable htAtt)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                XmlElement _element = xmlNode as XmlElement;
                foreach (DictionaryEntry  item in htAtt)
                {
                    if (_element.HasAttribute(item.Key.ToString()))
                    {
                        _element.SetAttribute(item.Key.ToString(), item.Value.ToString());
                    }
                }
                xmlDoc.Save(xmlFile);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建子节点
        /// </summary>
        /// <param name="xmlFile">XML文档完全文件名（包含物理路径）</param>
        /// <param name="xpath">要匹配的XPath表达式</param>
        /// <param name="xmlNodeName">节点名称</param>
        /// <param name="htAtt">需要更新的属性表，Key代表需要更新的属性，Value代表要更新的值</param>
        /// <returns>返回true为更新成功，否则失败</returns>
        public static bool CreateNode(string xmlFile,string xpath,string xmlNodeName,Hashtable htAtt)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);

                XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                foreach (DictionaryEntry item in htAtt)
                {
                    subElement.SetAttribute(item.Key.ToString(), item.Value.ToString());
                }
                xmlNode.AppendChild(subElement);

                xmlDoc.Save(xmlFile);
                return true;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private static void SerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        public static string Serialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SerializeInternal(stream, o, encoding);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static void SerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                SerializeInternal(file, o, encoding);
            }
        }

        public static T Deserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }
        }

        public static T DeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return Deserialize<T>(xml, encoding);
        }
    }
}
