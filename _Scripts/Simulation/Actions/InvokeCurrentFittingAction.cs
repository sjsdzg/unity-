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
    public class InvokeCurrentFittingAction : ActionBase
    {
        /// <summary>
        /// 序号 (-1代表不用小序号)
        /// </summary>
        public int number = -1;

        public InvokeCurrentFittingAction(int _number = -1)
        {
            number = _number;
        }

        public override void Execute()
        {
            if (number == -1)
            {
                PipeFittingManager.Instance.InvokeCurrentFitting();
                Completed();
            }
            else
            {
                PipeFittingManager.Instance.InvokeCurrentFitting(number);
                Completed();
            }
        }
    }
}
