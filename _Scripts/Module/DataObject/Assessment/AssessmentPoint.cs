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
    /// 考核点
    /// </summary>
    public class AssessmentPoint
    {
        private int id;
        /// <summary>
        /// 考核ID
        /// </summary>
        [XmlAttribute("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string region;
        /// <summary>
        /// 区间
        /// </summary>
        [XmlAttribute("region")]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string desc;
        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("desc")]
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

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

        private int score = 0;
        /// <summary>
        /// 得分
        /// </summary>
        [XmlIgnore]
        public int Score
        {
            get { return score; }
        }

        private int deduction;
        /// <summary>
        /// 扣分
        /// </summary>
        [XmlIgnore]
        public int Deduction
        {
            get { return deduction; }
        }

        /// <summary>
        /// 考核评级内容
        /// </summary>
        [XmlIgnore]
        public AssessmentGrade AssGrade { get; set; }

        private bool isFinished;
        /// <summary>
        /// 是否完成
        /// </summary>
        [XmlIgnore]
        public bool IsFinished
        {
            get { return isFinished; }
            set
            {
                isFinished = value;
                if (isFinished)
                {
                    if (deduction <= m_Value)
                        score = m_Value - deduction;
                    else
                        score = 0;
                }
                else
                {
                    score = 0;
                }
            }
        }

        private Rect m_Rect;
        /// <summary>
        /// 区间
        /// </summary>
        [XmlIgnore]
        public Rect Rect
        {
            get { return m_Rect; }
            set { m_Rect = value; }
        }


        /// <summary>
        /// 扣分方法
        /// </summary>
        /// <param name="_value"></param>
        public void Deduct(int _value)
        {
            deduction += _value;
        }

        /// <summary>
        /// 扣分方法
        /// </summary>
        public void Deduct()
        {
            deduction += AssGrade.Deduction;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("分值：" + Value);
            sb.Append("得分：" + Score);
            sb.Append("扣分:" + Deduction);
            sb.Append("是否完成：" + IsFinished);
            return sb.ToString();
        }
    }


    public class AssessmentPointCollection
    {
        [XmlArray("AssessmentPoints")]
        [XmlArrayItem("AssessmentPoint")]
        public List<AssessmentPoint> AssessmentPoints { get; set; }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public static AssessmentPointCollection Load(string path, Encoding encoding)
        {
            AssessmentPointCollection collection = null;
            try
            {
                collection = XMLHelper.DeserializeFromFile<AssessmentPointCollection>(path, encoding);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            return collection;
        }
    }
}
