using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class PLC_Element
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// PLC元件类型
        /// </summary>
        public PLC_ElementType Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id:" + Id + "  ");
            sb.Append("Name:" + Name + "  ");
            sb.Append("Type:" + Type.ToString() + "  ");
            sb.Append("Value:" + Value + "  ");
            sb.Append("Desc:" + Desc + "  ");
            return sb.ToString();
        }
    }

    /// <summary>
    /// PLC元件类型
    /// </summary>
    public enum PLC_ElementType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 阀门
        /// </summary>
        Valve,
        /// <summary>
        /// 管道
        /// </summary>
        Pipe,
        /// <summary>
        /// 流体
        /// </summary>
        Fluid,
    }
}
