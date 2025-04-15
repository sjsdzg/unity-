using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public class ObjectPool<T> : IPool<T> where T : new()
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private readonly Action<T> m_ActionOnSpawn;
        private readonly Action<T> m_ActionOnDespawn;

        /// <summary>
        /// 对象池内的数量
        /// </summary>
        public int Count { get { return m_Stack.Count; } }

        public ObjectPool(Action<T> actionOnSpawn, Action<T> actionOnDespawn)
        {
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
        public void Despawn(T element)
        {
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
                throw new ArgumentException("Internal error. Trying to destroy object that is already released to pool.");

            if (m_ActionOnDespawn != null)
                m_ActionOnDespawn(element);

            m_Stack.Push(element);
        }

        public void DespawnEndOfFrame(T element)
        {
            PoolManager.Instance.DespawnEndOfFrame(this, element);
        }

        /// <summary>
        /// 分配对象
        /// </summary>
        /// <returns></returns>
        protected virtual T Allocate()
        {
            return new T();
        }
    }
}
