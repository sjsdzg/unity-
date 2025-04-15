using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{ 
    public interface IPool<T>
    {
        /// <summary>
        /// 生产对象
        /// </summary>
        /// <returns></returns>
        T Spawn();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="element"></param>
        void Despawn(T element);
    }
}
