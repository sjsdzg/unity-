using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Simulation
{
    /// <summary>
    /// 影响模式
    /// </summary>
    public enum EffectModel
    {
        None,
        /// <summary>
        /// 反应温度影响模式
        /// </summary>
        TemperatureEffectModel,
        /// <summary>
        /// 溶剂用量影响模式
        /// </summary>
        SolventCapacityEffectModel,
        /// <summary>
        /// 反应时间影响模式
        /// </summary>
        ReactionTimeEffectModel,
        /// <summary>
        /// 反应压力影响模式
        /// </summary>
        PressureEffectModel,
    }
}
