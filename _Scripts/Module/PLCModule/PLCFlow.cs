using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace XFramework.Module
{
/// <summary>
/// PLC流程
/// </summary>
    [XmlType("PLCFlow")]
    public class PLCFlow
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("id")]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 区间
        /// </summary>
        [XmlAttribute("region")]
        public string Region { get; set; }

        /// <summary>
        /// 倍率
        /// </summary>
        [XmlAttribute("ratio")]
        public int Ratio { get; set; }
        /// <summary>
        /// 开始状态信息
        /// </summary>
        [XmlAttribute("startInfo")]
        public string StartInfo { get; set; }
        /// <summary>
        /// 运行中状态信息
        /// </summary>
        [XmlAttribute("runingInfo")]
        public string RuningInfo { get; set; }
        /// <summary>
        /// 结束状态信息
        /// </summary>
        [XmlAttribute("endInfo")]
        public string EndInfo { get; set; }

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
        
        /// <summary>
        /// 运行状态
        /// </summary>
        [XmlIgnore]
        public RunningStatus Status { get; set; }

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

    /// <summary>
    /// 运行状态
    /// </summary>
    public enum RunningStatus
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 开始
        /// </summary>
        Start,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 结束
        /// </summary>
        End,
    }
}
