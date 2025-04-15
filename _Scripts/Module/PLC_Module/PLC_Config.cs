using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    /// <summary>
    /// PLC配置
    /// </summary>
    public class PLC_Config
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 包含PLC工段列表
        /// </summary>
        public List<PLC_Stage> m_Stages = new List<PLC_Stage>();
    }
}
