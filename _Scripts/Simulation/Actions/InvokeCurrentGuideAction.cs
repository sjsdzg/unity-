using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Actions
{
    /// <summary>
    /// 调用当前引导
    /// </summary>
    public class InvokeCurrentGuideAction : ActionBase
    {
        /// <summary>
        /// 序号 (-1代表不用小序号)
        /// </summary>
        public int number;

        public InvokeCurrentGuideAction(int _number = -1)
        {
            number = _number;
        }

        public override void Execute()
        {
            if (number == -1)
            {
                ProductionGuideManager.Instance.ShowCurrentGuide();
                Completed();
            }
            else
            {
                ProductionGuideManager.Instance.ShowCurrentGuide(number);
                Completed();
            }
        }
    }
}
