using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Module
{
    [XmlType("PLCAttribute")]
    public class PLCAttribute
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
        /// 标签
        /// </summary>
        [XmlAttribute("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        [XmlAttribute("format")]
        public string Format { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// 是否实时更新
        /// </summary>
        [XmlAttribute("update")]
        public bool Update { get; set; }

        /// <summary>
        /// 轴的数量
        /// </summary>
        [XmlAttribute("axisNum")]
        public int AxisNum { get; set; }

        /// <summary>
        /// 轴的最小值
        /// </summary>
        [XmlAttribute("axisMinValue")]
        public float AxisMinValue { get; set; }

        /// <summary>
        /// 轴的最大值
        /// </summary>
        [XmlAttribute("axisMaxValue")]
        public float AxisMaxValue { get; set; }

        /// <summary>
        /// 轴的数量
        /// </summary>
        [XmlAttribute("xAxisNum")]
        public float XAxisNum { get; set; }

        /// <summary>
        /// 轴的最小值
        /// </summary>
        [XmlAttribute("xAxisMinValue")]
        public float XAxisMinValue { get; set; }

        /// <summary>
        /// 轴的最大值
        /// </summary>
        [XmlAttribute("xAxisMaxValue")]
        public float XAxisMaxValue { get; set; }

        /// <summary>
        /// PLC函数列表
        /// </summary>
        [XmlArray("Functions")]
        [XmlArrayItem("Function")]
        public List<PLCFunction> Functions { get; set; }

        public PLCFunction GetPLCFunction(float timer)
        {
            PLCFunction func = Functions.Find(x => { return timer >= x.MinTick && timer < x.MaxTick; });
            return func;
        }
    }

    /// <summary>
    /// PLC阶段集合
    /// </summary>
    [XmlType("PLCAttributeCollection")]
    public class PLCAttributeCollection : DataObject<PLCAttributeCollection>
    {
        /// <summary>
        /// 引导列表
        /// </summary>
        [XmlArray("Attributes")]
        [XmlArrayItem("Attribute")]
        public List<PLCAttribute> PLCAttributes { get; set; }
    }
}
