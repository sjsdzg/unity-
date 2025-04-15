using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using XFramework.Core;
using UnityEngine.EventSystems;

namespace XFramework.Common
{
    /// <summary>
    /// 交互组件
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        public class MisoperationEvent : UnityEvent<string> { }

        private MisoperationEvent m_OnMisoperation = new MisoperationEvent();
        /// <summary>
        /// 误操作，触发
        /// </summary>
        public MisoperationEvent OnMisoperation
        {
            get { return m_OnMisoperation; }
            set { m_OnMisoperation = value; }
        }

        /// <summary>
        /// 任务总数
        /// </summary>
        public int TaskCount { get { return m_TaskList.Count + m_InterimTasks.Count; } }

        /// <summary>
        /// 任务列表
        /// </summary>
        private List<Task> m_TaskList = new List<Task>();

        /// <summary>
        /// 可忽视的任务列表
        /// </summary>
        private List<Task> m_IgnoredTaskList = new List<Task>();

        /// <summary>
        /// 临时的任务列表
        /// </summary>
        private List<Task> m_InterimTasks = new List<Task>();

        /// <summary>
        /// isNext
        /// </summary>
        private bool isNext = false;

        /// <summary>
        /// nextCount
        /// </summary>
        private int frameCount;

        /// <summary>
        /// errorCount
        /// </summary>
        private int errorCount;

        public void AddTask(Task task, bool ignored = false)
        {
            if (ignored)
                m_IgnoredTaskList.Add(task);
            else
                m_TaskList.Add(task);

            task.OnStartEvent.AddListener(task_OnStartEvent);
            task.OnErrorEvent.AddListener(task_OnErrorEvent);
        }

        /// <summary>
        /// 添加一个任务及触发
        /// </summary>
        /// <param name="task"></param>
        /// <param name="eventID"></param>
        public void AddTask(Task task, EventTriggerType eventID = EventTriggerType.PointerClick, bool ignored = false)
        {
            AddTask(task, ignored);
            transform.TriggerAction(eventID, eventData =>
            {
                task.BaseEventData = eventData;
                task.Execute();
            });
        }

        /// <summary>
        /// 添加临时的任务，下一帧将被移除
        /// </summary>
        /// <param name="task"></param>
        public void AddInterimTask(Task task)
        {
            m_InterimTasks.Add(task);
            task.OnStartEvent.AddListener(task_OnStartEvent);
            task.OnErrorEvent.AddListener(task_OnErrorEvent);
        }

        public void RemoveTask(Task task)
        {
            m_TaskList.Remove(task);
        }

        public void RemoveAllTasks()
        {
            m_TaskList.Clear();
        }

        private void task_OnStartEvent()
        {
            isNext = true;
        }

        private void task_OnErrorEvent(Exception arg0)
        {
            errorCount++;
        }

        private void LateUpdate()
        {
            if (isNext)
            {
                frameCount++;
                if (frameCount >= 2)
                {
                    NextFrame();
                    isNext = false;
                    frameCount = 0;
                }
            }
        }

        /// <summary>
        /// 下一帧Action
        /// </summary>
        private void NextFrame()
        {
            Debug.Log("errorCount = : " + errorCount + "TaskCount = :" + TaskCount);
            if (errorCount == TaskCount && TaskCount > 0)
            {
                string msg = string.Format("Interactable[{0}]：出现误操作", gameObject.name);
                Debug.Log(msg);
                OnMisoperation.Invoke(msg);
                EventDispatcher.ExecuteEvent(Events.Interactable.Misoperation, msg);
            }
            errorCount = 0;
            m_InterimTasks.Clear();
        }
    }
}

