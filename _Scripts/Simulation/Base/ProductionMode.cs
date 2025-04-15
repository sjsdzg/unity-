using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Simulation
{
    /// <summary>
    /// 生产模式
    /// </summary>
    public enum ProductionMode
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// 学习模式
        /// </summary>
        Study,
        /// <summary>
        /// 考核模式
        /// </summary>
        Examine,
        /// <summary>
        /// 闯关模式
        /// </summary>
        Banditos,
    }
}
