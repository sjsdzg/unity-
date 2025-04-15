using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XFramework.Simulation;

namespace XFramework.Common
{
    /// <summary>
    /// ActionStateChangedEvent
    /// </summary>
    //public class ActionStateChangedEvent : UnityEvent<ActionState> { }

    public class OnErrorEvent : UnityEvent<Exception> { }

    public abstract class ActionBase : IAction
    {
        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 完成时触发
        /// </summary>
        public UnityEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        private OnErrorEvent m_OnError = new OnErrorEvent();
        /// <summary>
        /// 执行出现错误时，触发
        /// </summary>
        public OnErrorEvent OnError
        {
            get { return m_OnError; }
            set { m_OnError = value; }
        }

        /// <summary>
        /// 序列任务的位置
        /// </summary>
        public int position { get; set; }

        private Task m_Task;
        /// <summary>
        /// Executor
        /// </summary>
        public Task Task
        {
            get { return m_Task; }
            set { m_Task = value; }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="actionArgs">Action Args</param>
        public abstract void Execute();

        /// <summary>
        /// 完成
        /// </summary>
        public virtual void Completed()
        {
            OnCompleted.Invoke();
            Task.Next();
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="error"></param>
        public virtual void Error(Exception error)
        {
            Task.Error(error);
            OnError.Invoke(error);
        }

        //private ActionState m_ActionState;
        ///// <summary>
        ///// Action State
        ///// </summary>
        //public ActionState ActionState
        //{
        //    get { return m_ActionState; }
        //    set
        //    {
        //        m_ActionState = value;
        //        OnActionStateChanged.Invoke(m_ActionState);
        //    }
        //}

        //private ActionStateChangedEvent m_OnActionStateChanged = new ActionStateChangedEvent();
        ///// <summary>
        ///// ActionState改变时触发
        ///// </summary>
        //public ActionStateChangedEvent OnActionStateChanged
        //{
        //    get { return m_OnActionStateChanged; }
        //    set { m_OnActionStateChanged = value; }
        //}
    }
}
