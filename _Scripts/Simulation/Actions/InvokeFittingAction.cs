using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Actions
{
    /// <summary>
    /// 调用当前管件Action
    /// </summary>
    public class InvokeFittingAction : ActionBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        public InvokeFittingAction(string _name = "")
        {
            name = _name;
        }

        public override void Execute()
        {
            PipeFittingManager.Instance.InvokeFitting(name);
            Completed();
        }
    }
}
