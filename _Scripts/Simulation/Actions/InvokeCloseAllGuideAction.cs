using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Actions
{
    /// <summary>
    /// 调用关闭所有引导Action
    /// </summary>
    public class InvokeCloseAllGuideAction : ActionBase
    {
        public InvokeCloseAllGuideAction()
        {
           
        }

        public override void Execute()
        {
            ProductionGuideManager.Instance.CloseAllGuide();
            Completed();
        }
    }
}
