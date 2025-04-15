using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class UnityObjectPool<T> : IPool<T> where T : UnityEngine.Object
    {
        private Stack<T> m_Stack = new Stack<T>();
        protected Func<T> m_AllocFunc;
        protected Action<T> m_ActionOnSpawn;
        protected Action<T> m_ActionOnDespawn;

        /// <summary>
        /// 对象池内的数量
        /// </summary>
        public int Count { get { return m_Stack.Count; } }

        public UnityObjectPool()
        {

        }

        public UnityObjectPool(Func<T> allocFunc, Action<T> actionOnSpawn, Action<T> actionOnDespawn)
        {
            m_AllocFunc = allocFunc;
            m_ActionOnSpawn = actionOnSpawn;
            m_ActionOnDespawn = actionOnDespawn;
        }

        /// <summary>
        /// 生产对象
        /// </summary>
        /// <returns></returns>
        public T Spawn()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = Allocate();
            }
            else
            {
                element = m_Stack.Pop();
            }

            if (m_ActionOnSpawn != null)
                m_ActionOnSpawn(element);

            return element;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go"></param>
        public void Despawn(T element)
        {
            //if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            //    throw new ArgumentException("Internal error. Trying to destroy object that is already released to pool.");

            if (m_Stack.Count > 0 && m_Stack.Contains(element))
                return;

            if (m_ActionOnDespawn != null)
                m_ActionOnDespawn(element);

            m_Stack.Push(element);
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        protected T Allocate()
        {
            T t = null;
            if (m_AllocFunc != null)
            {
                t = m_AllocFunc.Invoke();
            }
            return t;
        }
    }
}
