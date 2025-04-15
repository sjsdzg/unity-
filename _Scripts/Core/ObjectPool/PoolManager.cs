using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class PoolManager : DDOLSingleton<PoolManager>
    {
        private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool"></param>
        /// <param name="t"></param>
        public void Despawn<T>(IPool<T> pool, T t)
        {
            pool.Despawn(t);
        }

        /// <summary>
        /// 一帧结束后，回收对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool"></param>
        /// <param name="t"></param>
        public void DespawnEndOfFrame<T>(IPool<T> pool, T t)
        {
            StartCoroutine(DespawnEnumerator(pool, t));
        }

        public IEnumerator DespawnEnumerator<T>(IPool<T> pool, T t)
        {
            yield return endOfFrame;
            yield return endOfFrame;
            Despawn(pool, t);
        }
    }
}
