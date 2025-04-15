using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace XFramework.Core
{
    public static class AsyncLoadOperationExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        public static AsyncLoadOperation OnStart(this AsyncLoadOperation operation, UnityAction<AsyncLoadOperation> startAction)
        {
            operation.OnStartEvent.AddListener(startAction);
            return operation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="updateAction"></param>
        /// <returns></returns>
        public static AsyncLoadOperation OnUpdate(this AsyncLoadOperation operation, UnityAction<AsyncLoadOperation> updateAction)
        {
            operation.OnUpdateEvent.AddListener(updateAction);
            return operation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="completedAction"></param>
        /// <returns></returns>
        public static AsyncLoadOperation OnCompleted(this AsyncLoadOperation operation, UnityAction<AsyncLoadOperation> completedAction)
        {
            operation.OnCompletedEvent.AddListener(completedAction);
            return operation;
        }
    }
}
