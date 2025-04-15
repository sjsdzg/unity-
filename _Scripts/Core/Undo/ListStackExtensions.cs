using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public static class ListStackExtensions
    {
        public static void Push(this List<IUndoRecord> list, IUndoRecord item)
        {
            list.Insert(0, item);
        }

        public static IUndoRecord Pop(this List<IUndoRecord> list)
        {
            IUndoRecord item = list[0];
            list.RemoveAt(0);
            return item;
        }
    }
}
