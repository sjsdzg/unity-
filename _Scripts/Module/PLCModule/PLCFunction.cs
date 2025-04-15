using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XFramework.Module
{
    /// <summary>
    /// PLC函数
    /// </summary>
    [XmlType("PLCFunction")]
    public class PLCFunction
    {
        /// <summary>
        /// 区间
        /// </summary>
        [XmlAttribute("region")]
        public string Region { get; set; }

        /// <summary>
        /// 函数表达式
        /// </summary>
        [XmlAttribute("func")]
        public string Function { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        [XmlIgnore]
        public float MinTick { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [XmlIgnore]
        public float MaxTick { get; set; }

        public void Internal()
        {
            string str = Region.Substring(1, Region.Length - 2);
            string[] strs = str.Split(',');
            if (strs.Length == 2)
            {
                MinTick = Convert.ToInt32(strs[0]);
                MaxTick = Convert.ToInt32(strs[1]);
            }
        }
    }
}
