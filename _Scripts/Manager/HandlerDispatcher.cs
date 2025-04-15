using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework
{
    public class HandlerDispatcher : DDOLSingleton<HandlerDispatcher>
    {
        /// <summary>
        /// 方法队列
        /// </summary>
        private Queue<Action> actions = new Queue<Action>();

        /// <summary>
        /// 
        /// </summary>
        private Queue<IEnumerator> iterators = new Queue<IEnumerator>();

        /// <summary>
        /// 网络是否活动
        /// </summary>
        public bool Active { get; set; }

        void Update()
        {
            if (Active)
            {
                NetworkManager.Instance.DispatchPackage();
                //分发行为
                DispatchAction();
                DispatchIterator();
            }
        }

        /// <summary>
        /// 增加方法
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(Action action)
        {
            lock (actions)
            {
                actions.Enqueue(action);
            }
        }

        public void AddIterator(IEnumerator iterator)
        {
            lock (iterators)
            {
                iterators.Enqueue(iterator);
            }
        }

        /// <summary>
        /// 分发
        /// </summary>
        public void DispatchAction()
        {
            if (actions.Count == 0)
                return;

            lock (actions)
            {
                while (actions.Count > 0)
                {
                    var action = actions.Dequeue();
                    if (action != null)
                    {
                        action.Invoke();
                    }
                }
            }
        }


        /// <summary>
        /// 分发
        /// </summary>
        public void DispatchIterator()
        {
            if (iterators.Count == 0)
                return;

            lock (iterators)
            {
                while (iterators.Count > 0)
                {
                    var iterator = iterators.Dequeue();
                    if (iterator != null)
                    {
                        StartCoroutine(iterator);
                    }
                }
            }
        }
    }
}
