using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine;
using XFramework.Simulation;
using UnityEngine.EventSystems;

namespace XFramework.Common
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public class Task : ITask<ActionBase>
    {
        /// <summary>
        /// 完成数量
        /// </summary>
        public int ExecutePosition { get; private set; }

        private int lastPosition = -1;
        /// <summary>
        /// 最后加入任务的位置(针对序列任务)
        /// </summary>
        public int LastPosition
        {
            get { return lastPosition; }
            private set { lastPosition = value; }
        }

        /// <summary>
        /// 计时器
        /// </summary>
        private System.Diagnostics.Stopwatch sw;

        /// <summary>
        /// 如果通过触发，将会有事件数据
        /// </summary>
        public BaseEventData BaseEventData { get; set; }

        /// <summary>
        /// 任务队列
        /// </summary>
        protected Queue<ActionBase> m_ActionQueue = new Queue<ActionBase>();

        /// <summary>
        /// 缓存任务队列
        /// </summary>
        protected Queue<ActionBase> m_CacheQueue = new Queue<ActionBase>();

        private UnityEvent m_OnStart = new UnityEvent();
        /// <summary>
        /// 开始执行时触发
        /// </summary>
        public UnityEvent OnStartEvent
        {
            get { return m_OnStart; }
            set { m_OnStart = value; }
        }

        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 完成时触发
        /// </summary>
        public UnityEvent OnCompletedEvent
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        private OnErrorEvent m_OnError = new OnErrorEvent();
        /// <summary>
        /// 执行出现错误时，触发
        /// </summary>
        public OnErrorEvent OnErrorEvent
        {
            get { return m_OnError; }
            set { m_OnError = value; }
        }

        /// <summary>
        /// New Executor
        /// </summary>
        /// <returns></returns>
        public static Task NewTask()
        {
            return new Task();
        }

        public static Task NewTask(GameObject gameObject)
        {
            Interactable interactable = gameObject.GetOrAddComponent<Interactable>();
            Task task = new Task();
            interactable.AddInterimTask(task);
            return task;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        public void Execute()
        {
            ActionBase[] cacheList = new ActionBase[m_CacheQueue.Count];
            m_CacheQueue.CopyTo(cacheList, 0);
            foreach (var item in cacheList)
            {
                m_ActionQueue.Enqueue(item);
            }
            //Start
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            OnStartEvent.Invoke();
            //
            if (m_CacheQueue.Count == 0)
            {
                Completed();
                return;
            }
            //取出所有同一个位置的所有Action, 
            int count = m_ActionQueue.Count(x => x.position == ExecutePosition);
            while (count > 0)
            {
                ActionBase action = m_ActionQueue.Dequeue();
                try
                {
                    action.Execute();
                }
                catch (Exception ex)
                {
                    action.Error(ex);
                }
                count--;
            }
        }

        /// <summary>
        /// 添加一个ActionBase
        /// </summary>
        public Task Append(ActionBase action)
        {
            LastPosition++;
            action.Task = this;
            action.position = LastPosition;
            m_CacheQueue.Enqueue(action);
            return this;
        }

        /// <summary>
        /// 并行添加一个Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task Jion(ActionBase action)
        {
            LastPosition = LastPosition == -1 ? 0 : LastPosition;
            action.Task = this;
            action.position = LastPosition;
            m_CacheQueue.Enqueue(action);
            return this;
        }

        /// <summary>
        /// 添加一个回调
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        //public Task Append(Action callback)
        //{
        //    return this;
        //}

        /// <summary>
        /// 添加一个iterator [执行协程任务]
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        //public Task Append(IEnumerator iterator)
        //{
        //    return this;
        //}

        int tick = 0;
        /// <summary>
        /// 执行下一个
        /// </summary>
        public void Next()
        {
            if (m_CacheQueue.Count == 0)
            {
                Completed();
                return;
            }

            tick++;
            //判断相应位置的所有任务是否都完成，如果不是则直接返回
            int stat = m_CacheQueue.Count(x => x.position == ExecutePosition);
            if (tick < stat)
                return;

            if (tick == stat)
            {
                ExecutePosition++;
                tick = 0;

                if (ExecutePosition > LastPosition)
                {
                    Completed();
                    return;
                }
            }

            //取出所有同一个位置的所有Action, 
            int count = m_ActionQueue.Count(x => x.position == ExecutePosition);
            while (count > 0)
            {
                ActionBase action = m_ActionQueue.Dequeue();
                try
                {
                    action.Execute();
                }
                catch (Exception ex)
                {
                    action.Error(ex);
                }
                count--;
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Completed()
        {
            try
            {
                OnCompletedEvent.Invoke();
                ExecutePosition = 0;
                sw.Stop();
                Debug.LogFormat("[Task] Total execute time : {0} milliseconds.", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Error(ex);
            };
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="error"></param>
        public void Error(Exception error)
        {
            OnErrorEvent.Invoke(error);
            //Debug.LogError(error);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            m_CacheQueue.Clear();
            m_ActionQueue.Clear();
            LastPosition = -1;
            ExecutePosition = 0;
        }
    }
}
