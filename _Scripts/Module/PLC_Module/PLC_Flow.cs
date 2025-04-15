using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// PLC流程
    /// </summary>
    public class PLC_Flow
    {
        /// <summary>
        /// 序列id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 包含的PLC内容
        /// </summary>
        public List<PLC_Element> m_Elements = new List<PLC_Element>();
    }
}
