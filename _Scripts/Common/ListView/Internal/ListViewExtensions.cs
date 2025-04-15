using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public static class ListViewExtensions
    {
        /// <summary>
        /// Foreach with index.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="handler">Handler.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int i = 0;
            foreach (T item in enumerable)
            {
                handler(item, i);
                i++;
            }
        }

        /// <summary>
        /// Foreach.
        /// </summary>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="handler">Handler.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> handler)
        {
            foreach (T item in enumerable)
            {
                handler(item);
            }
        }
    }
}
