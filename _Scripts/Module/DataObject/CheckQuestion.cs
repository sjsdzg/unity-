using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// 考核题目
    /// </summary>
    [XmlType("CheckQuestion")]
    public class CheckQuestion
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 题目类型
        /// </summary>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// 题目内容
        /// </summary>
        [XmlAttribute("content")]
        public string Content { get; set; }

        /// <summary>
        /// 选项
        /// </summary>
        [XmlAttribute("options")]
        public string Options { get; set; }

        /// <summary>
        /// 标准答案
        /// </summary>
        [XmlAttribute("key")]
        public string Key { get; set; }

        private int m_Value;
        /// <summary>
        /// 分值
        /// </summary>
        [XmlAttribute("value")]
        public int Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <summary>
        /// 得分
        /// </summary>
        [XmlIgnore]
        public int Score { get; set; }

        /// <summary>
        /// 标准答案
        /// </summary>
        [XmlIgnore]
        public string UserKey { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <returns></returns>
        public bool IsEquals()
        {
            return UserKey.Equals(Key);
        }
    }

    /// <summary>
    /// 考核题目集合
    /// </summary>
    [XmlType("CheckQuestionCollection")]
    public class CheckQuestionCollection
    {
        /// <summary>
        /// 引导列表
        /// </summary>
        [XmlArray("CheckQuestions")]
        [XmlArrayItem("CheckQuestion")]
        public List<CheckQuestion> CheckQuestions { get; set; }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public static CheckQuestionCollection Load(string path, Encoding encoding)
        {
            CheckQuestionCollection collection = null;
            try
            {
                collection = XMLHelper.DeserializeFromFile<CheckQuestionCollection>(path, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return collection;
        }
    }
}
