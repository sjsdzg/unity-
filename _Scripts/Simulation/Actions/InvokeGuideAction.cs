using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Actions
{
    public class InvokeGuideAction : ActionBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        public InvokeGuideAction(string _name)
        {
            name = _name;
        }

        public override void Execute()
        {
            ProductionGuideManager.Instance.ShowGuide(name);
            Completed();
        }
    }
}
