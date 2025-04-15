using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public interface IObjectPool
    {
        /// <summary>
        /// 生产对象
        /// </summary>
        /// <returns></returns>
        GameObject Spawn();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t"></param>
        void Despawn(GameObject go);
    }

    public interface IObjectPool<T>  where T : UnityEngine.Object
    {
        /// <summary>
        /// 生产对象
        /// </summary>
        /// <returns></returns>
        T Spawn();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t"></param>
        void Despawn(T t);
    }
}
