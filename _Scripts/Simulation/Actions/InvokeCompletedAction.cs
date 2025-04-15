using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Common;
using XFramework.Core;

namespace XFramework.Simulation
{
    /// <summary>
    /// 调用完成流程Action
    /// </summary>
    public class InvokeCompletedAction : ActionBase
    {
        /// <summary>
        /// seqId
        /// </summary>
        public int seqId;

        /// <summary>
        /// actId
        /// </summary>
        public int actId;

        public InvokeCompletedAction(int _seqId, int _actId)
        {
            seqId = _seqId;
            actId = _actId;
        }

        public override void Execute()
        {
            EventDispatcher.ExecuteEvent(Events.Procedure.Completed, seqId, actId);
            Completed();
        }
    }
}
