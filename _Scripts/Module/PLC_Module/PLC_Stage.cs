using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// PLC阶段
    /// </summary>
    public class PLC_Stage
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
        /// 包含的流程
        /// </summary>
        public List<PLC_Flow> m_Flows = new List<PLC_Flow>();
    }
}
