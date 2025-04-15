using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, x => x.Clear());

        public static List<T> Get()
        {
            return s_ListPool.Spawn();
        }

        public static void Release(List<T> toRelease)
        {
            s_ListPool.Despawn(toRelease);
        }

        public static void ReleaseEndOfFrame(List<T> toRelease)
        {

        }
    }
}
