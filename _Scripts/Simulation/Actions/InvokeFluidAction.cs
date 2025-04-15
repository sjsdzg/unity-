using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Component;
using XFramework.Simulation;
using UnityEngine;
namespace XFramework.Actions
{
    /// <summary>
    /// 调用当前管件Action
    /// </summary>
    public class InvokeFluidAction : ActionBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public FluidComponent fluid;
        public int m_Velocity;

        public InvokeFluidAction(FluidComponent _fluid,int _m_Velocity)
        {
            fluid = _fluid;
            m_Velocity = _m_Velocity;
        }

        public override void Execute()
        {
            fluid.UV = new Vector2(0, 1);
            fluid.Velocity = m_Velocity;
           
            Completed();
        }
    }
}
