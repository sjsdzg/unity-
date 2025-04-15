using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace XFramework.Common
{
    public class XMLUtil
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
    }
}
