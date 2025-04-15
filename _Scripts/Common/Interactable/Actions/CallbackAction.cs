using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public class CallbackAction : ActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        private Action action;

        public CallbackAction(Action _action)
        {
            action = _action;
        }

        public override void Execute()
        {
            if (action != null)
                action();

            Completed();
        }
    }
}
